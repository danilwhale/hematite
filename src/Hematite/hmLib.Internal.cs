using Hematite.Graphics.Backends;
using Hematite.Input;
using Hematite.Windowing;
using Hematite.Windowing.Platforms;

namespace Hematite;

public static partial class hmLib
{
    internal static readonly hmIPlatform Platform = new hmSDLPlatform();
    internal static readonly hmIBackend Backend = new hmGLBackend();
    [ThreadStatic] internal static hmWindow? CurrentWindow;

    internal static void UpdateWindow(hmWindow window)
    {
        foreach ((hmKeyCode key, bool pressed) in window.KeyboardState)
        {
            window.LastKeyboardState[key] = pressed;
        }
        foreach ((hmMouseButton button, bool pressed) in window.MouseState)
        {
            window.LastMouseState[button] = pressed;
        }
        window.LastMousePosition = window.MousePosition;
        window.MouseWheelVelocity = default;
        Platform.WindowUpdate(window);
    }
}