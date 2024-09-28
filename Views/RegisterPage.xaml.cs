using gym_rat.ViewModels;

namespace gym_rat.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private async void OnBackToLoginClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}