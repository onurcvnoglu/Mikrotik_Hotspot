using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotspot_Sİstemi_V0._1
{
    class MikroCek
    {
        ArrayList kullaniciVerileri = new ArrayList();
        ArrayList parcalar = new ArrayList();
        public void VeriAl(ListBox listbox)
        {
            MK mikrotik = new MK("192.168.0.1");
            if (mikrotik.Login("admin", "nurettin1"))
            {
                mikrotik.Send("/ip/hotspot/user/print");
                mikrotik.Send("=detail", true);
            }
            foreach (string h in mikrotik.Read())
            {
                kullaniciVerileri.Add(h);
            }

        }
    }
}
