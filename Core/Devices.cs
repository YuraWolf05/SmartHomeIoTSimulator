using SmartHomeIoTSimulator.Exceptions;

namespace SmartHomeIoTSimulator.Core;

public abstract class SmartDevice : ISmartDevice
{
    private static int _counter;

    private string _name;
    private bool _isOn;

    public int Id { get; }

    public string Name
    {
        get => _name;
        protected set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Назва пристрою не може бути порожньою.");

            _name = value;
        }
    }

    public bool IsOn
    {
        get => _isOn;
        private set => _isOn = value;
    }

    public static int TotalDevices => _counter;

    protected SmartDevice() : this("Невідомий пристрій", false) { }

    protected SmartDevice(string name) : this(name, false) { }

    protected SmartDevice(string name, bool isOn)
    {
        Id = ++_counter;
        _name = name;
        _isOn = isOn;
    }

    public virtual void TurnOn()
    {
        IsOn = true;
    }

    public virtual void TurnOff()
    {
        IsOn = false;
    }

    public virtual string GetStatus()
    {
        return $"{Name}: {(IsOn ? "увімкнено" : "вимкнено")}";
    }

    public override string ToString()
    {
        return $"[Device #{Id}] {GetStatus()}";
    }

    public override bool Equals(object? obj)
    {
        return obj is SmartDevice other && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public class Lamp : SmartDevice
{
    public Lamp() : base("Лампа") { }

    public Lamp(string name) : base(name) { }

    public Lamp(string name, bool isOn) : base(name, isOn) { }

    public override string GetStatus()
    {
        return $"Освітлення «{Name}»: {(IsOn ? "увімкнено" : "вимкнено")}";
    }
}

public class Heater : SmartDevice
{
    public Heater() : base("Обігрівач") { }

    public Heater(string name) : base(name) { }

    public Heater(string name, bool isOn) : base(name, isOn) { }

    public override string GetStatus()
    {
        return $"Опалення «{Name}»: {(IsOn ? "працює" : "вимкнено")}";
    }
}

public class AirConditioner : SmartDevice
{
    public AirConditioner() : base("Кондиціонер") { }

    public AirConditioner(string name) : base(name) { }

    public override string GetStatus()
    {
        return $"Кондиціонер «{Name}»: {(IsOn ? "охолоджує" : "вимкнено")}";
    }
}