using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    
    public Transform _Player;
    public GameObject viewMode;
    public GameObject Evensystem;

    private int viewer = 3;
    void Start()
    {
        viewer = Evensystem.gameObject.GetComponent<dataScript>().viewer;
    }

    public void attatchPlayer(int x)
    {
        _Player = GameObject.Find("player" + x).transform;
    }

    void LateUpdate()
    {
        viewer = Evensystem.gameObject.GetComponent<dataScript>().viewer;

        float dx = viewMode.gameObject.GetComponent<viewControl>().input2.x;
        float dy = viewMode.gameObject.GetComponent<viewControl>().input2.y;

        if(viewer == 1)
        {
            transform.position = new Vector3(0,  8, -5);
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if(viewer == 3)
        {
            transform.position = new Vector3(_Player.position.x + 2.5f, _Player.position.y + 10 + dx/10, _Player.position.z - 15 + Mathf.Abs(dx)/6);
            transform.localRotation = Quaternion.Euler(dx, 0, 0);
            transform.RotateAround(_Player.position, _Player.up, dy);
        }
    }
}
