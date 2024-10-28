// See https://aka.ms/new-console-template for more information
using EffectiveMobile;
using System.Globalization;
using System.Configuration;

try
{
    //  initialize custom logger
    string defaultDeliveryLog = MySimpleLogger.DEFAULT_DELIVERY_LOG;
    string deliveryLog = GetParameter("_deliveryLog", false) ?? defaultDeliveryLog;
    deliveryLog = deliveryLog.EndsWith(".txt") ? deliveryLog : defaultDeliveryLog;
    deliveryLog = IsValidPath(deliveryLog) ? deliveryLog : defaultDeliveryLog;

    MySimpleLogger.CreateInstance(deliveryLog);
    MySimpleLogger.GetInstance().Log($"---- Запуск программы {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} ----");

    //  initialize default orders file
    string defaultDeliveryData = "./data/orders.txt";
    string deliveryData = GetParameter("_deliveryData", false) ?? defaultDeliveryData;
    deliveryData = deliveryData.EndsWith(".txt") ? deliveryData : defaultDeliveryData;
    deliveryLog = IsValidPath(deliveryLog) ? deliveryData : defaultDeliveryData;

    IRepository realRepository = new FileRepository(deliveryData);
    IRepository proxy = new ProxyRepository(realRepository);

    Requester requester = new(proxy);

    //  initialize default result file
    string defaultDeliveryOrder = "./data/result.txt";
    string deliveryOrder = GetParameter("_deliveryOrder", false) ?? defaultDeliveryOrder;
    deliveryOrder = deliveryOrder.EndsWith(".txt") ? deliveryOrder : defaultDeliveryOrder;
    deliveryOrder = IsValidPath(deliveryOrder) ? deliveryOrder : defaultDeliveryOrder;

    //  get params
    string? cityDistrict = GetParameter("_cityDistrict", false);
    string? firstDeliveryDateTime = GetParameter("_firstDeliveryDateTime", false);

    requester.RunWithParams(deliveryOrder, cityDistrict, firstDeliveryDateTime);
}
catch (Exception e) { Console.WriteLine(e.Message + " : " + e.StackTrace); }

string? GetParameter(string argName, bool isFlag)
{
    string? result = null;
    for (int i = 0; i < args.Length; i++)
    {
        if (args[i].Equals(argName))
        {
            if (isFlag)
                result = args[i];
            else
                result = (++i < args.Length) ? args[i] : null;
            break;
        }
    }
    return result;
}

bool IsValidPath(string path, bool allowRelativePaths = false)
{
    bool isValid = true;

    try
    {
        string fullPath = Path.GetFullPath(path);

        if (allowRelativePaths)
            isValid = Path.IsPathRooted(path);
        else
        {
            string root = Path.GetPathRoot(path);
            isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
        }
    }
    catch { isValid = false; }

    return isValid;
}




