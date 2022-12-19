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
using System.Net;

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
        //https://www.youtube.com/watch?v=bPaTOLxCvKU
        try
        {
            var path = FileSystem.Current.AppDataDirectory;
            var fullpath = Path.Combine(path, "userdata.xml");

            Daten daten = new Daten();

            daten.Ipaddress = IpAddress;
            //daten.Ide = entIde.Text;
            //daten.Programmiersprache = entProgrammiersprache.Text;

            SaveData .SaveDaten(daten, fullpath);

            await Shell.Current.DisplayAlert("Saved!", "Note has been saved!", "OK");
        }
        catch (Exception ex)
        {
            // entProgrammiersprache.Text = ex.Message;
            await Shell.Current.DisplayAlert("ERROR", "you have saved parameter" + ex.ToString(), "", "OK");
        }
    }

  
    [RelayCommand]
    async Task LoadDataAsync()
    {
        //https://www.youtube.com/watch?v=bPaTOLxCvKU

        var path = FileSystem.Current.AppDataDirectory;
        var fullpath = Path.Combine(path, "userdata.xml");
        Debug.WriteLine("fullpath...." + fullpath);

        if (File.Exists(fullpath))
        {
            XmlSerializer xs = new XmlSerializer(typeof(Daten));
            FileStream read = new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Debug.WriteLine("read..." + read);
            Daten daten = (Daten)xs.Deserialize(read);

            IpAddress = daten.Ipaddress;
            //entIde.Text = daten.Ide;
            //entProgrammiersprache.Text = daten.Programmiersprache;

            read.Close();

            await Shell.Current.DisplayAlert("Loaded!", "Note has been loaded!", "OK");
        }
    }

    [RelayCommand]
    async Task LoadStartDataAsync()
    {
        //Hinweise...
        #region
        //https://www.youtube.com/watch?v=KfOutBu83-g
        //Build Action - Content - Copy always - geht nicht .... vielleicht mal was für später...

        //https://www.youtube.com/watch?v=8PLlHDfN-ew
        //auch was für später...

        //https://www.youtube.com/watch?v=gUjb_HCjsCM
        //auch gruscht....

        //https://www.c-sharpcorner.com/UploadFile/rohatash/working-with-file-class-in-C-Sharp/
        //usw...

        //https://www.youtube.com/watch?v=tApBDuVwCrc
        //usw...

        //https://www.youtube.com/watch?v=3xqIXS1SBaU
        //usw...

        // var sourceDirectoryPath = Path.Combine(Environment.CurrentDirectory, "CopyThis");
        // var sourceDirectoryInfo = new DirectoryInfo(sourceDirectoryPath);   

        // var targetDirectoryPath = Path.Combine(Environment.CurrentDirectory, "CopyDestination");
        // var targetDirectoryPath = Path.Combine(FileSystem.Current.AppDataDirectory, "CopyDestination");
        // var targetDirectoryInfo = new DirectoryInfo(targetDirectoryPath);

        // CopyFiles(sourceDirectoryInfo,targetDirectoryInfo) funktioniert nur mit festen Verzeichnis Pfaden deshalb dieser Umweg

        #endregion

        //files direkt unter Namespace auflisten bzw. lesen
        Assembly assembly = Assembly.GetExecutingAssembly();

        #region
        //string[] allNames = assembly.GetManifestResourceNames();
        //foreach (var item in allNames)
        //{
        //    Debug.WriteLine(item);
        //}
        //Debug.WriteLine("test..." + allNames[0]);
        #endregion

        // Debug.WriteLine("1 " + assembly.FullName);
        //https://stackoverflow.com/questions/4405813/copying-embedded-resource-as-file-to-disk-in-c-sharp
        //This is the easiest way to save an embedded resource:
        var stream = assembly.GetManifestResourceStream("PLCcom.userdataEmbededResource.xml");
        //var stream = assembly.GetManifestResourceStream("name of the manifest resourse");

        var destinationpath = FileSystem.Current.AppDataDirectory;
        var fulldestinationpath = Path.Combine(destinationpath, "userdata.xml");
        //da wird die neue *.xml Datei im Arbeitsverzeichnis der App erzeugt
        var fileStream = File.Create(fulldestinationpath);
        //var fileStream = File.Create(@"C:\Temp\Test.xml");
        stream.Seek(0, SeekOrigin.Begin);
        stream.CopyTo(fileStream);
        fileStream.Close();
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


