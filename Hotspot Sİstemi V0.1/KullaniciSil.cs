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
            string date = (string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));
            SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            SqlCommand komut = new SqlCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from HotspotTBL where sure < '" + date + "'";
            komut.ExecuteNonQuery();
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                svId = dr["serverId"].ToString();
                kullaniciAdi = dr["kullaniciAdi"].ToString();
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

        public void serverVeri()  //Server Bİlgilerini Çektik.
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            SqlCommand komut = new SqlCommand();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select * from ServerTBL where serverId='" + svId + "'";
            komut.ExecuteNonQuery();
            SqlDataReader dr = komut.ExecuteReader();

            if (dr.Read())
            {
                svKulAdi = dr["kullaniciAdi"].ToString();
                svSifre = dr["sifre"].ToString();
                svIp = dr["ipAdres"].ToString();
            }

            dr.Close();
            baglanti.Close();
        }

        public void MikrotikKullaniciSil()
        {
            
        }
    }
}
