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
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1", 9527);
            NetworkStream ns = client.GetStream();
            StreamWriter sr = new StreamWriter(ns);
            byte[] buffer = new byte[256];

            List<string> fileList = new List<string>
            {
                @"d:\test1.txt",
                @"d:\test2.txt",
                @"d:\test3.txt",
                @"d:\test4.txt",
            };

            sr.WriteLine(fileList.Count);
            sr.Flush();
            for (int i = 0; i < fileList.Count; i++)
            {
                byte[] fileBytes = File.ReadAllBytes(fileList[i]);
                Console.WriteLine(fileBytes.Length);
                sr.WriteLine(fileBytes.Length);
                sr.Flush();
                string fileName = Path.GetRelativePath(@"d:\", fileList[i]);
                Console.WriteLine(fileName);
                sr.WriteLine(fileName);
                sr.Flush();
                ns.Read(buffer);
                client.Client.SendFile(fileList[i]);
                ns.Read(buffer);
            }

            sr.Close();
            client.Close();
        }
    }
}
