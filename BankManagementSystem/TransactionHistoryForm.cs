using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace BankManagementSystem
{
    public partial class TransactionHistoryForm : Form
    {
        private string connectionString = "server=localhost;user id=root;password=;database=bank_system";

        public TransactionHistoryForm()
        {
            InitializeComponent();
        }

        private void TransactionHistoryForm_Load(object sender, EventArgs e)
        {
            cmbTransactionType.Items.Clear();
            cmbTransactionType.Items.AddRange(new[] { "All", "Cash In", "Cash Out" });
            cmbTransactionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTransactionType.SelectedIndex = 0;

            LoadAllTransactions();
        }

        private void LoadAllTransactions()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM transactions ORDER BY date DESC";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvTransactions.DataSource = dt;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            string selectedType = cmbTransactionType.SelectedItem.ToString();
            DateTime selectedDate = dtpFilter.Value.Date;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                StringBuilder queryBuilder = new StringBuilder("SELECT * FROM transactions WHERE 1=1");

                if (!chkAllDates.Checked)
                {
                    queryBuilder.Append(" AND DATE(date) = @date");
                }

                if (selectedType != "All")
                {
                    queryBuilder.Append(" AND transaction_type = @type");
                }

                if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text))
                {
                    queryBuilder.Append(" AND account_number = @acc");
                }

                MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn);

                if (!chkAllDates.Checked)
                    cmd.Parameters.AddWithValue("@date", selectedDate);

                if (selectedType != "All")
                    cmd.Parameters.AddWithValue("@type", selectedType);

                if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text))
                    cmd.Parameters.AddWithValue("@acc", txtAccountNumber.Text);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvTransactions.DataSource = dt;
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (dgvTransactions.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "TransactionHistory.pdf"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportToPdf(sfd.FileName);
                }
            }
        }

        private void ExportToPdf(string filename)
        {
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 20f);

            try
            {
                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    Paragraph title = new Paragraph("Transaction History", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD));
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 20f;
                    pdfDoc.Add(title);

                    PdfPTable pdfTable = new PdfPTable(dgvTransactions.Columns.Count);
                    pdfTable.WidthPercentage = 100;

                    // Add headers
                    foreach (DataGridViewColumn column in dgvTransactions.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText))
                        {
                            BackgroundColor = BaseColor.LIGHT_GRAY
                        };
                        pdfTable.AddCell(cell);
                    }

                    // Add rows
                    foreach (DataGridViewRow row in dgvTransactions.Rows)
                    {
                        if (row.IsNewRow) continue;

                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            pdfTable.AddCell(new Phrase(cell.Value?.ToString() ?? ""));
                        }
                    }

                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();

                    MessageBox.Show("PDF exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to export PDF: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
