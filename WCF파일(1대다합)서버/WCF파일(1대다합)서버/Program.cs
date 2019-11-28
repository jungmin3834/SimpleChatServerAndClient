using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCF파일_1대다합_서버
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri(ConfigurationManager.AppSettings["addr"]);
            ServiceHost host = new ServiceHost(typeof(WCF파일_1대다합_서버.CFile), uri);

            host.Open();
            Console.WriteLine("파일 서비스를 시작합니다. {0}", uri.ToString());
            Console.WriteLine("http://127.0.0.1:7000/GetFileService");
            Console.WriteLine("멈추려면 엔터");
            Console.ReadLine();

            host.Abort();
            host.Close();
        }
    }
}
