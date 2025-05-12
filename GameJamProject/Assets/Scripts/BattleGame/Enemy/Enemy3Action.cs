using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Action : EnemyActionBase
{
    [Tooltip("距离原点停止的距离")]
    public float distance = 6;
    [Tooltip("子弹射击间隔时间")]
    public float fireTime = 1;

    private GameObject bullet;

    private void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet/Bullet2");
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
        //到达外环停止移动
        if (Vector2.Distance(transform.position, Vector2.zero) < distance)
            return;
        //位移
        transform.position += transform.up * speed * Time.deltaTime;
    }

    IEnumerator FireBullet()
    {
        yield return new WaitForSeconds(3);
        while (true)
        {
            Instantiate(bullet, transform.position, transform.localRotation);
            yield return new WaitForSeconds(fireTime);
        }
    }
}
