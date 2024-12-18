using CefSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace TareksWebForm
{
    public partial class TareksWeb : Form
    {
        private bool isPageLoaded = false;

        public TareksWeb()
        {
            InitializeComponent();
            chromiumWebBrowser1.LoadingStateChanged += chromiumWebBrowser1_LoadingStateChanged;

        }

        private void btnImza_Click(object sender, EventArgs e)
        {
            if (!isPageLoaded)
            {
                chromiumWebBrowser1.LoadUrl("https://eortak.dtm.gov.tr/eortak/login/selectApplication.htm");
                isPageLoaded = false;
                while (!isPageLoaded)
                {
                }

                chromiumWebBrowser1.ExecuteScriptAsync("selectSigningDevice('Turkcell');");

                string phoneNumberScript = "document.getElementById('phoneNumber').value = '';";
                chromiumWebBrowser1.ExecuteScriptAsync(phoneNumberScript);
                //chromiumWebBrowser1.ExecuteScriptAsync("document.getElementById('saveButton').click();");


            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            dataGridView4.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView4_CellDoubleClick);

        }

        public string GetConfig()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Siber.cfg");

            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("Siber.cfg dosyası bulunamadı!");
                return null;
            }

            string[] lines = File.ReadAllLines(configFilePath);

            string mainConnectionString = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("MainConnectionString=", StringComparison.OrdinalIgnoreCase))
                {
                    mainConnectionString = line.Substring("MainConnectionString=".Length).Trim();
                    break;

                }


            }
            string mainConnectionStringDecoded = Decode64(mainConnectionString);
            return mainConnectionStringDecoded;

        }

      
        public string Decode64(string input)
        {
            var result = new StringBuilder();
            int a = 0;
            int b = 0;
            string Codes64 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/";


            foreach (char c in input)
            {
                int x = Codes64.IndexOf(c);
                if (x >= 0)
                {
                    b = b * 64 + x;
                    a += 6;

                    while (a >= 8)
                    {
                        a -= 8;
                        int temp = b >> a;
                        b %= (1 << a);
                        temp %= 256;
                        result.Append((char)temp);
                    }
                }
                else
                {
                    break; 
                }
            }

            return result.ToString();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (!isPageLoaded)
            {
                chromiumWebBrowser1.LoadUrl("https://google.com");
            }
        }


        private bool hasButtonClicked = false;
        private bool isPageRefreshed = false;


        private void chromiumWebBrowser1_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading && !isPageLoaded)
            {
                isPageLoaded = true;

                if (dataGridView5.Rows.Count > 0 && dataGridView5.CurrentRow != null)
                {
                    var selectedRowIndex = dataGridView5.CurrentRow.Index;
                    var columnIndexes = new Dictionary<string, int>
                    {
                        {"Gönderici Firma", dataGridView5.Columns["Gönderici Firma"].Index},
                        {"Sevk Ülke Ad", dataGridView5.Columns["Sevk Ülke Ad"].Index},
                        {"sevkulke",  dataGridView5.Columns["sevkulke"].Index},
                        {"Tic. Yapılan Ülke", dataGridView5.Columns["Tic. Yapılan Ülke"].Index},
                        {"ticyapilanulke", dataGridView5.Columns["ticyapilanulke"].Index},
                        {"Çıkış Ülke Ad", dataGridView5.Columns["Çıkış Ülke Ad"].Index},
                        {"cikisulke", dataGridView5.Columns["cikisulke"].Index},
                        {"Taşıma Şekli", dataGridView5.Columns["Taşıma Şekli"].Index},
                        {"tasimasekli", dataGridView5.Columns["tasimasekli"].Index},
                        {"Eşyanın Bulunduğu Yer", dataGridView5.Columns["Eşyanın Bulunduğu Yer"].Index},
                        {"Giriş Çıkış Gümrük", dataGridView5.Columns["Giriş Çıkış Gümrük"].Index},
                        {"Yük Boşaltma Gümrük", dataGridView5.Columns["Yük Boşaltma Gümrük"].Index},
                        {"Konteyner No", dataGridView5.Columns["Konteyner No"].Index},
                        {"Fatura Tarihi", dataGridView5.Columns["Fatura Tarihi"].Index},
                        {"Fatura No", dataGridView5.Columns["Fatura No"].Index},
                        {"Değer", dataGridView5.Columns["Değer"].Index},
                        {"Miktar", dataGridView5.Columns["Miktar"].Index},
                        {"Menşe Ülke", dataGridView5.Columns["Menşe Ülke"].Index},
                        {"menseulke",  dataGridView5.Columns["menseulke"].Index},
                        {"Belge Türü", dataGridView5.Columns["Belge Türü"].Index},
                        {"webvalue", dataGridView5.Columns["webvalue"].Index},




                    };

                    // Seçili satırdaki değerleri al
                    var values = new Dictionary<string, string>();
                    foreach (var column in columnIndexes)
                    {
                        values[column.Key] = dataGridView5.Rows[selectedRowIndex].Cells[column.Value].Value?.ToString() ?? string.Empty;
                    }



                    var scripts = new Dictionary<string, string>
                    {


                        //{"webkodutext", $@"var labelField = document.getElementById('form:bilgiTipi_label');
                        //                if (labelField) {{ labelField.innerText = '{values["Belge Türü"]}'; }}"},

                        //{"webKoduinput", $@"var labelinput = document.getElementById('form:bilgiTipi_input');
                        //                if (labelinput) {{
                        //                    labelinput.value = '{values["webvalue"]}';
                        //                    var bilgitipiLabel = document.getElementById('form:bilgiTipi_label');
                        //                    if (bilgitipiLabel) {{
                        //                        bilgitipiLabel.click(); 
                        //                    }}
                        //                }}"},


                    //    {
                    //        "webKoduinputclick", $@"var listItems = document.querySelectorAll('li'); var result = false; " +
                    //                   $"listItems.forEach(function(item) {{ if (item.innerText.trim() === '{values["Belge Türü"]}') {{ item.click(); result = true; }} }}); result;"},



                        { "gondericiUnvan", $@"var inputField = document.getElementById('form:gondericiUnvan');
                                        if (inputField) {{ inputField.value = '{values["Gönderici Firma"]}'; }}"},
                        {"sevkUlkeinput", $@"var labelinput = document.getElementById('form:sevkUlkesi_input');
                                        if (labelinput) {{ labelinput.value = '{values["sevkulke"]}'; }}"},
                        {"sevkUlke", $@"var labelField = document.getElementById('form:sevkUlkesi_label');
                                        if (labelField) {{ labelField.innerText = '{values["Sevk Ülke Ad"]}'; }}"},
                        {"ticyapilanulkeinput", $@"var labelinput = document.getElementById('form:ticaretUlkesi_input');
                                        if (labelinput) {{ labelinput.value = '{values["ticyapilanulke"]}'; }}"},
                        {"ticUlke", $@"var labelField = document.getElementById('form:ticaretUlkesi_label');
                                        if (labelField) {{ labelField.innerText = '{values["Tic. Yapılan Ülke"]}'; }}"},
                        {"cikisulkeinput", $@"var labelinput = document.getElementById('form:cikisUlkesi_input');
                                        if (labelinput) {{ labelinput.value = '{values["cikisulke"]}'; }}"},
                        {"cikisUlke", $@"var labelField = document.getElementById('form:cikisUlkesi_label');
                                        if (labelField) {{ labelField.innerText = '{values["Çıkış Ülke Ad"]}'; }}"},
                        {"tasimaSekli", $@"var labelField = document.getElementById('form:tasimaSekli_label');
                                        if (labelField) {{ labelField.innerText = '{values["Taşıma Şekli"]}'; }}"},
                        {"esya", $@"var inputField = document.getElementById('form:bulunduguYer');
                                        if (inputField) {{ inputField.value = '{values["Eşyanın Bulunduğu Yer"]}'; }}"},
                        {"girisCikis", $@"var labelField = document.getElementById('form:girisGumruk_label');
                                        if (labelField) {{ labelField.innerText = '{values["Giriş Çıkış Gümrük"]}'; }}"},
                        {"yukBosaltma", $@"var labelField = document.getElementById('form:bosaltmaGumruk_label');
                                        if (labelField) {{ labelField.innerText = '{values["Yük Boşaltma Gümrük"]}'; }}"},
                        {"konteynerno", $@"var inputField = document.getElementById('form:konteynerNo');
                                        if (inputField) {{ inputField.value = '{values["Konteyner No"]}'; }}"},
                        {"faturatarihi", $@"var inputField = document.getElementById('form:faturaTarihi_input');
                                        if (inputField) {{
                                            var rawDate = '{values["Fatura Tarihi"]}';
                                            var formattedDate = rawDate.split('.')[0].padStart(2, '0') + '/' +
                                                                rawDate.split('.')[1].padStart(2, '0') + '/' +
                                                                rawDate.split('.')[2].split(' ')[0];
                                            inputField.value = formattedDate; }}"},
                        {"faturano", $@"var inputField = document.getElementById('form:faturaNo');
                                        if (inputField) {{ inputField.value = '{values["Fatura No"]}'; }}"},
                        {"deger", $@"var inputField = document.getElementById('form:deger');
                                 if (inputField) {{ inputField.value = '{values["Değer"]}';}}"},
                        {"miktar", $@"var inputField = document.getElementById('form:miktar');
                                        if (inputField) {{ inputField.value = '{values["Miktar"]}'; }}"},
                        {"menseUlkeinput", $@"var labelinput = document.getElementById('form:menseUlke_input');
                                        if (labelinput) {{ labelinput.value = '{values["menseulke"]}'; }}"},

                        {"menseulke", $@"var labelField = document.getElementById('form:menseUlke_label');
                                        if (labelField) {{ labelField.innerText = '{values["Menşe Ülke"]}'; }}"},


                    };

                    scripts.Add("OptionByValue", $@"
                    function OptionByValue(elementId, optionText) {{
                        var selectElement = document.getElementById(elementId);
                        if (selectElement) {{
                            for (var i = 0; i < selectElement.options.length; i++) {{
                                if (selectElement.options[i].text === optionText) {{
                                    return selectElement.options[i].value;
                                }}
                            }}
                        }}
                        return '';
                    }}

                    var selectedValue;
                    selectedValue = OptionByValue('form:tasimaSekli_input', '{values["Taşıma Şekli"]}');
                    var labelInput = document.getElementById('form:tasimaSekli_input');
                    if (labelInput) {{
                        labelInput.value = selectedValue;
                    }}
                    selectedValue = OptionByValue('form:girisGumruk_input', '{values["Giriş Çıkış Gümrük"]}');
                    var labelInput = document.getElementById('form:girisGumruk_input');
                    if (labelInput) {{
                        labelInput.value = selectedValue;
                    }}
                    selectedValue = OptionByValue('form:bosaltmaGumruk_input', '{values["Yük Boşaltma Gümrük"]}');
                    var labelInput = document.getElementById('form:bosaltmaGumruk_input');
                    if (labelInput) {{
                        labelInput.value = selectedValue;
                    }}
                    ");

                    var esyaTanimiScript = @"
                        var esyaTanimi = '';
                        var esyaTanimiElement = document.querySelectorAll('table tbody tr:nth-of-type(2) td:nth-of-type(2) span')[0];
                        if (esyaTanimiElement) {
                            esyaTanimi = esyaTanimiElement.innerText;
                        }

                        // 'form:kaplarVeEsyaninTanimi' input alanına değeri yaz
                        var inputField = document.getElementById('form:kaplarVeEsyaninTanimi');
                        if (inputField) {
                            inputField.value = esyaTanimi;
                        }
                    ";

                    var belgeturuclick = $@"var listItems = document.querySelectorAll('li'); var result = false; " +
                       $"listItems.forEach(function(item) {{ if (item.innerText.trim() === '{values["Belge Türü"]}') {{ item.click(); result = true; }} }}); result;";

                    // JavaScript kodlarını çalıştır
                    chromiumWebBrowser1.ExecuteScriptAsync(belgeturuclick);

                    foreach (var script in scripts)
                    {
                        chromiumWebBrowser1.ExecuteScriptAsync(script.Value);

                    }
                    chromiumWebBrowser1.ExecuteScriptAsync(esyaTanimiScript);



                }


            }




        }





        private void LoadDataIntoGridView(string tareksmasterid)
        {
            string connectionString = GetConfig();
            if (connectionString.Contains("Provider"))
            {
                connectionString = connectionString.Substring(connectionString.IndexOf("Password="));
            }
            string grid1 = @"
    select newid() as rowid,
        beyannameid, kalemid, referansno, edisirano, gtip, faturano,
        faturatarih, aciklama,
        (select COUNT(*) from sdi_tareksdetay td where td.kalemid=v.kalemid and td.faturatarih=v.faturatarih 
            and td.tareksmasterid = v.tareksmasterid and td.faturano = v.faturano ) as istalepsayi,
        (select COUNT(*) from sgm_kalemdetay kd where kd.kalemid=v.kalemid and kd.faturatarih=v.faturatarih 
            and kd.faturano = v.faturano) kalemdetaysayi,
        (select (case when count(kalemid) = 0 then 0 else 1 end) from sdi_tareksdetay td where td.kalemid=v.kalemid 
            and td.faturatarih=v.faturatarih and td.faturano = v.faturano and td.tareksmasterid = v.tareksmasterid) as formeklendi
    from sdi_tareksistalep_view v
    where tareksmasterid = @TareksMasterId
    group by beyannameid, kalemid, referansno, edisirano, gtip, faturano, faturatarih, aciklama, tareksmasterid
    order by edisirano, faturatarih, aciklama";

            string grid3 = $"select [Belge Tarihi],[Belge Numarası],[referansno] as 'Referans No',[Gönderici Firma],[Sevk Ülke Ad],[Tic. Yapılan Ülke],[Çıkış Ülke Ad],[Üretici Firma],[Üretici Firma Adres]," +
                $" [Üretici Firma Telefon], [Menşe Ülke], [Taşıma Şekli], esyaninbulunduguyer as [Eşyanın Bulunduğu Yer], [Giriş Çıkış Gümrük]," +
                $"[Yük Boşaltma Gümrük], gtip as 'GTİP', [TSE], konteynerno as 'Konteyner No', [Belge Türü],[Ürün Cinsi],[Ürün Yapım Malzemesi],[Ürün Yaş Grubu],[Edi Sıra No], [Fatura Tarihi], [Fatura No], [Değer], " +
                $"[Dolar Değer], [Miktar], [Marka],[Model], [Birim Ad],[Değer Birimi], [İmal Yılı], [İthalat Amacı Tipi], [Eşya Kıymet], [KG Miktar], [Gümrük Miktar], [Muafiyet], [Ürün Adı], [Tebliğ Kapsamı Standartı],[Taşıma Senedi No], " +
                $"isnull(TRY_Convert(int,sevkulke),0) as sevkulke, isnull(TRY_Convert(int,ticyapilanulke),0) as ticyapilanulke, " +
                $"isnull(TRY_Convert(int,cikisulke),0) as cikisulke, isnull(TRY_Convert(int,tasimasekli),0) as tasimasekli, " +
                $"isnull(TRY_Convert(int,menseulke),0) as menseulke, webvalue, tesvikkapsaminda, tareksdetayid, tareksvalue,[Belge Türü Master] from sdi_tareksdetay_view where tareksmasterid = @TareksMasterId";

            string grid5 = $"select [Belge Tarihi],[Belge Numarası],[referansno] as 'Referans No',[Gönderici Firma],[Sevk Ülke Ad],[Tic. Yapılan Ülke],[Çıkış Ülke Ad],[Üretici Firma],[Üretici Firma Adres]," +
                $" [Üretici Firma Telefon], [Menşe Ülke], [Taşıma Şekli], esyaninbulunduguyer as [Eşyanın Bulunduğu Yer], [Giriş Çıkış Gümrük]," +
                $"[Yük Boşaltma Gümrük], gtip as 'GTİP', [TSE], konteynerno as 'Konteyner No', [Belge Türü],[Ürün Cinsi],[Ürün Yapım Malzemesi],[Ürün Yaş Grubu],[Edi Sıra No], [Fatura Tarihi], [Fatura No], [Değer], " +
                $"[Dolar Değer], [Miktar], [Marka],[Model], [Birim Ad],[Değer Birimi], [İmal Yılı], [İthalat Amacı Tipi], [Eşya Kıymet], [KG Miktar], [Gümrük Miktar], [Muafiyet], [Ürün Adı], [Tebliğ Kapsamı Standartı],[Taşıma Senedi No], " +
                $"isnull(TRY_Convert(int,sevkulke),0) as sevkulke, isnull(TRY_Convert(int,ticyapilanulke),0) as ticyapilanulke, " +
                $"isnull(TRY_Convert(int,cikisulke),0) as cikisulke, isnull(TRY_Convert(int,tasimasekli),0) as tasimasekli, " +
                $"isnull(TRY_Convert(int,menseulke),0) as menseulke, webvalue, tesvikkapsaminda, tareksdetayid, tareksvalue,[Belge Türü Master] from sdi_tareksdetay_view where tareksmasterid = @TareksMasterId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(grid1, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@TareksMasterId", tareksmasterid);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;

                    // tareksmasterid sütununu gizle
                    if (dataGridView1.Columns["tareksmasterid"] != null)
                    {
                        dataGridView1.Columns["tareksmasterid"].Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }

                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(grid3, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@TareksMasterId", tareksmasterid);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView3.DataSource = dataTable;

                    if (dataGridView4.Columns["tareksmasterid"] != null)
                    {
                        dataGridView4.Columns["tareksmasterid"].Visible = false;
                    }
                    if (dataGridView4.Columns["beyannameid"] != null)
                    {
                        dataGridView4.Columns["beyannameid"].Visible = false;
                    }
                    if (dataGridView4.Columns["musteriid"] != null)
                    {
                        dataGridView4.Columns["musteriid"].Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }

                // dataGridView5'e veri yükleme
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(grid5, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@TareksMasterId", tareksmasterid);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView5.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }

                label4.Text = "Toplam Kalem: " + dataGridView5.Rows.Count;
            }
        }


        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = e.RowIndex;

            if (selectedRowIndex >= 0 && selectedRowIndex < dataGridView4.Rows.Count)
            {
                string tareksmasterid = dataGridView4.Rows[selectedRowIndex].Cells["tareksmasterid"].Value.ToString();

                LoadDataIntoGridView(tareksmasterid);
                foreach (TabPage tab in tabControl1.TabPages)
                {
                    if (tab.Text == "Tareks Web")
                    {
                        tabControl1.SelectedTab = tab;
                        break;
                    }
                }
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {

            string connectionString = GetConfig();
            if (connectionString.Contains("Provider"))
            {
                connectionString = connectionString.Substring(connectionString.IndexOf("Password="));
            }


            string referansNo = textBox4.Text.Trim();
            string departmanAd = textBox5.Text.Trim();
            string musteriAd = textBox2.Text.Trim();
            string durum = string.Join(",", chkDurum.CheckedItems.Cast<string>());

            string grid4 = "select * from sdi_tareksarama_view where firmaGrup = 'UNIVERSAL'";

            if (!string.IsNullOrEmpty(referansNo))
            {
                grid4 += " AND referansno = @ReferansNo";
            }
            if (!string.IsNullOrEmpty(departmanAd))
            {
                grid4 += " AND departmanad = @DepartmanAd";
            }
            if (!string.IsNullOrEmpty(musteriAd))
            {
                grid4 += " AND musteriad LIKE @MusteriAd";
            }
            if (!string.IsNullOrEmpty(durum))
            {
                grid4 += " AND durum = @Durum";
            }

            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(grid4, connection);

                    if (!string.IsNullOrEmpty(referansNo))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("@ReferansNo", referansNo);
                    }
                    if (!string.IsNullOrEmpty(departmanAd))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("@DepartmanAd", departmanAd);
                    }
                    if (!string.IsNullOrEmpty(musteriAd))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("@MusteriAd", musteriAd + "%");
                    }
                    if (!string.IsNullOrEmpty(durum))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("@Durum", durum);
                    }

                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView4.DataSource = dataTable;

                    if (dataGridView4.Columns["tareksmasterid"] != null)
                    {
                        dataGridView4.Columns["tareksmasterid"].Visible = false;
                    }
                    if (dataGridView4.Columns["beyannameid"] != null)
                    {
                        dataGridView4.Columns["beyannameid"].Visible = false;
                    }
                    if (dataGridView4.Columns["musteriid"] != null)
                    {
                        dataGridView4.Columns["musteriid"].Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }




        private async void btnTescilOncesi_Click(object sender, EventArgs e)
        {
            string basvuru = @"
        var links = document.querySelectorAll('a');
        for (var i = 0; i < links.length; i++) {
            if (links[i].innerText.includes('BAŞVURU')) {
                links[i].click();
                break;
            }
        }";
            chromiumWebBrowser1.ExecuteScriptAsync(basvuru);


            await Task.Delay(400);

            string tescilöncesi = @"
        var links = document.querySelectorAll('a');
        for (var i = 0; i < links.length; i++) {
            if (links[i].innerText.includes('TESCİL ÖNCESİ')) {
                links[i].click();
                break;
            }
        }";
            chromiumWebBrowser1.ExecuteScriptAsync(tescilöncesi);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSorgula_Click(object sender, EventArgs e)
        {
            string sorgula = @"
        var links = document.querySelectorAll('a');
        for (var i = 0; i < links.length; i++) {
            if (links[i].innerText.includes('SORGULA')) {
                links[i].click();
                break;
            }
        }";
            chromiumWebBrowser1.ExecuteScriptAsync(sorgula);

        }


        private async void btnIthalat_Click(object sender, EventArgs e)
        {
            string sorgula = @"
        var links = document.querySelectorAll('a');
        for (var i = 0; i < links.length; i++) {
            if (links[i].innerText.includes('SORGULA')) {
                links[i].click();
                break;
            }
        }";
            chromiumWebBrowser1.ExecuteScriptAsync(sorgula);
            await Task.Delay(400);

            string ithalat = @"
        var links = document.querySelectorAll('a');
        for (var i = 0; i < links.length; i++) {
            if (links[i].innerText.includes('İTHALAT')) {
                links[i].click();
                break;
            }
        }";
            chromiumWebBrowser1.ExecuteScriptAsync(ithalat);
        }

        private async void btnListele_Click(object sender, EventArgs e)
        {
            string script = "document.getElementById('form:listeleButton').click();";
            var result = await chromiumWebBrowser1.EvaluateScriptAsync(script);
        }

        private void btnKredi_Click(object sender, EventArgs e)
        {
            string krediButtonScript = @"
        var links = document.querySelectorAll('a');btnDoldur_Click
        for (var i = 0; i < links.length; i++) {
            if (links[i].innerText.includes('KREDİ')) {
                links[i].click();
                break;
            }
        }";
            chromiumWebBrowser1.ExecuteScriptAsync(krediButtonScript);
        }


       

        private async void btnKalemEkle_Click(object sender, EventArgs e)
        {
            string script = "document.getElementById('form:yeniKalemButton').click();";
            var result = await chromiumWebBrowser1.EvaluateScriptAsync(script);
            await Task.Delay(500);

            if (dataGridView3.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView3.SelectedRows[0].Index;

                string tareksmasterid = dataGridView3.Rows[selectedRowIndex].Cells["tareksdetayid"].Value.ToString();
                string gtip = dataGridView3.Rows[selectedRowIndex].Cells["GTİP"].Value.ToString().Replace(".", "");

                listView1.View = View.Details;

                if (listView1.Columns.Count == 0)
                {
                    listView1.Columns.Add("Tareks Detay ID", -2, HorizontalAlignment.Left);
                }

                ListViewItem item = new ListViewItem(tareksmasterid);
                listView1.Items.Add(item);

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

                string gtipScript = $@"var inputField = document.getElementById('form:gtipNo');
                                if (inputField) {{ inputField.value = '{gtip}'; }}";

                await chromiumWebBrowser1.EvaluateScriptAsync(gtipScript);
            }
        }







        private async void btnDogrula_Click(object sender, EventArgs e)
        {
            string script = "document.getElementById('form:j_idt532').click();";
            var result = await chromiumWebBrowser1.EvaluateScriptAsync(script);

        }


        private bool isPageReloaded = false;
        private void btnDoldur_Click(object sender, EventArgs e)
        {

            if (dataGridView3.Rows.Count > 0 && dataGridView3.CurrentRow != null)
            {
                var selectedRowIndex = dataGridView3.CurrentRow.Index;

                var columnIndexes = new Dictionary<string, int>
                {
                    {"Belge Türü", dataGridView3.Columns["Belge Türü"].Index},
                    {"webvalue", dataGridView3.Columns["webvalue"].Index},
                    {"Gönderici Firma", dataGridView3.Columns["Gönderici Firma"].Index},
                    {"Sevk Ülke Ad", dataGridView3.Columns["Sevk Ülke Ad"].Index},
                    {"sevkulke",  dataGridView3.Columns["sevkulke"].Index},
                    {"Tic. Yapılan Ülke", dataGridView3.Columns["Tic. Yapılan Ülke"].Index},
                    {"ticyapilanulke", dataGridView3.Columns["ticyapilanulke"].Index},
                    {"Çıkış Ülke Ad", dataGridView3.Columns["Çıkış Ülke Ad"].Index},
                    {"cikisulke", dataGridView3.Columns["cikisulke"].Index},
                    {"Taşıma Şekli", dataGridView3.Columns["Taşıma Şekli"].Index},
                    {"tasimasekli", dataGridView3.Columns["tasimasekli"].Index},
                    {"Eşyanın Bulunduğu Yer", dataGridView3.Columns["Eşyanın Bulunduğu Yer"].Index},
                    {"Giriş Çıkış Gümrük", dataGridView3.Columns["Giriş Çıkış Gümrük"].Index},
                    {"Yük Boşaltma Gümrük", dataGridView3.Columns["Yük Boşaltma Gümrük"].Index},
                    {"Konteyner No", dataGridView3.Columns["Konteyner No"].Index},
                    {"Fatura Tarihi", dataGridView3.Columns["Fatura Tarihi"].Index},
                    {"Fatura No", dataGridView3.Columns["Fatura No"].Index},
                    {"Değer", dataGridView3.Columns["Değer"].Index},
                    {"Dolar Değer", dataGridView3.Columns["Dolar Değer"].Index},
                    {"Miktar", dataGridView3.Columns["Miktar"].Index},
                    {"Menşe Ülke", dataGridView3.Columns["Menşe Ülke"].Index},
                    {"menseulke",  dataGridView3.Columns["menseulke"].Index},
                    //{"Üretici Firma Adres", dataGridView3.Columns["Üretici Firma Adres"].Index},
                    //{"Üretici Firma Telefon", dataGridView3.Columns["Üretici Firma Telefon"].Index},
                    {"Belge Numarası", dataGridView3.Columns["Belge Numarası"].Index},
                    {"Belge Tarihi", dataGridView3.Columns["Belge Tarihi"].Index},
                    {"Edi Sıra No", dataGridView3.Columns["Edi Sıra No"].Index},
                    {"Taşıma Senedi No", dataGridView3.Columns["Taşıma Senedi No"].Index},
                    {"Marka", dataGridView3.Columns["Marka"].Index},
                    {"Model", dataGridView3.Columns["Model"].Index},
                    {"Eşya Kıymet", dataGridView3.Columns["Eşya Kıymet"].Index},
                    {"KG Miktar", dataGridView3.Columns["KG Miktar"].Index},
                    {"Gümrük Miktar", dataGridView3.Columns["Gümrük Miktar"].Index},
                    {"Değer Birimi", dataGridView3.Columns["Değer Birimi"].Index},
                    {"Birim Ad", dataGridView3.Columns["Birim Ad"].Index},
                    {"İmal Yılı", dataGridView3.Columns["İmal Yılı"].Index},
                    {"İthalat Amacı Tipi", dataGridView3.Columns["İthalat Amacı Tipi"].Index},
                    {"tesvikkapsaminda", dataGridView3.Columns["tesvikkapsaminda"].Index},
                    {"tareksvalue", dataGridView3.Columns["tareksvalue"].Index},
                    {"Muafiyet", dataGridView3.Columns["Muafiyet"].Index},
                    {"Üretici Firma", dataGridView3.Columns["Üretici Firma"].Index},
                    {"Üretici Firma Adres", dataGridView3.Columns["Üretici Firma Adres"].Index},
                    {"Üretici Firma Telefon", dataGridView3.Columns["Üretici Firma Telefon"].Index},
                    {"Ürün Cinsi", dataGridView3.Columns["Ürün Cinsi"].Index},
                    {"Ürün Yapım Malzemesi", dataGridView3.Columns["Ürün Yapım Malzemesi"].Index},
                    {"Ürün Yaş Grubu", dataGridView3.Columns["Ürün Yaş Grubu"].Index},
                    {"Ürün Adı", dataGridView3.Columns["Ürün Adı"].Index},
                    {"TSE", dataGridView3.Columns["TSE"].Index},




                };

                var values = new Dictionary<string, string>();
                foreach (var column in columnIndexes)
                {
                    values[column.Key] = dataGridView3.Rows[selectedRowIndex].Cells[column.Value].Value?.ToString() ?? string.Empty;

                }

                var webkodu = new Dictionary<string, string>
                {
                    { "webkodutext", $@"var labelField = document.getElementById('form:bilgiTipi_label');
                                            if (labelField) {{ 
                                                labelField.innerText = '{values["Belge Türü"]}'; 
                                            }}"},
                    { "webKoduinput", $@"var dropdown = document.getElementById('form:bilgiTipi_label');
                                            if (dropdown) {{
                                                dropdown.click();
                                                setTimeout(function() {{
                                                    var listItems = document.querySelectorAll('#form\\:bilgiTipi_panel li');
                                                    listItems.forEach(function(item) {{
                                                        if (item.innerText.trim() === '{values["Belge Türü"]}') {{
                                                            var event = new MouseEvent('click', {{
                                                                view: window,
                                                                bubbles: true,
                                                                cancelable: true
                                                            }});
                                                            item.dispatchEvent(event); 
                                                        }}
                                                    }});
                                                }}, 100); 
                                            }}"},

                    { "webKoduinputclick", $@"var listItems = document.querySelectorAll('li'); 
                                    var result = false; 
                                    listItems.forEach(function(item) {{ 
                                        if (item.innerText.trim() === '{values["Belge Türü"]}') {{ 
                                            item.click(); 
                                            result = true; 
                                        }} 
                                    }}); 
                                    result;"}

                };




                var scripts = new Dictionary<string, string>

                    {



                    { "gondericiUnvan", $@"var inputField = document.getElementById('form:gondericiUnvan');
                                    if (inputField) {{ inputField.value = '{values["Gönderici Firma"]}'; }}"},
                    {"sevkUlkeinput", $@"var labelinput = document.getElementById('form:sevkUlkesi_input');
                                    if (labelinput) {{ labelinput.value = '{values["sevkulke"]}'; }}"},
                    {"sevkUlke", $@"var labelField = document.getElementById('form:sevkUlkesi_label');
                                    if (labelField) {{ labelField.innerText = '{values["Sevk Ülke Ad"]}'; }}"},
                    {"ticyapilanulkeinput", $@"var labelinput = document.getElementById('form:ticaretUlkesi_input');
                                    if (labelinput) {{ labelinput.value = '{values["ticyapilanulke"]}'; }}"},
                    {"ticUlke", $@"var labelField = document.getElementById('form:ticaretUlkesi_label');
                                    if (labelField) {{ labelField.innerText = '{values["Tic. Yapılan Ülke"]}'; }}"},
                    {"cikisulkeinput", $@"var labelinput = document.getElementById('form:cikisUlkesi_input');
                                    if (labelinput) {{ labelinput.value = '{values["cikisulke"]}'; }}"},
                    {"cikisUlke", $@"var labelField = document.getElementById('form:cikisUlkesi_label');
                                    if (labelField) {{ labelField.innerText = '{values["Çıkış Ülke Ad"]}'; }}"},
                    {"tasimaSekli", $@"var labelField = document.getElementById('form:tasimaSekli_label');
                                    if (labelField) {{ labelField.innerText = '{values["Taşıma Şekli"]}'; }}"},
                    {"esya", $@"var inputField = document.getElementById('form:bulunduguYer');
                                    if (inputField) {{ inputField.value = '{values["Eşyanın Bulunduğu Yer"]}'; }}"},
                    {"girisCikis", $@"var labelField = document.getElementById('form:girisGumruk_label');
                                    if (labelField) {{ labelField.innerText = '{values["Giriş Çıkış Gümrük"]}'; }}"},
                    {"yukBosaltma", $@"var labelField = document.getElementById('form:bosaltmaGumruk_label');
                                    if (labelField) {{ labelField.innerText = '{values["Yük Boşaltma Gümrük"]}'; }}"},
                    {"konteynerno", $@"var inputField = document.getElementById('form:konteynerNo');
                                    if (inputField) {{ inputField.value = '{values["Konteyner No"]}'; }}"},
                    {"faturatarihi", $@"var inputField = document.getElementById('form:faturaTarihi_input');
                                    if (inputField) {{
                                        var rawDate = '{values["Fatura Tarihi"]}';
                                        var formattedDate = rawDate.split('.')[0].padStart(2, '0') + '/' +
                                                            rawDate.split('.')[1].padStart(2, '0') + '/' +
                                                            rawDate.split('.')[2].split(' ')[0];
                                        inputField.value = formattedDate; }}"},
                    {"faturano", $@"var inputField = document.getElementById('form:faturaNo');
                                    if (inputField) {{ inputField.value = '{values["Fatura No"]}'; }}"},
                    {"deger", $@"var inputField = document.getElementById('form:deger');
                         if (inputField) {{ inputField.value = '{values["Değer"].Replace(",", ".")}';}}"},
                    {"dolardeger", $@"var inputField = document.getElementById('form:dolarDeger');
                         if (inputField) {{ inputField.value = '{values["Dolar Değer"].Replace(",", ".")}';}}"},
                    {"miktar", $@"var inputField = document.getElementById('form:miktar');
                                    if (inputField) {{ inputField.value = '{values["Miktar"]}'; }}"},
                    {"menseUlkeinput", $@"var labelinput = document.getElementById('form:menseUlke_input');
                                    if (labelinput) {{ labelinput.value = '{values["menseulke"]}'; }}"},
                    {"menseulke", $@"var labelField = document.getElementById('form:menseUlke_label');
                                    if (labelField) {{ labelField.innerText = '{values["Menşe Ülke"]}'; }}"},
                    {"tescilNo", $@"var inputField = document.getElementById('form:bilgi');
                                    if (inputField) {{ inputField.value = '{values["Belge Numarası"]}'; }}"},
                    {"tescilTarihi", $@"var inputField = document.getElementById('form:bilgiTarihi_input');
                                    if (inputField) {{
                                        var rawDate = '{values["Belge Tarihi"]}';
                                        var formattedDate = rawDate.split('.')[0].padStart(2, '0') + '/' +
                                                            rawDate.split('.')[1].padStart(2, '0') + '/' +
                                                            rawDate.split('.')[2].split(' ')[0];
                                        inputField.value = formattedDate; }}"},
                    {"ediSirano", $@"var inputField = document.getElementById('form:satirNo');
                                    if (inputField) {{ inputField.value = '{values["Edi Sıra No"]}'; }}"},
                    {"tasimaSenedi", $@"var inputField = document.getElementById('form:tasimaSenediNo');
                                    if (inputField) {{ inputField.value = '{values["Taşıma Senedi No"]}'; }}"},
                    {"esyaKiymet", $@"var inputField = document.getElementById('form:esyaKiymet');
                         if (inputField) {{ inputField.value = '{values["Eşya Kıymet"].Replace(",", ".")}';}}"},

                    {"kgMiktar", $@"var inputField = document.getElementById('form:kgMiktar');
                         if (inputField) {{ inputField.value = '{values["KG Miktar"].Replace(",", ".")}';}}"},
                    {"gumrukMiktar", $@"var inputField = document.getElementById('form:gumrukMiktar');
                         if (inputField) {{ inputField.value = '{values["Gümrük Miktar"].Replace(",", ".")}';}}"},
                    {"birimi", $@"var labelField = document.getElementById('form:miktarBirimi_label');
                                    if (labelField) {{ labelField.innerText = '{values["Birim Ad"]}'; }}"},
                    {"gumrukMiktariBirimi", $@"var labelField = document.getElementById('form:gumrukMiktarBirimi_label');
                                    if (labelField) {{ labelField.innerText = '{values["Birim Ad"]}'; }}"},
                    {"imalYili", $@"var labelField = document.getElementById('form:imalYili_label');
                                    if (labelField) {{ labelField.innerText = '{values["İmal Yılı"]}'; }}"},
                    {"ithalatAmaciTipi", $@"var labelField = document.getElementById('form:ithalatAmaciTipi_label');
                                    if (labelField) {{ labelField.innerText = '{values["İthalat Amacı Tipi"]}'; }}"},
                    {"DegerBirimi", $@"var labelField = document.getElementById('form:degerBirimi_label');
                                    if (labelField) {{ labelField.innerText = '{values["Değer Birimi"]}'; }}"},
                    {"muafiyetlabel", $@"var labelField = document.getElementById('form:muafiyet_label');
                                    if (labelField) {{ labelField.innerText = '{values["Muafiyet"]}'; }}"},
                    {"muafiyetinput", $@"var labelinput = document.getElementById('form:muafiyet_input');
                        if (labelinput) {{ labelinput.value = '{values["tareksvalue"]}'; }}"},



    };


                if (values["tesvikkapsaminda"] == "True")
                {
                    scripts.Add("tesvikkapsaminda", $@"var tesvikkapsaminda = document.getElementById('form:tesvikKapsaminda_input');
                        if (tesvikkapsaminda) {{ tesvikkapsaminda.click(); }}");
                }

                scripts.Add("OptionByValue", $@"
    function OptionByValue(elementId, optionText) {{
        var selectElement = document.getElementById(elementId);
        if (selectElement) {{
            for (var i = 0; i < selectElement.options.length; i++) {{
                if (selectElement.options[i].text === optionText) {{
                    return selectElement.options[i].value;
                }}
            }}
        }}
        return '';
    }}

    var selectedValue;
    selectedValue = OptionByValue('form:tasimaSekli_input', '{values["Taşıma Şekli"]}');
    var labelInput = document.getElementById('form:tasimaSekli_input');
    if (labelInput) {{
        labelInput.value = selectedValue;
    }}
    selectedValue = OptionByValue('form:girisGumruk_input', '{values["Giriş Çıkış Gümrük"]}');
    var labelInput = document.getElementById('form:girisGumruk_input');
    if (labelInput) {{
        labelInput.value = selectedValue;
    }}
    selectedValue = OptionByValue('form:bosaltmaGumruk_input', '{values["Yük Boşaltma Gümrük"]}');
    var labelInput = document.getElementById('form:bosaltmaGumruk_input');
    if (labelInput) {{
        labelInput.value = selectedValue;
    }}
 selectedValue = OptionByValue('form:degerBirimi_input', '{values["Değer Birimi"]}');
    var labelInput = document.getElementById('form:degerBirimi_input');
    if (labelInput) {{
        labelInput.value = selectedValue;
    }}
 selectedValue = OptionByValue('form:miktarBirimi_input', '{values["Birim Ad"]}');
    var labelInput = document.getElementById('form:miktarBirimi_input');
    if (labelInput) {{
        labelInput.value = selectedValue;
    }}
 selectedValue = OptionByValue('form:gumrukMiktarBirimi_input', '{values["Birim Ad"]}');
    var labelInput = document.getElementById('form:gumrukMiktarBirimi_input');
    if (labelInput) {{
        labelInput.value = selectedValue;
    }}

 selectedValue = OptionByValue('form:imalYili_input', '{values["İmal Yılı"]}');
    var labelInput = document.getElementById('form:imalYili_input');
    if (labelInput) {{
        labelInput.value = selectedValue;
    }}
 selectedValue = OptionByValue('form:ithalatAmaciTipi_input', '{values["İthalat Amacı Tipi"]}');
    var labelInput = document.getElementById('form:ithalatAmaciTipi_input');
    if (labelInput) {{
        labelInput.value = selectedValue;
    }}


    
                ");



                var esyaTanimiScript = @"
        var esyaTanimi = '';
        var esyaTanimiElement = document.querySelectorAll('table tbody tr:nth-of-type(2) td:nth-of-type(2) span')[0];
        if (esyaTanimiElement) {
            esyaTanimi = esyaTanimiElement.innerText;
        }

        // 'form:kaplarVeEsyaninTanimi' input alanına değeri yaz
        var inputField = document.getElementById('form:kaplarVeEsyaninTanimi');
        if (inputField) {
            inputField.value = esyaTanimi;
        }
    ";
                //  var belgeturuclick = $@"listItems.forEach(function(item) {{ if (item.innerText.trim() === 'TASIMA_BELGESI') {{ item.click(); result = true; }} }}); result;";







                string FUrunAdi, FImalatciUnvani, FMarka, FModel, FUrunCinsi, FUrunYapimMalzemesi,
                               FUrunYasGrubu, FUrunKorumaOzelligi, FImalatciAdresi, FImalatciTelefonu,
                               FTebligKapsamiStandartlar;

                string belgeturu = values["Belge Türü"];

                string script2 = $@"
    // Eğer 'tds' zaten tanımlıysa, yeniden tanımlama yerine mevcut değeri yeniden kullan
    if (typeof tds === 'undefined') {{
        var tds = document.querySelectorAll('td');
    }} else {{
        tds = document.querySelectorAll('td');
    }}

    var result = [];
    var j = 0;

    tds.forEach(td => {{
        let text = td.innerText.trim();
        if (text === '') return;

        if (text === '* ÜRÜN ADI') {{
            result.push({{ key: 'FUrunAdi', value: `form:ozellikTable:${{j}}:a` }});
            j++;
        }} else if (text === '* İMALATÇI UNVANI') {{
            let belgeturu = '{belgeturu}'; 
            result.push({{ key: 'FImalatciUnvani', value: belgeturu === '2' 
                ? `form:ozellikTable:${{j}}:a` 
                : `form:ozellikTable:${{j}}:iunvan` }});
            j++;
        }} else if (text === '* MARKA') {{
            let belgeturu = '{belgeturu}';
            result.push({{ key: 'FMarka', value: belgeturu === '2' 
                ? `form:ozellikTable:${{j}}:a` 
                : `form:ozellikTable:${{j}}:marka` }});
            j++;
        }} else if (text === '* MODEL') {{
            let belgeturu = '{belgeturu}';
            result.push({{ key: 'FModel', value: belgeturu === '2' 
                ? `form:ozellikTable:${{j}}:a` 
                : `form:ozellikTable:${{j}}:model` }});
            j++;
        }} else if (text === '* ÜRÜN CİNSİ') {{
            result.push({{ key: 'FUrunCinsi', value: `form:ozellikTable:${{j}}:` }});
            j++;
        }} else if (text === '* ÜRÜN YAPIM MALZEMESİ') {{
            result.push({{ key: 'FUrunYapimMalzemesi', value: `form:ozellikTable:${{j}}:` }});
            j++;
        }} else if (text === '* ÜRÜN YAŞ GRUBU') {{
            result.push({{ key: 'FUrunYasGrubu', value: `form:ozellikTable:${{j}}:` }});
            j++;
        }} else if (text === '* ÜRÜN KORUMA ÖZELLİĞİ') {{
            result.push({{ key: 'FUrunKorumaOzelligi', value: `form:ozellikTable:${{j}}:e_input` }});
            j++;
        }} else if (text === '* İMALATÇI ADRESİ') {{
            result.push({{ key: 'FImalatciAdresi', value: `form:ozellikTable:${{j}}:a` }});
            j++;
        }} else if (text === '* İMALATÇI TELEFONU') {{
            result.push({{ key: 'FImalatciTelefonu', value: `form:ozellikTable:${{j}}:a` }});
            j++;
        }} else if (text === '* TEBLİĞ KAPSAMI STANDARTLAR') {{
            result.push({{ key: 'FTebligKapsamiStandartlar', value: `form:ozellikTable:${{j}}:` }});
            j++;
        }} else if (text === 'DİĞER STANDARTLAR') {{
            result.push({{ key: 'FDigerStandartlar', value: `form:ozellikTable:${{j}}:b` }});
            j++;
        }} else if (text === '* TİCARİ İSİM') {{
            result.push({{ key: 'FTicariisim', value: `form:ozellikTable:${{j}}:a` }});
            j++;
        }} else if (text === '* SERİ/ŞASİ NUMARASI') {{
            result.push({{ key: 'FSeriSasiNo', value: `form:ozellikTable:${{j}}:a` }});
            j++;
        }}
    }});

    delete tds;
    delete result;
    delete j;
    result;";
                var response = chromiumWebBrowser1.EvaluateScriptAsync(script2).Result;

                string scriptDynamic = "";
                var results = response.Result as IEnumerable<dynamic>;



                foreach (var item in results)
                {
                    var key = (item as IDictionary<string, object>)?["key"]?.ToString();
                    var value = (item as IDictionary<string, object>)?["value"]?.ToString();
                    switch (key)
                    {
                        case "FUrunAdi": FUrunAdi = value.ToString(); break;
                        case "FImalatciUnvani": FImalatciUnvani = value.ToString(); break;
                        case "FMarka": FMarka = value.ToString(); break;
                        case "FModel": FModel = value.ToString(); break;
                        case "FUrunCinsi": FUrunCinsi = value.ToString(); break;
                        case "FUrunYapimMalzemesi": FUrunYapimMalzemesi = value.ToString(); break;
                        case "FUrunYasGrubu": FUrunYasGrubu = value.ToString(); break;
                        case "FImalatciAdresi": FImalatciAdresi = value.ToString(); break;
                        case "FImalatciTelefonu": FImalatciTelefonu = value.ToString(); break;
                        case "FTebligKapsamiStandartlar": FTebligKapsamiStandartlar = value.ToString(); break;
                        case "FUrunKorumaOzelligi": FUrunKorumaOzelligi = value.ToString(); break;

                            //belge türü 2 olduğundan kullanmadık
                            //case "FDigerStandartlar": FDigerStandartlar = value.ToString(); break;
                            //case "FTicariisim": FTicariisim = value.ToString(); break;
                            //case "FSeriSasiNo": FSeriSasiNo = value.ToString(); break;

                    };

                    if (key == "FModel")
                    {
                        scriptDynamic = $"document.getElementById('{value}Buton').click();";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                        System.Threading.Thread.Sleep(500);
                        scriptDynamic = $@"var inputField = document.getElementById('{value}b');
                        if (inputField) {{ inputField.value = '{values["Model"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                    };
                    if (key == "FMarka")
                    {
                        scriptDynamic = $"document.getElementById('{value}Buton').click();";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                        System.Threading.Thread.Sleep(500);
                        scriptDynamic = $@"var inputField = document.getElementById('{value}b');
                        if (inputField) {{ inputField.value = '{values["Marka"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                    };
                    if (key == "FImalatciUnvani")
                    {
                        scriptDynamic = $@"var inputField = document.getElementById('{value}');
                        if (inputField) {{ inputField.value = '{values["Üretici Firma"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                    };
                    if (key == "FImalatciAdresi")
                    {
                        scriptDynamic = $@"var inputField = document.getElementById('{value}');
                        if (inputField) {{ inputField.value = '{values["Üretici Firma Adres"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                    };
                    if (key == "FImalatciTelefonu")
                    {
                        scriptDynamic = $@"var inputField = document.getElementById('{value}');
                        if (inputField) {{ inputField.value = '{values["Üretici Firma Telefon"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                    };
                    if (key == "FUrunCinsi")
                    {
                        scriptDynamic = $@"var labelinput = document.getElementById('{value}e_input');
                        if (labelinput) {{ labelinput.value = '{values["Ürün Cinsi"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                        scriptDynamic = $@"var labelinput = document.getElementById('{value}e_label');
                        if (labelinput) {{ labelinput.innerText = '{values["Ürün Cinsi"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);

                    };

                    if (key == "FUrunYapimMalzemesi")
                    {
                        scriptDynamic = $@"var labelinput = document.getElementById('{value}e_input');
                        if (labelinput) {{ labelinput.value = '{values["Ürün Yapım Malzemesi"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                        scriptDynamic = $@"var labelinput = document.getElementById('{value}e_label');
                        if (labelinput) {{ labelinput.innerText = '{values["Ürün Yapım Malzemesi"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);

                    };
                    if (key == "FUrunYasGrubu")
                    {
                        scriptDynamic = $@"var labelinput = document.getElementById('{value}e_input');
                        if (labelinput) {{ labelinput.value = '{values["Ürün Yaş Grubu"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                        scriptDynamic = $@"var labelinput = document.getElementById('{value}e_label');
                        if (labelinput) {{ labelinput.innerText = '{values["Ürün Yaş Grubu"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);

                    };
                    if (key == "FUrunAdi")
                    {
                        scriptDynamic = $@"var labelinput = document.getElementById('{value}');
                        if (labelinput) {{ labelinput.value = '{values["Ürün Adı"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);

                    };
                    if (key == "FTebligKapsamiStandartlar")
                    {
                        scriptDynamic = $@"var labelinput = document.getElementById('{value}e_input');
                        if (labelinput) {{ labelinput.value = '{values["TSE"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);
                        scriptDynamic = $@"var labelinput = document.getElementById('{value}e_label');
                        if (labelinput) {{ labelinput.innerText = '{values["TSE"]}'; }}";
                        chromiumWebBrowser1.ExecuteScriptAsync(scriptDynamic);

                    };

                }


                foreach (var script in webkodu)
                {
                    chromiumWebBrowser1.ExecuteScriptAsync(script.Value);
                }
                System.Threading.Thread.Sleep(500);
                foreach (var script in scripts)
                {
                    chromiumWebBrowser1.ExecuteScriptAsync(script.Value);
                    chromiumWebBrowser1.ExecuteScriptAsync(esyaTanimiScript);

                }


            }
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

     
    }

}
