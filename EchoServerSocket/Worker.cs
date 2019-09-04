using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoServerSocket
{
    public class Worker
    {
        // Define type
        private delegate double MyCalcDelegate(double x, double y);

        // Define reference
        private MyCalcDelegate calcMethod;

        public Worker()
        {

        }

        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, 7);// Loopback er ens egen maskine, port 7 er standard for EchoServer
            server.Start();
            while (true)
            {
                TcpClient socket = server.AcceptTcpClient();// Denne venter hårdt på en client

                // Starter ny tråd
                Task.Run(() =>
                {
                    // Indsætter metoder (delegate) som lambda
                    TcpClient tmpSocket = socket;
                    DoClient(tmpSocket);
                }); 

            }


        }

        public void DoClient(TcpClient socket)
        {
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                string input = sr.ReadLine();
                if (input.Contains(':'))
                {
                    sw.WriteLine(TimeStampChecker(input));
                }
                else sw.WriteLine(SplitInput(input));
                sw.Flush(); // Dette sikrer at writeren ikke buffer i hukommelsen, men sender ud konstant
            }

            socket?.Close();
        }

        private DateTime TimeStampChecker(string input)
        {
            string[] arrayOfInput = input.Split(':');
            arrayOfInput[1] = arrayOfInput[1] + ":" + arrayOfInput[2];
            return DateTime.ParseExact(arrayOfInput[1], "d/M yyyy HH:mm", CultureInfo.InvariantCulture); // Formatet, der sendes til serveren skal passe 100 % med det format, den forventer( det indtastede).
        }

        private double SplitInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))return 0;

            string[] arrayOfInput = input.Split(" ");

            switch (arrayOfInput[0].ToLower())
            {
                case "add":
                    calcMethod = CalcDLL3Sem.Calculator.Sum;
                    return calcMethod(ParseToDouble(arrayOfInput[1]), ParseToDouble(arrayOfInput[2]));
                case "subtract":
                    calcMethod = CalcDLL3Sem.Calculator.Subtract;
                    return calcMethod(ParseToDouble(arrayOfInput[1]), ParseToDouble(arrayOfInput[2]));
                case "divide":
                    calcMethod = CalcDLL3Sem.Calculator.Divide;
                    return calcMethod(ParseToDouble(arrayOfInput[1]), ParseToDouble(arrayOfInput[2]));
                case "multiply":
                    calcMethod = CalcDLL3Sem.Calculator.Multiply;
                    return calcMethod(ParseToDouble(arrayOfInput[1]), ParseToDouble(arrayOfInput[2]));
                default:
                    return 0;
            }
        }

        private double ParseToDouble(string stringToParse)
        {
            double d;
            if (stringToParse.Contains('.'))
            {
                stringToParse = stringToParse.Replace(".", ",");
            }
            if (!double.TryParse(stringToParse, out d))
            {
                throw new Exception("Cannot parse");
            }

            return d;
        }


    }
}
