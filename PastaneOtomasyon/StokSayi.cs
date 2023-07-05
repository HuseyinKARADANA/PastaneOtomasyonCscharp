using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PastaneOtomasyon
{
    public partial class StokSayi : Form
    {
        public StokSayi()
        {
            InitializeComponent();
        }
        public int stok = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtSayi.Text.Equals(""))
            {
                MessageBox.Show("Geçerli bir sayı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                stok = int.Parse(txtSayi.Text);
                this.Hide();
            }
           
        }
    }
}
