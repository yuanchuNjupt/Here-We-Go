using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clipper2Lib;
using System.Net.NetworkInformation;
using System.Net;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;

public class ShapeBaseController : MonoBehaviour
{
    [Tooltip("血量")]
    public double hp;
    [Tooltip("攻击")]
    public double atk;
    [Tooltip("血量速率增加系数")]
    public float hpIncFactor;
    [Tooltip("攻击速率增加系数")]
    public float atkIncFactor;

    [Tooltip("血量回复速率（/s）")]
    public double hpRate;
    [Tooltip("攻击增长速率（/s）")]
    public double atkRate;

    //游戏背景
    public GameObject backGround;

    protected double MaxHp;
    protected double time = 0;

    //当前帧
    private Vector2 currentPos;
    private Quaternion currentAngle;
    //上一帧
    protected Quaternion previousAngle;
    protected Vector2 previousPos;

    //记录重叠的图形
    protected HashSet<GameObject> overShapes = new HashSet<GameObject>();
    protected SpriteRenderer sr;
    protected Color color;
    //初始透明度
    protected float a;

    protected void Start()
    {
        MaxHp = hp;
        previousAngle = transform.rotation;
        previousPos = transform.position;
        overShapes.Add(this.gameObject);
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        a = color.a;
    }

    protected void FixedUpdate()
    {
        GameDataMgr.instance.isBlock = false;
        previousPos = currentPos;
        previousAngle = currentAngle;
        currentPos = transform.position;
        currentAngle = transform.rotation;
    }

