using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PolygonCollider2DMgr
{
    private static PolygonCollider2DMgr instance = new PolygonCollider2DMgr();
    public static PolygonCollider2DMgr Instance => instance;

    //存储已加载的PolygonCollider2D组件
    private static Dictionary<GameObject, PolygonCollider2D> loadedPolygonCollider2D = new Dictionary<GameObject, PolygonCollider2D>();

    /// <summary>
    /// 存储并加载PolygonCollider2D组件
    /// </summary>
    public PolygonCollider2D LoadPolygonCollider2D(GameObject gameObject)
    {
        if (!loadedPolygonCollider2D.ContainsKey(gameObject))
        {
            PolygonCollider2D sr = gameObject.GetComponent<PolygonCollider2D>();
            loadedPolygonCollider2D.Add(gameObject, sr);
        }
        return loadedPolygonCollider2D[gameObject];
    }
}
