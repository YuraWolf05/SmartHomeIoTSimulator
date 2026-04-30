using SmartHomeIoTSimulator.Core;
using SmartHomeIoTSimulator.Infrastructure;
using SmartHomeIoTSimulator.Patterns;

namespace SmartHomeIoTSimulator.UI;

public class MainForm : Form
{
    private readonly TemperatureSensor _temperatureSensor;
    private readonly MotionSensor _motionSensor;
    private readonly LightSensor _lightSensor;
    private readonly SmartController _controller;
    private readonly SmartHomeFacade _facade;

    private readonly Repository<Sensor> _sensorRepository = new();
    private readonly Repository<ISmartDevice> _deviceRepository = new();

    private readonly ScheduledDeviceDecorator _lamp;
    private readonly EcoModeDeviceDecorator _heater;
    private readonly ISmartDevice _conditioner;

    private TextBox _outputBox = null!;
    private NumericUpDown _temperatureInput = null!;
    private NumericUpDown _lightInput = null!;
    private NumericUpDown _ecoInput = null!;
    private CheckBox _motionCheckBox = null!;
    private TextBox _scheduleInput = null!;

    private Button _lampToggleButton = null!;
    private Button _heaterToggleButton = null!;
    private Button _conditionerToggleButton = null!;
    private Button _scheduleToggleButton = null!;

    public MainForm()
    {
        Text = "SmartHome IoT Simulator";
        Width = 1100;
        Height = 760;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(235, 238, 245);
        Font = new Font("Segoe UI", 10);

        _temperatureSensor = new TemperatureSensor(22);
        _motionSensor = new MotionSensor(false);
        _lightSensor = new LightSensor(80);

        ISmartDevice baseLamp = DeviceFactory.Create("lamp", "Лампа у вітальні");
        ISmartDevice baseHeater = DeviceFactory.Create("heater", "Обігрівач у спальні");
        _conditioner = DeviceFactory.Create("airconditioner", "Кондиціонер");

        _lamp = new ScheduledDeviceDecorator(baseLamp, "18:00 - 23:00");
        _heater = new EcoModeDeviceDecorator(baseHeater, 2);

        _controller = new SmartController("Головний контролер");

        _controller.AddDevice(_lamp);
        _controller.AddDevice(_heater);
        _controller.AddDevice(_conditioner);

        _controller.AddStrategy(new TemperatureAutomationStrategy());
        _controller.AddStrategy(new MotionAutomationStrategy());
        _controller.AddStrategy(new LightAutomationStrategy());

        _facade = new SmartHomeFacade(
            _temperatureSensor,
            _motionSensor,
            _lightSensor,
            _controller
        );

        _sensorRepository.Add(_temperatureSensor);
        _sensorRepository.Add(_motionSensor);
        _sensorRepository.Add(_lightSensor);

        _deviceRepository.Add(_lamp);
        _deviceRepository.Add(_heater);
        _deviceRepository.Add(_conditioner);

        BuildInterface();
        RefreshToggleButtons();
        ShowStatus("Система запущена.");
    }

