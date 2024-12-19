 public Form1()
 {
     InitializeComponent();
 }
 SqlConnection con = new SqlConnection(@"Data Source =DESKTOP-6263DLS\SQLEXPRESS01; Initial Catalog = Etut; Integrated Security = True");
 
 private void label2_Click(object sender, EventArgs e)
 {

 }
 private void OgrenciListele(string ad = "", string soyad = "")
 {
     try
     {
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


 private void OgrenciSil(int ogrenciId)
 {
     try
     {
         con.Open();

         // Öğrenciyi sil
         SqlCommand cmdDeleteOgrenci = new SqlCommand("DELETE FROM Ogrenci WHERE OgrenciID = @OgrenciID", con);
         cmdDeleteOgrenci.Parameters.AddWithValue("@OgrenciID", ogrenciId);
         int result = cmdDeleteOgrenci.ExecuteNonQuery();

         if (result > 0)
         {
             MessageBox.Show("Öğrenci ve ilgili veriler başarıyla silindi.");
             OgrenciListele();  // Listeyi güncelle
         }
         else
         {
             MessageBox.Show("Öğrenci silinemedi.");
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

 private void button1_Click(object sender, EventArgs e)
 {
     string ad = ogrenciAdTxt.Text.Trim();    // Öğrenci adı
     string soyad = ogrenciSoyadTxt.Text.Trim();  // Öğrenci soyadı

     // Öğrenci listeleme fonksiyonunu arama kriterlerine göre çağır
     OgrenciListele(ad, soyad);
 }

 private void Form1_Load_1(object sender, EventArgs e)
 {
     // TODO: This line of code loads data into the 'etutDataSet2.SilinenOdemePlanlari' table. You can move, or remove it, as needed.
     this.silinenOdemePlanlariTableAdapter.Fill(this.etutDataSet2.SilinenOdemePlanlari);
     // TODO: This line of code loads data into the 'etutDataSet1.SilinenOgrenciler' table. You can move, or remove it, as needed.
     this.silinenOgrencilerTableAdapter.Fill(this.etutDataSet1.SilinenOgrenciler);
     OgrenciListele();

 }

 private void buttonSil_Click_1(object sender, EventArgs e)
 {
     if (dataGridView1.SelectedRows.Count > 0)
     {
         // Seçilen satırdan öğrenci id'sini al
         int ogrenciId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ogrenciID"].Value);

         // Kullanıcıdan onay al
         DialogResult dialogResult = MessageBox.Show("Bu öğrenciyi silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.Yes)
         {
             OgrenciSil(ogrenciId);
         }
     }
     else
     {
         MessageBox.Show("Lütfen bir öğrenci seçin.");
     }
 }
