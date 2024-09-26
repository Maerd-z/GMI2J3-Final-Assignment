using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.ShoppingCart.Interfaces
{
    public interface IDB
    {
        void SendToDB(KeyValuePair<string, string> cartData);
    }
}
