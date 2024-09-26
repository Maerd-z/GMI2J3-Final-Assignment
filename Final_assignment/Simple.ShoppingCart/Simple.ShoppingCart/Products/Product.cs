
namespace Simple.ShoppingCart.Products;

public class Product
{
    public const decimal BREAD_PRICE = 2.00m;
    public const decimal BUTTER_PRICE = 1.80m;
    public const decimal MILK_PRICE = 1.15m;

    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Count { get; set; }
    protected decimal Price { get; set; }
}