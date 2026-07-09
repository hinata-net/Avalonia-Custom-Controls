using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Transformation;
using System.Runtime.CompilerServices;

namespace HinataNET.Controls;

public class MagneticEffect : AvaloniaObject
{
    public static readonly AttachedProperty<bool> IsActiveProperty =
        AvaloniaProperty.RegisterAttached<MagneticEffect, Control, bool>("IsActive", false);

    public static readonly AttachedProperty<double> RadiusProperty =
        AvaloniaProperty.RegisterAttached<MagneticEffect, Control, double>("Radius", 120.0);

    public static readonly AttachedProperty<double> PullProperty =
        AvaloniaProperty.RegisterAttached<MagneticEffect, Control, double>("Pull", 0.3);

    public static void SetIsActive(Control element, bool value) => element.SetValue(IsActiveProperty, value);
    public static bool GetIsActive(Control element) => element.GetValue(IsActiveProperty);
    public static void SetRadius(Control element, double value) => element.SetValue(RadiusProperty, value);
    public static double GetRadius(Control element) => element.GetValue(RadiusProperty);
    public static void SetPull(Control element, double value) => element.SetValue(PullProperty, value);
    public static double GetPull(Control element) => element.GetValue(PullProperty);

    static MagneticEffect()
    {
        IsActiveProperty.Changed.AddClassHandler<Control>(OnIsActiveChanged);
    }

    private static readonly ConditionalWeakTable<Control, MagneticState> _states = [];

    private static void OnIsActiveChanged(Control control, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is true)
        {
            control.AttachedToVisualTree += OnAttached;
            control.DetachedFromVisualTree += OnDetached;

            var topLevel = TopLevel.GetTopLevel(control);
            if (topLevel != null)
            {
                AttachState(control, topLevel);
            }
        }
        else
        {
            control.AttachedToVisualTree -= OnAttached;
            control.DetachedFromVisualTree -= OnDetached;
            DetachState(control);
        }
    }

    private static void OnAttached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is Control control)
        {
            var topLevel = TopLevel.GetTopLevel(control);
            if (topLevel != null)
            {
                AttachState(control, topLevel);
            }
        }
    }

    private static void OnDetached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is Control control)
        {
            DetachState(control);
        }
    }

    private static void AttachState(Control control, TopLevel topLevel)
    {
        if (!_states.TryGetValue(control, out _))
        {
            var state = new MagneticState(control);
            _states.Add(control, state);
            state.Attach(topLevel);
        }
    }

    private static void DetachState(Control control)
    {
        if (_states.TryGetValue(control, out var state))
        {
            state.Detach();
            _states.Remove(control);
        }
    }

    private class MagneticState(Control target)
    {
        private readonly Control _target = target;
        private TopLevel? _topLevel;

        public void Attach(TopLevel topLevel)
        {
            _topLevel = topLevel;
            _topLevel.PointerMoved += PointerMoved;
            _topLevel.PointerExited += PointerExited;
        }

        public void Detach()
        {
            if (_topLevel != null)
            {
                _topLevel.PointerMoved -= PointerMoved;
                _topLevel.PointerExited -= PointerExited;
                _topLevel = null;
            }
            ResetPosition();
        }

        private void PointerMoved(object? sender, PointerEventArgs e)
        {
            if (_target.Parent is Visual visualParent)
            {
                var pos = e.GetPosition(visualParent);
                var center = new Point(_target.Bounds.Center.X, _target.Bounds.Center.Y);
                double dx = pos.X - center.X;
                double dy = pos.Y - center.Y;
                double distance = Math.Sqrt(dx * dx + dy * dy);

                double radius = GetRadius(_target);
                double pull = GetPull(_target);

                if (distance < radius)
                {
                    _target.Classes.Remove("magnetic-returning");

                    double power = Math.Pow(1.0 - (distance / radius), 2);
                    double factor = pull * power;
                    double offsetX = dx * factor;
                    double offsetY = dy * factor;

                    _target.RenderTransform = TransformOperations.Parse(
                        $"translate({offsetX.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)}px, {offsetY.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)}px)");
                }
                else
                {
                    ResetPosition();
                }
            }
        }

        private void PointerExited(object? sender, PointerEventArgs e) => ResetPosition();

        private void ResetPosition()
        {
            if (_target.RenderTransform != null && _target.RenderTransform.ToString() != "none")
            {
                _target.Classes.Add("magnetic-returning");
                _target.RenderTransform = TransformOperations.Parse("none");
            }
        }
    }
}