    protected virtual void Update()
    {
        //透明度随血量减少而减少
        color.a = (float)(hp / MaxHp) * a;
        sr.color = color;
        if (hp <= 0)
        {
            Destroy(this.gameObject);
            //播放屏闪动画
            backGround.GetComponent<Animator>().SetTrigger("shapeDead");
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        GameObject newShape = collision.gameObject;
        int overShapeCount = 0;
        foreach (var shape in overShapes)
        {
            if (IsOverLapping(newShape, shape))
            {
                overShapeCount++;
            }
        }

        if (overShapeCount > 2)
        {
            print("该区域已有3个图形重叠，阻止新图形进入！");
            GameDataMgr.instance.isBlock = true;
            GameDataMgr.instance.isDragged = false;
            newShape.transform.position = newShape.GetComponent<ShapeBaseController>().previousPos;
            newShape.transform.rotation = newShape.GetComponent<ShapeBaseController>().previousAngle;
        }
        else
        {
            overShapes.Add(newShape);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        overShapes.Remove(collision.gameObject);
    }

    #region 计算重叠面积
    /// <summary>
    /// 计算两个 PolygonCollider2D 的重叠面积
    /// </summary>
    /// <param name="polyA">第一个多边形</param>
    /// <param name="polyB">第二个多边形</param>
    /// <returns>重叠的面积</returns>
    private float CalculateOverlapArea(PolygonCollider2D polyA, PolygonCollider2D polyB)
    {
        // 1. 将 Unity 的 PolygonCollider2D 顶点转换为 Clipper 库可用的格式（IntPoint）
        Path64 pathA = new Path64(ConvertToClipperPath(polyA));
        Path64 pathB = new Path64(ConvertToClipperPath(polyB));

        Paths64 pathsA = new Paths64 { pathA };
        Paths64 pathsB = new Paths64 { pathB };

        // 2. 创建 Clipper 实例，并添加两个多边形路径
        Paths64 solution = new Paths64();
        Clipper64 clipper = new Clipper64();
        clipper.AddSubject(pathA);// 设定第一个多边形为主体
        clipper.AddClip(pathB);    // 设定第二个多边形为裁剪对象

        // 3. 计算两个多边形的交集（Intersection）
        clipper.Execute(Clipper2Lib.ClipType.Intersection, FillRule.NonZero, solution);

        // 4. 计算交集区域的面积
        float totalArea = 0;
        foreach (var polygon in solution)
        {
            totalArea += ComputePolygonArea(polygon);
        }

        return totalArea; // 返回最终的重叠面积
    }

    /// <summary>
    /// 将 PolygonCollider2D 的顶点转换为 Clipper 库支持的 IntPoint 格式
    /// </summary>
    /// <param name="poly">PolygonCollider2D 对象</param>
    /// <returns>ClipperLib 可用的多边形路径</returns>
    private List<Point64> ConvertToClipperPath(PolygonCollider2D poly)
    {
        List<Point64> path = new List<Point64>();

        // 遍历多边形的所有顶点
        foreach (Vector2 point in poly.points)
        {
            // 将本地坐标转换为世界坐标
            Vector2 worldPoint = poly.transform.TransformPoint(point);

            // 由于 Clipper 使用整数计算，我们将坐标放大 1000 倍（防止浮点数精度丢失）
            path.Add(new Point64(worldPoint.x * 1000, worldPoint.y * 1000));
        }
        return path;
    }

    /// <summary>
    /// 计算一个多边形的面积（适用于 ClipperLib 处理的 IntPoint 多边形）
    /// </summary>
    /// <param name="polygon">Clipper 库格式的多边形</param>
    /// <returns>该多边形的面积</returns>
    private float ComputePolygonArea(List<Point64> polygon)
    {
        int n = polygon.Count;
        if (n < 3) return 0; // 如果多边形的顶点少于 3 个，面积为 0

        float area = 0;

        // 使用 **Shoelace 公式** 计算多边形面积
        for (int i = 0; i < n; i++)
        {
            Point64 p1 = polygon[i];              // 当前顶点
            Point64 p2 = polygon[(i + 1) % n];    // 下一个顶点（环绕取余）

            // 计算叉积并累加
            area += (p1.X * p2.Y - p2.X * p1.Y);
        }

        // 取绝对值，并将单位还原（之前放大了 1000 倍，所以现在需要除以 1000²）
        return Mathf.Abs(area) / (2 * 1000 * 1000);
    }
    #endregion

    /// <summary>
    /// 图形重叠血量增益计算
    /// </summary>
    /// <param name="collision">重叠（碰撞）的图形</param>
    /// <param name="hpIncFactor">血量增加倍数</param>
    /// <param name="origialHp_">重叠（碰撞）的图形的初始血量值</param>
    protected float CalculateAddHp(GameObject collision, float hpIncFactor)
    {
        return CalculateOverlapArea(PolygonCollider2DMgr.Instance.LoadPolygonCollider2D(gameObject), PolygonCollider2DMgr.Instance.LoadPolygonCollider2D(collision)) * hpIncFactor;
    }

    /// <summary>
    /// 图形重叠攻击增益计算
    /// </summary>
    /// <param name="collision">重叠（碰撞）的图形</param>
    /// <param name="atkIncFactor">攻击增加倍数</param>
    /// <param name="origialHp_">重叠（碰撞）的图形的初始攻击值</param>

    protected float CalculateAddAtk(GameObject collision, float atkIncFactor)
    {
        return CalculateOverlapArea(PolygonCollider2DMgr.Instance.LoadPolygonCollider2D(gameObject), PolygonCollider2DMgr.Instance.LoadPolygonCollider2D(collision)) * atkIncFactor;
    }

    /// <summary>
    /// 检测两个图形是否重叠
    /// </summary>
    private bool IsOverLapping(GameObject a, GameObject b)
    {
        if (a == b) return false;
        var colA = a.GetComponent<Collider2D>();
        var colB = b.GetComponent<Collider2D>();

        var distance = Physics2D.Distance(colA, colB);

        // 如果它们发生实际重叠，或者接近但距离很小，也可以认为是“覆盖”
        return distance.isOverlapped || distance.distance < 0.01f;
    }
}
