using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//������
public abstract class BasePanel : MonoBehaviour
{
    // Start is called before the first frame update
    private CanvasGroup canvasGroup;
    //���뵭���ٶ�
    private float alphaspeed = 10;
    //��ʾ��������?
    private bool isShow = false;
    //���غ����ɶ?
    private UnityAction hideCallBack = null;
    //��ʾ��ʱͣ
    private UnityAction showCallBack = null;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null )
        {
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
    }
    protected virtual void Start()
    {
        Init();
    }
    //ע��ؼ��¼�  ����ע��
    public abstract void Init();
  
    //���ί��
    public virtual void Showme(UnityAction callBack)//Unity callback
    {
        canvasGroup.alpha = 0;
        isShow = true;

        showCallBack = callBack;
    }

    public virtual void Hideme(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;

        hideCallBack = callBack;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if ( isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaspeed * Time.deltaTime;
            if ( canvasGroup.alpha >= 1 )
            {
                canvasGroup.alpha = 1;
                showCallBack?.Invoke();
            }
        }
        else if( !isShow && canvasGroup.alpha != 0 )
        {
            canvasGroup.alpha -= alphaspeed * Time.deltaTime;
            if( canvasGroup.alpha <= 0 )
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
            }
        }
    }
}
