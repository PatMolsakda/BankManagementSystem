using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace BankManagementSystem
{
    public partial class CustomerAccountsForm : Form
    {
        private string connectionString = "server=localhost;user=root;password=;database=bank_system;";

        public CustomerAccountsForm()
        {
            InitializeComponent();
        }

        private void CustomerAccountsForm_Load(object sender, EventArgs e)
        {
            LoadAccounts();
            dgvAccounts.CellFormatting += dgvAccounts_CellFormatting;
            txtSearch.Text = "Search by name or account number";
            txtSearch.ForeColor = Color.Gray;
            dgvAccounts.AllowUserToAddRows = false;
        }

        private void LoadAccounts(string search = "")
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM customer_accounts WHERE name LIKE @search OR account_number LIKE @search";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvAccounts.DataSource = dt;
            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtAccountNumber.Text == "" || txtBalance.Text == "")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO customer_accounts (name, account_number, balance) VALUES (@name, @acc, @balance)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@acc", txtAccountNumber.Text);
                cmd.Parameters.AddWithValue("@balance", Convert.ToDecimal(txtBalance.Text));

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account added.");
                    LoadAccounts();
                    ClearFields();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAccounts(txtSearch.Text);
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtAccountNumber.Clear();
            txtBalance.Clear();
        }

        private int selectedAccountId = -1;

        private void dgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvAccounts.Rows[e.RowIndex];

            selectedAccountId = Convert.ToInt32(row.Cells["id"].Value);

            txtAccountNumber.Text = row.Cells["account_number"].Value.ToString();
            txtName.Text = row.Cells["name"].Value.ToString();

            decimal balance = Convert.ToDecimal(row.Cells["balance"].Value);
            txtBalance.Text = balance.ToString("N2");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedAccountId == -1)
            {
                MessageBox.Show("Select an account to update.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE customer_accounts SET name = @name, account_number = @acc, balance = @balance WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", selectedAccountId);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@acc", txtAccountNumber.Text);
                cmd.Parameters.AddWithValue("@balance", Convert.ToDecimal(txtBalance.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Account updated.");
                LoadAccounts();
                ClearFields();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedAccountId == -1)
            {
                MessageBox.Show("Select an account to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this account?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM customer_accounts WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", selectedAccountId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Account deleted.");
                LoadAccounts();
                ClearFields();
            }
        }
        private void dgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvAccounts.Columns[e.ColumnIndex].Name == "balance" && e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal amount))
            {
                e.Value = $"{amount:N2} USD";
                e.FormattingApplied = true;
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search by name or account number")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search by name or account number";
                txtSearch.ForeColor = Color.Gray;
            }
        }
        private void txtAccNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                return;

            e.Handled = true;
        }
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsLetter(e.KeyChar) || e.KeyChar == ' ')
                return;

            e.Handled = true;
        }
        private void txtBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                return;

            if (e.KeyChar == '.' && !txtBalance.Text.Contains("."))
                return;

            e.Handled = true;
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.FileName = "CustomerAccounts.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
                PdfWriter.GetInstance(pdfDoc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                pdfDoc.Open();

                Paragraph title = new Paragraph("Customer Accounts", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20f;
                pdfDoc.Add(title);

                PdfPTable pdfTable = new PdfPTable(dgvAccounts.Columns.Count);
                pdfTable.WidthPercentage = 100;

                foreach (DataGridViewColumn column in dgvAccounts.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                    cell.BackgroundColor = new BaseColor(230, 230, 230);
                    pdfTable.AddCell(cell);
                }

                foreach (DataGridViewRow row in dgvAccounts.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string value = cell.Value?.ToString() ?? "";
                        if (cell.OwningColumn.Name == "balance" && decimal.TryParse(value, out decimal balance))
                        {
                            value = balance.ToString("N2") + " USD";
                        }
                        pdfTable.AddCell(value);
                    }
                }

                pdfDoc.Add(pdfTable);
                pdfDoc.Close();

                MessageBox.Show("PDF exported successfully.");
            }
        }

    }
}
