using gym_rat.ViewModels;

namespace gym_rat.Views;

public partial class GoalDetailsPage : ContentPage
{
	public GoalDetailsPage(GoalDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}