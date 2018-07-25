using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotspot_Sİstemi_V0._1
{
    class yonetici
    {

        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");

        public void yoneticiEkle(string kullAdi, string sifre, string email, string seviye)
        {
            //SERVER EKLEME
            if (kullAdi == "" || sifre == "" || email == "" || seviye == "")
            {
                MessageBox.Show("Boş alan Bırakmayınız");
            }
            else
            {
                try
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = baglanti;
                    cmd.CommandText = "Select * from YoneticiTBL where kullaniciAdi='" + kullAdi + "' or email='" + email + "'";
                    cmd.ExecuteNonQuery();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Bu kullanıcı adı bulunmaktadır.Farklı bir ad giriniz");
                    }
                    else
                    {
                        dr.Close();
                        string kayit = "insert into YoneticiTBL(kullaniciAdi,sifre,email,seviye) values (@kullaniciAdi,@sifre,@email,@seviye)";
                        SqlCommand komut = new SqlCommand(kayit, baglanti);
                        komut.Parameters.AddWithValue("@kullaniciAdi", kullAdi);
                        komut.Parameters.AddWithValue("@sifre", sifre);
                        komut.Parameters.AddWithValue("@email", email);
                        komut.Parameters.AddWithValue("@seviye", seviye);
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("Yönetici Kayıt İşlemi Gerçekleşti.");
                    }
                    baglanti.Close();
                }
                catch (Exception hata)
                {
                    MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                }
            }
        }

        public void yoneticiBul(TextBox kullAdi,TextBox sifre,TextBox email,TextBox seviye,Button guncelle,Button sil)
        {
            SqlCommand komut = new SqlCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from YoneticiTBL where kullaniciAdi='"+kullAdi.Text+"'";
            komut.ExecuteNonQuery();
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                sifre.Text = dr["sifre"].ToString();
                email.Text = dr["email"].ToString();
                seviye.Text = dr["seviye"].ToString();

                kullAdi.ReadOnly = true;
                guncelle.Enabled = true;
                sil.Enabled = true;
            }
            else
            {
                MessageBox.Show("Bu Kullanıcıya ait Bilgi bulunmamaktadır. ");
            }
            baglanti.Close();
        }
        public void yoneticiSil(string kullAdiSil)
        {
            if (kullAdiSil == "hms")
            {
                MessageBox.Show("Bu yönetici Silinemez","Uyarı");
            }
            else
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
                    SqlCommand komut = new SqlCommand();
                    if (baglanti.State==ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    komut.Connection = baglanti;
                    komut.CommandText = "delete from YoneticiTBL where kullaniciAdi='" + kullAdiSil + "' DBCC CHECKIDENT('YoneticiTBL', RESEED, 0)";
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Kullanıcı Silindi");
                }
                catch (Exception hata)
                {
                    MessageBox.Show("İşlem hatası" + hata.Message);
                }
            }
        }
        public void yoneticiGuncelle(TextBox kullAdi,TextBox sifre,TextBox email,TextBox seviye)
        {
            if (sifre.Text == "" || email.Text == "" || seviye.Text == "")
            {
                MessageBox.Show("Boş alan bırakmayınız");
            }
            else
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
                    SqlCommand cmd = new SqlCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    cmd.Connection = baglanti;
                    cmd.CommandText = "update YoneticiTBL set sifre=@sifre, email=@email, seviye=@seviye where kullaniciAdi='" + kullAdi.Text + "' ";
                    cmd.Parameters.AddWithValue("@sifre", sifre.Text);
                    cmd.Parameters.AddWithValue("@email", email.Text);
                    cmd.Parameters.AddWithValue("@seviye", seviye.Text);
                    cmd.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Kullanıcı Bilgileri Güncellendi");
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Güncelleme işlemi başarısız " + hata.Message);
                }
            }
        }
        public void yoneticiListele(ListBox listbox)
        {
            SqlCommand komut = new SqlCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from YoneticiTBL";
            komut.ExecuteNonQuery();
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                listbox.Items.Add(dr["kullaniciAdi"].ToString());
            }
            baglanti.Close();
        }
    }
}
