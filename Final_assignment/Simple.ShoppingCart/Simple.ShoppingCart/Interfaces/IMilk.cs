namespace Simple.ShoppingCart.Interfaces;

public interface IMilk
{
    int Count { get; set; }
    decimal AddMilk(string basket, decimal subtotal);
    decimal RemoveMilk(string basket, decimal subtotal);
}
