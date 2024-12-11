using OtoServisSatis.BL;
using OtoServisSatis.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtoServisSatis.WindowsApp
{
    public partial class MarkaYonetimi : Form
    {
        public MarkaYonetimi()
        {
            InitializeComponent();
        }
        MarkaManager manager = new MarkaManager();
        void Yukle()
        {
            dgvMarkalar.DataSource = manager.GetAll();
        }
        void Temizle()
        {
            lblId.Text = "0";
            txtMarkaAdi.Text = string.Empty;
        }

        private void dgvMarkalar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Tıklanan hücrenin geçerli bir satıra ait olduğunu kontrol edin
            {
                // DataGridView'den Id ve Adi değerlerini al
                lblId.Text = dgvMarkalar.Rows[e.RowIndex].Cells["Id"].Value.ToString(); // Id kolonu
                txtMarkaAdi.Text = dgvMarkalar.Rows[e.RowIndex].Cells["Adi"].Value.ToString(); // Adi kolonu
            }
        }

        private void MarkaYonetimi_Load(object sender, EventArgs e)
        {
            Yukle();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Boş Marka Adı Kontrolü
                if (string.IsNullOrWhiteSpace(txtMarkaAdi.Text))
                {
                    MessageBox.Show("Marka adı boş olamaz! Lütfen bir marka adı giriniz.");
                    return; // İşlemi durdur
                }

                int İslemSonucu = manager.Add(
                    new Marka
                    {
                        Adi = txtMarkaAdi.Text
                    }
                );

                if (İslemSonucu > 0)
                {
                    Yukle();
                    Temizle();
                    MessageBox.Show("Marka Eklendi");
                }
            }
            catch (Exception ha)
            {
                MessageBox.Show("Hata Oluştu! Kayıt Eklenemedi!");
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {

            try
            {
                if (lblId.Text != "0")
                {
                    int İslemSonucu = manager.Update(
                                       new Marka
                                       {
                                           Id = int.Parse(lblId.Text),
                                           Adi = txtMarkaAdi.Text
                                       }
                                       );
                    if (İslemSonucu > 0)
                    {
                        Yukle();
                        Temizle();

                        MessageBox.Show("Marka Güncellendi!");
                    }
                    else MessageBox.Show("Yeni Marka Eski Marka İle Aynı Olamaz!");
                }

            }
            catch (Exception hata)
            {

                MessageBox.Show("Hata Oluştu! Kayıt Güncellenemedi!");

            }
        }

        private void txtMarkaAdi_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvMarkalar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblId.Text != "0")
                {
                    var marka = manager.Get(int.Parse(lblId.Text));
                    int İslemSonucu = manager.Delete(marka);
                    if (İslemSonucu > 0)
                    {
                        Yukle();
                        Temizle();

                        MessageBox.Show("Marka Silindi!");
                    }
                    else MessageBox.Show("Hata Silmek istediğiniz kaydı seçiniz!");

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hata Oluştu! Kayıt Silinemedi!");


            }
        }
    }
}