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

            var stockExists = _context.Stocks.Any(s => s.id == stock.id);

            if (stockExists)
            {
                var stockUpdated = _context.Stocks.Update(stock);
                _context.SaveChanges();

                return stockUpdated.Entity;
            }

            var stockFromDatabase = _context.Stocks.Add(stock);
            _context.SaveChanges();

            return stockFromDatabase.Entity;
        }
    }
}
