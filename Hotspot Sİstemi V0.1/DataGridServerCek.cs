using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;

namespace Hotspot_Sİstemi_V0._1
{
    class DataGridServerCek
    {
        //SqlConnection baglanti = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString);
        //SqlDataAdapter da;
        //DataTable dt;
        //public void DataGridServer(DataGridView dataGridView1, string HserverAdi)
        //{
        //    if (baglanti.State == ConnectionState.Closed)
        //        baglanti.Open();
        //    SqlCommand cmd = new SqlCommand("Select S.serverAdi from ServerTBL S , HotspotTBL H where S.serverId=H.serverId and H.serverId='" + HserverAdi + "'", baglanti);
        //    da = new SqlDataAdapter(cmd);
        //    dt = new DataTable();
        //    da.Fill(dt);
        //    dataGridView1.DataSource = dt;
        //}
    }
}
