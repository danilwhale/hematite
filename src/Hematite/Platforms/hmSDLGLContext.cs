using System.Diagnostics.CodeAnalysis;
using SDL;
using Silk.NET.Core.Contexts;
using Silk.NET.Core.Loader;
using static SDL.SDL3;

namespace Hematite.Platforms;

public sealed unsafe class hmSDLGLContext(SDL_Window* window, SDL_GLContextState* context) : IGLContext
{
    public nint Handle => (nint)context;
    public IGLContextSource? Source => null;
    public bool IsCurrent => SDL_GL_GetCurrentContext() == context;

    public IntPtr GetProcAddress(string proc, int? slot = null)
    {
        SDL_ClearError();
        nint address = SDL_GL_GetProcAddress(proc);

        string? error = SDL_GetError();
        if (!string.IsNullOrWhiteSpace(error)) throw new Exception(error);
        if (address == nint.Zero) throw new SymbolLoadingException(proc);
        return address;
    }

    public bool TryGetProcAddress(string proc, [UnscopedRef] out IntPtr addr, int? slot = null)
    {
        SDL_ClearError();
        addr = SDL_GL_GetProcAddress(proc);
        if (!string.IsNullOrWhiteSpace(SDL_GetError())) return false;
        if (addr == nint.Zero) return false;
        return true;
    }

    public void SwapInterval(int interval)
    {
        SDL_GL_SetSwapInterval(interval);
    }

    public void SwapBuffers()
    {
        SDL_GL_SwapWindow(window);
    }

    public void MakeCurrent()
    {
        SDL_GL_MakeCurrent(window, context);
    }

    public void Clear()
    {
        SDL_GL_MakeCurrent(window, null);
    }

    public void Dispose()
    {
        SDL_GL_DestroyContext(context);
    }
}