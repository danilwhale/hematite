using Hematite.Graphics;
using Hematite.Windowing;

namespace Hematite.Platforms;

public abstract class Driver : IEquatable<Driver>
{
    // just laundered Platform code
    private static readonly List<Driver> RegisteredDrivers = [];
    private static Driver? _currentDriver;
    private static string? _targetDriver;

    public static IReadOnlyList<Driver> Drivers => RegisteredDrivers;

    public static bool TryRegister(Driver driver)
    {
        if (RegisteredDrivers.Contains(driver)) return false;

        RegisteredDrivers.Add(driver);
        return true;
    }

    public static Driver? GetForApi(DriverApi api)
    {
        if (_currentDriver is not null && (_currentDriver.Api & api) != 0)
        {
            return _currentDriver;
        }

        if (!string.IsNullOrWhiteSpace(_targetDriver))
        {
            foreach (Driver driver in RegisteredDrivers)
            {
                if (!driver.UniqueName.Equals(_targetDriver, StringComparison.OrdinalIgnoreCase)) continue;

                if ((driver.Api & api) == 0) break; // looks like wanted driver doesn't support required api

                return _currentDriver = driver;
            }
        }

        foreach (Driver driver in RegisteredDrivers)
        {
            if ((driver.Api & api) == 0) continue;

            return _currentDriver = driver;
        }

        return null;
    }

    public static void SetTarget(string uniqueName) => _targetDriver = uniqueName;

    public abstract string Name { get; }
    public abstract string UniqueName { get; }
    public abstract DriverApi Api { get; }

    public abstract GraphicsDevice CreateDevice(Window window, IGraphicsContext context);

    public bool Equals(Driver? other) =>
        other is not null && (ReferenceEquals(this, other) || UniqueName.Equals(other.UniqueName, StringComparison.OrdinalIgnoreCase));

    public override int GetHashCode() => UniqueName.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is Driver other && Equals(other);

    public static bool operator ==(Driver? left, Driver? right) => Equals(left, right);
    public static bool operator !=(Driver? left, Driver? right) => !Equals(left, right);
}