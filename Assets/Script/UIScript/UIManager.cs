using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //����
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;
    //������canvas
    private Transform canvasTrans;
    private UIManager()
    {
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //ȫ��ϷΨһ��canvas 
        GameObject.DontDestroyOnLoad(canvas);
    }


    //�����滻
    //���ڴ洢��ʾ�ŵ����
    //�������ʱ ֱ�ӻ�ȡ�ֵ��еĶ�Ӧ���
    private Dictionary<string, BasePanel> panelDIc = new Dictionary<string, BasePanel>();

    //��ʾ
    /// <summary>
    /// 
    /// </summary> 
    /// <typeparam name="T">�������</typeparam>
    /// <param name="isTimeStop">�Ƿ�ʱͣ</param>
    /// <returns></returns>
    public T ShowPanel<T>(bool isTimeStop = false) where T : BasePanel
    {
        //���� : T��������ֱ���һ��
        string panelName = typeof(T).Name;
        //�Ƿ�֮ǰ�Ѿ���ʾ?
        if (panelDIc.ContainsKey(panelName))
        {
            return panelDIc[panelName] as T;
        }
        //����Ԥ���� ����������
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));

        //�õ���ŵ�canvas��
        panelObj.transform.SetParent(canvasTrans , false);

        //ָ����� ��ʾ�߼� ��������������~
        T panel = panelObj.GetComponent<T>();
        //�����ű��浽�ֵ��� �����������
        panelDIc.Add(panelName, panel);
        //��ʾ
        if (isTimeStop)
        {
            panel.Showme(() =>
            {
                Time.timeScale = 0;
            });
        }
        else
        {
            panel.Showme(null);
        }


        //���þͷ���
        return panel;
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="isFade">�Ƿ�Ҫ���뵭��</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if(panelDIc.ContainsKey(panelName))
        {
            //��Ҫ���뵭����?
            if (isFade)
            {
                //��������ɾ
                panelDIc[panelName].Hideme(() =>
                {
                    GameObject.Destroy(panelDIc[panelName].gameObject);
                    //ɾ���ֵ����Ľű�
                    panelDIc.Remove(panelName);
                });
            }
            else
            {
                GameObject.Destroy(panelDIc[panelName].gameObject);
                //ɾ���ֵ����Ľű�
                panelDIc.Remove(panelName);
            }
            
        }
    }
    //Get
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDIc.ContainsKey(panelName))
        {
            return panelDIc[panelName] as T;
        }
        //û��� 
        return null;
    }
}
