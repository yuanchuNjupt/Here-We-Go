using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Text number;
    public Text AfterInfo;
    public override void Init()
    {
        UpdateNumber(GameDataMgr.Instance.GameData.MaxPictures);
    }

    public void UpdateNumber(int numbers)
    {
        number.text = "剩余图形数 : " + numbers;
    }
}
