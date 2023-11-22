using OrderingApi.Consumers;

namespace OrderingApi.BackgroundServices;

public class StockConsumerBackgroundService : IHostedService
{
    StockConsumer _stockConsumer;

    public StockConsumerBackgroundService(StockConsumer stockConsumer)
    {
        _stockConsumer = stockConsumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _stockConsumer.Consume();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
