using SmartHomeIoTSimulator.Exceptions;

namespace SmartHomeIoTSimulator.Core;

public abstract class Sensor : IIdentifiable
{
    private static int _counter;
    private readonly List<ISensorObserver> _observers = new();

    private string _name;
    private double _value;

    public int Id { get; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Назва сенсора не може бути порожньою.");

            _name = value;
        }
    }

    public double Value
    {
        get => _value;
        protected set => _value = value;
    }

    public static int TotalSensors => _counter;

    protected Sensor() : this("Невідомий сенсор", 0) { }

    protected Sensor(string name) : this(name, 0) { }

    protected Sensor(string name, double value)
    {
        Id = ++_counter;
        _name = name;
        _value = value;
    }

    public void Attach(ISensorObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Detach(ISensorObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void Notify()
    {
        foreach (var observer in _observers)
            observer.Update(this);
    }

    public virtual void SetValue(double value)
    {
        Value = value;
        Notify();
    }

    public virtual string GetInfo()
    {
        return $"{Name}: {Value}";
    }

    public override string ToString()
    {
        return $"[Sensor #{Id}] {GetInfo()}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Sensor other && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public class TemperatureSensor : Sensor
{
    public TemperatureSensor() : base("Датчик температури", 0) { }

    public TemperatureSensor(double value) : base("Датчик температури", value) { }

    public TemperatureSensor(string name, double value) : base(name, value) { }

    public override string GetInfo()
    {
        return $"{Name}: {Value} °C";
    }
}

public class MotionSensor : Sensor
{
    public MotionSensor() : base("Датчик руху", 0) { }

    public MotionSensor(bool motionDetected) : base("Датчик руху", motionDetected ? 1 : 0) { }

    public bool IsMotionDetected => Value > 0;

    public void SetMotion(bool detected)
    {
        SetValue(detected ? 1 : 0);
    }

    public override string GetInfo()
    {
        return $"{Name}: {(IsMotionDetected ? "рух виявлено" : "руху немає")}";
    }
}

public class LightSensor : Sensor
{
    public LightSensor() : base("Датчик освітлення", 0) { }

    public LightSensor(double value) : base("Датчик освітлення", value) { }

    public override string GetInfo()
    {
        return $"{Name}: {Value} lux";
    }
}