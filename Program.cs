using System.Runtime.InteropServices;

Console.WriteLine("");
if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    throw new InvalidOperationException("It works only on Linux.");
}