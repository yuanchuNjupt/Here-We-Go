using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public float CirMaxScale = 0.93f;  //0.93f   //8.5f
    public float CirMinScale = 0.35f;  //0.35f   //3.2f
    public float TriMaxScale = 0.265f;
    public float TriMinScale = 0.1f;
    public int MaxPictures = 7;
    public float Scrollspeed = 1f;
    public float CirInitScale = 0.5f;    // 0.5f   //5f
    public float TriInitScale = 0.175f;    // 0.1f   //1f
    public float PointRadius = 0.09f * 1.5f;
    public float RotateSpeed = 90f;
    //初步规定
    //环形外圈r = 4.62f
    //环形内圈r = 4.52f
    public int CurrentUnlockedPicture = 0;
}

public class GameDataMgr
{
    private static GameDataMgr _instance = new GameDataMgr();
    public static GameDataMgr Instance => _instance;
    public GameData GameData;

    public GameDataMgr()
    {
        GameData = new GameData();
    }

   
}
