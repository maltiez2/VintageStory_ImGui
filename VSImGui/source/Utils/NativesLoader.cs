using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Common;

namespace VSImGui;

internal static class NativesLoader
{
    public static bool Load(ILogger logger, ModSystem mod)
    {
        DllLoader loader = DllLoader.Loader();
        if (!loader.Load("cimgui", logger, mod.Mod)) return false;
        if (!loader.Load("cimguizmo", logger, mod.Mod)) return false;
        if (!loader.Load("cimnodes", logger, mod.Mod)) return false;
        if (!loader.Load("cimplot", logger, mod.Mod)) return false;
        return true;
    }
}

internal abstract class DllLoader
{
    public static DllLoader Loader()
    {
        return RuntimeEnv.OS switch
        {
            OS.Windows => new WindowsDllLoader(),
            OS.Mac => new MacDllLoader(),
            OS.Linux => new LinuxDllLoader(),
            _ => new WindowsDllLoader()
        };
    }

    protected DllLoader()
    {

    }

    public bool Load(string dllName, ILogger logger, Mod mod)
    {
        string suffix = RuntimeEnv.OS switch
        {
            OS.Windows => ".dll",
            OS.Mac => ".dylib",
            OS.Linux => ".so",
            _ => throw new ArgumentOutOfRangeException()
        };
        string prefix = RuntimeEnv.OS switch
        {
            OS.Windows => "win/",
            OS.Mac => "mac/",
            OS.Linux => "linux/",
            _ => throw new ArgumentOutOfRangeException()
        };

        string dllPath = $"{((ModContainer)mod).FolderPath}/native/{prefix}{dllName}{suffix}";

        return Load(dllPath, logger);
    }

    protected abstract bool Load(string dllPath, ILogger logger);
}

internal class WindowsDllLoader : DllLoader
{
    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern IntPtr LoadLibrary(string fileName);
    [DllImport("kernel32")]
    private static extern uint GetLastError();
    [DllImport("kernel32")]
    private static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId,
        uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize, IntPtr[] Arguments);

    protected override bool Load(string dllPath, ILogger logger)
    {
        IntPtr? handle = LoadLibrary(dllPath);

        if (handle == IntPtr.Zero)
        {
            uint errorCode = GetLastError();

            StringBuilder errorMessageBuilder = new(255);
            _ = FormatMessage(0x00001000, IntPtr.Zero, errorCode, 0, errorMessageBuilder, 255, null);
            string errorMessage = errorMessageBuilder.ToString();

            Exception innerException = new Win32Exception();
            Exception exception = new DllNotFoundException("Unable to load library: " + dllPath, innerException);
            logger.Fatal($"Failed to load embedded DLL:\n\nDlError:\n{errorMessage}\n\nException:\n{exception}");
            return false;
        }

        return true;
    }
}

internal class LinuxDllLoader : DllLoader
{
    [DllImport("libdl.so.2")]
    static extern IntPtr dlopen(string fileName, int flags);
    [DllImport("libdl.so.2")]
    private static extern IntPtr dlerror();

    protected override bool Load(string dllPath, ILogger logger)
    {
        IntPtr? handle = dlopen(dllPath, 1);

        if (handle == IntPtr.Zero)
        {
            Exception innerException = new Win32Exception();
            Exception exception = new DllNotFoundException("Unable to load library: " + dllPath, innerException);
            logger.Fatal($"Failed to load embedded DLL:\n\nDlError:\n{Marshal.PtrToStringAnsi(dlerror())}\n\nException:\n{exception}");
            return false;
        }

        return true;
    }
}

internal class MacDllLoader : DllLoader
{
    [DllImport("libdl.dylib", EntryPoint = "dlopen")]
    private static extern IntPtr dlopen(string filename, int flags);
    [DllImport("libdl.dylib")]
    private static extern IntPtr dlerror();

    protected override bool Load(string dllPath, ILogger logger)
    {
        IntPtr? handle = dlopen(dllPath, 1);

        if (handle == IntPtr.Zero)
        {
            Exception innerException = new Win32Exception();
            Exception exception = new DllNotFoundException("Unable to load library: " + dllPath, innerException);
            logger.Fatal($"Failed to load embedded DLL:\n\nDlError:\n{Marshal.PtrToStringAnsi(dlerror())}\n\nException:\n{exception}");
            return false;
        }

        return true;
    }
}
