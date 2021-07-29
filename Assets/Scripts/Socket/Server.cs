//待解决：
//1,服务端向客户端发送消息。ko
//2,服务端为多个客户端服务。
//3,为每个客户端创建实例并命名。


using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

public class Server : MonoBehaviour
{
    public GameObject EventSystem;

    
    private string strMessage0;
    //private List<Client> clientList = new List<Client>();
    private Socket socket;
    //private int newPlayer = 0;
    private int mark = -1;
    private string newPlayerName;
    private byte[] buffer = new byte[1024];
    private string strMessage1;
    private string PlayerName;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void changeInfo(string strMessage0)
    {
        this.strMessage0 = strMessage0;
    }

    public void CreatServer(string SIpv4,int SPort)
    {
        //print("CreatServer");
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Parse(SIpv4), SPort));
        socket.Listen(0);
        StartAccept();
        Console.Read();    
    }

    void StartAccept()
    {
        //print("StartAccept");
        socket.BeginAccept(AcceptCallback, null);

    }

    void StartReceive(Socket client)
    {
        //print("StartReceive");
        client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, client);

    }

    void ReceiveCallback(IAsyncResult iar)
    {
        //print("ReceiveCallback");
        Socket client = iar.AsyncState as Socket;
        int len = client.EndReceive(iar);
        if (len == 0)
        {
            return;
        }
        string str = Encoding.UTF8.GetString(buffer, 0, len);
        mark = int.Parse("" + str[0]);
        if (mark == 1)
        {
            strMessage1 = str;
        }
        //print(str);
        client.Send(Encoding.UTF8.GetBytes(strMessage0));
        StartReceive(client);
    }

    void AcceptCallback(IAsyncResult iar)
    {
        //print("AcceptCallback");
        Socket client = socket.EndAccept(iar);
        StartReceive(client);
        StartAccept();
    }

    // Update is called once per frame
    void Update()
    {
        if (mark == 1)
        {
            EventSystem.GetComponent<player2Script>().decode(strMessage1);
        }
    }

}