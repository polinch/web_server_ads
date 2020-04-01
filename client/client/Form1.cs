using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace client
{
    public partial class Form1 : Form
    {

        TcpClient client = new TcpClient();


        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string host = textBox1.Text;
            int port;
            if (host.Equals(""))
            {
                MessageBox.Show("Host is empty", "Error", MessageBoxButtons.OK);
                return;
            }
            try
            {
                port = Int32.Parse(textBox2.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Port is empty", "Error", MessageBoxButtons.OK);
                return;
            }

            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
            try
            {
                client.Connect(serverEndPoint);
            }
            catch(SocketException)
            {
                MessageBox.Show("Connection error", "Error", MessageBoxButtons.OK);
            }

        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            client.Close();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void getAllAdsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sendMessage("get");
        }

        private void sendMessage(string message)
        {

            NetworkStream clientStream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
            Byte[] data = new Byte[3000];
            int bytesCount = clientStream.Read(data, 0, data.Length);
            string response = Encoding.UTF8.GetString(data, 0, bytesCount).Trim('\0');
            textBox5.Text = response;

        }

        private void newAdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string title = textBox3.Text;
            string body = textBox4.Text;
            if (title.Equals("") || body.Equals(""))
            {
                MessageBox.Show("Field is empty", "Error", MessageBoxButtons.OK);
                return;
            }
            string message = "post " + title + " " + body;
            sendMessage(message);
        }
    }
}
