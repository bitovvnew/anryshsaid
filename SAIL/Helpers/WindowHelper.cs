using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SAIL.Helpers;

public static class WindowHelper
{
    private const int DwmwaUseImmersiveDarkMode = 20;
    private const int DwmwaMicaEffect = 1029;
    private const int DwmwaSystemBackdropType = 38;
    private const int DwmSystembackdropTypeMica = 2;
    private const int DwmSystembackdropTypeAcrylic = 3;

    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    public static void ApplyModernChrome(Window window, bool darkMode = true)
    {
        window.SourceInitialized += (_, _) =>
        {
            var handle = new WindowInteropHelper(window).Handle;
            if (handle == IntPtr.Zero) return;

            int dark = darkMode ? 1 : 0;
            DwmSetWindowAttribute(handle, DwmwaUseImmersiveDarkMode, ref dark, sizeof(int));

            int backdrop = DwmSystembackdropTypeMica;
            if (DwmSetWindowAttribute(handle, DwmwaSystemBackdropType, ref backdrop, sizeof(int)) != 0)
            {
                backdrop = DwmSystembackdropTypeAcrylic;
                DwmSetWindowAttribute(handle, DwmwaSystemBackdropType, ref backdrop, sizeof(int));
            }
        };
    }

    public static void ApplyDarkMode(Window window, bool darkMode = true)
    {
        window.SourceInitialized += (_, _) =>
        {
            var handle = new WindowInteropHelper(window).Handle;
            if (handle == IntPtr.Zero) return;

            int dark = darkMode ? 1 : 0;
            DwmSetWindowAttribute(handle, DwmwaUseImmersiveDarkMode, ref dark, sizeof(int));
        };
    }

    public static void EnableBlur(Window window)
    {
        ApplyDarkMode(window);
    }
}
