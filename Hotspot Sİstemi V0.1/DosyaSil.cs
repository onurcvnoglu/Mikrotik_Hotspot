using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotspot_Sİstemi_V0._1
{
    class DosyaSil
    {
        public void SetupSil()
        {
            try
            {
                if (File.Exists(Application.StartupPath+ "\\Hotspot Setup.zip"))
                {
                    File.Delete(Application.StartupPath + "\\Hotspot Setup.zip");
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Güncelleme İşlemi Devam Ediyor : "+hata);
            }
        }
    }
}