    private void BuildInterface()
    {
        Label title = new Label
        {
            Text = "Система «Розумний будинок» (IoT Simulator)",
            Left = 25,
            Top = 20,
            Width = 1000,
            Height = 42,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Color.FromArgb(35, 45, 65)
        };

        GroupBox sensorsBox = CreateGroupBox("Сенсори", 25, 80, 500, 190);

        Label tempLabel = CreateLabel("Температура:", 20, 35);
        _temperatureInput = new NumericUpDown
        {
            Left = 170,
            Top = 32,
            Width = 120,
            Minimum = -30,
            Maximum = 50,
            DecimalPlaces = 1,
            Value = 22,
            TextAlign = HorizontalAlignment.Center
        };

        Button tempButton = CreatePrimaryButton("Змінити", 315, 30, 140);
        tempButton.Click += (_, _) =>
        {
            _facade.ChangeTemperature((double)_temperatureInput.Value);
            RefreshToggleButtons();
            ShowStatus("Температуру змінено. Якщо температура нижча за 20°C — обігрівач увімкнеться автоматично.");
        };

        Label lightLabel = CreateLabel("Освітлення:", 20, 80);
        _lightInput = new NumericUpDown
        {
            Left = 170,
            Top = 77,
            Width = 120,
            Minimum = 0,
            Maximum = 100,
            Value = 80,
            TextAlign = HorizontalAlignment.Center
        };

        Button lightButton = CreatePrimaryButton("Змінити", 315, 75, 140);
        lightButton.Click += (_, _) =>
        {
            _facade.ChangeLightLevel((double)_lightInput.Value);
            RefreshToggleButtons();
            ShowStatus("Рівень освітлення змінено. Якщо освітлення нижче 40 lux — лампа увімкнеться автоматично.");
        };

        _motionCheckBox = new CheckBox
        {
            Text = "Рух виявлено",
            Left = 20,
            Top = 125,
            Width = 180
        };

        Button motionButton = CreatePrimaryButton("Застосувати рух", 315, 120, 140);
        motionButton.Click += (_, _) =>
        {
            _facade.DetectMotion(_motionCheckBox.Checked);
            RefreshToggleButtons();

            ShowStatus(_motionCheckBox.Checked
                ? "Датчик руху виявив рух. Лампа увімкнулась автоматично."
                : "Датчик руху не фіксує рух. Лампа може бути вимкнена автоматично.");
        };

        sensorsBox.Controls.Add(tempLabel);
        sensorsBox.Controls.Add(_temperatureInput);
        sensorsBox.Controls.Add(tempButton);
        sensorsBox.Controls.Add(lightLabel);
        sensorsBox.Controls.Add(_lightInput);
        sensorsBox.Controls.Add(lightButton);
        sensorsBox.Controls.Add(_motionCheckBox);
        sensorsBox.Controls.Add(motionButton);

        GroupBox devicesBox = CreateGroupBox("Керування пристроями", 550, 80, 500, 270);

        _lampToggleButton = CreateToggleButton("", 20, 35, 215);
        _lampToggleButton.Click += (_, _) =>
        {
            if (_lamp.IsOn)
                _lamp.TurnOff();
            else
                _lamp.TurnOn();

            RefreshToggleButtons();
            ShowStatus(_lamp.IsOn ? "Лампу увімкнено вручну." : "Лампу вимкнено вручну.");
        };

        _heaterToggleButton = CreateToggleButton("", 260, 35, 215);
        _heaterToggleButton.Click += (_, _) =>
        {
            if (_heater.IsOn)
                _heater.TurnOff();
            else
                _heater.TurnOn();

            RefreshToggleButtons();
            ShowStatus(_heater.IsOn ? "Обігрівач увімкнено вручну." : "Обігрівач вимкнено вручну.");
        };

        _conditionerToggleButton = CreateToggleButton("", 20, 85, 215);
        _conditionerToggleButton.Click += (_, _) =>
        {
            if (_conditioner.IsOn)
                _conditioner.TurnOff();
            else
                _conditioner.TurnOn();

            RefreshToggleButtons();
            ShowStatus(_conditioner.IsOn ? "Кондиціонер увімкнено вручну." : "Кондиціонер вимкнено вручну.");
        };

        Label scheduleLabel = CreateLabel("Розклад лампи:", 20, 145);
        _scheduleInput = new TextBox
        {
            Left = 160,
            Top = 142,
            Width = 180,
            Text = "18:00 - 23:00",
            TextAlign = HorizontalAlignment.Center
        };

        Button scheduleButton = CreateSecondaryButton("Змінити", 355, 140, 120);
        scheduleButton.Click += (_, _) =>
        {
            _lamp.SetSchedule(_scheduleInput.Text);
            ShowStatus("Розклад освітлення змінено.");
        };

        _scheduleToggleButton = CreateToggleButton("", 20, 195, 215);
        _scheduleToggleButton.Click += (_, _) =>
        {
            if (_lamp.IsScheduleEnabled)
                _lamp.DisableSchedule();
            else
                _lamp.EnableSchedule();

            RefreshToggleButtons();
            ShowStatus(_lamp.IsScheduleEnabled ? "Розклад освітлення увімкнено." : "Розклад освітлення вимкнено.");
        };

        Label ecoLabel = CreateLabel("Екорежим:", 245, 198);
        _ecoInput = new NumericUpDown
        {
            Left = 365,
            Top = 195,
            Width = 70,
            Minimum = 0,
            Maximum = 5,
            Value = 2,
            TextAlign = HorizontalAlignment.Center
        };

        Button ecoButton = CreateSecondaryButton("OK", 445, 193, 40);
        ecoButton.Click += (_, _) =>
        {
            _heater.SetEcoLevel((int)_ecoInput.Value);
            ShowStatus("Рівень екорежиму обігрівача змінено.");
        };

        devicesBox.Controls.Add(_lampToggleButton);
        devicesBox.Controls.Add(_heaterToggleButton);
        devicesBox.Controls.Add(_conditionerToggleButton);
        devicesBox.Controls.Add(scheduleLabel);
        devicesBox.Controls.Add(_scheduleInput);
        devicesBox.Controls.Add(scheduleButton);
        devicesBox.Controls.Add(_scheduleToggleButton);
        devicesBox.Controls.Add(ecoLabel);
        devicesBox.Controls.Add(_ecoInput);
        devicesBox.Controls.Add(ecoButton);

        GroupBox actionsBox = CreateGroupBox("Додаткові дії", 25, 285, 500, 65);

        Button statusButton = CreateSecondaryButton("Показати стан", 20, 25, 140);
        statusButton.Click += (_, _) => ShowStatus("Оновлено стан системи.");

        Button linqButton = CreateSecondaryButton("LINQ-звіти", 175, 25, 120);
        linqButton.Click += (_, _) => ShowLinqReport();

        Button demoButton = CreateSecondaryButton("Демо", 310, 25, 80);
        demoButton.Click += (_, _) => RunDemo();

        Button explanationButton = CreateSecondaryButton("Пояснення", 405, 25, 80);
        explanationButton.Click += (_, _) => ShowExplanation();

        actionsBox.Controls.Add(statusButton);
        actionsBox.Controls.Add(linqButton);
        actionsBox.Controls.Add(demoButton);
        actionsBox.Controls.Add(explanationButton);

        _outputBox = new TextBox
        {
            Left = 25,
            Top = 375,
            Width = 1025,
            Height = 315,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 10),
            BackColor = Color.FromArgb(25, 30, 40),
            ForeColor = Color.FromArgb(230, 240, 250),
            BorderStyle = BorderStyle.FixedSingle
        };

