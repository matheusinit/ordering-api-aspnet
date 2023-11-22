namespace OrderingApi.Consumers;

using System.Threading;
using Confluent.Kafka;
using OrderingApi.Config;

public class StockKafkaConsumer : StockConsumer
{
    public Task Consume()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = Env.KAFKA_URL,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "stock.quantity-00"
        };

        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            consumer.Subscribe("stock.quantity");
            var cts = new CancellationTokenSource();

            try
            {
                while (true)
                {
                    var data = consumer.Consume(cts.Token);
                    Console.WriteLine(data.Message.Value);
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }

        return Task.CompletedTask;
    }
}
