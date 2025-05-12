using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShpaeController : ShapeBaseController
{
    protected override void Update()
    {
        base.Update();
        if (!GameDataMgr.instance.isBattleStart) return;
        time += Time.deltaTime;
        //增加血量
        if (time >= 1)
        {
            hp += hpRate;
            if (hp > MaxHp) hp = MaxHp;
            time = 0;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D (collision);
        //接触图形后反弹敌方子弹
        if (collision.CompareTag("EnemyBullet"))
        {
            //调转子弹方向
            Vector3 rotation = collision.transform.eulerAngles;
            rotation.z += 180;
            collision.transform.eulerAngles = rotation;
            //去除敌方子弹标签
            collision.tag = "Untagged";
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var c = collision.GetComponent<ShapeBaseController>();
        if (collision.CompareTag("Player"))
        {
            //计算collison受到的伤害增益
            c.atkRate = Math.Round(CalculateAddAtk(collision.gameObject, atkIncFactor), 2);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (GameDataMgr.instance.isBattleStart || !GameDataMgr.instance.isDragged)
            return;
        base.OnTriggerExit2D(collision);
        if (collision.GetComponent<GreenShpaeController>() != null)
        {
            hpRate = 0;
        }
    }
}
