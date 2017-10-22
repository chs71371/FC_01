using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page_BattleUI : MonoBehaviour {

    private Text Txt_DistanceTip;

    private Text Txt_TimeTip;

    private List<Animator> levelAnimatorList = new List<Animator>();

    private Transform Tran_BattlePanel;

    private Transform Tran_MenuPanel;

    public enum eState
    {
        Menu,
        Battle
    }

    public eState curState= eState.Menu;

    private float mTime;


    private void Awake()
    {
        BattleManager.Instance.mBattleUI = this;
    }


    void Start()
    {

        Tran_BattlePanel = gameObject.transform.Find("Tran_BattlePanel");
        Tran_MenuPanel = gameObject.transform.Find("Tran_Menu");


        Txt_DistanceTip = gameObject.transform.Find("Tran_BattlePanel/Txt_DistanceTip").GetComponent<Text>();
        Txt_TimeTip = gameObject.transform.Find("Tran_BattlePanel/Txt_TimeTip").GetComponent<Text>();


        for (int i = 0; i < 8; i++)
        {
            levelAnimatorList.Add(gameObject.transform.Find("Tran_Menu/" + i).GetComponent<Animator>());
        }


        ShowLevel();
    }


    void Update()
    {
        switch (curState)
        {
            case eState.Menu:
                mTime += Time.deltaTime;
                if (mTime > 3.5f)
                {
                    ShowBattle();
                    mTime = 0;
                }
                break;
            case eState.Battle:
                if (BattleManager.Instance.curState == BattleManager.eState.Run)
                {
                    Txt_DistanceTip.text = "剩余路程：" + (int)(BattleManager.Instance.maxDistance - BattleManager.Instance.mPlayer.curProgress) + "米";
                    Txt_TimeTip.text = "剩余时间：" + (int)BattleManager.Instance.remainingTime + "秒";
                }
                break;
        }
    }


    public void ShowLevel()
    {
        Tran_BattlePanel.gameObject.SetActive(false);
        Tran_MenuPanel.gameObject.SetActive(true);
        levelAnimatorList[BattleManager.Instance.curLevelIndex].enabled = true;
        curState = eState.Menu;
    }

    public void ShowBattle()
    {
        BattleManager.Instance.BattleReady();
        Tran_BattlePanel.gameObject.SetActive(true);
        Tran_MenuPanel.gameObject.SetActive(false);
        levelAnimatorList[BattleManager.Instance.curLevelIndex].enabled = false;
        curState = eState.Battle;
    }

}
