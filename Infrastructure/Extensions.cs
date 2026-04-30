using SmartHomeIoTSimulator.Core;

namespace SmartHomeIoTSimulator.Infrastructure;

public static class Extensions
{
    public static IEnumerable<ISmartDevice> OnlyEnabled(this IEnumerable<ISmartDevice> devices)
    {
        return devices.Where(d => d.IsOn);
    }

    public static void PrintAllStatuses(this IEnumerable<ISmartDevice> devices)
    {
        foreach (var device in devices)
            Console.WriteLine(device.GetStatus());
    }

    public static double AverageSensorValue(this IEnumerable<Sensor> sensors)
    {
        return sensors.Average(s => s.Value);
    }
}