using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusReceiver_test_1
{
    class Program
    {

        const string SBconnection = "";
        const string QName = "";
        static IQueueClient qClient;
        
        static void Main(string[] args)
        {
            Console.WriteLine($"Start at: {DateTime.UtcNow}");
            MainAsync().GetAwaiter().GetResult();
            Console.WriteLine($"Received all at: {DateTime.UtcNow}");
        }

        static async Task MainAsync()
        {
            qClient = new QueueClient(SBconnection, QName);
            RegisterOnMessageHandlerAndReceieMessages();
            Console.ReadKey();
            await qClient.CloseAsync();
        }

        static void RegisterOnMessageHandlerAndReceieMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExeptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            qClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var toString_test = System.Text.Encoding.Default.GetString(message.Body);

            Console.WriteLine(
              $"Received message: SequenceNumber: {toString_test}"
                );
            await qClient.CompleteAsync(message.SystemProperties.LockToken);
        }

       static Task ExeptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            //TODO
            Console.WriteLine("Error");
            return Task.CompletedTask;
        }
    }
}
