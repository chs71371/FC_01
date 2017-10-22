using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dest.Math;

public class BattleManager : MonoBehaviour {

    public static BattleManager Instance;

    public Page_BattleUI mBattleUI;
    /// <summary>
    /// 当前关卡序列
    /// </summary>
    [HideInInspector]
    public int curLevelIndex;
    /// <summary>
    /// 相机相对位置
    /// </summary>
    public Vector3 cameraPos;
    /// <summary>
    /// 当前玩家
    /// </summary>
    public BattlePlayer mPlayer;
    /// <summary>
    /// 当前关卡
    /// </summary>
    private GameObject curLevel;
    /// <summary>
    /// 当前寻路线段
    /// </summary>
    public CatmullRomSpline3 curSpline;
    /// <summary>
    /// 剩余时间
    /// </summary>
    public float remainingTime;
    /// <summary>
    /// 最大距离
    /// </summary>
    public float maxDistance;


    public enum eState
    {
        Wait,
        Run,
        Win,
        Lose
    }

    public eState curState = eState.Wait;



    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        InitData();
        InitUI();
    }


    public void BattleReady()
    {
        InitPlayer();
        InitScene();
        BattleReStart();
    }


    public void BattleReStart()
    {
        remainingTime = 60;
        mPlayer.curProgress = 0;
        curState = eState.Run;
    }



    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData() 
    {
        curLevelIndex = 0;
    }


    /// <summary>
    /// 初始化UI
    /// </summary>
    public void InitUI()
    {
        var obj = Resources.Load("ui/UIRoot") as GameObject;
        obj = GameObject.Instantiate(obj);
        obj.name = "UIRoot";
    }

 
    /// <summary>
    /// 初始化玩家
    /// </summary>
    public void InitPlayer()
    {
        var obj = Resources.Load("prefabs/Player") as GameObject;
        obj = GameObject.Instantiate(obj);
        obj.name = "Player";
        mPlayer = obj.AddComponent<BattlePlayer>();
    }


    public void InitScene()
    {
        curLevel = CreatLevelLand("level_land_0"+ curLevelIndex, new Vector3(0, 0, 0));
        curSpline = curLevel.transform.Find("Wayfinding").GetComponent<CatmullRomSpline3>();

        var _splineLength = curSpline.ParametrizeByArcLength(100);
        maxDistance = curSpline.CalcTotalLength();
    }



    private void Update()
    {
        if (curState != eState.Run)
        {
            return;
        }

        remainingTime -= Time.deltaTime;

        if (mPlayer.curProgress >= maxDistance)
        {
            SetBattleOver(eState.Win);
        }

        if (remainingTime<=0)
        {
            SetBattleOver(eState.Lose);
        }
    }

    private void LateUpdate()
    {
        if (mPlayer != null)
        {
            var targerPos = mPlayer.mWayPos + mPlayer.gameObject.transform.rotation * cameraPos;
            Camera.main.transform.position = targerPos;
            Camera.main.transform.forward = mPlayer.mWayDir;
        }
    }


    public GameObject CreatLevelLand(string rLandIndex,Vector3 rPos)
    {
        var obj = Resources.Load("prefabs/"+rLandIndex) as GameObject;
        obj = GameObject.Instantiate(obj);
        obj.name = rLandIndex;
        obj.transform.position = rPos;
        obj.transform.eulerAngles = Vector3.zero;
        return obj;
    }

    /// <summary>
    /// 设置关卡战斗结束
    /// </summary>
    public void SetBattleOver(eState rState)
    {
        curState = rState;

        switch (curState)
        {
            case eState.Win:
                curLevelIndex++;
                Destroy(mPlayer.gameObject);
                Destroy(curLevel);
                mBattleUI.ShowLevel();
                break;
            case eState.Lose:
                BattleReStart();
                break;
        }

    }
}