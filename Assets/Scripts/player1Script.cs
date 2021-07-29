using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1Script : MonoBehaviour
{
    public GameObject Client;
    public GameObject Server;
    public GameObject stickMode = null;
    public GameObject viewMode = null;
    public GameObject _Player = null;
    public GameObject nameTag = null;
    public int _switch = 0;
    public int _switch2 = 0;
    public float walkSpeed = 30;
    private int attackState = 0;
    public int handState = 0;

    public float actionCD = 0;
    public float fireCD = 0;
    public bool hit = false;//是否击中
    public GameObject hitObj = null;//攻击的对象

    private Animator _animator;
    //private float fighting = 1;

    private float jump = 0;
    private float jumpTimer = 0;
    //private float attack1Timer = 0;
    //private float attack2Timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        attackState = 0;
        jumpTimer = 0;
    }

    public void turnSwitch(int i)
    {
        _switch = i;
        if (_switch2 == 1)
        {
            //Server.GetComponent<Server>().changeInfo("0" + Server.GetComponent<Server>);
        }

        if (_switch2 == 2)//向服务端发送实时信息
        {
            //Client.GetComponent<Client>().Send("1" + _Player.transform.position + Ia + "," + handState + "," + Ix + "," + Iz + "," + jump + "," + jumpTimer);
        }
    }

    public void attatchPlayer(int x)
    {
        _Player = GameObject.Find("player" + x);
        nameTag = GameObject.Find("NameTag" + x);
        _animator = _Player.GetComponent<Animator>();
        nameTag.SetActive(false);
        _switch2 = x;
    }

    public void beingHurt()
    {
        changeFightState(-2);
    }


    public void changeFightState(int way)
    {
        if (attackState != -3) {//不僵直
            if (way == 1)//3段普攻
            {

                if (attackState == 0 || attackState == -2)
                {
                    if (actionCD <= 0.15f)
                    {
                        attackState = 1;
                        handState = attackState;
                        _animator.SetInteger("Hand", 1);
                        actionCD = 0.767f;
                        
                    }
                }
                else if (attackState == 1)
                {
                    if (actionCD <= 0.15f)
                    {
                        attackState = 2;
                        handState = attackState;
                        _animator.SetInteger("Hand", 2);
                        actionCD = 0.767f;
                    }
                }
                else if (attackState == 2)
                {
                    if (actionCD <= 0.05f)
                    {
                        attackState = 3;
                        handState = attackState;
                        _animator.SetInteger("Hand", 3);
                        actionCD = 1.6f;
                    }
                }
                else if (attackState == 3)
                {
                    if (actionCD <= 0.05f)
                    {
                        attackState = 1;
                        handState = attackState;
                        _animator.SetInteger("Hand", 1);
                        actionCD = 0.767f;
                    }
                }
                else if (attackState == -1)//反击
                {
                    if (actionCD <= 0.5f)
                    {
                        attackState = -2;
                        handState = attackState;
                        _animator.SetInteger("Hand", -2);
                        actionCD = 0.467f;
                    }
                }
            }
            else if (way == 2)//特殊攻击
            {
                if (actionCD <= 0 && fireCD <= 0)
                {
                    attackState = 4;
                    handState = attackState;
                    _animator.SetInteger("Hand", 4);
                    actionCD = 0.667f;
                    fireCD = 2f;
                }
            }
            else if (way == -1)//格挡
            {
                attackState = -1;
                if (actionCD <= 0)
                {
                    attackState = -1;
                    handState = attackState;
                    _animator.SetInteger("Hand", -1);
                    actionCD = 1.2f;
                }
            }
        }
        if(way == -2)//受击
        {
            if(attackState != -1 && attackState != 3 )
            {
                attackState = -3;
                handState = attackState;
                _animator.SetInteger("Hand", -3);
                actionCD = 0.25f;
            }
        }

        //if (!(_animator.GetCurrentAnimatorStateInfo(0).IsName("attack1") || _animator.GetCurrentAnimatorStateInfo(0).IsName("attack2")))
        //{
        //    if(way == 1)
        //    {
        //        _animator.SetInteger("Hand", 1);
        //    }
        //    else if(way == 2)
        //    {
        //        _animator.SetInteger("Hand", -1);
        //    }
        //}
    }


    public void Jump()
    {
        if (_Player.gameObject.GetComponent<Rigidbody>().velocity.y <= 0.1f && _Player.gameObject.GetComponent<Rigidbody>().velocity.y >= -0.1f && jumpTimer < 0.01f)
        {
            _Player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 36;
            jumpTimer = 1f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        float dx = 0;
        float dz = 0;
        float Ix = 0;
        float Iz = 0;
        float Ia = 0;
        float angle = 0;
        if (_switch == 1){

            Ia = viewMode.gameObject.GetComponent<viewControl>().input2.y;
            if (actionCD >= 0)
            {
                Ix = 0;
                Iz = 0;
                if(attackState == 1 || attackState == 2)//二段击
                {
                    if(actionCD < 0.6f && actionCD > 0.5f)
                    {
                        Iz = 2f;
                    }
                }
                else if(attackState == 3)//霸体击
                {
                    if (actionCD < 1.2f && actionCD > 1.1f)
                    {
                        Iz = 1f;
                    }
                    else if(actionCD < 0.9f && actionCD > 0.8f)
                    {
                        Iz = 1f;
                    }
                    else if (actionCD < 0.6f && actionCD > 0.5f)
                    {
                        Iz = 1f;
                    }
                    else if (actionCD < 0.35f && actionCD > 0.25f)
                    {
                        Iz = 2f;
                    }
                }
                else if (attackState == 4)//特击
                {
                    if (actionCD < 0.4f && actionCD > 0.3f)
                    {
                        Iz = -2f;
                    }
                }
                else if (attackState == -2)//反击
                {
                    if (actionCD < 0.4f && actionCD > 0.3f)
                    {
                        Iz = 2f;
                    }
                }
                else if (attackState == -3)//僵直
                {
                    if (actionCD < 0.2f && actionCD > 0.1f)
                    {
                        Iz = -1f;
                    }
                }
            }
            else
            {
                Ix = stickMode.gameObject.GetComponent<MyjoyStick>().input.x;
                Iz = stickMode.gameObject.GetComponent<MyjoyStick>().input.y;
            }
            actionCD -= Time.deltaTime;
            fireCD -= Time.deltaTime;
            if (actionCD <= -0.1 && actionCD >= -0.5f)
            {
                //fighting = 1;
                handState = 0;
                _animator.SetInteger("Hand", 0);
            }
            else if (actionCD < -0.5f)
            {
                attackState = 0;
            }

            
            angle = -1 * Mathf.PI * viewMode.gameObject.GetComponent<viewControl>().input2.y / 180;

            if (Ix != 0 || Iz != 0)
            {
                dx = Mathf.Cos(angle) * Ix - Mathf.Sin(angle) * Iz;
                dz = Mathf.Sin(angle) * Ix + Mathf.Cos(angle) * Iz;

                //state = 1;
            }
            else
            {
                //state = 0;
            }

            float dy = _Player.gameObject.GetComponent<Rigidbody>().velocity.y;//y方向速度不变
            Vector3 moveDirection = new Vector3(0, dy, 0); ;
            moveDirection = new Vector3(walkSpeed * dx, dy, walkSpeed * dz);
            //设定速度
            _Player.gameObject.GetComponent<Rigidbody>().velocity = moveDirection;
            //转向
            _Player.transform.localRotation = Quaternion.Euler(0, Ia, 0);

            AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
            //跳跃动画
            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
                _animator.SetFloat("JumpTimer", jumpTimer);
                if (jumpTimer > 0.66f)
                {
                    jump = 3 - 3 * jumpTimer;
                    _animator.SetFloat("Jump", jump);
                }
                else if (jumpTimer <= 0.66f && jumpTimer > 0.33f)
                {
                    jump = 1;
                    _animator.SetFloat("Jump", jump);
                }
                else if (jumpTimer <= 0.33f)
                {
                    jump = 3 * jumpTimer;
                    _animator.SetFloat("Jump", jump);
                }
            }
            ////end attack
            //if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && (_animator.GetCurrentAnimatorStateInfo(0).IsName("attack1") || _animator.GetCurrentAnimatorStateInfo(0).IsName("attack2")))
            //{
            //    _animator.SetInteger("Hand", 0);
            //}


            //设置动画状态
            //_animator.SetInteger("State", state);

            _animator.SetFloat("Ix", Ix);
            _animator.SetFloat("Iz", Iz);

            if(_switch2 == 1)
            {
                Server.GetComponent<Server>().changeInfo("1" + _Player.transform.position + Ia + "," + handState + "," + Ix + "," + Iz + "," + jump + "," + jumpTimer );
            }

            if(_switch2 == 2)//向服务端发送实时信息
            {
                Client.GetComponent<Client>().Send("1" + _Player.transform.position + Ia + "," + handState + "," + Ix + "," + Iz + "," + jump + "," + jumpTimer);
            }

        }
    }
        
}
