using Moq;
using SmartHomeIoTSimulator.Core;
using SmartHomeIoTSimulator.Infrastructure;
using SmartHomeIoTSimulator.Patterns;
using SmartHomeIoTSimulator.Persistence;

namespace SmartHomeIoTSimulator.Tests;

public class SmartHomeTests
{
    [Fact]
    public void TemperatureSensor_SetValue_ShouldChangeValue()
    {
        var sensor = new TemperatureSensor(20);

        sensor.SetValue(25);

        Assert.Equal(25, sensor.Value);
    }

    [Fact]
    public void Observer_ShouldBeNotified_WhenSensorValueChanged()
    {
        var sensor = new TemperatureSensor(20);
        var observerMock = new Mock<ISensorObserver>();

        sensor.Attach(observerMock.Object);
        sensor.SetValue(18);

        observerMock.Verify(o => o.Update(sensor), Times.Once);
    }

    [Fact]
    public void TemperatureStrategy_ShouldTurnOnHeater_WhenTemperatureIsLow()
    {
        var sensor = new TemperatureSensor(17);
        ISmartDevice heater = new Heater("Обігрівач у спальні");

        var devices = new List<ISmartDevice> { heater };
        var strategy = new TemperatureAutomationStrategy();

        strategy.Execute(sensor, devices);

        Assert.True(heater.IsOn);
    }

    [Fact]
    public void Repository_ShouldStoreAndReturnSensors()
    {
        var repository = new Repository<Sensor>();
        var sensor = new LightSensor(70);

        repository.Add(sensor);

        Assert.Single(repository.GetAll());
        Assert.Equal(sensor, repository.GetById(sensor.Id));
    }

    [Fact]
    public void JsonSerialization_ShouldSaveAndLoadState()
    {
        var service = new SmartHomeStateService();

        var state = new SmartHomeState
        {
            Temperature = 21,
            MotionDetected = true,
            LightLevel = 35,
            LampOn = true,
            HeaterOn = false,
            ConditionerOn = true,
            LampSchedule = "18:00 - 23:00",
            IsScheduleEnabled = true,
            HeaterEcoLevel = 2
        };

        string path = Path.Combine(Path.GetTempPath(), "smart_home_test.json");

        service.SaveToJson(state, path);
        SmartHomeState loaded = service.LoadFromJson(path);

        Assert.Equal(state.Temperature, loaded.Temperature);
        Assert.Equal(state.MotionDetected, loaded.MotionDetected);
        Assert.Equal(state.LampSchedule, loaded.LampSchedule);
        Assert.Equal(state.HeaterEcoLevel, loaded.HeaterEcoLevel);
    }
}