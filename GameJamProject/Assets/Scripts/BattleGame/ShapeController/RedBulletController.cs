using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBulletController : MonoBehaviour
{
    public float speed = 1.5f;
    public GameObject deadEff;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //造成伤害
            collision.gameObject.GetComponent<EnemyActionBase>().hp -= GameDataMgr.instance.attack;
            //销毁子弹
            Destroy(gameObject);
            //产生特效
            Instantiate(deadEff, this.transform.position, deadEff.transform.rotation);
        }
    }
}
