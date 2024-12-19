  public Form1()
  {
      InitializeComponent();
  }
  SqlConnection con = new SqlConnection(@"Data Source =DESKTOP-6263DLS\SQLEXPRESS01; Initial Catalog = Etut; Integrated Security = True");
  private void Form1_Load(object sender, EventArgs e)
  {
      // TODO: This line of code loads data into the 'etutDataSet.Ogrenci' table. You can move, or remove it, as needed.
      this.ogrenciTableAdapter.Fill(this.etutDataSet.Ogrenci);

  }

  private void button1_Click(object sender, EventArgs e)
  {
      string ad = ogrenciAdTxt.Text.Trim();    // Öğrenci adı
      string soyad = ogrenciSoyadTxt.Text.Trim();  // Öğrenci soyadı

      // Öğrenci listeleme fonksiyonunu arama kriterlerine göre çağır
      OgrenciListele(ad, soyad);
  }
  private void OgrenciListele(string ad = "", string soyad = "")
  {
      try
      {
          con.Open();
          string storedProcedureName = "OgrenciListele"; // Stored Procedure adı

          SqlCommand cmd = new SqlCommand(storedProcedureName, con);
          cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure olduğunu belirt

          cmd.Parameters.AddWithValue("@Ad", ad);
          cmd.Parameters.AddWithValue("@Soyad", soyad);

          SqlDataAdapter da = new SqlDataAdapter(cmd);
          DataTable dt = new DataTable();
          da.Fill(dt);

          if (dt.Rows.Count > 0)
          {
              dataGridView1.DataSource = dt; // DataGridView'i güncelle
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
      finally
      {
          con.Close();
      }
  }
  private void DataGridView1_SelectionChanged(object sender, EventArgs e)
  {
      if (dataGridView1.SelectedRows.Count > 0)
      {
          int ogrenciID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ogrenciID"].Value);
          DersleriGetir(ogrenciID); // Seçili öğrenciye göre dersleri getir
      }
  }

  private void NotKaydet()
  {
      // Seçilen ders kaydı ID'sini alıyoruz.
      int dersKayitID = Convert.ToInt32(comboBoxDersler.SelectedValue);

      // TextBox'lardan not bilgilerini alıyoruz. Eğer boş ise `null` bırakıyoruz.
      decimal? not1 = string.IsNullOrWhiteSpace(textBoxNot1.Text) ? (decimal?)null : Convert.ToDecimal(textBoxNot1.Text);
      decimal? not2 = string.IsNullOrWhiteSpace(textBoxNot2.Text) ? (decimal?)null : Convert.ToDecimal(textBoxNot2.Text);
      decimal? not3 = string.IsNullOrWhiteSpace(textBoxNot3.Text) ? (decimal?)null : Convert.ToDecimal(textBoxNot3.Text);

      // Veritabanı bağlantısını açıyoruz.
      
          con.Open();

          // Sorguyu yazıyoruz. Hangi notlar dolu ise sadece onları güncelleyecek.
          string query = @"
      UPDATE Sinavlar
      SET 
          sinav1_puan = CASE WHEN @Not1 IS NOT NULL THEN @Not1 ELSE sinav1_puan END,
          sinav2_puan = CASE WHEN @Not2 IS NOT NULL THEN @Not2 ELSE sinav2_puan END,
          sinav3_puan = CASE WHEN @Not3 IS NOT NULL THEN @Not3 ELSE sinav3_puan END
      WHERE kayitID = @DersKayitID;";

          // SqlCommand ile sorguyu çalıştırıyoruz.
          using (SqlCommand cmd = new SqlCommand(query, con))
          {
              // Parametreleri ekliyoruz.
              cmd.Parameters.AddWithValue("@Not1", (object)not1 ?? DBNull.Value);
              cmd.Parameters.AddWithValue("@Not2", (object)not2 ?? DBNull.Value);
              cmd.Parameters.AddWithValue("@Not3", (object)not3 ?? DBNull.Value);
              cmd.Parameters.AddWithValue("@DersKayitID", dersKayitID);

              // Sorguyu çalıştırıyoruz.
              int affectedRows = cmd.ExecuteNonQuery();
              MessageBox.Show($"{affectedRows} kayıt güncellendi.");
          
          }
  }
  private void DersleriGetir(int ogrenciID)
  {
      try
      {
          con.Open();
          string query = @"SELECT dk.kayitID, d.ders_ad
                   FROM Ders_Kayit dk
                   INNER JOIN Dersler d ON dk.dersID = d.dersID
                   WHERE dk.ogrenciID = @OgrenciID";

          SqlCommand cmd = new SqlCommand(query, con);
          cmd.Parameters.AddWithValue("@OgrenciID", ogrenciID);

          SqlDataAdapter da = new SqlDataAdapter(cmd);
          DataTable dt = new DataTable();
          da.Fill(dt);

          comboBoxDersler.DisplayMember = "ders_ad";
          comboBoxDersler.ValueMember = "kayitID";
          comboBoxDersler.DataSource = dt; // Ders listesini ComboBox'a aktar
      }
      catch (Exception ex)
      {
          MessageBox.Show("Hata: " + ex.Message);
      }
      finally
      {
          con.Close();
      }
  }

  private void button2_Click(object sender, EventArgs e)
  {
      NotKaydet();
  }
