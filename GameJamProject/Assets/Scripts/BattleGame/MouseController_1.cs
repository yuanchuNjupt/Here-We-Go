using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 适用于BattleScene里的拖拽
/// </summary>
public class MouseController_1 : MonoBehaviour
{
    private Camera cam;
    private Vector2 offSet;
    private GameObject target;
    private Vector2 rotateStartDir;
    private bool isRotate = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        //如果战斗开始，禁止鼠标拖动检测
        if (GameDataMgr.instance.isBattleStart) return;
        //当按下鼠标时
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(GetMouseWorldPos(), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("DragObj"));
            if (hits.Length != 0)
            {
                // 按照 SpriteRenderer 的 order 排序（从高到低）
                System.Array.Sort(hits, (a, b) => SpriteRendererMgr.Instance.LoadSpriteRenderer(b.collider.gameObject).sortingOrder.CompareTo(SpriteRendererMgr.Instance.LoadSpriteRenderer(a.collider.gameObject).sortingOrder)
                );
                //记录射线接触的拖拽对象(最高order对象)
                target = hits[0].collider.gameObject;
                //获取鼠标点击位置与物体中心的偏移量
                offSet = (Vector2)target.transform.position - GetMouseWorldPos();
            }
            GameDataMgr.instance.isDragged = true;
        }
        //当鼠标右键按下时
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(GetMouseWorldPos(), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("DragObj"));
            if (hit.collider != null)
            {
                //记录射线接触的拖拽对象
                target = hit.collider.gameObject;
                Vector2 center = target.transform.position;
                rotateStartDir = GetMouseWorldPos() - center;
                isRotate = true;
            }
            GameDataMgr.instance.isDragged = true;
        }

        //当鼠标拖拽时
        if (Input.GetMouseButton(0) && target != null && !GameDataMgr.instance.isBlock)
        {
            target.transform.position = offSet + GetMouseWorldPos();
        }
        //当鼠标右键按住时
        if (Input.GetMouseButton(1) && isRotate && !GameDataMgr.instance.isBlock)
        {
            Vector2 center = target.transform.position;
            Vector2 currentDir = GetMouseWorldPos() - center;
            float angle = Vector2.SignedAngle(rotateStartDir, currentDir);
            target.transform.Rotate(0, 0, angle);
            rotateStartDir = currentDir;
        }

        //当鼠标松开时
        if (Input.GetMouseButtonUp(0))
        {
            if (target != null)
            {
                target = null;
                isRotate = false;
            }
            GameDataMgr.instance.isDragged = false;
        }
        //当鼠标右键松开时
        if (Input.GetMouseButtonUp(1))
        {
            isRotate = false;
            GameDataMgr.instance.isBlock = false;
            GameDataMgr.instance.isDragged = false;
        }
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
