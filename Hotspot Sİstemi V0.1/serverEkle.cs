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
    public partial class serverEkle : Form
    {
        public serverEkle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SERVER EKLEME
            if (textBox1.Text=="" || textBox3.Text=="" || textBox4.Text=="" || textBox2.Text=="")
            {
                MessageBox.Show("Boş alan Bırakmayınız");
            }
            else
            {
                SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
                try
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = baglanti;
                    cmd.CommandText = "Select * from ServerTBL where serverAdi='" + textBox1.Text + "' or ipAdres='" + textBox3.Text + "'";
                    cmd.ExecuteNonQuery();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Bu alana ait Server bulunmaktadır.Farklı bir server giriniz");
                    }
                    else
                    {
                        dr.Close();
                        string kayit = "insert into ServerTBL(serverAdi,ipAdres,kullaniciAdi,sifre) values (@serverAdi,@ipAdres,@kullaniciAdi,@sifre)";
                        SqlCommand komut = new SqlCommand(kayit, baglanti);
                        komut.Parameters.AddWithValue("@serverAdi", textBox1.Text);
                        komut.Parameters.AddWithValue("@ipAdres", textBox3.Text);
                        komut.Parameters.AddWithValue("@kullaniciAdi", textBox2.Text);
                        komut.Parameters.AddWithValue("@sifre", textBox4.Text);
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("Server Kayıt İşlemi Gerçekleşti.");
                    }
                    baglanti.Close();
                }
                catch (Exception hata)
                {
                    MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                }
            }
            

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
