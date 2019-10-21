using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace ServiceBusSubscriber
{
    public class SubscriptionFactory : ISubscriptionFactory
    {
        private readonly ServiceBusConfiguration _configuration;

        public SubscriptionFactory()
        {
            //Used instance init here, since not having DI container.
            _configuration = new ServiceBusConfiguration();
        }

        public async Task<ISubscriptionClient> GetSubscriptionClient()
        {
            await CreateSubscriptionIfNotExists();
            var subscriptionClient = new SubscriptionClient(_configuration.Endpoint, _configuration.TopicName, _configuration.InvoiceSubscriptionName);

            await ValidateSubscriptionRules(subscriptionClient, MessageLabels.MyMessageLabel);

            return subscriptionClient;
        }

        public async Task CreateSubscriptionIfNotExists()
        {
            var client = new ManagementClient(_configuration.Endpoint);
            if (!await client.SubscriptionExistsAsync(_configuration.TopicName, _configuration.RestApiSubscriptionName))
                await client.CreateSubscriptionAsync(
                    new SubscriptionDescription(_configuration.TopicName, _configuration.RestApiSubscriptionName)
                    {
                        LockDuration = new TimeSpan(0, 0, _configuration.LockDurationMinutes, 0),
                    });
            await client.CloseAsync();
        }

        public async Task 
            ValidateSubscriptionRules(ISubscriptionClient subscriptionClient, string filterName)
        {
            var filterExist = false;
            var hasDefaultFilter = false;
            var filterCount = 0;

            foreach (var rule in await subscriptionClient.GetRulesAsync())
            {
                if (rule.Name == filterName && rule.Filter is CorrelationFilter filter &&
                    filter.Label == filterName)
                    filterExist = true;
                if (rule.Name == "$Default") hasDefaultFilter = true;
                filterCount++;
            }

            if (filterCount != 1 && hasDefaultFilter)
                throw new Exception("Invalid combination of subscription name and filter");

            if (hasDefaultFilter)
                await subscriptionClient.RemoveRuleAsync("$Default");

            if (!filterExist)
                await subscriptionClient.AddRuleAsync(new RuleDescription(filterName, new CorrelationFilter
                {
                    ContentType = "application/json",
                    Label = filterName
                }));
        }
    }

   
}
