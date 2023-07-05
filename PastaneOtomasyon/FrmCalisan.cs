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
    public partial class FrmCalisan : Form
    {
        public FrmCalisan()
        {
            InitializeComponent();
        }
        sqlBaglantisi bgl = new sqlBaglantisi();
        int id = -1;


        public void Listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM TblCalisanlar", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            bgl.baglanti().Close();
        }
        public void Temizle()
        {
            txtAd.Text = "";
            txtAdres.Text = "";
            txtSoyad.Text = "";
            txtTC.Text = "";
            txtTel.Text = "";
            cbPozisyon.SelectedIndex = -1;
            id = -1;

        }
        private void FrmCalisan_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmAnasayfa frm = new FrmAnasayfa();
            frm.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == -1)
                {
                    MessageBox.Show("Lütfen silinecek olan çalışanı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    OleDbCommand komut = new OleDbCommand("DELETE FROM TblCalisanlar WHERE id=@p1", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", id);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Çalışan başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int secilen = dataGridView1.SelectedCells[0].RowIndex;
                id = int.Parse(dataGridView1.Rows[secilen].Cells[0].Value.ToString());
                txtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
                txtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
                txtTel.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
                cbPozisyon.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
                txtTC.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
                txtAdres.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ad = txtAd.Text;
            string soyad = txtSoyad.Text;
            string TCNo = txtTC.Text;
            string tel = txtTel.Text;
            if (ad.Equals("") || soyad.Equals("") || TCNo.Equals("") || tel.Equals(""))
            {
                MessageBox.Show("Ad-Soyad- T.C. No-Tel No alanlarını doldurunuz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    OleDbCommand komut = new OleDbCommand("Insert Into TblCalisanlar (ad,soyad,telNo,pozisyon,TCNo,adres) VALUES (@p1,@p2,@p3,@p4,@p5,@p6)", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", ad);
                    komut.Parameters.AddWithValue("@p2", soyad);
                    komut.Parameters.AddWithValue("@p3", tel);
                    komut.Parameters.AddWithValue("@p4", cbPozisyon.Text);
                    komut.Parameters.AddWithValue("@p5", TCNo);
                    komut.Parameters.AddWithValue("@p6", txtAdres.Text);
                    komut.ExecuteNonQuery();

                    MessageBox.Show("Çalışan başarıyla kaydedildi.", "Kayıt Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bgl.baglanti().Close();
                    Listele();
                    Temizle();
                }
                catch
                {
                    MessageBox.Show("Çalışan eklerken hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string ad = txtAd.Text;
            string soyad = txtSoyad.Text;
            string TCNo = txtTC.Text;
            string tel = txtTel.Text;
            if (ad.Equals("") || soyad.Equals("") || TCNo.Equals("") || tel.Equals(""))
            {
                MessageBox.Show("Ad-Soyad- T.C. No-Tel No alanlarını doldurunuz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    OleDbCommand komut = new OleDbCommand("UPDATE TblCalisanlar SET ad=@p1,soyad=@p2,telNo=@p3,pozisyon=@p4,TCNo=@p5,adres=@p6 WHERE id=@p7", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", ad);
                    komut.Parameters.AddWithValue("@p2", soyad);
                    komut.Parameters.AddWithValue("@p3", tel);
                    komut.Parameters.AddWithValue("@p4", cbPozisyon.Text);
                    komut.Parameters.AddWithValue("@p5", TCNo);
                    komut.Parameters.AddWithValue("@p6", txtAdres.Text);
                    komut.Parameters.AddWithValue("@p7", id);
                    komut.ExecuteNonQuery();

                    MessageBox.Show("Çalışan başarıyla güncellendi.", "Güncelleme Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bgl.baglanti().Close();
                    Listele();
                    Temizle();
                }
                catch
                {
                    MessageBox.Show("Çalışan güncellerken hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
