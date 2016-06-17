using System;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;

namespace GettingStartedWithQueues
{
    class Program
    {
        static void Main(string[] args)
        {
            var role = args[0];



            var connectionString = "connectionstring here";
           // var queueName = "Devqueue";

            //var client = QueueClient.CreateFromConnectionString(connectionString, queueName);

            // SqlFilter byTypes = new SqlFilter("MessageNumber > 3");


            SubscriptionClient sbClient = SubscriptionClient.CreateFromConnectionString
            (connectionString, "Documents", role,ReceiveMode.PeekLock);

            Console.WriteLine(string.Format("Role {0}", role));

            Console.WriteLine("Waiting");


            while (true)
            {
                var message = sbClient.Receive(TimeSpan.FromSeconds(15));
                try
                {
                    if (message != null)
                    {
                        Console.WriteLine(String.Format("Message body: {0}", message.GetBody<String>()));
                        Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
                        Console.WriteLine(String.Format("Message SequenceNumber: {0}", message.SequenceNumber));
                        Console.WriteLine(String.Format("Message DeliveryCount: {0}", message.DeliveryCount));
                        Console.WriteLine(String.Format("Property DocumentationType: {0}", message.Properties["DocumentationType"].ToString()));
                        // Remove message from subscription.
                        //message.Complete();
                        //message.Abandon();
                    }
                    else
                    {
                        Console.WriteLine("No message in queue yet");
                    }
                }
                catch (Exception)
                {
                    // Indicates a problem, unlock message in subscription.
                    message.Abandon();
                }
            }

        }
    }
}
