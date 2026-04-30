using SmartHomeIoTSimulator.Core;

namespace SmartHomeIoTSimulator.Patterns;

public class SmartHomeFacade
{
    private readonly TemperatureSensor _temperatureSensor;
    private readonly MotionSensor _motionSensor;
    private readonly LightSensor _lightSensor;
    private readonly SmartController _controller;

    public SmartHomeFacade(
        TemperatureSensor temperatureSensor,
        MotionSensor motionSensor,
        LightSensor lightSensor,
        SmartController controller)
    {
        _temperatureSensor = temperatureSensor;
        _motionSensor = motionSensor;
        _lightSensor = lightSensor;
        _controller = controller;

        _temperatureSensor.Attach(_controller);
        _motionSensor.Attach(_controller);
        _lightSensor.Attach(_controller);
    }

    public void ChangeTemperature(double value)
    {
        _temperatureSensor.SetValue(value);
    }

    public void DetectMotion(bool detected)
    {
        _motionSensor.SetMotion(detected);
    }

    public void ChangeLightLevel(double value)
    {
        _lightSensor.SetValue(value);
    }

    public void ShowStatus()
    {
        Console.WriteLine("\nПоточний стан системи:");

        foreach (var device in _controller.GetDevices())
            Console.WriteLine(device.GetStatus());
    }
}