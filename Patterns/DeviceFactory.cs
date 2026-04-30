using SmartHomeIoTSimulator.Core;
using SmartHomeIoTSimulator.Exceptions;

namespace SmartHomeIoTSimulator.Patterns;

public static class DeviceFactory
{
    public static ISmartDevice Create(string type, string name)
    {
        return type.ToLower() switch
        {
            "lamp" => new Lamp(name),
            "heater" => new Heater(name),
            "airconditioner" => new AirConditioner(name),
            _ => throw new DeviceCreationException($"Невідомий тип пристрою: {type}")
        };
    }
}