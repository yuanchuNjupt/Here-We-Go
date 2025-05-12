using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 1.5f;
    public float attack = 1;
    public GameObject deadEff;

    private bool isRebound = false;

    private void Start()
    {
        Destroy(gameObject, 15);
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //造成伤害
            collision.gameObject.GetComponent<ShapeBaseController>().hp -= attack;
            //如果是蓝色反弹图形，则跳过
            if (collision.gameObject.name.Equals("BlueShape"))
            {
                isRebound = true;
                return;
            }
            Dead();
        }
        else if (collision.CompareTag("Enemy") && isRebound)
        {
            //造成伤害
            collision.gameObject.GetComponent<EnemyActionBase>().hp -= GameDataMgr.instance.attack;
            //销毁子弹
            Destroy(gameObject);
            //产生特效
            Instantiate(deadEff, this.transform.position, deadEff.transform.rotation);
        }
    }

    public void Dead()
    {
        //销毁子弹
        Destroy(gameObject);
        //产生特效
        Instantiate(deadEff, this.transform.position, deadEff.transform.rotation);
    }
}
