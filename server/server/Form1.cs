using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace server
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        Thread listenThread;
        static string workPath = Directory.GetCurrentDirectory() + "\\ads.txt";
        myLogger logger = new myLogger(Directory.GetCurrentDirectory() + "\\log.txt");
        static int countOfConnections = 0;
        static bool workFlag = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            workFlag = false;
            Process.GetCurrentProcess().Kill();
            Close();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            workFlag = true;
            listener = new TcpListener(IPAddress.Any, 12000);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
            logger.logBind();
        }

        private void ListenForClients()
        {
            listener.Start();
            //logger.logListen();

            while (workFlag)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(client);
                }
                catch (SocketException)
                {
                    break;
                }
            }

        }

        //функция вторичного потока
        private void HandleClient(object client)
        {

            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            countOfConnections++;
            //logger.logNewConnection();

            byte[] message = new byte[5000];
            int bytes;

            while(workFlag)
            {
                try
                {
                    bytes = clientStream.Read(message, 0, 5000);
                    //logger.logReceive(bytes);
                }
                catch
                {
                    break;
                }

                string messageFromClient = Encoding.UTF8.GetString(message, 0, bytes).Trim('\0');
                string[] msg = messageFromClient.Split('*');
                if (msg[0].Equals("GET"))
                {
                    string tempStr;
                    string messageToClient = "";
                    try
                    {
                        using (StreamReader sr = new StreamReader(workPath))
                        {
                            while ((tempStr = sr.ReadLine()) != null)
                            {
                                messageToClient += String.Concat(tempStr, "\r\n");
                            }
                        }
                    }
                    catch
                    {
                        messageToClient = "Error";
                    }
                    Echo(messageToClient, clientStream);
                }
                else if (msg[0].Equals("POST"))
                {
                    string messageToClient = "";
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(workPath, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine(msg[1]);
                            sw.WriteLine(msg[2]);
                            sw.WriteLine("--------------------------------------------");
                        }
                        messageToClient = "Ad added";
                    }
                    catch
                    {
                        messageToClient = "Error";
                    }
                    Echo(messageToClient, clientStream);
                }

            }
            tcpClient.Close();
            //logger.logDisconnect();

        }

        private void Echo(string message, NetworkStream clientStream)
        {

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            clientStream.Write(buffer, 0, buffer.Length);
            //logger.logSend(buffer.Length);
            clientStream.Flush();

        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            workFlag = false;
            listener.Stop();
            logger.logCloseConnection(countOfConnections);
        }
    }
}
