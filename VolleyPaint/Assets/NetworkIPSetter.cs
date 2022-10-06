using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using System;
using System.Text;
using System.Net;

public class NetworkIPSetter : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        string host = Dns.GetHostName();

        // Getting ip address using host name
        IPHostEntry ip = Dns.GetHostEntry(host);
        Debug.Log(GetLocalIPAddress());
    }

    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
