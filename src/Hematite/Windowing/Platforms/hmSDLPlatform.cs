using System.Collections.Frozen;
using System.Numerics;
using Hematite.Graphics.Backends;
using Hematite.Input;
using SDL;
using Silk.NET.OpenGL;
using Vortice.Mathematics;
using static SDL.SDL3;

namespace Hematite.Windowing.Platforms;

internal sealed unsafe class hmSDLPlatform : hmIPlatform
{
    private static readonly FrozenDictionary<hmKeyCode, SDL_Keycode> KeyCodesByHm = new Dictionary<hmKeyCode, SDL_Keycode>
    {
        [hmKeyCode.None] = SDL_Keycode.SDLK_UNKNOWN,
        [hmKeyCode.Space] = SDL_Keycode.SDLK_SPACE,
        [hmKeyCode.Apostrophe] = SDL_Keycode.SDLK_APOSTROPHE,
        [hmKeyCode.Comma] = SDL_Keycode.SDLK_COMMA,
        [hmKeyCode.Minus] = SDL_Keycode.SDLK_MINUS,
        [hmKeyCode.Period] = SDL_Keycode.SDLK_PERIOD,
        [hmKeyCode.Slash] = SDL_Keycode.SDLK_SLASH,
        [hmKeyCode.D0] = SDL_Keycode.SDLK_0,
        [hmKeyCode.D1] = SDL_Keycode.SDLK_1,
        [hmKeyCode.D2] = SDL_Keycode.SDLK_2,
        [hmKeyCode.D3] = SDL_Keycode.SDLK_3,
        [hmKeyCode.D4] = SDL_Keycode.SDLK_4,
        [hmKeyCode.D5] = SDL_Keycode.SDLK_5,
        [hmKeyCode.D6] = SDL_Keycode.SDLK_6,
        [hmKeyCode.D7] = SDL_Keycode.SDLK_7,
        [hmKeyCode.D8] = SDL_Keycode.SDLK_8,
        [hmKeyCode.D9] = SDL_Keycode.SDLK_9,
        [hmKeyCode.Semicolon] = SDL_Keycode.SDLK_SEMICOLON,
        [hmKeyCode.Equal] = SDL_Keycode.SDLK_EQUALS,
        [hmKeyCode.A] = SDL_Keycode.SDLK_A,
        [hmKeyCode.B] = SDL_Keycode.SDLK_B,
        [hmKeyCode.C] = SDL_Keycode.SDLK_C,
        [hmKeyCode.D] = SDL_Keycode.SDLK_D,
        [hmKeyCode.E] = SDL_Keycode.SDLK_E,
        [hmKeyCode.F] = SDL_Keycode.SDLK_F,
        [hmKeyCode.G] = SDL_Keycode.SDLK_G,
        [hmKeyCode.H] = SDL_Keycode.SDLK_H,
        [hmKeyCode.J] = SDL_Keycode.SDLK_J,
        [hmKeyCode.K] = SDL_Keycode.SDLK_K,
        [hmKeyCode.L] = SDL_Keycode.SDLK_L,
        [hmKeyCode.M] = SDL_Keycode.SDLK_M,
        [hmKeyCode.N] = SDL_Keycode.SDLK_N,
        [hmKeyCode.O] = SDL_Keycode.SDLK_O,
        [hmKeyCode.P] = SDL_Keycode.SDLK_P,
        [hmKeyCode.Q] = SDL_Keycode.SDLK_Q,
        [hmKeyCode.R] = SDL_Keycode.SDLK_R,
        [hmKeyCode.S] = SDL_Keycode.SDLK_S,
        [hmKeyCode.T] = SDL_Keycode.SDLK_T,
        [hmKeyCode.U] = SDL_Keycode.SDLK_U,
        [hmKeyCode.V] = SDL_Keycode.SDLK_V,
        [hmKeyCode.W] = SDL_Keycode.SDLK_W,
        [hmKeyCode.X] = SDL_Keycode.SDLK_X,
        [hmKeyCode.Y] = SDL_Keycode.SDLK_Y,
        [hmKeyCode.Z] = SDL_Keycode.SDLK_Z,
        [hmKeyCode.LeftBracket] = SDL_Keycode.SDLK_LEFTBRACKET,
        [hmKeyCode.Backslash] = SDL_Keycode.SDLK_BACKSLASH,
        [hmKeyCode.RightBracket] = SDL_Keycode.SDLK_RIGHTBRACKET,
        [hmKeyCode.Grave] = SDL_Keycode.SDLK_GRAVE,
        [hmKeyCode.Escape] = SDL_Keycode.SDLK_ESCAPE,
        [hmKeyCode.Enter] = SDL_Keycode.SDLK_RETURN,
        [hmKeyCode.Tab] = SDL_Keycode.SDLK_TAB,
        [hmKeyCode.Backspace] = SDL_Keycode.SDLK_BACKSPACE,
        [hmKeyCode.Insert] = SDL_Keycode.SDLK_INSERT,
        [hmKeyCode.Delete] = SDL_Keycode.SDLK_DELETE,
        [hmKeyCode.Right] = SDL_Keycode.SDLK_RIGHT,
        [hmKeyCode.Left] = SDL_Keycode.SDLK_LEFT,
        [hmKeyCode.Down] = SDL_Keycode.SDLK_DOWN,
        [hmKeyCode.Up] = SDL_Keycode.SDLK_UP,
        [hmKeyCode.PageUp] = SDL_Keycode.SDLK_PAGEUP,
        [hmKeyCode.PageDown] = SDL_Keycode.SDLK_PAGEDOWN,
        [hmKeyCode.Home] = SDL_Keycode.SDLK_HOME,
        [hmKeyCode.End] = SDL_Keycode.SDLK_END,
        [hmKeyCode.CapsLock] = SDL_Keycode.SDLK_CAPSLOCK,
        [hmKeyCode.ScrollLock] = SDL_Keycode.SDLK_SCROLLLOCK,
        [hmKeyCode.NumLock] = SDL_Keycode.SDLK_NUMLOCKCLEAR,
        [hmKeyCode.PrintScreen] = SDL_Keycode.SDLK_PRINTSCREEN,
        [hmKeyCode.Pause] = SDL_Keycode.SDLK_PAUSE,
        [hmKeyCode.F1] = SDL_Keycode.SDLK_F1,
        [hmKeyCode.F2] = SDL_Keycode.SDLK_F2,
        [hmKeyCode.F3] = SDL_Keycode.SDLK_F3,
        [hmKeyCode.F4] = SDL_Keycode.SDLK_F4,
        [hmKeyCode.F5] = SDL_Keycode.SDLK_F5,
        [hmKeyCode.F6] = SDL_Keycode.SDLK_F6,
        [hmKeyCode.F7] = SDL_Keycode.SDLK_F7,
        [hmKeyCode.F8] = SDL_Keycode.SDLK_F8,
        [hmKeyCode.F9] = SDL_Keycode.SDLK_F9,
        [hmKeyCode.F10] = SDL_Keycode.SDLK_F10,
        [hmKeyCode.F11] = SDL_Keycode.SDLK_F11,
        [hmKeyCode.F12] = SDL_Keycode.SDLK_F12,
        [hmKeyCode.F13] = SDL_Keycode.SDLK_F13,
        [hmKeyCode.F14] = SDL_Keycode.SDLK_F14,
        [hmKeyCode.F15] = SDL_Keycode.SDLK_F15,
        [hmKeyCode.F16] = SDL_Keycode.SDLK_F16,
        [hmKeyCode.F17] = SDL_Keycode.SDLK_F17,
        [hmKeyCode.F18] = SDL_Keycode.SDLK_F18,
        [hmKeyCode.F19] = SDL_Keycode.SDLK_F19,
        [hmKeyCode.F20] = SDL_Keycode.SDLK_F20,
        [hmKeyCode.F21] = SDL_Keycode.SDLK_F21,
        [hmKeyCode.F22] = SDL_Keycode.SDLK_F22,
        [hmKeyCode.F23] = SDL_Keycode.SDLK_F23,
        [hmKeyCode.F24] = SDL_Keycode.SDLK_F24,
        [hmKeyCode.Kp0] = SDL_Keycode.SDLK_KP_0,
        [hmKeyCode.Kp1] = SDL_Keycode.SDLK_KP_1,
        [hmKeyCode.Kp2] = SDL_Keycode.SDLK_KP_2,
        [hmKeyCode.Kp3] = SDL_Keycode.SDLK_KP_3,
        [hmKeyCode.Kp4] = SDL_Keycode.SDLK_KP_4,
        [hmKeyCode.Kp5] = SDL_Keycode.SDLK_KP_5,
        [hmKeyCode.Kp6] = SDL_Keycode.SDLK_KP_6,
        [hmKeyCode.Kp7] = SDL_Keycode.SDLK_KP_7,
        [hmKeyCode.Kp8] = SDL_Keycode.SDLK_KP_8,
        [hmKeyCode.Kp9] = SDL_Keycode.SDLK_KP_9,
        [hmKeyCode.KpDecimal] = SDL_Keycode.SDLK_KP_DECIMAL,
        [hmKeyCode.KpDivide] = SDL_Keycode.SDLK_KP_DIVIDE,
        [hmKeyCode.KpMultiply] = SDL_Keycode.SDLK_KP_MULTIPLY,
        [hmKeyCode.KpSubtract] = SDL_Keycode.SDLK_KP_MINUS,
        [hmKeyCode.KpAdd] = SDL_Keycode.SDLK_KP_PLUS,
        [hmKeyCode.KpEnter] = SDL_Keycode.SDLK_KP_ENTER,
        [hmKeyCode.KpEqual] = SDL_Keycode.SDLK_KP_EQUALS,
        [hmKeyCode.LeftShift] = SDL_Keycode.SDLK_LSHIFT,
        [hmKeyCode.LeftControl] = SDL_Keycode.SDLK_LCTRL,
        [hmKeyCode.LeftAlt] = SDL_Keycode.SDLK_LALT,
        [hmKeyCode.LeftSuper] = SDL_Keycode.SDLK_LMETA,
        [hmKeyCode.RightShift] = SDL_Keycode.SDLK_RSHIFT,
        [hmKeyCode.RightControl] = SDL_Keycode.SDLK_RCTRL,
        [hmKeyCode.RightAlt] = SDL_Keycode.SDLK_RALT,
        [hmKeyCode.RightSuper] = SDL_Keycode.SDLK_RMETA,
        [hmKeyCode.Menu] = SDL_Keycode.SDLK_MENU
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<SDL_Keycode, hmKeyCode> KeyCodesBySdl = KeyCodesByHm
        .Select(kv => new KeyValuePair<SDL_Keycode, hmKeyCode>(kv.Value, kv.Key))
        .ToFrozenDictionary();

    private readonly Dictionary<SDL_WindowID, hmWindow> _windows = [];

    public bool TryInitialize()
    {
        if (SDL_WasInit(SDL_InitFlags.SDL_INIT_VIDEO) != 0) return false;
        return SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO);
    }

