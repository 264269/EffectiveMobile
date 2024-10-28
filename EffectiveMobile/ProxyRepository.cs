namespace EffectiveMobile
{
    internal class ProxyRepository : IRepository
    {
        private IRepository _repository;
        private List<Order> _orders;

        public ProxyRepository(IRepository repository)
        {
            _repository = repository;
            MySimpleLogger.GetInstance().Log($"Создание прокси для {repository}");
        }

        public bool CheckUpdate()
        {
            MySimpleLogger.GetInstance().Log($"Запрос к прокси на проверку обновления");
            return _repository.CheckUpdate();
        }

        public List<Order> ReadOrders()
        {
            MySimpleLogger.GetInstance().Log($"Запрос к прокси на чтение данных");
            if (CheckUpdate())
                _orders = _repository.ReadOrders();
            return _orders;
        }

        public void WriteOrders(List<Order> orders)
        {
            MySimpleLogger.GetInstance().Log($"Запрос к прокси на запись данных");
            _repository.WriteOrders(orders);
        }
    }
}
