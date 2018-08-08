using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;

namespace Hotspot_Sİstemi_V0._1
{
	public partial class anaGiris : Form
	{
        int sayac = 0; //timer için
        yonetici yonetici = new yonetici();
        public anaGiris()
		{
			InitializeComponent();
		}
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            textBox1.Text = Properties.Settings.Default["kuladi"].ToString();
            textBox2.Text = Properties.Settings.Default["sifre"].ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //YÖNETİCİ GİRİŞİ
            SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
            SqlCeCommand komut = new SqlCeCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from YoneticiTBL where kullaniciAdi='"+textBox1.Text+"' and sifre='"+textBox2.Text+"'";
            komut.ExecuteNonQuery();
            //SqlDataReader dr = komut.ExecuteReader();
            SqlCeDataReader dr=komut.ExecuteReader();
            
            if (dr.Read())
            {
                timer1.Start(); //timerı başlat
                //serverBilgi sBilgi = new serverBilgi();
                GenelSayfa gs = new GenelSayfa();
                this.Hide();
                gs.Show();
                if (checkBox1.Checked)
                {
                    Properties.Settings.Default["kuladi"] = textBox1.Text;
                    Properties.Settings.Default["sifre"] = textBox2.Text;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default["kuladi"] = "";
                    Properties.Settings.Default["sifre"] = "";
                    Properties.Settings.Default.Save();
                }
                Properties.Settings.Default["yoneticiAdi"] = textBox1.Text;
            }
            else
            {
                label4.Text = "Kullanıcı adı veya Şifre yanlış! ";
                textBox1.Text = "";
                textBox2.Text = "";
            }
            baglanti.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;

            if (sayac % 127 == 0)
            {
                KullaniciSil kSil = new KullaniciSil();
                kSil.kullanici_Sil();
            }
        }

        private void anaGiris_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start("http://hmsotel.com");
        }
    }
}
