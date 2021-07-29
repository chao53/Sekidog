using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
public class player2Script : MonoBehaviour
{
    public GameObject player2;
    public GameObject nameTag;
    //private GameObject c1;
    private Animator _animator;

    // Start is called before the first frame update 
    void Start()
    {
        
    }

    public void attatchPlayer(int x)
    {
        player2 = GameObject.Find("player" + x);
        nameTag = GameObject.Find("NameTag" + x);
        _animator = player2.GetComponent<Animator>();
    }

    public void decode(string str)
    {
        String[] vs = str.Split(',', '(', ')'); 
        if(int.Parse(vs[0]) == 0)
        {
            nameTag.GetComponent<TMP_Text>().text = vs[1];
        }
        else if(int.Parse(vs[0]) == 1)
        {
            player2.transform.position = new Vector3(float.Parse(vs[1]), float.Parse(vs[2]), float.Parse(vs[3]));
            player2.transform.localRotation = Quaternion.Euler(0, float.Parse(vs[4]), 0);
            _animator.SetInteger("Hand", int.Parse(vs[5]));
            _animator.SetFloat("Ix", float.Parse(vs[6]));
            _animator.SetFloat("Iz", float.Parse(vs[7]));
            _animator.SetFloat("Jump", float.Parse(vs[8]));
            _animator.SetFloat("JumpTimer", float.Parse(vs[9]));
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
