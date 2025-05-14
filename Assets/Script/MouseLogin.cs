using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum MouseState
{
    HavePic,
    NoPic
}
public class MouseLogin : MonoBehaviour
{
    private RaycastHit2D hitInfo;
    private Vector2 ClickPos;
    private MouseState mouseState = MouseState.NoPic;
    private GameObject currentPic;
    private int picNumber = GameDataMgr.Instance.GameData.MaxPictures;
    // Start is called before the first frame update
    void Start()
    {
        //初始化UI
        UIManager.Instance.ShowPanel<GamePanel>();
    }

    // Update is called once per frame
    void Update()
    {
        //如果点击鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("鼠标左键按下");
            //如果当前鼠标没有依附图片
            if (mouseState == MouseState.NoPic)
            {
                //如果点击到图片
                // LayerMask.GetMask("Picture")
                ClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hitInfo = Physics2D.Raycast(ClickPos, Vector2.zero , Mathf.Infinity , LayerMask.GetMask("Picture"));
                if (hitInfo.collider)
                {
                    Debug.Log("点击到图片");
                    mouseState = MouseState.HavePic;
                    if (hitInfo.collider.CompareTag("TriangleMouse"))
                    {
                        currentPic = hitInfo.collider.gameObject.transform.parent.gameObject;
                    }
                    else
                    {
                        currentPic = hitInfo.collider.gameObject;

                    }
                    currentPic.GetComponent<PictureLogin>().state = PictureState.FollowMouse;
                }
                //没点到就无事发生
            }
            else if (mouseState == MouseState.HavePic)
            {
                Debug.Log("取消图片的附着");
                //取消图片的附着
                
                mouseState = MouseState.NoPic;

                currentPic.GetComponent<PictureLogin>().state = PictureState.Static;
                currentPic = null;
            }
        }
        //提供取出圆 取出三角 删除图形
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //当前鼠标没有依附图片
            if (mouseState == MouseState.NoPic && picNumber > 0)
            {
                //生成圆形
                Debug.Log("生成圆形");
                picNumber--;
                currentPic = Instantiate(Resources.Load<GameObject>("2"));
                mouseState = MouseState.HavePic;
                //更新UI
                UIManager.Instance.GetPanel<GamePanel>().UpdateNumber(picNumber);

            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //当前鼠标没有依附图片
            if (mouseState == MouseState.NoPic && picNumber > 0)
            {
                Debug.Log("生成三角形");
                //生成三角形
                picNumber--;
                currentPic = Instantiate(Resources.Load<GameObject>("3"));
                mouseState = MouseState.HavePic;
                // Instantiate(Resources.Load<GameObject>("Triangle"));
                UIManager.Instance.GetPanel<GamePanel>().UpdateNumber(picNumber);
            }
        }
        //删除图形
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (mouseState == MouseState.HavePic)
            {
                //删除图形
                picNumber++;
                Destroy(currentPic);
                currentPic = null;
                mouseState = MouseState.NoPic;
                //更新UI
                UIManager.Instance.GetPanel<GamePanel>().UpdateNumber(picNumber);
            }
        }
        //旋转三角形
        if (Input.GetKey(KeyCode.Q) && mouseState == MouseState.HavePic && currentPic.GetComponent<PictureLogin>().type == PictureType.Triangle)
        {
            currentPic.transform.Rotate(new Vector3(0 , 0 , 1) ,Time.deltaTime * GameDataMgr.Instance.GameData.RotateSpeed);
        }
        if (Input.GetKey(KeyCode.E) && mouseState == MouseState.HavePic && currentPic.GetComponent<PictureLogin>().type == PictureType.Triangle)
        {
            currentPic.transform.Rotate(new Vector3(0 , 0 , -1) ,Time.deltaTime * GameDataMgr.Instance.GameData.RotateSpeed);
        }
    }
}
