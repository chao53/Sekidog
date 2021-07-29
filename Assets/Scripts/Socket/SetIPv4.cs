using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Net.Sockets;
using System.Net;
public class SetIPv4 : MonoBehaviour
{
    private string thisIp = "0.0.0.0";
    // Start is called before the first frame update
    void Start()
    {
        string name = Dns.GetHostName();
        IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
        foreach (IPAddress ipa in ipadrlist)
        {
            
            if (ipa.AddressFamily == AddressFamily.InterNetwork)
            {
                print(ipa.ToString());
                thisIp = ipa.ToString();
            }
        }
        this.GetComponent<InputField>().text = thisIp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
