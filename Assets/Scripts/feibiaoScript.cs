using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feibiaoScript : MonoBehaviour
{
    public bool canHurt = true;
    GameObject EventSystem;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
        
        
    }
    void OnCollisionStay(Collision col)
    {
        if (canHurt)
        {
            if (col.gameObject.tag == "player" && col.gameObject.name != EventSystem.GetComponent<player1Script>().player1.name)
            {
                EventSystem.GetComponent<player1Script>().feibiaoHit(col.gameObject);
                canHurt = false;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
