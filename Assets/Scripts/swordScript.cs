using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordScript : MonoBehaviour
{
    GameObject EventSystem;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "player" && col.name != EventSystem.GetComponent<player1Script>()._Player.name)
        {
            print("1");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
