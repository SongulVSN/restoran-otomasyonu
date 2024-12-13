using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestoranOtomasyonuProje
{
    public partial class Form3 : Form
    {
        Baglanti baglan = new Baglanti();

        public string kullanici_adi;
        public Form3()
        {
            InitializeComponent();
        }

        void Listele()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Kullanicilar", baglan.Conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'restoranOtomasyonDataSet1.Kullanicilar' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.kullanicilarTableAdapter.Fill(this.restoranOtomasyonDataSet1.Kullanicilar);
            // TODO: Bu kod satırı 'restoranOtomasyonDataSet.Masalar' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.masalarTableAdapter.Fill(this.restoranOtomasyonDataSet.Masalar);
            label1.Text = kullanici_adi;
            Listele();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Onaylama Mesajı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button10_Click(object sender, EventArgs e)
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

        private void button11_Click(object sender, EventArgs e)
        {
            //EKLE KOMUTU
            try
            {
                string komut = "INSERT INTO Kullanicilar (kullanici_adi, sifre, izin) VALUES (@p1, @p2, @p3)";
                SqlCommand cmd = new SqlCommand(komut, baglan.Conn());

                cmd.Parameters.AddWithValue("@p1", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@p2", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@p3", checkBox1.Checked);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Kullanıcı başarıyla eklendi.");
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
            SqlCommand cmd = new SqlCommand("DELETE FROM Kullanicilar WHERE kullanici_id=@p1", baglan.Conn());
            cmd.Parameters.AddWithValue("@p1", textBox3.Text);
            cmd.ExecuteNonQuery();
            Listele();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString();
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value?.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value?.ToString();
    
                if (dataGridView1.Rows[e.RowIndex].Cells["izinDataGridViewCheckBoxColumn"]?.Value != DBNull.Value)
                {
                    checkBox1.Checked = Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells["izinDataGridViewCheckBoxColumn"].Value);
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //GÜNCELLE BUTONU
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                try
                {
                    int userId = Convert.ToInt32(textBox3.Text);
                    string komut = "UPDATE Kullanicilar SET kullanici_adi = @p1, sifre = @p2, izin = @p3 WHERE kullanici_id = @p4";
                    using (SqlCommand cmd = new SqlCommand(komut, baglan.Conn()))
                    {
                        cmd.Parameters.AddWithValue("@p1", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@p2", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@p3", checkBox1.Checked);
                        cmd.Parameters.AddWithValue("@p4", userId);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Kullanıcı başarıyla güncellendi.");
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
                MessageBox.Show("Lütfen güncellemek istediğiniz kullanıcıyı seçin.", "Bilgi Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            this.Hide();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
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
