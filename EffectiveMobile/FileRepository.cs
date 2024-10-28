using System.Globalization;

namespace EffectiveMobile
{
    internal class FileRepository : IRepository
    {
        private readonly string _file;
        private DateTime _lastPulled;

        public FileRepository(string filePath)
        {
            _file = filePath;
            CreateFileIfNecessary();
            MySimpleLogger.GetInstance().Log($"Создание FileRespoitory({_file})");
        }

        public bool CheckUpdate()
        {
            MySimpleLogger.GetInstance().Log($"Проверка изменений в {_file}");
            try { return File.GetLastWriteTime(_file) != _lastPulled; }
            catch (Exception e) { MySimpleLogger.GetInstance().Log($"При проверке файла произошла ошибка: {e.ToString}"); }
            return false;
        }

        public List<Order> ReadOrders()
        {
            MySimpleLogger.GetInstance().Log($"Считывание данных из {_file}");
            var orders = new List<Order>();
            List<int> ids = new List<int>();
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

                            if (fields.Length != 4)
                                throw new Exception($"Неверное количество значений в строке: {fields.Length} (ожидаемое количество: 4).");

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

                            if (ids.Contains(order.Id))
                                throw new Exception($"Повторение строки с id = {order.Id} (для {order})");
                            orders.Add(order);
                            ids.Add(order.Id);
                        }
                        catch (Exception e) { MySimpleLogger.GetInstance().Log($"Произошла ошибка при обработке строки: {e.Message}"); }
                    }
                }
            }
            catch (Exception e) { MySimpleLogger.GetInstance().Log($"Произошла ошибка при чтении файла: {e.Message}"); }

            return orders;
        }


        public void WriteOrders(List<Order> orders)
        {
            MySimpleLogger.GetInstance().Log($"Запись данных в {_file}");
            try
            {
                List<string> OrderFileRepresentationList = new List<string>();

                foreach (Order order in orders)
                    OrderFileRepresentationList.Add(order.ToStringFileRepresentation());

                File.WriteAllLines(_file, OrderFileRepresentationList);
            }
            catch (Exception e) { MySimpleLogger.GetInstance().Log($"Произошла ошибка при записи данных: ({e.Message})"); }
        }

        public void CreateFileIfNecessary()
        {
            if (File.Exists(_file))
                return;

            string? dir = Path.GetDirectoryName(_file);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
        }

        public override string? ToString()
        {
            return _file;
        }
    }
}