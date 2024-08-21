using CefSharp;
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
using CefSharp.WinForms;

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

        private void button2_Click(object sender, EventArgs e)
        {
            if (!isPageLoaded)
            {
                chromiumWebBrowser1.LoadUrl("https://eortak.dtm.gov.tr/eortak/login/selectApplication.htm");
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            dataGridView4.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView4_CellDoubleClick);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!isPageLoaded)
            {
                chromiumWebBrowser1.LoadUrl("https://eortak.dtm.gov.tr/eortak/login/selectApplication.htm");
            }
        }

        
        private bool hasButtonClicked = false;

        private void chromiumWebBrowser1_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading && !isPageLoaded)
            {
                isPageLoaded = true;

                chromiumWebBrowser1.ExecuteScriptAsync("selectSigningDevice('Turkcell');");

                string phoneNumberScript = "document.getElementById('phoneNumber').value = '05301698028';";
                chromiumWebBrowser1.ExecuteScriptAsync(phoneNumberScript);
                chromiumWebBrowser1.ExecuteScriptAsync("document.getElementById('saveButton').click();");
                chromiumWebBrowser1.LoadingStateChanged -= chromiumWebBrowser1_LoadingStateChanged;
            }
        }


        private void LoadDataIntoGridView(string tareksmasterid)
        {
            string connectionString = "Data Source=192.168.0.106\\GD;Initial Catalog=Sec2024;User ID=sqllive;Password=I76PC9Lke;Connection Timeout=3600; MultipleActiveResultSets=true";

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

            string grid3 = $"select  * from sdi_tareksdetay where tareksmasterid = @TareksMasterId order by edisirano, faturatarih";

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

                label4.Text = "Toplam: " + dataGridView3.Rows.Count;

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
  //          labelTotalCount.Text = "Toplam: " + dataGridView4.Rows.Count;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=192.168.0.106\\GD;Initial Catalog=Sec2024;User ID=sqllive;Password=I76PC9Lke;Connection Timeout=3600; MultipleActiveResultSets=true";

            string referansNo = textBox4.Text.Trim();
            string grid4 = "select * from sdi_tareksarama_view where firmaGrup = 'UNIVERSAL'";
            if (!string.IsNullOrEmpty(referansNo))
            {
                grid4 += " AND referansno = @ReferansNo";
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

                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView4.DataSource = dataTable;

//                    labelTotalCount.Text = "Toplam: " + dataTable.Rows.Count;

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

  
    }
}
