namespace SmartHomeIoTSimulator.Persistence;

public class SmartHomeState
{
    public double Temperature { get; set; }
    public bool MotionDetected { get; set; }
    public double LightLevel { get; set; }

    public bool LampOn { get; set; }
    public bool HeaterOn { get; set; }
    public bool ConditionerOn { get; set; }

    public string LampSchedule { get; set; } = "18:00 - 23:00";
    public bool IsScheduleEnabled { get; set; }

    public int HeaterEcoLevel { get; set; }
}