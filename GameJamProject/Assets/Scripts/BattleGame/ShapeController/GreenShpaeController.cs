using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenShpaeController : ShapeBaseController
{
    protected override void Update()
    {
        base.Update();
        if (!GameDataMgr.instance.isBattleStart) return;
        print("start");
        time += Time.deltaTime;
        if (time >= 1)
        {
            //增加血量与伤害
            atk += atkRate;
            hp += hpRate;
            if (hp > MaxHp) hp = MaxHp;
            time = 0;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var c = collision.GetComponent<ShapeBaseController>();
        if (collision.CompareTag("Player"))
        {
            //计算collison受到的血量增益
            c.hpRate = Math.Round(CalculateAddHp(collision.gameObject, hpIncFactor), 2);
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (GameDataMgr.instance.isBattleStart || !GameDataMgr.instance.isDragged)
            return;
        if (collision.GetComponent<GreenShpaeController>() != null)
        {
            hpRate = 0;
        }
        else
        {
            atkRate = 0;
        }
    }
}
