using PLCcom.ViewModel;

namespace PLCcom;

//public partial class MainPage : ContentPage
//{
//	int count = 0;

//	public MainPage()
//	{
//		InitializeComponent();
//	}

//	private void OnCounterClicked(object sender, EventArgs e)
//	{
//		count++;

//		if (count == 1)
//			CounterBtn.Text = $"Clicked {count} time";
//		else
//			CounterBtn.Text = $"Clicked {count} times";

//		SemanticScreenReader.Announce(CounterBtn.Text);
//	}
//}

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnAlertClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");

    }
}


