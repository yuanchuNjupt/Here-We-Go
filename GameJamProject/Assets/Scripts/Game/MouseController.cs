using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Camera cam;
    private Vector2 offSet;
    private GameObject target;
    //原始图层的层级
    private int originalOrder;
    //当前的最大层级数
    private int currentMaxOrder = 1;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        //当按下鼠标时
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(GetMouseWorldPos(), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("DragObj"));
            if (hit.collider != null)
            {
                //记录射线接触的拖拽对象
                target = hit.collider.gameObject;
                //获取鼠标点击位置与物体中心的偏移量
                offSet = (Vector2)target.transform.position - GetMouseWorldPos();
                //将拖拽的图形置顶
                TopUpOrder(target);//默认的解密图形的层数应该为1
            }
        }
        //当鼠标拖拽时
        if (Input.GetMouseButton(0) && target != null)
        {
            target.transform.position = offSet + GetMouseWorldPos();
        }
        //当鼠标松开时
        if (Input.GetMouseButtonUp(0))
        {
            target = null;
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

    /// <summary>
    /// 置顶该对象的图层层级及恢复层级
    /// </summary>
    private void TopUpOrder(GameObject target)
    {
        SpriteRenderer renderer = SpriteRendererMgr.Instance.LoadSpriteRenderer(target.transform.GetChild(0).gameObject);
        currentMaxOrder++;//每次点击都会增加最大层级，确保每次点击对象的层级都能置顶
        renderer.sortingOrder = currentMaxOrder;
    }
}
