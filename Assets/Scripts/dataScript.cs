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
    public GameObject soundSet1;
    public GameObject soundSet2;
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
        this.GetComponent<deCodeScript>().attatchPlayer(i);
        this.GetComponent<player1Script>().turnSwitch(1);
        Camera1.GetComponent<cameraScript>().attatchPlayer(i);
        Camera2.GetComponent<cameraScript>().attatchPlayer(i);
        GameObject.Find("Sword" + i).GetComponent<swordScript>()._switch = true;

        soundSet1.transform.parent = GameObject.Find("player" + i).transform;
        soundSet1.transform.localPosition = new Vector3(0, 2, 0);
        soundSet2.transform.parent = GameObject.Find("player" + (3 - i)).transform;
        soundSet2.transform.localPosition = new Vector3(0, 2, 0);

        if (i == 1)
        {
            Server.GetComponent<Server>().CreatServer(SInputIpv4.GetComponent<InputField>().text.Trim(), int.Parse(SInputPort.GetComponent<InputField>().text.Replace("_", "")));
            this.GetComponent<player1Script>().attatchPlayer(i, SInputName.GetComponent<InputField>().text.Replace("_", ""));
            SPanel.SetActive(false);
            GPanel.SetActive(true);
        }
        else if (i == 2)
        {
            Client.GetComponent<Client>().startConnect(CInputIPv4.GetComponent<InputField>().text.Trim(), int.Parse(CInputPort.GetComponent<InputField>().text.Replace("_", "")));
            this.GetComponent<player1Script>().attatchPlayer(i, CInputName.GetComponent<InputField>().text.Replace("_", ""));
            CPanel.SetActive(false);
            GPanel.SetActive(true);

        }
    }

    public void changeViewer()
    {
        if (viewer == 3)
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