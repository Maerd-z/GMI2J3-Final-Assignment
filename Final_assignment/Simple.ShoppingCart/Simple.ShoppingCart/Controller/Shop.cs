
using System.Text;
using Simple.ShoppingCart.Interfaces;

namespace Simple.ShoppingCart.Controller;

public class Shop : IShop
{
    /// <summary>
    /// Shows the first entry visit text 
    /// </summary>
    /// <returns></returns>
    public string MainText()
    {
        var mainString = new StringBuilder();

        mainString.Append("Welcome to the Simple Shopping Cart®");
        mainString.Append(Environment.NewLine + "We have the following items in stock:");
        mainString.Append(Environment.NewLine + "* Bread - €2.00 (per loaf)");
        mainString.Append(Environment.NewLine + "* Butter - €1.80 (per block)");
        mainString.Append(Environment.NewLine + "* Milk - €1.15 (per pack)");
        mainString.Append(Environment.NewLine + "-------");
        mainString.Append(Environment.NewLine + "Special offers:");
        mainString.Append(Environment.NewLine + "- Buy 2 blocks of Butter and get a loaf of Bread for half the price");
        mainString.Append(Environment.NewLine + "- Buy 3 packs of Milk and get the 4th pack free (or buy 4 packs of Milk and get it for the cost of 3)");
        mainString.Append(Environment.NewLine);
        mainString.Append(Environment.NewLine + "Please type in your command (Add, Remove, Show, Pay, Quit etc. with first letter case sensitive)");
        mainString.Append(Environment.NewLine + "and orders (case insensitive) repeatedly as follows:" + Environment.NewLine);
        mainString.Append("Example 1: First 'A' and enter, then 'bread butter butter milk' and enter to buy these products" + Environment.NewLine);
        mainString.Append("Example 2: First 'R' and enter, then 'bread butter butter milk' and enter to remove these products" + Environment.NewLine);

        return mainString.ToString();
    }
}
