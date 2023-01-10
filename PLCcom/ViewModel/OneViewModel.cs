//using Android.OS;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PLCcom.Model;
using PLCcom.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PLCcom.ViewModel;

public partial class OneViewModel : ObservableObject
{

    S7PlcComService _plcService;
    PLCcomDevice _client;

    public OneViewModel(S7PlcComService plcService)
    {
        _plcService = plcService;
    }

    public OneViewModel(PLCcomDevice client)
    {
        _client = client;
    }


    [RelayCommand]
    Task BackDetail() => Shell.Current.GoToAsync("../SetupPage");



    [RelayCommand]
    async Task ConnectAsync()
    {
        int plcType = 0;
        //       _client = new TCP_ISO_Device(ipAddress, 0, 1, ePLCType.S7_1200_compatibel);
        //if (selectedItemPlcType == "S7_200")
        //    plcType = 0;
        //if (selectedItemPlcType == "S7_300")
        //    plcType = 1;
        //if (selectedItemPlcType == "S7_1200")
        //    plcType = 2;
        //if (selectedItemPlcType == "S7_1500")
        //    plcType = 3;

        //_plcService.Connect(ipAddress, 0, 0, plcType);
    _plcService.Connect("192.168.10.85", 0, 1, 2);
//        _plcService.Connect("192.168.2.85", 0, 1, 2);

        if (_plcService.ConnectionState == ConnectionStates.Online)
        {
            await Shell.Current.DisplayAlert("Connection successful",
                       "you can change parameter" +
                       "", "OK");
        }
        else
        {
            await Shell.Current.DisplayAlert("Connection not successful",
                       "tray again" +
                       "", "OK");
        }

        //    Debug.WriteLine("Button Connect betättigt");

        //[RelayCommand]
        //async Task StopNeuAsync()
        //{
        //    await _plcService.WriteStop();
        //    // Debug.Writeline("wer");
        //}

    }

    [RelayCommand]
    async Task TestAsync()
    {
        await Shell.Current.DisplayAlert("Test",
                      "tray again" +
                      "", "OK");
    }

    //[RelayCommand]
    // async Task StopNeuAsync()
    // {
    //       await _plcService.WriteStop();
    ////     await WriteToPlc.WriteStop(client);
    ///
    // }

    //[RelayCommand]
    //async Task StopNeuAsync()
    //{
    //    await Task.Run(() =>
    //    {
    //        //await Shell.Current.DisplayAlert("StopNeu",
    //        //              "tray again" +
    //        //              "", "OK");

    //        //await _plcService.WriteStop();
    //        WriteToPlc.WriteStop(_client);
    //    });
    //}


    [RelayCommand]
    async Task StopNeuAsync()
    {
      
      await _plcService.WriteStop();
      //  WriteToPlc.WriteStop(_client);
        //WriteToPlc.WriteStop(_client);

    }
}
