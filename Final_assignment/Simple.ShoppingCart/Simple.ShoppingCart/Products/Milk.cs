using System.Text.RegularExpressions;
using Simple.ShoppingCart.Controller;
using Simple.ShoppingCart.Interfaces;

namespace Simple.ShoppingCart.Products;

public class Milk : Product, IMilk
{
    public Milk() : base()
    {
        Price = MILK_PRICE;
    }


    /// <summary>
    /// The interface implementation for adding milk
    /// </summary>
    /// <returns>subtotal for milk</returns>
    public decimal AddMilk(string basket, decimal subtotal)
    {
        foreach (Match m in Regex.Matches(basket, OrderProcessor.MILK, RegexOptions.IgnoreCase))
        {
            Count++;
            subtotal += Price;
        }

        return subtotal;
    }


    /// <summary>
    /// The interface implementation for removing milk
    /// </summary>
    /// <returns>subtotal for milk</returns>
    public decimal RemoveMilk(string basket, decimal subtotal)
    {
        foreach (Match a in Regex.Matches(basket, GetType().Name, RegexOptions.IgnoreCase))
        {
            subtotal -= Price;
            Count--;
        }

        return subtotal;
    }
}
