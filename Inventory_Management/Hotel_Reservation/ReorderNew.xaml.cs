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
    /// Interaction logic for ReorderNew.xaml
    /// </summary>
    public partial class ReorderNew : Window
    {
        int iReorderNo;
        int ipono;
        string temppono = null;

        string itemno = "";
        string vendor = "";
        string purchaseprice = "";

        public ReorderNew()
        {
            InitializeComponent();
            GenerateReorderNo();
        }

        private void Clear_NewRecord()
        {
            textBoxReorderNo.Text = "";
            datePickerSelectDate.Text = "";
            textBoxItemNo.Text = "";
            textBoxReorderQty.Text = "";
            textBoxVendor.Text = "";
            textBoxPurchasePrice.Text = "";
            textBoxDescription.Text = "";
        }

        private void Add_NewReorder()
        {
            string reorderno = textBoxReorderNo.Text;
            string reorderdate = datePickerSelectDate.Text;
            string itemno = textBoxItemNo.Text;
            string reorderqty = textBoxReorderQty.Text;
            string vendor = textBoxVendor.Text;
            string purchaseprice = textBoxPurchasePrice.Text;
            string description = textBoxDescription.Text;
            int receivedqty = 0;

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            //string query = "Insert into reorder (ReorderNo,ReorderDate,ItemNo,OrderedQty,Vendor,PurchasePrice,Description,ReceivedQty,PONo) values('" + reorderno + "','" + reorderdate + "','" + itemno + "','" + reorderqty + "','" + vendor + "','" + purchaseprice + "','" + description + "','" + receivedqty + "','" + ipono + "')";

            MySqlCommand cmd1 = new MySqlCommand("Insert into reorder (ReorderNo,ReorderDate,ItemNo,OrderedQty,Vendor,PurchasePrice,Description,ReceivedQty,PONo) values('" + reorderno + "','" + reorderdate + "','" + itemno + "','" + reorderqty + "','" + vendor + "','" + purchaseprice + "','" + description + "','" + receivedqty + "','" + ipono + "')", con1);

            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
     
        private void buttonGetReorder_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT ItemNo,Name,PurchasePrice,Vendor,ReorderLevel,NoInStock FROM item WHERE ReorderLevel >= NoInStock", con);
            cmd.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd);
            DataTable datatbl = new DataTable("item");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);

            con.Close();
        }

            private void GenerateReorderNo()
            {
                MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
                con.Open();
                string Query = "select ReorderNo from reorder";
                MySqlCommand cmd = new MySqlCommand(Query, con);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    iReorderNo = Convert.ToInt32(dr.GetString(0));
                    temppono = dr.GetString(0);
                }

                if (temppono == null)
                {
                    iReorderNo = 1;
                    temppono = Convert.ToString(iReorderNo);
                }
                else
                {
                    iReorderNo = iReorderNo + 1;
                    temppono = Convert.ToString(iReorderNo);
                }

                textBoxReorderNo.Text = temppono;

                con.Close();
            }

            private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                string sRowIndex = dataGrid1.SelectedIndex.ToString();
        
                DataRowView DRowView = dataGrid1.CurrentCell.Item as DataRowView;
                            
                 if (DRowView != null)
                 {
                     itemno = DRowView.Row[0].ToString();
                     vendor = DRowView.Row[3].ToString();
                     purchaseprice = DRowView.Row[2].ToString();
                 }

                                
                 textBoxItemNo.Text = itemno;
                 textBoxVendor.Text = vendor;
                 textBoxPurchasePrice.Text = purchaseprice;
            }

            private void buttonCreatePO_Click(object sender, RoutedEventArgs e)
            {
                var newNewPo = new NewPO();

                //newNewPo.textBoxPOVendorNo.Text = textBoxVendor.Text;
                               
                newNewPo.ShowDialog();

                ipono = newNewPo.iPONo; // Get PO number from NewPO
                Add_NewReorder();
                Clear_NewRecord();
            }

    }
}

