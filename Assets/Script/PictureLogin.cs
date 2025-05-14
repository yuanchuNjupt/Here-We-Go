using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PictureState
{
    FollowMouse,
    Static
}

public enum PictureType
{
    Circle,
    Triangle,
}
public class PictureLogin : MonoBehaviour
{
    public PictureState state = PictureState.Static;
    // Start is called before the first frame update
    public float zOffset = 10;
    public float scale;
    public PictureType type;
    void Start()
    {
        state = PictureState.FollowMouse;
        switch (type)
        {
            case PictureType.Circle:
                scale = GameDataMgr.Instance.GameData.CirInitScale;
                break;
            case PictureType.Triangle:
                scale = GameDataMgr.Instance.GameData.TriInitScale;
                break;
        }
        this.transform.localScale = FloatToVector3(scale);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == PictureState.FollowMouse)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zOffset; // 确保此值与相机 Clipping Planes 匹配
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = worldPos;
            //钳制范围
            if (type == PictureType.Triangle)
            {
                scale = Mathf.Clamp(scale + Input.GetAxis("Mouse ScrollWheel") * 0.05f, GameDataMgr.Instance.GameData.TriMinScale, GameDataMgr.Instance.GameData.TriMaxScale);
            }
            else if (type == PictureType.Circle)
            {
                scale = Mathf.Clamp(scale + Input.GetAxis("Mouse ScrollWheel") * 0.1f,GameDataMgr.Instance.GameData.CirMinScale , GameDataMgr.Instance.GameData.CirMaxScale);
            }
            //应用缩放
            transform.localScale = FloatToVector3(scale);
        }
    }

    public Vector3 FloatToVector3(float value)
    {
        return new Vector3(value, value, value);
    }
}
