using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BoomEff : MonoBehaviour
{
    public float attack = 5;
    [Tooltip("爆炸扩散大小")]
    public Vector3 finalScale = new Vector3(1.5f, 1.5f, 1.5f);
    [Tooltip("爆炸扩散持续时间")]
    public float expandTime = 1.5f;
    [Tooltip("爆炸消失时间")]
    public float expandFadeTime = 1.5f;

    private float time = 0;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        Destroy(gameObject, 3);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time < expandTime)
        {
            //放大对象
            float t = Mathf.Clamp01(time / expandTime);
            transform.localScale = Vector3.LerpUnclamped(Vector3.zero, finalScale, t * (2 - t));
        }
        else
        {
            //渐变消失
            Color color = sr.color;
            float t = Mathf.Clamp01((time - expandTime) / expandFadeTime);
            color.a = Mathf.Lerp(sr.color.a, 0, t * t);
            sr.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //造成伤害
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ShapeBaseController>().hp -= attack;
        }
        else if (collision.CompareTag("EnemyBullet"))
        {
            collision.gameObject.GetComponent<BulletController>().Dead();
            return;
        }
        else if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyActionBase>().hp -= attack;
        }
    }
}