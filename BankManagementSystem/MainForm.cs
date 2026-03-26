using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BankManagementSystem;


namespace BankManagementSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            ManageEmployeesForm form = new ManageEmployeesForm();
            form.ShowDialog();
        }

        private void btnCustomerInfo_Click(object sender, EventArgs e)
        {
            CustomerAccountsForm form = new CustomerAccountsForm();
            form.ShowDialog();
        }

        private void btnCashIn_Click(object sender, EventArgs e)
        {
            CashInForm form = new CashInForm();
            form.ShowDialog();
        }

        private void btnCashOut_Click(object sender, EventArgs e)
        {
            CashOutForm form = new CashOutForm();
            form.ShowDialog();
        }

        private void btnTransactionHistory_Click(object sender, EventArgs e)
        {
            TransactionHistoryForm historyForm = new TransactionHistoryForm();
            historyForm.ShowDialog();
        }
    }
}
