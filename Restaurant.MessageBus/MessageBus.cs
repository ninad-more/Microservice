using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;

namespace Restaurant.MessageBus
{
    public class MessageBus : IMessageBus
    {
        //private readonly string connectionString = ConfigurationManager.AppSettings["connectionstring"].ToString();
        private readonly string connectionString = "";
        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            try
            {
                await using var client = new ServiceBusClient(connectionString);
                ServiceBusSender sender = client.CreateSender(topicName);
                var jsonMessage = JsonConvert.SerializeObject(message);

                ServiceBusMessage finalMessage = new(Encoding.UTF8.GetBytes(jsonMessage))
                {
                    CorrelationId = Guid.NewGuid().ToString()
                };

                await sender.SendMessageAsync(finalMessage);
                await client.DisposeAsync();
            }
            catch(Exception ex)
            {
                var e = ex.Message;
            }


        }
    }
}
