namespace OrderingApi.Consumers;

using System.Threading;
using Confluent.Kafka;
using OrderingApi.Config;
using System.Text.Json;
using OrderingApi.Data;
using OrderingApi.Services;

public class StockKafkaConsumer : StockConsumer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string topic = "stock.quantity";
    private CreateOrUpdateStockService _createOrUpdateStockService;

    public StockKafkaConsumer(
        IServiceProvider serviceProvider,
        CreateOrUpdateStockService createOrUpdateStockService
    )
    {
        _serviceProvider = serviceProvider;
        _createOrUpdateStockService = createOrUpdateStockService;
    }

    public Task Consume()
    {
        var config = GetKafkaConfig();

        using (var consumer = GetKafkaConsumer(config))
        {
            consumer.Subscribe(topic);

            TryToConsumeOrCloseOnTermination(consumer);
        }

        return Task.CompletedTask;
    }

    ConsumerConfig GetKafkaConfig()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = Env.KAFKA_URL,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "stock.quantity-00"
        };

        return config;
    }

    IConsumer<Ignore, String> GetKafkaConsumer(ConsumerConfig config)
    {
        var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

        return consumer;
    }

    void TryToConsumeOrCloseOnTermination(IConsumer<Ignore, String> consumer)
    {
        try
        {
            ConsumeDataUntilAppTermination(consumer);
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }

    void ConsumeDataUntilAppTermination(IConsumer<Ignore, String> consumer)
    {
        while (true)
        {
            var data = consumer.Consume();

            var stock = DeserializeData(data.Message.Value);
            _createOrUpdateStockService.Execute(stock);
        }
    }

    Stock? DeserializeData(String streamData)
    {
        return JsonSerializer.Deserialize<Stock>(streamData);
    }
}
