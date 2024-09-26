
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Simple.ShoppingCart.Controller;
using Simple.ShoppingCart.Products;
using Simple.ShoppingCart.Interfaces;
using Simple.ShoppingCart.Exceptions;
// https://github.com/tomakita/Colorful.Console
using Console = Colorful.Console;
using System.Drawing;
using Figgle;
using Autofac;


namespace Simple.ShoppingCart;

class Program
{
    // for Autofac
    private static IContainer? Container { get; set; }

    /// <summary>
    /// Castle Windsor https://github.com/castleproject/Windsor is best of breed, mature Inversion of Control
    /// https://github.com/castleproject/Windsor/blob/master/docs/ioc.md container available for .NET.
    /// </summary>
    /// <returns></returns>
    private static IOrderProcessor GetOrderContainerWindsor(WindsorContainer container)
    {
        // CREATE A WINDSOR CONTAINER OBJECT AND REGISTER THE INTERFACES, AND THEIR CONCRETE IMPLEMENTATIONS
        container.Register(Component.For<IOrderProcessor>().ImplementedBy<OrderProcessor>());
        container.Register(Component.For<IShop>().ImplementedBy<Shop>());
        container.Register(Component.For<IBread>().ImplementedBy<Bread>());
        container.Register(Component.For<IButter>().ImplementedBy<Butter>());
        container.Register(Component.For<IMilk>().ImplementedBy<Milk>());

        // CREATE THE MAIN OBJECTS AND INVOKE ITS METHOD(S) AS DESIRED
        return container.Resolve<IOrderProcessor>();
    }


    /// <summary>
    /// Autofac https://github.com/autofac/ 
    /// https://autofac.org/
    /// https://autofaccn.readthedocs.io/en/latest/getting-started/index.html
    /// </summary>
    /// <returns></returns>
    private static IOrderProcessor GetOrderContainerAutofac()
    {
        // CREATE AUTFAC CONTAINER
        var builder = new ContainerBuilder();
        builder.RegisterType<OrderProcessor>().As<IOrderProcessor>();
        builder.RegisterType<Shop>().As<IShop>();
        builder.RegisterType<Bread>().As<IBread>();
        builder.RegisterType<Butter>().As<IButter>();
        builder.RegisterType<Milk>().As<IMilk>();

        // build can only be called once
        Container = builder.Build();
        return Container.Resolve<IOrderProcessor>();
    }


    /// <summary>
    /// Get a real OrderProcessor object
    /// </summary>
    /// <returns></returns>
    private static OrderProcessor GetRealOrderObject()
    {
        var bread = new Bread();
        var butter = new Butter();
        var milk = new Milk();
        return new OrderProcessor(bread, butter, milk);
    }

    /// <summary>
    /// Get an IOrderProcessor interface variable
    /// </summary>
    /// <returns></returns>
    private static IOrderProcessor GetOrderInterfaceVar()
    {
        var bread = (IBread) new Bread();
        var butter = (IButter) new Butter();
        var milk = (IMilk) new Milk();

        return new OrderProcessor(bread, butter, milk);
    }

    static void Main(string[] args)
    {
        // render '€' correct in the console output
        Console.OutputEncoding = Encoding.Default;
        var showMenu = true;

        Console.WriteLine(FiggleFonts.Standard.Render("Simple ShoppingCart"));

        // it does not matter what you use here of container, real object or interface variable
        /*
        // Castle Windsor
        var container = new WindsorContainer();
        var orderProcessor = GetOrderContainerWindsor(container);
        var shop = container.Resolve<IShop>();
        */

        /*
        // Autofac
        var orderProcessor = GetOrderContainerAutofac();
        var shop = Container.Resolve<IShop>();
        */

        /*
        // get OrderProcessor obj variable
        var orderProcessor = GetRealOrderObject();
        var shop = new Shop();
        */

        // get IOrderProcessor interface variable
        var orderProcessor = GetOrderInterfaceVar();
        var shop = (IShop) new Shop();
        

        Console.WriteLine(shop.MainText());
        Console.WriteLine("Choose 'A' to Add Product(s) To Cart", Color.Yellow);
        Console.WriteLine("Choose 'R' to Remove Product(s) From Cart", Color.Yellow);
        Console.WriteLine("Choose 'S' to Show Current Cart and Applicable Offers", Color.Yellow);
        Console.WriteLine("Choose 'P' to Pay for the Cart/Order", Color.Yellow);

        //Console.WriteLine("Choose '???' to Perform other command as Clear the Session etc.");

        Console.WriteLine("Choose 'Q' to Exit");

        while (showMenu)
        {
            Console.Write($"{Environment.NewLine}Perform your choice: ");

            var command = Console.ReadLine();
            try
            {
                switch (command)
                {
                    case "A":
                        var addToBasket = Console.ReadLine();
                        Console.WriteLine(orderProcessor.ProcessOrders(addToBasket), Color.Green);
                        break;
                    case "R":
                        var removeFromBasket = Console.ReadLine();
                        Console.WriteLine(orderProcessor.RemoveOrders(removeFromBasket), Color.Green);
                        break;
                    case "S":
                        Console.WriteLine(orderProcessor.ShowOrders(), Color.Green);
                        break;
                    case "P":
                        Console.WriteLine(orderProcessor.PayOrders(), Color.Green);
                        break;
                    case "Q":
                        showMenu = false;
                        break;
                    default:
                        Console.WriteLine("Unknown command!", Color.Red);
                        break;
                }
            }
            catch (IllegalStateException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
