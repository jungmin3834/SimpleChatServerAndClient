using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ServiceModel;

namespace WCF체팅_1대다_서버
{
    class Program
    {
        static void Main(string[] args)
        {
            //Address
            Uri uri = new Uri(ConfigurationManager.AppSettings["addr"]);
            //Contract-> Setting
            //Binding -> App.Config
            ServiceHost host = new ServiceHost(typeof(WCF체팅_1대다_서버.ChatService), uri);

            //오픈
            host.Open();
            Console.WriteLine("채팅 서비스를 시작합니다. {0}", uri.ToString());
            Console.WriteLine("http://61.81.99.75:9000/GetService");
            Console.WriteLine("멈추시려면 엔터를 눌러주세요..");
            Console.ReadLine();
            //서비스
            host.Abort();
            host.Close();

        }

    }
}
