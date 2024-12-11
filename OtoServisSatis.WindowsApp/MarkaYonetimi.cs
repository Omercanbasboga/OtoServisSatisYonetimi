using OtoServisSatis.BL;
using OtoServisSatis.Entities;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OtoServisSatis.WindowsApp
{
    public partial class MarkaYonetimi : Form
    {
        public MarkaYonetimi()
        {
            InitializeComponent();
        }

        MarkaManager manager = new MarkaManager(); // Marka işlemleri için manager sınıfı

        void Yukle()
        {
            dgvMarkalar.DataSource = manager.GetAll(); // Tüm markaları listele
        }

        void Temizle()
        {
            lblId.Text = "0";
            txtMarkaAdi.Text = string.Empty; // Alanları sıfırla
        }

        private void dgvMarkalar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kullanıcı geçerli bir satıra tıkladıysa
            {
                lblId.Text = dgvMarkalar.Rows[e.RowIndex].Cells["Id"].Value.ToString(); // Tıklanan satırdaki Id'yi al
                txtMarkaAdi.Text = dgvMarkalar.Rows[e.RowIndex].Cells["Adi"].Value.ToString(); // Tıklanan satırdaki marka adını al
            }
        }

        private void MarkaYonetimi_Load(object sender, EventArgs e)
        {
            Yukle(); // Form yüklendiğinde markaları listele
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Marka adı boş mu kontrol ediliyor
                if (string.IsNullOrWhiteSpace(txtMarkaAdi.Text))
                {
                    MessageBox.Show("Marka adı boş olamaz! Lütfen bir marka adı giriniz.");
                    return; // İşlemi durdur
                }

                // Marka adı daha önce eklenmiş mi kontrol ediliyor
                if (manager.GetAll().Any(m => m.Adi.Equals(txtMarkaAdi.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Bu marka zaten mevcut! Lütfen başka bir marka adı giriniz.");
                    return; // İşlemi durdur
                }

                int İslemSonucu = manager.Add(
                    new Marka
                    {
                        Adi = txtMarkaAdi.Text.Trim() // Kullanıcıdan alınan marka adı (boşluklar temizlenir)
                    });

                if (İslemSonucu > 0)
                {
                    Yukle(); // Listeyi yenile
                    Temizle(); // Alanları sıfırla
                    MessageBox.Show("Marka Eklendi");
                }
            }
            catch
            {
                MessageBox.Show("Hata Oluştu! Kayıt Eklenemedi!");
            }
        }



        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblId.Text != "0") // Güncellemek için bir kayıt seçilmiş mi?
                {
                    // Yeni marka adı daha önce başka bir kayıt için mevcut mu kontrol ediliyor
                    if (manager.GetAll().Any(m => m.Adi.Equals(txtMarkaAdi.Text.Trim(), StringComparison.OrdinalIgnoreCase) && m.Id != int.Parse(lblId.Text)))
                    {
                        MessageBox.Show("Bu marka adı zaten kullanılıyor! Lütfen başka bir marka adı giriniz.");
                        return; // İşlemi durdur
                    }

                    int İslemSonucu = manager.Update(
                        new Marka
                        {
                            Id = int.Parse(lblId.Text), // Seçili ID
                            Adi = txtMarkaAdi.Text.Trim() // Güncellenen marka adı (boşluklar temizlenir)
                        });

                    if (İslemSonucu > 0)
                    {
                        Yukle(); // Listeyi yenile
                        Temizle(); // Alanları sıfırla
                        MessageBox.Show("Marka Güncellendi!");
                    }
                    else
                    {
                        MessageBox.Show("Yeni Marka Eski Marka İle Aynı Olamaz!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Hata Oluştu! Kayıt Güncellenemedi!");
            }
        }



        private void btnSil_Click(object sender, EventArgs e) // Marka silme butonu
        {
            try
            {
                if (lblId.Text != "0") // Silmek için bir kayıt seçilmiş mi?
                {
                    var marka = manager.Get(int.Parse(lblId.Text)); // Seçilen kaydı getir
                    int İslemSonucu = manager.Delete(marka);

                    if (İslemSonucu > 0)
                    {
                        Yukle(); // Listeyi yenile
                        Temizle(); // Alanları sıfırla
                        MessageBox.Show("Marka Silindi!");
                    }
                    else
                    {
                        MessageBox.Show("Hata: Silmek istediğiniz kaydı seçiniz!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Hata Oluştu! Kayıt Silinemedi!");
            }
        }
    }
}
