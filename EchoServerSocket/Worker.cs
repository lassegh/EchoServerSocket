using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EchoServerSocket
{
    class Worker
    {
        public Worker()
        {

        }

        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, 7);// Loopback er ens egen maskine, port 7 er standard for EchoServer
            server.Start();
            while (true)
            {
                DoClient(server.AcceptTcpClient()); // Denne venter hårdt på en client

            }


        }

        public void DoClient(TcpClient socket)
        {
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                string str = sr.ReadLine();
                sw.WriteLine(str);
                sw.Flush(); // Dette sikrer at writeren ikke buffer i hukommelsen, men sender ud konstant
            }
        }
    }
}
