using System;
using System.Linq;

public static class StringExtensions
{
	public static readonly char[] TrimCharacters = new char[] { ' ', '\t' };
	public static readonly char[] NewLineTrimCharacters = new char[] { '\r', '\n' };

	public static string TrimWithin(this string input, char[] trimCharacters = null)
	{
		if (trimCharacters == null)
			trimCharacters = TrimCharacters;

		string[] split = input.Split(trimCharacters, StringSplitOptions.RemoveEmptyEntries);
		return string.Join(" ", split);
	}

	public static string TrimNewLines(this string input)
	{
		string[] split = input.Split(NewLineTrimCharacters, StringSplitOptions.RemoveEmptyEntries);
		return string.Join("\r\n", split.Select(el => el.Trim()).Where(s => s != string.Empty));
	}

	public static bool IsNullOrEmpty(this string value)
	{
		return string.IsNullOrEmpty(value);
	}
}