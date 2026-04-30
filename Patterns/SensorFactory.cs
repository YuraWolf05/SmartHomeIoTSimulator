using SmartHomeIoTSimulator.Core;
using SmartHomeIoTSimulator.Exceptions;

namespace SmartHomeIoTSimulator.Patterns;

public static class SensorFactory
{
    public static Sensor Create(string type, double value)
    {
        return type.ToLower() switch
        {
            "temperature" => new TemperatureSensor(value),
            "motion" => new MotionSensor(value > 0),
            "light" => new LightSensor(value),
            _ => throw new SensorCreationException($"Невідомий тип сенсора: {type}")
        };
    }
}