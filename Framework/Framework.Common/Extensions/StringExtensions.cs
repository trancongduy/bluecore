using System;
using System.Reflection;

namespace Framework.Common.Extensions
{
	public static class StringExtensions
	{
		public static TOut ParseOrDefault<TOut>(this string input)
		{
			return input.ParseOrDefault(default(TOut));
		}

		public static TOut ParseOrDefault<TOut>(this string input, TOut defaultValue)
		{
			Type type = typeof(TOut);
            MethodInfo parseMethod = type.GetTypeInfo().GetMethod("Parse", new[] { typeof(string) });

		    if (parseMethod == null) return defaultValue;

		    try
		    {
		        var value = parseMethod.Invoke(null, new object[] {input});
		        return value is TOut @out ? @out : defaultValue;
		    }
		    catch
		    {
		        return defaultValue;
		    }
		}

		public static bool TryParseOrDefault<TOut>(this string input, out TOut output)
		{
			return input.TryParseOrDefault(out output, default(TOut));
		}

		public static bool TryParseOrDefault<TOut>(this string input, out TOut output, TOut defaultValue)
		{
			output = defaultValue;

			Type type = typeof(TOut);
			MethodInfo parseMethod = type.GetTypeInfo().GetMethod(
				"TryParse",
				new[] { typeof(string), typeof(TOut).MakeByRefType() });

		    if (parseMethod == null) return false;

		    object[] parameters = {input, output};
		    try
		    {
		        var value = parseMethod.Invoke(null, parameters);

		        if (value is bool successful)
		        {
		            if (successful)
		            {
		                output = (TOut)parameters[1];
		                return true;
		            }
		        }
		    }
		    catch
		    {
		        return false;
		    }

		    return false;
		}
	}
}
