using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace girişsayfası
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        // Sabit parola değerleri
        private const string MudurParola = "mudur1234"; // Müdür için parola
        private const string OgretmenParola = "ogretmen1234"; // Öğretmen için parola

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
         
                string kullaniciAdi = textBox1.Text.Trim(); // Kullanıcı adı alanı
                string parola = textBox2.Text.Trim(); // Parola alanı

                // Kullanıcı adı ve parola kontrolü
                if (kullaniciAdi.Equals("Müdür", StringComparison.OrdinalIgnoreCase) && parola == MudurParola)
                {
                    MessageBox.Show("Müdür girişi başarılı!", "Giriş Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Müdür için yapılacak işlemleri buraya ekleyebilirsiniz
                }
                else if (kullaniciAdi.Equals("Öğretmen", StringComparison.OrdinalIgnoreCase) && parola == OgretmenParola)
                {
                    MessageBox.Show("Öğretmen girişi başarılı!", "Giriş Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Öğretmen için yapılacak işlemleri buraya ekleyebilirsiniz
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya parola hatalı.", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }
    }
}
