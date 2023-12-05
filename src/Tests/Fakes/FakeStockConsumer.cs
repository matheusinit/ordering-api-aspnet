namespace Ordering.Tests.Fakes;

using OrderingApi.Consumers;

public class FakeStockConsumer : StockConsumer
{
    public Task Consume()
    {
        return Task.CompletedTask;
    }
}
