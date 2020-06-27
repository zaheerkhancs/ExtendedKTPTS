using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ExtendedKtpts
{
    public partial class GTNReportViewForm : Form
    {
        public static string UniCon = ConfigurationManager.ConnectionStrings["ExtKTPTS.Properties.Settings.ConnectionString"].ToString();
        public GTNReportViewForm()
        {
            InitializeComponent();
        }

        private void GTNReportViewForm_Load(object sender, EventArgs e)
        {

        }
        public void GetGTNCodeList()
        {
            string year = dtDate.Value.Year.ToString();
            string month = dtDate.Value.Month.ToString();
            string day = dtDate.Value.Day.ToString();
            string purchasedate = year + "-" + month + "-" + day;

            var Qry = "SELECT DISTINCT TOP (100) PERCENT " +
                "dbo.GTN.TransferNoteID, " +
                "dbo.GTN.GTNCode  " +
                "FROM dbo.GTN INNER JOIN dbo.TransferredBales ON dbo.GTN.TransferNoteID = dbo.TransferredBales.GTNID " +
                "INNER JOIN dbo.Bale ON dbo.TransferredBales.BaleID = dbo.Bale.BaleID " +
                "WHERE(dbo.Bale.Date = '" + purchasedate + "') ORDER BY dbo.GTN.TransferNoteID";
            try
            {
                var dt = new DataTable();
                var sqlCon = new SqlConnection(UniCon);
                var da = new SqlDataAdapter(Qry, sqlCon);
                da.Fill(dt);
                this.cmbGTN.ValueMember = dt.Columns["TransferNoteID"].ToString();
                this.cmbGTN.DisplayMember = dt.Columns["GTNCode"].ToString();
                cmbGTN.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message.ToString());
            }
            finally
            {


            }
        }
        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            GetGTNCodeList();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(@"D:\\Solution Code\\ExtendedKtpts\\CascadeReport.rpt");

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = cmbGTN.Text;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["@gtnCode"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;

            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crystalReportViewer1.ReportSource = cryRpt;
            crystalReportViewer1.Refresh();

            /*
            var param = new CrystalDecisions.Shared.ParameterValues();
            var paramVal = new CrystalDecisions.Shared.ParameterDiscreteValue();
            paramVal.Value = cmbGTN.Text;
            param.Add(paramVal);
            CascadeReport rpt = new CascadeReport();
            rpt.SetParameterValue("@gtnCode", param);
            //crystalReportViewer1.ReportSource = rpt;

            */
            /*
            ParameterDiscreteValue objDiscreteValue;
            ParameterField objParameterField;
            objDiscreteValue = new ParameterDiscreteValue();
            objDiscreteValue.Value = cmbGTN.Text;
            objParameterField = crystalReportViewer1.ParameterFieldInfo["@gtnCode"];
            objParameterField.CurrentValues.Add(objDiscreteValue);
            crystalReportViewer1.ParameterFieldInfo.Add(objParameterField);
            */
            //crystalReportViewer1.ReportSource = Application.StartupPath + "\\" + "~\\CrystalReportNonWiz.rpt"; 
            //crystalReportViewer1.ReportSource = @"D:\\Solution Code\\ExtendedKtpts\\CascadeReport.rpt";
            //crystalReportViewer1.Refresh();
        }

        private void GTNReportViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //crystalReportViewer1.ReportSource.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
