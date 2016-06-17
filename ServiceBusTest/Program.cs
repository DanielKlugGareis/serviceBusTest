using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Resolver;

namespace ServiceBusTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var connectionString = "connectionstring here";
            //  var queueName = "Devqueue";

            TopicClient client = TopicClient.CreateFromConnectionString(connectionString, "Documents");

            TopicDescription td = new TopicDescription("Documents");
            td.MaxSizeInMegabytes = 5120;
            td.AutoDeleteOnIdle = TimeSpan.FromDays(7);
            
          //  td.AvailabilityStatus = EntityAvailabilityStatus.Available;
            
            td.DefaultMessageTimeToLive = TimeSpan.FromDays(7);

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            
            if (namespaceManager.TopicExists("Documents"))
            {
                namespaceManager.DeleteTopic("Documents");
            }
            namespaceManager.CreateTopic(td);

            //Create all topics

            foreach (var role in new string[] { "FastApprover", "STApprover", "BOSupervisor", "All" })
            {
                Resolver.SubscriptionResolver.CreateSubscriptionFilter(role, connectionString);
            }


            var text = Console.ReadLine().ToLowerInvariant();
            while (text != "exit")
            {
                BrokeredMessage message;
                if (text.Split(' ')[0] == "send")
                {
                    text = text.Substring(text.IndexOf(' ')).Trim();
                    message = new BrokeredMessage(string.Format("ID : {0} TEXT: {1}", Guid.NewGuid().ToString(), text));
                    message.TimeToLive = TimeSpan.FromDays(1);
                    message.Properties.Add("DocumentationType", (int)Enum.Parse(typeof(DocumentationType), text.Split(' ')[0]));

                    Console.WriteLine(string.Format("About to send message to Topic {0}, message with documentation type {1}", "Documents", text.Split(' ')[0]));

                    client.Send(message);
                    Console.WriteLine("Sended");
                }
                else if (text.Split(' ')[0] == "peek")
                {
                    message = client.Peek(long.Parse(text.Split(' ')[1]));
                    if (message != null)
                    {
                        Console.WriteLine(String.Format("Message body: {0}", message.GetBody<String>()));
                        Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
                        Console.WriteLine(String.Format("Message SequenceNumber: {0}", message.SequenceNumber));
                        Console.WriteLine(String.Format("Property DocumentationType: {0}", message.Properties["DocumentationType"].ToString()));
                    }
                    else
                    {
                        Console.WriteLine("No mesage found");
                    }


                }

                text = Console.ReadLine();
            }




        }
    }
}
