using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace StockCalc
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int LocX = 0;
            int LocY = 0;
            int Width = 433;
            int Height = 266;
 
            try
            {
                LocX = Convert.ToInt16(Network.ReadIniFile("LocStockCalc", "X", @".\setting.ini"));
                LocY = Convert.ToInt16(Network.ReadIniFile("LocStockCalc", "Y", @".\setting.ini"));
                Width = Convert.ToInt16(Network.ReadIniFile("SizeMain", "Width", @".\setting.ini"));
                Height = Convert.ToInt16(Network.ReadIniFile("SizeMain", "Height", @".\setting.ini"));
            }
            catch
            {}

            if (LocX > SystemInformation.VirtualScreen.Width ||
                 LocY > SystemInformation.VirtualScreen.Height ||
                 LocX < 0 || LocY < 0 )
            {
                LocX = 0;
                LocY = 0;
            }

            if (Width > SystemInformation.VirtualScreen.Width ||
                Height > SystemInformation.VirtualScreen.Height ||
                Width < 0 || Height < 0)
            {
                Width = 433;
                Height = 266;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MdiParents Mdi = new MdiParents();
            Mdi.StartPosition = FormStartPosition.Manual;
            Mdi.Location = new Point(LocX, LocY);
            Mdi.Size = new System.Drawing.Size(Width, Height);
            Application.Run(Mdi);
        }
    }
}