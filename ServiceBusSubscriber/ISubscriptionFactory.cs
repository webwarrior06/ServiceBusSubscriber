using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusSubscriber
{
    public interface ISubscriptionFactory
    {
        Task<ISubscriptionClient> GetSubscriptionClient();
        Task CreateSubscriptionIfNotExists();
        Task ValidateSubscriptionRules(ISubscriptionClient subscriptionClient, string filterName);
    }
}