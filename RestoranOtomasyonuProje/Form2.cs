using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RestoranOtomasyonuProje
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        Baglanti baglan = new Baglanti();
        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0'; 
                textBox3.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*'; 
                textBox3.PasswordChar = '*';  
            }
        }
        public bool Bosluk()
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text =="")
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
            //Kayıt olma butonu
            if (Bosluk())
            {
                MessageBox.Show("Tüm alanaları doldurmalısınız!!", "Hata Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string komut = "SELECT * FROM Kullanicilar WHERE kullanici_adi = @p1";
                    SqlCommand kullaniciadi = new SqlCommand(komut, baglan.Conn());
                    kullaniciadi.Parameters.AddWithValue("@p1", textBox1.Text.Trim());
                    SqlDataAdapter adapter = new SqlDataAdapter(kullaniciadi);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count >= 1)
                    {
                        string usern = textBox1.Text.Substring(0, 1).ToUpper() + textBox1.Text.Substring(1);
                        MessageBox.Show(usern + " kullanıcı adı alınmış. Başka bir kullanıcı adı deneyin!", "Hata Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (textBox2.Text != textBox3.Text)
                    {
                        MessageBox.Show("Şifreler uyuşmuyor!", "Hata Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string komut1 = "INSERT INTO Kullanicilar (kullanici_adi, sifre, izin) VALUES(@p1, @p2, @p3)";

                        SqlCommand cmd = new SqlCommand(komut1, baglan.Conn());

                        cmd.Parameters.AddWithValue("@p1", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@p2", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@p3", 0);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Kullanıcı başarıyla oluşturuldu", "Bilgi Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Form1 loginform = new Form1();
                        loginform.Show();
                        this.Hide();
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

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
