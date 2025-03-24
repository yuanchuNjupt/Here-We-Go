using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpriteRendererMgr
{
    private static SpriteRendererMgr instance = new SpriteRendererMgr();
    public static SpriteRendererMgr Instance => instance;

    //存储已加载的SpriteRenderer组件
    private static Dictionary<GameObject, SpriteRenderer> loadedRenderer = new Dictionary<GameObject, SpriteRenderer>();

    /// <summary>
    /// 存储并加载SpriteRenderer组件
    /// </summary>
    public SpriteRenderer LoadSpriteRenderer(GameObject gameObject)
    {
        if (!loadedRenderer.ContainsKey(gameObject))
        {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            loadedRenderer.Add(gameObject, sr);
        }
        return loadedRenderer[gameObject];
    }
}
