using Hematite.Windowing;

namespace Hematite;

public abstract class Platform : IEquatable<Platform>
{
    private static readonly List<Platform> RegisteredPlatforms = [];
    private static Platform? _currentPlatform;
    private static string? _targetPlatform;

    public static IReadOnlyList<Platform> Platforms => RegisteredPlatforms;

    public static Platform? Current
    {
        get
        {
            // try to select platform from registered list 
            if (_currentPlatform is null)
            {
                // prioritize target platform specified by the user
                if (!string.IsNullOrWhiteSpace(_targetPlatform))
                {
                    foreach (Platform platform in RegisteredPlatforms)
                    {
                        if (!platform.IsUsable) continue;
                        if (!platform.UniqueName.Equals(_targetPlatform, StringComparison.OrdinalIgnoreCase)) continue;

                        _currentPlatform = platform;
                    }
                }
                
                // fallback to first usable platform
                _currentPlatform ??= RegisteredPlatforms.FirstOrDefault(p => p.IsUsable);
            }

            return _currentPlatform;
        }
    }

    public static bool TryRegister(Platform platform)
    {
        if (RegisteredPlatforms.Contains(platform)) return false;

        RegisteredPlatforms.Add(platform);
        return true;
    }

    public static void SetTarget(string uniqueName) => _targetPlatform = uniqueName;

    public abstract string Name { get; }
    public abstract string UniqueName { get; }
    public abstract bool IsUsable { get; }
    
    public abstract Window MakeWindow(ref readonly WindowDescriptor descriptor);

    public bool Equals(Platform? other) =>
        other is not null && (ReferenceEquals(this, other) || UniqueName.Equals(other.UniqueName, StringComparison.OrdinalIgnoreCase));

    public override int GetHashCode() => UniqueName.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is Platform other && Equals(other);

    public static bool operator ==(Platform? left, Platform? right) => Equals(left, right);
    public static bool operator !=(Platform? left, Platform? right) => !Equals(left, right);
}