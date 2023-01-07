using SignalRPrototype.Shared.Models;

namespace SignalRPrototype.Server.Utility;

public static class ProductHelper
{
    private static readonly List<Product> Products;
    private static readonly List<Store> Stores;

    static ProductHelper()
    {
        Products = new List<Product>
        {
            new() { ProductId = Guid.NewGuid(), Name = "SOFT DRINKS" },
            new() { ProductId = Guid.NewGuid(), Name = "MILK" },
            new() { ProductId = Guid.NewGuid(), Name = "CHIPS" },
            new() { ProductId = Guid.NewGuid(), Name = "EGGS" },
            new() { ProductId = Guid.NewGuid(), Name = "BREAD" },
            new() { ProductId = Guid.NewGuid(), Name = "BREAKFAST CEREAL" },
            new() { ProductId = Guid.NewGuid(), Name = "BLOCK CHEESE" },
            new() { ProductId = Guid.NewGuid(), Name = "BEER" },
            new() { ProductId = Guid.NewGuid(), Name = "WATER" },
            new() { ProductId = Guid.NewGuid(), Name = "CHOCOLATE BARS" },
        };

        Stores = new List<Store>
        {
            new() { StoreId = Guid.NewGuid(), Name = "Coles Supermarket", Location = 0 },
            new() { StoreId = Guid.NewGuid(), Name = "IGA", Location = 0 },
            new() { StoreId = Guid.NewGuid(), Name = "Woolworths Supermarket", Location = 0 },
            new() { StoreId = Guid.NewGuid(), Name = "Coles Express", Location = 0 },
            new() { StoreId = Guid.NewGuid(), Name = "Aldi Australia", Location = 0 },
        };
    }
    
    public static ProductPrice GenerateProductPrice()
    {
        Random rand = new();
        return new ProductPrice()
        {
            ProductId = GetRandomProduct().ProductId,
            StoreId = GetRandomStore().StoreId,
            Price = (decimal)rand.Next(200, 1000) / 100,
            TimeGenerated = DateTime.UtcNow
        };
    }

    public static ProductPrice[] GenerateProductPricesForStoreIds(Guid[] storeIds)
    {
        Random rand = new();
        var result = storeIds.SelectMany(storeId =>
            Products.Select(product => new ProductPrice()
            {
                StoreId = storeId,
                ProductId = product.ProductId,
                TimeGenerated = DateTime.UtcNow,
                Price = (decimal)rand.Next(200, 1000) / 100
            })
        );
        return result.ToArray();
    }

    public static IEnumerable<Product> GetAllProducts() => Products;
    public static IEnumerable<Store> GetAllStores() => Stores;

    private static Product GetRandomProduct()
    {
        Random rand = new();
        return Products[rand.Next(0, Products.Count)];
    }
    
    private static Store GetRandomStore()
    {
        Random rand = new();
        return Stores[rand.Next(0, Stores.Count)];
    }
}