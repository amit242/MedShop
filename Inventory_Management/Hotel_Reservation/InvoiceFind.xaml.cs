using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for InvoiceFind.xaml
    /// </summary>
    public partial class InvoiceFind : Window
    {
        public InvoiceFind()
        {
            InitializeComponent();
        }

        private void Find_Invoice()
        {
            string invoiceNo = textBoxInvoiceNo.Text;
            string customerNo = textBoxCustomerNo.Text;
            string invoiceFrom = datePickerInvoiceFrom.Text;
            string invoiceTo = datePickerInvoiceTo.Text;

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM invoice_table WHERE (InvoiceNo='" + invoiceNo + "') OR (InvoiceCustomerNo='" + customerNo + "') OR ((InvoiceDate>='" + invoiceFrom + "') AND (InvoiceDate<='" + invoiceTo + "'))", con1);
            cmd1.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd1);
            DataTable datatbl = new DataTable("vendor_table");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);

            con1.Close();
        }

        private void buttonStartSearch_Click(object sender, RoutedEventArgs e)
        {
            Find_Invoice();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
