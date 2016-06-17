
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverName = "ServiceBusTest.exe";
            var clientName = "ServiceBusTest_Client.exe";

            var localPath = AppDomain.CurrentDomain.BaseDirectory;

            //init all others

            var serviceProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = serverName,
                    WorkingDirectory = localPath,
                }
            };
            serviceProcess.Start();

            System.Threading.Thread.Sleep(20 * 1000);


            List<Process> clients = new List<Process>();
            foreach (var role in new string[] { "FastApprover", "STApprover", "BOSupervisor", "All" })
            {
                var clientProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = clientName,
                        WorkingDirectory = localPath,
                        Arguments = role
                    }
                };
                clientProcess.Start();

                clients.Add(clientProcess);
            }


            Console.WriteLine("Enter to close all");

            serviceProcess.Close();

            foreach(var c in clients)
            {
                c.Close();
            }

            //init server 


        }
    }
}