using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenShpaeController : ShapeBaseController
{
    private void Start()
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
            if (hp > MaxHp) hp = MaxHp;
            time = 0;
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        //计算collison受到的血量增益
        c.hpRate = Math.Round(CalculateAddHp(collision.gameObject, hpIncFactor), 2);
    }
}
