using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PastaneOtomasyon
{
    public partial class FrmMuhasebe : Form
    {
        public FrmMuhasebe()
        {
            InitializeComponent();
        }
        sqlBaglantisi bgl = new sqlBaglantisi();


        public void Listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT urunAd,stok FROM TblUrunler", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            bgl.baglanti().Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FrmAnasayfa frm=new FrmAnasayfa();
            frm.Show();
            this.Hide();
        }
        public void UrunSatisTablo()
        {

            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT u.urunAd AS UrunAdi, SUM(s.tutar) AS ToplamKasaGeliri FROM TblSatis s INNER JOIN TblUrunler u ON s.urunId = u.id GROUP BY u.urunAd", bgl.baglanti());
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            bgl.baglanti().Close();


            
        }
        public void UrunSatisGrafik()
        {
            OleDbCommand komut = new OleDbCommand("SELECT u.urunAd AS UrunAdi, SUM(s.tutar) AS ToplamKasaGeliri FROM TblSatis s INNER JOIN TblUrunler u ON s.urunId = u.id GROUP BY u.urunAd", bgl.baglanti());
            OleDbDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                chart1.Series["Urunler"].Points.AddXY(dr[0], dr[1]);
            }
        }
        private void FrmMuhasebe_Load(object sender, EventArgs e)
        {
            Listele();
            UrunSatisGrafik();
            UrunSatisTablo();
        }
    }
}
