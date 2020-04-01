using System;
using System.IO;


namespace server
{

    public class myLogger
    {

        private string logPath;

        public myLogger(string path)
        {
            logPath = path;
        }

        public void logBind()
        {
            using (StreamWriter sw = new StreamWriter(logPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine("--------------------------------------------------------------------");
                sw.WriteLine("Server is running " + DateTime.Now);
            }
        }

        public void logListen()
        {
            using (StreamWriter sw = new StreamWriter(logPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine("Listening mode " + DateTime.Now);
            }
        }

        public void logNewConnection()
        {
            using (StreamWriter sw = new StreamWriter(logPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine("--------------------------------------------------------------------");
                sw.WriteLine("New connection " + DateTime.Now);
            }
        }

        public void logReceive(int count)
        {
            using (StreamWriter sw = new StreamWriter(logPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine("Received data " + count + " bytes " + DateTime.Now);
            }
        }

        public void logSend(int count)
        {
            using (StreamWriter sw = new StreamWriter(logPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine("Data sent " + count + " bytes " + DateTime.Now);
            }
        }

        public void logCloseConnection(int count)
        {
            using (StreamWriter sw = new StreamWriter(logPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine("Number of connections " + count);
                sw.WriteLine("Server is stopped " + DateTime.Now);
                sw.WriteLine("--------------------------------------------------------------------");
            }
        }

        public void logDisconnect()
        {
            using (StreamWriter sw = new StreamWriter(logPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine("Client disconnected " + DateTime.Now);
            }
        }

    }

}