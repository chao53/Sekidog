using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dataScript : MonoBehaviour
{
    public GameObject Server;
    public GameObject Client;
    public GameObject SInputIpv4;
    public GameObject SInputPort;
    public GameObject SInputName;
    public GameObject CInputIPv4;
    public GameObject CInputPort;
    public GameObject CInputName;
    public GameObject SPanel;
    public GameObject CPanel;
    public GameObject GPanel;

    public int viewer = 3;
    public int playerID = 1;
    public string localIP = "0.0.0.0";
    public GameObject Camera1;
    public GameObject Camera2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeIP(string IP)
    {
        localIP = IP;
    }
    public void changePlayerID(int i)
    {
        playerID = i;
        this.GetComponent<player2Script>().attatchPlayer(3-i);
        this.GetComponent<player1Script>().attatchPlayer(i);
        this.GetComponent<player1Script>().turnSwitch(1);
        Camera1.GetComponent<cameraScript>().attatchPlayer(i);
        Camera2.GetComponent<cameraScript>().attatchPlayer(i);

        if(i == 1)
        {
            Server.GetComponent<Server>().CreatServer(SInputIpv4.GetComponent<InputField>().text, int.Parse(SInputPort.GetComponent<InputField>().text));
            SPanel.SetActive(false);
            GPanel.SetActive(true);
            //PlayerName = SInputName.GetComponent<InputField>().text;
        }
        else if(i == 2)
        {
            Client.GetComponent<Client>().startConnect(CInputIPv4.GetComponent<InputField>().text, int.Parse(CInputPort.GetComponent<InputField>().text));
            CPanel.SetActive(false);
            GPanel.SetActive(true);
            //PlayerName = inputName.GetComponent<InputField>().text;
        }
    }

    public void changeViewer()
    {
        if(viewer == 3)
        {
            viewer = 1;
        }
        else
        {
            viewer = 3;
        } 
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
