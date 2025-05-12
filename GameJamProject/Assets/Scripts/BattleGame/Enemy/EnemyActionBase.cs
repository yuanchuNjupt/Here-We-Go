using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyActionBase : MonoBehaviour
{
    public float hp = 10;
    public float speed = 1;
    public GameObject deadDestoryEff;

    protected virtual void Update()
    {
/*        if (transform.position.magnitude < 0.1f)
            return;*/
        if(hp <= 0)
        {
            //死亡销毁对象并产生死亡特效
            Destroy(gameObject);
            Instantiate(deadDestoryEff, this.transform.position, deadDestoryEff.transform.rotation);
            return;
        }
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
