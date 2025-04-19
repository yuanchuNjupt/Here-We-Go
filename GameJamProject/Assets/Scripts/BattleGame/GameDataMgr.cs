using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    public static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //战斗是否开始
    public bool isBattleStart = false;
    //是否开启阻挡图形进入
    public bool isBlock = false;
}
