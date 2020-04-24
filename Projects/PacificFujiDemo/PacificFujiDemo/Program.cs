using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacificFujiDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            PacificFujiRoutine _routine = new PacificFujiRoutine();
            try
            {
                _routine.ProcessFiles();
            }
            finally { Environment.Exit(0); }
        }
    }
}
