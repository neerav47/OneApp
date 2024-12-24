namespace OneApp.CustomControls;

public partial class LoadingIndicator : StackLayout
{
	public LoadingIndicator()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
		propertyName: nameof(LoadingText),
		returnType: typeof(string),
		declaringType: typeof(LoadingIndicator),
		defaultValue: "",
		defaultBindingMode: BindingMode.TwoWay
	);

	public string LoadingText
	{
		get { return (string)GetValue(LoadingTextProperty); }
		set { SetValue(LoadingTextProperty, value); }
	}

	public static readonly BindableProperty ColorProperty = BindableProperty.Create(
		propertyName: nameof(Color),
		returnType: typeof(Color),
		declaringType: typeof(LoadingIndicator),
		defaultBindingMode: BindingMode.TwoWay
	);

	public Color Color
	{
		get { return (Color)GetValue(ColorProperty); }
		set { SetValue(ColorProperty, value); }
	}
}