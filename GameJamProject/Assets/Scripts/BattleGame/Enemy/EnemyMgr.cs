using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyMgr : MonoBehaviour
{
    [Tooltip("内环（长方形怪的停止位置）距与外环（所有怪生成的位置）的半径差")]
    public float padding = 0.5f;

    [Tooltip("三角形怪生成间隔时间")]
    public float enemy1Time = 1;
    [Tooltip("圆形怪生成间隔时间")]
    public float enemy2Time = 2;
    [Tooltip("长方形怪生成间隔时间")]
    public float enemy3Time = 10;

    [Tooltip("三角形怪一次生成数量")]
    public int enemy1Num;
    [Tooltip("圆形怪一次生成数量")]
    public int enemy2Num;
    [Tooltip("长方形形怪一次生成数量")]
    public int enemy3Num;

    private GameObject enemy1;
    private GameObject enemy2;
    private GameObject enemy3;
    //屏幕的底和高
    private float screenTop;
    private float screenBottom;
    //屏幕的高度（即外圆的直径）
    private float screenHeght;
    //生成敌人的方向
    private int side;

    void Start()
    {
        Camera cam = Camera.main;
        screenTop = cam.ViewportToWorldPoint(new Vector3(0, 1, -cam.transform.position.z)).y;
        screenBottom = cam.ViewportToWorldPoint(new Vector3(0, 0, -cam.transform.position.z)).y;
        screenHeght = screenTop - screenBottom;
        //加载敌人预设体
        enemy1 = Resources.Load<GameObject>("Prefabs/Enemy/Enemy1");
        enemy2 = Resources.Load<GameObject>("Prefabs/Enemy/Enemy2");
        enemy3 = Resources.Load<GameObject>("Prefabs/Enemy/Enemy3");
        StartCoroutine(CreateEnemy1());
        StartCoroutine(CreateEnemy2());
        StartCoroutine(CreateEnemy3());
    }

    #region 生成敌人的协程
    IEnumerator CreateEnemy1()
    {
        yield return new WaitForSeconds(enemy1Time);
        while (true)
        {
            for(int i = 0; i < enemy1Num; i++)
            {
                CreateObjOnCicle(enemy1);
            }
            yield return new WaitForSeconds(enemy1Time);
        }
    }

    IEnumerator CreateEnemy2()
    {
        yield return new WaitForSeconds(enemy2Time);
        while (true)
        {
            for (int i = 0; i < enemy2Num; i++)
            {
                CreateObjOnCicle(enemy2);
            }
            yield return new WaitForSeconds(enemy2Time);
        }
    }

    IEnumerator CreateEnemy3()
    {
        yield return new WaitForSeconds(enemy3Time);
        while (true)
        {
                        for (int i = 0; i < enemy3Num; i++)
            {
                CreateObjOnCicle(enemy3);
            }
            yield return new WaitForSeconds(enemy3Time);
        }
    }
    #endregion

    /// <summary>
    /// 在圆形边缘产生敌人
    /// </summary>
    private void CreateObjOnCicle(GameObject enemy)
    {
        int angle = Random.Range(0, 360);
        //角度转弧度
        float rad = angle * Mathf.Deg2Rad;
        //计算圆周上的位置
        float x = Mathf.Cos(rad) * screenHeght/2;
        float y =  Mathf.Sin(rad) * screenHeght/2;
        Vector3 pos = new Vector3(x, y, 0);

        //计算敌人朝向原点的方向
        Vector3 dir = (Vector3.zero - pos).normalized;
        float ang = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, ang-90);

        Instantiate(enemy, pos, rotation);
    }
}
