using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyActionBase : MonoBehaviour
{
    public float hp = 10;
    public float atk = 1;
    public float speed = 1;

    protected virtual void Update()
    {
        if (transform.position.magnitude < 0.1f)
            return;

        transform.position += transform.up * speed * Time.deltaTime;
    }

}
