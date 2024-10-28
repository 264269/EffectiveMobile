using System.Globalization;

namespace EffectiveMobile
{
    internal class FileRepository(string FilePath) : IRepository
    {
        private readonly string _file = FilePath;
        private DateTime _lastPulled;

        public bool CheckUpdate()
        {
            try { return File.GetLastWriteTime(_file) != _lastPulled; }
            catch (Exception e) {
                Console.WriteLine($"При проверке файла произошла ошибка: {e.ToString}"); }
            return false;
        }

        public List<Order> ReadOrders()
        {
            var orders = new List<Order>();
            _lastPulled = File.GetLastWriteTime(_file);

            try
            {
                using (var reader = new StreamReader(_file))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            var fields = line.Split(',');

                            if (fields.Length != 4) { throw new Exception($"Неверное количество значений в строке: {fields.Length} (ожидаемое количество: 4)."); }

                            int id = int.Parse(fields[0]);
                            double weight = double.Parse(fields[1], CultureInfo.InvariantCulture);
                            int district = int.Parse(fields[2]);
                            DateTime deliveryTime = DateTime.ParseExact(fields[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            var order = new Order.OrderBuilder()
                                .SetId(id)
                                .SetWeight(weight)
                                .SetDistrict(district)
                                .SetDeliveryTime(deliveryTime)
                                .Build();

                            orders.Add(order);
                        }
                        catch (Exception ex) { Console.WriteLine($"Произошла ошибка при чтении файла: {ex.Message}"); }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Произошла ошибка при чтении файла: {ex.Message}"); }

            return orders;
        }


        public void WriteOrders(List<Order> orders)
        {
            try
            {
                List<string> OrderFileRepresentationList = new List<string>();
                foreach (Order order in orders)
                {
                    OrderFileRepresentationList.Add(order.ToStringFileRepresentation());
                }
                File.WriteAllLines(_file, OrderFileRepresentationList);
            } catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

    }
}