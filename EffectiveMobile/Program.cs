// See https://aka.ms/new-console-template for more information
using EffectiveMobile;
using System.Globalization;
using System.Configuration;

IRepository realRepository = new FileRepository("../../../data/orders.txt");
IRepository proxy = new ProxyRepository(realRepository);
Requester requester = new Requester(proxy, args);
requester.Run();

//var logger = NLogBuilder

//foreach (string item in ConfigurationManager.AppSettings)
//{
//    Console.WriteLine($"{item} = {ConfigurationManager.AppSettings[item]}");
//}
//foreach (var order in proxy.ReadOrders())
//{
//    Console.WriteLine(order);
//}

//foreach (var order in proxy.ReadOrders())
//{
//    Console.WriteLine(order);
//}




