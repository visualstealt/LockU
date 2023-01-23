using System.Runtime.InteropServices;

const string fileName = "cc62283d-9cdc-4ead-9a6f-f2f578125ac5a";
var sleepTimeInSeconds = TimeSpan.FromMilliseconds(50);
var debugEnabled = Environment.GetCommandLineArgs().Contains("-verbose");

Console.WriteLine("LockU starting!");

var initiallyDetected = IsUsbDetected();

if (initiallyDetected)
{
    while (true)
    {
        PrintDiagnostics();

        if (!IsUsbDetected())
        {
            LockWorkStation();
        }

        Thread.Sleep(sleepTimeInSeconds);
    }
}

Console.WriteLine("LockU closing!");

[DllImport("user32.dll")]
static extern void LockWorkStation();

static bool IsUsbDetected()
=> DriveInfo
        .GetDrives()
        .Where(possibleDevices => possibleDevices.DriveType == DriveType.Removable && possibleDevices.IsReady)
        .Any(drive => drive
            .RootDirectory
            .EnumerateFiles(fileName, SearchOption.AllDirectories)
            .Any(x => x.Name == fileName));

void PrintDiagnostics()
{
    if (debugEnabled)
    {
        Console.WriteLine($"LockDriveUsbDetected: {IsUsbDetected()}");
    }
}