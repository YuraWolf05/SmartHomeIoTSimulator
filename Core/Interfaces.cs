namespace SmartHomeIoTSimulator.Core;

public interface IIdentifiable
{
    int Id { get; }
    string Name { get; }
}

public interface ISensorObserver
{
    void Update(Sensor sensor);
}

public interface ISmartDevice : IIdentifiable
{
    bool IsOn { get; }
    void TurnOn();
    void TurnOff();
    string GetStatus();
}

public interface IAutomationStrategy
{
    void Execute(Sensor sensor, IEnumerable<ISmartDevice> devices);
}