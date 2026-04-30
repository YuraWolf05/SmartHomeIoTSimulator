using SmartHomeIoTSimulator.Core;

namespace SmartHomeIoTSimulator.Patterns;

public abstract class DeviceDecorator : ISmartDevice
{
    protected readonly ISmartDevice Device;

    public int Id => Device.Id;
    public string Name => Device.Name;
    public bool IsOn => Device.IsOn;

    protected DeviceDecorator(ISmartDevice device)
    {
        Device = device;
    }

    public virtual void TurnOn() => Device.TurnOn();

    public virtual void TurnOff() => Device.TurnOff();

    public virtual string GetStatus() => Device.GetStatus();
}

public class EcoModeDeviceDecorator : DeviceDecorator
{
    public int EcoLevel { get; private set; }

    public EcoModeDeviceDecorator(ISmartDevice device, int ecoLevel) : base(device)
    {
        EcoLevel = ecoLevel;
    }

    public void SetEcoLevel(int level)
    {
        EcoLevel = level;
    }

    public override string GetStatus()
    {
        return $"{base.GetStatus()} | Екорежим: рівень {EcoLevel}";
    }
}

public class ScheduledDeviceDecorator : DeviceDecorator
{
    public string Schedule { get; private set; }
    public bool IsScheduleEnabled { get; private set; }

    public ScheduledDeviceDecorator(ISmartDevice device, string schedule) : base(device)
    {
        Schedule = schedule;
        IsScheduleEnabled = true;
    }

    public void SetSchedule(string schedule)
    {
        Schedule = string.IsNullOrWhiteSpace(schedule)
            ? "розклад не задано"
            : schedule;
    }

    public void EnableSchedule()
    {
        IsScheduleEnabled = true;
    }

    public void DisableSchedule()
    {
        IsScheduleEnabled = false;
    }

    public override string GetStatus()
    {
        string state = IsScheduleEnabled ? "увімкнено" : "вимкнено";
        return $"{base.GetStatus()} | Розклад: {Schedule} ({state})";
    }
}