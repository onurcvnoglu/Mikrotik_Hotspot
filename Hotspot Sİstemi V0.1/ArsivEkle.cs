using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace Hotspot_Sİstemi_V0._1
{
    class ArsivEkle
    {
        string kullaniciAdi;
        string sifre;
        string email;
        string telNo;
        string yoneticiAdi;

        public void Listele(string sillKulAdiAl,string silSvId)
        {
            yoneticiAdi = Properties.Settings.Default["yoneticiAdi"].ToString();

            SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
            try
            {
                string tarih = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
                SqlCeCommand cmd = new SqlCeCommand();
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                cmd.Connection = baglanti;
                cmd.CommandText = "select * from HotspotTBL where serverId='" + silSvId + "' and kullaniciAdi='" + sillKulAdiAl + "'";
                cmd.ExecuteNonQuery();
                SqlCeDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    kullaniciAdi = dr["kullaniciAdi"].ToString();
                    sifre = dr["sifre"].ToString();
                    email = dr["email"].ToString();
                    telNo = dr["telNo"].ToString();
                }
                dr.Close();
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                string kayit = "insert into ArsivTBL(yoneticiAdi,kullaniciAdi,sifre,email,telNo,tarih) values(@yoneticiAdi,@kullaniciAdi,@sifre,@email,@telNo,@tarih)";
                SqlCeCommand komut = new SqlCeCommand(kayit, baglanti);
                komut.Parameters.AddWithValue("@yoneticiAdi", yoneticiAdi);
                komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                komut.Parameters.AddWithValue("@sifre", sifre);
                komut.Parameters.AddWithValue("@email", email);
                komut.Parameters.AddWithValue("@telNo", telNo);
                komut.Parameters.AddWithValue("@tarih", tarih);
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            catch (Exception h)
            {
                MessageBox.Show("İşlem hatası" + h);
            }
        }
        public void excele_aktar(DataGridView dg)
        {
            dg.AllowUserToAddRows = false;
            System.Globalization.CultureInfo dil = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
            Microsoft.Office.Interop.Excel.Application Tablo = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook kitap = Tablo.Workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet sayfa = (Microsoft.Office.Interop.Excel.Worksheet)Tablo.ActiveSheet;
            System.Threading.Thread.CurrentThread.CurrentCulture = dil;
            Tablo.Visible = true;
            sayfa = (Worksheet)kitap.ActiveSheet;
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                for (int j = 0; j < dg.ColumnCount; j++)
                {
                    if (i == 0)
                    {
                        Tablo.Cells[1, j + 1] = dg.Columns[j].HeaderText;
                        Tablo.Cells[1, j + 1].ColumnWidth = 20;
                    }
                    Tablo.Cells[i + 2, j + 1] = dg.Rows[i].Cells[j].Value.ToString();
                }
            }
            Tablo.Visible = true;
            Tablo.UserControl = true;
        }

        public void serverListele(string svAdiSil)
        {
            yoneticiAdi = Properties.Settings.Default["yoneticiAdi"].ToString();

            SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
            try
            {
                string tarih = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
                SqlCeCommand cmd = new SqlCeCommand();
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                cmd.Connection = baglanti;
                cmd.CommandText = "select * from HotspotTBL H, ServerTBL S where H.serverId=S.serverId and S.serverAdi='" + svAdiSil + "'";
                cmd.ExecuteNonQuery();
                SqlCeDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    kullaniciAdi = dr["kullaniciAdi"].ToString();
                    sifre = dr["sifre"].ToString();
                    email = dr["email"].ToString();
                    telNo = dr["telNo"].ToString();

                    string kayit = "insert into ArsivTBL(yoneticiAdi,kullaniciAdi,sifre,email,telNo,tarih) values(@yoneticiAdi,@kullaniciAdi,@sifre,@email,@telNo,@tarih)";
                    SqlCeCommand komut = new SqlCeCommand(kayit, baglanti);
                    komut.Parameters.AddWithValue("@yoneticiAdi", yoneticiAdi);
                    komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    komut.Parameters.AddWithValue("@sifre", sifre);
                    komut.Parameters.AddWithValue("@email", email);
                    komut.Parameters.AddWithValue("@telNo", telNo);
                    komut.Parameters.AddWithValue("@tarih", tarih);
                    komut.ExecuteNonQuery();
                }
                dr.Close();
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("İşlem hatası : "+hata);
            }
        }
    }
}
