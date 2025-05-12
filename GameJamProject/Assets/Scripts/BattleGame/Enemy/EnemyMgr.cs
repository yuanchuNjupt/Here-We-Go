using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

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

    [Tooltip("第一波三角形怪一次生成数量")]
    public int firstEnemy1Num = 6;
    [Tooltip("第二波三角形怪一次生成数量")]
    public int secondEnemy1Num = 3;
    [Tooltip("第三波三角形怪一次生成数量")]
    public int thirdEnemy1Num = 5;
    [Tooltip("第二波圆形怪一次生成数量")]
    public int secondEnemy2Num = 2;
    [Tooltip("第三波圆形怪一次生成数量")]
    public int thirdEnemy2Num = 3;
    [Tooltip("长方形怪一次生成数量")]
    public int enemy3Num = 3;

    [Tooltip("第一波敌人截至时间")]
    public int firstTime = 15;
    [Tooltip("第二波敌人截至时间")]
    public int secondTime = 25;
    [Tooltip("第三波敌人截至时间")]
    public int thirdTime = 40;

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
    //累计游戏时间
    private float time = 0;
    //波次
    private int phase = 0;

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
        StartCoroutine(CreateEnemy3());
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (phase == 0 && time < firstTime)
        {
            //第一波
            print("第一波");
            StartCoroutine(CreateEnemy1(firstEnemy1Num));
            phase = 1;
        }
        else if (phase == 1 && time >= firstTime && time < secondTime)
        {
            //第二波
            print("第er波");
            StopCoroutine(CreateEnemy1(firstEnemy1Num));
            StartCoroutine(CreateEnemy1(secondEnemy1Num));
            StartCoroutine(CreateEnemy2(secondEnemy2Num));
            phase = 2;
        }
        else if (phase == 2 && time >= secondTime && time < thirdTime)
        {
            //第三波
            print("第san波");
            StopCoroutine(CreateEnemy1(secondEnemy1Num));
            StopCoroutine(CreateEnemy2(secondEnemy2Num));
            StartCoroutine(CreateEnemy1(thirdEnemy1Num));
            StartCoroutine(CreateEnemy2(thirdEnemy2Num));
            phase = 3;
        }
        else if (phase == 3)
        {
            //结束所有进程
            StopCoroutine(CreateEnemy1(thirdEnemy1Num));
            StopCoroutine(CreateEnemy2(thirdEnemy2Num));
            StopCoroutine(CreateEnemy3());
            //通关
            print("通关");
        }
    }
    #region 生成敌人的协程
    IEnumerator CreateEnemy1(int enemy1Num)
    {
        yield return new WaitForSeconds(enemy1Time);
        while (true)
        {
            for (int i = 0; i < enemy1Num; i++)
            {
                CreateObjOnCicle(enemy1, true);
            }
            yield return new WaitForSeconds(enemy1Time);
        }
    }

    IEnumerator CreateEnemy2(int enemy2Num)
    {
        yield return new WaitForSeconds(enemy2Time);
        while (true)
        {
            for (int i = 0; i < enemy2Num; i++)
            {
                CreateObjOnCicle(enemy2, false);
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
                float angle = RandomSixSide();
                for (int j = 0; j < 3; j++)
                {
                    //角度转弧度
                    float rad = angle * Mathf.Deg2Rad;
                    angle += 2;
                    //计算圆周上的位置
                    float x = Mathf.Cos(rad) * screenHeght / 2;
                    float y = Mathf.Sin(rad) * screenHeght / 2;
                    Vector3 pos = new Vector3(x, y, 0);

                    //计算敌人朝向原点的方向
                    Vector3 dir = (Vector3.zero - pos).normalized;
                    float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.Euler(0, 0, ang - 90);

                    Instantiate(enemy3, pos, rotation);
                }
            }
            yield return new WaitForSeconds(enemy3Time);
        }
    }
    #endregion

    /// <summary>
    /// 在圆形边缘产生单个敌人
    /// </summary>
    /// <param name="isRandom">是否随机生成敌人</param>
    private void CreateObjOnCicle(GameObject enemy, bool isRandom)
    {
        float angle;
        if (!isRandom)
        {
            angle = RandomSixSide();
        }
        else
        {
            angle = Random.Range(0, 360);
        }
        //角度转弧度
        float rad = angle * Mathf.Deg2Rad;
        //计算圆周上的位置
        float x = Mathf.Cos(rad) * screenHeght / 2;
        float y = Mathf.Sin(rad) * screenHeght / 2;
        Vector3 pos = new Vector3(x, y, 0);

        //计算敌人朝向原点的方向
        Vector3 dir = (Vector3.zero - pos).normalized;
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, ang - 90);

        Instantiate(enemy, pos, rotation);
    }

    private float RandomSixSide()
    {
        float angle;
        //在六个角度范围发射
        int index = Random.Range(0, 6);
        switch (index)
        {
            case 0:
                return angle = Random.Range(22.5f, 37.5f);
            case 1:
                return angle = Random.Range(82.5f, 97.5f);
            case 2:
                return angle = Random.Range(142.5f, 157.5f);
            case 3:
                return angle = Random.Range(202.5f, 217.5f);
            case 4:
                return angle = Random.Range(262.5f, 277.5f);
            case 5:
                return angle = Random.Range(322.5f, 337.5f);
            default:
                print("error");
                return angle = Random.Range(0, 360);
        }
    }
}
