using System.Runtime.InteropServices;

const int sleepTimeInSeconds = 1;
const string fileName = "cc62283d-9cdc-4ead-9a6f-f2f578125ac5a";

Console.WriteLine("LockU starting!");

var initiallyDetected = IsUsbDetected();

if (initiallyDetected)
{
    while (true)
    {
        Console.WriteLine($"LockDriveUsbDetected: {IsUsbDetected()}");

        if (!IsUsbDetected())
        {
            LockWorkStation();
        }

        Thread.Sleep(TimeSpan.FromSeconds(sleepTimeInSeconds));
    }
}

Console.WriteLine($"LockDriveUsbDetected: {IsUsbDetected()}");

[DllImport("user32.dll")]
static extern void LockWorkStation();

static bool IsUsbDetected()
=>  DriveInfo
        .GetDrives()
        .Where(possibleDevices => possibleDevices.DriveType == DriveType.Removable && possibleDevices.IsReady)
        .Any(drive => drive
            .RootDirectory
            .EnumerateFiles(fileName, SearchOption.AllDirectories)
            .Any(x => x.Name == fileName));