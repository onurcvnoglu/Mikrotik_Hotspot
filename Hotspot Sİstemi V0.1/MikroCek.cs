using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Hotspot_Sİstemi_V0._1
{
    class MikroCek
    {
        TimeSpan saatEkle;
        public void VeriAl(string svIP, string svKulAdi, string svSifre, int serverId)
        {
            try
            {
                MK mikrotik = new MK(svIP);
                if (mikrotik.Login(svKulAdi, svSifre))
                {
                    mikrotik.Send("/ip/hotspot/user/print");
                    mikrotik.Send("=detail=", true);

                    foreach (string h in mikrotik.Read())
                    {
                        if (h != "!done")
                        {
                            string kuladi = h.Substring(h.IndexOf("name") + 5);
                            string kes = kuladi.Substring(kuladi.IndexOf("=password"));
                            kuladi = kuladi.Replace(kes, "");
                            string sifre = h.Substring(h.IndexOf("password=") + 9);
                            string sifreKes = sifre.Substring(sifre.IndexOf("=profile"));
                            sifre = sifre.Replace(sifreKes, "");
                            try
                            {
                                SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                                SqlCeCommand cmd = new SqlCeCommand();
                                if (baglanti.State == ConnectionState.Closed)
                                {
                                    baglanti.Open();
                                }
                                cmd.Connection = baglanti;
                                cmd.CommandText = "select * from HotspotTBL H , ServerTBL S where H.serverId='" + serverId + "' and H.kullaniciAdi='" + kuladi + "'";
                                cmd.ExecuteNonQuery();
                                SqlCeDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {

                                }
                                else
                                {
                                    dr.Close();//datareader i kapattık 
                                    saatEkle = TimeSpan.FromDays(365);
                                    //KULLANICI EKLEME

                                    //gün ve saat ekleme
                                    string date = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now.Add(saatEkle));//zamanı gün olarak arttırdık
                                    if (baglanti.State == ConnectionState.Closed)
                                        baglanti.Open();
                                    string kayit = "insert into HotspotTBL(serverId,kullaniciAdi,sifre,sure) values (@serverId,@kullaniciAdi,@sifre,@sure)";
                                    SqlCeCommand komut = new SqlCeCommand(kayit, baglanti);
                                    komut.Parameters.AddWithValue("@serverId", serverId);
                                    komut.Parameters.AddWithValue("@kullaniciAdi", kuladi);
                                    komut.Parameters.AddWithValue("@sifre", sifre);
                                    komut.Parameters.AddWithValue("@sure", date);
                                    komut.ExecuteNonQuery();
                                    baglanti.Close();
                                }
                            }
                            catch (Exception hata)
                            {
                                MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Bağlantı işlemi başarısız");
                    mikrotik.Close();
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Server bağlantısı başarısız");
            }
            
        }
    }
}
