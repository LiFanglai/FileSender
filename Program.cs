using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace FileSender
{
    class Program
    {
        static List<string> filePathList = new List<string>();

        static void Main(string[] args)
        {
            SendFile(@"d:\test", "127.0.0.1", 9527);
        }

        public static void SendFile(string root, string ip, int port)
        {
            TcpClient client = new TcpClient(ip, port);
            NetworkStream ns = client.GetStream();
            StreamWriter sr = new StreamWriter(ns);
            byte[] buffer = new byte[4];

            filePathList.Clear();
            getFileRecur(root, root);

            sr.WriteLine(filePathList.Count);
            sr.Flush();
            for (int i = 0; i < filePathList.Count; i++)
            {
                byte[] fileBytes = File.ReadAllBytes(filePathList[i]);
                Console.WriteLine(fileBytes.Length);
                sr.WriteLine(fileBytes.Length);
                sr.Flush();
                string fileName = Path.GetRelativePath(root, filePathList[i]);
                Console.WriteLine(fileName);
                sr.WriteLine(fileName);
                sr.Flush();
                ns.Read(buffer);
                client.Client.SendFile(filePathList[i]);
                ns.Read(buffer);
            }

            sr.Close();
            client.Close();
        }

        public static void getFileRecur(string path, string root)
        {
            if (Directory.Exists(path))
            {
                foreach(string p in Directory.GetFiles(path))
                {
                    filePathList.Add(p);
                }

                foreach (string subDir in Directory.GetDirectories(path))
                {
                    getFileRecur(subDir, root);
                }
            }
        }
    }
}
