using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace HinataNET.Controls;

public partial class GlassProfileCard : UserControl
{
    public static readonly StyledProperty<string> UserNameProperty =
        AvaloniaProperty.Register<GlassProfileCard, string>(nameof(UserName), "User Name");

    public static readonly StyledProperty<string> UserRoleProperty =
        AvaloniaProperty.Register<GlassProfileCard, string>(nameof(UserRole), "Role");

    public static readonly StyledProperty<string> ShotsCountProperty =
        AvaloniaProperty.Register<GlassProfileCard, string>(nameof(ShotsCount), "0");

    public static readonly StyledProperty<string> FollowersCountProperty =
        AvaloniaProperty.Register<GlassProfileCard, string>(nameof(FollowersCount), "0");

    public static readonly StyledProperty<string> RatingProperty =
        AvaloniaProperty.Register<GlassProfileCard, string>(nameof(Rating), "0.0");

    public static readonly StyledProperty<bool> IsFollowingProperty =
        AvaloniaProperty.Register<GlassProfileCard, bool>(nameof(IsFollowing));

    public static readonly StyledProperty<bool> IsMessageSentProperty =
        AvaloniaProperty.Register<GlassProfileCard, bool>(nameof(IsMessageSent));

    public static readonly StyledProperty<ICommand?> FollowCommandProperty =
        AvaloniaProperty.Register<GlassProfileCard, ICommand?>(nameof(FollowCommand));

    public static readonly StyledProperty<ICommand?> MessageCommandProperty =
        AvaloniaProperty.Register<GlassProfileCard, ICommand?>(nameof(MessageCommand));

    public string UserName
    {
        get => GetValue(UserNameProperty);
        set => SetValue(UserNameProperty, value);
    }

    public string UserRole
    {
        get => GetValue(UserRoleProperty);
        set => SetValue(UserRoleProperty, value);
    }

    public string ShotsCount
    {
        get => GetValue(ShotsCountProperty);
        set => SetValue(ShotsCountProperty, value);
    }

    public string FollowersCount
    {
        get => GetValue(FollowersCountProperty);
        set => SetValue(FollowersCountProperty, value);
    }

    public string Rating
    {
        get => GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }

    public bool IsFollowing
    {
        get => GetValue(IsFollowingProperty);
        set => SetValue(IsFollowingProperty, value);
    }

    public bool IsMessageSent
    {
        get => GetValue(IsMessageSentProperty);
        set => SetValue(IsMessageSentProperty, value);
    }

    public ICommand? FollowCommand
    {
        get => GetValue(FollowCommandProperty);
        set => SetValue(FollowCommandProperty, value);
    }

    public ICommand? MessageCommand
    {
        get => GetValue(MessageCommandProperty);
        set => SetValue(MessageCommandProperty, value);
    }

    public GlassProfileCard()
    {
        InitializeComponent();
    }
}