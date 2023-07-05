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
    public partial class FrmIcindekiler : Form
    {
        public FrmIcindekiler()
        {
            InitializeComponent();
        }
        public int Urunid;
        sqlBaglantisi bgl = new sqlBaglantisi();
        string secilenMalzeme="";


        public void Listele()
        {
            DataTable dt = new DataTable();
            OleDbCommand komut= new OleDbCommand("SELECT MalzemeAd,MalzemeAdet,ToplamFiyat FROM TblUrunMalzemesi WHERE UrunID=@p1", 
                   bgl.baglanti());
            komut.Parameters.AddWithValue("@p1",Urunid);
            OleDbDataAdapter da = new OleDbDataAdapter(komut);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            bgl.baglanti().Close();
        }
        public void Temizle()
        {
            txtAdet.Text = "";
            cbMalzemeler.SelectedIndex = -1;
            secilenMalzeme = "";

        }
        public void malzemeDoldur()
        {  
            OleDbDataAdapter da = new OleDbDataAdapter("Select * From TblMalzemeler", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbMalzemeler.ValueMember = "malzemeFiyat";
            cbMalzemeler.DisplayMember = "malzemeAd";
            cbMalzemeler.DataSource = dt;
            bgl.baglanti().Close();
        }
        public void bilgiGetir()
        {
            try
            {
                OleDbCommand komut = new OleDbCommand("SELECT  urunAd,fiyat,stok FROM TblUrunler WHERE id=@p1", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", Urunid);
                OleDbDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    lblUrunAd.Text = dr[0].ToString();
                    lblUrunFiyat.Text = dr[1].ToString();
                    lblUrunStok.Text = dr[2].ToString();
                }
                bgl.baglanti().Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void FrmIcindekiler_Load(object sender, EventArgs e)
        {
            Listele();
            malzemeDoldur();
            bilgiGetir();
            Temizle();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FrmUrunler frm= new FrmUrunler();
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string adet=txtAdet.Text;
            string malzeme = cbMalzemeler.Text;
            if(adet.Equals("") || malzeme.Equals(""))
            {
                MessageBox.Show("Bütün alanları doldurunuz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    double fiyat = int.Parse(adet) * Convert.ToDouble(cbMalzemeler.SelectedValue.ToString());
                    OleDbCommand komut = new OleDbCommand("Insert Into TblUrunMalzemesi (UrunID,MalzemeAd,MalzemeAdet,ToplamFiyat) VALUES (@p1,@p2,@p3,@p4)", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", Urunid);
                    komut.Parameters.AddWithValue("@p2", malzeme);
                    komut.Parameters.AddWithValue("@p3", adet);
                    komut.Parameters.AddWithValue("@p4", fiyat);
                    komut.ExecuteNonQuery();

                    MessageBox.Show("Tarif başarıyla eklendi.", "Kayıt Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bgl.baglanti().Close();
                    Listele();
                    Temizle();
                }
                catch
                {
                    MessageBox.Show("Tarif eklerken hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int secilen = dataGridView1.SelectedCells[0].RowIndex;
                secilenMalzeme = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
                cbMalzemeler.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
                txtAdet.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string adet = txtAdet.Text;
            if (secilenMalzeme.Equals(""))
            {
                MessageBox.Show("Güncellenecek ürünü seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (adet.Equals(""))
                {
                    MessageBox.Show("Güncellenecek ürünün adedini giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        double fiyat = int.Parse(adet) * Convert.ToDouble(cbMalzemeler.SelectedValue.ToString());
                        OleDbCommand komut = new OleDbCommand("UPDATE TblUrunMalzemesi SET MalzemeAdet=@p1,ToplamFiyat=@p2 WHERE UrunID=@p3 and MalzemeAd=@p4", bgl.baglanti());
                        komut.Parameters.AddWithValue("@p1", adet);
                        komut.Parameters.AddWithValue("@p2", fiyat);
                        komut.Parameters.AddWithValue("@p3", Urunid);
                        komut.Parameters.AddWithValue("@p4", secilenMalzeme);

                        komut.ExecuteNonQuery();

                        MessageBox.Show("Tarif başarıyla güncellendi.", "Kayıt Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bgl.baglanti().Close();
                        Listele();
                        Temizle();
                    }
                    catch
                    {
                        MessageBox.Show("Tarif güncellerken hata oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (secilenMalzeme.Equals(""))
                {
                    MessageBox.Show("Lütfen silinecek olan malzemeyi seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    OleDbCommand komut = new OleDbCommand("DELETE FROM TblUrunMalzemesi WHERE UrunID=@p1 and MalzemeAd=@p2", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", Urunid);
                    komut.Parameters.AddWithValue("@p2", secilenMalzeme);
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
    }
}
