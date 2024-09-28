using gym_rat.ViewModels;

namespace gym_rat.Views;

public partial class LandingPage : ContentPage
{
	public LandingPage(LandingPageViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
		Shell.SetNavBarIsVisible(this, false);
	}
}