        Controls.Add(title);
        Controls.Add(sensorsBox);
        Controls.Add(devicesBox);
        Controls.Add(actionsBox);
        Controls.Add(_outputBox);
    }

    private GroupBox CreateGroupBox(string text, int left, int top, int width, int height)
    {
        return new GroupBox
        {
            Text = text,
            Left = left,
            Top = top,
            Width = width,
            Height = height,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(35, 45, 65),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };
    }

    private Label CreateLabel(string text, int left, int top)
    {
        return new Label
        {
            Text = text,
            Left = left,
            Top = top,
            Width = 140,
            Height = 25,
            Font = new Font("Segoe UI", 10)
        };
    }

    private Button CreatePrimaryButton(string text, int left, int top, int width)
    {
        Button button = new Button
        {
            Text = text,
            Left = left,
            Top = top,
            Width = width,
            Height = 32,
            BackColor = Color.FromArgb(55, 105, 180),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };

        button.FlatAppearance.BorderSize = 0;
        return button;
    }

    private Button CreateSecondaryButton(string text, int left, int top, int width)
    {
        Button button = new Button
        {
            Text = text,
            Left = left,
            Top = top,
            Width = width,
            Height = 32,
            BackColor = Color.FromArgb(90, 100, 120),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };

        button.FlatAppearance.BorderSize = 0;
        return button;
    }

    private Button CreateToggleButton(string text, int left, int top, int width)
    {
        Button button = new Button
        {
            Text = text,
            Left = left,
            Top = top,
            Width = width,
            Height = 38,
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        button.FlatAppearance.BorderSize = 0;
        return button;
    }

    private void SetToggleView(Button button, bool isOn, string name)
    {
        button.Text = isOn ? $"● ON  {name}" : $"● OFF {name}";
        button.BackColor = isOn
            ? Color.FromArgb(40, 160, 90)
            : Color.FromArgb(190, 65, 65);
    }

    private void RefreshToggleButtons()
    {
        SetToggleView(_lampToggleButton, _lamp.IsOn, "Лампа");
        SetToggleView(_heaterToggleButton, _heater.IsOn, "Обігрівач");
        SetToggleView(_conditionerToggleButton, _conditioner.IsOn, "Кондиціонер");
        SetToggleView(_scheduleToggleButton, _lamp.IsScheduleEnabled, "Розклад");
    }

    private void ShowStatus(string message)
    {
        _outputBox.Clear();

        WriteLine("=== SmartHome IoT Simulator ===");
        WriteLine(message);
        WriteLine("");

        WriteLine("Сенсори:");
        foreach (Sensor sensor in _sensorRepository.GetAll())
            WriteLine($"- {sensor.GetInfo()}");

        WriteLine("");
        WriteLine("Пристрої:");
        foreach (ISmartDevice device in _deviceRepository.GetAll())
            WriteLine($"- {device.GetStatus()}");
    }

    private void ShowLinqReport()
    {
        _outputBox.Clear();

        WriteLine("=== LINQ-звіт ===");
        WriteLine("");

        var enabledDevices = _deviceRepository
            .GetAll()
            .Where(device => device.IsOn)
            .Select(device => device.Name)
            .ToList();

        WriteLine("Увімкнені пристрої:");
        if (enabledDevices.Count == 0)
            WriteLine("- немає увімкнених пристроїв");
        else
            foreach (var name in enabledDevices)
                WriteLine($"- {name}");

        WriteLine("");

        double averageSensorValue = _sensorRepository.GetAll().AverageSensorValue();
        WriteLine($"Середнє значення сенсорів: {averageSensorValue:F2}");

        WriteLine("");

        var groupedDevices = _deviceRepository
            .GetAll()
            .GroupBy(device => device.IsOn ? "Увімкнені" : "Вимкнені");

        WriteLine("Групування пристроїв:");
        foreach (var group in groupedDevices)
        {
            WriteLine(group.Key + ":");
            foreach (var device in group)
                WriteLine($"- {device.Name}");
        }
    }

    private void RunDemo()
    {
        _facade.ChangeTemperature(17);
        _facade.DetectMotion(true);
        _facade.ChangeLightLevel(20);
        _conditioner.TurnOn();
        _lamp.EnableSchedule();

        _temperatureInput.Value = 17;
        _lightInput.Value = 20;
        _motionCheckBox.Checked = true;

        RefreshToggleButtons();
        ShowStatus("Демо виконано: температура знижена, рух виявлено, освітлення зменшено, кондиціонер увімкнено вручну.");
    }

    private void ShowExplanation()
    {
        _outputBox.Clear();

        WriteLine("=== Пояснення роботи системи ===");
        WriteLine("");
        WriteLine("1. Датчик температури:");
        WriteLine("- якщо температура менша за 20°C, обігрівач вмикається автоматично;");
        WriteLine("- якщо температура 20°C або вище, обігрівач вимикається.");
        WriteLine("");
        WriteLine("2. Датчик руху:");
        WriteLine("- якщо рух виявлено, лампа вмикається автоматично;");
        WriteLine("- якщо руху немає, лампа може бути вимкнена автоматично.");
        WriteLine("");
        WriteLine("3. Датчик освітлення:");
        WriteLine("- якщо освітлення нижче 40 lux, лампа вмикається;");
        WriteLine("- якщо освітлення достатнє, лампа вимикається.");
        WriteLine("");
        WriteLine("4. Кондиціонер:");
        WriteLine("- керується вручну кнопкою ON/OFF.");
        WriteLine("");
        WriteLine("5. Decorator:");
        WriteLine("- лампа має додатковий розклад роботи;");
        WriteLine("- розклад можна увімкнути або вимкнути;");
        WriteLine("- обігрівач має додатковий екорежим.");
    }

    private void WriteLine(string text)
    {
        _outputBox.AppendText(text + Environment.NewLine);
    }
}