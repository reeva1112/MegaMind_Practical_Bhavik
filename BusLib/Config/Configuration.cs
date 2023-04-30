using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Configuration;

namespace BusLib.Config
{
    public class Configuration
    {
       

        public static string InterNetServerConnStr
        {
            get { return ConfigurationSettings.AppSettings["ServerConnectionString"].ToString(); }
        }

        public static string ServerConnStr
        {
            get { return ConfigurationSettings.AppSettings["ServerConnectionString"].ToString(); }
        }

        public static string DesktopServerConnStr
        {
            get { return ConfigurationSettings.AppSettings["DesktopConnectionString"].ToString(); }
        }

        public static string DesktopServerConnStrGet(string IP, string Pass)
        {
            return ConfigurationSettings.AppSettings["DesktopConnectionString"].ToString() + "Data Source = " + IP + ";Password = " + Pass;
        }

    

       
    }
}
