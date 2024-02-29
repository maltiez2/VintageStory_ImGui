using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Common;

namespace VSImGui;

/// <summary>
/// Provides method to load all supported binaries for current platform
/// </summary>
internal static class NativesLoader
{
    /// <summary>
    /// Loads all supported binaries for current platform
    /// </summary>
    /// <param name="logger">To report errors</param>
    /// <param name="mod">To get path to folder with natives</param>
    /// <returns></returns>
    public static bool Load(ILogger logger, ModSystem mod)
    {
        DllLoader loader = DllLoader.Loader();
        foreach (string library in _nativeLibraries)
        {
            if (!loader.Load(library, logger, mod.Mod)) return false;
        }
        return true;
    }
    /// <summary>
    /// Supported libraries to load
    /// </summary>
    private static readonly HashSet<string> _nativeLibraries = new()
    {
        "cimgui",
        "cimguizmo",
        "cimnodes",
        "cimplot"
    };
}

/// <summary>
/// Base class for native dll loaders for different platforms
/// </summary>
internal abstract class DllLoader
{
    /// <summary>
    /// Returns loader for current platform
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Loads specified native dll.<br/>
    /// Platform specific paths:
    /// <list type="bullet">
    /// <item>Windows: '/native/win/{<paramref name="dllName"/>}.dll'</item>
    /// <item>Linux: '/native/linux/{<paramref name="dllName"/>}.so'</item>
    /// <item>Mac: '/native/mac/{<paramref name="dllName"/>}.dylib'</item>
    /// </list>
    /// </summary>
    /// <param name="dllName">Native dll name, not path and without extension</param>
    /// <param name="logger">To log errors</param>
    /// <param name="mod">Mod that has specified native library in /native/{platform} directory</param>
    /// <returns>true if was successfully loaded</returns>
    public bool Load(string dllName, ILogger logger, Mod mod)
    {
        string suffix = RuntimeEnv.OS switch
        {
            OS.Windows => ".dll",
            OS.Mac => ".dylib",
            OS.Linux => ".so",
            _ => ".so"
        };
        string prefix = RuntimeEnv.OS switch
        {
            OS.Windows => "win/",
            OS.Mac => "mac/",
            OS.Linux => "linux/",
            _ => "linux"
        };

        string dllPath = $"{((ModContainer)mod).FolderPath}/native/{prefix}{dllName}{suffix}";

        return Load(dllPath, logger);
    }

    /// <summary>
    /// Load dll using platofrm specific functions
    /// </summary>
    /// <param name="dllPath">Full path to dll</param>
    /// <param name="logger">To log errors</param>
    /// <returns>true if was successfuly loaded</returns>
    protected abstract bool Load(string dllPath, ILogger logger);
}

/// <summary>
/// Loads native dll on Windows
/// </summary>
internal partial class WindowsDllLoader : DllLoader
{
    /// <summary>
    /// Function from 'kernel32.dll' that is used to load dlls on windows
    /// </summary>
    /// <param name="fileName">Full path to dll</param>
    /// <returns><see cref="IntPtr.Zero"/> if failed to load library</returns>
    [LibraryImport("kernel32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr LoadLibrary(string fileName);
    /// <summary>
    /// Retrieves error code of error occurred while loading dynamic library 
    /// </summary>
    /// <returns>Error code</returns>
    [LibraryImport("kernel32")]
    private static partial uint GetLastError();
    /// <summary>
    /// Retrieves error message for error code provided by <see cref="GetLastError"/>
    /// </summary>
    /// <param name="dwFlags"></param>
    /// <param name="lpSource"></param>
    /// <param name="dwMessageId">Error code from <see cref="GetLastError"/></param>
    /// <param name="dwLanguageId"></param>
    /// <param name="lpBuffer">Error message will be written into this object</param>
    /// <param name="nSize"></param>
    /// <param name="Arguments"></param>
    /// <returns></returns>
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId,
        uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize, IntPtr[] Arguments);

    /// <summary>
    /// Loads specified dll using windows specific functions
    /// </summary>
    /// <param name="dllPath">Full path to dll</param>
    /// <param name="logger">To log errors</param>
    /// <returns>true if was successfully loaded</returns>
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

/// <summary>
/// Loads specified dll using linux specific functions
/// </summary>
/// <param name="dllPath">Full path to dll</param>
/// <param name="logger">To log errors</param>
/// <returns>true if was successfully loaded</returns>
internal partial class LinuxDllLoader : DllLoader
{
    [LibraryImport("libdl.so.2", StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr dlopen(string fileName, int flags);
    [LibraryImport("libdl.so.2")]
    private static partial IntPtr dlerror();

    /// <summary>
    /// Loads specified dll using linux specific functions
    /// </summary>
    /// <param name="dllPath">Full path to dll</param>
    /// <param name="logger">To log errors</param>
    /// <returns>true if was successfully loaded</returns>
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

/// <summary>
/// Loads specified dll using OSX (Mac) specific functions
/// </summary>
/// <param name="dllPath">Full path to dll</param>
/// <param name="logger">To log errors</param>
/// <returns>true if was successfully loaded</returns>
internal class MacDllLoader : DllLoader
{
    [DllImport("libdl.dylib", EntryPoint = "dlopen", CharSet = CharSet.Unicode)]
    private static extern IntPtr dlopen(string filename, int flags);
    [DllImport("libdl.dylib")]
    private static extern IntPtr dlerror();

    /// <summary>
    /// Loads specified dll using OSX (Mac) specific functions
    /// </summary>
    /// <param name="dllPath">Full path to dll</param>
    /// <param name="logger">To log errors</param>
    /// <returns>true if was successfully loaded</returns>
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