    public void Update()
    {
        SDL_Event ev;
        while (SDL_PollEvent(&ev))
        {
            switch (ev.Type)
            {
                case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
                {
                    hmWindow window = _windows[ev.window.windowID];
                    hmLib.Backend.WindowHandleResize(
                        window,
                        Math.Max(1, ev.window.data1),
                        Math.Max(1, ev.window.data2)
                    );
                    window.WasResized = true;
                    break;
                }
                case SDL_EventType.SDL_EVENT_WINDOW_MOVED:
                    _windows[ev.window.windowID].WasMoved = true;
                    break;
                case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                    _windows[ev.window.windowID].ShouldClose = true;
                    break;
                case SDL_EventType.SDL_EVENT_KEY_DOWN or SDL_EventType.SDL_EVENT_KEY_UP:
                {
                    if (!KeyCodesBySdl.TryGetValue(ev.key.key, out hmKeyCode key)) break;
                    _windows[ev.key.windowID].KeyboardState[key] = ev.key.down;
                    break;
                }
                case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN or SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP:
                    _windows[ev.button.windowID].MouseState[(hmMouseButton)ev.button.button] = ev.button.down;
                    break;
                case SDL_EventType.SDL_EVENT_MOUSE_MOTION:
                    _windows[ev.motion.windowID].MousePosition = new Vector2(ev.motion.x, ev.motion.y);
                    break;
                case SDL_EventType.SDL_EVENT_MOUSE_WHEEL:
                {
                    Vector2 velocity = new(ev.wheel.x, ev.wheel.y);
                    if (ev.wheel.direction == SDL_MouseWheelDirection.SDL_MOUSEWHEEL_FLIPPED) velocity *= -1;
                    _windows[ev.wheel.windowID].MouseWheelVelocity = velocity;
                    break;
                }
            }
        }
    }

