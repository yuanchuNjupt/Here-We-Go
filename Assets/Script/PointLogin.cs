using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public enum PointType
{
    Three, //150 , 100 , 50 , 0 , 255 red
    Two    //150 , 75 ,0 , 255 red
}
//对于两种不同的点 要进行两种判断 所以一开始就要说明是什么点
public class PointLogin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public PointType pointType;
    public int lockNumber;
    private SpriteRenderer sr;
    private Color color;
    private bool check;
    
    private HashSet<Collider2D> _objectsInRing = new HashSet<Collider2D>();
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        //图形进入
        lockNumber--;
        UpdateState();
        Debug.Log(this.gameObject.name + "图形进入" + lockNumber);

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Vector3.Distance(other.transform.position, transform.position) <= 0.09f * 6f +
            other.gameObject.GetComponent<PictureLogin>().scale * 4.62f &&
            Vector3.Distance(other.transform.position, transform.position) >=
            other.gameObject.GetComponent<PictureLogin>().scale * 4.52f - 0.09f * 6f)
        {
            Debug.Log("图形一直在里面");
        }
        else
        {
            Debug.Log("图形不在里面");
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        lockNumber++;
        UpdateState();
        Debug.Log(this.gameObject.name + "图形退出" + lockNumber);
    }*/
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Circle"))
        {

            PictureLogin pictureLogin = other.GetComponent<PictureLogin>();
            if (pictureLogin == null) return;

            // 计算关键参数
            float outerThreshold = 0.09f * 6f + pictureLogin.scale * 4.62f;
            float innerThreshold = pictureLogin.scale * 4.52f - 0.09f * 6f;
            float currentDistance = Vector3.Distance(other.transform.position, transform.position);

            // 判断是否在环形区域内
            bool isInRing = currentDistance <= outerThreshold && currentDistance >= innerThreshold;

            // 状态切换检测
            if (isInRing)
            {
                if (!_objectsInRing.Contains(other))
                {
                    OnEnterRing(other); // 触发进入事件
                    _objectsInRing.Add(other);
                }
            }
            else
            {
                if (_objectsInRing.Contains(other))
                {
                    OnExitRing(other); // 触发退出事件
                    _objectsInRing.Remove(other);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Triangle")) 
        {
            lockNumber++;
            UpdateState();
        }
        // 确保物体离开时清除状态
        else if (other.gameObject.CompareTag("Circle")) 
        {
            if (_objectsInRing.Contains(other))
            {
                OnExitRing(other);
                _objectsInRing.Remove(other);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Triangle"))
        {
            lockNumber--;
            UpdateState();
        }
    }

    // 进入环形区域事件（只触发一次）
    private void OnEnterRing(Collider2D target)
    {
        Debug.Log($"进入环形区域: {target.name}");
        // 在此添加进入逻辑（如播放音效、触发任务等）
        lockNumber--;
        UpdateState();
        Debug.Log(this.gameObject.name + "图形进入" + lockNumber);
    }

    // 退出环形区域事件（只触发一次）
    private void OnExitRing(Collider2D target)
    {
        Debug.Log($"退出环形区域: {target.name}");
        // 在此添加退出逻辑
        lockNumber++;
        UpdateState();
        Debug.Log(this.gameObject.name + "图形退出" + lockNumber);
    }

    void Start()
    {
       if (pointType == PointType.Three)
           lockNumber = 3;
       else if (pointType == PointType.Two)
           lockNumber = 2;
       sr = GetComponent<SpriteRenderer>();
       color = sr.color;
       UpdateState();
    }
    public void UpdateState()
    {
        if (pointType == PointType.Three)
        {
            switch (lockNumber)
            {
                case 3:
                    ChangeAlpha(150);
                    break;
                case 2:
                    ChangeAlpha(100);
                    break;
                case 1:
                    ChangeAlpha(50);
                    break;
                case 0:
                    ChangeAlpha(0);
                    CheckTrigger.Instance.CheckAllPoint();
                    break;
                default:
                    ChangeAlpha(255 , true);
                    break;
            }
        }
        else if (pointType == PointType.Two)
        {
            switch (lockNumber)
            {
               
                case 2:
                    ChangeAlpha(150);
                    break;
                case 1:
                    ChangeAlpha(75);
                    break;
                case 0:
                    ChangeAlpha(0);
                    CheckTrigger.Instance.CheckAllPoint();
                    break;
                default:
                    ChangeAlpha(255 , true);
                    break;
            }
        }
    }

    public void ChangeAlpha(float alpha , bool isRed = false)
    {
        Color c;
        if (isRed)
        {
            c = Color.red;
        }
        else
        {
            c = color;
        }
        c.a = alpha / 255f;
        sr.color = c;
    }
    
}
