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
using System.Threading;
using System.Data.SqlServerCe;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using tik4net;
using tik4net.Objects;
using tik4net.Objects.Ip.Hotspot;

namespace Hotspot_Sİstemi_V0._1
{
    public partial class GenelSayfa : Form
    {
        int sayac = 0;
        int sayac2 = 0;
        SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
        SqlCeDataAdapter da;
        
        DataTable dt;
        serverAyar sAyar = new serverAyar();
        yonetici yoneticiDuzenle = new yonetici();
        Kullanici kullanici = new Kullanici();
        serverAyar serverayar = new serverAyar();
        KullaniciSil kullanSil = new KullaniciSil();
        string svID; //serverId si için
        string svIp; //serveripsi
        string svKulAdi; //server kullanıcı adı
        string svSifre; //server şifresi
        string svIdGuncel;

        public GenelSayfa()
        {
            InitializeComponent();
        }

        private void GenelSayfa_Load(object sender, EventArgs e)
        {
            label46.Text = Properties.Settings.Default["otelKodu"].ToString(); // otel kodunu al, default olarak

            if (label46.Text!="")
            {
                timer2.Start();
            }
            else
            {
                timer2.Stop();
            }
            tabControl1.TabPages.Remove(tabPage3);
            DosyaSil ds = new DosyaSil();
            ds.SetupSil();
            dateTimePicker4.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            kullanSil.kullanici_Sil();
            timer1.Start();
            SqlCeCommand cmd = new SqlCeCommand("Select * from HotspotTBL where serverId='"+svID+"' order by sure", baglanti);
            da = new SqlCeDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            //isimleri değiştirme

            dataGridView1.Columns[0].HeaderCell.Value = "Server ID";
            dataGridView1.Columns[2].HeaderCell.Value = "Kullanıcı Adı";
            dataGridView1.Columns[3].HeaderCell.Value = "Şifre";
            dataGridView1.Columns[5].HeaderCell.Value = "Süre";
            dataGridView1.Columns[6].HeaderCell.Value = "Telefon No";
            dataGridView1.CurrentCell = null;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                serverAdiGuncelTxt.Text = listBox1.SelectedItem.ToString();
                sAyar.serverBul(serverAdiGuncelTxt, ipAdresGuncelTxt, kulAdiGuncelTxt, sifreGuncelTxt, guncelleBtn, silBtn);
            }
        }

        private void guncelleBtn_Click(object sender, EventArgs e)
        {
            sAyar.serverGuncelle(ipAdresGuncelTxt, kulAdiGuncelTxt, sifreGuncelTxt, serverAdiGuncelTxt);
            tabPage3_Enter(sender, e);
            //tabPage4_Enter(sender, e);
        }

