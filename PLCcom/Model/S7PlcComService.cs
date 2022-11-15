using PLCcom.Converters;
using PLCcom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PLCcom.Model
{
   public class S7PlcComService
    {
        //#region Private member
        //PLCcomDevice Device = new TCP_ISO_Device();
        //System.Resources.ResourceManager resources;
        //private delegate void dlgtOnConnectionStateChange(object sender, eConnectionState e);
        //#endregion

        //#region Internal member
        //internal static int CountOpenDialogs = 0;
        //#endregion


        //private readonly S7Client _client;
        // PLCcomDevice _client = new TCP_ISO_Device();

        #region Private member
        PLCcomDevice _client = new TCP_ISO_Device();
        System.Resources.ResourceManager resources;
        private delegate void dlgtOnConnectionStateChange(object sender, eConnectionState e);
        #endregion

        #region Internal member
        internal static int CountOpenDialogs = 0;
        #endregion


        //  PLCcomDevice _client; 
        private readonly System.Timers.Timer _timer;
        private DateTime _lastScanTime;

        private volatile object _locker = new object();
        public S7PlcComService()
        {
            //  _client = new S7Client();
            //   _client = new TCP_ISO_Device();
            _timer = new System.Timers.Timer();
            _timer.Interval = 100;
            _timer.Elapsed += OnTimerElapsed;
        }

        public ConnectionStates ConnectionState { get; private set; }

        public bool HighLimit { get; private set; }

        public bool LowLimit { get; private set; }

        public bool PumpState { get; private set; }

        public int TankLevel { get; private set; }

        public short Read_Int { get; private set; }
        public int Write_Int { get; private set; }

        public TimeSpan ScanTime { get; private set; }


        public event EventHandler ValuesRefreshed;

        public void Connect(string ipAddress, int rack, int slot, int plcType)
        {
            try
            {
                ConnectionState = ConnectionStates.Connecting;

                authentication.User = "walter_gerätebau";
                authentication.Serial = "86900-1919243-102135-2867724";

                //Declare Device and
                //create TCP_ISO_Device instance from PLCcomDevice
                //_client = new TCP_ISO_Device("192.168.10.85", 0, 0, ePLCType.S7_1500_compatibel);
                if (plcType == 0)
                {
                    _client = new TCP_ISO_Device(ipAddress, 0, 1, ePLCType.S7_200_compatibel);
                }
                else if (plcType == 1)
                {
                    _client = new TCP_ISO_Device(ipAddress, 0, 1, ePLCType.S7_300_400_compatibel);
                }
                else if (plcType == 2)
                {
                    _client = new TCP_ISO_Device(ipAddress, 0, 1, ePLCType.S7_1200_compatibel);
                }
                else
                {
                    _client = new TCP_ISO_Device(ipAddress, 0, 1, ePLCType.S7_1500_compatibel);
                }


                //set autoconnect to true and idle time till disconnect to 10000 milliseconds
                _client.setAutoConnect(true, 10000);


                //int result = _client.ConnectTo(ipAddress, rack, slot);
                //int result = _client.Connect();
                ConnectResult res = _client.Connect();
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show(resources.GetString("connect_successful") + Environment.NewLine + resources.GetString("MessageText") + res.Message, "");
                    //        MessageBox.Show("connect_successful" + Environment.NewLine + res.Message.ToString());
                    Debug.WriteLine("connect_successful" + Environment.NewLine + res.Message.ToString());

                    ConnectionState = ConnectionStates.Online;
                    _timer.Start();
                    Debug.WriteLine(ConnectionState);
                }
                else
                {
                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Connection error: " + _client. .ErrorText(result));
                    //   MessageBox.Show(resources.GetString("connect_unsuccessful") + Environment.NewLine + resources.GetString("MessageText") + res.Message, "");
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Connection error: ");
                    ConnectionState = ConnectionStates.Offline;
                }
                OnValuesRefreshed();
            }
            catch
            {
                ConnectionState = ConnectionStates.Offline;
                OnValuesRefreshed();
                throw;
            }
        }

        public void Disconnect()
        {
            if (true)
            {
                _timer.Stop();
                _client.DisConnect();
                ConnectionState = ConnectionStates.Offline;
                OnValuesRefreshed();

                //TODO Gruscht
                HOTkeineVerbindung();
            }
        }

        void HOTkeineVerbindung()
        {
            if (ConnectionState.ToString() == "Offline")
            {
                PumpState = false;
                HighLimit = false;
                LowLimit = false;
                TankLevel = 0;
                Write_Int = 0;
            }
        }

        public async Task WriteStartOriginal()
        {
            await Task.Run(() =>
            {
                int writeResult = WriteBit("DB1.DBX0.0", true);
                if (writeResult != 0)
                {
                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                }
                Thread.Sleep(30);
                writeResult = WriteBit("DB1.DBX0.0", false);
                if (writeResult != 0)
                {
                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                }
            });
        }

        public async Task WriteStartExec()
        {
            await Task.Run(() =>
            {
                int writeResult = WriteBitExec("DB1.DBX0.0", true);
                if (writeResult != 0)
                {
                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                }
                Thread.Sleep(30);
                writeResult = WriteBitExec("DB1.DBX0.0", false);
                if (writeResult != 0)
                {
                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                }
            });
        }

        public async Task StartWriteIntCommand(short wert)
        {
            await Task.Run(() =>
            {
                //string message0 = " Weiter " + wert;
                //MessageBox.Show(message0);
                // int writeResult = WriteIntExec(1, 6, 0, 1086);
                int writeResult = WriteIntExec(1, 6, wert);
            });
        }

        public async Task StartWriteIntCommand(int db, int pos, short wert)
        {
            await Task.Run(() =>
            {
                //string message0 = " Weiter " + wert;
                //MessageBox.Show(message0);
                // int writeResult = WriteIntExec(1, 6, 0, 1086);
                int writeResult = WriteIntExec(db, pos, wert);
            });
        }


        public async Task WriteStart()
        {
            await Task.Run(() =>
            {
                //declare a WriteRequest object and
                //set the request parameters
                WriteDataRequest myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                                                                       1,                   //DB / only for datablock operations otherwise 0
                                                                       0,                   //write start adress
                                                                       0);                  //add writable Data here
                                                                                            //in  this case => write 4 bytes in DB100
                                                                                            //    myWriteRequest.addByte(new byte[] { 11 });

                myWriteRequest.addBit(true);

                //write
                Debug.WriteLine("begin write...");
                WriteDataResult res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: " + res.Message);
                    Debug.WriteLine("Write successfull! Message: " + res.Message);
                }
                else
                {
                    //MessageBox.Show("Write not successfull! Message: " + res.Message);
                    Debug.WriteLine("Write not successfull! Message: " + res.Message);
                }

                //declare a WriteRequest object and
                //set the request parameters
                myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                                                                       1,                 //DB / only for datablock operations otherwise 0
                                                                       0,
                                                                       0);                  //write start adress
                                                                                            //add writable Data here
                                                                                            //in  this case => write 4 bytes in DB100
                                                                                            //    myWriteRequest.addByte(new byte[] { 11 })
                Thread.Sleep(30);
                myWriteRequest.addBit(false);

                //write
                Debug.WriteLine("begin write...");
                res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: RST" + res.Message);
                    Debug.WriteLine("Write successfull! Message: RST" + res.Message);
                }
                else
                {
                    //MessageBox.Show("Write not successfull! Message: RST" + res.Message);
                    Debug.WriteLine("Write not successfull! Message: RST" + res.Message);
                }
            });
        }

        public async Task WriteBit()
        {
            await Task.Run(() =>
            {
                //declare a WriteRequest object and
                //set the request parameters
                WriteDataRequest myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                                                                       1,                   //DB / only for datablock operations otherwise 0
                                                                       0,                   //write start adress
                                                                       5);                  //add writable Data here
                                                                                            //in  this case => write 4 bytes in DB100
                                                                                            //    myWriteRequest.addByte(new byte[] { 11 });

                myWriteRequest.addBit(true);

                //write
                Debug.WriteLine("begin write...");
                WriteDataResult res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: " + res.Message);
                    Debug.WriteLine("Write successfull! Message: " + res.Message);
                }
                else
                {
                    //MessageBox.Show("Write not successfull! Message: " + res.Message);
                    Debug.WriteLine("Write not successfull! Message: " + res.Message);
                }

                //declare a WriteRequest object and
                //set the request parameters
                myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                                                                       1,                 //DB / only for datablock operations otherwise 0
                                                                       0,
                                                                       5);                  //write start adress
                                                                                            //add writable Data here
                                                                                            //in  this case => write 4 bytes in DB100
                                                                                            //    myWriteRequest.addByte(new byte[] { 11 })
                Thread.Sleep(30);
                myWriteRequest.addBit(false);

                //write
                Debug.WriteLine("begin write...");
                res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: RST" + res.Message);
                    Debug.WriteLine("Write successfull! Message: RST" + res.Message);
                }
                else
                {
                    //MessageBox.Show("Write not successfull! Message: RST" + res.Message);
                    Debug.WriteLine("Write not successfull! Message: RST" + res.Message);
                }
            });
        }
        public async Task WriteStopOriginal()
        {
            await Task.Run(() =>
            {
                int writeResult = WriteBit("DB1.DBX0.1", true);
                if (writeResult != 0)
                {
                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                }
                Thread.Sleep(30);
                writeResult = WriteBit("DB1.DBX0.1", false);
                if (writeResult != 0)
                {
                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                }
            });
        }


        public async Task WriteStopExec(string db, bool value)
        {
            await Task.Run(() =>
            {
                int writeResult = WriteBitExec(db, value);
                //  int writeResult = WriteBitExec(db, true);
                if (writeResult != 0)
                {
                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                }
                Thread.Sleep(30);

                // bool Variable Invertieren
                value = !value;
                writeResult = WriteBitExec(db, value);
                //  writeResult = WriteBitExec(db, false);
                if (writeResult != 0)
                {
                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                }
            });
        }
        public async Task WriteStop()
        {
            await Task.Run(() =>
            {
                //declare a WriteRequest object and
                //set the request parameters
                WriteDataRequest myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                                                                       1,                 //DB / only for datablock operations otherwise 0
                                                                       0,
                                                                       1);                  //write start adress
                                                                                            //add writable Data here
                                                                                            //in  this case => write 4 bytes in DB100
                                                                                            //    myWriteRequest.addByte(new byte[] { 11 });
                myWriteRequest.addBit(true);

                //write
                Debug.WriteLine("begin write...");
                WriteDataResult res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: " + res.Message);
                    Debug.WriteLine("Write successfull! Message: " + res.Message);
                }
                else
                {
                    //MessageBox.Show("Write not successfull! Message: " + res.Message);
                    Debug.WriteLine("Write not successfull! Message: " + res.Message);
                }

                //declare a WriteRequest object and
                //set the request parameters
                myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                                                                       1,                 //DB / only for datablock operations otherwise 0
                                                                       0,
                                                                       1);                  //write start adress
                                                                                            //add writable Data here
                                                                                            //in  this case => write 4 bytes in DB100
                                                                                            //    myWriteRequest.addByte(new byte[] { 11 })
                Thread.Sleep(30);
                myWriteRequest.addBit(false);

                //write
                Debug.WriteLine("begin write...");
                res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: RST" + res.Message);
                    Debug.WriteLine("Write successfull! Message: RST" + res.Message);
                }
                else
                {
                    //MessageBox.Show("Write not successfull! Message: RST" + res.Message);
                    Debug.WriteLine("Write not successfull! Message: RST" + res.Message);
                }
            });
        }


        public async Task WriteOut_0_0()
        {
            await Task.Run(() =>
            {
                //int writeResult = WriteBit("DB1.DBX0.5", true);
                //if (writeResult != 0)
                //{
                //    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                //    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                //}
                //declare a WriteRequest object and
                //set the request parameters
                WriteDataRequest myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                                                                       1,                 //DB / only for datablock operations otherwise 0
                                                                       0,
                                                                       5);                  //write start adress
                                                                                            //add writable Data here
                                                                                            //in  this case => write 4 bytes in DB100
                                                                                            //    myWriteRequest.addByte(new byte[] { 11 });
                myWriteRequest.addBit(true);

                //write
                Debug.WriteLine("begin write...");
                WriteDataResult res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: ON " + res.Message);
                    Debug.WriteLine("Write successfull! Message: ON " + res.Message);
                }
                else
                {
                    //MessageBox.Show("Write not successfull! Message: " + res.Message);
                    Debug.WriteLine("Write not successfull! Message: " + res.Message);
                }


            });
        }
        public async Task WriteOut_0_0_false()
        {
            await Task.Run(() =>
            {
                //int writeResult = WriteBit("DB1.DBX0.5", false);
                //if (writeResult != 0)
                //{
                //    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                //    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: ");
                //}
                //declare a WriteRequest object and
                //set the request parameters
                WriteDataRequest myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                                                                       1,                 //DB / only for datablock operations otherwise 0
                                                                       0,
                                                                       5);                  //write start adress
                                                                                            //add writable Data here
                                                                                            //in  this case => write 4 bytes in DB100
                                                                                            //    myWriteRequest.addByte(new byte[] { 11 });
                myWriteRequest.addBit(false);

                //write
                Debug.WriteLine("begin write...");
                WriteDataResult res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: OFF " + res.Message);
                    Debug.WriteLine("Write successfull! Message: OFF " + res.Message);
                }
                else
                {
                    //MessageBox.Show("Write not successfull! Message: " + res.Message);
                    Debug.WriteLine("Write not successfull! Message: " + res.Message);
                }


            });
        }
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                ScanTime = DateTime.Now - _lastScanTime;
             RefreshValues();
                OnValuesRefreshed();
            }
            finally
            {
                _timer.Start();
            }
            _lastScanTime = DateTime.Now;
        }

        private void RefreshValues()
        {
            lock (_locker)
            {
                //declare a ReadRequest object
                //set the request parameters
                //read 10 Bytes from DB100
                //HOT 221108 ReadDataRequest myReadRequest = new ReadDataRequest(eRegion.DataBlock, 1, 0, eDataType.BYTE, 10);
                ReadDataRequest myReadRequest = new ReadDataRequest(eRegion.DataBlock, 1, 0, eDataType.BYTE, 4);

                //read from device
                ReadDataResult res = _client.ReadData(myReadRequest);

                //evaluate results
                //    txtMessage.Text = res.Message;
                Debug.WriteLine(res.Message);

                // byte buffer foreach Schleife
                byte[] buffer = new byte[10];
                //   var db1 = new Db1();


                //  int bitNumber;
                if (res.Quality == OperationResult.eQuality.GOOD)
                {
                    int Position = 0;
                    foreach (Object item in res.GetValues())
                    {
                        string? v = item.ToString();
                        buffer[Position] = Convert.ToByte(v);
                        Console.WriteLine("read Byte " + Position++.ToString() + " " + v);
                    }

                    // Hier kommen alles rein was in der HMI  angezeigt wird                  
                    PumpState = S7.GetBitAt(buffer, 0, 2);
                    HighLimit = S7.GetBitAt(buffer, 0, 3);
                    LowLimit = S7.GetBitAt(buffer, 0, 4);

                    TankLevel = S7.GetIntAt(buffer, 2);
                    //todo HOT das mit dem db1 ist nur ein Test
                    //wollte eigentliche ReadClass machen oder so...
                    //db1.TankLevel = S7.GetIntAt(buffer, 2);
                    //TankLevel = db1.TankLevel;

                    Write_Int = S7.GetIntAt(buffer, 8);

                    Debug.WriteLine("Tanklevel: " + TankLevel);
                }
                else
                {
                    Debug.WriteLine("read not successfull! Message: " + res.Message);
                }
                //MessageBox.Show("buffer0 " + buffer[0].ToString());
                //MessageBox.Show("buffer1 " + buffer[1].ToString());
                //MessageBox.Show("buffer2 " + buffer[3].ToString());
            }
        }

        private int WriteBitExec(string address, bool value)
        {
            var strings = address.Split('.');
            int db = Convert.ToInt32(strings[0].Replace("DB", ""));
            int pos = Convert.ToInt32(strings[1].Replace("DBX", ""));
            int bit = Convert.ToInt32(strings[2]);
            return WriteBitExec(db, pos, bit, value);
        }

        private int WriteBitExec(int db, int pos, int bit, bool value)
        {
            lock (_locker)
            {
                //var buffer = new byte[1];
                ////HOT
                ////  S7.SetBitAt(ref buffer, 0, bit, value);
                ////   S7.SetBitAt(buffer, 0, bit, value);
                ////return _client.WriteArea(S7Consts.S7AreaDB, db, pos + bit, buffer.Length, S7Consts.S7WLBit, buffer);
                //return 10;
                WriteDataRequest myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                                                                 db,                 //DB / only for datablock operations otherwise 0
                                                                 pos,
                                                                Convert.ToByte(bit));                  //write start adress
                                                                                                       //add writable Data here
                                                                                                       //in  this case => write 4 bytes in DB100
                                                                                                       //    myWriteRequest.addByte(new byte[] { 11 });
                myWriteRequest.addBit(value);

                //write
                Debug.WriteLine("begin write...");
                WriteDataResult res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: ON EXEC" + res.Message);
                    Debug.WriteLine("Write successfull! Message: ON EXEC" + res.Message);
                    //todo HOT da muss man noch den richtigen Rüchgabewert rein machen 
                    return 0;
                }
                else
                {
                    Debug.WriteLine("Write not successfull! Message: " + res.Message);
                    //MessageBox.Show("Write not successfull! Message: " + res.Message);
                    return 1;
                }

            }
        }

        private int WriteIntExec(int db, int pos, short value)
        {
            lock (_locker)
            {
                //var buffer = new byte[1];
                ////HOT
                ////  S7.SetBitAt(ref buffer, 0, bit, value);
                ////   S7.SetBitAt(buffer, 0, bit, value);
                ////return _client.WriteArea(S7Consts.S7AreaDB, db, pos + bit, buffer.Length, S7Consts.S7WLBit, buffer);
                //return 10;
                ////WriteDataRequest myWriteRequest = new WriteDataRequest(eRegion.DataBlock,   //Region
                ////                                                 db,                 //DB / only for datablock operations otherwise 0
                ////                                                 pos,
                ////                                                Convert.ToByte(intEin));                  //write start adress
                ////                                                                                       //add writable Data here
                ////                                                                                       //in  this case => write 4 bytes in DB100
                ////                                                                                       //    myWriteRequest.addByte(new byte[] { 11 });
                //////myWriteRequest.addBit(value);
                ////myWriteRequest.addInt((short)value);

                //public static void SetDIntAt(this byte[] buffer, int pos, int value)
                byte[] bufferI = { 0, 0 };
                //  byte[] bufferInt = { 0, 0 };

                //S7.SetIntAt(bufferI, 0, 1024);
                S7.SetIntAt(bufferI, 0, value);

                //TankLevel = S7.GetIntAt(bufferI, 2);

                //S7.SetIntAt(2, 2);
                WriteDataRequest myWriteRequest = new WriteDataRequest(eRegion.DataBlock, db, pos);
                // myWriteRequest.addByte(new byte[] { 1, 0 });
                myWriteRequest.addByte(bufferI);

                //write
                Debug.WriteLine("begin write...");
                WriteDataResult res = _client.WriteData(myWriteRequest);

                //evaluate results
                if (res.Quality.Equals(OperationResult.eQuality.GOOD))
                {
                    //MessageBox.Show("Write successfull! Message: ON EXEC" + res.Message);
                    Debug.WriteLine("Write successfull! Message: ON EXEC" + res.Message);
                    //todo HOT da muss man noch den richtigen Rüchgabewert rein machen 
                    return 0;
                }
                else
                {
                    //MessageBox.Show("Write not successfull! Message: " + res.Message);
                    Debug.WriteLine("Write not successfull! Message: " + res.Message);
                    return 1;
                }

            }
        }
        private int WriteBit(string address, bool value)
        {
            var strings = address.Split('.');
            int db = Convert.ToInt32(strings[0].Replace("DB", ""));
            int pos = Convert.ToInt32(strings[1].Replace("DBX", ""));
            int bit = Convert.ToInt32(strings[2]);
            return WriteBit(db, pos, bit, value);
        }

        private int WriteBit(int db, int pos, int bit, bool value)
        {
            lock (_locker)
            {
                var buffer = new byte[1];
                //HOT
                //  S7.SetBitAt(ref buffer, 0, bit, value);
                //   S7.SetBitAt(buffer, 0, bit, value);
                //return _client.WriteArea(S7Consts.S7AreaDB, db, pos + bit, buffer.Length, S7Consts.S7WLBit, buffer);
                return 10;
            }
        }

        private void OnValuesRefreshed()
        {
            ValuesRefreshed?.Invoke(this, new EventArgs());
        }
    }
}

