using System;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using EchoServerSocket;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTest
{
    [TestClass]
    public class UnitTest1
    {
        private Worker worker;

        [TestInitialize]
        public void Initiate()
        {
            worker = new Worker();
            worker.Start();
            
        }

        [TestMethod]
        public void TestMethod1()
        {
            string strin;
            using (TcpClient socket = new TcpClient("localhost", 7))// Destination og portnummer
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                string str = "Den besked jeg sender";

                sw.WriteLine(str);
                sw.Flush();

                strin = sr.ReadLine();
                Console.WriteLine(strin);
            }
            Assert.AreEqual("4",strin);
        }
    }
}
