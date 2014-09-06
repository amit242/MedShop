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
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : Window
    {
        AddOrderItemInvoice addorderiteminvoice = new AddOrderItemInvoice(); // For invoking add order item window get item no

        string sItemNo;
        string sQuantity;
        string sUnitPrice;

        public int iInvoiceNo;
        string tempinvoiceno = null;
        
        public Invoice()
        {
            InitializeComponent();
            ShowShipping();
            ShowInvoiceType();
            ShowPaymentMethod();
            GenerateInvoiceNo();
            Set_Date_Today();
        }

        // Show the current date and time at the window load. NOT WORKING NOW
        private void Set_Date_Today()
        {
            datePickerOrderDate.DisplayDate = DateTime.Now;
            datePickerInvoiceDate.DisplayDate = DateTime.Now;
            datePickerShippedDate.DisplayDate = DateTime.Now;
        }

        private void buttonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newCustomerAdd = new CustomerAdd();
            newCustomerAdd.ShowDialog();
            Get_Customer_No();
            Show_Customer();
        }

        /* Gets the last vendor Id from vendor_table */
        private void Get_Customer_No()
        {
            string tempcustomerno = "";

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select * from customer_table";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                tempcustomerno = dr.GetString(2);
            }
            textBoxInvoiceCustomerNo.Text = tempcustomerno;
        }

        private void Show_Customer()
        {
            string code = textBoxInvoiceCustomerNo.Text;

            if (code.Length == 0)
            {
                return;
            }

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select * from customer_table WHERE BCustomerNo='" + code + "'";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                textBoxInvoiceFirstName.Text = dr.GetString(0);
                textBoxInvoiceLastName.Text = dr.GetString(1);
                textBoxInvoiceCustomerNo.Text = dr.GetString(2);
                textBoxInvoiceBusinessName.Text = dr.GetString(3);
                textBoxInvoiceAddress1.Text = dr.GetString(4);
                textBoxInvoiceAddress2.Text = dr.GetString(5);
                textBoxInvoiceCity.Text = dr.GetString(6);
                textBoxInvoiceState.Text = dr.GetString(7);
                textBoxInvoiceCountry.Text = dr.GetString(8);
                textBoxInvoicePinCode.Text = dr.GetString(9);         
                textBoxInvoicePhone.Text = dr.GetString(19);
                textBoxInvoiceEmail.Text = dr.GetString(20);
                textBoxInvoiceFax.Text = dr.GetString(21);
                textBoxInvoiceDiscount.Text = dr.GetString(22);
                textBoxInvoiceMobile.Text = dr.GetString(23);
            }
            con.Close();
        }

        private void buttonResetCustomer_Click(object sender, RoutedEventArgs e)
        {
            clearCustomerRecord();
        }

        private void clearCustomerRecord() 
        {
            textBoxInvoiceFirstName.Text = "";
            textBoxInvoiceLastName.Text = "";
            textBoxInvoiceCustomerNo.Text = "";
            textBoxInvoiceBusinessName.Text = "";
            textBoxInvoiceAddress1.Text = "";
            textBoxInvoiceAddress2.Text = "";
            textBoxInvoiceCity.Text = "";
            textBoxInvoiceState.Text = "";
            textBoxInvoicePinCode.Text = "";
            textBoxInvoiceCountry.Text = "";

            textBoxInvoicePhone.Text = "";
            textBoxInvoiceEmail.Text = "";
            textBoxInvoiceFax.Text = "";
            textBoxInvoiceDiscount.Text = "";
            textBoxInvoiceMobile.Text = "";
        }

        private void ShowShipping()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from configuration_table WHERE Shipping IS NOT NULL";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                if (dr.GetString(2) != null)
                {
                    string Name = dr.GetString(2);
                    comboBoxShippingMethod.Items.Add(Name);
                }
            }
            con1.Close();
        }

        private void ShowInvoiceType()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from configuration_table WHERE InvoiceTerms IS NOT NULL";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                if (dr.GetString(3) != null)
                {
                    string Name = dr.GetString(3);
                    comboBoxInvoiceTerm.Items.Add(Name);
                }
            }
            con1.Close();
        }

        private void ShowPaymentMethod()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from configuration_table WHERE PaymentMethod IS NOT NULL";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                if (dr.GetString(0) != null)
                {
                    string paymentmethod = dr.GetString(0);
                    comboBoxPaymentMethod.Items.Add(paymentmethod);
                }
            }
            con1.Close();
        }

        private void buttonCustomerSearch_Click(object sender, RoutedEventArgs e)
        {
            var newCustomerSearch = new CustomerSearch();

            newCustomerSearch.ShowDialog();

            textBoxInvoiceCustomerNo.Text = newCustomerSearch.CustomerNo; // Get Customer number from CustomerSearch
            Show_Customer();
        }

        private void buttonAddOrderItem_Click(object sender, RoutedEventArgs e)
        {
            string invoiceno = textBoxInvoiceNo.Text;
            addorderiteminvoice.textBoxInvoiceNo.Text = invoiceno;

            addorderiteminvoice.ShowDialog();
            Show_Items();
            Show_Total();
        }

        // Show selected invoice items in DataGrid
        private void Show_Items()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT ItemNo,ItemName,Quantity,UnitPrice,Amount FROM invoiceorderitem_table", con1);
            cmd1.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd1);
            DataTable datatbl = new DataTable("invoiceorderitem_table");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);

            con1.Close();
        }

        private void Show_Total()
        {
            int iTotal = 0;
            string invoiceno = textBoxInvoiceNo.Text;

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "SELECT Amount from invoiceorderitem_table WHERE InvoiceNo='" + invoiceno + "'";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                iTotal = iTotal + Convert.ToInt32(dr.GetString(0));
            }
            textBoxTotal.Text = Convert.ToString(iTotal);
        }

        private void buttonDeleteOrderItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteOrderItem(); // Delete selected order item from datagrid
            Show_Items(); // Shows the updated table after deletion
        }

        /* Delete selected order item from datagrid */
        private void DeleteOrderItem()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "DELETE FROM orderitem_table WHERE (ItemNo='" + sItemNo + "') AND (Quantity='" + sQuantity + "') AND (UnitPrice='" + sUnitPrice + "')", con1);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con1.Close();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = dataGrid1.CurrentCell.Item as DataRowView;
            if (drv != null)
            {
                sItemNo = drv.Row[0].ToString();
                sQuantity = drv.Row[2].ToString();
                sUnitPrice = drv.Row[3].ToString();
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonCancleInvoice_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonCompleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            Add_Invoice_to_DB();
            Reset_Invoice_Fields();
            GenerateInvoiceNo();
        }

        private void Add_Invoice_to_DB()
        {
            string invoiceno = textBoxInvoiceNo.Text;
            string customerno = textBoxInvoiceCustomerNo.Text;
            string discount = textBoxInvoiceDiscount.Text;
            string orderdate = datePickerOrderDate.Text;
            string invoicedate = datePickerInvoiceDate.Text;
            string shippeddate = datePickerShippedDate.Text;
            string invoiceterm = comboBoxInvoiceTerm.Text;
            string shippingmethod = comboBoxShippingMethod.Text;
            string warranty = comboBoxWarrantyInvoice.Text;
            string paymentmethod = comboBoxPaymentMethod.Text;
            string invoicenotes = textBoxInvoiceNotes.Text;
            string shippingcharge = textBoxShipping.Text;
            string total = textBoxTotal.Text;
            string paid = textBoxPaid.Text;
            string owing = textBoxOwing.Text;

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand(
                                "Insert into invoice_table (InvoiceNo,InvoiceCustomerNo,Discount,OrderDate,InvoiceDate,ShippedDate,InvoiceTerm,ShippingMethod,WarrantyInvoice,PaymentMethod,InvoiceNotes,ShippingCharge,Paid,Owing,Total) values('" + invoiceno + "','" + customerno + "','" + discount + "','" + orderdate + "','" + invoicedate + "','" + shippeddate + "','" + invoiceterm + "','" + shippingmethod + "','" + warranty + "','" + paymentmethod + "','" + invoicenotes + "','" + shippingcharge + "','" + total + "','"
                                + paid + "','" + owing + "')", con1);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();
        }

        private void Reset_Invoice_Fields()
        {
            //textBoxInvoiceNo.Text = " ";
            textBoxInvoiceCustomerNo.Text = " ";
            textBoxInvoiceDiscount.Text = " ";

            datePickerOrderDate.SelectedDate = null;
            datePickerOrderDate.DisplayDate = DateTime.Now;

            datePickerInvoiceDate.SelectedDate = null;
            datePickerInvoiceDate.DisplayDate = DateTime.Now;

            datePickerShippedDate.SelectedDate = null;
            datePickerShippedDate.DisplayDate = DateTime.Now;

            //datePickerOrderDate.Text = " ";
            //datePickerInvoiceDate.Text = " ";
            //datePickerShippedDate.Text = " ";
            comboBoxInvoiceTerm.Text = " ";
            comboBoxShippingMethod.Text = " ";
            comboBoxWarrantyInvoice.Text = " ";
            comboBoxPaymentMethod.Text = " ";
            textBoxInvoiceNotes.Text = " ";
            textBoxShipping.Text = " ";
            textBoxTotal.Text = " ";
            textBoxPaid.Text = " ";
            textBoxOwing.Text = " ";

            textBoxInvoiceFirstName.Text = " ";
            textBoxInvoiceLastName.Text = " ";
            textBoxInvoiceBusinessName.Text = " ";
            textBoxInvoiceAddress1.Text = " ";
            textBoxInvoiceAddress2.Text = " ";
            textBoxInvoiceCity.Text = " ";
            textBoxInvoiceState.Text = " ";
            textBoxInvoicePinCode.Text = " ";
            textBoxInvoiceCountry.Text = " ";
            textBoxInvoiceFax.Text = " ";
            textBoxInvoicePhone.Text = " ";
            textBoxInvoiceEmail.Text = " ";
            textBoxInvoiceDiscount.Text = " ";
            textBoxInvoiceCustomerNo.Text = " ";
            textBoxInvoiceMobile.Text = " ";

            dataGrid1.ItemsSource = null;
            dataGrid1.Items.Refresh();
        }

        /* Generates the next Invoice number*/
        private void GenerateInvoiceNo()
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select InvoiceNo from invoice_table";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                iInvoiceNo = Convert.ToInt32(dr.GetString(0));
                tempinvoiceno = dr.GetString(0);
            }

            if (tempinvoiceno == null)
            {
                iInvoiceNo = 1;
                tempinvoiceno = Convert.ToString(iInvoiceNo);
            }
            else
            {
                iInvoiceNo = iInvoiceNo + 1;
                tempinvoiceno = Convert.ToString(iInvoiceNo);
            }

            textBoxInvoiceNo.Text = tempinvoiceno;

            con.Close();
        }

        // Add shipping charge with total and show
        private void textBoxShipping_LostFocus (object sender, RoutedEventArgs e) 
        {
            int iSCharge;
            int iTotal;
            int iGrandTotal;

            iTotal = Convert.ToInt32(textBoxTotal.Text);
            iSCharge = Convert.ToInt32(textBoxShipping.Text);

            iGrandTotal = iTotal + iSCharge;

            textBoxTotal.Text = iGrandTotal.ToString();
        }

        private void textBoxPaid_LostFocus (object sender, RoutedEventArgs e) 
        {
            int iPaid;
            int iOwing;
            int iTotal;

            iTotal = Convert.ToInt32(textBoxTotal.Text);
            iPaid = Convert.ToInt32(textBoxPaid.Text);

            iOwing = iTotal - iPaid;

            textBoxOwing.Text = iOwing.ToString();
        }
    }
}
