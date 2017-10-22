using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page_Battle : MonoBehaviour {

    public Text curTime;

    public Text curProgress;


 
	

	void Update ()
    {
        curTime.text ="剩余时间:"+ (int)(BattleManager.Instance.maxTime - BattleManager.Instance.curTime);

        curProgress.text = "剩余路程:" + (int)(BattleManager.Instance.maxProgress - BattleManager.Instance.curProgress)+"(米)";

    }
}
