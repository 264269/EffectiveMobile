using System.Globalization;

namespace EffectiveMobile
{
    internal class Requester(IRepository repository)
    {
        IRepository repository = repository;

        public void RunWithParams(string orderFile, string? district, string? date)
        {
            MySimpleLogger.GetInstance().Log($"Запуск фильтрации заказов: orderFile={orderFile}, district={district}, date={date}");
            List<Order> orders = repository.ReadOrders();
            List<Order> result = [];
            foreach (Order order in orders)
            {
                if (!CheckDistrict(district, order) || !CheckDate(date, order))
                    continue;
                result.Add(order);
            }

            IRepository outRepository = new FileRepository(orderFile);
            outRepository.WriteOrders(result);
        }

        public static bool CheckDistrict(string? district, Order order)
        {
            if (district == null)
                return false;
            try
            {
                return order.District == int.Parse(district);
            }
            catch { MySimpleLogger.GetInstance().Log("Не удалось распознать значение параметра _cityDistrict"); }
            return false;
        }

        public static bool CheckDate(string? date, Order order)
        {
            if (date == null)
                return false;
            try
            {
                DateTime dateParsed = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                bool from = order.DeliveryTime.CompareTo(dateParsed) >= 0;
                bool to = order.DeliveryTime.CompareTo(dateParsed.AddMinutes(30)) <= 0;
                return from && to;
            }
            catch { MySimpleLogger.GetInstance().Log("Не удалось распознать значение параметра _firstDeliveryDateTime"); }
            return false;
        }
    }
}
