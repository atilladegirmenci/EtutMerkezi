    public Form1()
    {
        InitializeComponent();
    }
    SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-6263DLS\SQLEXPRESS01; Initial Catalog=Etut; Integrated Security=True");
    private void ImportTxtToSQL(string filePath, char separator = ',')
    {
        try
        {
            // Dosyayı oku
            var lines = File.ReadAllLines(filePath);

            if (lines.Length > 0)
            {
                // İlk satırdan sütun başlıklarını al
                var headers = lines[0].Split(separator);

                // SQL'e aktar
                using (var connection = new SqlConnection(baglanti.ConnectionString))
                {
                    connection.Open();

                    // Verileri ekleme komutu
                    foreach (var line in lines.Skip(1)) // İlk satır başlık olduğu için atla
                    {
                        var data = line.Split(separator).Select(d => d.Trim()).ToArray(); // Veriyi temizle

                        // Satırda beklenen veri sayısı kontrolü
                        if (data.Length != 10)
                        {
                            MessageBox.Show($"Hata: Satırda beklenenden farklı veri sayısı var. Satırdaki veri sayısı: {data.Length}. Satır: {line}", "Veri Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;  // Eksik veriye sahip satırı atla
                        }

                        // SQL INSERT komutunu oluştur
                        var insertCommand = new SqlCommand(
                            "INSERT INTO Ogrenci (tc_no, ad, soyad, sinif_seviye, dogum_tarihi, tel_no, adres, kayit_tarihi, kayit_bitis_tarihi, kayit_ücret) " +
                            "VALUES (@tc_no, @ad, @soyad, @sinif_seviye, @dogum_tarihi, @tel_no, @adres, @kayit_tarihi, @kayit_bitis_tarihi, @kayit_ücret)", connection);

                        // Parametreleri ekle
                        insertCommand.Parameters.AddWithValue("@tc_no", data[0]);
                        insertCommand.Parameters.AddWithValue("@ad", data[1]);
                        insertCommand.Parameters.AddWithValue("@soyad", data[2]);
                        insertCommand.Parameters.AddWithValue("@sinif_seviye", data[3]);
                        insertCommand.Parameters.AddWithValue("@dogum_tarihi", data[4]);
                        insertCommand.Parameters.AddWithValue("@tel_no", data[5]);
                        insertCommand.Parameters.AddWithValue("@adres", data[6]);
                        insertCommand.Parameters.AddWithValue("@kayit_tarihi", data[7]);
                        insertCommand.Parameters.AddWithValue("@kayit_bitis_tarihi", data[8]);
                        insertCommand.Parameters.AddWithValue("@kayit_ücret", data[9]);

                        // Komutu çalıştır
                        insertCommand.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Veriler başarıyla SQL tablosuna aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Dosya boş!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void ExportDataGridViewToPDF(DataGridView dgv)
    {
        try
        {
            // Kullanıcıdan PDF dosyasının kaydedileceği yeri seçmesini isteyin
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files|*.pdf";
            saveFileDialog.Title = "PDF Olarak Kaydet";
            saveFileDialog.FileName = "Veriler.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                // PDF belgesi oluşturuluyor
                Document doc = new Document(PageSize.A4);

                // PDF yazarını ayarla
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                // Belgeyi aç
                doc.Open();

                // Tabloyu oluştur
                PdfPTable table = new PdfPTable(dgv.ColumnCount);

                // Kolon başlıklarını ekle
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }

                // Veri satırlarını ekle
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow) // Yeni satırları atla
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(cell.Value?.ToString() ?? string.Empty);
                        }
                    }
                }

                // Tabloyu PDF'e ekle
                doc.Add(table);

                // PDF belgesini kapat
                doc.Close();

                // Kullanıcıya başarılı mesajı
                MessageBox.Show("PDF başarıyla oluşturuldu ve kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void Form1_Load(object sender, EventArgs e)
    {
        OgrenciListele();
        OgretmenListele();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        string ogrenciArananAd = txtArananAd.Text;
        string ogretmenArananAd = textBox1.Text;
        OgretmenListele(ogretmenArananAd);
        OgrenciListele(ogrenciArananAd);
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
        ExportDataGridViewToPDF(dataGridView1);

    }

    private void txtArananAd_TextChanged(object sender, EventArgs e)
    {

    }

    private void OgretmenListele(string ad = "")
    {
        try
        {
            string storedProcedureName = "OgretmenListele"; // Stored Procedure adı

            SqlCommand cmd = new SqlCommand(storedProcedureName, baglanti);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure olduğunu belirt

            cmd.Parameters.AddWithValue("@isim", ad);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = dt;  // DataGridView'i güncelle
            }
            else
            {
                MessageBox.Show("Aradığınız kriterlere uygun öğretmen bulunamadı.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Hata: " + ex.Message);
        }
    
}
    private void OgrenciListele(string ad = "")
    {
         try
         {
            string storedProcedureName = "OgrenciListele"; // Stored Procedure adı

            SqlCommand cmd = new SqlCommand(storedProcedureName, baglanti);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure olduğunu belirt

            cmd.Parameters.AddWithValue("@Ad", ad);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;  // DataGridView'i güncelle
            }
            else
            {
                MessageBox.Show("Aradığınız kriterlere uygun öğrenci bulunamadı.");
            }
         }
         catch (Exception ex)
         {
                MessageBox.Show("Hata: " + ex.Message);
         }
    }
    private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }

    private void button2_Click(object sender, EventArgs e)
    {
        ExportDataGridViewToPDF(dataGridView2);
    }

    private void button3_Click(object sender, EventArgs e)
    {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt";
            openFileDialog.Title = "TXT Dosyasını Seçin";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                ImportTxtToSQL(filePath);
            }
        

    }
