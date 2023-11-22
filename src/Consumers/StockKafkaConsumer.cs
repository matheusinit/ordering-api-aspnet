namespace OrderingApi.Consumers;

using System.Threading;
using Confluent.Kafka;

public class StockKafkaConsumer : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "0.0.0.0:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "stock.quantity"
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

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
