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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            InitializeSiparisDetaylari();
            ListeleSiparis();
            LoadMasalar();
            LoadMenu();
        }
        private void Form6_Load(object sender, EventArgs e)
        {
            InitializeSiparisDetaylari(); 
        }
        private DataTable siparisDetaylari;
        Baglanti baglan = new Baglanti();
        private void InitializeSiparisDetaylari()
        {
            siparisDetaylari = new DataTable();
            siparisDetaylari.Columns.Add("urun_kodu", typeof(int));
            siparisDetaylari.Columns.Add("isim", typeof(string));
            siparisDetaylari.Columns.Add("fiyat", typeof(decimal));
            siparisDetaylari.Columns.Add("adet", typeof(int));
            siparisDetaylari.Columns.Add("tutar", typeof(decimal), "fiyat * adet");
            dataGridViewSiparisDetay.DataSource = siparisDetaylari;

            if (!dataGridViewSiparisDetay.Columns.Contains("tutar"))
            {
                dataGridViewSiparisDetay.Columns.Add("tutar", "Tutar");
            }
        }
        private void LoadMasalar()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT masa_id FROM Masalar", baglan.Conn());
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "masa_id";
            comboBox1.ValueMember = "masa_id";
        }

        private void LoadMenu()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT urun_kodu, isim, fiyat FROM Menu", baglan.Conn());
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewMenu.DataSource = dt;
        }

        private void ListeleSiparis()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Siparis", baglan.Conn());
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewSiparis.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                int firstSiparisNo = Convert.ToInt32(dt.Rows[0]["siparis_no"]);
                ListeleSiparisDetay(firstSiparisNo);
            }
        }

        private void ListeleSiparisDetay(int siparisNo)
        {
            SqlCommand cmd = new SqlCommand("SELECT sd.siparis_no, sd.urun_kodu, sd.adet, sd.fiyat, m.isim FROM SiparisDetay sd JOIN Menu m ON sd.urun_kodu = m.urun_kodu WHERE sd.siparis_no = @siparis_no", baglan.Conn());
            cmd.Parameters.AddWithValue("@siparis_no", siparisNo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewSiparisDetay.DataSource = dt;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Siparis (masa_id, siparis_zamani, toplam_tutar) VALUES (@masa_id, @siparis_zamani, @toplam_tutar); SELECT SCOPE_IDENTITY();", baglan.Conn());
                cmd.Parameters.AddWithValue("@masa_id", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@siparis_zamani", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@toplam_tutar", decimal.Parse(textBox1.Text));
                int siparisNo = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (DataRow row in siparisDetaylari.Rows)
                {
                    SqlCommand detayCmd = new SqlCommand("INSERT INTO SiparisDetay (siparis_no, urun_kodu, adet, fiyat) VALUES (@siparis_no, @urun_kodu, @adet, @fiyat)", baglan.Conn());
                    detayCmd.Parameters.AddWithValue("@siparis_no", siparisNo);
                    detayCmd.Parameters.AddWithValue("@urun_kodu", row["urun_kodu"]);
                    detayCmd.Parameters.AddWithValue("@adet", row["adet"]);
                    detayCmd.Parameters.AddWithValue("@fiyat", row["fiyat"]);
                    detayCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Sipariş başarıyla eklendi.");
                ListeleSiparis();
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
            try
            {
                int siparisNo = int.Parse(textBox3.Text);

                SqlCommand detayCmd = new SqlCommand("DELETE FROM SiparisDetay WHERE siparis_no = @siparis_no", baglan.Conn());
                detayCmd.Parameters.AddWithValue("@siparis_no", siparisNo);
                detayCmd.ExecuteNonQuery();

                SqlCommand cmd = new SqlCommand("DELETE FROM Siparis WHERE siparis_no = @siparis_no", baglan.Conn());
                cmd.Parameters.AddWithValue("@siparis_no", siparisNo);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Sipariş ve ilgili detaylar başarıyla silindi.");
                ListeleSiparis();
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
            try
            {
               
                SqlCommand cmd = new SqlCommand("UPDATE Siparis SET masa_id = @masa_id, siparis_zamani = @siparis_zamani, toplam_tutar = @toplam_tutar WHERE siparis_no = @siparis_no", baglan.Conn());
                cmd.Parameters.AddWithValue("@masa_id", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@siparis_zamani", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@toplam_tutar", decimal.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("@siparis_no", int.Parse(textBox3.Text));
                cmd.ExecuteNonQuery();

                SqlCommand detayCmd = new SqlCommand("DELETE FROM SiparisDetay WHERE siparis_no = @siparis_no", baglan.Conn());
                detayCmd.Parameters.AddWithValue("@siparis_no", int.Parse(textBox3.Text));
                detayCmd.ExecuteNonQuery();

                foreach (DataRow row in siparisDetaylari.Rows)
                {
                    SqlCommand ekleCmd = new SqlCommand("INSERT INTO SiparisDetay (siparis_no, urun_kodu, adet, fiyat) VALUES (@siparis_no, @urun_kodu, @adet, @fiyat)", baglan.Conn());
                    ekleCmd.Parameters.AddWithValue("@siparis_no", int.Parse(textBox3.Text));
                    ekleCmd.Parameters.AddWithValue("@urun_kodu", row["urun_kodu"]);
                    ekleCmd.Parameters.AddWithValue("@adet", row["adet"]);
                    ekleCmd.Parameters.AddWithValue("@fiyat", row["fiyat"]);
                    ekleCmd.ExecuteNonQuery();
                }
                MessageBox.Show("Sipariş başarıyla güncellendi.");
                ListeleSiparis();
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

        private void button16_Click(object sender, EventArgs e)
        {
            if (dataGridViewMenu.SelectedRows.Count > 0 && numericUpDown1.Value > 0)
            {
                var row = dataGridViewMenu.SelectedRows[0];
                int urunKodu = Convert.ToInt32(row.Cells["urun_kodu"].Value);
                string isim = row.Cells["isim"].Value.ToString();
                decimal fiyat = Convert.ToDecimal(row.Cells["fiyat"].Value);
                int adet = (int)numericUpDown1.Value;

                var newRow = siparisDetaylari.NewRow();
                newRow["urun_kodu"] = urunKodu;
                newRow["isim"] = isim;
                newRow["fiyat"] = fiyat;
                newRow["adet"] = adet;
                
                siparisDetaylari.Rows.Add(newRow);

                CalculateTotal();
            }
            else
            {
                MessageBox.Show("Lütfen bir ürün seçin ve adet girin.");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (dataGridViewSiparisDetay.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridViewSiparisDetay.SelectedRows)
                {
                    try
                    {
                        int urunKodu = Convert.ToInt32(row.Cells["urun_kodu"].Value);
                        int siparisNo = Convert.ToInt32(row.Cells["siparis_no"].Value);

                        SqlCommand cmd = new SqlCommand("DELETE FROM SiparisDetay WHERE urun_kodu = @urun_kodu AND siparis_no = @siparis_no", baglan.Conn());
                        cmd.Parameters.AddWithValue("@urun_kodu", urunKodu);
                        cmd.Parameters.AddWithValue("@siparis_no", siparisNo);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Ürün başarıyla silindi.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                    finally
                    {
                        baglan.Conn().Close();
                    }

                    dataGridViewSiparisDetay.Rows.Remove(row);
                }

                CalculateTotal();
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz ürünü seçin.");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridViewSiparisDetay.SelectedRows.Count > 0)
            {
                var row = dataGridViewSiparisDetay.SelectedRows[0];
                var row2 = dataGridViewSiparis.SelectedRows[0];

                if (int.TryParse(textBox2.Text, out int urunKodu))
                {
                    int siparisNo = Convert.ToInt32(dataGridViewSiparisDetay.Rows[row.Index].Cells["siparis_no"].Value);
                    decimal fiyat = Convert.ToDecimal(dataGridViewSiparisDetay.Rows[row.Index].Cells["fiyat"].Value);
                    int adet = (int)numericUpDown1.Value;

                    dataGridViewSiparisDetay.Rows[row.Index].Cells["adet"].Value = adet;
                    dataGridViewSiparis.Rows[row.Index].Cells["tutar"].Value = fiyat * adet;

                    CalculateTotal();

                    try
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE SiparisDetay SET adet = @adet, fiyat = @fiyat WHERE siparis_no = @siparis_no AND urun_kodu = @urun_kodu", baglan.Conn());
                        cmd.Parameters.AddWithValue("@adet", adet);
                        cmd.Parameters.AddWithValue("@fiyat", fiyat);
                        cmd.Parameters.AddWithValue("@siparis_no", siparisNo);
                        cmd.Parameters.AddWithValue("@urun_kodu", urunKodu);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Ürün başarıyla güncellendi.");
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
                    MessageBox.Show("Lütfen geçerli bir ürün kodu girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz ürünü seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewSiparisDetay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridViewSiparisDetay.Rows[e.RowIndex];
                textBox2.Text = row.Cells["urun_kodu"].Value.ToString();
                numericUpDown1.Value = Convert.ToDecimal(row.Cells["adet"].Value);
            }
        }
        private void CalculateTotal()
        {
            decimal total = 0;
            foreach (DataRow row in siparisDetaylari.Rows)
            {
                total += (decimal)row["tutar"];
            }
            textBox1.Text = total.ToString("0.00");
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

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            this.Hide();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show(); this.Hide();
        }

        private void dataGridViewSiparis_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cellValue = dataGridViewSiparis.Rows[e.RowIndex].Cells["siparis_no"].Value;
                if (cellValue != null && cellValue != DBNull.Value)
                {
                    int siparisNo = Convert.ToInt32(cellValue);
                    textBox3.Text = siparisNo.ToString();
                    comboBox1.SelectedValue = dataGridViewSiparis.Rows[e.RowIndex].Cells["masa_id"].Value;
                    dateTimePicker1.Value = Convert.ToDateTime(dataGridViewSiparis.Rows[e.RowIndex].Cells["siparis_zamani"].Value);
                    textBox1.Text = dataGridViewSiparis.Rows[e.RowIndex].Cells["toplam_tutar"].Value.ToString();

                    ListeleSiparisDetay(siparisNo); 
                }
                else
                {
                    MessageBox.Show("Seçili hücrede sipariş numarası yok.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
