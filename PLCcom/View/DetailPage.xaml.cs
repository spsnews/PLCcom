using PLCcom.Helpers;
using PLCcom.ViewModel;


namespace PLCcom;

public partial class DetailPage : ContentPage
{
    public DetailPage(DetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    //private async void OnDetailAlertClicked(object sender, EventArgs e)
    //{
    //    await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
    //}
}