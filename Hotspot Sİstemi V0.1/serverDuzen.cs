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
    public partial class serverDuzen : Form
    {
        serverAyar sAyar = new serverAyar();

        public serverDuzen()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sAyar.serverGuncelle(ipAdresGuncelTxt, kulAdiGuncelTxt, sifreGuncelTxt, serverAdiGuncelTxt);
            serverDuzen_Load(sender, e);
        }

        private void silBtn_Click(object sender, EventArgs e)
        {
            sAyar.routerKullaniciKontrol(serverAdiGuncelTxt);
            sAyar.serverSil(serverAdiGuncelTxt);

            serverAdiGuncelTxt.Text = "";
            serverAdiGuncelTxt.ReadOnly = false;
            ipAdresGuncelTxt.Text = "";
            kulAdiGuncelTxt.Text = "";
            sifreGuncelTxt.Text = "";
            guncelleBtn.Enabled = false;
            serverDuzen_Load(sender, e);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            serverAdiGuncelTxt.Text = "";
            serverAdiGuncelTxt.ReadOnly = false;
            ipAdresGuncelTxt.Text = "";
            kulAdiGuncelTxt.Text = "";
            sifreGuncelTxt.Text = "";
            guncelleBtn.Enabled = false;
            silBtn.Enabled = false;
            serverDuzen_Load(sender, e);
        }

        private void serverDuzen_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            sAyar.serverListele(listBox1);

            if (listBox1.Items.Count>0)
            {
                listBox1.SelectedIndex = 0;
            }
        }
        
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                serverAdiGuncelTxt.Text = listBox1.SelectedItem.ToString();
                sAyar.serverBul(serverAdiGuncelTxt, ipAdresGuncelTxt, kulAdiGuncelTxt, sifreGuncelTxt, guncelleBtn, silBtn);
            }
        }

        private void ServerEkleMenuStrip_Click(object sender, EventArgs e)
        {
            serverEkle sEkle = new serverEkle();
            sEkle.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        private void yoneticiDuzenleMenuStrip_Click(object sender, EventArgs e)
        {
            yoneticiKayit yKayit = new yoneticiKayit();
            yKayit.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        private void kullaniciAyarMenuStrip_Click(object sender, EventArgs e)
        {
            yoneticiAyar yAyar = new yoneticiAyar();
            yAyar.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        private void cikisMenuStrip_Click(object sender, EventArgs e)
        {
            anaGiris aGiris = new anaGiris();
            aGiris.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        private void anaSayfaMenuStrip_Click(object sender, EventArgs e)
        {
            serverBilgi sbilgi = new serverBilgi();
            sbilgi.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
