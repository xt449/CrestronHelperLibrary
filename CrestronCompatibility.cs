using Crestron.SimplSharp;
using System;
using System.IO;
using System.Text;

namespace CrestronHelperLibrary
{
	public static class CrestronCompatibility
	{
		private static readonly StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
		private static readonly CrestronStandardOutput crestronStandardOutput = new CrestronStandardOutput();

		public static void Initialize()
		{
			Console.SetOut(crestronStandardOutput);
		}

		public class CrestronStandardOutput : TextWriter
		{
			public override Encoding Encoding => Encoding.Latin1;

			public override void Write(char value)
			{
				Write(value.ToString());
			}

			public override void Write(char[]? buffer)
			{
				Write(new string(buffer));
			}

			public override void Write(char[] buffer, int index, int count)
			{
				Write(new string(buffer, index, count));
			}

			public override void Write(ReadOnlySpan<char> buffer)
			{
				Write(new string(buffer));
			}

			public override void Write(string? value)
			{
				// Standard output
				standardOutput.Write(value);

				// Crestron output
				CrestronConsole.Print(value ?? string.Empty);
				ErrorLog.Notice(value ?? string.Empty);
			}

			public override void WriteLine()
			{
				// Standard output
				standardOutput.WriteLine();

				// Crestron output
				CrestronConsole.PrintLine(string.Empty);
				ErrorLog.Notice(string.Empty);
			}

			public override void WriteLine(char value)
			{
				WriteLine(value.ToString());
			}

			public override void WriteLine(char[]? buffer)
			{
				WriteLine(new string(buffer));
			}

			public override void WriteLine(char[] buffer, int index, int count)
			{
				WriteLine(new string(buffer, index, count));
			}

			public override void WriteLine(ReadOnlySpan<char> buffer)
			{
				WriteLine(new string(buffer));
			}

			public override void WriteLine(bool value)
			{
				WriteLine(value ? "True" : "False");
			}

			public override void WriteLine(int value)
			{
				WriteLine(value.ToString(FormatProvider));
			}

			public override void WriteLine(uint value)
			{
				WriteLine(value.ToString(FormatProvider));
			}

			public override void WriteLine(long value)
			{
				WriteLine(value.ToString(FormatProvider));
			}

			public override void WriteLine(ulong value)
			{
				WriteLine(value.ToString(FormatProvider));
			}

			public override void WriteLine(float value)
			{
				WriteLine(value.ToString(FormatProvider));
			}

			public override void WriteLine(double value)
			{
				WriteLine(value.ToString(FormatProvider));
			}

			public override void WriteLine(decimal value)
			{
				WriteLine(value.ToString(FormatProvider));
			}

			public override void WriteLine(string? value)
			{
				// Standard output
				standardOutput.WriteLine(value);

				// Crestron output
				CrestronConsole.PrintLine(value ?? string.Empty);
				ErrorLog.Notice(value ?? string.Empty);
			}

			public override void WriteLine(StringBuilder? value)
			{
				Write(value);
				WriteLine();
			}
		}
	}
}
