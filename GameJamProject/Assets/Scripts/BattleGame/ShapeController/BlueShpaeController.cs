using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShpaeController : ShapeBaseController
{
    // Update is called once per frame
    protected  void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) GameDataMgr.instance.isBattleStart = true;//测试用
        if (!GameDataMgr.instance.isBattleStart) return;
        time += Time.deltaTime;
        if (time >= 1)
        {
            hp += hpRate;
            if (hp > MaxHp) hp = MaxHp;
            time = 0;
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        base.OnTriggerStay2D(collision);
        //计算collison受到的伤害增益
        c.atkRate =Math.Round(CalculateAddAtk(collision.gameObject, atkIncFactor), 2);
    }
}
