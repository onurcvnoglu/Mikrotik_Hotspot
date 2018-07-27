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
using System.Data.SQLite;

namespace Hotspot_Sİstemi_V0._1
{
    public partial class GenelSayfa : Form
    {
        int sayac = 0;
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        SqlDataAdapter da;

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

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void GenelSayfa_Load(object sender, EventArgs e)
        {
            kullanSil.kullanici_Sil();
            timer1.Start();
            //SqlCommand cmd = new SqlCommand("KullaniciBilgileri", baglanti);
            //cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("Select * from HotspotTBL order by sure", baglanti);
            da = new SqlDataAdapter(cmd);
            //ds = new DataSet();
            dt = new DataTable();
            //dr.Fill(ds, "Fill");
            da.Fill(dt);
            //dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = dt;
            //isimleri değiştirme

            dataGridView1.Columns[0].HeaderCell.Value = "Server ID";
            //dataGridView1.Columns[1].HeaderCell.Value = "Kullanıcı Adı";
            dataGridView1.Columns[2].HeaderCell.Value = "Kullanıcı Adı";
            dataGridView1.Columns[3].HeaderCell.Value = "Şifre";
            //dataGridView1.Columns[4].HeaderCell.Value = "Telefon No";
            dataGridView1.Columns[5].HeaderCell.Value = "Süre";
            //dataGridView1.Columns[5].Width = 130;
            //dataGridView1.Columns[0].Width = 120;
            //dataGridView1.Columns[1].Width = 120;
            dataGridView1.CurrentCell = null;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[6].Visible = false;
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
            //
            //listBox1.Items.Clear();
            //sAyar.serverListele(listBox1);

            //if (listBox1.Items.Count > 0)
            //{
            //    listBox1.SelectedIndex = 0;
            //}
            //
            listBox2.Items.Clear();
            yoneticiDuzenle.yoneticiListele(listBox2);
            listBox2.SelectedIndex = 0;
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
            dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm:ss";

            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            //listBox1.Items.Clear();
            listBox4.Items.Clear();
            listBox3.Items.Clear();
            listBox6.Items.Clear();
            serverayar.serverListele(listBox6);
            serverayar.serverListele(listBox4);
            serverayar.serverListele(listBox3);
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

            //listBox3_SelectedIndexChanged(sender, e);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                textBox5.Text = listBox3.SelectedItem.ToString();
                try
                {
                    SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
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
                    SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                    SqlCommand cmd = new SqlCommand();
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    cmd.Connection = baglanti;
                    cmd.CommandText = "select * from HotspotTBL H , ServerTBL S where H.serverId='" + svID + "' and (H.kullaniciAdi='" + textBox8.Text + "' and H.email='" + textBox6.Text + "') or H.kullaniciAdi='" + textBox8.Text + "'";
                    cmd.ExecuteNonQuery();
                    SqlDataReader dr = cmd.ExecuteReader();
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
                            //
                            //TimeSpan gunEkle = TimeSpan.FromDays(dateTimePicker2.Value.Day/*Convert.ToInt32(textBox7.Text)*/);
                            //TimeSpan zamanEkle = gunEkle.Add(saatEkle);
                            string date = string.Format("{0:dd/MM/yyyy HH:mm:ss}", dateTimePicker2.Value.Add(saatEkle)/*DateTime.Now.Add(zamanEkle)*/);//zamanı gün olarak arttırdık
                            if (baglanti.State == ConnectionState.Closed)
                                baglanti.Open();
                            string kayit = "insert into HotspotTBL(serverId,kullaniciAdi,sifre,email,sure,telNo) values (@serverId,@kullaniciAdi,@sifre,@email,@sure,@telNo)";
                            SqlCommand komut = new SqlCommand(kayit, baglanti);
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

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem != null && listBox4.SelectedItem != null)
            {
                textBox10.Text = listBox5.SelectedItem.ToString();
                svAdiTxt.Text = listBox4.SelectedItem.ToString();
                //Kullanıcı bulma işlemi
                SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                SqlCommand komut = new SqlCommand();
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                komut.Connection = baglanti;
                komut.CommandText = "select * from HotspotTBL H , ServerTBL S where S.serverId=H.serverId and H.kullaniciAdi='" + textBox10.Text + "' and S.serverAdi='" + svAdiTxt.Text + "'";
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
            //textBox8.Text = "";
            //textBox8.ReadOnly = true;
            //textBox7.Text = "";
            //textBox7.ReadOnly = true;
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
                SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                SqlCommand komut = new SqlCommand();
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
            SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
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
                    string date = string.Format("{0:dd/MM/yyyy HH:mm:ss}", dateTimePicker3.Value.Add(saatEkle)/*Convert.ToDateTime(sure).Add(zamanEkle)*/);//zamanı gün olarak güncelle
                    //date = string.Format("{0:dd/MM/yyyy HH:mm:ss}", sure.AddHours(Convert.ToInt32(guncelSaatTxt.Text)));//zamanı saat olarak güncelle
                    SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                    SqlCommand cmd = new SqlCommand();
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
                    if (!mikrotik.Login(svKulAdi, svSifre))
                    {
                        MessageBox.Show("Bağlantı işlemi başarısız");
                        mikrotik.Close();
                        return;
                    }
                    else
                    {
                        mikrotik.Send("/ip/hotspot/user/add");
                        mikrotik.Send("=name=" + textBox10.Text + "");
                        mikrotik.Send("=password=" + kulSifreTxt.Text + "");
                        mikrotik.Send("=profile=default", true);
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
            SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
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

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.CurrentCell!=null)
            //{
            //    tabControl1.SelectedTab = tabPage5;
            //    button8_Click(sender, e);
            //    string serverAdiAl = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            //    string kulAdiAl = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            //    listBox4.SelectedItem = listBox4.GetItemText(serverAdiAl);
            //    listBox5.SelectedItem = listBox5.GetItemText(kulAdiAl);
            //}
            
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            GenelSayfa_Load(sender,e);
            tabPage5_Enter(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;

            if (sayac % 300 == 0)
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
            guncelleBtn_Click(sender, e);
        }

        private void silBtn_Click_1(object sender, EventArgs e)
        {
            silBtn_Click(sender, e);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //SERVER EKLEME
            if (textBox1.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Boş alan Bırakmayınız");
            }
            else
            {
                SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                try
                {
                    if (baglanti.State == ConnectionState.Closed)
                        baglanti.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = baglanti;
                    cmd.CommandText = "Select * from ServerTBL where serverAdi='" + textBox1.Text + "' or ipAdres='" + textBox3.Text + "'";
                    cmd.ExecuteNonQuery();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Bu alana ait Server bulunmaktadır.Farklı bir server giriniz");
                    }
                    else
                    {
                        dr.Close();
                        string kayit = "insert into ServerTBL(serverAdi,ipAdres,kullaniciAdi,sifre) values (@serverAdi,@ipAdres,@kullaniciAdi,@sifre)";
                        SqlCommand komut = new SqlCommand(kayit, baglanti);
                        komut.Parameters.AddWithValue("@serverAdi", textBox1.Text);
                        komut.Parameters.AddWithValue("@ipAdres", textBox3.Text);
                        komut.Parameters.AddWithValue("@kullaniciAdi", textBox2.Text);
                        komut.Parameters.AddWithValue("@sifre", textBox4.Text);
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("Server Kayıt İşlemi Gerçekleşti.");
                    }
                    baglanti.Close();
                }
                catch (Exception hata)
                {
                    MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell!=null)
            {
                svIdGuncel = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                try
                {
                    SqlCommandBuilder cmdBldr = new SqlCommandBuilder(da); //güncelleme
                    cmdBldr.GetUpdateCommand();
                    da.Update(dt);

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
                        mikrotik.Send("=.id=" + dataGridView1.CurrentRow.Cells[2].Value.ToString() + "", true);
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
                        mikrotik.Send("=name=" + dataGridView1.CurrentRow.Cells[2].Value.ToString() + "");
                        mikrotik.Send("=password=" + dataGridView1.CurrentRow.Cells[3].Value.ToString() + "");
                        mikrotik.Send("=profile=default", true);
                    }
                    MessageBox.Show("Kullanıcı Güncellendi");
                }
                catch (Exception)
                {
                    MessageBox.Show("Gerekli alanları doğru şekilde giriniz");
                }
                GenelSayfa_Load(sender, e);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            //if (dataGridView1.CurrentCell != null)
            //{
            //    tabControl1.SelectedTab = tabPage5;
            //    button8_Click(sender, e);
            //    string serverAdiAl = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            //    string kulAdiAl = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            //    listBox4.SelectedItem = listBox4.GetItemText(serverAdiAl);
            //    listBox5.SelectedItem = listBox5.GetItemText(kulAdiAl);
            //}
        }

        private void button16_Click(object sender, EventArgs e)
        {
            GenelSayfa_Load(sender, e);
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedItem != null)
            {
                textBox5.Text = listBox6.SelectedItem.ToString();
                try
                {
                    SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotspot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
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

        private void button17_Click(object sender, EventArgs e)
        {
            textBox8.Text = textBox12.Text;
            textBox7.Text = textBox11.Text;
            saatTxt.Text = textBox13.Text;
            dateTimePicker4.Text = dateTimePicker2.Text;
            button10_Click(sender, e);
            GenelSayfa_Load(sender, e);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string serverAdiAl = listBox6.SelectedItem.ToString();
            string kulAdiAl = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            listBox4.SelectedItem = listBox4.GetItemText(serverAdiAl);
            listBox5.SelectedItem = listBox5.GetItemText(kulAdiAl);
            button11_Click(sender, e);
            GenelSayfa_Load(sender, e);
        }
    }
}
