using gym_rat.ViewModels;

namespace gym_rat.Views;

public partial class LoginPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
	public LoginPage(LoginViewModel viewModel, IServiceProvider serviceProvider)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_serviceProvider = serviceProvider;
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		var registerPage = _serviceProvider.GetRequiredService<RegisterPage>();
		await Navigation.PushAsync(registerPage);
	}
}