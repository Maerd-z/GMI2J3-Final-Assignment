using Simple.ShoppingCart.Controller;
using Simple.ShoppingCart.Interfaces;
using Simple.ShoppingCart.Products;
using Moq;
using System.Text;
using Simple.ShoppingCart.Exceptions;
using Autofac.Core;

namespace SimpleCartTestProject
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void ProcessOrders_1Bread_NoDiscounts()
        {
            string basket = "bread";
            string expected = "€2,00";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            string result = op.ProcessOrders(basket);

            Assert.IsTrue(result.Contains(expected));
        }

        [TestMethod]
        public void ProcessOrders_2Butter1Bread_CorrectDiscount()
        {
            string basket = "butter butter bread";
            string expected = "€4,60";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            string result = op.ProcessOrders(basket);

            Assert.IsTrue(result.Contains(expected));
        }

        [TestMethod]
        public void ProcessOrders_4Butter2Bread_Only1BreadIsDiscounted()
        {
            string basket = "butter butter butter butter bread bread";
            string expected = "€10,2";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            string result = op.ProcessOrders(basket);

            Assert.IsTrue(result.Contains(expected));

        }

        [TestMethod]
        public void ProcessOrders_4Milk_4thIsFree()
        {
            string basket = "milk milk milk milk";
            string expected = "€3,45";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            string result = op.ProcessOrders(basket);

            Assert.IsTrue(result.Contains(expected));
        }

        [TestMethod]
        public void ProcessOrders_12Milk_Only4thIsFree()
        {
            string basket = "milk milk milk milk milk milk milk milk milk milk milk milk";
            string expected = "€12,65";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            string result = op.ProcessOrders(basket);

            Assert.IsTrue(result.Contains(expected));
        }

        [TestMethod]
        public void ProcessOrders_MixedBasket_NoDiscount()
        {
            string basket = "butter milk milk milk bread";
            string expected = "€7,25";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            string result = op.ProcessOrders(basket);

            Assert.IsTrue(result.Contains(expected));
        }

        [TestMethod]
        public void ProcessOrders_MixedBasketWithOfferDeal_CanBeDiscounted()
        {
            string basket = "butter butter milk milk milk bread";
            string expected = "€8,05";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            string result = op.ProcessOrders(basket);

            Assert.IsTrue(result.Contains(expected));
        }

        [TestMethod]
        public void RemoveOrders_1Bread_CorrectCost()
        {
            string basket = "bread";
            string expected = "€0,00";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            op.ProcessOrders(basket);
            string result = op.RemoveOrders(basket);

            Assert.IsTrue(result.Contains(expected));
        }

        [TestMethod]
        public void RemoveOrders_1Bread_Remove2Bread_ThrowsIllegalTransitionException()
        {
            string basket = "bread";
            string illegalRemoval = "bread bread";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());
            op.ProcessOrders(basket);

            Assert.ThrowsException<IllegalStateException>(() => op.RemoveOrders(illegalRemoval));
        }

        [TestMethod]
        public void RemoveOrders_2Bread_RemovesOne()
        {
            string basket = "bread";
            string expected = "€2,00";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            op.ProcessOrders("bread bread");
            string result = op.RemoveOrders(basket);

            Assert.IsTrue(result.Contains(expected));
        }

        [TestMethod]
        public void RemoveOrders_2Butter1Bread_DiscountIsRemovedCorrectly()
        {
            string basket = "bread";
            string expected = "€3,60";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            op.ProcessOrders("butter butter bread");
            string result = op.RemoveOrders(basket);

            Assert.IsTrue(result.Contains(expected));
        }

        [TestMethod]
        public void RemoveAndProcessOrders_DiscountIsWipedWithEmptyCart_NoDiscount()
        {
            string basket = "milk milk milk milk";
            string expected = "Paid! €2,30";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            op.ProcessOrders(basket);
            op.RemoveOrders(basket);
            op.ProcessOrders("milk milk");
            string actual = op.PayOrders();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveAndProcessOrders_DiscountIsWiped_WhenRemovingConstituentItem_NoDiscount()
        {
            string basket = "milk milk milk milk";
            string expected = "Paid! €3,45";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            op.ProcessOrders(basket);
            op.RemoveOrders("milk milk");
            op.ProcessOrders("milk");
            string actual = op.PayOrders();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveAndProcessOrders_RepeatedAddAndRemoval_MoneyPrinter()
        {
            string basket = "milk milk milk milk";
            string expected = "Paid! €3,45";
            IOrderProcessor op = new OrderProcessor(new Bread(), new Butter(), new Milk());

            for (int i = 0; i < 4; i++)
            {
                op.ProcessOrders(basket);
                op.RemoveOrders(basket);
            }
            op.ProcessOrders(basket);
            string actual = op.PayOrders();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SaveToDB_2Bread()
        {
            KeyValuePair<string, string> data = new KeyValuePair<string, string>("123","123");
            KeyValuePair<string, string> mockData = new KeyValuePair<string, string>("bread", "2");
            Mock<IDB> mock = new Mock<IDB>();
            mock.Setup(x => x.SendToDB(It.IsAny<KeyValuePair<string,string>>())).Callback<KeyValuePair<string,string>>((cartData) => data = cartData);

            mock.Object.SendToDB(mockData);

            mock.Verify(m => m.SendToDB(It.IsAny<KeyValuePair<string,string>>()), Times.Once);
            Assert.AreEqual(data, mockData);
        }
    }
}