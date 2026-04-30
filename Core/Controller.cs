namespace SmartHomeIoTSimulator.Core;

public class SmartController : ISensorObserver
{
    private readonly List<ISmartDevice> _devices = new();
    private readonly List<IAutomationStrategy> _strategies = new();

    public string Name { get; }

    public SmartController(string name)
    {
        Name = name;
    }

    public void AddDevice(ISmartDevice device)
    {
        _devices.Add(device);
    }

    public void AddStrategy(IAutomationStrategy strategy)
    {
        _strategies.Add(strategy);
    }

    public void Update(Sensor sensor)
    {
        Console.WriteLine($"\nКонтролер «{Name}» отримав сигнал: {sensor.GetInfo()}");

        foreach (var strategy in _strategies)
            strategy.Execute(sensor, _devices);
    }

    public IEnumerable<ISmartDevice> GetDevices()
    {
        return _devices;
    }
}