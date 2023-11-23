namespace OrderingApi.Consumers;

using System.Threading;
using Confluent.Kafka;
using OrderingApi.Config;
using System.Text.Json;
using OrderingApi.Data;

public class StockKafkaConsumer : StockConsumer
{
    private readonly IServiceProvider _serviceProvider;

    public StockKafkaConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Consume()
    {
        var _context = new ApplicationContext();

        using (var scope = _serviceProvider.CreateScope())
        {
            _context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

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

                        var stock = JsonSerializer.Deserialize<Stock>(data.Message.Value);

                        if (stock == null)
                        {
                            continue;
                        }

                        _context.Stocks.Add(stock);
                        _context.SaveChanges();
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }

        return Task.CompletedTask;
    }
}
