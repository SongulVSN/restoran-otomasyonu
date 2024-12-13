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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            LoadMasalar();
            LoadOdemeTurleri();
            Listele();
            LoadSiparisNo();
        }
        Baglanti baglan = new Baglanti();
        private void LoadMasalar()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT masa_id FROM Masalar", baglan.Conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "masa_id";
                comboBox1.ValueMember = "masa_id";
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

        private void LoadSiparisNo()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT siparis_no FROM Siparis", baglan.Conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                comboBox3.DataSource = dt;
                comboBox3.DisplayMember = "siparis_no";
                comboBox3.ValueMember = "siparis_no";
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
        private void LoadOdemeTurleri()
        {
            comboBox2.Items.AddRange(new string[] { "Nakit", "Kredi Kartı" });
        }

        private void Listele()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Satis", baglan.Conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewSatis.DataSource = dt;
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

        private void button11_Click(object sender, EventArgs e)
        {
            //EKLE 
            try
            {
                decimal toplamTutar = CalculateTotalTutar((int)comboBox3.SelectedValue);

                string komut = "INSERT INTO Satis (masa_id, odeme_turu, toplam_tutar) VALUES (@masa_id, @odeme_turu, @toplam_tutar)";
                SqlCommand cmd = new SqlCommand(komut, baglan.Conn());

                cmd.Parameters.AddWithValue("@masa_id", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@odeme_turu", comboBox2.SelectedItem);
                cmd.Parameters.AddWithValue("@toplam_tutar", toplamTutar);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Satış başarıyla eklendi.");
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
            //SİL
            try
            {
                string komut = "DELETE FROM Satis WHERE odeme_no = @odeme_no";
                SqlCommand cmd = new SqlCommand(komut, baglan.Conn());

                cmd.Parameters.AddWithValue("@odeme_no", int.Parse(textBox3.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Satış başarıyla silindi.");
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
            //GÜNCELLE
            try
            {
                decimal toplamTutar = CalculateTotalTutar((int)comboBox3.SelectedValue);

                string komut = "UPDATE Satis SET masa_id = @masa_id, odeme_turu = @odeme_turu, toplam_tutar = @toplam_tutar WHERE odeme_no = @odeme_no";
                SqlCommand cmd = new SqlCommand(komut, baglan.Conn());

                cmd.Parameters.AddWithValue("@masa_id", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@odeme_turu", comboBox2.SelectedItem);
                cmd.Parameters.AddWithValue("@toplam_tutar", toplamTutar);
                cmd.Parameters.AddWithValue("@odeme_no", int.Parse(textBox3.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Satış başarıyla güncellendi.");
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
        private decimal CalculateTotalTutar(int siparisNo)
        {
            try
            {
                string komut = "SELECT SUM(adet * fiyat) AS toplam_tutar FROM SiparisDetay WHERE siparis_no = @siparis_no";
                SqlCommand cmd = new SqlCommand(komut, baglan.Conn());
                cmd.Parameters.AddWithValue("@siparis_no", siparisNo);

                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                return 0;
            }
            finally
            {
                baglan.Conn().Close();
            }
        }
        private void dataGridViewSatis_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox3.Text = dataGridViewSatis.Rows[e.RowIndex].Cells["odeme_no"].Value.ToString();
                comboBox1.SelectedValue = dataGridViewSatis.Rows[e.RowIndex].Cells["masa_id"].Value;
                comboBox2.SelectedItem = dataGridViewSatis.Rows[e.RowIndex].Cells["odeme_turu"].Value.ToString();
                textBox1.Text = dataGridViewSatis.Rows[e.RowIndex].Cells["toplam_tutar"].Value.ToString();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
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
            form6.Show(); this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show(); this.Hide();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show(); this.Hide();
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

        private void label7_Click(object sender, EventArgs e)
        {
            //x
            DialogResult result = MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Onaylama Mesajı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
