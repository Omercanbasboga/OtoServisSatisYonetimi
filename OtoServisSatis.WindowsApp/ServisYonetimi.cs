using DevExpress.XtraEditors; // DevExpress mesaj kutuları için
using OtoServisSatis.BL;
using OtoServisSatis.Entities;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OtoServisSatis.WindowsApp
{
    public partial class ServisYonetimi : Form
    {
        public ServisYonetimi()
        {
            InitializeComponent();
        }

        private ServisManager manager = new ServisManager();

        // Verileri yükleme metodu
        void Yukle()
        {
            try
            {
                var servisler = manager.GetAll();
                if (servisler == null || !servisler.Any())
                {
                    XtraMessageBox.Show("Veritabanında servis kaydı bulunamadı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvServisler.DataSource = null;
                }
                else
                {
                    dgvServisler.DataSource = servisler;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Veriler yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Form alanlarını temizleme metodu
        void Temizle()
        {
            try
            {
                var nesneler = groupControl1.Controls.OfType<TextBox>();
                foreach (var item in nesneler)
                {
                    item.Clear();
                }
                lblId.Text = "0";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Temizleme sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Form yüklendiğinde çağrılan olay
        private void ServisYonetimi_Load(object sender, EventArgs e)
        {
            Yukle();
        }

        // Servis kaydetme işlemi
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAracPlaka.Text) || string.IsNullOrWhiteSpace(txtServisUcreti.Text))
                {
                    XtraMessageBox.Show("Lütfen gerekli tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var sonuc = manager.Add(new Servis
                {
                    AracPlaka = txtAracPlaka.Text,
                    AracSorunu = txtAracSorunu.Text,
                    GarantiKapsamindaMi = cbGaranti.Checked,
                    KasaTipi = txtKasaTipi.Text,
                    Marka = txtMarka.Text,
                    Model = txtModel.Text,
                    Notlar = txtNotlar.Text,
                    SaseNo = txtSaseNo.Text,
                    ServiseGelisTarihi = dtpServiseGelisTarihi.Value,
                    ServistenCikisTarihi = dtpServistenCikisTarihi.Value,
                    ServisUcreti = Convert.ToDecimal(txtServisUcreti.Text),
                    YapilanIslemler = txtYapilanIslemler.Text
                });

                if (sonuc > 0)
                {
                    Temizle();
                    Yukle();
                    XtraMessageBox.Show("Servis başarıyla kaydedildi!", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("Servis kaydedilemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Servis kaydedilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Servis güncelleme işlemi
        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblId.Text == "0")
                {
                    XtraMessageBox.Show("Lütfen güncellenecek bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var sonuc = manager.Update(new Servis
                {
                    Id = Convert.ToInt32(lblId.Text),
                    AracPlaka = txtAracPlaka.Text,
                    AracSorunu = txtAracSorunu.Text,
                    GarantiKapsamindaMi = cbGaranti.Checked,
                    KasaTipi = txtKasaTipi.Text,
                    Marka = txtMarka.Text,
                    Model = txtModel.Text,
                    Notlar = txtNotlar.Text,
                    SaseNo = txtSaseNo.Text,
                    ServiseGelisTarihi = dtpServiseGelisTarihi.Value,
                    ServistenCikisTarihi = dtpServistenCikisTarihi.Value,
                    ServisUcreti = Convert.ToDecimal(txtServisUcreti.Text),
                    YapilanIslemler = txtYapilanIslemler.Text
                });

                if (sonuc > 0)
                {
                    Temizle();
                    Yukle();
                    XtraMessageBox.Show("Servis başarıyla güncellendi!", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("Servis güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Servis güncellenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Servis silme işlemi
        private void bntSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblId.Text == "0")
                {
                    XtraMessageBox.Show("Lütfen silinecek bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = XtraMessageBox.Show("Bu kaydı silmek istediğinizden emin misiniz?",
                                                        "Silme Onayı",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    var sonuc = manager.Delete(Convert.ToInt32(lblId.Text));
                    if (sonuc > 0)
                    {
                        Temizle();
                        Yukle();
                        XtraMessageBox.Show("Servis başarıyla silindi!", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show("Servis silinemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Servis silinirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DataGridView'de bir satır seçildiğinde verileri forma doldurma
        private void dgvServisler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvServisler.CurrentRow == null || dgvServisler.CurrentRow.Cells[0].Value == null)
                {
                    XtraMessageBox.Show("Geçerli bir satır seçilmedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var servis = manager.Find(Convert.ToInt32(dgvServisler.CurrentRow.Cells[0].Value));
                if (servis != null)
                {
                    txtAracPlaka.Text = servis.AracPlaka;
                    txtAracSorunu.Text = servis.AracSorunu;
                    txtKasaTipi.Text = servis.KasaTipi;
                    txtMarka.Text = servis.Marka;
                    txtModel.Text = servis.Model;
                    txtNotlar.Text = servis.Notlar;
                    txtSaseNo.Text = servis.SaseNo;
                    txtServisUcreti.Text = servis.ServisUcreti.ToString();
                    txtYapilanIslemler.Text = servis.YapilanIslemler;
                    dtpServiseGelisTarihi.Value = servis.ServiseGelisTarihi;
                    dtpServistenCikisTarihi.Value = servis.ServistenCikisTarihi;
                    cbGaranti.Checked = servis.GarantiKapsamindaMi;
                    lblId.Text = servis.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Hata oluştu! Kayıt yüklenemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
