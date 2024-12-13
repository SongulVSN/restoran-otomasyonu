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

namespace RestoranOtomasyonuProje
{
    public partial class Form4 : Form
    {
        Baglanti baglan = new Baglanti();
        public Form4()
        {
            InitializeComponent();
        }
        void Listele()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Masalar", baglan.Conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
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
            //EKLE 
            string query = "INSERT INTO Masalar (kapasite, durum, rezervasyon_durumu) VALUES (@kapasite, @durum, @rezervasyon_durumu)";
            SqlCommand komut = new SqlCommand(query, baglan.Conn());
            komut.Parameters.AddWithValue("@kapasite", numericUpDown1.Value);
            komut.Parameters.AddWithValue("@durum", comboBox1.SelectedItem.ToString());
            komut.Parameters.AddWithValue("@rezervasyon_durumu", checkBox1.Checked);

            try
            {
                komut.ExecuteNonQuery();
                MessageBox.Show("Masa başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglan.Conn().Close();
            }
            Listele();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //SİL BUTONU

            try
            {
                int masaId = int.Parse(textBox1.Text);

                SqlCommand deleteOrderDetailsCmd = new SqlCommand("DELETE FROM SiparisDetay WHERE siparis_no IN (SELECT siparis_no FROM Siparis WHERE masa_id = @masa_id)", baglan.Conn());
                deleteOrderDetailsCmd.Parameters.AddWithValue("@masa_id", masaId);
                deleteOrderDetailsCmd.ExecuteNonQuery();

                SqlCommand deleteOrdersCmd = new SqlCommand("DELETE FROM Siparis WHERE masa_id = @masa_id", baglan.Conn());
                deleteOrdersCmd.Parameters.AddWithValue("@masa_id", masaId);
                deleteOrdersCmd.ExecuteNonQuery();

                SqlCommand deleteSalesCmd = new SqlCommand("DELETE FROM Satis WHERE masa_id = @masa_id", baglan.Conn());
                deleteSalesCmd.Parameters.AddWithValue("@masa_id", masaId);
                deleteSalesCmd.ExecuteNonQuery();

                SqlCommand deleteTableCmd = new SqlCommand("DELETE FROM Masalar WHERE masa_id = @masa_id", baglan.Conn());
                deleteTableCmd.Parameters.AddWithValue("@masa_id", masaId);
                deleteTableCmd.ExecuteNonQuery();

                MessageBox.Show("Masa ve ilgili siparişler ve satışlar başarıyla silindi.");
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

        private void button13_Click(object sender, EventArgs e)
        {
            //Güncelle

            if (dataGridView1.CurrentRow != null)
            {
                int masa_id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["masa_id"].Value);
                string connectionString = @"Data Source=SONGÜL;Initial Catalog=RestoranOtomasyon;Integrated Security=True";
                string query = "UPDATE Masalar SET kapasite = @kapasite, durum = @durum, rezervasyon_durumu = @rezervasyon_durumu WHERE masa_id = @masa_id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@masa_id", masa_id);
                        command.Parameters.AddWithValue("@kapasite", numericUpDown1.Value);
                        command.Parameters.AddWithValue("@durum", comboBox1.Text);
                        command.Parameters.AddWithValue("@rezervasyon_durumu", checkBox1.Checked);

                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Masa başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Listele();  
                        }
                        else
                        {
                            MessageBox.Show("Güncelleme işlemi başarısız oldu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek masayı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            decimal numericValue;
            if (decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), out numericValue))
            {
                numericUpDown1.Value = numericValue;
            }
            else
            {
                numericUpDown1.Value = numericUpDown1.Minimum; 
            }

            if (dataGridView1.Rows[e.RowIndex].Cells[2].Value != null)
            {
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
            else
            {
                comboBox1.SelectedIndex = -1; 
            }

       
            bool checkBoxValue;
            if (bool.TryParse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(), out checkBoxValue))
            {
                checkBox1.Checked = checkBoxValue;
            }
            else
            {
                checkBox1.Checked = false; 
            }
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

        private void button16_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            this.Hide();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show(); this.Hide();
        }
    }
}
