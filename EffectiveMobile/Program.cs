// See https://aka.ms/new-console-template for more information
using EffectiveMobile;
using System.Globalization;
using System.Configuration;


try
{
    string deliveryLog = GetParameter("_deliveryLog", false) ?? "./log/log.txt";
    deliveryLog = deliveryLog.EndsWith(".txt") ? deliveryLog : "./log/log.txt";

    MySimpleLogger.CreateInstance(deliveryLog);
    MySimpleLogger.GetInstance().Log($"---- Запуск программы {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} ----");

    string? deliveryData = GetParameter("_deliveryData", false) ?? "./data/orders.txt";
    deliveryData = deliveryData.EndsWith(".txt") ? deliveryData : "./data/orders.txt";

    IRepository realRepository = new FileRepository(deliveryData);
    IRepository proxy = new ProxyRepository(realRepository);

    Requester requester = new(proxy);

    string deliveryOrder = GetParameter("_deliveryOrder", false) ?? "./data/result.txt";
    deliveryOrder = deliveryOrder.EndsWith(".txt") ? deliveryOrder : "./data/result.txt";

    string? cityDistrict = GetParameter("_cityDistrict", false);
    string? firstDeliveryDateTime = GetParameter("_firstDeliveryDateTime", false);

    requester.RunWithParams(deliveryOrder, cityDistrict, firstDeliveryDateTime);
}
catch (Exception e) { Console.WriteLine(e.Message + " : " + e.StackTrace); }

string? GetParameter(string argName, bool isFlag)
{
    try
    {
        for (int i = 0; i < args.Length; i++)
            if (args[i].Equals(argName))
                return isFlag ? args[i] : args[++i];
    }
    catch (Exception e) { Console.WriteLine(e.Message); }
    Console.WriteLine($"Не удалось найти параметр {argName}");
    return null;
}




