using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public float  size=5;
 
    public enum eState
    {
        Wait,
        Run,
        Over
    }

    public eState curState = eState.Wait;

    private float startRunTime = 0;

 
    void Update()
    {

        switch (curState)
        {
            case eState.Wait:
                Vector3 distancePos = BattleManager.Instance.mPlayer.gameObject.transform.position - gameObject.transform.position;

                if (BattleManager.Instance.mPlayer.transform.position.y > 0)
                {
                    return;
                }

                if (Mathf.Abs(distancePos.x) < size*transform.localScale.x && Mathf.Abs(distancePos.z) < size * transform.localScale.z)
                {
                    BattleManager.Instance.mPlayer.SetPlayerMoveOrStop(false);
                    curState = eState.Run;
                    startRunTime = Time.time;
                }
                break;
            case eState.Run:
                if (Time.time - startRunTime >= 2)
                {
                    curState = eState.Over;
                    BattleManager.Instance.mPlayer.SetPlayerMoveOrStop(true);
                }
                break;
        }
    }
}
