using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotspot_Sİstemi_V0._1
{
    class serverAyar
    {
        string svIp;
        string svKulAdi;
        string svSifre;
        string silKulAdi;
        
        SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");

        public void serverGuncelle(TextBox ipAdres, TextBox kullaniciAdi, TextBox sifre,TextBox serverAdi)
        {
            if (sifre.Text == "" || kullaniciAdi.Text == "" || ipAdres.Text == "")
            {
                MessageBox.Show("Boş alan bırakmayınız");
            }
            else
            {
                try
                {
                    SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                    SqlCeCommand cmd = new SqlCeCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    cmd.Connection = baglanti;
                    cmd.CommandText = "update ServerTBL set sifre=@sifre, kullaniciAdi=@kullaniciAdi, ipAdres=@ipAdres where serverAdi='" + serverAdi.Text + "' ";
                    cmd.Parameters.AddWithValue("@sifre", sifre.Text);
                    cmd.Parameters.AddWithValue("@ipAdres", ipAdres.Text);
                    cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi.Text);
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
        public void serverBul(TextBox serverAdi, TextBox ipAdres, TextBox kullaniciAdi, TextBox sifre, Button guncelle, Button sil)
        {

            SqlCeCommand komut = new SqlCeCommand();
            if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
            komut.Connection = baglanti;
            komut.CommandText = "select * from ServerTBL where serverAdi='" + serverAdi.Text + "'";
            komut.ExecuteNonQuery();
            SqlCeDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                sifre.Text = dr["sifre"].ToString();
                ipAdres.Text = dr["ipAdres"].ToString();
                kullaniciAdi.Text = dr["kullaniciAdi"].ToString();
                serverAdi.ReadOnly = true;
                guncelle.Enabled = true;
                sil.Enabled = true;

            }
            else
            {
                MessageBox.Show("Bu Server ile ilgili bilgi bulunmamaktadır.");
            }
            dr.Close();
            baglanti.Close();
        }
        public void serverSil(TextBox serverAdiSil)
        {
            try
            {
                SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                SqlCeCommand komut = new SqlCeCommand();
                if (baglanti.State==ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                komut.Connection = baglanti;
                komut.CommandText = "delete from ServerTBL where serverAdi='" + serverAdiSil.Text + "'";
                komut.ExecuteNonQuery();
                MessageBox.Show("Server Silindi");
            }
            catch (Exception hata)
            {
                MessageBox.Show("İşlem hatası" + hata.Message);
            }
        }
        public void serverListele(ListBox listbox)
        {
            SqlCeCommand cmd = new SqlCeCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            cmd.Connection = baglanti;
            cmd.CommandText = "select * from ServerTBL ORDER BY serverId";
            cmd.ExecuteNonQuery();
            SqlCeDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listbox.Items.Add(dr["serverAdi"].ToString());
            }
            dr.Close();
            baglanti.Close();
        }
        public void kullaniciServerListe(ListBox listbox2,ListBox listbox1)
        {
            SqlCeCommand komut = new SqlCeCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from HotspotTBL H , ServerTBL S where S.serverId=H.serverId and S.serverAdi='" + listbox2.SelectedItem.ToString() + "'";
            komut.ExecuteNonQuery();
            SqlCeDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                listbox1.Items.Add(dr["kullaniciAdi"].ToString());
            }
            dr.Close();
            baglanti.Close();
        }
        public void routerKullaniciKontrol(TextBox serverAdi)
        {
            //mikrotik için server bilgileri çektik 
            try
            {
                SqlCeCommand komut = new SqlCeCommand();
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                komut.Connection = baglanti;
                komut.CommandText = "select * from ServerTBL where serverAdi='" + serverAdi.Text + "'";
                komut.ExecuteNonQuery();
                SqlCeDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    svIp = dr["ipAdres"].ToString();
                    svKulAdi = dr["kullaniciAdi"].ToString();
                    svSifre = dr["sifre"].ToString();
                }
                dr.Close();
                baglanti.Close();
                routerSil(serverAdi);
            }
            catch (Exception hata)
            {
                MessageBox.Show("Bir hata ile karşılaştınız : " + hata.Message);
            }
        }
        public void routerSil(TextBox routerSilServerAdi)
        {
            //Sqlden Server silindiğinde mikrotik cihazdaki tüm userlar da silinecek.

            try
            {
                SqlCeCommand komut = new SqlCeCommand();
                MK mikrotik = new MK(svIp);

                if (!mikrotik.Login(svKulAdi, svSifre))
                {
                    mikrotik.Close();
                }
                else
                {
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    komut.Connection = baglanti;
                    komut.CommandText = "select H.kullaniciAdi from HotspotTBL H, ServerTBL S where S.serverId=H.serverId and S.serverAdi='" + routerSilServerAdi.Text + "'";
                    komut.ExecuteNonQuery();
                    SqlCeDataReader dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        silKulAdi = dr["kullaniciAdi"].ToString();
                        mikrotik.Send("/ip/hotspot/user/remove");
                        mikrotik.Send("=.id=" + silKulAdi + "", true);
                    }
                    dr.Close();
                    baglanti.Close();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Server Silindi");
            }
            
        }
    }
}
