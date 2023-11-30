namespace OrderingApi.Services;

using OrderingApi.Data;

public class CreateOrUpdateStockService
{
    private readonly IServiceProvider _serviceProvider;

    public CreateOrUpdateStockService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Stock? Execute(Stock? stock)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            if (stock == null)
            {
                return null;
            }

            var stockFromDatabase = _context.Stocks.Add(stock);
            _context.SaveChanges();

            return stockFromDatabase.Entity;
        }
    }
}
