using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject zhunxing;

    public AudioSource sound1;//人移动的声音
    public AudioSource sound2;//剑的声音
    public AudioSource sound3;//剑碰撞的声音
    public AudioSource sound4;//火焰

    public int totalHp = 200;
    private int Hp = 200;

    private string playerName;

    public int _switch = 0;
    public int _switch2 = 0;//绑定玩家
    public float walkSpeed = 30;
    private int actionState = 0;
    public int handState = 0;

    public float actionCD = 0;
    private float fireCD = 0;
    private float feibiaoHurtCD = 0;
    private float rollCD = 0;
    private int roll = 0;
    private float speedAdjust = 1;
    private float zhunxingTimer = 0;

    private int feibiaoNum = 0;
    private Vector3 feibiaoOrigin = new Vector3(0, 0, 0);
    private Vector3 feibiaoVelocity = new Vector3(0, 0, 0);

    private int spark1 = 0;
    private int spark2 = 0;
    private int spark3 = 0;
    private int spark3switch = 1;

    private bool hadhurt = true;//是否已造成伤害
    private bool hit = false;//是否击中
    private int TotalDam = 0;//总造成伤害
    //private int totalBeDam = 0;//总受伤
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
        Hp = totalHp;
    }

    public void turnSwitch(int i)
    {
        _switch = i;
    }

    public void attatchPlayer(int x, string name)
    {
        player1 = GameObject.Find("player" + x);
        player2 = GameObject.Find("player" + (3 - x));
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

    public void feibiaoHit(GameObject hitObj)
    {
        print(feibiaoHurtCD);
        if (feibiaoHurtCD < 0)
        {
            Vector2 p2face = new Vector2(Mathf.Sin(player2.transform.rotation.eulerAngles.y * Mathf.PI / 180), Mathf.Cos(player2.transform.rotation.eulerAngles.y * Mathf.PI / 180));
            Vector2 toward = new Vector2((player1.transform.position - player2.transform.position).x, (player1.transform.position - player2.transform.position).z).normalized;

            if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)
            {
                GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                sound3.clip = Resources.Load<AudioClip>("Musics/Hurt");
                sound3.Play();
                spark2++;
                Hurt(1);
            }
            else
            {
                print("duang");
                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                sound3.clip = Resources.Load<AudioClip>("Musics/duang" + (spark1 % 2 + 1));
                sound3.Play();
                spark1++;
            }
            feibiaoHurtCD = 0.3f;
        }
    }


    public void beHurt(int Dam)
    {
        Hp -= Dam; ;
        changeActionState(-2);
    }

    public void Hurt(int Dam)
    {
        zhunxingTimer = 0.5f;
        TotalDam += Dam;
    }

    public void changeActionState(int way)
    {
        if (actionState != -3)
        {//不僵直

            if (way == 1)//3段普攻
            {
                if (actionState == 0 || actionState == -2 || actionState == 6 || actionState == 8)
                {
                    if (actionCD <= 0.15f)
                    {
                        actionState = 1;
                        handState = actionState;
                        _animator.SetInteger("Hand", 1);
                        sound2.clip = Resources.Load<AudioClip>("Musics/swordWind");
                        sound2.Play();
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
                        sound2.clip = Resources.Load<AudioClip>("Musics/swordWind2");
                        sound2.Play();
                        GameObject c02 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight02"), player1.transform.position, player1.transform.rotation);
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
                        sound2.clip = Resources.Load<AudioClip>("Musics/swordWind3");
                        sound2.Play();
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
                        sound2.clip = Resources.Load<AudioClip>("Musics/swordWind");
                        sound2.Play();
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
                        sound2.clip = Resources.Load<AudioClip>("Musics/swordWind2");
                        sound2.Play();
                        GameObject c00 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight00"), player1.transform.position, player1.transform.rotation);
                        hadhurt = false;
                        actionCD = 0.467f;
                    }
                }
                else if (actionState == 5)//落地斩
                {
                    if (actionCD >= -0.1f && actionCD <= 0.5f)
                    {
                        actionState = 6;
                        handState = actionState;
                        _animator.SetInteger("Hand", 6);
                        sound2.clip = Resources.Load<AudioClip>("Musics/swordWind2");
                        sound2.Play();
                        GameObject c06 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight06"), player1.transform.position, player1.transform.rotation);
                        hadhurt = false;
                        actionCD = 0.567f;
                    }
                }
                else if (actionState == 7)//上撩击
                {
                    if (actionCD <= 0.4f)
                    {
                        actionState = 8;
                        handState = actionState;
                        _animator.SetInteger("Hand", 8);
                        sound2.clip = Resources.Load<AudioClip>("Musics/swordWind");
                        sound2.Play();
                        GameObject c08 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight08"), player1.transform.position, player1.transform.rotation);
                        hadhurt = false;
                        actionCD = 0.433f;
                    }
                }
            }
            else if (way == 2 && roll != 1)//特殊攻击
            {
                if (actionCD <= 0 && fireCD <= 0)
                {
                    actionState = 4;
                    handState = actionState;
                    _animator.SetInteger("Hand", 4);
                    sound4.clip = Resources.Load<AudioClip>("Musics/fire");
                    sound4.Play();
                    StartCoroutine(fireCountdown());
                    hadhurt = false;
                    actionCD = 0.667f;
                    fireCD = 2f;
                }
            }
            else if (way == -1 && roll != 1)//格挡
            {
                if (actionCD <= 0)
                {
                    actionState = -1;
                    handState = actionState;
                    _animator.SetInteger("Hand", -1);

                    actionCD = 0.733f;
                }
            }
            else if (way == 3 && roll != 1)//二段跳
            {
                if (actionCD <= 0)
                {
                    actionState = 5;
                    handState = actionState;
                    _animator.SetInteger("Hand", 5);
                    actionCD = 0.5f;
                }
            }
            else if (way == 4 && roll != 1)//手里剑
            {
                if (actionCD <= 0)
                {
                    actionState = 9;
                    handState = actionState;
                    _animator.SetInteger("Hand", 9);
                    spark3switch = 0;
                    StartCoroutine(feibiaoCountdown(-0.2f));
                    actionCD = 0.5f;
                }
                else if (actionState == 9 && actionCD < 0.4f)
                {
                    spark3switch++;
                    StartCoroutine(feibiaoCountdown(actionCD));
                    actionCD += 0.5f;
                }
            }
        }
        if (way == -2)//受击
        {
            if (actionState != 3)//除了霸体外，僵直
            {
                actionState = -3;
                handState = actionState;
                _animator.SetInteger("Hand", -3);
                actionCD = 0.1f;
            }
        }
    }

    IEnumerator feibiaoCountdown(float exTime)
    {
        int m = spark3switch % 2;
        for (float timer = 0.3f + exTime; timer >= 0; timer -= Time.deltaTime)
        {
            speedAdjust = 0;
            yield return 0;
        }
        sound2.clip = Resources.Load<AudioClip>("Musics/feibiao");
        sound2.Play();
        GameObject csp3 = Instantiate(Resources.Load<GameObject>("Prefabs/spark" + (3 + m)), player1.transform.position, player1.transform.rotation);
        Vector3 spPos = csp3.transform.GetChild(0).position;
        spark3++;
        for (float timer = 0.5f; timer >= 0; timer -= Time.deltaTime)
        {
            speedAdjust = 0;
            yield return 0;
        }
        speedAdjust = 3;
        //定义射线
        Ray m_ray;
        //保存碰撞信息
        RaycastHit m_hit;
        //创建一条从摄像机发出经过屏幕上的鼠标点的一条射线
        m_ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        //判断射线是否碰撞到物体

        GameObject cf1 = Instantiate(Resources.Load<GameObject>("Prefabs/feibiao"), spPos, Quaternion.Euler(0, player1.transform.rotation.eulerAngles.y + 90, 0));
        GameObject cf2 = Instantiate(Resources.Load<GameObject>("Prefabs/feibiao"), spPos, Quaternion.Euler(0, player1.transform.rotation.eulerAngles.y + 90, 0));
        GameObject cf3 = Instantiate(Resources.Load<GameObject>("Prefabs/feibiao"), spPos, Quaternion.Euler(0, player1.transform.rotation.eulerAngles.y + 90, 0));
        feibiaoNum++;
        feibiaoOrigin = spPos;
        if (Physics.Raycast(m_ray, out m_hit))
        {
            if (m == 1)
            {
                feibiaoVelocity = (m_hit.point - cf1.transform.position).normalized * 100 + new Vector3(0, 0.5f, 0);
            }
            else
            {
                feibiaoVelocity = (m_hit.point - cf1.transform.position).normalized * 100;
            }
        }
        else
        {
            feibiaoVelocity = new Vector3(Mathf.Sin(Camera.main.transform.rotation.eulerAngles.y * Mathf.PI / 180) * Mathf.Cos(Camera.main.transform.rotation.eulerAngles.z * Mathf.PI / 180),
            -Mathf.Sin(Camera.main.transform.rotation.eulerAngles.x * Mathf.PI / 180) * Mathf.Cos(Camera.main.transform.rotation.eulerAngles.z * Mathf.PI / 180),
            Mathf.Cos(Camera.main.transform.rotation.eulerAngles.x * Mathf.PI / 180) * Mathf.Cos(Camera.main.transform.rotation.eulerAngles.y * Mathf.PI / 180)).normalized * 100;
        }
        cf1.GetComponent<Rigidbody>().velocity = feibiaoVelocity;
        cf2.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Cos(Mathf.PI / 36) * feibiaoVelocity.x - Mathf.Sin(Mathf.PI / 36) * feibiaoVelocity.z, feibiaoVelocity.y, Mathf.Sin(Mathf.PI / 36) * feibiaoVelocity.x + Mathf.Cos(Mathf.PI / 36) * feibiaoVelocity.z);
        cf3.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Cos(-Mathf.PI / 36) * feibiaoVelocity.x - Mathf.Sin(-Mathf.PI / 36) * feibiaoVelocity.z, feibiaoVelocity.y, Mathf.Sin(-Mathf.PI / 36) * feibiaoVelocity.x + Mathf.Cos(-Mathf.PI / 36) * feibiaoVelocity.z);
        for (float timer = 0.2f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        speedAdjust = 1;
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
            sound1.clip = Resources.Load<AudioClip>("Musics/jump");
            sound1.Play();
            CanSecondJump = true;
        }
        else if (jumpTimer > 0.4f && CanSecondJump)
        {
            player1.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 36;
            changeActionState(3);
            sound1.clip = Resources.Load<AudioClip>("Musics/jump");
            sound1.Play();
            CanSecondJump = false;
        }
    }

    public void Roll()
    {
        if (rollCD <= 0 && actionCD <= 0 && actionState == 0)
        {
            rollCD = 1.068f;
            speedAdjust = 2f;
            _animator.SetInteger("roll", 1);
            sound1.clip = Resources.Load<AudioClip>("Musics/jump");
            sound1.Play();
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
            if (timer < 0.75f && speedAdjust > 0.8f)
            {
                speedAdjust -= 3 * Time.deltaTime;
            }
            yield return 0;
        }
        _animator.SetInteger("roll", 0);
        roll = 0;
        speedAdjust = 1;
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

        if (_switch == 1)
        {

            actionCD -= Time.deltaTime;
            fireCD -= Time.deltaTime;
            rollCD -= Time.deltaTime;
            feibiaoHurtCD -= Time.deltaTime;
            
            Ia = viewMode.gameObject.GetComponent<viewControl>().input2.y;
            Vector2 p2face = new Vector2(Mathf.Sin(player2.transform.rotation.eulerAngles.y * Mathf.PI / 180), Mathf.Cos(player2.transform.rotation.eulerAngles.y * Mathf.PI / 180));
            Vector2 toward = new Vector2((player1.transform.position - player2.transform.position).x, (player1.transform.position - player2.transform.position).z).normalized;

            
            if(zhunxingTimer > 0)
            {
                zhunxingTimer -= Time.deltaTime;
                zhunxing.transform.localScale = new Vector3(1 + 2*zhunxingTimer, 1 + 2*zhunxingTimer, 1);
                zhunxing.transform.localRotation = Quaternion.Euler(0, 0, 45);
                //zhunxing.GetComponent<Image>().color = new Color(1,0,0);
            }
            else
            {
                zhunxing.transform.localScale = new Vector3(1, 1, 1);
                zhunxing.transform.localRotation = Quaternion.Euler(0, 0, 0);
                //zhunxing.GetComponent<Image>().color = new Color(1, 1, 1);
            }



            if (actionCD >= 0 && actionState != 5 && actionState != 7 && actionState != 9)//二段跳,扔飞镖和翻滚可以移动
            {
                Ix = 0;
                Iz = 0;
                if (actionState == 1 || actionState == 2)//二段击
                {
                    if (actionCD < 0.6f && actionCD > 0.5f)
                    {
                        Iz = 2f;
                    }
                    if (hit && !hadhurt)
                    {
                        if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)//无格挡，或背击
                        {
                            print("hurt");
                            GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            sound3.clip = Resources.Load<AudioClip>("Musics/Hurt");
                            sound3.Play();
                            spark2++;
                            Hurt(3);
                        }
                        else
                        {
                            print("duang");

                            changeActionState(-2);//被格挡，僵直
                            GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            sound3.clip = Resources.Load<AudioClip>("Musics/duang" + (spark1 % 2 + 1));
                            sound3.Play();
                            spark1++;
                        }
                        hadhurt = true;
                        hit = false;
                    }
                }
                else if (actionState == 3)//霸体击
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
                                sound3.clip = Resources.Load<AudioClip>("Musics/Hurt");
                                sound3.Play();
                                spark2++;
                                Hurt(1);
                            }
                            else
                            {
                                print("duang");
                                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                sound3.clip = Resources.Load<AudioClip>("Musics/duang" + (spark1 % 2 + 1));
                                sound3.Play();
                                spark1++;
                            }
                            hadhurt = true;
                        }
                    }
                    else if (actionCD < 1.1f && actionCD > 0.9f)
                    {
                        hadhurt = false;
                    }
                    else if (actionCD < 0.9f && actionCD > 0.8f)
                    {
                        Iz = 1f;
                        if (hit && !hadhurt)
                        {
                            if (this.GetComponent<deCodeScript>().player2State != -1 || Vector2.Dot(p2face, toward) < 0)
                            {
                                print("hurt");
                                GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                sound3.clip = Resources.Load<AudioClip>("Musics/Hurt");
                                sound3.Play();
                                spark2++;
                                Hurt(1);
                            }
                            else
                            {
                                print("duang");
                                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                sound3.clip = Resources.Load<AudioClip>("Musics/duang" + (spark1 % 2 + 1));
                                sound3.Play();
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
                                sound3.clip = Resources.Load<AudioClip>("Musics/Hurt");
                                sound3.Play();
                                spark2++;
                                Hurt(1);
                            }
                            else
                            {
                                print("duang");
                                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                sound3.clip = Resources.Load<AudioClip>("Musics/duang" + (spark1 % 2 + 1));
                                sound3.Play();
                                spark1++;
                            }
                            hadhurt = true;
                        }
                    }
                    else if (actionCD < 0.5f && actionCD > 0.35f)
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
                                sound3.clip = Resources.Load<AudioClip>("Musics/Hurt");
                                sound3.Play();
                                spark2++;
                                Hurt(3);
                            }
                            else
                            {
                                print("duang");
                                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                                sound3.clip = Resources.Load<AudioClip>("Musics/duang" + (spark1 % 2 + 1));
                                sound3.Play();
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
                            sound3.clip = Resources.Load<AudioClip>("Musics/Hurt");
                            sound3.Play();
                            spark2++;
                            Hurt(3);
                        }
                        else
                        {
                            changeActionState(-2);//被格挡，僵直
                            print("duang");
                            GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            sound3.clip = Resources.Load<AudioClip>("Musics/duang" + (spark1 % 2 + 1));
                            sound3.Play();
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
                            sound3.clip = Resources.Load<AudioClip>("Musics/Hurt");
                            sound3.Play();
                            spark2++;
                            Hurt(6);
                        }
                        else
                        {
                            changeActionState(-2);//被格挡，僵直
                            print("duang");
                            GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            sound3.clip = Resources.Load<AudioClip>("Musics/duang" + (spark1 % 2 + 1));
                            sound3.Play();
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
                            sound3.clip = Resources.Load<AudioClip>("Musics/Hurt");
                            sound3.Play();
                            spark2++;
                            Hurt(2);
                        }
                        else
                        {
                            changeActionState(-2);//被格挡，僵直
                            print("duang");
                            GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), player2.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                            sound3.clip = Resources.Load<AudioClip>("Musics/duang" + (spark1 % 2 + 1));
                            sound3.Play();
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
                        Hurt(2);
                        hadhurt = true;
                        hit = false;
                    }
                }
                else if (actionState == -3)//僵直
                {
                    if (actionCD < 0.1f && actionCD > 0)
                    {
                        Ix = 1.5f * (toward.x * Mathf.Cos(angle) + toward.y * Mathf.Sin(angle));
                        Iz = 1.5f * (-toward.x * Mathf.Sin(angle) + toward.y * Mathf.Cos(angle));
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




            playerBlood.GetComponent<RectTransform>().sizeDelta = new Vector2(400 * (float)Hp / totalHp, 20);
            playerBlood.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200 * (float)(totalHp - Hp) / totalHp, 10);


            if (Ix != 0 || Iz != 0)
            {
                dx = Mathf.Cos(angle) * Ix - Mathf.Sin(angle) * Iz;
                dz = Mathf.Sin(angle) * Ix + Mathf.Cos(angle) * Iz;

            }
            float dy = player1.gameObject.GetComponent<Rigidbody>().velocity.y;//y方向速度不变
            Vector3 moveDirection = new Vector3(0, dy, 0); ;
            moveDirection = new Vector3(speedAdjust * walkSpeed * dx, dy, speedAdjust * walkSpeed * dz);
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

            if (_switch2 == 1)
            {
                Server.GetComponent<Server>().changeInfo("1_" + playerName + "_" + player1.transform.position.x + "_" + player1.transform.position.y + "_" + player1.transform.position.z
                    + "_" + Ia + "_" + handState + "_" + Ix + "_" + Iz + "_" + jump + "_" + jumpTimer + "_" + roll + "_" + TotalDam + "_" + Hp + "_" + spark1 + "_" + spark2 + "_"
                    + spark3 + "_" + spark3switch + "_" + feibiaoNum + "_" + feibiaoOrigin.x + "_" + feibiaoOrigin.y + "_" + feibiaoOrigin.z + "_" + feibiaoVelocity.x + "_" + feibiaoVelocity.y
                    + "_" + feibiaoVelocity.z + "_");
            }

            if (_switch2 == 2)//向服务端发送实时信息
            {
                Client.GetComponent<Client>().Send("1_" + playerName + "_" + player1.transform.position.x + "_" + player1.transform.position.y + "_" + player1.transform.position.z
                    + "_" + Ia + "_" + handState + "_" + Ix + "_" + Iz + "_" + jump + "_" + jumpTimer + "_" + roll + "_" + TotalDam + "_" + Hp + "_" + spark1 + "_" + spark2 + "_"
                    + spark3 + "_" + spark3switch + "_" + feibiaoNum + "_" + feibiaoOrigin.x + "_" + feibiaoOrigin.y + "_" + feibiaoOrigin.z + "_" + feibiaoVelocity.x + "_" + feibiaoVelocity.y
                    + "_" + feibiaoVelocity.z + "_");
            }
        }
    }
}
