using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeScript : MonoBehaviour
{
    public GameObject Evensystem;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Evensystem.gameObject.GetComponent<dataScript>().changeViewer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
