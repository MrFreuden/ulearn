using System;
using System.ComponentModel;
using System.Reflection;

namespace FractalPainting.Infrastructure.UiActions;

public static class EnumExtensions
{
	public static string GetDescription(this Enum enumValue)
	{
		var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
		return fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();
	}
}