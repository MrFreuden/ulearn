using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FractalPainting.App;

partial class MessageBox : Window
{
	public enum MessageBoxButtons
	{
		Ok,
		OkCancel,
		YesNo,
		YesNoCancel
	}

	public enum MessageBoxResult
	{
		Ok,
		Cancel,
		Yes,
		No
	}
	
	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public MessageBox()
	{
		InitializeComponent();
	}

	public static Task<MessageBoxResult> Show(Window? parent, string text, string title, MessageBoxButtons buttons)
	{
		var msgbox = new MessageBox
		{
			Title = title
		};
		msgbox.FindControl<TextBlock>("Text")!.Text = text;
		var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");

		var res = MessageBoxResult.Ok;

		void AddButton(string caption, MessageBoxResult r, bool def = false)
		{
			var btn = new Button {Content = caption};
			btn.Click += (_, __) => { 
				res = r;
				msgbox.Close();
			};
			buttonPanel?.Children.Add(btn);
			if (def)
				res = r;
		}

		switch (buttons)
		{
			case MessageBoxButtons.Ok or MessageBoxButtons.OkCancel:
				AddButton("Ok", MessageBoxResult.Ok, true);
				break;
			case MessageBoxButtons.YesNo or MessageBoxButtons.YesNoCancel:
				AddButton("Yes", MessageBoxResult.Yes);
				AddButton("No", MessageBoxResult.No, true);
				break;
		}

		if (buttons is MessageBoxButtons.OkCancel or MessageBoxButtons.YesNoCancel)
			AddButton("Cancel", MessageBoxResult.Cancel, true);


		var tcs = new TaskCompletionSource<MessageBoxResult>();
		msgbox.Closed += delegate { tcs.TrySetResult(res); };
		if (parent != null)
			msgbox.ShowDialog(parent);
		else msgbox.Show();
		return tcs.Task;
	}
}