using Simple.ShoppingCart.Controller;
using Simple.ShoppingCart.Exceptions;
using Simple.ShoppingCart.Interfaces;
using Simple.ShoppingCart.Products;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCartTestProject
{
    [TestClass]
    public class CartStateTests
    {
        [TestMethod]
        public void RemoveOrders_EmptyCart_ThrowsIllegalStateException()
        {
            string basket = "bread";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            Assert.ThrowsException<IllegalStateException>(() => op.RemoveOrders(basket));
        }

        [TestMethod]
        public void PayFor_EmptyCart_ThrowsIllegalStateTransition()
        {
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            // Since there is no exception for illegal state transitions I just use IllegalStateException.
            Assert.ThrowsException<IllegalStateException>(() => op.PayOrders()); 
        }

        [TestMethod]
        public void RemoveOrders_PaidForCart_ThrowsIllegalStateTransition()
        {
            string basket = "bread";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            op.ProcessOrders(basket);
            op.PayOrders();

            Assert.ThrowsException<IllegalStateException>(() => op.RemoveOrders(basket));
        }

        [TestMethod]
        public void PayFor_PaidForCart_ThrowsIllegalStateException()
        {
            string basket = "bread";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            op.ProcessOrders(basket);
            op.PayOrders();

            Assert.ThrowsException<IllegalStateException>(() => op.PayOrders());
        }

        [TestMethod]
        public void ProcessOrders_PaidForCart_ThrowsIllegalStateException()
        {
            string basket = "bread";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            op.ProcessOrders(basket);
            op.PayOrders();

            Assert.ThrowsException<IllegalStateException>(() => op.ProcessOrders("milk"));
        }
    }
}
