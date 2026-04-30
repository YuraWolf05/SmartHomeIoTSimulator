using SmartHomeIoTSimulator.Core;

namespace SmartHomeIoTSimulator.Patterns;

public class TemperatureAutomationStrategy : IAutomationStrategy
{
    public void Execute(Sensor sensor, IEnumerable<ISmartDevice> devices)
    {
        if (sensor is not TemperatureSensor temperatureSensor)
            return;

        var heater = devices.FirstOrDefault(d => d.Name.Contains("Обігрівач"));

        if (heater == null)
            return;

        if (temperatureSensor.Value < 20)
            heater.TurnOn();
        else
            heater.TurnOff();
    }
}

public class MotionAutomationStrategy : IAutomationStrategy
{
    public void Execute(Sensor sensor, IEnumerable<ISmartDevice> devices)
    {
        if (sensor is not MotionSensor motionSensor)
            return;

        var lamp = devices.FirstOrDefault(d => d.Name.Contains("Лампа"));

        if (lamp == null)
            return;

        if (motionSensor.IsMotionDetected)
            lamp.TurnOn();
        else
            lamp.TurnOff();
    }
}

public class LightAutomationStrategy : IAutomationStrategy
{
    public void Execute(Sensor sensor, IEnumerable<ISmartDevice> devices)
    {
        if (sensor is not LightSensor lightSensor)
            return;

        var lamp = devices.FirstOrDefault(d => d.Name.Contains("Лампа"));

        if (lamp == null)
            return;

        if (lightSensor.Value < 40)
            lamp.TurnOn();
        else
            lamp.TurnOff();
    }
}