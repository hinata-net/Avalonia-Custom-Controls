using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace HinataNET.Controls;

public partial class ExpandingSearchBar : UserControl
{
    public ExpandingSearchBar()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        topLevel?.AddHandler(PointerPressedEvent, TopLevel_PointerPressed, RoutingStrategies.Tunnel);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        topLevel?.RemoveHandler(PointerPressedEvent, TopLevel_PointerPressed);
    }

    private async void MainButton_OnChecked(object? sender, RoutedEventArgs e)
    {
        var textBox = MainButton.FindDescendantOfType<TextBox>();
        if (textBox == null) return;

        if (MainButton.IsChecked == true)
        {
            await Task.Delay(350);

            if (MainButton.IsChecked == true)
            {
                Dispatcher.UIThread.Post(() => textBox.Focus());
            }
        }
        else
        {
            textBox.Text = string.Empty;
        }
    }

    private void TopLevel_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (MainButton.IsChecked == true)
        {
            if (e.Source is Visual clickSource && !this.IsVisualAncestorOf(clickSource) && clickSource != this)
            {
                MainButton.IsChecked = false;
                TopLevel.GetTopLevel(this)?.Focus();
            }
        }
    }
}