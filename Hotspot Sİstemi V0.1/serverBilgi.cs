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
    public partial class serverBilgi : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = Hotspot; Integrated Security = True");
        serverEkle sEkle = new serverEkle();
        yoneticiAyar sAyar = new yoneticiAyar();
        anaGiris aGiris = new anaGiris();
        int sayac = 0;

        public serverBilgi()
        {
            InitializeComponent();
        }
        private void ServerEkleMenuStrip_Click_1(object sender, EventArgs e)
        {
            serverEkle sEkle = new serverEkle();
            sEkle.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        private void serverDuzenleMenuStrip_Click_1(object sender, EventArgs e)
        {
            serverDuzen sDuzen = new serverDuzen();
            sDuzen.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        private void yoneticiDuzenleMenuStrip_Click_1(object sender, EventArgs e)
        {
            yoneticiKayit yKayit = new yoneticiKayit();
            yKayit.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        private void kullaniciAyarMenuStrip_Click_1(object sender, EventArgs e)
        {
            sAyar.Show();
            this.Hide();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        private void cikisMenuStrip_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            aGiris.Show();

            this.Dispose();
            GC.SuppressFinalize(this);
        }

        public void serverBilgi_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("KullaniciBilgileri", baglanti);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dr = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            dr.Fill(ds, "Fill");
            dataGridView1.DataSource = ds.Tables[0];
            //isimleri değiştirme
            dataGridView1.Columns[0].HeaderCell.Value = "Server Adı";
            dataGridView1.Columns[1].HeaderCell.Value = "Kullanıcı Adı";
            dataGridView1.Columns[2].HeaderCell.Value = "Şifre";
            dataGridView1.Columns[3].HeaderCell.Value = "E-Posta";
            dataGridView1.Columns[4].HeaderCell.Value = "Telefon No";
            dataGridView1.Columns[5].HeaderCell.Value = "Süre";
            dataGridView1.Columns[5].Width = 130;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;

            if (sayac % 60 == 0)
            {
                serverBilgi_Load(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serverBilgi_Load(sender, e);
        }
    }
}
