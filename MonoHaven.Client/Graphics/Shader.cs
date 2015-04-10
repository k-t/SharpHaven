using System;
using OpenTK.Graphics.OpenGL;

namespace SharpHaven.Graphics
{
	public class Shader : IDisposable
	{
		private readonly int program;
		private readonly int vertexShader;
		private readonly int fragmentShader;

		public Shader(string vertexShader, string fragmentShader)
		{
			this.vertexShader = CreateShader(ShaderType.VertexShader, vertexShader);
			this.fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShader);
			this.program = CreateProgram(this.vertexShader, this.fragmentShader);
		}

		public void Dispose()
		{
			if (program != 0)
				GL.DeleteProgram(program);

			if (vertexShader != 0)
				GL.DeleteShader(vertexShader);

			if (fragmentShader != 0)
				GL.DeleteShader(fragmentShader);
		}

		public void Begin()
		{
			GL.UseProgram(program);
		}

		public void End()
		{
			GL.UseProgram(0);
		}

		public int GetAttributeLocation(string name)
		{
			return GL.GetAttribLocation(program, name);
		}

		private static int CreateProgram(int vertexShader, int fragmentShader)
		{
			int linkStatus;

			int program = GL.CreateProgram();
			GL.AttachShader(program, vertexShader);
			GL.AttachShader(program, fragmentShader);
			GL.LinkProgram(program);
			GL.GetProgram(program, GetProgramParameterName.LinkStatus, out linkStatus);

			if (linkStatus != 1)
			{
				var log = GL.GetProgramInfoLog(program);
				GL.DeleteProgram(program);
				// TODO: specific exceptions
				throw new Exception("Program linking error:\n" + log);
			}

			return program;
		}

		private static int CreateShader(ShaderType type, string src)
		{
			int compileStatus;

			int shader = GL.CreateShader(type);
			GL.ShaderSource(shader, src);
			GL.CompileShader(shader);
			GL.GetShader(shader, ShaderParameter.CompileStatus, out compileStatus);

			if (compileStatus != 1)
			{
				var log = GL.GetShaderInfoLog(shader);
				GL.DeleteShader(shader);
				// TODO: specific exceptions
				throw new Exception("Shader compilation error:\n" + log);
			}

			return shader;
		}
	}
}
