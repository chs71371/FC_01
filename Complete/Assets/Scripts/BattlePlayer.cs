using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dest.Math;

public class BattlePlayer : MonoBehaviour
{

    /// <summary>
    /// 向前移动速度
    /// </summary>
    public float moveSpeed = 20;
    /// <summary>
    /// 左右移动速度
    /// </summary>
    public float horizontalSpeed = 5;
    /// <summary>
    /// 跳跃的力
    /// </summary>
    public float jumpForce = 4;
    /// <summary>
    /// 下落的力
    /// </summary>
    public float fallForce = -9.8f;
    /// <summary>
    /// 当前水平位置
    /// </summary>
    public float horizontalPos = 0;

    [SerializeField]
    private float jumpSpeed = 0;

    public bool isJump = false;

    public enum eState
    {
        Move,
        Stop,
    }

    public eState curState = eState.Move;

    /// <summary>
    /// 当前路程 （米）
    /// </summary>
    public float curProgress;
    /// <summary>
    /// 当前路径位置
    /// </summary>
    public Vector3 mWayPos;
    /// <summary>
    /// 当前路径方向
    /// </summary>
    public Vector3 mWayDir;


    void Start()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.transform.eulerAngles = Vector3.zero;
    }


    public void Jump()
    {
        if (isJump)
        {
            return;
        }
        jumpSpeed = jumpForce;
        isJump = true;
    }

    public void SetPlayerMoveOrStop(bool isActive)
    {
        if (isActive)
        {
            curState = eState.Move;
        }
        else
        {
            curState = eState.Stop;
        }

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


        if (curState == eState.Move)
        {
            curProgress += moveSpeed * Time.deltaTime;

            PositionTangent data;
            BattleManager.Instance.curSpline.EvalPositionTangentParametrized(curProgress, out data);
            mWayPos = BattleManager.Instance.curSpline.transform.TransformPoint(data.Position);
            mWayDir = data.Tangent;
            gameObject.transform.forward = mWayDir;


            horizontalPos += Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;
            horizontalPos = Mathf.Clamp(horizontalPos, -3f, 3f);

            Vector3 movePos = mWayPos + gameObject.transform.rotation * new Vector3(horizontalPos, 0, 0);
            gameObject.transform.position = new Vector3(movePos.x, gameObject.transform.position.y, movePos.z);
        }


        if (isJump)
        {
            jumpSpeed += fallForce * Time.deltaTime;

            gameObject.transform.position += new Vector3(0, jumpSpeed, 0) * Time.deltaTime;

            Vector3 curPos = gameObject.transform.position;

            if (curPos.y <= 0)
            {
                curPos.y = 0;
                gameObject.transform.position = curPos;
                isJump = false;
            }
        }
    }
}