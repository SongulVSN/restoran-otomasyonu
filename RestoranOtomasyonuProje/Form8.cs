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
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
            LoadZRaporu();
        }
        Baglanti baglan = new Baglanti();

        private void LoadZRaporu()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ZRaporu", baglan.Conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
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
            try
            {
                DateTime raporTarihi = dateTimePicker1.Value.Date;

                SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM ZRaporu WHERE rapor_tarihi = @rapor_tarihi", baglan.Conn());
                cmdCheck.Parameters.AddWithValue("@rapor_tarihi", raporTarihi);
                int raporCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (raporCount > 0)
                {
                    MessageBox.Show("Bu tarih için zaten bir rapor oluşturulmuş.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlCommand cmdToplamSatis = new SqlCommand("SELECT SUM(toplam_tutar) FROM Satis WHERE CAST(odeme_tarihi AS DATE) = @rapor_tarihi", baglan.Conn());
                cmdToplamSatis.Parameters.AddWithValue("@rapor_tarihi", raporTarihi);
                object resultToplamSatis = cmdToplamSatis.ExecuteScalar();
                decimal toplamSatis = (resultToplamSatis != DBNull.Value) ? Convert.ToDecimal(resultToplamSatis) : 0;

                SqlCommand cmdIslemSayisi = new SqlCommand("SELECT COUNT(*) FROM Siparis WHERE CAST(siparis_zamani AS DATE) = @rapor_tarihi", baglan.Conn());
                cmdIslemSayisi.Parameters.AddWithValue("@rapor_tarihi", raporTarihi);
                int islemSayisi = Convert.ToInt32(cmdIslemSayisi.ExecuteScalar());

                SqlCommand cmdZRaporu = new SqlCommand("INSERT INTO ZRaporu (rapor_tarihi, toplam_satis, islem_sayisi) VALUES (@rapor_tarihi, @toplam_satis, @islem_sayisi)", baglan.Conn());
                cmdZRaporu.Parameters.AddWithValue("@rapor_tarihi", raporTarihi);
                cmdZRaporu.Parameters.AddWithValue("@toplam_satis", toplamSatis);
                cmdZRaporu.Parameters.AddWithValue("@islem_sayisi", islemSayisi);

                cmdZRaporu.ExecuteNonQuery();

                MessageBox.Show("Z Raporu başarıyla oluşturuldu.");
                LoadZRaporu();
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
                DateTime raporTarihi = dateTimePicker1.Value.Date;

                SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM ZRaporu WHERE rapor_tarihi = @rapor_tarihi", baglan.Conn());
                cmdCheck.Parameters.AddWithValue("@rapor_tarihi", raporTarihi);
                int raporCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (raporCount == 0)
                {
                    MessageBox.Show("Bu tarih için bir rapor bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlCommand cmdSilZRaporu = new SqlCommand("DELETE FROM ZRaporu WHERE rapor_tarihi = @rapor_tarihi", baglan.Conn());
                cmdSilZRaporu.Parameters.AddWithValue("@rapor_tarihi", raporTarihi);

                cmdSilZRaporu.ExecuteNonQuery();

                MessageBox.Show("Z Raporu başarıyla silindi.");
                LoadZRaporu();
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
            Form9 form9 = new Form9();
            form9.Show();
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
            form6.Show(); 
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
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
