using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotspot_Sİstemi_V0._1
{
    public partial class yukleniyorEkranı : Form
    {
        public yukleniyorEkranı()
        {
            InitializeComponent();
        }

        private void yukleniyorEkranı_Load(object sender, EventArgs e)
        {
            label1.Text = "Kullanıcılar Yükleniyor. Bekleyiniz";
        }
    }
}
