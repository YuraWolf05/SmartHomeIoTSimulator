using System.Text.Json;
using System.Xml.Serialization;

namespace SmartHomeIoTSimulator.Persistence;

public class SmartHomeStateService
{
    public void SaveToJson(SmartHomeState state, string path)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(state, options);
        File.WriteAllText(path, json);
    }

    public SmartHomeState LoadFromJson(string path)
    {
        string json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<SmartHomeState>(json)
               ?? throw new InvalidOperationException("Не вдалося прочитати JSON-файл.");
    }

    public void SaveToXml(SmartHomeState state, string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SmartHomeState));

        using FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, state);
    }

    public SmartHomeState LoadFromXml(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SmartHomeState));

        using FileStream stream = new FileStream(path, FileMode.Open);

        return serializer.Deserialize(stream) as SmartHomeState
               ?? throw new InvalidOperationException("Не вдалося прочитати XML-файл.");
    }
}