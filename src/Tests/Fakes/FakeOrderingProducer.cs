using OrderingApi.Producers;

namespace OrderingApi.Tests.Fakes;

public class FakeOrderingProducer : OrderingProducer
{
    public Task<bool> SendOrderThroughMessageQueue(string topic, OrderToProduce order)
    {
        return Task.FromResult(true);
    }
}
