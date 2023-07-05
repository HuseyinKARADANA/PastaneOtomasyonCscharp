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
    public partial class FrmSatis : Form
    {
        public FrmSatis()
        {
            InitializeComponent();
        }

        sqlBaglantisi bgl = new sqlBaglantisi();
        int id = -1;
        double fiyat = 0;
        int stok = 0;

        public void Listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT id,urunAd,stok,fiyat FROM TblUrunler", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            bgl.baglanti().Close();
        }
        public void Temizle()
        {
            lblUrunAd.Text = "------";
            lblTutar.Text = "------";
            txtAdet.Text = "";
            id = -1;
            fiyat = 0;
            stok = 0;

        }

        private void FrmSatis_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmAnasayfa frm= new FrmAnasayfa();
            frm.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int secilen = dataGridView1.SelectedCells[0].RowIndex;
                id = int.Parse(dataGridView1.Rows[secilen].Cells[0].Value.ToString());
                lblUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
                stok = int.Parse(dataGridView1.Rows[secilen].Cells[2].Value.ToString());
                fiyat =double.Parse( dataGridView1.Rows[secilen].Cells[3].Value.ToString());
                
            }
            catch { }
        }

        private void txtAdet_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblTutar.Text = (fiyat * int.Parse(txtAdet.Text)).ToString();
            }
            catch {
                lblTutar.Text = "------";
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtAdet.Text.Equals("") )
            {
                MessageBox.Show("Alınacak olan ürünün adedini giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (lblUrunAd.Text.Equals("------"))
                {
                    MessageBox.Show("Alınacak olan ürünü seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (stok < int.Parse(txtAdet.Text))
                    {
                        MessageBox.Show("Stokta yeteri kadar ürün yok.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        OleDbCommand komut = new OleDbCommand("INSERT INTO TblSatis (urunId,adet,tutar) VALUES (@p1,@p2,@p3)", bgl.baglanti());
                        komut.Parameters.AddWithValue("@p1", id);
                        komut.Parameters.AddWithValue("@p2", txtAdet.Text);
                        komut.Parameters.AddWithValue("@p3", lblTutar.Text);
                        komut.ExecuteNonQuery();
                        bgl.baglanti().Close();

                        OleDbCommand komut2 = new OleDbCommand("UPDATE TblUrunler Set stok=stok-@q1 WHERE id=@q2", bgl.baglanti());
                        komut2.Parameters.AddWithValue("@q1", txtAdet.Text);
                        komut2.Parameters.AddWithValue("@q2", id);
                        komut2.ExecuteNonQuery();
                        MessageBox.Show(lblUrunAd.Text + " ürününden " + txtAdet.Text + " tanesi/kilogramı " + lblTutar.Text + "TL ye satıldı.", "Satış Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bgl.baglanti().Close();
                        Listele();
                        Temizle();
                    }
                   
                }
            }
        }
    }
}
