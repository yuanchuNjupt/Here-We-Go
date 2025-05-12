using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Action : EnemyActionBase
{
    [Tooltip("子弹射击间隔时间")]
    public float fireTime = 1;

    private GameObject bullet;
    private bool isMove = true;

    private void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet/Bullet1");
        StartCoroutine(FireBullet());
    }

    protected override void Update()
    {
        if (hp <= 0)
        {
            //死亡销毁对象并产生死亡特效
            Destroy(gameObject);
            Instantiate(deadDestoryEff, this.transform.position, deadDestoryEff.transform.rotation);
            return;
        }
        if (!isMove)
            return;
        transform.position += transform.up * speed * Time.deltaTime;
    }
    IEnumerator FireBullet()
    {
        yield return new WaitForSeconds(2.5f);
        while (true)
        {
            Instantiate(bullet, transform.position, transform.localRotation);
            yield return new WaitForSeconds(fireTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //将对象依附在玩家上
            transform.SetParent(collision.transform, true);
            //禁止移动
            isMove = false;
        }
    }
}
