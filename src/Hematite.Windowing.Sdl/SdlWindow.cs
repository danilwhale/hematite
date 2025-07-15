using SDL;
using Vortice.Mathematics;
using static SDL.SDL3;

namespace Hematite.Windowing.SDL;

public sealed unsafe class SdlWindow : Window
{
    public override nint Handle => (nint)_window;
    public override bool ShouldClose { get; protected set; }

    public override WindowBorder Border
    {
        get
        {
            SDL_WindowFlags flags = SDL_GetWindowFlags(_window);
            if ((flags & SDL_WindowFlags.SDL_WINDOW_BORDERLESS) != 0)
                return WindowBorder.Borderless;
            if ((flags & SDL_WindowFlags.SDL_WINDOW_RESIZABLE) != 0)
                return WindowBorder.Resizable;

            return WindowBorder.Fixed;
        }
        set
        {
            SDL_WindowFlags flags = SDL_GetWindowFlags(_window);

            bool isBorderless = (flags & SDL_WindowFlags.SDL_WINDOW_BORDERLESS) != 0;
            bool isResizable = (flags & SDL_WindowFlags.SDL_WINDOW_RESIZABLE) != 0;

            SDL_SetWindowBordered(_window, !(isBorderless || value == WindowBorder.Borderless));
            SDL_SetWindowResizable(_window, isResizable || value == WindowBorder.Resizable);
        }
    }

    public override WindowState State
    {
        get
        {
            SDL_WindowFlags flags = SDL_GetWindowFlags(_window);
            if ((flags & SDL_WindowFlags.SDL_WINDOW_MAXIMIZED) != 0)
                return WindowState.Maximized;
            if ((flags & SDL_WindowFlags.SDL_WINDOW_MINIMIZED) != 0)
                return WindowState.Minimized;
            if ((flags & SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) != 0)
                return WindowState.Fullscreen;
            if ((flags & SDL_WindowFlags.SDL_WINDOW_HIDDEN) != 0)
                return WindowState.Hidden;

            return WindowState.Normal;
        }
        set
        {
            SDL_WindowFlags flags = SDL_GetWindowFlags(_window);

            bool isMaximized = (flags & SDL_WindowFlags.SDL_WINDOW_MAXIMIZED) != 0;
            bool isMinimized = (flags & SDL_WindowFlags.SDL_WINDOW_MINIMIZED) != 0;
            bool isFullscreen = (flags & SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) != 0;
            bool isHidden = (flags & SDL_WindowFlags.SDL_WINDOW_HIDDEN) != 0;

            if (isMaximized || value == WindowState.Maximized) SDL_MaximizeWindow(_window);
            else if (isMinimized || value == WindowState.Minimized) SDL_MinimizeWindow(_window);
            else SDL_RestoreWindow(_window);

            SDL_SetWindowFullscreen(_window, isFullscreen || value == WindowState.Fullscreen);

            if (isHidden || value == WindowState.Hidden) SDL_HideWindow(_window);
            else SDL_ShowWindow(_window);
        }
    }

    public override string? Title
    {
        get => SDL_GetWindowTitle(_window);
        set => SDL_SetWindowTitle(_window, value);
    }

    public override SizeI Size
    {
        get
        {
            int w, h;
            SDL_GetWindowSizeInPixels(_window, &w, &h);
            return new SizeI(w, h);
        }
        set => SDL_SetWindowSize(_window, value.Width, value.Height);
    }

    public override SizeI MinSize
    {
        get
        {
            int w, h;
            SDL_GetWindowMinimumSize(_window, &w, &h);
            return new SizeI(w, h);
        }
        set => SDL_SetWindowMinimumSize(_window, value.Width, value.Height);
    }

    public override SizeI MaxSize
    {
        get
        {
            int w, h;
            SDL_GetWindowMaximumSize(_window, &w, &h);
            return new SizeI(w, h);
        }
        set => SDL_SetWindowMaximumSize(_window, value.Width, value.Height);
    }

    public override Int2 Position
    {
        get
        {
            int x, y;
            SDL_GetWindowPosition(_window, &x, &y);
            return new Int2(x, y);
        }
        set => SDL_SetWindowPosition(_window, value.X, value.Y);
    }

    public override float Opacity
    {
        get => SDL_GetWindowOpacity(_window);
        set => SDL_SetWindowOpacity(_window, value);
    }

    public override event Action<Int2>? Resized;
    public override event Action<Int2>? Moved;

    private readonly SDL_Window* _window;

    internal SdlWindow(SDL_Window* window)
    {
        _window = window;
    }

    public override void Update()
    {
        SDL_Event ev;
        while (SDL_PollEvent(&ev))
        {
            switch (ev.Type)
            {
                case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
                    Resized?.Invoke(new Int2(ev.window.data1, ev.window.data2));
                    break;
                case SDL_EventType.SDL_EVENT_WINDOW_MOVED:
                    Moved?.Invoke(new Int2(ev.window.data1, ev.window.data2));
                    break;
                case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                    ShouldClose = true;
                    break;
                // TODO: input
            }
        }
    }

    public override void Dispose()
    {
        SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO);
    }
}