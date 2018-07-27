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
    class Kullanici
    {
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public void kullaniciListele(ListBox listbox)
        {
            SqlCommand komut = new SqlCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from HotspotTBL";
            komut.ExecuteNonQuery();
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                listbox.Items.Add(dr["kullaniciAdi"].ToString());
            }
            baglanti.Close();
        }
        public void kullaniciBul(TextBox kAdiTxt,TextBox svAdiTxt,Button button6,Button silBtn,string svIdGuncel,TextBox kulSifreTxt,TextBox emailTxt,DateTimePicker dateTimePicker1)
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            SqlCommand komut = new SqlCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from HotspotTBL H , ServerTBL S where S.serverId=H.serverId and H.kullaniciAdi='" + kAdiTxt.Text + "' and S.serverAdi='" + svAdiTxt.Text + "'";
            komut.ExecuteNonQuery();
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                svIdGuncel = dr["serverId"].ToString();
                kulSifreTxt.Text = dr["sifre"].ToString();
                emailTxt.Text = dr["email"].ToString();
                dateTimePicker1.Text = dr["sure"].ToString();

                kAdiTxt.ReadOnly = true;
                svAdiTxt.ReadOnly = true;
                button6.Enabled = true;
                silBtn.Enabled = true;
            }
            else
            {
                MessageBox.Show("Bu Server ve Kullanıcıya ait Bilgi bulunmamaktadır. ");
            }
            baglanti.Close();
        }

    }
}
