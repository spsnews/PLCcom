using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCcom.Helpers;
using PLCcom.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PLCcom.View;
//using System.Net;

namespace PLCcom.ViewModel;

 //  [QueryProperty(nameof(Count), nameof(Count))]


public partial class SetupViewModel : ObservableObject
{

   //  DetailViewModel dv= new DetailViewModel(this);
    
    //[ObservableProperty]
    //int count;

    //  public PLCcomDevice Device;

    [ObservableProperty]
    string timeAktuell = "23";

    [ObservableProperty]
    ConnectionStates connectionState;

    [ObservableProperty]
    string ipAddress;

    [ObservableProperty]
    bool pumpState;

    [ObservableProperty]
    int tankLevel;

    [ObservableProperty]
    bool highLimit;

    [ObservableProperty]
    bool lowLimit;

    [ObservableProperty]
    string selectedItemPlcType;

    //   public string SelectedItemPlcType { get; set; }

    //[RelayCommand]
    //Task Back() => Shell.Current.GoToAsync("..");

    [RelayCommand]
    Task BackDetail() => Shell.Current.GoToAsync("../DetailPage");
    //TODO: Refactor GoToAsync to nameof(...)
    //TODO: Remove commented out code

    [RelayCommand]
    Task GoToOne() =>
    Shell.Current.GoToAsync("../OnePage");

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
        _plcService.Connect(IpAddress, 0, 1, 2);

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

    }

    [RelayCommand]
    async Task DisconnectAsync()
    {
        _plcService.Disconnect();
        //Device.DisConnect();
    }

    [RelayCommand]
    async Task StartAsync()
    {
        await _plcService.WriteStart();
    }

    [RelayCommand]
    async Task StopAsync()
    {
        await _plcService.WriteStop();
    }

    S7PlcComService _plcService;
    string ipaddress;
    public SetupViewModel(S7PlcComService plcService)
    {
        _plcService = plcService;
        ipaddress = "192.168.2.85";

        UpdateThreadGUI();
    }


    private bool stop = true;


    private void UpdateThreadGUI()
    {
        if (stop)
        {
            stop = false;
            //System.Threading.Thread thread = new System.Threading.Thread(ThreadFunction);
            Thread updateThread = new Thread(LongRunning);
            updateThread.Name = "UpdateThread";
            //keine Ahnung ob man das benoetigt
            updateThread.IsBackground = true;
            updateThread.Start();
            //  btnStartTime.Content = "Stop Time";
            //if (beenden)
        }
        else
        {
            stop = true;
            // btnStartTime.Content = "Start Time";
        }
    }



    bool schleifeBearbeiten = true;

    public void LongRunning()
    {
        try
        {
            while (schleifeBearbeiten)
            {
                //Debug.WriteLine("HOT Static thread procedure." + " " + TimeAktuell.ToString());

                UpdateGuiBackroud();

                Thread.Sleep(1000);
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
    }

    private void UpdateGuiBackroud()
    {
        // TimeHOT = "Multithrea" + ccount;
        TimeAktuell = DateTime.Now.ToString();

        ConnectionState = _plcService.ConnectionState;
        PumpState = _plcService.PumpState;
        HighLimit = _plcService.HighLimit;
        LowLimit = _plcService.LowLimit;
        TankLevel = _plcService.TankLevel;
        //Write_Int = _plcService.Write_Int;
        //ScanTime = _plcService.ScanTime;


    }

}


