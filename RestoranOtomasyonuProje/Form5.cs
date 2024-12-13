using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace RestoranOtomasyonuProje
{
    public partial class Form5 : Form
    {
        
        public Form5()
        {
            InitializeComponent();
        }

        Baglanti baglan = new Baglanti();
        private void Form5_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'restoranOtomasyonDataSet2.Menu' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.menuTableAdapter.Fill(this.restoranOtomasyonDataSet2.Menu);
            Listele();
            
        }
        void Listele()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Menu", baglan.Conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Onaylama Mesajı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ÇIKIŞ YAP
            DialogResult check = MessageBox.Show("Çıkış yapmak istediğinize emin misiniz?", "Onay Mesajı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //EKLE KOMUTU
            try
            {
                string komut = "INSERT INTO Menu (isim, fiyat, kategori, aciklama) VALUES (@p1, @p2, @p3, @p4)";
                SqlCommand cmd = new SqlCommand(komut, baglan.Conn());

                cmd.Parameters.AddWithValue("@p1", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@p2", textBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@p3", textBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@p4", textBox5.Text.Trim());

                cmd.ExecuteNonQuery();

                MessageBox.Show("Ürün başarıyla menüye eklendi!");
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglan.Conn().Close();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //SİL BUTONU
            SqlCommand cmd = new SqlCommand("DELETE FROM Menu WHERE urun_kodu=@p1", baglan.Conn());
            cmd.Parameters.AddWithValue("@p1", textBox1.Text);
            cmd.ExecuteNonQuery();
            Listele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //GÜNCELLE BUTONU
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                try
                {
                    string komut = "UPDATE Menu SET isim = @p1, fiyat = @p2, kategori = @p3, aciklama = @p4 WHERE urun_kodu = @p5";
                    using (SqlCommand cmd = new SqlCommand(komut, baglan.Conn()))
                    {
                        cmd.Parameters.AddWithValue("@p1", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@p2", textBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@p3", textBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@p4", textBox5.Text.Trim());
                        cmd.Parameters.AddWithValue("@p5", Convert.ToInt32(textBox1.Text.Trim()));

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Ürün başarıyla güncellendi.");
                    Listele();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
                    baglan.Conn().Close();
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz ürünü seçin.", "Bilgi Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
            this.Hide();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            this.Hide();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show();
            this.Hide();
        }
    }
}
    

