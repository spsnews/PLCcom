using PLCcom.Model;
using PLCcom.ViewModel;

namespace PLCcom;

public partial class SetupPage : ContentPage
{

    //TODO: Comments
    //TODO: Refactor S7PlcComService

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="vm"></param>
    public SetupPage(SetupViewModel vm)
    {
        InitializeComponent();
       
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}