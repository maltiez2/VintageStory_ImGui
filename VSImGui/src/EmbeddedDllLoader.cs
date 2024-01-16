using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace VSImGui
{
    public class EmbeddedDllClass // Author: Rasmus Tanggaard (Dmitry221060 - VS discord server) (https://github.com/Ridderrasmus)
    {
        private static string tempFolder;

        public static void ExtractEmbeddedDlls(ILogger logger)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();

            foreach (var dllName in resourceNames)
            {
                ExtractEmbeddedDll(dllName, logger);
            }
        }

        public static void ExtractEmbeddedDll(string resourceName, ILogger logger)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            tempFolder ??= $"{assemblyName.Name}.{assemblyName.Version}";

            byte[] resourceBytes;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                resourceBytes = memoryStream.ToArray();
            }

            string dirName = Path.Combine(Path.GetTempPath(), tempFolder);
            if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);

            string[] resourceNameParts = resourceName.Split(".");
            string dllName = $"{resourceNameParts[resourceNameParts.Length - 2]}.{resourceNameParts[resourceNameParts.Length - 1]}";
            string dllPath = Path.Combine(dirName, dllName);
            bool alreadyExtracted = false;
            if (File.Exists(dllPath))
            {
                byte[] existingResource = File.ReadAllBytes(dllPath);
                alreadyExtracted = resourceBytes.SequenceEqual(existingResource);
            }
            if (alreadyExtracted) return;
            File.WriteAllBytes(dllPath, resourceBytes);
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string fileName);

        [DllImport("libdl.so.2")]
        static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("libdl.so.2")]
        private static extern IntPtr dlerror();

        public static bool LoadDll(string dllName)
        {
            if (tempFolder == null) throw new Exception("Cannot load embedded dlls before extracting them");

            string dllPath = Path.Combine(Path.GetTempPath(), tempFolder, dllName);
            IntPtr handle = LoadLibrary(dllPath);

            if (handle == IntPtr.Zero)
            {
                Exception innerException = new Win32Exception();
                Exception e = new DllNotFoundException("Unable to load library: " + dllName + " from " + tempFolder, innerException);
                Console.WriteLine($"Failed to load embedded DLL:\n{e}");
                return false;
            }

            return true;
        }

        public static bool LoadImGui(ILogger logger)
        {
            if (tempFolder == null) throw new Exception("Cannot load embedded dlls before extracting them");

            string dllName = "";

            switch (RuntimeEnv.OS)
            {
                case OS.Windows:
                    dllName = "cimgui.dll";
                    break;
                case OS.Mac:
                    dllName = "libcimgui.dylib";
                    break;
                case OS.Linux:
                    dllName = "libcimgui.so";
                    break;
            }

            string dllPath = Path.Combine(Path.GetTempPath(), tempFolder, dllName);

            IntPtr? handle = null;

            switch (RuntimeEnv.OS)
            {
                case OS.Windows:
                    handle = LoadLibrary(dllPath);
                    break;
                case OS.Mac:
                    break;
                case OS.Linux:
                    handle = dlopen(dllPath, 1);
                    break;
            }

            if (handle == IntPtr.Zero)
            {
                Exception innerException = new Win32Exception();
                Exception e = new DllNotFoundException("Unable to load library: " + dllName + " from " + tempFolder, innerException);
                logger.Fatal($"Failed to load embedded DLL:\n\nDlError:\n{Marshal.PtrToStringAnsi(dlerror())}\n\nException:\n{e}");
                return false;
            }

            return true;
        }
    }
}
