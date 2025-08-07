using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;

namespace FractalPainting.Infrastructure.UiActions;

public static class UiActionExtensions
{
	public static MenuItem[] ToMenuItems(this IUiAction[] actions)
	{
		var items = actions.GroupBy(a => a.Category)
			.OrderBy(a => a.Key)
			.Select(g => CreateTopLevelMenuItem(g.Key, g.ToList()))
			.ToArray();
		return items;
	}

	private static MenuItem CreateTopLevelMenuItem(MenuCategory category, IList<IUiAction> items)
	{
		var menuItems = items.Select(a => a.ToMenuItem()).ToArray();
		return new MenuItem
		{
			Header = category.GetDescription(),
			ItemsSource = menuItems
		};
	}

	private static MenuItem ToMenuItem(this IUiAction action)
	{
		return new MenuItem
		{
			Header = action.Name,
			Command = action,
			Tag = action
		};
	}
}