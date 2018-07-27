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
            //MikroCek mc=new MikroCek();
            //mc.VeriAl(label1);

            timer1.Stop();
            textBox1.Text = Properties.Settings.Default["kuladi"].ToString();
            textBox2.Text = Properties.Settings.Default["sifre"].ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //YÖNETİCİ GİRİŞİ
            SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            //SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
            SqlCommand komut = new SqlCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from YoneticiTBL where kullaniciAdi='"+textBox1.Text+"' and sifre='"+textBox2.Text+"'";
            komut.ExecuteNonQuery();
            SqlDataReader dr = komut.ExecuteReader();
            
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
                }
                else
                {
                    Properties.Settings.Default["kuladi"] = "";
                    Properties.Settings.Default["sifre"] = "";
                }
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

            if (sayac % 20 == 0)
            {
                KullaniciSil kSil = new KullaniciSil();
                kSil.kullanici_Sil();
            }
        }
    }
}
