using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    private static CheckTrigger _instance;
    public static CheckTrigger Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    //检测所有的点位出否都是解锁状态
    public List<PointLogin> pointLogins;

    public void CheckAllPoint()
    {
        Debug.Log("Check All Point");
        foreach (PointLogin pointLogin in pointLogins)
        {
            if(pointLogin.lockNumber != 0 )
                return;
        }
        Debug.Log("All Point is Unlock");
        UIManager.Instance.GetPanel<GamePanel>().AfterInfo.text = "检测到已满足通关条件\n请进行后续处理";
    }


}
