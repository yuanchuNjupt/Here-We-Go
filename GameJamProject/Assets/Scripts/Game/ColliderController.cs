using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [Tooltip("作为解谜的图形")]
    public GameObject[] targetShapes;
    [Tooltip("渐变消失的时间")]
    public float fadeTime = 8;
    [Tooltip("解谜图形延迟消失")]
    public float lateTime = 0.5f;
    [Tooltip("解谜图形的父类对象")]
    public GameObject[] fatherObj;
    [Tooltip("主图形中心")]
    public Vector2 centerPos = new Vector2(0,0);
    [Tooltip("移动速度")]
    public float speed = 1;

    //记录每个解谜图形的碰撞器是否全部触发
    private Dictionary<GameObject, Dictionary<Collider2D, bool>> shapeStates = new Dictionary<GameObject, Dictionary<Collider2D, bool>>();
    private Animator animator;

    void Start()
    {
        Collider2D[] colliders;
        List<Collider2D> list;
        //单一解谜图形上的碰撞器状态
        Dictionary<Collider2D, bool> triggerStates;

        //将每个图形的碰撞器分别存储
        foreach (GameObject target in targetShapes)
        {
            colliders = target.GetComponentsInChildren<Collider2D>();
            list = colliders.Where(c => c).ToList();
            triggerStates = list.ToDictionary(collider => collider, isbool => false);
            shapeStates[target] = triggerStates;
        }

        animator = GetComponent<Animator>();
        //更新主图形坐标
        GameDataMgr.instance.centerPos = centerPos;
    }

    void Update()
    {
        //鼠标抬起时检测
        if (Input.GetMouseButtonUp(0))
        {
            //检查所有图形的所有碰撞器是否触发
            if (shapeStates.Values.All(shape => shape.Values.All(state => state)))
            {
                StartCoroutine(MainIE());
                animator.enabled = true;
                Camera.main.GetComponent<Animator>().enabled = true;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //遍历所有对象上的碰撞器触发状态
        foreach (var target in shapeStates)
        {
            Dictionary<Collider2D, bool> dic = target.Value;
            if (dic.ContainsKey(collider))
            {
                dic[collider] = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        //遍历所有对象上的碰撞器触发状态
        foreach (var target in shapeStates)
        {
            Dictionary<Collider2D, bool> dic = target.Value;
            if (dic.ContainsKey(collider))
            {
                dic[collider] = false;
            }
        }
    }

    /// <summary>
    /// 所有解谜图形朝一个点移动
    /// </summary>
    private IEnumerator ShapeMoveToCenter(Vector3 targetPos)
    {
        //设置判断对象是否到达目标位置的容差
        float tolerance = 0.01f;
        bool allReach = false;
        while (!allReach)
        {
            allReach = true;
            foreach (GameObject obj in fatherObj)
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPos, Time.deltaTime * speed);
                if (Vector3.Distance(obj.transform.position, targetPos) > tolerance)
                {
                    allReach = false;
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// 使所有解谜图形渐变消失
    /// </summary>
    private IEnumerator FadeOutShapes()
    {
        yield return new WaitForSeconds(lateTime);

        float nowTime = 0;
        Dictionary<GameObject, SpriteRenderer> renderers = new Dictionary<GameObject, SpriteRenderer>();
        foreach (GameObject target in targetShapes)
        {
            SpriteRenderer sr = SpriteRendererMgr.Instance.LoadSpriteRenderer(target);
            renderers[target] = sr;
        }

        while (nowTime < fadeTime)
        {
            nowTime += Time.deltaTime;
            foreach (GameObject obj in renderers.Keys)
            {
                Color c = renderers[obj].color;
                float originalAlpha = c.a;
                renderers[obj].color = new Color(c.r, c.g, c.b, Mathf.Lerp(originalAlpha, 0, nowTime / fadeTime));
            }
            yield return null;
        }
        foreach (GameObject obj in targetShapes)
        {
            obj.gameObject.SetActive(false);
        }
    }

    //主协程
    private IEnumerator MainIE()
    {
        //先移动
        print("移动");
        yield return StartCoroutine(ShapeMoveToCenter(centerPos));
        //再渐变
        print("渐变");
        yield return StartCoroutine(FadeOutShapes());
    }
}
