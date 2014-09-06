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
    /// Interaction logic for InventoryAdd.xaml
    /// </summary>
    public partial class InventoryAdd : Window
    {
        public ObservableCollection<BoolStringClass> TheList { get; set; }
        int tempcategoryid = 0;
        int tempwarehouseid = 0;

        public InventoryAdd()
        {
            InitializeComponent();
            ShowCategory();
            ShowTax();
            ShowVendor();
            ShowWarehouse();
        }

        // added for binding checkbox with listbox
        public class BoolStringClass
        {
            public string TheText { get; set; }
            public int TheValue { get; set; }
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

        private void list_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ShowSubCategory();
        }

        private void getparentid()
        {
            string category = "";
            string tempcategory = (string)comboBoxCategory.SelectedItem;
            if (tempcategory != null)
            {
                category = tempcategory;
            }

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM category WHERE Name='" + category + "'", con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            if (tempcategory != null)
            {
                dr.Read();
                tempcategoryid = Convert.ToInt32(dr.GetString(0));
            }
            con1.Close();
        }

        private void ShowSubCategory()
        {
            getparentid();

            string tempcategoryidstring = Convert.ToString(tempcategoryid);

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM category WHERE ParentId='" + tempcategoryidstring + "'", con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                string Name = dr.GetString(1);
                comboBoxSubCategory.Items.Add(Name);
            }
            con1.Close();
        }

        private void ShowTax()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from taxname";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                string Name = dr.GetString(0);
                comboBoxTaxRate.Items.Add(Name);
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

        private void ShowWarehouse()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from warehouse_location WHERE ParentId IS NULL";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                string Name = dr.GetString(1);
                comboBoxWarehouse.Items.Add(Name);
            }
            con1.Close();
        }

        private void ShowLocation()
        {
            getlocationparentid();

            string tempwarehouseidstring = Convert.ToString(tempwarehouseid);
            
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM warehouse_location WHERE ParentId='" + tempwarehouseidstring + "'", con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                string Name = dr.GetString(1);
                comboBoxLocation.Items.Add(Name);
            }
            con1.Close();
        }

        private void getlocationparentid()
        {
            string warehouse = "";
            string tempwarehouse = (string)comboBoxWarehouse.SelectedItem;
            if (tempwarehouse != null)
            {
                warehouse = tempwarehouse;
            }

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM warehouse_location WHERE Name='" + warehouse + "'", con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            if (tempwarehouse != null)
            {
                dr.Read();
                tempwarehouseid = Convert.ToInt32(dr.GetString(0));
            }
            con1.Close();
        }

        private void list_SelectionChanged_Warehouse(object sender, RoutedEventArgs e)
        {
            ShowLocation();
        }

        public void Reset_Inventory_Fields()
        {
            textBoxItemNo.Text = "";
            textBoxName.Text = "";
            textBoxLongDescription.Text = "";
            textBoxSalesPrice.Text = "";
            textBoxPurchasePrice.Text = "";
            comboBoxTaxRate.Text = "";
            comboBoxCategory.Text = "";
            comboBoxSubCategory.Text = "";
            comboBoxVendor.Text = "";
            comboBoxWarehouse.Text = "";
            comboBoxLocation.Text = "";
            textBoxReorderLevel.Text = "";
            textBoxReorderPoint.Text = "";
        }

        private void Fill_Inventory_Details()
        {
            string itemno = textBoxItemNo.Text;
            string name = textBoxName.Text;
            string description = textBoxLongDescription.Text;
            string salesprice = textBoxSalesPrice.Text;
            string purchaseprice = textBoxPurchasePrice.Text;
            string taxrate = comboBoxTaxRate.Text;
            string category = comboBoxCategory.Text;
            string subcategory = comboBoxSubCategory.Text;
            string vendor = comboBoxVendor.Text;
            string warehouse = comboBoxWarehouse.Text;
            string location = comboBoxLocation.Text;
            string reorderlevel = textBoxReorderLevel.Text;
            string reorderpoint = textBoxReorderPoint.Text;
            string minstockllevel = "0";
            string noinstock = "0";

            
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand(
                                "Insert into item (ItemNo,Name,Description,SalesPrice,PurchasePrice,TaxRate,Category,SubCategory,Vendor,Warehouse,Location,ReorderLevel,ReorderPoint,MinStockLevel,NoInStock) values('" + itemno + "','" + name + "','" + description + "','" + salesprice + "','" + purchaseprice + "','" + taxrate + "','" + category + "','" + subcategory + "','" + vendor + "','" + warehouse + "','" + location + "','" + reorderlevel + "','" + reorderpoint + "','" + minstockllevel + "','" + noinstock + "')", con1);
            
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();

        }

        private void buttonCloseInvWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonOK_Click_1(object sender, RoutedEventArgs e)
        {
            Fill_Inventory_Details();
            Reset_Inventory_Fields();
        }
    }
}
