using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectiveMobile
{
    internal interface IRepository
    {
        public List<Order> ReadOrders();
        public bool CheckUpdate();
        public void WriteOrders(List<Order> orders);
    }
}
