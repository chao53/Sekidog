using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1Script : MonoBehaviour
{
    public GameObject EventSystem;
    public GameObject Client;
    public GameObject Server;
    public GameObject stickMode = null;
    public GameObject viewMode = null;
    public GameObject player1 = null;
    public GameObject player2 = null;
    public GameObject nameTag = null;
    public GameObject playerBlood = null;
    public GameObject enemyRedBlood = null;
    public int totalHp = 200;

    private string playerName;
    
    public int _switch = 0;
    public int _switch2 = 0;
    public float walkSpeed = 30;
    private int actionState = 0;
    public int handState = 0;

    public float actionCD = 0;
    private float fireCD = 0;
    private float rollCD = 0;
    private int roll = 0;
    private float rolling = 1;

    private int spark1 = 0;
    private int spark2 = 0;

    private bool hadhurt = true;//是否已造成伤害
    private bool hit = false;//是否击中
    private int TotalDam = 0;//总造成伤害
    private int totalBeDam = 0;//总受伤
    private GameObject hitObj = null;//攻击的对象

    public Animator _animator;
    //private float fighting = 1;

    private bool CanSecondJump = true;
    private float jump = 0;
    private float jumpTimer = 0;
    //private float attack1Timer = 0;
    //private float attack2Timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
        actionState = 0;
        jumpTimer = 0;
    }

    public void turnSwitch(int i)
    {
        _switch = i;
    }

    public void attatchPlayer(int x,string name)
    {
        player1 = GameObject.Find("player" + x);
        player2 = GameObject.Find("player" + (3 - x));
        enemyRedBlood = GameObject.Find("redBlood" + (3 - x));
        _animator = player1.GetComponent<Animator>();
        GameObject.Find("blackBlood" + x).SetActive(false);
        GameObject.Find("NameTag" + x).SetActive(false);
        _switch2 = x;
        playerName = name;
    }

    public void swordHit(GameObject hitObj)
    {
        hit = true;
        this.hitObj = hitObj;
    }

    //public void swordLeave()
    //{
    //    hit = false;
    //}


    public void beHurt(int totalBeDam)
    {
        this.totalBeDam = totalBeDam;
        changeActionState(-2);
    }


    public void changeActionState(int way)
    {
        if (actionState != -3) {//不僵直
            
            if (way == 1)//3段普攻
            {               
                if (actionState == 0 || actionState == -2 || actionState == 6 || actionState == 8)
                {
                    if (actionCD <= 0.15f)
                    {
                        actionState = 1;
                        handState = actionState;
                        _animator.SetInteger("Hand", 1);
                        GameObject c01 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight01"), player1.transform.position, player1.transform.rotation);
                        hadhurt = false;
                        actionCD = 0.767f;
                        
                    }
                }
                else if (actionState == 1)
                {
                    if (actionCD <= 0.15f)
                    {
                        actionState = 2;
                        handState = actionState;
                        _animator.SetInteger("Hand", 2);
                        GameObject c02 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight02"), player1.transform.position,player1.transform.rotation);
                        hadhurt = false;
                        actionCD = 0.767f;
                    }
                }
                else if (actionState == 2)
                {
                    if (actionCD <= 0.05f)
                    {
                        actionState = 3;
                        handState = actionState;
                        _animator.SetInteger("Hand", 3);
                        StartCoroutine(swordCountdown());                       
                        hadhurt = false;
                        actionCD = 1.6f;
                    }
                }
                else if (actionState == 3)
                {
                    if (actionCD <= 0.05f)
                    {
                        actionState = 1;
                        handState = actionState;
                        _animator.SetInteger("Hand", 1);                        
                        GameObject c01 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight01"), player1.transform.position, player1.transform.rotation);
                        hadhurt = false;
                        actionCD = 0.767f;
                    }
                }
                else if (actionState == -1)//反击
                {
                    if (actionCD <= 0.5f)
                    {
                        actionState = -2;
                        handState = actionState;
                        _animator.SetInteger("Hand", -2);
                        GameObject c00 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight00"), player1.transform.position, player1.transform.rotation);
                        hadhurt = false;
                        actionCD = 0.467f;
                    }
                }
                else if(actionState == 5)//落地斩
                {
                    if (actionCD >= -0.1f &&actionCD <= 0.5f)
                    {
                        print(actionCD);
                        actionState = 6;
                        handState = actionState;
                        _animator.SetInteger("Hand", 6);
                        GameObject c06 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight06"), player1.transform.position, player1.transform.rotation);
                        hadhurt = false;
                        actionCD = 0.567f;
                    }
                }
                else if(actionState == 7)//上撩击
                {
                    if (actionCD <= 0.4f)
                    {
                        actionState = 8;
                        handState = actionState;
                        _animator.SetInteger("Hand", 8);
                        GameObject c08 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight08"), player1.transform.position, player1.transform.rotation);
                        hadhurt = false;
                        actionCD = 0.433f;
                    }
                }
            }
            else if (way == 2 && rolling == 1)//特殊攻击
            {
                if (actionCD <= 0 && fireCD <= 0)
                {
                    actionState = 4;
                    handState = actionState;
                    _animator.SetInteger("Hand", 4);
                    StartCoroutine(fireCountdown());
                    hadhurt = false;
                    actionCD = 0.667f;
                    fireCD = 2f;
                }
            }
            else if (way == -1 && rolling == 1)//格挡
            {               
                if (actionCD <= 0)
                {
                    actionState = -1;
                    handState = actionState;
                    _animator.SetInteger("Hand", -1);

                    actionCD = 0.733f;
                }
            }
            else if(way == 3 && rolling == 1)//二段跳
            {
                if(actionCD <= 0)
                {
                    actionState = 5;
                    handState = actionState;
                    _animator.SetInteger("Hand", 5);
                    actionCD = 0.5f;
                }
            }
        } 
        if(way == -2)//受击
        {
            if(actionState != 3)//除了霸体外，僵直
            {
                actionState = -3;
                handState = actionState;
                _animator.SetInteger("Hand", -3);
                actionCD = 0.1f;
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

    IEnumerator swordCountdown()
    {
        for (float timer = 0.2f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        GameObject c031 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight031"), player1.transform.position, player1.transform.rotation);
        for (float timer = 0.4f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        GameObject c032 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight032"), player1.transform.position, player1.transform.rotation);
        for (float timer = 0.2f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        GameObject c033 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight033"), player1.transform.position, player1.transform.rotation);
        for (float timer = 0.3f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        GameObject c034 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight034"), player1.transform.position, player1.transform.rotation);
    }

    IEnumerator fireCountdown()
    {
        for (float timer = 0.4f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }        
        GameObject c1 = Instantiate(Resources.Load<GameObject>("Prefabs/fire"), player1.transform.position +
            new Vector3(Mathf.Sin(player1.transform.rotation.eulerAngles.y * Mathf.PI / 180) * 5, 7, Mathf.Cos(player1.transform.rotation.eulerAngles.y * Mathf.PI / 180) * 5), player1.transform.rotation);
    }

    public void Jump()
    {
        if (player1.gameObject.GetComponent<Rigidbody>().velocity.y <= 0.1f && player1.gameObject.GetComponent<Rigidbody>().velocity.y >= -0.1f && jumpTimer < 0.01f && actionCD <= 0)
        {
            player1.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 36;
            jumpTimer = 1f;
            CanSecondJump = true;
        }
        else if(jumpTimer > 0.4f && CanSecondJump)
        {
            player1.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 36;
            changeActionState(3);
            CanSecondJump = false;
        }
    }

    public void Roll()
    {
        if (rollCD <= 0 && actionCD <= 0 && actionState == 0)
        {
            rollCD = 2;
            rolling = 2f;
            _animator.SetInteger("roll", 1);
            roll = 1;
            actionCD = 1f;
            actionState = 7;
            StartCoroutine(rollCountdown());
        }
    }
    IEnumerator rollCountdown()
    {
        for (float timer = 1.067f; timer >= 0; timer -= Time.deltaTime)
        {
            if(timer <0.75f && rolling > 0.8f)
            {
                rolling -= 3 * Time.deltaTime;
            }       
            yield return 0;
        }
        _animator.SetInteger("roll", 0);
        roll = 0;
        rolling = 1;
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
        angle = -1 * Mathf.PI * viewMode.gameObject.GetComponent<viewControl>().input2.y / 180;
       
        if (_switch == 1){

            actionCD -= Time.deltaTime;
            fireCD -= Time.deltaTime;
            rollCD -= Time.deltaTime;
            Ia = viewMode.gameObject.GetComponent<viewControl>().input2.y;

            Vector2 p2face = new Vector2(Mathf.Sin(player2.transform.rotation.eulerAngles.y * Mathf.PI / 180), Mathf.Cos(player2.transform.rotation.eulerAngles.y * Mathf.PI / 180));
            Vector2 toward = new Vector2((player1.transform.position - player2.transform.position).x, (player1.transform.position - player2.transform.position).z).normalized;

            //print(Vector2.Dot(p2face, toward));
            if (actionCD >= 0 && actionState != 5 && actionState != 7)//二段跳和翻滚可以移动
            {
                Ix = 0;
                Iz = 0;
                if(actionState == 1 || actionState == 2)//二段击
                {
                    if(actionCD < 0.6f && actionCD > 0.5f)
                    {
                        Iz = 2f;
                    }
                    if(hit && !hadhurt)
                    {
                        if(this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)//无格挡，或背击
                        {
                            print("hurt");
                            GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0,6,0), Quaternion.Euler(0, 0, 0));
                            spark2++;
                            TotalDam += 3;
                        }
                        else
                        {
                            print("duang");
                            changeActionState(-2);//被格挡，僵直
                            GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0,0,0));
                            spark1++;                      
                        }
                        hadhurt = true;
                        hit = false;
                    }
                }
                else if(actionState == 3)//霸体击
                {
                    if (actionCD < 1.2f && actionCD > 1.1f)
                    {
                        Iz = 1f;
                        if (hit && !hadhurt)
                        {
                            if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)
                            {
                                print("hurt");
                                GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                spark2++;
                                TotalDam += 1;
                            }
                            else
                            {
                                print("duang");
                                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                spark1++;
                            }
                            hadhurt = true;
                        }
                    }
                    else if(actionCD < 1.1f && actionCD > 0.9f)
                    {
                        hadhurt = false;
                    }
                    else if(actionCD < 0.9f && actionCD > 0.8f)
                    { 
                        Iz = 1f;
                        if (hit && !hadhurt)
                        {
                            if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)
                            {
                                print("hurt");
                                GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                spark2++;
                                TotalDam += 1;
                            }
                            else
                            {
                                print("duang");
                                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                spark1++;
                            }
                            hadhurt = true;
                        }
                    }
                    else if (actionCD < 0.8f && actionCD > 0.6f)
                    {
                        hadhurt = false;
                    }
                    else if (actionCD < 0.6f && actionCD > 0.5f)
                    {
                        Iz = 1f;
                        if (hit && !hadhurt)
                        {
                            if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)
                            {
                                print("hurt");
                                GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                spark2++;
                                TotalDam += 1;
                            }
                            else
                            {
                                print("duang");
                                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                spark1++;
                            }
                            hadhurt = true;
                        }
                    }
                    else if(actionCD < 0.5f && actionCD > 0.35f)
                    {
                        hadhurt = false;
                    }
                    else if (actionCD < 0.35f && actionCD > 0.25f)
                    {
                        Iz = 2f;
                        if (hit && !hadhurt)
                        {
                            if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)
                            {
                                print("hurt");
                                GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                spark2++;
                                TotalDam += 3;
                            }
                            else
                            {
                                print("duang");
                                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                spark1++;
                            }
                            hadhurt = true;
                            hit = false;
                        }
                    }
                }
                else if (actionState == -2)//反击
                {
                    if (actionCD < 0.4f && actionCD > 0.3f)
                    {
                        Iz = 2f;
                    }
                    if (hit && !hadhurt)
                    {
                        if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)
                        {
                            print("hurt");
                            GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            spark2++;
                            TotalDam += 3;
                        }
                        else
                        {
                            changeActionState(-2);//被格挡，僵直
                            print("duang");
                            GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            spark1++;
                        }
                        hadhurt = true;
                        hit = false;
                        print(hit);
                    }
                }
                else if (actionState == 6)//落地斩
                {
                    if (actionCD < 0.5f && actionCD > 0.3f)
                    {
                        Iz = 2f; 
                    }
                    if (hit && !hadhurt)
                    {
                        if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)
                        {
                            print("hurt");
                            GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            spark2++;
                            TotalDam += 5;
                        }
                        else
                        {
                            changeActionState(-2);//被格挡，僵直
                            print("duang");
                            GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            spark1++;
                        }
                        hadhurt = true;
                        hit = false;
                    }
                }
                else if (actionState == 8)//上撩击
                {
                    if (actionCD < 0.3f && actionCD > 0.2f)
                    {
                        Iz = 2f;
                    }
                    if (hit && !hadhurt)
                    {
                        if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)
                        {
                            print("hurt");
                            GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            spark2++;
                            TotalDam += 2;
                        }
                        else
                        {
                            changeActionState(-2);//被格挡，僵直
                            print("duang");
                            GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            spark1++;
                        }
                        hadhurt = true;
                        hit = false;
                        print(hit);
                    }
                }
                else if (actionState == 4)//特击
                {
                    if (actionCD < 0.4f && actionCD > 0.3f)
                    {
                        Iz = -2f;
                        
                    }
                    if (hit && !hadhurt)
                    {
                        print("hurt");

                        TotalDam += 3;
                        hadhurt = true;
                        hit = false;
                    }
                }
                else if (actionState == -3)//僵直
                {
                    if (actionCD < 0.1f && actionCD > 0)
                    {                       
                        Ix = 1.5f*(toward.x * Mathf.Cos(angle) + toward.y * Mathf.Sin(angle));
                        Iz = 1.5f*(-toward.x * Mathf.Sin(angle) + toward.y * Mathf.Cos(angle));
                        //print(toward.x + " " + toward.y + " " + Ix + " " + Iz + " " + angle);
                    }
                }
            }
            else
            {
                Ix = stickMode.gameObject.GetComponent<MyjoyStick>().input.x;
                Iz = stickMode.gameObject.GetComponent<MyjoyStick>().input.y;
            }

            if (actionCD <= -0.1f && actionCD >= -0.5f)
            {
                handState = 0;
                _animator.SetInteger("Hand", 0);
            }
            else if (actionCD < -0.5f)
            {
                hit = false;
                actionState = 0;
            }

            //print(actionState + " " + actionCD);


            enemyRedBlood.transform.localScale = new Vector3((float)(totalHp - TotalDam) / totalHp, 1, 1);
            enemyRedBlood.transform.localPosition = new Vector3(-0.5f*TotalDam/ totalHp, 0, -0.001f);


            playerBlood.GetComponent<RectTransform>().sizeDelta = new Vector2(400* (float)(totalHp - totalBeDam) / totalHp, 20);
            playerBlood.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200 * (float)totalBeDam/ totalHp, 10);
            //print(totalBeDam);
            

            if (Ix != 0 || Iz != 0)
            {
                dx = Mathf.Cos(angle) * Ix - Mathf.Sin(angle) * Iz;
                dz = Mathf.Sin(angle) * Ix + Mathf.Cos(angle) * Iz;

            }
            float dy = player1.gameObject.GetComponent<Rigidbody>().velocity.y;//y方向速度不变
            Vector3 moveDirection = new Vector3(0, dy, 0); ;
            moveDirection = new Vector3(rolling*walkSpeed * dx, dy, rolling*walkSpeed * dz);
            //设定速度
            player1.gameObject.GetComponent<Rigidbody>().velocity = moveDirection;
            //转向
            player1.transform.localRotation = Quaternion.Euler(0, Ia, 0);

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
                Server.GetComponent<Server>().changeInfo("1_"+ playerName + "_" + player1.transform.position.x + "_" + player1.transform.position.y + "_" + player1.transform.position.z 
                    + "_" + Ia + "_" + handState + "_" + Ix + "_" + Iz + "_" + jump + "_" + jumpTimer + "_" + roll + "_" + TotalDam + "_" + spark1 + "_" + spark2 + "_");
            }

            if(_switch2 == 2)//向服务端发送实时信息
            {
                Client.GetComponent<Client>().Send("1_" + playerName + "_" + player1.transform.position.x + "_" + player1.transform.position.y + "_" + player1.transform.position.z
                    + "_" + Ia + "_" + handState + "_" + Ix + "_" + Iz + "_" + jump + "_" + jumpTimer + "_" + roll + "_" + TotalDam + "_" + spark1 + "_" + spark2 + "_");
            }
        }
    }
}
