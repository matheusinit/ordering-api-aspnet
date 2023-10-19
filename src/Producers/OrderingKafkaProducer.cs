namespace OrderingApi.Producers;

using System.Net;
using System.Text.Json;
using Confluent.Kafka;

public class OrderingKafkaProducer : OrderingProducer
{
    public async Task<bool> SendOrderThroughMessageQueue(string topic, OrderToProduce order)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            ClientId = Dns.GetHostName()
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            var orderInJson = JsonSerializer.Serialize<OrderToProduce>(order);

            var result = await producer.ProduceAsync(
                topic,
                new Message<Null, string> { Value = orderInJson }
            );

            return await Task.FromResult(true);
        }
    }
}
