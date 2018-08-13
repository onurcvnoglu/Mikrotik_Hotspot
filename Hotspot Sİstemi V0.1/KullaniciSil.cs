using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tik4net;
using tik4net.Objects;
using tik4net.Objects.Ip.Hotspot;

namespace Hotspot_Sİstemi_V0._1
{
    class KullaniciSil
    {
        string svKulAdi;
        string svSifre;
        string svIp;
        string svId;
        string kullaniciAdi;

        public void kullanici_Sil()
        {
            //yönetici her programı çalıştırdığında süresi dolan kullanıcıları silecek.. 
            string date = (string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now));
            SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
            SqlCeCommand komut = new SqlCeCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from HotspotTBL where sure < '" + date + "'";
            komut.ExecuteNonQuery();
            SqlCeDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                svId = dr["serverId"].ToString();
                kullaniciAdi = dr["kullaniciAdi"].ToString();
                //
                ArsivEkle aEkle = new ArsivEkle();
                aEkle.Listele(kullaniciAdi, svId);
                //
                serverVeri();
                MK mikrotik = new MK(svIp);
                if (!mikrotik.Login(svKulAdi, svSifre))
                {
                    MessageBox.Show("Bağlantı işlemi başarısız");
                    mikrotik.Close();
                    return;
                }
                else
                {
                    mikrotik.Send("/ip/hotspot/user/remove");
                    mikrotik.Send("=.id=" + kullaniciAdi + "", true);
                }
            }
            dr.Close();
            
            komut.CommandText = "delete from HotspotTBL where sure < '" + date + "' ";
            komut.ExecuteNonQuery();
            ////

            baglanti.Close();
            ///mikrotik
        }

        public void serverVeri()  //Server Bilgilerini Çektik.
        {
            SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
            SqlCeCommand komut = new SqlCeCommand();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select * from ServerTBL where serverId='" + svId + "'";
            komut.ExecuteNonQuery();
            SqlCeDataReader dr = komut.ExecuteReader();

            if (dr.Read())
            {
                svKulAdi = dr["kullaniciAdi"].ToString();
                svSifre = dr["sifre"].ToString();
                svIp = dr["ipAdres"].ToString();
            }

            dr.Close();
            baglanti.Close();
        }

        public void kullaniciSifirla(DataGridView dg,ListBox listbox)
        {
            try
            {
                SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                SqlCeCommand komut = new SqlCeCommand();
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                komut.Connection = baglanti;
                komut.CommandText = "select * from HotspotTBL H,ServerTBL S where S.serverAdi='" + listbox.SelectedItem + "'";
                komut.ExecuteNonQuery();
                SqlCeDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    svId = dr["serverId"].ToString();
                    kullaniciAdi = dr["kullaniciAdi"].ToString();
                    //
                    ArsivEkle aEkle = new ArsivEkle();
                    aEkle.Listele(kullaniciAdi, svId);
                    //
                    serverVeri();
                    MK mikrotik = new MK(svIp);
                    if (!mikrotik.Login(svKulAdi, svSifre))
                    {
                        MessageBox.Show("Bağlantı işlemi başarısız");
                        mikrotik.Close();
                        return;
                    }
                    else
                    {
                        mikrotik.Send("/ip/hotspot/user/remove");
                        mikrotik.Send("=.id=" + kullaniciAdi + "", true);
                    }
                }
                dr.Close();
                
                komut.CommandText = "delete from HotspotTBL where serverId='" + svId + "'";
                komut.ExecuteNonQuery();
                ////
                baglanti.Close();
            }
            catch (Exception)
            {

            }
        }
    }
}
