using System.Globalization;

namespace EffectiveMobile
{
    internal class Requester(IRepository repository, string[] args)
    {
        IRepository repository = repository;
        string[] args = args;

        public void Run()
        {
            RunWithParams();
            //try
            //{
            //    string? isInteractive = GetParameter("--i", true);
            //    if (isInteractive != null) { RunInteractive(); } else { RunWithParams(); }
            //}
            //catch (Exception e) { Console.WriteLine(e.Message); }
        }

        //public void RunInteractive()
        //{

        //}

        public void RunWithParams()
        {
            string? orderFileName = GetOrderFile() ?? throw new ArgumentException("Необходимо указать путь к файлу с результатом (_deliveryOrder)");
            string? logFileName = GetOrderFile() ?? throw new ArgumentException("Необходимо указать путь к файлу с результатом (_deliveryOrder)");
            int? district = GetDistrict();
            DateTime? date = GetDate();

            List<Order> orders = repository.ReadOrders();
            List<Order> result = [];
            foreach (Order order in orders)
            {
                if (district.HasValue)
                {
                    bool sameDistrict = order.District.Equals(district.Value);
                    if (!sameDistrict) continue;
                }
                if (date.HasValue)
                {
                    bool from = order.DeliveryTime.CompareTo(date.Value) >= 0;
                    bool to = order.DeliveryTime.CompareTo(date.Value.AddMinutes(30)) <= 0;
                    bool fromAndTo = from && to;
                    if (!fromAndTo) continue;
                }
                result.Add(order);
            }

            IRepository outRepository = new FileRepository(orderFileName);
            outRepository.WriteOrders(result);
            foreach (Order order in result) { Console.WriteLine(order.ToString()); }
        }

        public int? GetDistrict()
        {
            string argKey = "_cityDistrict";
            string? argValue = GetParameter(argKey, false);
            if (argValue != null)
            {
                try { return int.Parse(argValue); }
                catch (Exception e) { Console.WriteLine($"Не удалось распознать значение параметра {argKey} : {argValue}"); }
            }
            return null;
        }

        public DateTime? GetDate()
        {
            string argKey = "_firstDeliveryDateTime";
            string? argValue = GetParameter(argKey, false);
            if (argValue != null)
            {
                try { return DateTime.ParseExact(argValue, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); }
                catch (Exception e) { Console.WriteLine($"Не удалось распознать значение параметра {argKey} : {argValue}"); }
            }
            return null;
        }

        public string? GetOrderFile()
        {
            string argKey = "_deliveryOrder";
            string? argValue = GetParameter(argKey, false);
            if (argValue != null)
            {
                return argValue;
            }
            return null;
        }

        public string? GetLogFile()
        {
            string argKey = "_deliveryLog";
            string? argValue = GetParameter(argKey, false);
            if (argValue != null)
            {
                return argValue;
            }
            return null;
        }

        private string? GetParameter(string argName, bool isFlag)
        {
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Equals(argName))
                    {
                        return isFlag ? args[i] : args[++i];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine($"Не удалось найти параметр {argName}");
            return null;
        }
    }
}
