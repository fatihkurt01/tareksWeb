using System;
using System.Windows.Forms;

namespace TareksWeb
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
            //Application.Run(new TareksWebForm.TareksWeb());
        }
    }
}
