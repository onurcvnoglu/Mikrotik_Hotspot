using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotspot_Sİstemi_V0._1
{
    public partial class yoneticiAyar : Form
    {
        Kullanici kullanici = new Kullanici();
        serverAyar serverayar = new serverAyar();
        string svID; //serverId si için
        string svIp; //serveripsi
        string svKulAdi; //server kullanıcı adı
        string svSifre; //server şifresi
        string svIdGuncel;

        public yoneticiAyar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            groupBox1.Visible = true;
            groupBox2.Visible = false;
            textBox5.ReadOnly = false;
            textBox5.Text = "";
            textBox1.Text = "";
            textBox1.ReadOnly = true;
            textBox4.Text = "";
            textBox4.ReadOnly = true;
            textBox6.Text = "";
            textBox6.ReadOnly = true;
            listBox3_SelectedIndexChanged(sender, e);
            saatTxt.Text = "";
            saatTxt.ReadOnly = false;
            saatTxt.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            groupBox1.Visible = false;
            kulSifreTxt.Text = "";
            emailTxt.Text = "";
            kAdiTxt.ReadOnly = false;
            svAdiTxt.ReadOnly = false;
            kAdiTxt.Text = "";
            svAdiTxt.Text = "";
            dateTimePicker1.Text = "";
            listBox2_SelectedIndexChanged(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //KULLANICI GÜNCELLEME
            if (kulSifreTxt.Text == "" || telnoDuzenTxt.Text=="")
            {
                MessageBox.Show("Boş alan bırakmayınız");
            }
            else
            {
                try
                {
                    if (guncelSaatTxt.Text=="")
                    {
                        guncelSaatTxt.Text = "0";
                    }
                    //TimeSpan gunEkle = TimeSpan.FromDays(Convert.ToInt32(guncelGunTxt.Text));
                    TimeSpan saatEkle = TimeSpan.FromHours(Convert.ToInt32(guncelSaatTxt.Text));
                    //TimeSpan zamanEkle = gunEkle.Add(saatEkle);
                    string sure = dateTimePicker1.Text;
                    string date = string.Format("{0:dd/MM/yyyy HH:mm:ss}", dateTimePicker3.Value.Add(saatEkle)/*Convert.ToDateTime(sure).Add(zamanEkle)*/);//zamanı gün olarak güncelle
                    //date = string.Format("{0:dd/MM/yyyy HH:mm:ss}", sure.AddHours(Convert.ToInt32(guncelSaatTxt.Text)));//zamanı saat olarak güncelle
                    SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
                    SqlCommand cmd = new SqlCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    cmd.Connection = baglanti;
                    cmd.CommandText = "update HotspotTBL set sifre=@sifre, email=@email, sure=@sure, telNo=@telNo where serverId='" + svIdGuncel + "' and kullaniciAdi='" + kAdiTxt.Text + "' ";
                    cmd.Parameters.AddWithValue("@sifre", kulSifreTxt.Text);
                    cmd.Parameters.AddWithValue("@email", emailTxt.Text);
                    cmd.Parameters.AddWithValue("@sure", date);
                    cmd.Parameters.AddWithValue("@telNo", telnoDuzenTxt.Text);
                    cmd.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Kullanıcı Bilgileri Güncellendi");
                    serverVeriCek();
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
                        mikrotik.Send("=.id=" + kAdiTxt.Text + "", true);
                    }
                    if (!mikrotik.Login(svKulAdi, svSifre))
                    {
                        MessageBox.Show("Bağlantı işlemi başarısız");
                        mikrotik.Close();
                        return;
                    }
                    else
                    {
                        mikrotik.Send("/ip/hotspot/user/add");
                        mikrotik.Send("=name=" + kAdiTxt.Text + "");
                        mikrotik.Send("=password=" + kulSifreTxt.Text + "");
                        mikrotik.Send("=profile=default", true);
                    }
                    
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Güncelleme işlemi başarısız " + hata.Message);
                }
                yoneticiAyar_Load(sender, e);
                guncelSaatTxt.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox4.Text == "" || telNoTxt.Text=="")
            {
                MessageBox.Show("Gerekli (*) Alanları Doldurunuz");
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
                    cmd.CommandText = "select * from HotspotTBL H , ServerTBL S where H.serverId='" + svID + "' and (H.kullaniciAdi='" + textBox1.Text + "' and H.email='" + textBox6.Text + "') or H.kullaniciAdi='" + textBox1.Text + "'";
                    cmd.ExecuteNonQuery();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Bu kullanıcı adına ait bir kullanıcı bulunmaktadır.");
                        textBox1.Text = "";
                        textBox4.Text = "";
                        textBox6.Text = "";
                        saatTxt.Text = "";
                    }
                    else
                    {
                        dr.Close();//datareader i kapattık 
                        
                        //// mikrotik
                        MK mikrotik = new MK(svIp);
                        if (!mikrotik.Login(svKulAdi, svSifre))
                        {
                            MessageBox.Show("Bağlantı işlemi başarısız");
                            mikrotik.Close();
                            return;
                        }
                        else
                        {
                            mikrotik.Send("/ip/hotspot/user/add");
                            mikrotik.Send("=name=" + textBox1.Text + "");
                            mikrotik.Send("=password=" + textBox4.Text + "");
                            mikrotik.Send("=profile=default", true);
                            //KULLANICI EKLEME
                            
                            if (saatTxt.Text=="")
                            {
                                saatTxt.Text = "0";
                            }
                            //gün ve saat ekleme
                            //
                            //TimeSpan gunEkle = TimeSpan.FromDays(dateTimePicker2.Value.Day/*Convert.ToInt32(textBox7.Text)*/);
                            TimeSpan saatEkle = TimeSpan.FromHours(Convert.ToInt32(saatTxt.Text));
                            //TimeSpan zamanEkle = gunEkle.Add(saatEkle);
                            string date = string.Format("{0:dd/MM/yyyy HH:mm:ss}", dateTimePicker2.Value.Add(saatEkle)/*DateTime.Now.Add(zamanEkle)*/);//zamanı gün olarak arttırdık
                            if (baglanti.State == ConnectionState.Closed)
                                baglanti.Open();
                            string kayit = "insert into HotspotTBL(serverId,kullaniciAdi,sifre,email,sure,telNo) values (@serverId,@kullaniciAdi,@sifre,@email,@sure,@telNo)";
                            SqlCommand komut = new SqlCommand(kayit, baglanti);
                            komut.Parameters.AddWithValue("@serverId", svID);
                            komut.Parameters.AddWithValue("@kullaniciAdi", textBox1.Text);
                            komut.Parameters.AddWithValue("@sifre", textBox4.Text);
                            komut.Parameters.AddWithValue("@email", textBox6.Text);
                            komut.Parameters.AddWithValue("@sure", date);
                            komut.Parameters.AddWithValue("@telNo", telNoTxt.Text);
                            komut.ExecuteNonQuery();
                            MessageBox.Show("Kullanıcı Kayıt İşlemi Gerçekleşti.");
                            baglanti.Close();
                        }
                        ////
                    }

                    /////////

                }
                catch (Exception hata)
                {
                    MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                }

            }
        }

        private void yoneticiAyar_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm:ss";

            button4.Enabled = false;
            //listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            serverayar.serverListele(listBox2);
            serverayar.serverListele(listBox3);
            if (listBox1.Items.Count>0)
            {
                listBox1.SelectedIndex = 0;
            }
            if (listBox2.Items.Count>0)
            {
                listBox2.SelectedIndex = 0;
            }
            if (listBox3.Items.Count > 0)
            {
                listBox3.SelectedIndex = 0;
            }

            //listBox3_SelectedIndexChanged(sender, e);
        }

        public void ServerVeri()
        {
            SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
            SqlCommand komut = new SqlCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from ServerTBL S , HotspotTBL H where S.serverId=H.serverId and H.kullaniciAdi='" + kAdiTxt.Text + "' and S.serverAdi='" + svAdiTxt.Text + "'";
            komut.ExecuteNonQuery();
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                kulSifreTxt.Text = dr["sifre"].ToString();
                emailTxt.Text = dr["email"].ToString();
                dateTimePicker1.Text = dr["sure"].ToString();
                //telnoDuzenTxt.Text = dr["telNo"].ToString();
            }
        }
        public void serverVeriCek()  //Server Bilgilerini Çektik.
        {
            SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
            SqlCommand komut = new SqlCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from ServerTBL where serverId='" + svIdGuncel + "'";
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

        private void silBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
                SqlCommand komut = new SqlCommand();
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                komut.Connection = baglanti;
                komut.CommandText = "delete from HotspotTBL where kullaniciAdi='" + kAdiTxt.Text + "' and serverId='"+svIdGuncel+"'";
                komut.ExecuteNonQuery();
                serverVeriCek();
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
                    mikrotik.Send("=.id=" + kAdiTxt.Text + "", true);
                }
                MessageBox.Show("Kullanıcı Silindi");
                //groupBox2.Visible = false;
            }
            catch (Exception hata)
            {
                MessageBox.Show("İşlem hatası" + hata.Message);
            }
            yoneticiAyar_Load(sender, e);
        }

        private void saatTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void guncelSaatTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '-';
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //şifreyi göstermek için
                textBox4.PasswordChar = '\0';
                checkBox1.Text = "Şifreyi Gizle";
            }
            else
            {
                //şifreyi gizlemek için
                textBox4.PasswordChar = '*';
                checkBox1.Text = "Şifreyi Göster";
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                textBox5.Text = listBox3.SelectedItem.ToString();
                try
                {
                    SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
                    SqlCommand komut = new SqlCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    komut.Connection = baglanti;
                    komut.CommandText = "select * from ServerTBL where serverAdi='" + textBox5.Text + "'";
                    komut.ExecuteNonQuery();
                    SqlDataReader dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        svID = dr["serverId"].ToString();
                        svIp = dr["ipAdres"].ToString();
                        svKulAdi = dr["kullaniciAdi"].ToString();
                        svSifre = dr["sifre"].ToString();
                        textBox5.ReadOnly = true;
                        button4.Enabled = true;
                        textBox1.ReadOnly = false;
                        textBox4.ReadOnly = false;
                        textBox6.ReadOnly = false;
                        //textBox7.ReadOnly = false;
                        saatTxt.ReadOnly = false;
                    }
                    else
                    {

                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Bir hata oluştu" + hata.Message);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && listBox2.SelectedItem != null)
            {
                kAdiTxt.Text = listBox1.SelectedItem.ToString();
                svAdiTxt.Text = listBox2.SelectedItem.ToString();
                //Kullanıcı bulma işlemi
                SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
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
                    telnoDuzenTxt.Text = dr["telNo"].ToString();

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
                dateTimePicker3.Text = dateTimePicker1.Text;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                kAdiTxt.Text = "";
                svAdiTxt.Text = "";
                listBox1.Items.Clear();
                serverayar.kullaniciServerListe(listBox2, listBox1);
            }
        }

        private void ServerEkleMenuStrip_Click(object sender, EventArgs e)
        {
            serverEkle sEkle = new serverEkle();
            sEkle.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
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
            //yoneticiAyar yAyar = new yoneticiAyar();
            //yAyar.Show();
            //this.Hide();
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
