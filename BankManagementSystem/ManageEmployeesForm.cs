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
    public partial class ManageEmployeesForm : Form
    {
        private string connectionString = "server=localhost;user=root;password=;database=bank_system;";
        private int selectedID = -1;

        public ManageEmployeesForm()
        {
            InitializeComponent();
        }

        private void ManageEmployeesForm_Load(object sender, EventArgs e)
        {
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Teller");
            cmbRole.Items.Add("Manager");

            LoadEmployees();
            dgvEmployees.AllowUserToAddRows = false;

        }

        private void LoadEmployees()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM employees", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvEmployees.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO employees (name, username, password, role) VALUES (@name, @username, @password, @role)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee added.");
                LoadEmployees();
                ClearFields();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Select an employee first.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE employees SET name = @name, username = @username, password = @password, role = @role WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", selectedID);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee updated.");
                LoadEmployees();
                ClearFields();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Select an employee first.");
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this employee?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.No) return;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM employees WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", selectedID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Employee deleted.");
                LoadEmployees();
                ClearFields();
            }
        }

        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];
                selectedID = Convert.ToInt32(row.Cells["id"].Value);
                txtID.Text = selectedID.ToString();
                txtName.Text = row.Cells["name"].Value.ToString();
                txtUsername.Text = row.Cells["username"].Value.ToString();
                txtPassword.Text = row.Cells["password"].Value.ToString();
                cmbRole.SelectedItem = row.Cells["role"].Value.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtID.Clear();
            txtName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            selectedID = -1;
        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvEmployees_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];
                selectedID = Convert.ToInt32(row.Cells["id"].Value);
                txtID.Text = selectedID.ToString();
                txtName.Text = row.Cells["name"].Value.ToString();
                txtUsername.Text = row.Cells["username"].Value.ToString();
                txtPassword.Text = row.Cells["password"].Value.ToString();
                cmbRole.SelectedItem = row.Cells["role"].Value.ToString();
            }
        }
        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.FileName = "Employees.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
                PdfWriter.GetInstance(pdfDoc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                pdfDoc.Open();

                Paragraph title = new Paragraph("Employee List", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20f;
                pdfDoc.Add(title);

                PdfPTable pdfTable = new PdfPTable(dgvEmployees.Columns.Count);
                pdfTable.WidthPercentage = 100;

                foreach (DataGridViewColumn column in dgvEmployees.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                    cell.BackgroundColor = new BaseColor(240, 240, 240);
                    pdfTable.AddCell(cell);
                }

                foreach (DataGridViewRow row in dgvEmployees.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        pdfTable.AddCell(cell.Value?.ToString() ?? "");
                    }
                }

                pdfDoc.Add(pdfTable);
                pdfDoc.Close();

                MessageBox.Show("PDF exported successfully.");
            }
        }

    }

}
