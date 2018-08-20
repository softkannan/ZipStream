using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ZipStream
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ZipManager mgr = new ZipManager();
            mgr.Execute();
        }
    }
}
