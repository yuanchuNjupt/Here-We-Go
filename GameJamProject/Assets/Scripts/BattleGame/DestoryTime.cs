using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryTime : MonoBehaviour
{
    [Tooltip("设定销毁时间")]
    public float destroyTime = 2;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
