using Simple.ShoppingCart.Exceptions;
using Simple.ShoppingCart.Interfaces;
using System.Diagnostics;
using System.Text;

namespace Simple.ShoppingCart.Controller;

public class OrderProcessor : IOrderProcessor
{
    private const decimal MILK_DISCOUNT = 1.15m;
    private const decimal BREAD_DISCOUNT = 1.00m;

    private readonly IBread _bread;
    private readonly IButter _butter;
    private readonly IMilk _milk;
    private readonly string _cartId;

    private CartState _state;

    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

    // Shops product names
    public const string BREAD = "bread";
    public const string BUTTER = "butter";
    public const string MILK = "milk";

    /// <summary>
    /// Real obj, interface, Castle Windsor or Autofac etc. calls the OrderProcessor constructor from container.Resolve
    /// From our tests we can call the OrderProcessor constructor with new/concrete or mocked objects.
    /// </summary>
    /// <returns></returns>
    public OrderProcessor(IBread bread, IButter butter, IMilk milk)
    {
        _bread = bread;
        _butter = butter;
        _milk = milk;
        _cartId = Guid.NewGuid().ToString();
        // Press the CTRL+ALT+O key combination to make sure that the VS Output window is visible
        Debug.WriteLine("CartId: " + _cartId);
        // Trace.WriteLine("CartId " + _cartId); // also visible in release
        _state = CartState.Empty;
        Debug.WriteLine("State: " + _state);
    }


    /// <summary>
    /// Here the Add in the order processing is done
    /// </summary>
    /// <returns>The current result from a customers orders as a string</returns>
    public string ProcessOrders(string basket) // basket = product you wanted to add.
    {
        var sb = new StringBuilder();
        var orderDictionary = new Dictionary<string, int>();
        var discountDictionary = new Dictionary<string, decimal>();

        //
        // states and nulls to check and other various fixes here ...
        //

        if (_state == CartState.PaidFor)
        {
            Total = 0;
            throw new IllegalStateException("Cannot add items to a PaidFor cart.");
        }
        if (_state == CartState.Empty)
        {
            _state = CartState.Active;
        }

        if (basket.Contains(BREAD))
        {
            Subtotal = _bread.AddBread(basket, Subtotal);
            var breadCount = _bread.Count;
            orderDictionary.Add(BREAD, breadCount);
        }

        if (basket.Contains(BUTTER))
        {
            Subtotal = _butter.AddButter(basket, Subtotal);
            var butterCount = _butter.Count;
            orderDictionary.Add(BUTTER, butterCount);

            // may need fix? Changed the BREAD_DISCOUNT to 1 since it isn't used as a factor.
            if (butterCount >= 2 && orderDictionary.ContainsKey(BREAD))
            {
                discountDictionary.Add("Bread Discount", BREAD_DISCOUNT);
            }
        }

        if (basket.Contains(MILK))
        {
            Subtotal = _milk.AddMilk(basket, Subtotal);
            var milkCount = _milk.Count;
            orderDictionary.Add(MILK, milkCount);

            if (milkCount == 4) 
            {
                discountDictionary.Add("1 free packs of Milk", MILK_DISCOUNT);
            }
            else if (milkCount >= 6)
            {
                discountDictionary.Add("2 free packs of Milk", 2 * MILK_DISCOUNT);
            }
        }


        // current shopping cart content
        sb.Append($"{Environment.NewLine}Your orders:");

        foreach (var order in orderDictionary)
        {
            sb.Append($"{Environment.NewLine}{order.Key} count: {order.Value}");
        }

        sb.Append($"{Environment.NewLine}Subtotal: €{Subtotal}");
        sb.Append($"{Environment.NewLine}---------------------------");


        if (discountDictionary.Count == 0)
        {
            sb.Append($"{Environment.NewLine}No offers applicable");
        }

        else
        {
            foreach (var item in discountDictionary)
            {
                sb.Append($"{Environment.NewLine}{item.Key} €{item.Value} off");
                Discount += item.Value;
            }
        }

        Total = Subtotal - Discount;
        sb.Append($"{Environment.NewLine}Total: €{Total}");

        return sb.ToString();
    }


