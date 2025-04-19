using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShpaeController : ShapeBaseController
{
    void Start()
    {
        MaxHp = hp;
    }

    protected void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) GameDataMgr.instance.isBattleStart = true;//测试用
        if (!GameDataMgr.instance.isBattleStart) return;
        time += Time.deltaTime;
        if (time >= 1)
        {
            atk += atkRate;
            hp += hpRate;
            if(hp>MaxHp) hp = MaxHp;
            time = 0;
        }
    }
}
