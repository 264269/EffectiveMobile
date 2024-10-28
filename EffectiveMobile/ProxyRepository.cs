namespace EffectiveMobile
{
    internal class ProxyRepository(IRepository repository) : IRepository
    {
        private IRepository _repository = repository;
        private List<Order> _orders;

        public bool CheckUpdate()
        {
            return _repository.CheckUpdate();
        }

        public List<Order> ReadOrders()
        {
            if (CheckUpdate())
            {
                Console.WriteLine("From repository:");
                _orders = _repository.ReadOrders();
            }
            else { Console.WriteLine("From proxy:"); }
            return _orders;
        }

        public void WriteOrders(List<Order> orders)
        {
            _repository.WriteOrders(orders);
        }
    }
}
