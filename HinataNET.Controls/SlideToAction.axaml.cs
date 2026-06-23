using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace HinataNET.Controls;

public partial class SlideToAction : UserControl
{
    private double _currentX = 0;
    private readonly double _maxX = 256;
    private bool _isCompleted = false;

    private readonly Color _startColor = Color.Parse("#6366F1");
    private readonly Color _endColor = Color.Parse("#10B981");

    public static readonly RoutedEvent<RoutedEventArgs> CompletedEvent =
        RoutedEvent.Register<SlideToAction, RoutedEventArgs>(nameof(Completed), RoutingStrategies.Bubble);

    public event EventHandler<RoutedEventArgs> Completed
    {
        add => AddHandler(CompletedEvent, value);
        remove => RemoveHandler(CompletedEvent, value);
    }

    public SlideToAction()
    {
        InitializeComponent();

        SliderThumb.DragStarted += SliderThumb_DragStarted;
        SliderThumb.DragDelta += SliderThumb_DragDelta;
        SliderThumb.DragCompleted += SliderThumb_DragCompleted;
    }

    private void SliderThumb_DragStarted(object? sender, VectorEventArgs e)
    {
        if (_isCompleted) return;

        SliderThumb.Classes.Remove("animating");
        FillBorder.Classes.Remove("animating");

        SuccessText.IsVisible = true;
    }

    private void SliderThumb_DragDelta(object? sender, VectorEventArgs e)
    {
        if (_isCompleted) return;

        _currentX += e.Vector.X;
        if (_currentX < 0) _currentX = 0;
        if (_currentX > _maxX) _currentX = _maxX;

        UpdateVisuals();

        HintText.Opacity = 1.0 - (_currentX / _maxX);

        double progress = _currentX / _maxX;
        byte r = (byte)(_startColor.R + (_endColor.R - _startColor.R) * progress);
        byte g = (byte)(_startColor.G + (_endColor.G - _startColor.G) * progress);
        byte b = (byte)(_startColor.B + (_endColor.B - _startColor.B) * progress);

        FillBorder.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
    }

    private void UpdateVisuals()
    {
        SliderThumb.Margin = new Thickness(6 + _currentX, 0, 0, 0);
        FillBorder.Width = 64 + _currentX;
    }

    private async void SliderThumb_DragCompleted(object? sender, VectorEventArgs e)
    {
        if (_isCompleted) return;

        SliderThumb.Classes.Add("animating");
        FillBorder.Classes.Add("animating");

        if (_currentX >= _maxX * 0.95)
        {
            _currentX = _maxX;
            _isCompleted = true;
            UpdateVisuals();
            HintText.Opacity = 0;

            FillBorder.Background = new SolidColorBrush(_endColor);

            FillBorder.Classes.Add("success");
            SliderThumb.Classes.Add("success");
            SuccessText.Opacity = 1;

            RaiseEvent(new RoutedEventArgs(CompletedEvent));

            await Task.Delay(1500);
            Reset();
        }
        else
        {
            _currentX = 0;
            UpdateVisuals();
            HintText.Opacity = 1;

            FillBorder.ClearValue(Border.BackgroundProperty);
        }
    }

    private void Reset()
    {
        Dispatcher.UIThread.Post(() =>
        {
            _isCompleted = false;
            _currentX = 0;

            SuccessText.IsVisible = false;
            SuccessText.Opacity = 0;

            FillBorder.Classes.Remove("success");
            SliderThumb.Classes.Remove("success");

            UpdateVisuals();
            HintText.Opacity = 1;

            FillBorder.ClearValue(Border.BackgroundProperty);
        });
    }
}