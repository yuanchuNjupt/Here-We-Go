using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 1.5f;
    public float attack = 1;
    public GameObject deadEff;

    void Start()
    {
        
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
            //销毁子弹
            Destroy(gameObject);
            //产生特效
            Instantiate(deadEff, this.transform.position, deadEff.transform.rotation);
        }
    }
}
