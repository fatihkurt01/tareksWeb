using Microsoft.Win32;
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
using TareksWebForm;

namespace TareksWeb
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.AcceptButton = btnGiris;
            kullaniciGetir();
            this.Icon = new Icon("sec.ico");

        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGiris_Click(object sender, EventArgs e)
        {

            TareksWebForm.TareksWeb getconfig = new TareksWebForm.TareksWeb();
            string connectionString = getconfig.GetConfig();
            if (connectionString.Contains("Provider"))
            {
                connectionString = connectionString.Substring(connectionString.IndexOf("Password="));
            }

            string kullaniciadi = editKullaniciAdi.Text.Trim();
            string sifre = editSifre.Text.Trim().ToUpper();

            string query = "declare @user varchar(128) declare @sifre varchar(50) set @user = '" + kullaniciadi + "'" +
                "set @sifre='" + sifre + "' select kullaniciid, kod, ad, engelle, muhasebekod," +
                "dbo.sky_kullanici_ok(@user,@sifre) as kullaniciok, email, bilgeuser from sky_kullanici where kod=@user";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                connection.Open();
                dataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0 &&
                    dataTable.Rows[0]["kullaniciok"] != DBNull.Value &&
                    Convert.ToBoolean(dataTable.Rows[0]["kullaniciok"]))
                {
                    kullaniciYaz();
                    this.Hide();
                    TareksWebForm.TareksWeb tareksWebForm = new TareksWebForm.TareksWeb();
                    tareksWebForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı. Lütfen tekrar deneyin.",
                    "Giriş Başarısız",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                }


            }

        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kullaniciGetir()
        {

            RegistryKey kullanici = Registry.CurrentUser.OpenSubKey(@"Software\Siber", false);

            if (kullanici != null)
            {
                if (kullanici.GetValue("User") is string user && !string.IsNullOrWhiteSpace(user))
                {
                    editKullaniciAdi.Text = user.Trim();
                }
                kullanici.Close();
            }


        }

        private void kullaniciYaz()
        {
            RegistryKey kullanici = Registry.CurrentUser.OpenSubKey(@"Software\Siber", true);

            if (kullanici != null)
            {

                string userToWrite = editKullaniciAdi.Text?.Trim();
                kullanici.SetValue("User", userToWrite, RegistryValueKind.String);
                kullanici.Close();

            }

        }

    }
}
