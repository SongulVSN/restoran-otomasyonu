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
    public partial class Form1 : Form
    {
        Baglanti baglan = new Baglanti();
        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = this.button1;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0'; // Şifre gösteriliyor
            }
            else
            {
                textBox2.PasswordChar = '*'; // Şifre gizleniyor
            }
        }
        public bool Bosluk()
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Oturum Açma Butonu
            if(Bosluk())
            {
                MessageBox.Show("Tüm alanaları doldurmalısınız!!", "Hata Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Kullanicilar WHERE kullanici_adi=@p1 and sifre=@p2 and izin=@p3", baglan.Conn());

                    cmd.Parameters.AddWithValue("@p1", textBox1.Text);
                    cmd.Parameters.AddWithValue("@p2", textBox2.Text);
                    cmd.Parameters.AddWithValue("@p3", 1);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count >= 1)
                    {
                        MessageBox.Show("Oturum başarıyla açılıdı!", "Bilgi Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form3 form3 = new Form3();
                        form3.kullanici_adi = textBox1.Text;
                      
                        form3.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Yanlış parola/kullanıcı adı veya yetkilendirilmediniz.", "Hata Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

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
        }
    }
}
