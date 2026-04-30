using SmartHomeIoTSimulator.UI;

namespace SmartHomeIoTSimulator;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}