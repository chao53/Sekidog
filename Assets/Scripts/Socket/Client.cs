using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;

public class Client : MonoBehaviour
{

    public GameObject EventSystem;
    private Socket socket;
    private byte[] buffer = new byte[1024];
    private int mark = -1;
    private string strMessage;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void startConnect(string CIpv4,int CPort)
    {
        //print("startConnect");
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(IPAddress.Parse(CIpv4), CPort);
        StartReceive();
        
    }

    public void Send(String str)
    {
        //print("Send");
        //socket.Send(Encoding.UTF8.GetBytes(inputWord.GetComponent<InputField>().text));
        socket.Send(Encoding.UTF8.GetBytes(str));//sendÖ»ÄÜ·¢ËÍByte
    }

    void StartReceive()
    {
        //print("StartReceive");
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }

    void ReceiveCallback(IAsyncResult iar)
    {
        //print("ReceiveCallback");
        int len = socket.EndReceive(iar);
        if (len == 0)
        {
            return;
        }
        string str = Encoding.UTF8.GetString(buffer, 0, len);
        mark = int.Parse("" + str[0]);
        if (mark == 1)
        {
            strMessage = str;
        }
        //print(str);
        StartReceive();
    }
    // Update is called once per frame
    void Update()
    {
        if (mark == 1)
        {
            EventSystem.GetComponent<player2Script>().decode(strMessage);
        }
    }
}
