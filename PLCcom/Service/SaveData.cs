using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PLCcom.Service
{
   public        class SaveData
    {
        public static async Task SaveDatenAsync(object obj, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            // TextWriter filestream = new StreamWriter(@"c:\temp\userdata.xml");
            TextWriter filestream = new StreamWriter(filename);
            serializer.Serialize(filestream, obj);
            filestream.Close();

            await Shell.Current.DisplayAlert("Save successful",
           "you have saved parameter" +
           "", "OK");
        }
    }
}
