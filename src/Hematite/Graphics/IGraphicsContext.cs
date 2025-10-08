namespace Hematite.Graphics;

public interface IGraphicsContext : IDisposable
{
    nint Handle { get; }

    string? GetLastError();
}