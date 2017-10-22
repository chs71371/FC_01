using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {



    public float mStopTime = 2;

    private float curTime = 0;


    public enum eState
    {
        Wait,
        Run,
        Over,
    }

    public eState curState = eState.Wait;

    private void Update()
    {

        if (curState == eState.Run)
        {
            curTime += Time.deltaTime;

            if (curTime > mStopTime)
            {
                OnOver();
            }
        }

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            curState = eState.Run;
            OnTrigger();
        }
    }




    public void OnTrigger()
    {
        BattleManager.Instance.mPlayerScr.SetStopOrRun(false);
    }

    public void OnOver()
    {
        BattleManager.Instance.mPlayerScr.SetStopOrRun(true);
        curState = eState.Over;
    }

}
