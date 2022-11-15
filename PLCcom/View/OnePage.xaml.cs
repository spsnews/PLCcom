using PLCcom.Model;
using PLCcom.ViewModel;

namespace PLCcom.View;

public partial class OnePage : ContentPage
{

    public OnePage(OneViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}