namespace SmartHomeIoTSimulator.Exceptions;

public class SmartHomeException : Exception
{
    public SmartHomeException(string message) : base(message) { }
}

public class ValidationException : SmartHomeException
{
    public ValidationException(string message) : base(message) { }
}

public class DeviceCreationException : SmartHomeException
{
    public DeviceCreationException(string message) : base(message) { }
}

public class SensorCreationException : SmartHomeException
{
    public SensorCreationException(string message) : base(message) { }
}