        private void silBtn_Click(object sender, EventArgs e)
        {
            sAyar.routerKullaniciKontrol(serverAdiGuncelTxt);
            sAyar.serverSil(serverAdiGuncelTxt);

            serverAdiGuncelTxt.Text = "";
            serverAdiGuncelTxt.ReadOnly = false;
            ipAdresGuncelTxt.Text = "";
            kulAdiGuncelTxt.Text = "";
            sifreGuncelTxt.Text = "";
            guncelleBtn.Enabled = false;
            tabPage3_Enter(sender, e);
            //tabPage4_Enter(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serverAdiGuncelTxt.Text = "";
            serverAdiGuncelTxt.ReadOnly = false;
            ipAdresGuncelTxt.Text = "";
            kulAdiGuncelTxt.Text = "";
            sifreGuncelTxt.Text = "";
            guncelleBtn.Enabled = false;
            silBtn.Enabled = false;
            //tabPage4_Enter(sender, e);
            tabPage3_Enter(sender, e);
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            sAyar.serverListele(listBox1);

            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
            }
        }

        private void tabPage4_Enter(object sender, EventArgs e)
        {
            tabPage3_Enter(sender, e);
            listBox2.Items.Clear();
            yoneticiDuzenle.yoneticiListele(listBox2);
            listBox2.SelectedIndex = 0;
            tabPage5_Enter(sender,e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;
            groupBox4.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = true;
            groupBox3.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            textBox10.Text = "";
            sifreDuzTxt.Text = "";
            emailDuzTxt.Text = "";
            seviyeDuzTxt.Text = "";
            silBtn.Enabled = false;
            guncelBtn.Enabled = false;
            textBox10.ReadOnly = false;
            listBox2_SelectedIndexChanged(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            yonetici yoneticiK = new yonetici();
            yoneticiK.yoneticiEkle(kulAdiTxt.Text, sifreTxt.Text, emailTxt.Text, seviyeTxt.Text);

            kulAdiTxt.Text = "";
            sifreTxt.Text = "";
            emailTxt.Text = "";
            seviyeTxt.Text = "";
            tabPage4_Enter(sender, e);
        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            //Girince sıfırla
            textBox8.Text = "";
            textBox7.Text = "";
            saatTxt.Text = "";
            //
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            //listBox1.Items.Clear();
            listBox4.Items.Clear();
            listBox3.Items.Clear();
            listBox6.Items.Clear();
            listBox7.Items.Clear();
            serverayar.serverListele(listBox6);
            serverayar.serverListele(listBox4);
            serverayar.serverListele(listBox3);
            serverayar.serverListele(listBox7);
            if (listBox5.Items.Count > 0)
            {
                listBox5.SelectedIndex = 0;
            }
            if (listBox4.Items.Count > 0)
            {
                listBox4.SelectedIndex = 0;
            }
            if (listBox3.Items.Count > 0)
            {
                listBox3.SelectedIndex = 0;
            }
            if (listBox6.Items.Count > 0)
            {
                listBox6.SelectedIndex = 0;
            }
            if (listBox7.Items.Count > 0)
            {
                listBox7.SelectedIndex = 0;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                textBox5.Text = listBox3.SelectedItem.ToString();
                try
                {
                    SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                    SqlCeCommand komut = new SqlCeCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    komut.Connection = baglanti;
                    komut.CommandText = "select * from ServerTBL where serverAdi='" + textBox5.Text + "'";
                    komut.ExecuteNonQuery();
                    SqlCeDataReader dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        svID = dr["serverId"].ToString();
                        svIp = dr["ipAdres"].ToString();
                        svKulAdi = dr["kullaniciAdi"].ToString();
                        svSifre = dr["sifre"].ToString();
                        textBox5.ReadOnly = true;
                        button10.Enabled = true;
                        textBox8.ReadOnly = false;
                        textBox7.ReadOnly = false;
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
        //
        TimeSpan saatEkle;
        //
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox8.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show("Gerekli (*) Alanları Doldurunuz");
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
                    cmd.CommandText = "select * from HotspotTBL H , ServerTBL S where H.serverId='" + svID + "' and (H.kullaniciAdi='" + textBox8.Text + "' and H.email='" + textBox6.Text + "') or H.kullaniciAdi='" + textBox8.Text + "'";
                    cmd.ExecuteNonQuery();
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Bu kullanıcı adına ait bir kullanıcı bulunmaktadır.");
                        textBox8.Text = "";
                        textBox7.Text = "";
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
                            mikrotik.Send("=name=" + textBox8.Text + "");
                            mikrotik.Send("=password=" + textBox7.Text + "");
                            mikrotik.Send("=profile=default", true);
                            //KULLANICI EKLEME

                            if (saatTxt.Text == "")
                            {
                                saatEkle = TimeSpan.FromDays(365);
                            }
                            else
                            {
                                saatEkle = TimeSpan.FromHours(Convert.ToInt32(saatTxt.Text));
                            }

                            //gün ve saat ekleme
                            string date = string.Format("{0:yyyy/MM/dd HH:mm:ss}", dateTimePicker2.Value.Add(saatEkle));//zamanı gün olarak arttırdık
                            if (baglanti.State == ConnectionState.Closed)
                                baglanti.Open();
                            string kayit = "insert into HotspotTBL(serverId,kullaniciAdi,sifre,email,sure,telNo) values (@serverId,@kullaniciAdi,@sifre,@email,@sure,@telNo)";
                            SqlCeCommand komut = new SqlCeCommand(kayit, baglanti);
                            komut.Parameters.AddWithValue("@serverId", svID);
                            komut.Parameters.AddWithValue("@kullaniciAdi", textBox8.Text);
                            komut.Parameters.AddWithValue("@sifre", textBox7.Text);
                            komut.Parameters.AddWithValue("@email", textBox6.Text);
                            komut.Parameters.AddWithValue("@sure", date);
                            komut.Parameters.AddWithValue("@telNo", telNoTxt.Text);
                            komut.ExecuteNonQuery();
                            MessageBox.Show("Kullanıcı Kayıt İşlemi Gerçekleşti.");
                            baglanti.Close();
                        }
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                }

            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem != null && listBox4.SelectedItem != null)
            {
                textBox10.Text = listBox5.SelectedItem.ToString();
                svAdiTxt.Text = listBox4.SelectedItem.ToString();
                //Kullanıcı bulma işlemi
                SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                SqlCeCommand komut = new SqlCeCommand();
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                komut.Connection = baglanti;
                komut.CommandText = "select * from HotspotTBL H , ServerTBL S where S.serverId=H.serverId and H.kullaniciAdi='" + textBox10.Text + "' and S.serverAdi='" + svAdiTxt.Text + "'";
                komut.ExecuteNonQuery();
                SqlCeDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    svIdGuncel = dr["serverId"].ToString();
                    kulSifreTxt.Text = dr["sifre"].ToString();
                    emailTxt.Text = dr["email"].ToString();
                    dateTimePicker1.Text = dr["sure"].ToString();
                    telnoDuzenTxt.Text = dr["telNo"].ToString();

                    kAdiTxt.ReadOnly = true;
                    svAdiTxt.ReadOnly = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Bu Server ve Kullanıcıya ait Bilgi bulunmamaktadır. ");
                }
                baglanti.Close();
                dateTimePicker3.Text = dateTimePicker1.Text;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //button4.Enabled = false;
            groupBox5.Visible = true;
            groupBox6.Visible = false;
            textBox5.ReadOnly = false;
            textBox5.Text = "";
            textBox8.Text = "";
            textBox8.ReadOnly = true;
            listBox3_SelectedIndexChanged(sender, e);
            saatTxt.Text = "";
            saatTxt.ReadOnly = false;
            saatTxt.Text = "";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            groupBox6.Visible = true;
            groupBox5.Visible = false;
            kulSifreTxt.Text = "";
            emailTxt.Text = "";
            //kAdiTxt.ReadOnly = false;
            textBox10.ReadOnly = false;
            svAdiTxt.ReadOnly = false;
            textBox10.Text = "";
            svAdiTxt.Text = "";
            dateTimePicker1.Text = "";
            listBox4_SelectedIndexChanged(sender, e);
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem != null)
            {
                textBox10.Text = "";
                svAdiTxt.Text = "";
                listBox5.Items.Clear();
                serverayar.kullaniciServerListe(listBox4, listBox5);
            }
            button11.Enabled = false;
            button12.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                //
                ArsivEkle aEkle = new ArsivEkle();
                anaGiris agiris = new anaGiris();
                aEkle.Listele(textBox10.Text, svIdGuncel);
                //
                SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                SqlCeCommand komut = new SqlCeCommand();
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                komut.Connection = baglanti;
                komut.CommandText = "delete from HotspotTBL where kullaniciAdi='" + textBox10.Text + "' and serverId='" + svIdGuncel + "' ";
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
                    mikrotik.Send("=.id=" + textBox10.Text + "", true);
                }
                MessageBox.Show("Kullanıcı Silindi");
                //groupBox2.Visible = false;
                button11.Enabled = false;
                button12.Enabled = false;
            }
            catch (Exception hata)
            {
                MessageBox.Show("İşlem hatası" + hata.Message);
            }
            tabPage5_Enter(sender, e);
        }
        public void serverVeriCek()  //Server Bilgilerini Çektik.
        {
            SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
            SqlCeCommand komut = new SqlCeCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from ServerTBL where serverId='" + svIdGuncel + "'";
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

        private void button12_Click(object sender, EventArgs e)
        {
            //KULLANICI GÜNCELLEME
            if (kulSifreTxt.Text == "")
            {
                MessageBox.Show("Boş alan bırakmayınız");
            }
            //else if (textBox10.Text=="")
            //{
            //    MessageBox.Show("Önce Kullanıcı Seçiniz");
            //}
            else
            {
                try
                {
                    if (guncelSaatTxt.Text == "")
                    {
                        guncelSaatTxt.Text = "0";
                    }
                    //TimeSpan gunEkle = TimeSpan.FromDays(Convert.ToInt32(guncelGunTxt.Text));
                    TimeSpan saatEkle = TimeSpan.FromHours(Convert.ToInt32(guncelSaatTxt.Text));
                    //TimeSpan zamanEkle = gunEkle.Add(saatEkle);
                    string sure = dateTimePicker1.Text;
                    string date = string.Format("{0:yyyy/MM/dd HH:mm:ss}", dateTimePicker3.Value.Add(saatEkle)/*Convert.ToDateTime(sure).Add(zamanEkle)*/);//zamanı gün olarak güncelle
                    //date = string.Format("{0:dd/MM/yyyy HH:mm:ss}", sure.AddHours(Convert.ToInt32(guncelSaatTxt.Text)));//zamanı saat olarak güncelle
                    SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                    SqlCeCommand cmd = new SqlCeCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    cmd.Connection = baglanti;
                    cmd.CommandText = "update HotspotTBL set sifre=@sifre, email=@email, sure=@sure, telNo=@telNo where serverId='" + svIdGuncel + "' and kullaniciAdi='" + textBox10.Text + "' ";
                    cmd.Parameters.AddWithValue("@sifre", kulSifreTxt.Text);
                    cmd.Parameters.AddWithValue("@email", textBox9.Text);
                    cmd.Parameters.AddWithValue("@sure", date);
                    cmd.Parameters.AddWithValue("@telNo", telnoDuzenTxt.Text);
                    cmd.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Kullanıcı Bilgileri Güncellendi");
                    //
                    button12.Enabled = false;
                    button11.Enabled = false;
                    //
                    serverVeriCek();
                    using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                    {
                        connection.Open(svIp, svKulAdi, svSifre);
                        var updateCmd = connection.CreateCommandAndParameters("/ip/hotspot/user/set", "password", kulSifreTxt.Text, TikSpecialProperties.Id, textBox10.Text);
                        updateCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Güncelleme işlemi başarısız " + hata.Message);
                }
                tabPage5_Enter(sender, e);
                guncelSaatTxt.Text = "";
            }
        }

        private void tabPage6_Enter(object sender, EventArgs e)
        {
            anaGiris aGiris = new anaGiris();
            this.Hide();
            aGiris.Show();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                kAdiTxt.Text = listBox2.SelectedItem.ToString();
                yoneticiDuzenle.yoneticiBul(kAdiTxt, sifreDuzTxt, emailDuzTxt, seviyeDuzTxt, guncelBtn, button7);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            yonetici yoneticiS = new yonetici();
            yoneticiS.yoneticiSil(kAdiTxt.Text);
            tabPage4_Enter(sender, e);
        }

        private void guncelBtn_Click(object sender, EventArgs e)
        {
            yonetici yoneticiG = new yonetici();
            yoneticiG.yoneticiGuncelle(kAdiTxt, sifreDuzTxt, emailDuzTxt, seviyeDuzTxt);
            tabPage4_Enter(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
            tabPage4_Enter(sender, e);
        }
        public void ServerVeri()
        {
            SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
            SqlCeCommand komut = new SqlCeCommand();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            komut.Connection = baglanti;
            komut.CommandText = "select * from ServerTBL S , HotspotTBL H where S.serverId=H.serverId and H.kullaniciAdi='" + kAdiTxt.Text + "' and S.serverAdi='" + svAdiTxt.Text + "'";
            komut.ExecuteNonQuery();
            SqlCeDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                kulSifreTxt.Text = dr["sifre"].ToString();
                emailTxt.Text = dr["email"].ToString();
                dateTimePicker1.Text = dr["sure"].ToString();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                //şifreyi göstermek için
                textBox7.PasswordChar = '\0';
                checkBox2.Text = "Şifreyi Gizle";
            }
            else
            {
                //şifreyi gizlemek için
                textBox7.PasswordChar = '*';
                checkBox2.Text = "Şifreyi Göster";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //şifreyi göster
                checkBox1.Text = "Şifreyi Gizle";
                sifreTxt.PasswordChar = '\0';
            }
            else
            {
                //şifreyi gizle
                checkBox1.Text = "Şifreyi Göster";
                sifreTxt.PasswordChar = '*';
            }
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            GenelSayfa_Load(sender,e);
            tabPage5_Enter(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;

            if (sayac % 1800 == 0)
            {
                GenelSayfa_Load(sender, e);
                sayac = 0;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            groupBox1.Visible = true;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            groupBox4.Visible = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            tabPage3_Enter(sender, e);
            groupBox1.Visible = false;
            groupBox2.Visible = true;
            groupBox3.Visible = false;
            groupBox4.Visible = false;
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            listBox1_SelectedIndexChanged(sender, e);
        }

        private void guncelleBtn_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            guncelleBtn_Click(sender, e);
            tabPage5_Enter(sender, e);
        }

        private void silBtn_Click_1(object sender, EventArgs e)
        {
            silBtn_Click(sender, e);
            tabPage5_Enter(sender, e);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //SERVER EKLEME
            if (textBox1.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Boş alan Bırakmayınız");
            }
            else
            {
                SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                try
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();
                    SqlCeCommand cmd = new SqlCeCommand();
                    cmd.Connection = baglanti;
                    cmd.CommandText = "Select * from ServerTBL where serverAdi='" + textBox1.Text + "' or ipAdres='" + textBox3.Text + "'";
                    cmd.ExecuteNonQuery();
                    SqlCeDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Bu alana ait Server bulunmaktadır.Farklı bir server giriniz");
                    }
                    else
                    {
                        dr.Close();
                        MK mikrotik = new MK(textBox3.Text);
                        if (mikrotik.Login(textBox2.Text,textBox4.Text))
                        {
                            string kayit = "insert into ServerTBL(serverAdi,ipAdres,kullaniciAdi,sifre) values (@serverAdi,@ipAdres,@kullaniciAdi,@sifre)";
                            SqlCeCommand komut = new SqlCeCommand(kayit, baglanti);
                            komut.Parameters.AddWithValue("@serverAdi", textBox1.Text);
                            komut.Parameters.AddWithValue("@ipAdres", textBox3.Text);
                            komut.Parameters.AddWithValue("@kullaniciAdi", textBox2.Text);
                            komut.Parameters.AddWithValue("@sifre", textBox4.Text);
                            komut.ExecuteNonQuery();
                            baglanti.Close();
                            MessageBox.Show("Server Kayıt İşlemi Gerçekleşti.");
                            button13_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Server Bağlantısı Başarısız.Tekrar Deneyiniz", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    baglanti.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("İşlem Sırasında Hata Oluştu.Server Bilgilerini Tekrar Giriniz");
                }
            }
            tabPage5_Enter(sender, e);
        }

        private void button15_Click(object sender, EventArgs e) //anasayfa güncelle butonu
        {
            if (dataGridView1.CurrentCell!=null)    //datagridde seçiliyse işlemi yapıyor
            {
                svIdGuncel = dataGridView1.CurrentRow.Cells[0].Value.ToString();    //sv id ye seçili satırdaki serverin idsini atadık

                try
                {
                    SqlCeCommandBuilder cmdBldr = new SqlCeCommandBuilder(da); //güncelleme
                    cmdBldr.GetUpdateCommand();
                    da.Update(dt);

                    serverVeriCek();
                    using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                    {
                        connection.Open(svIp, svKulAdi, svSifre);

                        var updateCmd = connection.CreateCommandAndParameters("/ip/hotspot/user/set", "password", dataGridView1.CurrentRow.Cells[3].Value.ToString(), TikSpecialProperties.Id, dataGridView1.CurrentRow.Cells[2].Value.ToString());
                        updateCmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Kullanıcı Güncellendi");
                }
                catch (Exception)
                {
                    MessageBox.Show("Gerekli alanları doğru şekilde giriniz");
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı Seçiniz","Güncelleme",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            GenelSayfa_Load(sender, e);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Thread.Sleep(500);
            GenelSayfa_Load(sender, e);
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            if (listBox6.SelectedItem != null)
            {
                textBox5.Text = listBox6.SelectedItem.ToString();
                try
                {
                    SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                    SqlCeCommand komut = new SqlCeCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    komut.Connection = baglanti;
                    komut.CommandText = "select * from ServerTBL where serverAdi='" + textBox5.Text + "'";
                    komut.ExecuteNonQuery();
                    SqlCeDataReader dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        svID = dr["serverId"].ToString();
                        svIp = dr["ipAdres"].ToString();
                        svKulAdi = dr["kullaniciAdi"].ToString();
                        svSifre = dr["sifre"].ToString();
                        textBox5.ReadOnly = true;
                        button10.Enabled = true;
                        textBox8.ReadOnly = false;
                        textBox7.ReadOnly = false;
                        textBox6.ReadOnly = false;
                        //textBox7.ReadOnly = false;
                        saatTxt.ReadOnly = false;
                    }
                    dr.Close();
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Bir hata oluştu" + hata.Message);
                }
            }
            GenelSayfa_Load(sender, e);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedItem==null)
            {
                MessageBox.Show("Önce Server Ekleyiniz");
            }
            else
            {
                if (textBox12.Text == "" || textBox11.Text == "")
                {
                    MessageBox.Show("Gerekli (*) Alanları Doldurunuz");
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
                        cmd.CommandText = "select * from HotspotTBL H , ServerTBL S where H.serverId='" + svID + "' and (H.kullaniciAdi='" + textBox12.Text + "' and H.email='" + textBox6.Text + "') or H.kullaniciAdi='" + textBox12.Text + "'";
                        cmd.ExecuteNonQuery();
                        SqlCeDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            MessageBox.Show("Bu kullanıcı adına ait bir kullanıcı bulunmaktadır.");
                            textBox12.Text = "";
                            textBox11.Text = "";
                            textBox6.Text = "";
                            textBox13.Text = "";
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
                                mikrotik.Send("=name=" + textBox12.Text + "");
                                mikrotik.Send("=password=" + textBox11.Text + "");
                                mikrotik.Send("=profile=default", true);
                                //KULLANICI EKLEME

                                if (textBox13.Text == "")
                                {
                                    saatEkle = TimeSpan.FromDays(365);
                                }
                                else
                                {
                                    saatEkle = TimeSpan.FromHours(Convert.ToInt32(textBox13.Text));
                                }

                                //gün ve saat ekleme
                                string date = string.Format("{0:yyyy/MM/dd HH:mm:ss}", dateTimePicker4.Value.Add(saatEkle));//zamanı gün olarak arttırdık
                                if (baglanti.State == ConnectionState.Closed)
                                    baglanti.Open();
                                string kayit = "insert into HotspotTBL(serverId,kullaniciAdi,sifre,email,sure,telNo) values (@serverId,@kullaniciAdi,@sifre,@email,@sure,@telNo)";
                                SqlCeCommand komut = new SqlCeCommand(kayit, baglanti);
                                komut.Parameters.AddWithValue("@serverId", svID);
                                komut.Parameters.AddWithValue("@kullaniciAdi", textBox12.Text);
                                komut.Parameters.AddWithValue("@sifre", textBox11.Text);
                                komut.Parameters.AddWithValue("@email", textBox6.Text);
                                komut.Parameters.AddWithValue("@sure", date);
                                komut.Parameters.AddWithValue("@telNo", textBox14.Text);
                                komut.ExecuteNonQuery();
                                MessageBox.Show("Kullanıcı Kayıt İşlemi Gerçekleşti.");
                                baglanti.Close();
                            }
                        }
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                    }

                }
            }
            GenelSayfa_Load(sender, e);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow!=null)
            {
                string kulAdiAl = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                //
                ArsivEkle aEkle = new ArsivEkle();
                aEkle.Listele(kulAdiAl, dataGridView1.CurrentRow.Cells[0].Value.ToString());
                //
                try
                {
                    SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                    SqlCeCommand komut = new SqlCeCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    komut.Connection = baglanti;
                    komut.CommandText = "delete from HotspotTBL where kullaniciAdi='" + kulAdiAl + "' and serverId='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "' ";
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
                        mikrotik.Send("=.id=" + kulAdiAl + "", true);
                    }
                    MessageBox.Show("Kullanıcı Silindi");
                    
                    button11.Enabled = false;
                    button12.Enabled = false;
                }
                catch (Exception hata)
                {
                    MessageBox.Show("İşlem hatası" + hata.Message);
                }
                GenelSayfa_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Silmek istediğiniz kullanıcıyı seçiniz","Sil",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }
        private void button19_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (listBox6.SelectedItem==null)
            {
                MessageBox.Show("Önce Server Ekleyiniz");
            }
            else
            {
                MikroCek mc = new MikroCek();
                mc.VeriAl(svIp, svKulAdi, svSifre, Convert.ToInt32(svID));
            }
            GenelSayfa_Load(sender, e);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedItem==null)
            {
                MessageBox.Show("Önce Server Ekleyiniz");
            }
            else
            {
                Random rastgele = new Random();
                string harfler = "123456789";
                string sayilar = "123456789";
                string kelime = "";
                string sayi = "";
                for (int i = 0; i < 4; i++)
                {
                    kelime += harfler[rastgele.Next(harfler.Length)];
                }
                textBox12.Text = kelime;
                for (int i = 0; i < 4; i++)
                {
                    sayi += sayilar[rastgele.Next(sayilar.Length)];
                }
                textBox11.Text = sayi;
                textBox13.Text = "24";
                button17_Click(sender, e);
            }
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            svIdGuncel = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            try
            {
                SqlCeCommandBuilder cmdBldr = new SqlCeCommandBuilder(da); //güncelleme
                cmdBldr.GetUpdateCommand();
                da.Update(dt);

                serverVeriCek();
                using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                {
                    connection.Open(svIp, svKulAdi, svSifre);

                    var updateCmd = connection.CreateCommandAndParameters("/ip/hotspot/user/set", "password", dataGridView1.CurrentRow.Cells[3].Value.ToString(), TikSpecialProperties.Id, dataGridView1.CurrentRow.Cells[2].Value.ToString());
                    updateCmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Gerekli alanları doğru şekilde giriniz");
            }
        }

        private void GenelSayfa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {
                button18_Click(sender, e);
            }
            if (e.KeyCode==Keys.Enter)
            {
                dataGridView1.Focus();
            }
        }
        private void GenelSayfa_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            Application.Exit();
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SqlCeCommand cmd = new SqlCeCommand("Select yoneticiAdi,kullaniciAdi,sifre,email,tarih,telNo from ArsivTBL", baglanti);
            da = new SqlCeDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            dataGridView2.Columns[0].HeaderCell.Value = "Yönetici Adı";
            dataGridView2.Columns[1].HeaderCell.Value = "Kullanıcı Adı";
            dataGridView2.Columns[2].HeaderCell.Value = "Şifre";
            dataGridView2.Columns[3].HeaderCell.Value = "E-Posta";
            dataGridView2.Columns[4].HeaderCell.Value = "Tarih";
            dataGridView2.Columns[5].HeaderCell.Value = "Telefon No";
            dataGridView2.CurrentCell = null;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            ArsivEkle aEkle = new ArsivEkle();
            aEkle.excele_aktar(dataGridView2);
        }

        /// <summary>
        ///                       Versiyon Kontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void verKontrol_Click(object sender, EventArgs e)
        {
            UpdateEdilsinMi();
        }
        private void UpdateEdilsinMi()
        {
            if (UpdateKontrol())
            {

                DialogResult cevap = MessageBox.Show("Güncellemek İster misiniz?", "Güncelleme Bulundu!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (cevap==DialogResult.Yes)
                {
                    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(updateEt));
                    thread.Start();
                }
            }
        }
        public static void updateEt()
        {
            Application.Run(new updateProgress());
        }
        private Boolean UpdateKontrol()
        {
            Boolean durum;
            try
            {
                WebClient webclient = new WebClient();
                Stream webstream = webclient.OpenRead("https://www.hmsotel.com/hotspot/versiyonKontrol.php?versiyon=" + Program.VersiyonNo);
                StreamReader streamreader = new StreamReader(webstream);
                String donenyanit = streamreader.ReadToEnd();
                if (donenyanit== "Güncelleme Gerekli")
                {
                    durum = true;
                }
                else
                {
                    MessageBox.Show("Programınız Güncel");
                    durum = false;
                }
            }
            catch (Exception)
            {
                durum = false;
            }
            return durum;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        public class Person
        {
            public string identityNumber { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string roomName { get; set; }
        }

        private void button22_Click(object sender, EventArgs e) //json verisi okuma
        {
            try
            {
                var client = new RestClient("https://pro.hms.gen.tr/api/v1/inhotels");
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("apiaccess", label46.Text);
                request.AddHeader("apikey", "gBntSyYTyciViWbiF/t3kr9lMI6E+o5g8ghsWanm3Vg=");
                request.AddHeader("apisecret", "8019ed4b2613c9c8958966e217fb7792bf65308e84fa8e60f2086c59a9e6dd58");
                IRestResponse response = client.Execute(request);
                textBox15.Text = response.Content;

                File.WriteAllText(Application.StartupPath + "\\User.json", textBox15.Text);

                using (System.IO.StreamReader _StreamReader = new System.IO.StreamReader(Application.StartupPath + "\\User.json"))
                {
                    string jsonData = _StreamReader.ReadToEnd();
                    List<Person> listPerson = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(jsonData);

                    foreach (var _Person in listPerson)
                    {
                        if (listBox6.SelectedItem == null)
                        {

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
                                cmd.CommandText = "select * from HotspotTBL H , ServerTBL S where H.serverId='" + svID + "' and H.kullaniciAdi='" + _Person.identityNumber + "'";
                                cmd.ExecuteNonQuery();
                                SqlCeDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    //
                                    SqlCeCommand komut = new SqlCeCommand();
                                    if (baglanti.State == ConnectionState.Closed)
                                    {
                                        baglanti.Open();
                                    }
                                    komut.Connection = baglanti;
                                    komut.CommandText = "update HotspotTBL set sifre=@sifre where kullaniciAdi='" + _Person.identityNumber + "' ";
                                    komut.Parameters.AddWithValue("@sifre", _Person.roomName);
                                    komut.ExecuteNonQuery();
                                    //
                                    serverVeriCek();
                                    using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                                    {
                                        connection.Open(svIp, svKulAdi, svSifre);

                                        var updateCmd = connection.CreateCommandAndParameters("/ip/hotspot/user/set", "password", _Person.roomName, TikSpecialProperties.Id, _Person.identityNumber);
                                        updateCmd.ExecuteNonQuery();
                                    }
                                    //
                                }
                                else
                                {
                                    dr.Close();//datareader i kapattık 

                                    //// mikrotik
                                    // 7 Ağustos
                                    using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                                    {
                                        connection.Open(svIp, svKulAdi, svSifre);
                                        var createCommand = connection.CreateCommandAndParameters("/ip/hotspot/user/add", "name", _Person.identityNumber, "password", _Person.roomName);
                                        createCommand.ExecuteScalar();
                                    }

                                    //KULLANICI EKLEME

                                    saatEkle = TimeSpan.FromDays(365);
                                    //gün ve saat ekleme
                                    string date = string.Format("{0:yyyy/MM/dd HH:mm:ss}", dateTimePicker4.Value.Add(saatEkle));//zamanı gün olarak arttırdık
                                    if (baglanti.State == ConnectionState.Closed)
                                        baglanti.Open();
                                    string kayit = "insert into HotspotTBL(serverId,kullaniciAdi,sifre,email,sure,telNo) values (@serverId,@kullaniciAdi,@sifre,@email,@sure,@telNo)";
                                    SqlCeCommand komut = new SqlCeCommand(kayit, baglanti);
                                    komut.Parameters.AddWithValue("@serverId", svID);
                                    komut.Parameters.AddWithValue("@kullaniciAdi", _Person.identityNumber);
                                    komut.Parameters.AddWithValue("@sifre", _Person.roomName);
                                    komut.Parameters.AddWithValue("@email", "");
                                    komut.Parameters.AddWithValue("@sure", date);
                                    komut.Parameters.AddWithValue("@telNo", "");
                                    komut.ExecuteNonQuery();
                                    baglanti.Close();

                                    //
                                }
                            }
                            catch (Exception hata)
                            {
                                MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                            }
                        }
                    }
                    textBox15.Text = "";
                }
                if (File.Exists(Application.StartupPath + "\\User.json"))    //kullandıktan sonra json dosyasını sil..
                {
                    File.Delete(Application.StartupPath + "\\User.json");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Kullanıcı Verileri Alınamadı. Otel Kodunuzu Giriniz");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void kullSifirlaBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            KullaniciSil ks = new KullaniciSil();
            ks.kullaniciSifirla(dataGridView1, listBox6);
            GenelSayfa_Load(sender, e);
            MessageBox.Show("Tüm Kullanıcılar Silindi", "Sıfırla", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (listBox7.SelectedItem==null)
            {
                MessageBox.Show("Önce Server Ekleyiniz", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox16.Text = "";
            }
            else
            {
                string otelKodu = textBox16.Text;
                textBox16.Text = "";
                ///
                try
                {
                    //
                    MikroCek mc = new MikroCek();
                    mc.VeriAl(svIp, svKulAdi, svSifre, Convert.ToInt32(svID));
                    //
                    var client = new RestClient("https://pro.hms.gen.tr/api/v1/inhotels");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("apiaccess", otelKodu);
                    request.AddHeader("apikey", "gBntSyYTyciViWbiF/t3kr9lMI6E+o5g8ghsWanm3Vg=");
                    request.AddHeader("apisecret", "8019ed4b2613c9c8958966e217fb7792bf65308e84fa8e60f2086c59a9e6dd58");
                    IRestResponse response = client.Execute(request);
                    textBox15.Text = response.Content;

                    File.WriteAllText(Application.StartupPath + "\\User.json", textBox15.Text);

                    using (System.IO.StreamReader _StreamReader = new System.IO.StreamReader(Application.StartupPath + "\\User.json"))
                    {
                        string jsonData = _StreamReader.ReadToEnd();
                        List<Person> listPerson = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(jsonData);

                        foreach (var _Person in listPerson)
                        {
                            if (listBox6.SelectedItem == null)
                            {

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
                                    cmd.CommandText = "select * from HotspotTBL H , ServerTBL S where H.serverId='" + svID + "' and H.kullaniciAdi='" + _Person.identityNumber + "'";
                                    cmd.ExecuteNonQuery();
                                    SqlCeDataReader dr = cmd.ExecuteReader();
                                    if (dr.Read())
                                    {
                                        //
                                        SqlCeCommand komut = new SqlCeCommand();
                                        if (baglanti.State == ConnectionState.Closed)
                                        {
                                            baglanti.Open();
                                        }
                                        komut.Connection = baglanti;
                                        komut.CommandText = "update HotspotTBL set sifre=@sifre where kullaniciAdi='" + _Person.identityNumber + "' ";
                                        komut.Parameters.AddWithValue("@sifre", _Person.roomName);
                                        komut.ExecuteNonQuery();
                                        //
                                        serverVeriCek();
                                        using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                                        {
                                            connection.Open(svIp, svKulAdi, svSifre);

                                            var updateCmd = connection.CreateCommandAndParameters("/ip/hotspot/user/set", "password", _Person.roomName, TikSpecialProperties.Id, _Person.identityNumber);
                                            updateCmd.ExecuteNonQuery();
                                        }
                                        //
                                    }
                                    else
                                    {
                                        dr.Close();//datareader i kapattık 

                                        //// mikrotik
                                        // 7 Ağustos
                                        using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                                        {
                                            connection.Open(svIp, svKulAdi, svSifre);
                                            var createCommand = connection.CreateCommandAndParameters("/ip/hotspot/user/add", "name", _Person.identityNumber, "password", _Person.roomName);
                                            createCommand.ExecuteScalar();
                                        }

                                        //KULLANICI EKLEME

                                        saatEkle = TimeSpan.FromDays(365);
                                        //gün ve saat ekleme
                                        string date = string.Format("{0:yyyy/MM/dd HH:mm:ss}", dateTimePicker4.Value.Add(saatEkle));//zamanı gün olarak arttırdık
                                        if (baglanti.State == ConnectionState.Closed)
                                            baglanti.Open();
                                        string kayit = "insert into HotspotTBL(serverId,kullaniciAdi,sifre,email,sure,telNo) values (@serverId,@kullaniciAdi,@sifre,@email,@sure,@telNo)";
                                        SqlCeCommand komut = new SqlCeCommand(kayit, baglanti);
                                        komut.Parameters.AddWithValue("@serverId", svID);
                                        komut.Parameters.AddWithValue("@kullaniciAdi", _Person.identityNumber);
                                        komut.Parameters.AddWithValue("@sifre", _Person.roomName);
                                        komut.Parameters.AddWithValue("@email", "");
                                        komut.Parameters.AddWithValue("@sure", date);
                                        komut.Parameters.AddWithValue("@telNo", "");
                                        komut.ExecuteNonQuery();
                                        baglanti.Close();
                                        //
                                    }
                                }
                                catch (Exception hata)
                                {
                                    MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                                }
                            }
                        }
                        textBox15.Text = "";
                    }
                    if (File.Exists(Application.StartupPath + "\\User.json"))    //kullandıktan sonra json dosyasını sil..
                    {
                        File.Delete(Application.StartupPath + "\\User.json");
                    }
                    Properties.Settings.Default["otelKodu"] = otelKodu;
                    label46.Text = Properties.Settings.Default["otelKodu"].ToString();
                    Properties.Settings.Default.Save();
                    timer2.Start();
                }
                catch (Exception)
                {
                    MessageBox.Show("Kullanıcı Verileri Alınamadı. Otel Kodunuzu Tekrar Giriniz");
                    label46.Text = "";
                    Properties.Settings.Default["otelKodu"] = "";
                    Properties.Settings.Default.Save();
                    timer2.Stop();
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            sayac2++;

            if (sayac2 % 173 == 0)
            {
                button25_Click(sender, e);
                GenelSayfa_Load(sender, e);
                sayac2 = 0;
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["otelKodu"] = "";
            label46.Text = "";
            GenelSayfa_Load(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("http://hmsotel.com");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start("http://hmsotel.com");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start("http://hmsotel.com");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Process.Start("http://hmsotel.com");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (listBox6.SelectedItem == null)
            {
                MessageBox.Show("Önce Server Ekleyiniz");
            }
            else
            {
                try
                {
                    //
                    MikroCek mc = new MikroCek();
                    mc.VeriAl(svIp, svKulAdi, svSifre, Convert.ToInt32(svID));
                    //
                    var client = new RestClient("https://pro.hms.gen.tr/api/v1/inhotels");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("apiaccess", label46.Text);
                    request.AddHeader("apikey", "gBntSyYTyciViWbiF/t3kr9lMI6E+o5g8ghsWanm3Vg=");
                    request.AddHeader("apisecret", "8019ed4b2613c9c8958966e217fb7792bf65308e84fa8e60f2086c59a9e6dd58");
                    IRestResponse response = client.Execute(request);
                    textBox15.Text = response.Content;

                    File.WriteAllText(Application.StartupPath + "\\User.json", textBox15.Text);

                    using (System.IO.StreamReader _StreamReader = new System.IO.StreamReader(Application.StartupPath + "\\User.json"))
                    {
                        string jsonData = _StreamReader.ReadToEnd();
                        List<Person> listPerson = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(jsonData);

                        foreach (var _Person in listPerson)
                        {
                            if (listBox6.SelectedItem == null)
                            {

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
                                    cmd.CommandText = "select * from HotspotTBL H , ServerTBL S where H.serverId='" + svID + "' and H.kullaniciAdi='" + _Person.identityNumber + "'";
                                    cmd.ExecuteNonQuery();
                                    SqlCeDataReader dr = cmd.ExecuteReader();
                                    if (dr.Read())
                                    {
                                        //
                                        SqlCeCommand komut = new SqlCeCommand();
                                        if (baglanti.State == ConnectionState.Closed)
                                        {
                                            baglanti.Open();
                                        }
                                        komut.Connection = baglanti;
                                        komut.CommandText = "update HotspotTBL set sifre=@sifre where kullaniciAdi='" + _Person.identityNumber + "' ";
                                        komut.Parameters.AddWithValue("@sifre", _Person.roomName);
                                        komut.ExecuteNonQuery();
                                        //
                                        serverVeriCek();
                                        using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                                        {
                                            connection.Open(svIp, svKulAdi, svSifre);

                                            var updateCmd = connection.CreateCommandAndParameters("/ip/hotspot/user/set", "password", _Person.roomName, TikSpecialProperties.Id, _Person.identityNumber);
                                            updateCmd.ExecuteNonQuery();
                                        }
                                        //
                                    }
                                    else
                                    {
                                        dr.Close();//datareader i kapattık 

                                        //// mikrotik
                                        // 7 Ağustos
                                        using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                                        {
                                            connection.Open(svIp, svKulAdi, svSifre);
                                            var createCommand = connection.CreateCommandAndParameters("/ip/hotspot/user/add", "name", _Person.identityNumber, "password", _Person.roomName);
                                            createCommand.ExecuteScalar();
                                        }

                                        //KULLANICI EKLEME

                                        saatEkle = TimeSpan.FromDays(365);
                                        //gün ve saat ekleme
                                        string date = string.Format("{0:yyyy/MM/dd HH:mm:ss}", dateTimePicker4.Value.Add(saatEkle));//zamanı gün olarak arttırdık
                                        if (baglanti.State == ConnectionState.Closed)
                                            baglanti.Open();
                                        string kayit = "insert into HotspotTBL(serverId,kullaniciAdi,sifre,email,sure,telNo) values (@serverId,@kullaniciAdi,@sifre,@email,@sure,@telNo)";
                                        SqlCeCommand komut = new SqlCeCommand(kayit, baglanti);
                                        komut.Parameters.AddWithValue("@serverId", svID);
                                        komut.Parameters.AddWithValue("@kullaniciAdi", _Person.identityNumber);
                                        komut.Parameters.AddWithValue("@sifre", _Person.roomName);
                                        komut.Parameters.AddWithValue("@email", "");
                                        komut.Parameters.AddWithValue("@sure", date);
                                        komut.Parameters.AddWithValue("@telNo", "");
                                        komut.ExecuteNonQuery();
                                        baglanti.Close();

                                        //
                                    }
                                }
                                catch (Exception hata)
                                {
                                    MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                                }
                            }
                        }
                        textBox15.Text = "";
                    }
                    if (File.Exists(Application.StartupPath + "\\User.json"))    //kullandıktan sonra json dosyasını sil..
                    {
                        File.Delete(Application.StartupPath + "\\User.json");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Kullanıcı Verileri Alınamadı. Otel Kodunuzu Tekrar Giriniz");
                    label46.Text = "";
                    Properties.Settings.Default["otelKodu"] = "";
                    Properties.Settings.Default.Save();
                    timer2.Stop();
                }
                //
            }
            GenelSayfa_Load(sender, e);
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox7.SelectedItem != null)
            {
                textBox5.Text = listBox7.SelectedItem.ToString();
                try
                {
                    SqlCeConnection baglanti = new SqlCeConnection(@"Data Source=Hotspot.sdf;Persist Security Info=False;");
                    SqlCeCommand komut = new SqlCeCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    komut.Connection = baglanti;
                    komut.CommandText = "select * from ServerTBL where serverAdi='" + textBox5.Text + "'";
                    komut.ExecuteNonQuery();
                    SqlCeDataReader dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        svID = dr["serverId"].ToString();
                        svIp = dr["ipAdres"].ToString();
                        svKulAdi = dr["kullaniciAdi"].ToString();
                        svSifre = dr["sifre"].ToString();
                        textBox5.ReadOnly = true;
                        button10.Enabled = true;
                        textBox8.ReadOnly = false;
                        textBox7.ReadOnly = false;
                        textBox6.ReadOnly = false;
                        saatTxt.ReadOnly = false;
                    }
                    dr.Close();
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Bir hata oluştu" + hata.Message);
                }
            }
        }
    }
}
