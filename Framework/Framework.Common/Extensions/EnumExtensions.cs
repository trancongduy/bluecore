using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Framework.Common.Extensions
{
	public static class EnumExtension
	{
		public static string GetName<TEnum>(TEnum src)
		{
			return Enum.GetName(typeof(TEnum), src);
		}

		public static IDictionary<int, string> GetAll<TEnum>() where TEnum : struct
		{
			var enumerationType = typeof(TEnum);

			if (!enumerationType.GetTypeInfo().IsEnum)
				throw new ArgumentException("Enumeration type is expected.");

			var dictionary = new Dictionary<int, string>();

			foreach (int value in Enum.GetValues(enumerationType))
			{
				//var name = Enum.GetName(enumerationType, value);

				var name = Enum.Parse(enumerationType, value.ToString()).DisplayName();
				dictionary.Add(value, name);
			}

			return dictionary;
		}

		public static string DisplayName<TEnum>(this TEnum value)
		{
            var attrs = (DescriptionAttribute[])value.GetType().GetRuntimeField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			return attrs.Length > 0 ? attrs[0].Description : Enum.GetName(typeof(TEnum), value);
		}
	}
}
