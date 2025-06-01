using System.Collections.Frozen;
using System.Numerics;
using Silk.NET.OpenGL;

namespace Hematite.Resources;

public sealed unsafe partial class Effect : IDisposable
{
    public readonly GL Gl;
    public readonly uint Program;

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
    }

    public void Use()
    {
        Gl.UseProgram(Program);
    }

    public int GetLocation(string uniform)
    {
#if !DEBUG
        return Gl.GetUniformLocation(Program, uniform);
#else
        int location = Gl.GetUniformLocation(Program, uniform);
        if (location < 0)
        {
            throw new ArgumentException("Specified uniform was not found in the shader program", nameof(uniform));
        }

        return location;
#endif
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