namespace Simple.ShoppingCart.Interfaces;

public interface IButter
{
    int Count { get; set; }
    decimal AddButter(string basket, decimal subtotal);
    decimal RemoveButter(string basket, decimal subtotal);
}
