using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Action : EnemyActionBase
{
    public float fireTime = 1;

    private GameObject bullet;

    private void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet/Bullet1");
        StartCoroutine(FireBullet());
    }

    protected override void Update()
    {
        base.Update();
    }
    IEnumerator FireBullet()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            Instantiate(bullet, transform.position, transform.localRotation);
            yield return new WaitForSeconds(fireTime);
        }
    }
}
