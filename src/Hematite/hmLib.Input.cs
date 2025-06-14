using System.Numerics;
using Hematite.Input;
using Hematite.Windowing;
using Vortice.Mathematics;

namespace Hematite;

public static partial class hmLib
{
    public static partial bool hmInputIsKeyPressed(hmWindow? window, hmKeyCode key)
    {
        window ??= CurrentWindow;
        return window is not null && window.KeyboardState.TryGetValue(key, out bool pressed) && pressed;
    }

    public static partial bool hmInputIsKeyJustPressed(hmWindow? window, hmKeyCode key)
    {
        window ??= CurrentWindow;
        return window is not null &&
               (!window.LastKeyboardState.TryGetValue(key, out bool wasPressed) || !wasPressed) &&
               (window.KeyboardState.TryGetValue(key, out bool pressed) && pressed);
    }

    public static partial bool hmInputIsKeyJustReleased(hmWindow? window, hmKeyCode key)
    {
        window ??= CurrentWindow;
        return window is not null &&
               (window.LastKeyboardState.TryGetValue(key, out bool wasPressed) && wasPressed) &&
               (!window.KeyboardState.TryGetValue(key, out bool pressed) || !pressed);
    }

    public static partial int hmInputGetKeyAxis(hmWindow? window, hmKeyCode negativeKey, hmKeyCode positiveKey)
    {
        window ??= CurrentWindow;
        if (window is null) return 0;
        return (window.KeyboardState.TryGetValue(positiveKey, out bool positivePressed) && positivePressed ? 1 : 0) -
               (window.KeyboardState.TryGetValue(negativeKey, out bool negativePressed) && negativePressed ? 1 : 0);
    }

    public static partial bool hmInputIsMouseButtonPressed(hmWindow? window, hmMouseButton button)
    {
        window ??= CurrentWindow;
        return window is not null && window.MouseState.TryGetValue(button, out bool pressed) && pressed;
    }

    public static partial bool hmInputIsMouseButtonJustPressed(hmWindow? window, hmMouseButton button)
    {
        window ??= CurrentWindow;
        return window is not null &&
               (!window.LastMouseState.TryGetValue(button, out bool wasPressed) || !wasPressed) &&
               (window.MouseState.TryGetValue(button, out bool pressed) && pressed);
    }

    public static partial bool hmInputIsMouseButtonJustReleased(hmWindow? window, hmMouseButton button)
    {
        window ??= CurrentWindow;
        return window is not null &&
               (window.LastMouseState.TryGetValue(button, out bool wasPressed) && wasPressed) &&
               (!window.MouseState.TryGetValue(button, out bool pressed) || !pressed);
    }

    public static partial Vector2 hmInputGetMousePosition(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window?.MousePosition ?? default;
    }

    public static partial void hmInputWarpMouse(hmWindow? window, Vector2 position)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        Platform.InputWarpMouse(window, position);
    }

    public static partial Vector2 hmInputGetMouseVelocity(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? default : window.MousePosition - window.LastMousePosition;
    }

    public static partial Vector2 hmInputGetMouseWheelVelocity(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window?.MouseWheelVelocity ?? default;
    }

    public static partial void hmInputSetMouseLocked(hmWindow? window, bool locked)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        SizeI size = Platform.WindowGetSize(window);
        Platform.InputWarpMouse(window, new Vector2(size.Width * 0.5f, size.Height * 0.5f));
        Platform.InputSetMouseLocked(window, locked);
    }

    public static partial bool hmInputIsMouseLocked(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window != null && Platform.InputIsMouseLocked(window);
    }
}