using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
public class deCodeScript : MonoBehaviour
{
    public GameObject EventSystem;
    public GameObject player2;
    public GameObject nameTag;
    public int player2State = 0;
    //private GameObject c1;
    private Animator _animator;
    private int TotalDam = 0;
    private int spark1 = 0;
    private int spark2 = 0;
    private bool firing = false;
    private bool a0ing = false;
    private bool a1ing = false;
    private bool a2ing = false;
    private bool a3ing = false;
    private bool a4ing = false;
    private bool a5ing = false;
    // Start is called before the first frame update 
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
    }

    public void attatchPlayer(int x)
    {
        player2 = GameObject.Find("player" + x);
        nameTag = GameObject.Find("NameTag" + x);
        _animator = player2.GetComponent<Animator>();
    }

    public void decode(string str)
    {
        String[] vs = str.Split('_'); 
        //if(int.Parse(vs[0]) == 0)
        //{
        //    print(str);
        //    nameTag.GetComponent<TMP_Text>().text = vs[1];

        //}
        if(int.Parse(vs[0]) == 1)
        {
            nameTag.GetComponent<TMP_Text>().text = vs[1];
            player2.transform.position = new Vector3(float.Parse(vs[2]), float.Parse(vs[3]), float.Parse(vs[4]));
            player2.transform.localRotation = Quaternion.Euler(0, float.Parse(vs[5]), 0);
            _animator.SetInteger("Hand", int.Parse(vs[6]));
            if(vs[6] == "-1")
            {
                player2State = -1;
            }
            else
            {
                player2State = 0;
            }

            if(vs[6] == "4" && !firing)
            {
                StartCoroutine(fireCountdown());
                firing = true;
            }
            else if (vs[6] == "1" && !a1ing)
            {
                StartCoroutine(a1Countdown());
                a1ing = true;
            }
            else if (vs[6] == "2" && !a2ing)
            {
                StartCoroutine(a2Countdown());
                a2ing = true;
            }
            else if (vs[6] == "3" && !a3ing)
            {
                StartCoroutine(a3Countdown());
                a3ing = true;
            }
            else if(vs[6] == "-2" && !a0ing)
            {
                StartCoroutine(a0Countdown());
                a0ing = true;
            }
            else if (vs[6] == "6" && !a4ing)
            {
                StartCoroutine(a4Countdown());
                a4ing = true;
            }
            else if (vs[6] == "8" && !a5ing)
            {
                StartCoroutine(a5Countdown());
                a5ing = true;
            }
            _animator.SetFloat("Ix", float.Parse(vs[7]));
            _animator.SetFloat("Iz", float.Parse(vs[8]));
            _animator.SetFloat("Jump", float.Parse(vs[9]));
            _animator.SetFloat("JumpTimer", float.Parse(vs[10]));
            _animator.SetInteger("roll",int.Parse(vs[11]));
            if (int.Parse(vs[12]) > TotalDam)
            {
                EventSystem.GetComponent<player1Script>().beHurt(TotalDam);
                TotalDam = int.Parse(vs[12]);
            }

            if(int.Parse(vs[13]) > spark1)
            {
                GameObject csp1 = Instantiate(Resources.Load<GameObject>("Prefabs/spark"), EventSystem.GetComponent<player1Script>().player1.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                spark1 = int.Parse(vs[13]);
            }

            if(int.Parse(vs[14]) > spark2)
            {
                GameObject csp2 = Instantiate(Resources.Load<GameObject>("Prefabs/spark2"), EventSystem.GetComponent<player1Script>().player1.transform.position + new Vector3(0, 6, 0), Quaternion.Euler(0, 0, 0));
                spark2 = int.Parse(vs[14]);
            }
        }
    }

    IEnumerator a1Countdown()
    {
        GameObject c01 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight01"), player2.transform.position, player2.transform.rotation);
        for (float timer = 1f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        a1ing = false; 
    }

    IEnumerator a2Countdown()
    {
        GameObject c02 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight02"), player2.transform.position, player2.transform.rotation);
        for (float timer = 1f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        a2ing = false;
    }

    IEnumerator a3Countdown()
    {
        for (float timer = 0.2f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        GameObject c031 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight031"), player2.transform.position, player2.transform.rotation);
        for (float timer = 0.4f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        GameObject c032 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight032"), player2.transform.position, player2.transform.rotation);
        for (float timer = 0.2f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        GameObject c033 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight033"), player2.transform.position, player2.transform.rotation);
        for (float timer = 0.3f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        GameObject c034 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight034"), player2.transform.position, player2.transform.rotation);
        for (float timer = 1f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        a3ing = false;
    }
    IEnumerator a0Countdown()
    {
        GameObject c00 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight00"), player2.transform.position, player2.transform.rotation);
        for (float timer = 0.5f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        a0ing = false;
    }

    IEnumerator a4Countdown()
    {
        GameObject c06 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight06"), player2.transform.position, player2.transform.rotation);
        for (float timer = 1f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        a4ing = false;
    }

    IEnumerator a5Countdown()
    {
        GameObject c08 = Instantiate(Resources.Load<GameObject>("Prefabs/SLight08"), player2.transform.position, player2.transform.rotation);
        for (float timer = 1f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        a5ing = false;
    }

    IEnumerator fireCountdown()
    {
        for (float timer = 0.4f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }           
        GameObject c2 = Instantiate(Resources.Load<GameObject>("Prefabs/fire"), player2.transform.position +
            new Vector3(Mathf.Sin(player2.transform.rotation.eulerAngles.y * Mathf.PI / 180) * 5, 7, Mathf.Cos(player2.transform.rotation.eulerAngles.y * Mathf.PI / 180) * 5), player2.transform.rotation);
        for (float timer = 0.4f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        firing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
