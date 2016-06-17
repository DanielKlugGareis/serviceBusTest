using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resolver
{
    public class SubscriptionResolver
    {

        public static List<DocumentationType> GetDocsByRole(string role)
        {
            switch (role)
            {
                case "FastApprover":
                    return new List<DocumentationType>
                    {
                        DocumentationType.FastApproval,
                    };
                case "STApprover":
                    return new List<DocumentationType>
                    {
                        DocumentationType.StreetTeams,
                    };
                case "BOSupervisor":
                    return new List<DocumentationType>
                    {
                        DocumentationType.FastApproval,
                        DocumentationType.StreetTeams,
                    };
                default:
                    return null;
            }
        }


        public static void CreateSubscriptionFilter(string role, string connectionString)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            SubscriptionDescription sd = new SubscriptionDescription("Documents", role);
            sd.MaxDeliveryCount = short.MaxValue;
            var types = GetDocsByRole(role);

            if (types != null)
            {

                SqlFilter byTypes = new SqlFilter(string.Format("DocumentationType in ({0})",
                    string.Join(",",
                    types.Select(D => ((int)D).ToString()).ToArray()
                    ))
                    );
                if (namespaceManager.SubscriptionExists("Documents", role))
                {
                    namespaceManager.DeleteSubscription("Documents", role);
                }
                namespaceManager.CreateSubscription(sd, byTypes);

            }
            else {

                if (namespaceManager.SubscriptionExists("Documents", role))
                {
                    namespaceManager.DeleteSubscription("Documents", role);
                }
                namespaceManager.CreateSubscription(sd);
            }
        }
    }


}
