using System.Text.RegularExpressions;
using Simple.ShoppingCart.Controller;
using Simple.ShoppingCart.Interfaces;

namespace Simple.ShoppingCart.Products;

public class Butter : Product, IButter
{
    public Butter() : base()
    {
        Price = BUTTER_PRICE;
    }


    /// <summary>
    /// The interface implementation for adding butter
    /// </summary>
    /// <returns>subtotal for butter</returns>
    public decimal AddButter(string basket, decimal subtotal)
    {
        foreach (Match a in Regex.Matches(basket, OrderProcessor.BUTTER, RegexOptions.IgnoreCase))
        {
            subtotal += Price;
            Count++;
        }

        return subtotal;
    }

    /// <summary>
    /// The interface implementation for removing butter
    /// </summary>
    /// <returns>subtotal for butter</returns>
    public decimal RemoveButter(string basket, decimal subtotal)
    {
        foreach (Match b in Regex.Matches(basket, GetType().Name, RegexOptions.IgnoreCase))
        {
            Count--;
            subtotal -= Price;
        }

        return subtotal;
    }
}
