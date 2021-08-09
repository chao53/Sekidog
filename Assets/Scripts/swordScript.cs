using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordScript : MonoBehaviour
{
    public bool _switch = false;
    GameObject EventSystem;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
    }
    void OnTriggerStay(Collider col)
    {
        
        if (col.tag == "player" && col.name != EventSystem.GetComponent<player1Script>().player1.name && _switch)
        {
            EventSystem.GetComponent<player1Script>().swordHit(col.gameObject);
        }
    }

    //private void OnTriggerExit(Collider col)
    //{
    //    if (col.tag == "player" && col.name != EventSystem.GetComponent<player1Script>().player1.name && _switch)
    //    {
    //        EventSystem.GetComponent<player1Script>().swordLeave();
    //    }
    //}


    // Update is called once per frame
    void Update()
    {
        
    }
}
