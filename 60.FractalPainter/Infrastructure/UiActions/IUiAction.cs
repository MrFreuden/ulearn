using System.Windows.Input;

namespace FractalPainting.Infrastructure.UiActions;

public interface IUiAction: ICommand
{
	MenuCategory Category { get; }
	string Name { get; }
}