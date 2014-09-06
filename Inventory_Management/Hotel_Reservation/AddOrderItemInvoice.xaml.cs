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
    /// Interaction logic for AddOrderItemInvoice.xaml
    /// </summary>
    public partial class AddOrderItemInvoice : Window
    {
        public AddOrderItemInvoice()
        {
            InitializeComponent();
        }

        private void textBoxItemNo_LostFocus(object sender, RoutedEventArgs e)
        {
            Show_Item();
        }

        private void Show_Item()
        {
            string itemno = textBoxItemNo.Text;

            if (itemno.Length == 0)
            {
                return;
            }

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select * from item WHERE ItemNo='" + itemno + "'";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                textBoxOrderItem.Text = dr.GetString(1);
                textBoxDescription.Text = dr.GetString(2);
                textBoxUnitPrice.Text = dr.GetString(4);
            }
        }

        private void textBoxQuantity_TextChanged(object sender, RoutedEventArgs e)
        {
            string qty = textBoxQuantity.Text;
            int iQty;
            int iUnitPrice;

            if (qty.Length == 0)
            {
                return;
            }

            iQty = Convert.ToInt32(qty);
            iUnitPrice = Convert.ToInt32(textBoxUnitPrice.Text);

            int iAmount = (iQty * iUnitPrice);
            textBoxAmount.Text = Convert.ToString(iAmount);
        }

        private void buttonCloseACWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Clear_Items()
        {
            textBoxInvoiceNo.Text = "";
            textBoxItemNo.Text = "";
            textBoxOrderItem.Text = "";
            textBoxDescription.Text = "";
            textBoxQuantity.Text = "";
            textBoxUnitPrice.Text = "";
            textBoxAmount.Text = "";
        }

        private void buttonAddItem_Click(object sender, RoutedEventArgs e)
        {
            string invoiceno = textBoxInvoiceNo.Text;
            string itemno = textBoxItemNo.Text;
            string orderitem = textBoxOrderItem.Text;
            string description = textBoxDescription.Text;
            string quantity = textBoxQuantity.Text;
            string unitprice = textBoxUnitPrice.Text;
            string amount = textBoxAmount.Text;

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "Insert into invoiceorderitem_table (InvoiceNo,ItemNo,ItemName,Description,Quantity,UnitPrice,Amount) values('" + invoiceno + "','" + itemno + "','" + orderitem + "','" + description + "','" + quantity + "','" + unitprice + "','" + amount + "')", con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();

            Clear_Items();
            textBoxInvoiceNo.Text = invoiceno;
        }
    }
}
