using Hematite.Graphics;
using SDL;
using static SDL.SDL3;

namespace Hematite.Windowing.SDL;

public sealed unsafe class SdlOpenGLContext : IOpenGLContext
{
    private readonly SDL_Window* _window;

    private readonly SDL_GLContextState* _context;

    public IntPtr Handle => (nint)_context;
    public bool IsCurrent => SDL_GL_GetCurrentContext() == _context;

    internal SdlOpenGLContext(SDL_Window* window, SDL_GLContextState* context)
    {
        _window = window;
        _context = context;
    }

    public string? GetLastError() => SDL_GetError();
    public bool TryGetProcAddress(string name, out IntPtr address) => (address = SDL_GL_GetProcAddress(name)) != 0;
    public void Clear() => SDL_GL_MakeCurrent(_window, null);
    public void MakeCurrent() => SDL_GL_MakeCurrent(_window, _context);
    public void SwapBuffers() => SDL_GL_SwapWindow(_window);
    public void SwapInterval(int interval) => SDL_GL_SetSwapInterval(interval);
    public void Dispose() => SDL_GL_DestroyContext(_context);
}