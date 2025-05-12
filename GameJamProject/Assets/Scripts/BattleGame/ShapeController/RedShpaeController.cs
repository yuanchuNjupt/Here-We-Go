using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShpaeController : ShapeBaseController
{
    [Tooltip("每秒射击子弹数量")]
    public int bulletNumbysec = 6;
    public GameObject bullet;

    private Camera cam;
    private float t;
    private float interval;

    private void Awake()
    {
        cam = Camera.main;
        //计算每秒射击频率
        interval = 1 / bulletNumbysec;
    }

    protected override void Update()
    {
        base.Update();
        if (!GameDataMgr.instance.isBattleStart) return;
        time += Time.deltaTime;
        //增加血量与伤害
        if (time >= 1)
        {
            atk += atkRate;
            GameDataMgr.instance.attack = (float)atk;
            hp += hpRate;
            if (hp > MaxHp) hp = MaxHp;
            time = 0;
        }
        //左键按住射击
        if (Input.GetMouseButton(0))
        {
            t += Time.deltaTime;
            if (t > interval)
            {
                Shoot();
                t = 0;
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (GameDataMgr.instance.isBattleStart || !GameDataMgr.instance.isDragged)
            return;
        base.OnTriggerExit2D (collision);
        if (collision.GetComponent<GreenShpaeController>() != null)
        {
            print(GameDataMgr.instance.isDragged);
            hpRate = 0;
        }
        else
        {
            atkRate = 0;
        }
    }

    /// <summary>
    /// 射击
    /// </summary>
    private void Shoot()
    {
        //计算对象至鼠标位置的方向向量
        Vector2 dir = GetMouseWorldPos() - (Vector2)this.transform.position;
        //计算角度
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        Instantiate(bullet, this.transform.position, Quaternion.Euler(0, 0, -angle));
    }

    /// <summary>
    /// 获取鼠标的Vector2D坐标
    /// </summary>
    private Vector2 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z;
        return cam.ScreenToWorldPoint(mousePos);
    }
}
