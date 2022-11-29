using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCcom.Service
{
    public class Daten
    {

     private   string ipaddress = "192.168.2.85";

        public string Ipaddress 
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }

    }
}
