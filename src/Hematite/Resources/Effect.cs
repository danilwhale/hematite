using System.Collections.Frozen;
using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Hematite.Resources;

public sealed unsafe partial class Effect : IDisposable
{
    private static ReadOnlySpan<GLEnum> ProgramResourceProps => [GLEnum.NameLength, GLEnum.Location];

    public readonly GL Gl;
    public readonly uint Program;
    private readonly FrozenDictionary<string, int> _uniforms;

    public Effect(GL gl, string vertexSource, string fragmentSource)
    {
        Gl = gl;

        uint vertex = gl.CreateShader(ShaderType.VertexShader);
        gl.ShaderSource(vertex, vertexSource);
        gl.CompileShader(vertex);
        if (gl.GetShader(vertex, ShaderParameterName.CompileStatus) == 0)
        {
            throw new Exception("failed to compile the vertex shader:\n" + gl.GetShaderInfoLog(vertex));
        }

        uint fragment = gl.CreateShader(ShaderType.FragmentShader);
        gl.ShaderSource(fragment, fragmentSource);
        gl.CompileShader(fragment);
        if (gl.GetShader(fragment, ShaderParameterName.CompileStatus) == 0)
        {
            throw new Exception("failed to compile the fragment shader:\n" + gl.GetShaderInfoLog(fragment));
        }

        Program = gl.CreateProgram();
        gl.AttachShader(Program, vertex);
        gl.AttachShader(Program, fragment);
        gl.LinkProgram(Program);
        if (gl.GetProgram(Program, ProgramPropertyARB.LinkStatus) == 0)
        {
            throw new Exception("failed to link the shader program:\n" + gl.GetProgramInfoLog(Program));
        }

        gl.DeleteShader(vertex);
        gl.DeleteShader(fragment);

        // get amount of active uniforms
        int activeUniforms;
        gl.GetProgramInterface(
            Program,
            ProgramInterface.Uniform,
            ProgramInterfacePName.ActiveResources,
            &activeUniforms
        );

        if (activeUniforms == 0)
        {
            _uniforms = FrozenDictionary<string, int>.Empty;
            return;
        }

        // load buffer with required props
        ReadOnlySpan<GLEnum> props = ProgramResourceProps;
        Span<int> values = stackalloc int[props.Length];

        // now let's get uniforms
        Dictionary<string, int> uniforms = [];
        
        fixed (GLEnum* pProps = props)
        fixed (int* pValues = values)
        {
            byte* nameBuf = stackalloc byte[512];
            for (uint i = 0; i < activeUniforms; i++)
            {
                // get props for a uniform
                gl.GetProgramResource(
                    Program, ProgramInterface.Uniform,
                    i,
                    (uint)props.Length, pProps,
                    (uint)values.Length, null, pValues
                );

                // get uniform name and put location into a dictionary
                gl.GetProgramResourceName(Program, ProgramInterface.Uniform, i, 512, null, nameBuf);
                uniforms[Marshal.PtrToStringUTF8((nint)nameBuf, values[0] - 1)] = values[1];
            }
        }

        _uniforms = uniforms.ToFrozenDictionary();
    }

    public void Use()
    {
        Gl.UseProgram(Program);
    }

    public int GetLocation(string uniform)
    {
        if (_uniforms.Count == 0 || !_uniforms.TryGetValue(uniform, out int location) || location == -1)
        {
#if DEBUG
            throw new ArgumentException("Specified uniform was not found in the shader program", nameof(uniform));
#else
            return -1;
#endif
        }

        return location;
    }

    public void SetValue(int location, float value) => Gl.ProgramUniform1(Program, location, value);
    public void SetValue(int location, int value) => Gl.ProgramUniform1(Program, location, value);
    public void SetValue(int location, uint value) => Gl.ProgramUniform1(Program, location, value);
    public void SetValue(int location, double value) => Gl.ProgramUniform1(Program, location, value);

    public void SetValue(int location, in Vector2 value)
    {
        fixed (Vector2* pValue = &value)
        {
            Gl.ProgramUniform2(Program, location, 1, (float*)pValue);
        }
    }

    public void SetValue(int location, in Vector3 value)
    {
        fixed (Vector3* pValue = &value)
        {
            Gl.ProgramUniform3(Program, location, 1, (float*)pValue);
        }
    }

    public void SetValue(int location, in Vector4 value)
    {
        fixed (Vector4* pValue = &value)
        {
            Gl.ProgramUniform4(Program, location, 1, (float*)pValue);
        }
    }

    public void SetValue(int location, in Matrix3x2 value)
    {
        fixed (Matrix3x2* pValue = &value)
        {
            Gl.ProgramUniformMatrix3x2(Program, location, 1, false, (float*)pValue);
        }
    }

    public void SetValue(int location, in Matrix4x4 value)
    {
        fixed (Matrix4x4* pValue = &value)
        {
            Gl.ProgramUniformMatrix4(Program, location, 1, false, (float*)pValue);
        }
    }

    public void Dispose()
    {
        Gl.DeleteProgram(Program);
    }
}