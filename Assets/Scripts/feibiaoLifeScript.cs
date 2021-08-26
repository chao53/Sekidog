using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feibiaoLifeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(life());
    }
    IEnumerator life()
    {
        this.GetComponent<feibiaoScript>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        for (float timer = 0.05f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<feibiaoScript>().enabled = true;
        for (float timer = 1f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        this.GetComponent<ConstantForce>().enabled = false;
        for (float timer = 0.8f; timer >= 0; timer -= Time.deltaTime)
        {
            yield return 0;
        }
        this.GetComponent<feibiaoScript>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
