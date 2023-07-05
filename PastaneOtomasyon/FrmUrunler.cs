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
    public partial class FrmUrunler : Form
    {
        public FrmUrunler()
        {
            InitializeComponent();
        }

        sqlBaglantisi bgl = new sqlBaglantisi();
        int id = -1;


        public void Listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM TblUrunler", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            bgl.baglanti().Close();
        }
        public void Temizle()
        {
            txtUrunAd.Text = "";
            txtSatisFiyat.Text = "";
            txtUrunStok.Text = "";
            id = -1;

        }

        private void FrmUrunler_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAd.Text;
            string urunFiyat = txtSatisFiyat.Text;
            string urunStok = txtUrunStok.Text;

            if (urunAd.Equals("") || urunFiyat.Equals("") || urunStok.Equals(""))
            {
                MessageBox.Show("Bütün alanları doldurunuz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    OleDbCommand komut = new OleDbCommand("Insert Into TblUrunler (urunAd,fiyat,stok) VALUES (@p1,@p2,@p3)", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", urunAd);
                    komut.Parameters.AddWithValue("@p2", urunFiyat);
                    komut.Parameters.AddWithValue("@p3", urunStok);
                    komut.ExecuteNonQuery();

                    MessageBox.Show("Ürün başarıyla eklendi.", "Kayıt Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bgl.baglanti().Close();
                    Listele();
                    Temizle();
                }
                catch
                {
                    MessageBox.Show("Ürün eklerken hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAd.Text;
            string urunFiyat = txtSatisFiyat.Text;
            string urunStok = txtUrunStok.Text;
            if (id == -1)
            {
                MessageBox.Show("Ürün bilgi güncelleme işlemi için bir ürün seçin...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (urunAd.Equals("") || urunFiyat.Equals("") || urunStok.Equals(""))
                {
                    MessageBox.Show("Bütün alanları doldurunuz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        OleDbCommand komut = new OleDbCommand("UPDATE TblUrunler SET urunAd=@p1,fiyat=@p2,stok=@p3 WHERE id=@p4", bgl.baglanti());
                        komut.Parameters.AddWithValue("@p1", urunAd);
                        komut.Parameters.AddWithValue("@p2", urunFiyat);
                        komut.Parameters.AddWithValue("@p3", urunStok);
                        komut.Parameters.AddWithValue("@p4", id);
                        komut.ExecuteNonQuery();

                        MessageBox.Show("Ürün başarıyla güncellendi.", "Kayıt Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bgl.baglanti().Close();
                        Listele();
                        Temizle();
                    }
                    catch
                    {
                        MessageBox.Show("Ürün güncellerken hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == -1)
                {
                    MessageBox.Show("Lütfen silinecek olan ürünü seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    OleDbCommand komut = new OleDbCommand("DELETE FROM TblUrunler WHERE id=@p1", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", id);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Ürün başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void button6_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmAnasayfa frm= new FrmAnasayfa();
            frm.Show();
            this.Hide();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int secilen = dataGridView1.SelectedCells[0].RowIndex;
                id = int.Parse(dataGridView1.Rows[secilen].Cells[0].Value.ToString());
                txtUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
                txtSatisFiyat.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
                txtUrunStok.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();

            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (id == -1)
            {
                MessageBox.Show("İçindekileri görmek için bir ürün seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                FrmIcindekiler frm = new FrmIcindekiler();
                frm.Urunid = id;
                frm.Show();
                this.Hide();
            }
            
        }
    }
}
