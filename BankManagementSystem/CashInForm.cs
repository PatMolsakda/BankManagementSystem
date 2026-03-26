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

namespace BankManagementSystem
{
    public partial class CashInForm : Form
    {
        private string connectionString = "server=localhost;user=root;password=;database=bank_system;";

        public CashInForm()
        {
            InitializeComponent();
        }

        private void CashInForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "";
        }

        private void btnCashIn_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";

            if (txtAccount.Text == "" || txtAmount.Text == "")
            {
                lblStatus.Text = "Please fill in all fields.";
                return;
            }

            if (!decimal.TryParse(txtAmount.Text.Trim(), out decimal amount) || amount <= 0)
            {
                lblStatus.Text = "Invalid amount.";
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string updateQuery = "UPDATE customer_accounts SET balance = balance + @cashAmount WHERE account_number = @accNumber";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@cashAmount", amount);
                    updateCmd.Parameters.AddWithValue("@accNumber", txtAccount.Text);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        string insertQuery = "INSERT INTO transactions (account_number, transaction_type, amount) VALUES (@accNum, 'Cash In', @amt)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@accNum", txtAccount.Text);
                        insertCmd.Parameters.AddWithValue("@amt", amount);

                        insertCmd.ExecuteNonQuery();
                        lblStatus.Text = "Cash in successful.";

                        txtAccount.Clear();
                        txtAmount.Clear();
                        txtAccount.Focus();
                    }
                    else
                    {
                        lblStatus.Text = "Account not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && txtAmount.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtAccNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTransactionHistory_Click(object sender, EventArgs e)
        {
            TransactionHistoryForm historyForm = new TransactionHistoryForm();
            historyForm.ShowDialog();
        }
    }
}


