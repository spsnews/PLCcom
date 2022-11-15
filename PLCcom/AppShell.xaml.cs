using PLCcom.View;

namespace PLCcom;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        Routing.RegisterRoute(nameof(SetupPage), typeof(SetupPage));

        Routing.RegisterRoute(nameof(OnePage), typeof(OnePage));

        Routing.RegisterRoute(nameof(AnotherPage), typeof(AnotherPage));
    }
}
