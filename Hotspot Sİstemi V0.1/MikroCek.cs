using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tik4net;

namespace Hotspot_Sİstemi_V0._1
{
    class MikroCek
    {
        string[] parcalar;
        public void VeriAl(ListBox listbox,Label label)
        {
            //MK mikrotik = new MK("192.168.0.1");
            //if (mikrotik.Login("admin", "nurettin1"))
            //{
            //    mikrotik.Send("/ip/hotspot/user/print");
            //    mikrotik.Send("=detail", true);
            //}
            //foreach (string h in mikrotik.Read())
            //{
            //    listbox.Items.Clear();
            //    parcalar = h.Split('=');
            //    //foreach (var item in parcalar)
            //    //{
            //    //    listbox.Items.Add(item);
            //    //}
            //    for (int i = 0; i < parcalar.Length; i++)
            //    {
            //        listbox.Items.Add(i);
            //        if (i==4)
            //        {
            //            label.Text = listbox.Items[i].ToString();
            //        }
            //    }
            //}
        }
    }
}
