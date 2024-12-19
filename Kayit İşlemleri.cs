 public Form1()
 {
     InitializeComponent();
 }

 public void Form1_Load(object sender ,EventArgs e)
 {
     LoadDersler();
     LoadOgretmenler();
 }
 SqlConnection baglanti = new SqlConnection(@"Data Source =DESKTOP-6263DLS\SQLEXPRESS01; Initial Catalog = Etut; Integrated Security = True");
      

 private void LoadOgretmenler()
 {
     baglanti.Open();
     //SqlCommand cmd = new SqlCommand("SELECT ogretmenId, isim, soyisim FROM Ogretmenler", baglanti);
     SqlDataAdapter da = new SqlDataAdapter("SELECT OgretmenId, CONCAT(isim, ' ', soyisim) AS TamIsim FROM Ogretmenler", baglanti);
    
     // Öğretmenleri getir
     DataTable dt = new DataTable();
     da.Fill(dt);

     // ComboBox'a bağla
     comboBoxOgretmen.DataSource = null; // Eski veriyi temizle
     comboBoxOgretmen.Items.Clear();
     comboBoxOgretmen.DataSource = dt;
     comboBoxOgretmen.DisplayMember = "TamIsim"; // Görünecek değer
     comboBoxOgretmen.ValueMember = "ogretmenId"; // Seçildiğinde alınacak değer
     baglanti.Close();
 }
 private void LoadDersler()
 {
     baglanti.Open();

     SqlCommand command = new SqlCommand("SELECT dersID, ders_ad FROM Dersler", baglanti);
     SqlDataReader reader = command.ExecuteReader();

     Dictionary<string, int> dersler = new Dictionary<string, int>();

     while (reader.Read())
     {
         string dersAdi = reader["ders_ad"].ToString();
         int dersID = Convert.ToInt32(reader["dersID"]);

         // Ders adını CheckedListBox'a ekle
         checkedListBox1.Items.Add(dersAdi);

         // İlişkili ID'yi bir Dictionary veya başka bir yapıda sakla
         dersler.Add(dersAdi, dersID);
     }

     reader.Close();
     baglanti.Close();

     // Ders ID bilgisine daha sonra dersler Dictionary'sinden erişebilirsiniz
     checkedListBox1.Tag = dersler;
 }

 
 private void button1_Click(object sender, EventArgs e)
 {
     string ogrenciAd = txtAd.Text;
     string ogrenciSoyad = txtSoyad.Text;
     string tcNo = txtTcNo.Text;
     int sinif = Convert.ToInt32(txtSınıf.Text); 
     string dogumTarihitxt = txtDogumTar.Text; //trparse
     string telNo = txtTelNo.Text;
     string adres = txtAdres.Text;
     int kayitUcret = Convert.ToInt32(txtKayıtUcret.Text); 
     string kayitTarihtxt = txtKayıtTarih.Text; //tryparse
     DateTime kayitTarih;
     DateTime dogumTarihi;
     
     if(DateTime.TryParse(kayitTarihtxt, out kayitTarih) && DateTime.TryParse(dogumTarihitxt,out dogumTarihi))
     {
         baglanti.Open();

         SqlCommand command = new SqlCommand("INSERT INTO Ogrenci (Ad, Soyad, tc_no, sinif_seviye ,dogum_tarihi, tel_no ,adres ,kayit_ücret, kayit_tarihi)"+
         "VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9); SELECT SCOPE_IDENTITY();", baglanti);
         command.Parameters.AddWithValue("@p1", ogrenciAd);
         command.Parameters.AddWithValue("@p2", ogrenciSoyad);
         command.Parameters.AddWithValue("@p3", tcNo);
         command.Parameters.AddWithValue("@p4", sinif);
         command.Parameters.AddWithValue("@p5", dogumTarihi);
         command.Parameters.AddWithValue("@p6", telNo);
         command.Parameters.AddWithValue("@p7", adres);
         command.Parameters.AddWithValue("@p8", kayitUcret);
         command.Parameters.AddWithValue("@p9", kayitTarih);

         try
         {
             int ogrenciID = Convert.ToInt32(command.ExecuteScalar());

             Dictionary<string, int> dersler = (Dictionary<string, int>)checkedListBox1.Tag;


             foreach (string dersAdi in checkedListBox1.CheckedItems)
             {
                 int dersID = dersler[dersAdi];

                 Console.WriteLine($"Seçilen Ders: {dersAdi}, Ders ID: {dersID}");

                 SqlCommand dersKayitCommand = new SqlCommand("INSERT INTO Ders_Kayit (ogrenciID, dersID) VALUES (@ogrenciID, @dersID)", baglanti);
                 dersKayitCommand.Parameters.AddWithValue("@ogrenciID", ogrenciID);
                 dersKayitCommand.Parameters.AddWithValue("@dersID", dersID);

                 dersKayitCommand.ExecuteNonQuery();
             }

             SqlCommand odemePlanCommand = new SqlCommand( "INSERT INTO Odeme_Planlari (ogrenciID) " + "VALUES (@p1)",baglanti);
             odemePlanCommand.Parameters.AddWithValue("@p1", ogrenciID);
             odemePlanCommand.ExecuteNonQuery();

             MessageBox.Show("Öğrenci Kaydı Gerçekleştirildi!!!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
         catch (Exception ex)
         {
             MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }

         baglanti.Close();
     }
     else
     {
         MessageBox.Show("Lütfen geçerli bir tarih girin (ör. 2023-12-08)!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
     }
     
     
 }

 private void textBox1_TextChanged_1(object sender, EventArgs e)
 {


 }

 private void button2_Click(object sender, EventArgs e)
 {
     string ogretmenAd = txtOgretmenAd.Text;
     string ogretmenSoyad = txtOgretmenSoyad.Text;
     string brans = txtBrans.Text;
     string telNo = txtOgretmenTelNo.Text;
     string eposta = txtEPosta.Text;
     int maas = Convert.ToInt32(txtMaas.Text);

     baglanti.Open();

     SqlCommand command = new SqlCommand("INSERT INTO Ogretmenler (isim, soyisim, brans, tel_no, e_posta ,maas)" +
        "VALUES (@p1, @p2, @p3, @p4, @p5, @p6)", baglanti);
     command.Parameters.AddWithValue("@p1", ogretmenAd);
     command.Parameters.AddWithValue("@p2", ogretmenSoyad);
     command.Parameters.AddWithValue("@p3", brans);
     command.Parameters.AddWithValue("@p4", telNo);
     command.Parameters.AddWithValue("@p5", eposta);
     command.Parameters.AddWithValue("@p6", maas);
     
     try
     {
         command.ExecuteNonQuery();
         MessageBox.Show("Öğretmen Kaydı Gerçekleştirildi!!!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
     }
     catch (Exception ex)
     {
         MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
     }
     baglanti.Close();
 }

 private void label18_Click(object sender, EventArgs e)
 {

 }

 private void button3_Click(object sender, EventArgs e)
 {
     string sinifAdi = txtSinifAd.Text; // Kullanıcıdan sınıf adı alıyoruz

     // Sınıf adı boşsa hata mesajı veriyoruz
     if (string.IsNullOrWhiteSpace(sinifAdi))
     {
         MessageBox.Show("Lütfen geçerli bir sınıf adı girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
         return;
     }

     try
     {
         baglanti.Open();

         // Sınıf kaydını yapmak için SQL komutunu hazırlıyoruz
         SqlCommand command = new SqlCommand("INSERT INTO Sinif (sinif_ad) VALUES (@sinifAdi);", baglanti);
         command.Parameters.AddWithValue("@sinifAdi", sinifAdi);

         // Komut çalıştırılıyor ve sınıf ekleniyor
         command.ExecuteNonQuery();

         MessageBox.Show("Sınıf Kaydı Gerçekleştirildi!!!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
     }
     catch (Exception ex)
     {
         MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
     }
     finally
     {
         baglanti.Close();
     }
 }

 private void label23_Click(object sender, EventArgs e)
 {

 }

 private void button4_Click(object sender, EventArgs e)
 {
     string dersAdi = textBoxDersAd.Text;
     int ogretmenId = Convert.ToInt32(comboBoxOgretmen.SelectedValue);
     int kontenjan = Convert.ToInt32(textBoxKontenjan.Text);

     if (string.IsNullOrEmpty(dersAdi)|| comboBoxOgretmen.SelectedValue == null)
     {
         MessageBox.Show("Lütfen gerekli bilgileri girin!!!.");
         return;
     }
     else
     {
         baglanti.Open();
         SqlCommand cmd = new SqlCommand("INSERT INTO Dersler (ders_ad, ogretmenId, kontenjan) VALUES (@DersAd, @OgretmenId, @kontenjan)", baglanti);
         cmd.Parameters.AddWithValue("@DersAd", dersAdi);
         cmd.Parameters.AddWithValue("@OgretmenId", ogretmenId);
         cmd.Parameters.AddWithValue("@kontenjan", kontenjan);

         int result = cmd.ExecuteNonQuery();

         if (result > 0)
         {
             MessageBox.Show("Ders başarıyla kaydedildi!");
             textBoxDersAd.Clear();
         }
         else
         {
             MessageBox.Show("Kayıt başarısız.");
         }
         baglanti.Close();
     }

   private void button5_Click(object sender, EventArgs e)
 {
     using (OpenFileDialog openFileDialog = new OpenFileDialog())
     {
         openFileDialog.Filter = "Text Files (*.txt)|*.txt";
         openFileDialog.Title = "Bir Txt Dosyası Seçin";

         if (openFileDialog.ShowDialog() == DialogResult.OK)
         {
             string filePath = openFileDialog.FileName;
             ImportDataFromTxtToSql(filePath);
         }
     }


 }

 private void ImportDataFromTxtToSql(string filePath)
 {
     try
     {
         string[] lines = File.ReadAllLines(filePath);

         baglanti.Open();

         foreach (var line in lines)
         {
             string[] columns = line.Split(',');

             if (columns.Length == 10) // Satırın doğru formatta olduğundan emin olalım
             {
                 SqlCommand command = new SqlCommand("INSERT INTO Ogrenci (tc_no, ad, soyad, sinif_seviye, dogum_tarihi, tel_no, adres, kayit_tarihi, kayit_bitis_tarihi, kayit_ücret) " +
                                                    "VALUES (@tc_no, @ad, @soyad, @sinif_seviye, @dogum_tarihi, @tel_no, @adres, @kayit_tarihi, @kayit_bitis_tarihi, @kayit_ücret)", baglanti);

                 command.Parameters.AddWithValue("@tc_no", columns[0]);
                 command.Parameters.AddWithValue("@ad", columns[1]);
                 command.Parameters.AddWithValue("@soyad", columns[2]);
                 command.Parameters.AddWithValue("@sinif_seviye", columns[3]);
                 command.Parameters.AddWithValue("@dogum_tarihi", columns[4]);
                 command.Parameters.AddWithValue("@tel_no", columns[5]);
                 command.Parameters.AddWithValue("@adres", columns[6]);
                 command.Parameters.AddWithValue("@kayit_tarihi", columns[7]);
                 command.Parameters.AddWithValue("@kayit_bitis_tarihi", columns[8]);
                 command.Parameters.AddWithValue("@kayit_ücret", columns[9]);

                 command.ExecuteNonQuery();
             }
         }

         MessageBox.Show("Veriler başarıyla SQL veritabanına kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
     }
     catch (Exception ex)
     {
         MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
     }
     finally
     {
         baglanti.Close();
     }

 }


 }
