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
using System.Collections.ObjectModel; // added for binding checkbox with listbox

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for Stock_Reduce.xaml
    /// </summary>
    public partial class Stock_Reduce : Window
    {
        int qty = 0;
        string Itemno = "";
        
        public Stock_Reduce()
        {
            InitializeComponent();
            ShowCategory();
            ShowLocation();
            ShowVendor();
        }

        private void buttonStartSearch_Click(object sender, RoutedEventArgs e)
        {
            Show_DataGrid();
        }

        private void Show_DataGrid()
        {
            string itemno = textBoxItemNo.Text;
            string category = comboBoxCategory.Text;
            string location = comboBoxLocation.Text;
            string vendor = comboBoxVendor.Text;

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select ItemNo,Name,Warehouse,Location,PurchasePrice,NoInStock from item WHERE ItemNo='" + itemno + "' OR Category='" + category + "' OR Location='" + location + "' OR Vendor='" + vendor + "'";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            cmd.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd);
            DataTable datatbl = new DataTable("item");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);
        }

        private void ShowCategory()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from category WHERE ParentId IS NULL";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                string Name = dr.GetString(1);
                comboBoxCategory.Items.Add(Name);
            }
            con1.Close();
        }

        private void ShowLocation()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM warehouse_location WHERE ParentId IS NOT NULL", con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                string Name = dr.GetString(1);
                comboBoxLocation.Items.Add(Name);
            }
            con1.Close();
        }

        private void ShowVendor()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from vendor_table";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                string Name = dr.GetString(2);
                comboBoxVendor.Items.Add(Name);
            }
            con1.Close();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Insert_UpdatedValue()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand(
                                "UPDATE item SET NoInStock='" + qty + "' WHERE ItemNo='" + Itemno + "'", con1);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string rowindex = dataGrid1.SelectedIndex.ToString();
            string Text = "";
            int EnteredQty;

            DataRowView drv = dataGrid1.CurrentCell.Item as DataRowView;
            if (drv != null)
            {
                Text = drv.Row[5].ToString();
                Itemno = drv.Row[0].ToString();
            }

            qty = Convert.ToInt32(Text);
            EnteredQty = Convert.ToInt32(textBoxQtyReceived.Text);

            if (qty != 0 && qty > EnteredQty)
            {
                qty = qty - EnteredQty;
            }
            else
            {
                MessageBox.Show("Can not reduce stock as it is zero or Please enter a value less than stock value...'");
               
            }

            Insert_UpdatedValue();
            Show_DataGrid();
            textBoxQtyReceived.Text = "";
        }
    }
}
