using System;
using System.Windows.Forms;

namespace ConsoleApp1
{
    class ProgramGUI
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm());
        }
    }
}
