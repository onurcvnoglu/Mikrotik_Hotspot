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
    public partial class yoneticiKayit : Form
    {
        yonetici yoneticiDuzenle = new yonetici();
        public yoneticiKayit()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            yonetici yoneticiK = new yonetici();
            yoneticiK.yoneticiEkle(kulAdiTxt.Text, sifreTxt.Text, emailTxt.Text, seviyeTxt.Text);

            kulAdiTxt.Text = "";
            sifreTxt.Text = "";
            emailTxt.Text = "";
            seviyeTxt.Text = "";
            yoneticiKayit_Load(sender, e);
        }

        private void seviyeTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox2.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            groupBox1.Visible = false;
            kAdiTxt.Text = "";
            sifreDuzTxt.Text = "";
            emailDuzTxt.Text = "";
            seviyeDuzTxt.Text = "";
            silBtn.Enabled = false;
            guncelBtn.Enabled = false;
            kAdiTxt.ReadOnly = false;
            listBox1_SelectedIndexChanged(sender, e);
        }

        private void guncelBtn_Click(object sender, EventArgs e)
        {
            yonetici yoneticiG = new yonetici();
            yoneticiG.yoneticiGuncelle(kAdiTxt, sifreDuzTxt, emailDuzTxt, seviyeDuzTxt);
            yoneticiKayit_Load(sender, e);
        }

        private void silBtn_Click(object sender, EventArgs e)
        {
            yonetici yoneticiS = new yonetici();
            yoneticiS.yoneticiSil(kAdiTxt.Text);
            //groupBox2.Visible = false;
            yoneticiKayit_Load(sender, e);
        }

        private void seviyeTxt_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void seviyeDuzTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //şifreyi göster
                checkBox1.Text = "Şifreyi Gizle";
                sifreTxt.PasswordChar = '\0';
            }
            else
            {
                //şifreyi gizle
                checkBox1.Text = "Şifreyi Göster";
                sifreTxt.PasswordChar = '*';
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
            yoneticiKayit_Load(sender, e);
        }

        private void yoneticiKayit_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            yoneticiDuzenle.yoneticiListele(listBox1);
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                kAdiTxt.Text = listBox1.SelectedItem.ToString();
                yoneticiDuzenle.yoneticiBul(kAdiTxt, sifreDuzTxt, emailDuzTxt, seviyeDuzTxt, guncelBtn, silBtn);
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

        private void serverDuzenleMenuStrip_Click(object sender, EventArgs e)
        {
            serverDuzen sDuzen = new serverDuzen();
            sDuzen.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        private void yoneticiDuzenleMenuStrip_Click(object sender, EventArgs e)
        {
            //yoneticiKayit ykayit = new yoneticiKayit();
            //ykayit.Show();
            //this.Hide();
        }

        private void kullaniciAyarMenuStrip_Click(object sender, EventArgs e)
        {
            yoneticiAyar yayar = new yoneticiAyar();
            yayar.Show();
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
