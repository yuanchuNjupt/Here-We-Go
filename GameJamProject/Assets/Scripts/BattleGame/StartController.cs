using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StartController : MonoBehaviour
{
    [Tooltip("摄像头到达目标所需时间")]
    public float smoothTime = 1;
    [Tooltip("开始游戏后，生成敌人的时间")]
    public float delay = 3;

    private GameObject enemyMgr;
    private float time;
    private Vector3 targetPos;
    //摄像头初速度
    private Vector3 velocity;

    void Start()
    {
        enemyMgr = GameObject.Find("EnemyMgr");
        enemyMgr.SetActive(false);
        targetPos = new Vector3(transform.position.x, transform.position.y, -15);
    }

    void Update()
    {
        //离开准备阶段开始游戏（等待UI连接）
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameDataMgr.instance.isBattleStart = true;
            StartCoroutine(StartEnemy());
        }
        if (GameDataMgr.instance.isBattleStart)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }

    //开始生成敌人
    IEnumerator StartEnemy()
    {
        yield return new WaitForSeconds(delay);
        enemyMgr.SetActive(true);
        //失活该脚本，减少内存消耗
        this.GetComponent<StartController>().enabled = false;
    }
}
