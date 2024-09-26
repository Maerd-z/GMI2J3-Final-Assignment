using System.Text.RegularExpressions;
using Simple.ShoppingCart.Controller;
using Simple.ShoppingCart.Interfaces;

namespace Simple.ShoppingCart.Products;

public class Bread : Product, IBread
{
    public Bread() : base()
    {
        Price = BREAD_PRICE;
    }


    /// <summary>
    /// The interface implementation for adding bread
    /// </summary>
    /// <returns>subtotal for bread</returns>
    public decimal AddBread(string basket, decimal subtotal)
    {
        foreach (Match b in Regex.Matches(basket, OrderProcessor.BREAD, RegexOptions.IgnoreCase))
        {
            Count++;
            subtotal += Price;
        }

        return subtotal;
    }


    /// <summary>
    /// The interface implementation for removing bread
    /// </summary>
    /// <returns>subtotal for bread</returns>
    public decimal RemoveBread(string basket, decimal subtotal)
    {
        foreach (Match b in Regex.Matches(basket, GetType().Name, RegexOptions.IgnoreCase))
        {
            Count--;
            subtotal -= Price;
        }

        return subtotal;
    }
}
