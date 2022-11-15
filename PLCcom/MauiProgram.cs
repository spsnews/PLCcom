using PLCcom.Helpers;
using PLCcom.Model;
using PLCcom.PlatformImplementations;
using PLCcom.View;
using PLCcom.ViewModel;

namespace PLCcom;

//public static class MauiProgram
//{
//	public static MauiApp CreateMauiApp()
//	{
//		var builder = MauiApp.CreateBuilder();
//		builder
//			.UseMauiApp<App>()
//			.ConfigureFonts(fonts =>
//			{
//				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
//				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
//			});

//		return builder.Build();
//	}
//}

public static class MauiProgram
{
     public static MauiApp CreateMauiApp()
    {
   

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IConnectivity>((e) => Connectivity.Current);
        builder.Services.AddSingleton<IToast>((e) => new Toaster());

        //Services
        builder.Services.AddSingleton<S7PlcComService>();

        //ViewModels
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<DetailViewModel>();
        builder.Services.AddTransient<SetupViewModel>();
        builder.Services.AddTransient<OneViewModel>();

        //Pages
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<SetupPage>();
        builder.Services.AddTransient<OnePage>();


        var app = builder.Build();

        return app;
    }
}