    /// <summary>
    /// Here the Remove in order processing is done
    /// </summary>
    /// <returns>The current result from a customers orders as a string</returns>
    public string RemoveOrders(string basket) // You can reduce the Total if there is a discount and you type whatever.
    {
        //
        // more states and nulls to check, calculations to do and other various fixes here ...
        //
        if (_state == CartState.PaidFor)
        {
            Total = 0;
            throw new IllegalStateException("Can only remove from an active cart.");
        }
        if (_state == CartState.Empty)
        {
            Total = 0;
            throw new IllegalStateException("Can only remove from an active cart.");
        }
        if (basket.Contains(BREAD))
        {
            Subtotal = _bread.RemoveBread(basket, Subtotal);
        }

        if (basket.Contains(BUTTER))
        {
            Subtotal = _butter.RemoveButter(basket, Subtotal);
        }

        if (basket.Contains(MILK))
        {
            Subtotal = _milk.RemoveMilk(basket, Subtotal);
        }
        Total = Subtotal; // Prices are now updated when removing items.
        var discountDictionary = GetDiscount();
        var sb = new StringBuilder();

        if (discountDictionary.Count == 0)
        {
            sb.Append($"{Environment.NewLine}No offers applicable");
        }
        else
        {
            foreach (var item in discountDictionary)
            {
                sb.Append($"{Environment.NewLine}{item.Key} €{item.Value} off");
                Discount += item.Value;
            }
        }
        if (discountDictionary.Count >= 1) // Stop it from inappropriately lower Total.
        {
            Total = Subtotal - Discount;
        }
        sb.Append($"{Environment.NewLine}Total: €{Total}");

        if (_bread.Count == 0 && _butter.Count == 0 && _milk.Count == 0)
        {
            _state = CartState.Empty; // This was active. Changed to it becomes empty.
        }

        return sb.ToString();
    }



    /// <summary>
    /// Calculate the Discount on an order
    /// </summary>
    /// <returns>Discount as a Dictionary</returns>
    private Dictionary<string, decimal> GetDiscount()
    {
        //
        // check for errors in calculations and logic ...
        //
        var discountDictionary = new Dictionary<string, decimal>();

        if (_butter.Count >= 2 && _bread.Count >= 1)
        {
            discountDictionary.Add("Bread Discount", BREAD_DISCOUNT);
        }

        if (_milk.Count >= 4)
        {
            var milkDiscountCount = _milk.Count / 4;
            discountDictionary.Add(milkDiscountCount + "free packs of Milk", milkDiscountCount * MILK_DISCOUNT);
        }

        return discountDictionary;
    }



    /// <summary>
    /// Show the current cart/order
    /// </summary>
    /// <returns>The current result from a customers orders as a string</returns>
    public string ShowOrders()
    {
        var orderDictionary = new Dictionary<string, int>();
        var sb = new StringBuilder();

        if (_bread.Count > 0)
        {
            var breadCount = _bread.Count;
            orderDictionary.Add(BREAD, breadCount);
        }

        if (_butter.Count > 0)
        {
            var butterCount = _butter.Count;
            orderDictionary.Add(BUTTER, butterCount);
        }

        if (_milk.Count > 0)
        {
            var milkCount = _milk.Count;
            orderDictionary.Add(MILK, milkCount);
        }

        sb.Append($"{Environment.NewLine}Your orders:");

        foreach (var order in orderDictionary)
        {
            sb.Append($"{Environment.NewLine}{order.Key} count: {order.Value}");
        }

        return sb.ToString();
    }


    /// <summary>
    /// Pay for the current cart/order
    /// </summary>
    /// <returns>The paid amount</returns>
    public string PayOrders()
    {
        //
        // more state checks to do and other various fixes here ...
        //
        _state = CartState.PaidFor;
        //ClearSession();  Added this to stop it from adding to a PaidFor cart.
        return "Paid! €" + Total;
    }

    /// <summary>
    /// Clear Session (Cart/Order) and Start Over Again
    /// </summary>
    /// <returns>Session Info</returns>
    public string ClearSession()
    {
        ClearBasket();
        _state = CartState.Empty;
        return "Session Cleared!";
    }

    private void ClearBasket()
    {
        _bread.Count = 0;
        _butter.Count = 0;
        _milk.Count = 0;
    }
}
