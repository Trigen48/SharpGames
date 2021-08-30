using System;
using System.Collections.Generic;

using System.Text;
using System.Net;

namespace Snake
{
    public class NetWorking
    {
        int m_ip, m_port=3658;
        byte[] m_data=null;
       // System.Net.Sockets.TcpListener t=null;
        System.Net.Sockets.Socket host=null;
       // System.Net.Sockets.TcpClient client=null;


        public NetWorking()
        {
        }

        public NetWorking(int ip)
        {
        
        }

        public NetWorking(int ip, int port)
        {
        }

        public void CreateConnection()
        {
            System.Net.Sockets.SocketInformation infoHost= new System.Net.Sockets.SocketInformation();

            infoHost.Options = System.Net.Sockets.SocketInformationOptions.Connected;


            host = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.IPv4);

            host.Bind(new System.Net.IPEndPoint(IPAddress.Any,3566));
      
            
          
        }

        public void Connect()
        {
        }

        public byte[] Data
        {
            get
            {
                return m_data;
            }
        }

        public int Port
        {
            get
            {
                return m_port;
            }
            set
            {
                m_port = value;
            }
        }

        public int IP
        {
            get
            {
                return m_ip;
            }
            set
            {
                m_ip = value;
            }
        }

    }
}
