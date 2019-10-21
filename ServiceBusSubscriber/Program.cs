using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ServiceBusSubscriber
{
    class Program
    {
        private static ISubscriptionClient _subscriptionClient;


        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var subscriptionFactory = new SubscriptionFactory();

            _subscriptionClient = await subscriptionFactory.GetSubscriptionClient();

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            _subscriptionClient.RegisterMessageHandler(Consume, new MessageHandlerOptions(LogMessageHandlerException) {AutoComplete = true,MaxConcurrentCalls = 1});


            Console.ReadKey();
            await _subscriptionClient.CloseAsync();
        }

        public static Task Consume(Message message, CancellationToken cancellationToken)
        {

            ValidateMessage(message);

            var messageBody = Encoding.UTF8.GetString(message.Body);

            var convertedObject = JsonConvert.DeserializeObject<Person>(messageBody);

            Console.WriteLine(messageBody);

            return Task.CompletedTask;
        }

        private static void ValidateMessage(Message message)
        {
            if(message.Label != MessageLabels.MyMessageLabel)
                throw new Exception("Labels are not identical!");
        }

        private static Task LogMessageHandlerException(ExceptionReceivedEventArgs arg)
        {
            //exception to log
            throw new Exception();
        }


        


    }
}
