using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectiveMobile
{
    internal class Order
    {
        private readonly int _id;
        private readonly double _weight;
        private readonly int _district;
        private readonly DateTime _deliveryTime;

        public int Id { get { return _id; } }
        public double Weight { get { return _weight; } }
        public int District { get { return _district; } }
        public DateTime DeliveryTime { get { return _deliveryTime; } }

        private Order(OrderBuilder builder)
        {
            MySimpleLogger.GetInstance().Log($"Создание Order из {builder}");
            _id = builder.Id;
            _weight = builder.Weight;
            _district = builder.District;
            _deliveryTime = builder.DeliveryTime;
        }

        public class OrderBuilder
        {
            public int Id { get; private set; }
            public double Weight { get; private set; }
            public int District { get; private set; }
            public DateTime DeliveryTime { get; private set; }

            public OrderBuilder SetId(int id)
            {
                Id = id;
                return this;
            }

            public OrderBuilder SetWeight(double weight)
            {
                Weight = weight;
                return this;
            }

            public OrderBuilder SetDistrict(int district)
            {
                District = district;
                return this;
            }

            public OrderBuilder SetDeliveryTime(DateTime deliveryTime)
            {
                DeliveryTime = deliveryTime;
                return this;
            }

            public Order Build()
            {
                return new Order(this);
            }

            public override string? ToString()
            {
                return $"OrderBuilder: id={Id}, weight={Weight.ToString("N3").Replace(",", ".")}, district = {District}, deliveryTime = {DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")}";
            }
        }

        public override string? ToString()
        {
            //TODO: change Replace to Locale or smth
            return $"Order#{Id}: weight = {Weight.ToString("N3").Replace(",", ".")}, district = {District}, deliveryTime = {DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")}";
        }

        public string ToStringFileRepresentation()
        {
            //TODO: change Replace to Locale or smth
            return $"{Id},{Weight.ToString("N3").Replace(",",".")},{District},{DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")}";
        }
    }
}
