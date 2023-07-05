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
    public partial class FrmUretim : Form
    {
        public FrmUretim()
        {
            InitializeComponent();
        }

        sqlBaglantisi bgl = new sqlBaglantisi();
        int id = -1;


        public void Listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT ur.id,u.urunAd,ur.adet,ur.baslangicTarihi,ur.bitisTarihi FROM TblUretim ur INNER JOIN TblUrunler u ON u.id=ur.urunId", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            bgl.baglanti().Close();
        }
        public void Temizle()
        {
            cbUrunler.SelectedIndex=-1;
            txtAdet.Text = "";
            txtBaslangic.Text = "";
            txtBitis.Text = "";
            id = -1;

        }
        public void urunDoldur()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("Select * From TblUrunler", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbUrunler.ValueMember = "id";
            cbUrunler.DisplayMember = "urunAd";
            cbUrunler.DataSource = dt;
            bgl.baglanti().Close();
        }

        private void FrmUretim_Load(object sender, EventArgs e)
        {
            Listele();
            Temizle();
            urunDoldur();
            Temizle();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbUrunler.SelectedIndex==-1)
            {
                MessageBox.Show("Üretilecek ürünü seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (txtAdet.Text.Equals(""))
                {
                    MessageBox.Show("Üretilecek olan ürün adedini giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                       
                        OleDbCommand komut = new OleDbCommand("Insert Into TblUretim (urunId,adet,baslangicTarihi,bitisTarihi) VALUES (@p1,@p2,@p3,@p4)", bgl.baglanti());
                        komut.Parameters.AddWithValue("@p1", cbUrunler.SelectedValue);
                        komut.Parameters.AddWithValue("@p2", txtAdet.Text);
                        komut.Parameters.AddWithValue("@p3", txtBaslangic.Text);
                        komut.Parameters.AddWithValue("@p4", txtBitis.Text);
                        komut.ExecuteNonQuery();
                        bgl.baglanti().Close();
                        OleDbCommand komut2=new OleDbCommand("UPDATE TblUrunler SET stok=stok+@q1 WHERE urunAd=@q2",bgl.baglanti());
                        komut2.Parameters.AddWithValue("@q1",txtAdet.Text);
                        komut2.Parameters.AddWithValue("@q2",cbUrunler.Text);
                        komut2.ExecuteNonQuery();

                        MessageBox.Show(cbUrunler.Text+" üretime başlandı.", "Kayıt Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bgl.baglanti().Close();
                        Listele();
                        Temizle();
                    }
                    catch
                    {
                        MessageBox.Show("Üretime başlarken hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int secilen = dataGridView1.SelectedCells[0].RowIndex;
                id = int.Parse(dataGridView1.Rows[secilen].Cells[0].Value.ToString());
                cbUrunler.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
                txtAdet.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
                txtBaslangic.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
                txtBitis.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();

            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == -1)
                {
                    MessageBox.Show("Lütfen iptal edilecek olan üretimi seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    OleDbCommand komut = new OleDbCommand("DELETE FROM TblUretim WHERE id=@p1", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", id);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Üretim başarıyla iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Listele();
                    Temizle();
                    bgl.baglanti().Close();
                }
            }
            catch
            {
                MessageBox.Show("Beklenmeyen bir hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
