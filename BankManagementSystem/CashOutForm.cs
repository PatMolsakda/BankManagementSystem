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
    public partial class CashOutForm : Form
    {
        private string connectionString = "server=localhost;user=root;password=;database=bank_system;";

        public CashOutForm()
        {
            InitializeComponent();
        }

        private void CashOutForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            txtAccNum.Focus();
        }

        private void btnCashOut_Click(object sender, EventArgs e)
        {
            if (txtAccNum.Text == "" || txtAmount.Text == "")
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

                    // Check current balance
                    string checkQuery = "SELECT balance FROM customer_accounts WHERE account_number = @accNumber";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@accNumber", txtAccNum.Text);

                    object result = checkCmd.ExecuteScalar();

                    if (result == null)
                    {
                        lblStatus.Text = "Account not found.";
                        return;
                    }

                    decimal currentBalance = Convert.ToDecimal(result);

                    if (currentBalance < amount)
                    {
                        lblStatus.Text = "Insufficient funds.";
                        return;
                    }

                    // Update balance
                    string updateQuery = "UPDATE customer_accounts SET balance = balance - @cashAmount WHERE account_number = @accNumber";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@cashAmount", amount);
                    updateCmd.Parameters.AddWithValue("@accNumber", txtAccNum.Text);
                    updateCmd.ExecuteNonQuery();

                    // Insert transaction
                    string insertQuery = "INSERT INTO transactions (account_number, transaction_type, amount) VALUES (@accNum, 'Cash Out', @amt)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@accNum", txtAccNum.Text);
                    insertCmd.Parameters.AddWithValue("@amt", amount);
                    insertCmd.ExecuteNonQuery();

                    lblStatus.Text = "Cash out successful.";
                    txtAccNum.Clear();
                    txtAmount.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;

            if (e.KeyChar == '.' && txtAmount.Text.Contains("."))
                e.Handled = true;
        }

        private void txtAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void btnTransactionHistory_Click(object sender, EventArgs e)
        {
            TransactionHistoryForm historyForm = new TransactionHistoryForm();
            historyForm.ShowDialog();
        }


    }
}
