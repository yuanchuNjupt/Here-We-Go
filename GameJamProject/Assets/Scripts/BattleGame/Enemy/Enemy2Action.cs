using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Action : EnemyActionBase
{
    public GameObject boomEff;

    protected override void Update()
    {
        if (hp <= 0)
        {
            Dead();
        }
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Dead();
        }
    }

    public void Dead()
    {
        //销毁对象
        Destroy(gameObject);
        //产生爆炸
        Instantiate(boomEff, this.transform.position, this.transform.rotation);
    }
}
