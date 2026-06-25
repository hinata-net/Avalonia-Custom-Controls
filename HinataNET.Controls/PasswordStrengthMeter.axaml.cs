using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using System.Text.RegularExpressions;

namespace HinataNET.Controls;

public enum PasswordStrength
{
    None,
    TooShort,
    Weak,
    Fair,
    Good,
    Strong
}

public partial class PasswordStrengthMeter : UserControl
{
    private static readonly StreamGeometry EyeOpenIcon = StreamGeometry.Parse("M11.83,9L15,12.16C15,12.11 15,12.05 15,12A3,3 0 0,0 12,9C11.94,9 11.89,9 11.83,9M7.53,9.8L9.08,11.35C9.03,11.56 9,11.77 9,12A3,3 0 0,0 12,15C12.22,15 12.44,14.97 12.65,14.92L14.2,16.47C13.53,16.8 12.79,17 12,17A5,5 0 0,1 7,12C7,11.21 7.2,10.47 7.53,9.8M2,4.27L4.28,6.55L4.73,7C3.08,8.3 1.78,10 1,12C2.73,16.39 7,19.5 12,19.5C13.55,19.5 15.03,19.2 16.38,18.66L16.81,19.08L19.73,22L21,20.73L3.27,3M12,7A5,5 0 0,1 17,12C17,12.64 16.87,13.26 16.64,13.82L19.57,16.75C21.07,15.5 22.27,13.86 23,12C21.27,7.61 17,4.5 12,4.5C10.6,4.5 9.26,4.75 8,5.2L10.17,7.35C10.74,7.13 11.35,7 12,7Z");
    private static readonly StreamGeometry EyeClosedIcon = StreamGeometry.Parse("M12,9A3,3 0 0,0 9,12A3,3 0 0,0 12,15A3,3 0 0,0 15,12A3,3 0 0,0 12,9M12,17A5,5 0 0,1 7,12A5,5 0 0,1 12,7A5,5 0 0,1 17,12A5,5 0 0,1 12,17M12,4.5C7,4.5 2.73,7.61 1,12C2.73,16.39 7,19.5 12,19.5C17,19.5 21.27,16.39 23,12C21.27,7.61 17,4.5 12,4.5Z");

    private static readonly IBrush DefaultBrush = SolidColorBrush.Parse("#20FFFFFF");
    private static readonly IBrush ErrorBrush = SolidColorBrush.Parse("#F43F5E");
    private static readonly IBrush WarningBrush = SolidColorBrush.Parse("#F59E0B");
    private static readonly IBrush SuccessBrush = SolidColorBrush.Parse("#3B82F6");
    private static readonly IBrush PerfectBrush = SolidColorBrush.Parse("#10B981");

    private static readonly Regex DigitRegex = new(@"\d", RegexOptions.Compiled);
    private static readonly Regex UpperRegex = new(@"\p{Lu}", RegexOptions.Compiled);
    private static readonly Regex SpecialRegex = new(@"[!@#$%^&*(),.?""':{}|<>]", RegexOptions.Compiled);

    public static readonly StyledProperty<string> PasswordProperty = AvaloniaProperty.Register<PasswordStrengthMeter, string>(nameof(Password), string.Empty, defaultBindingMode: BindingMode.TwoWay);
    public static readonly StyledProperty<string> PlaceholderProperty = AvaloniaProperty.Register<PasswordStrengthMeter, string>(nameof(Placeholder), "Enter your password");
    public static readonly StyledProperty<int> MinLengthProperty = AvaloniaProperty.Register<PasswordStrengthMeter, int>(nameof(MinLength), 8);
    public static readonly StyledProperty<bool> RequireDigitProperty = AvaloniaProperty.Register<PasswordStrengthMeter, bool>(nameof(RequireDigit), true);
    public static readonly StyledProperty<bool> RequireUppercaseProperty = AvaloniaProperty.Register<PasswordStrengthMeter, bool>(nameof(RequireUppercase), true);
    public static readonly StyledProperty<bool> RequireSpecialCharProperty = AvaloniaProperty.Register<PasswordStrengthMeter, bool>(nameof(RequireSpecialChar), true);
    public static readonly StyledProperty<PasswordStrength> StrengthProperty = AvaloniaProperty.Register<PasswordStrengthMeter, PasswordStrength>(nameof(Strength), PasswordStrength.None);

    public string Password { get => GetValue(PasswordProperty); set => SetValue(PasswordProperty, value); }
    public string Placeholder { get => GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
    public int MinLength { get => GetValue(MinLengthProperty); set => SetValue(MinLengthProperty, value); }
    public bool RequireDigit { get => GetValue(RequireDigitProperty); set => SetValue(RequireDigitProperty, value); }
    public bool RequireUppercase { get => GetValue(RequireUppercaseProperty); set => SetValue(RequireUppercaseProperty, value); }
    public bool RequireSpecialChar { get => GetValue(RequireSpecialCharProperty); set => SetValue(RequireSpecialCharProperty, value); }
    public PasswordStrength Strength { get => GetValue(StrengthProperty); set => SetValue(StrengthProperty, value); }

    public PasswordStrengthMeter()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PasswordProperty ||
            change.Property == MinLengthProperty ||
            change.Property == RequireDigitProperty ||
            change.Property == RequireUppercaseProperty ||
            change.Property == RequireSpecialCharProperty)
        {
            ValidatePassword();
        }
    }

    private void RevealButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (RevealButton.IsChecked == true)
        {
            PasswordBox.PasswordChar = '\0';
            EyeIcon.Data = EyeOpenIcon;
            EyeIcon.Foreground = SolidColorBrush.Parse("#FFFFFF");
        }
        else
        {
            PasswordBox.PasswordChar = '•';
            EyeIcon.Data = EyeClosedIcon;
            EyeIcon.Foreground = SolidColorBrush.Parse("#80FFFFFF");
        }
    }

    private void ValidatePassword()
    {
        string currentPassword = Password ?? string.Empty;

        if (string.IsNullOrEmpty(currentPassword))
        {
            SetStrength(PasswordStrength.None, 0, DefaultBrush, "Password strength");
            return;
        }

        if (currentPassword.Length < MinLength)
        {
            SetStrength(PasswordStrength.TooShort, 0.10, ErrorBrush, "Too short");
            return;
        }

        int score = 1;

        if (!RequireUppercase || UpperRegex.IsMatch(currentPassword)) score++;
        if (!RequireDigit || DigitRegex.IsMatch(currentPassword)) score++;
        if (!RequireSpecialChar || SpecialRegex.IsMatch(currentPassword)) score++;

        switch (score)
        {
            case 1: SetStrength(PasswordStrength.Weak, 0.25, ErrorBrush, "Weak"); break;
            case 2: SetStrength(PasswordStrength.Fair, 0.50, WarningBrush, "Fair"); break;
            case 3: SetStrength(PasswordStrength.Good, 0.75, SuccessBrush, "Good"); break;
            case 4: SetStrength(PasswordStrength.Strong, 1.0, PerfectBrush, "Strong"); break;
        }
    }

    private void SetStrength(PasswordStrength strength, double scaleX, IBrush colorBrush, string text)
    {
        Strength = strength;

        var builder = new TransformOperations.Builder(1);
        builder.AppendScale(scaleX, 1);

        if (StrengthIndicator != null)
        {
            StrengthIndicator.RenderTransform = builder.Build();
            StrengthIndicator.Background = colorBrush;
        }

        if (StrengthText != null)
        {
            StrengthText.Text = text;
            StrengthText.Foreground = colorBrush;
        }
    }
}