using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Checkers
{
    public class ServerClient : MonoBehaviour
    {

        public string clientName;
        public TcpClient tcp;
        public bool isHost;

        public ServerClient(TcpClient tcp)
        {
            this.tcp = tcp;
        }
    }
}