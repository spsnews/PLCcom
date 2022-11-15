using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLCcom.Helpers;
using PLCcom.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace PLCcom.ViewModel;




//HOT this is the ols one, without installation of the CommuntyToolKit.MVVM
//public class MainViewModel : INotifyPropertyChanged
//{
//    string text;

//    public string Text {
//        get => text;
//        set
//        {
//            text = value;
//            OnPropertyChanged(nameof(Text));
//        }
//    }
//public event PropertyChangedEventHandler PropertyChanged;
//void OnPropertyChanged(string name) =>
//    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
//}


public partial class MainViewModel : ObservableObject
{
    Monkey monkey;
    IConnectivity connectivity;
    IToast toast;

      public MainViewModel(IConnectivity connectivity, IToast toast)
       {
        monkey = new Monkey { Name = "Mooch" };
        this.connectivity = connectivity;
        this.toast = toast;
    }

    [ObservableProperty]
    int count;

    [RelayCommand]
    async Task IncrementCountAsync()
    {
        if (count == 0)
        {

        }
        Count += 10;
    }

    [RelayCommand]
    Task Navigate() =>
        Shell.Current.GoToAsync($"{nameof(DetailPage)}?Count={Count}",
            new Dictionary<string, object>
            {
                ["Monkey"] = monkey
            });

    [RelayCommand]
    async Task RequestBluetooth()
    {
        if (DeviceInfo.Platform != DevicePlatform.Android)
            return;

        var status = PermissionStatus.Unknown;

        if (DeviceInfo.Version.Major >= 12)
        {
            status = await Permissions.CheckStatusAsync<MyBluetoothPermission>();

            if (status == PermissionStatus.Granted)
                return;

            if (Permissions.ShouldShowRationale<MyBluetoothPermission>())
            {
                await Shell.Current.DisplayAlert("Needs permissions", "BECAUSE!!!", "OK");
            }

            status = await Permissions.RequestAsync<MyBluetoothPermission>();


        }
        else
        {
            status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return;

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                await Shell.Current.DisplayAlert("Needs permissions", "BECAUSE!!!", "OK");
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();


        }


        if (status != PermissionStatus.Granted)
            await Shell.Current.DisplayAlert("Permission required",
                "Location permission is required for bluetooth scanning. " +
                "We do not store or use your location at all.", "OK");
    }

    [RelayCommand]
    async Task CheckInternet()
    {
        NetworkAccess accessType = connectivity.NetworkAccess;

        if (accessType == NetworkAccess.Internet)
        {
            toast.MakeToast("You Have Internet!");
        }
        else
        {
            await Shell.Current.DisplayAlert("Check internet!", $"Current status: {accessType}", "OK");
        }
    }
     
}