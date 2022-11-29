using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCcom.Service
{
    public class WriteToPlc
    {
        public static async Task Hallo()
        {
            await Task.Run(() =>
            {
                Debug.WriteLine("Hallop");
            });
        }

        public static void WriteStart(PLCcomDevice _client)
        {
            //  throw new NotImplementedException();
            //  Debug.WriteLine("Hallop");
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
        }

        public static void WriteStop(PLCcomDevice _client)
        {
            //await Shell.Current.DisplayAlert("Test",
            //        "tray again" +
            //        "", "OK");
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
        }
    }
}
