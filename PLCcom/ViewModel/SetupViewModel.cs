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
using System.Reflection;
using PLCcom.Service;
using System.Xml.Serialization;
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
    async Task SaveDataAsync()
    {
        try
        {
            Daten daten = new Daten();
            //DirectoryInfo[] cDirs = new DirectoryInfo(@"c:\").GetDirectories();
            // Set a variable to the Documents path.
            //string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string longfilename = @"" + exePath + "\\userdata.xml";

            daten.Ipaddress = IpAddress;
            //  daten.Ipaddress = "192";
            //daten.Ide = entIde.Text;
            //daten.Programmiersprache = entProgrammiersprache.Text;

            //HOT userdata.xml funktioniert leider nicht         
            //HOT SaveData.SaveDaten(daten, "userdata.xml");
            await SaveData.SaveDatenAsync(daten, longfilename);


            await Shell.Current.DisplayAlert("Save new successful",
         "you have saved parameter" +
         "", "OK");

            //  SaveData.SaveDaten(daten, @"relative/userdata.xml");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("ERROR",
  "you have saved parameter" + ex.ToString(),
  "", "OK");
            //entProgrammiersprache.Text = ex.Message;
        }
    }

    [RelayCommand]
    async Task LoadDataAsync()
    {
        //HOT normalerweise muesste userdata.xml reichen
        string exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string longfilename = @"" + exePath + "\\userdata.xml";

        //HOT if (File.Exists("userdata.xml"))
        if (File.Exists(longfilename))
        {
            XmlSerializer xs = new XmlSerializer(typeof(Daten));
            //HOT    FileStream read = new FileStream("userdata.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream read = new FileStream(longfilename, FileMode.Open, FileAccess.Read, FileShare.Read);
            Daten daten = (Daten)xs.Deserialize(read);

            //entName.Text = daten.Name;
            //entIde.Text = daten.Ide;
            //entProgrammiersprache.Text = daten.Programmiersprache;
            IpAddress = daten.Ipaddress;

            read.Close();
        }
    }

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
    // string ipaddress;
    public SetupViewModel(S7PlcComService plcService)
    {
        _plcService = plcService;
        //    ipaddress = "192.168.2.85";
        //   ipaddress = IpAddress;


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


