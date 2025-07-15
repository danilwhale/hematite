using Hematite.Graphics;
using SDL;
using static SDL.SDL3;

namespace Hematite.Windowing.SDL;

public sealed unsafe class SdlPlatform : Platform
{
    public const string Identifier = "hematite.sdl3";
    
    public static void Register()
    {
        TryRegister(new SdlPlatform());
    }

    public override string Name => "SDL3";
    public override string UniqueName => Identifier;

    public override bool IsUsable
    {
        get
        {
            if (!SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO))
            {
                return false;
            }

            SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO);
            return true;
        }
    }

    private SdlPlatform()
    {
    }

    public override Window MakeWindow(in WindowDescriptor descriptor)
    {
        if (!SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO))
        {
            throw new Exception($"Failed to initialize SDL: {SDL_GetError()}");
        }

        SDL_WindowFlags flags = 0;

        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (descriptor.Api)
        {
            case DriverApi.OpenGl:
                flags |= SDL_WindowFlags.SDL_WINDOW_OPENGL;
                break;
            case DriverApi.Vulkan:
                flags |= SDL_WindowFlags.SDL_WINDOW_VULKAN;
                break;
            // directx doesn't need special flags
            case DriverApi.Metal:
                flags |= SDL_WindowFlags.SDL_WINDOW_METAL;
                break;
        }
        if (descriptor.AlwaysOnTop) flags |= SDL_WindowFlags.SDL_WINDOW_ALWAYS_ON_TOP;
        if (descriptor.Transparent) flags |= SDL_WindowFlags.SDL_WINDOW_TRANSPARENT;
        if (descriptor.NotFocusable) flags |= SDL_WindowFlags.SDL_WINDOW_NOT_FOCUSABLE;
        flags |= descriptor.Border switch
        {
            WindowBorder.Borderless => SDL_WindowFlags.SDL_WINDOW_BORDERLESS,
            WindowBorder.Resizable => SDL_WindowFlags.SDL_WINDOW_RESIZABLE,
            _ => 0
        };
        flags |= descriptor.State switch
        {
            WindowState.Maximized => SDL_WindowFlags.SDL_WINDOW_MAXIMIZED,
            WindowState.Minimized => SDL_WindowFlags.SDL_WINDOW_MINIMIZED,
            WindowState.Fullscreen => SDL_WindowFlags.SDL_WINDOW_FULLSCREEN,
            WindowState.Hidden => SDL_WindowFlags.SDL_WINDOW_HIDDEN,
            _ => 0
        };

        SDL_Window* win = SDL_CreateWindow(descriptor.Title, descriptor.Size.Width, descriptor.Size.Height, flags);
        if (descriptor.Position is not null)
        {
            SDL_SetWindowPosition(win, descriptor.Position.Value.X, descriptor.Position.Value.Y);
        }

        return new SdlWindow(win);
    }
}