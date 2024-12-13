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
    public partial class Form9 : Form
    {
        Baglanti baglan = new Baglanti();
        public Form9()
        {
            InitializeComponent();
            Listele();
        }
        private void Listele()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Stok", baglan.Conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewStok.DataSource = dt;
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
                string komut = "INSERT INTO Stok (urun_ismi, mevcut_miktar, minimum_stok_miktari, tedarikci_bilgileri) VALUES (@urun_ismi, @mevcut_miktar, @minimum_stok_miktari, @tedarikci_bilgileri)";
                SqlCommand cmd = new SqlCommand(komut, baglan.Conn());

                cmd.Parameters.AddWithValue("@urun_ismi", textBox2.Text);
                cmd.Parameters.AddWithValue("@mevcut_miktar", int.Parse(textBox3.Text));
                cmd.Parameters.AddWithValue("@minimum_stok_miktari", int.Parse(textBox4.Text));
                cmd.Parameters.AddWithValue("@tedarikci_bilgileri", textBox5.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Ürün başarıyla eklendi.");
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
                string komut = "UPDATE Stok SET urun_ismi = @urun_ismi, mevcut_miktar = @mevcut_miktar, minimum_stok_miktari = @minimum_stok_miktari, tedarikci_bilgileri = @tedarikci_bilgileri WHERE urun_kodu = @urun_kodu";
                SqlCommand cmd = new SqlCommand(komut, baglan.Conn());

                cmd.Parameters.AddWithValue("@urun_kodu", int.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("@urun_ismi", textBox2.Text);
                cmd.Parameters.AddWithValue("@mevcut_miktar", int.Parse(textBox3.Text));
                cmd.Parameters.AddWithValue("@minimum_stok_miktari", int.Parse(textBox4.Text));
                cmd.Parameters.AddWithValue("@tedarikci_bilgileri", textBox5.Text);

                cmd.ExecuteNonQuery();

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

        private void button12_Click(object sender, EventArgs e)
        {
            //SİL
            try
            {
                string komut = "DELETE FROM Stok WHERE urun_kodu = @urun_kodu";
                SqlCommand cmd = new SqlCommand(komut, baglan.Conn());

                cmd.Parameters.AddWithValue("@urun_kodu", int.Parse(textBox1.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Ürün başarıyla silindi.");
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

        private void dataGridViewStok_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = dataGridViewStok.Rows[e.RowIndex].Cells["urun_kodu"].Value.ToString();
                textBox2.Text = dataGridViewStok.Rows[e.RowIndex].Cells["urun_ismi"].Value.ToString();
                textBox3.Text = dataGridViewStok.Rows[e.RowIndex].Cells["mevcut_miktar"].Value.ToString();
                textBox4.Text = dataGridViewStok.Rows[e.RowIndex].Cells["minimum_stok_miktari"].Value.ToString();
                textBox5.Text = dataGridViewStok.Rows[e.RowIndex].Cells["tedarikci_bilgileri"].Value.ToString();
            }
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
