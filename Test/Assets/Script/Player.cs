using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dest.Math;

public class Player : MonoBehaviour {


    public Vector3 offsetPos;

    public float moveSpeed = 1;

    public float horizontalSpeed = 1;

    public float limitLenth=3;

    public float jumpForce=10;

    public float gravity = -9.8f;

 

    private float horizontalPos;

    private float curFallSpeed;

   

    private bool isJump;

    private bool isCanMove=true;



	void Update ()
    {

        if (!isCanMove)
        {
            return;
        }

        horizontalPos += Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;
        horizontalPos = Mathf.Clamp(horizontalPos, -limitLenth, limitLenth);

        PositionTangent data;
        BattleManager.Instance.curProgress += Time.deltaTime* moveSpeed;

        BattleManager.Instance.mWay.EvalPositionTangentParametrized(BattleManager.Instance.curProgress, out data);
        Vector3 mWayPos = BattleManager.Instance.mWay.transform.TransformPoint(data.Position);
        Vector3 mWayDir = data.Tangent;
        gameObject.transform.forward = mWayDir;


        Vector3 movePos = mWayPos + gameObject.transform.rotation * new Vector3(horizontalPos, 0, 0);
        Debug.Log(movePos);
        gameObject.transform.position = new Vector3(movePos.x, gameObject.transform.position.y, movePos.z);



        BattleManager.Instance.mCam.transform.forward = mWayDir;
        BattleManager.Instance.mCam.transform.position = mWayPos + BattleManager.Instance.mCam.transform.rotation* offsetPos;


        if (!isJump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                OnJump();
            }
        }
        else
        {
            OnFall();
        }


        gameObject.transform.position += new Vector3(0, curFallSpeed*Time.deltaTime, 0);

    }

    void OnJump()
    {
        isJump = true;
        curFallSpeed = jumpForce;
    }


    void OnFall()
    {
        curFallSpeed += gravity * Time.deltaTime;

        if (gameObject.transform.position.y <= 0)
        {
            curFallSpeed = 0;
            isJump = false;
        }
    }


    public void SetStopOrRun(bool rAcitve)
    {
        isCanMove = rAcitve;
    }

}
