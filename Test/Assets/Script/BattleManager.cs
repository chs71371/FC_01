using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dest.Math;


public class BattleManager : MonoBehaviour
{

    public static BattleManager Instance;

    public GameObject mCam;

    public int curLevelId = 0;

    public List<GameObject> LevelList = new List<GameObject>();

    public GameObject mPlayer;

    public CatmullRomSpline3 mWay;

    public Player mPlayerScr;

    public float curProgress = 0;

    public float maxProgress;

    public float maxTime=60;

    public float curTime = 0;


    private GameObject curMapObj;
    private GameObject curPlayerObj;

    public void Awake()
    {
        Instance = this;
    }


    public void Start()
    {
        ReadBattle();
    }


    public void ReadBattle()
    {
        if (curLevelId >= LevelList.Count)
        {
            Debug.Log("游戏通关");
            return;
        }


        ClearData();
        mWay = LoadScene(curLevelId);
        LoadPlayer();
    }


    /// <summary>
    /// 创建好场景的预制体，并且将配置保存
    /// </summary>
    public CatmullRomSpline3 LoadScene(int rId)
    {
        var obj = GameObject.Instantiate(LevelList[rId]);
        obj.name = "Level:" + rId;
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        var scr= obj.GetComponent<CatmullRomSpline3>();
        maxProgress=scr.ParametrizeByArcLength(100);


        curMapObj = obj;
        return scr;
    }

    /// <summary>
    /// 初始化好玩家预制体，同时并将位置点重置到初始点
    /// </summary>
    public void LoadPlayer()
    {
        var obj = GameObject.Instantiate(mPlayer);
        obj.name = "Player";

        PositionTangent data;

        mWay.EvalPositionTangentParametrized(0, out data);
        mPlayer.transform.position = data.Position;
        mPlayer.transform.forward = data.Tangent;

        curPlayerObj = obj;

        mPlayerScr = obj.GetComponent<Player>();
    }



    public void Update()
    {
        OnCheckArriveWayEnd();
    }

    public void OnCheckArriveWayEnd()
    {
        if (curProgress >= maxProgress)
        {
            Win();
        }
        else
        {
            curTime += Time.deltaTime;
            if (curTime >= maxTime)
            {
                Lose();
            }
        }
           
    }

    public void Win()
    {
        Destroy(curMapObj);
        Destroy(curPlayerObj);
        curLevelId++;

        ReadBattle();
    }

    public void Lose()
    {
        Destroy(curMapObj);
        Destroy(curPlayerObj);

        ReadBattle();
    }

    public void ClearData()
    {
        curProgress = 0;
        curTime = 0;

    }

}
