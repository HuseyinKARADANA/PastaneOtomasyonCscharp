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
    public partial class FrmMalzemeler : Form
    {
        public FrmMalzemeler()
        {
            InitializeComponent();
        }
        sqlBaglantisi bgl=new sqlBaglantisi();
        int id = -1;
      

        public void Listele()
        {
            DataTable dt = new DataTable();           
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM TblMalzemeler", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            bgl.baglanti().Close();
        }
        public void Temizle()
        {
            txtMalzemeAd.Text = "";
            txtMalzemeFiyat.Text = "";
            txtMalzemeStok.Text = "";
            id = -1;
            
        }
        private void button5_Click(object sender, EventArgs e)
        {
            FrmAnasayfa frm= new FrmAnasayfa();
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string malzemeAd = txtMalzemeAd.Text;
            string malzemeFiyat = txtMalzemeFiyat.Text;
            string malzemeStok= txtMalzemeStok.Text;

           if(malzemeAd.Equals("") || malzemeFiyat.Equals("") || malzemeStok.Equals(""))
            {
                MessageBox.Show("Bütün alanları doldurunuz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    OleDbCommand komut = new OleDbCommand("Insert Into TblMalzemeler (malzemeAd,malzemeFiyat,malzemeStok) VALUES (@p1,@p2,@p3)", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", malzemeAd);
                    komut.Parameters.AddWithValue("@p2", malzemeFiyat);
                    komut.Parameters.AddWithValue("@p3", malzemeStok);
                    komut.ExecuteNonQuery();
                    
                    MessageBox.Show("Malzeme başarıyla eklendi.", "Kayıt Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bgl.baglanti().Close();
                    Listele();
                    Temizle();
                }
                catch
                {
                    MessageBox.Show("Malzeme eklerken hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void FrmMalzemeler_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int secilen = dataGridView1.SelectedCells[0].RowIndex;
                id = int.Parse(dataGridView1.Rows[secilen].Cells[0].Value.ToString());
                txtMalzemeAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
                txtMalzemeFiyat.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
                txtMalzemeStok.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
              
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (id == -1)
            {
                MessageBox.Show("Stok ekleme işlemi için bir malzeme seçin...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    StokSayi frm= new StokSayi();
                    frm.ShowDialog();
                    int stokEklenecek = frm.stok;
                    if( stokEklenecek != 0)
                    {
                        OleDbCommand komut = new OleDbCommand("UPDATE TblMalzemeler SET malzemeStok=malzemeStok+@p1 WHERE id=@p2", bgl.baglanti());
                        komut.Parameters.AddWithValue("@p1", stokEklenecek);
                        komut.Parameters.AddWithValue("@p2", id);
                        komut.ExecuteNonQuery();
                        Temizle();
                        MessageBox.Show("Stok başarıyla güncellendi.", "Stok Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bgl.baglanti().Close();
                        Listele();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Beklenmeyen bir hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == -1)
                {
                    MessageBox.Show("Lütfen silinecek olan malzemeyi seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    OleDbCommand komut = new OleDbCommand("DELETE FROM TblMalzemeler WHERE id=@p1", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", id);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Malzeme başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void button4_Click(object sender, EventArgs e)
        {
            string malzemeAd = txtMalzemeAd.Text;
            string malzemeFiyat = txtMalzemeFiyat.Text;
            string malzemeStok = txtMalzemeStok.Text;
            if (id == -1)
            {
                MessageBox.Show("Malzeme bilgi güncelleme işlemi için bir malzeme seçin...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (malzemeAd.Equals("") || malzemeFiyat.Equals("") || malzemeStok.Equals(""))
                {
                    MessageBox.Show("Bütün alanları doldurunuz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        OleDbCommand komut = new OleDbCommand("UPDATE TblMalzemeler SET malzemeAd=@p1,malzemeFiyat=@p2,malzemeStok=@p3 WHERE id=@p4", bgl.baglanti());
                        komut.Parameters.AddWithValue("@p1", malzemeAd);
                        komut.Parameters.AddWithValue("@p2", malzemeFiyat);
                        komut.Parameters.AddWithValue("@p3", malzemeStok);
                        komut.Parameters.AddWithValue("@p4", id);
                        komut.ExecuteNonQuery();

                        MessageBox.Show("Malzeme bilgileri başarıyla güncellendi.", "Güncelleme Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bgl.baglanti().Close();
                        Listele();
                        Temizle();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.StackTrace, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        MessageBox.Show("Malzeme bilgilerini güncellerken hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
