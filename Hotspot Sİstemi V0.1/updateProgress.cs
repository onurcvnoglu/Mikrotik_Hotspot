using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotspot_Sİstemi_V0._1
{
    public partial class updateProgress : Form
    {
        private string indirilenDosya = Application.StartupPath + "\\dosyam.exe";
        private string guncelDosya = "http://localhost/update/q.exe";
        private string dosyaAdi;
        public updateProgress()
        {
            InitializeComponent();
        }

        private void updateProgress_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            WebClient webclient = new WebClient();
            webclient.DownloadFileCompleted += new AsyncCompletedEventHandler(tamamlandi);
            webclient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webclient.DownloadFileAsync(new Uri(guncelDosya), indirilenDosya);
            string dosyaAdiUrl = guncelDosya;
            int strLength = dosyaAdiUrl.LastIndexOf(".");
            dosyaAdi = dosyaAdiUrl.Remove(0, strLength + 1);

        }
        private void ProgressChanged(object sender,DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            durum.Text = dosyaAdi + "(%" + e.ProgressPercentage.ToString() + ")";
        }
        private void tamamlandi(object sender,AsyncCompletedEventArgs e)
        {
            durum.Text = "Tamamlandı..";
            label2.Text = "Güncelleme İşlemi Tamamlandı";
            label2.ForeColor = System.Drawing.Color.Green;
            pictureBox1.Visible = true;
        }

        private void updateProgress_FormClosed(object sender, FormClosedEventArgs e)
        {
            GenelSayfa gsf = new GenelSayfa();
            gsf.Show();
        }
    }
}
