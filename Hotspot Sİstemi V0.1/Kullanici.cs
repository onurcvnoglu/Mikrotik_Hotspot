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
    class Kullanici
    {
        SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");

        public void kullaniciListele(ListBox listbox)
        {
            SqlCeCommand komut = new SqlCeCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from HotspotTBL";
            komut.ExecuteNonQuery();
            SqlCeDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                listbox.Items.Add(dr["kullaniciAdi"].ToString());
            }
            baglanti.Close();
        }
        public void kullaniciBul(TextBox kAdiTxt,TextBox svAdiTxt,Button button6,Button silBtn,string svIdGuncel,TextBox kulSifreTxt,TextBox emailTxt,DateTimePicker dateTimePicker1)
        {
            SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
            SqlCeCommand komut = new SqlCeCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from HotspotTBL H , ServerTBL S where S.serverId=H.serverId and H.kullaniciAdi='" + kAdiTxt.Text + "' and S.serverAdi='" + svAdiTxt.Text + "'";
            komut.ExecuteNonQuery();
            SqlCeDataReader dr = komut.ExecuteReader();
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