    public void Destroy()
    {
        if (SDL_WasInit(SDL_InitFlags.SDL_INIT_VIDEO) != 0) SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO);
        SDL_Quit();
    }

    public hmWindow? MakeWindow(ref readonly hmWindowDescriptor descriptor)
    {
        if (SDL_WasInit(SDL_InitFlags.SDL_INIT_VIDEO) == 0) return null;

        SDL_WindowFlags flags = 0;
        if (hmLib.Backend is hmGLBackend) flags |= SDL_WindowFlags.SDL_WINDOW_OPENGL;
        if (descriptor.AlwaysOnTop) flags |= SDL_WindowFlags.SDL_WINDOW_ALWAYS_ON_TOP;
        if (descriptor.Transparent) flags |= SDL_WindowFlags.SDL_WINDOW_TRANSPARENT;
        if (descriptor.NotFocusable) flags |= SDL_WindowFlags.SDL_WINDOW_NOT_FOCUSABLE;
        flags |= descriptor.Border switch
        {
            hmWindowBorder.Borderless => SDL_WindowFlags.SDL_WINDOW_BORDERLESS,
            hmWindowBorder.Resizable => SDL_WindowFlags.SDL_WINDOW_RESIZABLE,
            _ => 0
        };
        flags |= descriptor.State switch
        {
            hmWindowState.Maximized => SDL_WindowFlags.SDL_WINDOW_MAXIMIZED,
            hmWindowState.Minimized => SDL_WindowFlags.SDL_WINDOW_MINIMIZED,
            hmWindowState.Fullscreen => SDL_WindowFlags.SDL_WINDOW_FULLSCREEN,
            hmWindowState.Hidden => SDL_WindowFlags.SDL_WINDOW_HIDDEN,
            _ => 0
        };

        SDL_Window* win = SDL_CreateWindow(descriptor.Title, descriptor.Size.Width, descriptor.Size.Height, flags);
        if (descriptor.Position is not null)
        {
            SDL_SetWindowPosition(win, descriptor.Position.Value.X, descriptor.Position.Value.Y);
        }

        return _windows[SDL_GetWindowID(win)] = new hmWindow((nint)win, MakeGfxContext(win));

        static hmGfxContext MakeGfxContext(SDL_Window* window)
        {
            switch (hmLib.Backend)
            {
                case hmGLBackend:
                    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_ACCELERATED_VISUAL, 1);
                    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_DOUBLEBUFFER, 1);
                    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_DEPTH_SIZE, 24);
                    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_STENCIL_SIZE, 8);
                    // let's use 3.3 for now
                    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
                    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MINOR_VERSION, 3);

                    SDL_GLContextState* context = SDL_GL_CreateContext(window);
                    SDL_GL_MakeCurrent(window, context);

                    GL gl = GL.GetApi(new hmSDLGLContext(window, context));
                    return new hmGLContext(gl);
                default:
                    // we just hope that no other backend magically appear for now
                    return null;
            }
        }
    }

    public void WindowUpdate(hmWindow window)
    {
        SDL_Window* win = (SDL_Window*)window.Handle;
        if ((SDL_GetWindowFlags(win) & SDL_WindowFlags.SDL_WINDOW_OPENGL) != 0)
        {
            SDL_GL_SwapWindow(win);
        }
    }

    public hmWindowBorder WindowGetBorder(hmWindow window)
    {
        SDL_WindowFlags flags = SDL_GetWindowFlags((SDL_Window*)window.Handle);
        if ((flags & SDL_WindowFlags.SDL_WINDOW_BORDERLESS) != 0)
            return hmWindowBorder.Borderless;
        if ((flags & SDL_WindowFlags.SDL_WINDOW_RESIZABLE) != 0)
            return hmWindowBorder.Resizable;
        return hmWindowBorder.Fixed;
    }

    public void WindowSetBorder(hmWindow window, hmWindowBorder border)
    {
        SDL_Window* win = (SDL_Window*)window.Handle;
        SDL_WindowFlags flags = SDL_GetWindowFlags(win);

        bool isBorderless = (flags & SDL_WindowFlags.SDL_WINDOW_BORDERLESS) != 0;
        bool isResizable = (flags & SDL_WindowFlags.SDL_WINDOW_RESIZABLE) != 0;

        SDL_SetWindowBordered(win, !(isBorderless || border == hmWindowBorder.Borderless));
        SDL_SetWindowResizable(win, isResizable || border == hmWindowBorder.Resizable);
    }

    public hmWindowState WindowGetState(hmWindow window)
    {
        SDL_WindowFlags flags = SDL_GetWindowFlags((SDL_Window*)window.Handle);
        if ((flags & SDL_WindowFlags.SDL_WINDOW_MAXIMIZED) != 0)
            return hmWindowState.Maximized;
        if ((flags & SDL_WindowFlags.SDL_WINDOW_MINIMIZED) != 0)
            return hmWindowState.Minimized;
        if ((flags & SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) != 0)
            return hmWindowState.Fullscreen;
        if ((flags & SDL_WindowFlags.SDL_WINDOW_HIDDEN) != 0)
            return hmWindowState.Hidden;
        return hmWindowState.Normal;
    }

    public void WindowSetState(hmWindow window, hmWindowState state)
    {
        SDL_Window* win = (SDL_Window*)window.Handle;
        SDL_WindowFlags flags = SDL_GetWindowFlags(win);

        bool isMaximized = (flags & SDL_WindowFlags.SDL_WINDOW_MAXIMIZED) != 0;
        bool isMinimized = (flags & SDL_WindowFlags.SDL_WINDOW_MINIMIZED) != 0;
        bool isFullscreen = (flags & SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) != 0;
        bool isHidden = (flags & SDL_WindowFlags.SDL_WINDOW_HIDDEN) != 0;

        if (isMaximized || state == hmWindowState.Maximized) SDL_MaximizeWindow(win);
        else if (isMinimized || state == hmWindowState.Minimized) SDL_MinimizeWindow(win);
        else SDL_RestoreWindow(win);

        SDL_SetWindowFullscreen(win, isFullscreen || state == hmWindowState.Fullscreen);

        if (isHidden || state == hmWindowState.Hidden) SDL_HideWindow(win);
        else SDL_ShowWindow(win);
    }

    public string? WindowGetTitle(hmWindow window)
    {
        return SDL_GetWindowTitle((SDL_Window*)window.Handle);
    }

    public void WindowSetTitle(hmWindow window, string? title)
    {
        SDL_SetWindowTitle((SDL_Window*)window.Handle, title);
    }

    public SizeI WindowGetSize(hmWindow window)
    {
        int w, h;
        SDL_GetWindowSizeInPixels((SDL_Window*)window.Handle, &w, &h);
        return new SizeI(w, h);
    }

    public void WindowSetSize(hmWindow window, SizeI size)
    {
        SDL_SetWindowSize((SDL_Window*)window.Handle, size.Width, size.Height);
    }

    public SizeI WindowGetMinSize(hmWindow window)
    {
        int w, h;
        SDL_GetWindowMinimumSize((SDL_Window*)window.Handle, &w, &h);
        return new SizeI(w, h);
    }

    public void WindowSetMinSize(hmWindow window, SizeI minSize)
    {
        SDL_SetWindowMinimumSize((SDL_Window*)window.Handle, minSize.Width, minSize.Height);
    }

    public SizeI WindowGetMaxSize(hmWindow window)
    {
        int w, h;
        SDL_GetWindowMaximumSize((SDL_Window*)window.Handle, &w, &h);
        return new SizeI(w, h);
    }

    public void WindowSetMaxSize(hmWindow window, SizeI maxSize)
    {
        SDL_SetWindowMaximumSize((SDL_Window*)window.Handle, maxSize.Width, maxSize.Height);
    }

    public Int2 WindowGetPosition(hmWindow window)
    {
        int x, y;
        SDL_GetWindowPosition((SDL_Window*)window.Handle, &x, &y);
        return new Int2(x, y);
    }

    public void WindowSetPosition(hmWindow window, Int2 position)
    {
        SDL_SetWindowPosition((SDL_Window*)window.Handle, position.X, position.Y);
    }

    public float WindowGetOpacity(hmWindow window)
    {
        return SDL_GetWindowOpacity((SDL_Window*)window.Handle);
    }

    public void WindowSetOpacity(hmWindow window, float opacity)
    {
        SDL_SetWindowOpacity((SDL_Window*)window.Handle, opacity);
    }

    public void DestroyWindow(hmWindow window)
    {
        SDL_Window* win = (SDL_Window*)window.Handle;

        _windows.Remove(SDL_GetWindowID(win));
        SDL_DestroyWindow(win);
    }

    public void InputWarpMouse(hmWindow window, Vector2 position)
    {
        SDL_WarpMouseInWindow((SDL_Window*)window.Handle, position.X, position.Y);
    }

    public Vector2 InputGetMouseVelocity(hmWindow window)
    {
        if (SDL_GetWindowRelativeMouseMode((SDL_Window*)window.Handle))
        {
            float x, y;
            SDL_GetRelativeMouseState(&x, &y);
            return new Vector2(x, y);
        }

        return window.MousePosition - window.LastMousePosition;
    }

    public void InputSetMouseLocked(hmWindow window, bool locked)
    {
        SDL_SetWindowRelativeMouseMode((SDL_Window*)window.Handle, locked);
    }

    public bool InputIsMouseLocked(hmWindow window)
    {
        return SDL_GetWindowRelativeMouseMode((SDL_Window*)window.Handle);
    }
}