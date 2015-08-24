using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.IO;
using System.Net;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start.");
            Temp();
            Console.WriteLine("End.");
            Console.ReadKey();
        }

        private static void Temp()
        {
            //ExpressionTree.Start();
            //DynamicQueryFeatures.MiscDemo();
            Demo();
        }

        #region Demo
        private static void Demo()
        {
            HttpClient client = new HttpClient();
            string url = "http://localhost:9636/Hello.xml";  // IIS

            string data = @"<?xml version=""1.0"" encoding=""utf-8""?><HelloRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://soa.ctrip.com/thingstodo/order/settlementopenapi/v1""><OrderId>123456</OrderId><UnitQuantity>4</UnitQuantity><EndDate>2015-08-04T17:56:43.5959493+08:00</EndDate><Price>99.99</Price></HelloRequest>";
            byte[] buf = Encoding.UTF8.GetBytes(data);
            StringContent conent = new StringContent(data);
            Task<HttpResponseMessage> message = client.PostAsync(url, conent);
            message.Wait();
            HttpResponseMessage result = message.Result;
            Task<String> btking = result.Content.ReadAsStringAsync();
            btking.Wait();
            Console.WriteLine(btking.Result);
        }
        #endregion
    }

}