using Hematite.Graphics.Backends;
using SDL;
using Silk.NET.OpenGL;
using Vortice.Mathematics;
using static SDL.SDL3;

namespace Hematite.Windowing.Platforms;

internal sealed unsafe class hmSDLPlatform : hmIPlatform
{
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
                    hmLib.Backend.WindowHandleResize(
                        _windows[ev.window.windowID],
                        Math.Max(1, ev.window.data1),
                        Math.Max(1, ev.window.data2)
                    );
                    break;
                case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                    _windows[ev.window.windowID].ShouldClose = true;
                    break;
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

    public bool WindowShouldClose(hmWindow window)
    {
        return window.ShouldClose;
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
}