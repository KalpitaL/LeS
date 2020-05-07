using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS_SKShipping_Watin_WebScrapper
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {            
            SKShippingScrapper _skShipping = new SKShippingScrapper();
            _skShipping.StartProcess();           
        }
    }
}
