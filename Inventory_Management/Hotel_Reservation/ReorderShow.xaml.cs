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
    /// Interaction logic for ReorderShow.xaml
    /// </summary>
    public partial class ReorderShow : Window
    {
        int qty, tempqty = 0;
        string reorderno = "";
        string itemno = "";
        int noinstock = 0;
        //string location = "";
        
        public ReorderShow()
        {
            InitializeComponent();
            ShowItemNo();
        }

        private void ShowItemNo()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from item";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                string Name = dr.GetString(0);
                comboBoxItemNo.Items.Add(Name);
            }
            con1.Close();
        }

        private void buttonStartSearch_Click(object sender, RoutedEventArgs e)
        {
            Show_DataGrid();
        }

        private void Show_DataGrid()
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select ReorderNo,ItemNo,Description,PurchasePrice,OrderedQty,ReceivedQty from reorder";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            cmd.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd);
            DataTable datatbl = new DataTable("reorder");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);
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
                                "UPDATE reorder SET ReceivedQty='" + qty + "' WHERE ReorderNo='" + reorderno + "'", con1);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();
            //MessageBox.Show(" 2222^^^^^^^...'");
          
        }

        private void Insert_UpdateInItem() 
        {
            Get_NoINStock();
            Update_itemDB();
            //MessageBox.Show(" 333$$$$$$...'");
        }

        private void Update_itemDB() 
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "UPDATE item SET NoInStock='" + noinstock + "' WHERE ItemNo='" + itemno + "'", con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void Get_NoINStock() 
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from item WHERE ItemNo='" + itemno + "'";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            dr.Read();
            noinstock = Convert.ToInt32(dr.GetString(14));

            con1.Close();
            noinstock = noinstock + tempqty; // Updating total no in item database
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string rowindex = dataGrid1.SelectedIndex.ToString();
            string Text = "";
            
            DataRowView drv = dataGrid1.CurrentCell.Item as DataRowView;
            if (drv != null)
            {
                Text = drv.Row[5].ToString();
                reorderno = drv.Row[0].ToString();
                itemno = drv.Row[1].ToString();
                qty = Convert.ToInt32(Text);
                if (textBoxQtyReceived.Text != null)
                {
                    tempqty = Convert.ToInt32(textBoxQtyReceived.Text);

                }
                qty = qty + tempqty;
                Insert_UpdatedValue();
                Show_DataGrid();
                
                textBoxQtyReceived.Text = "";
                //MessageBox.Show(" 1111 %%%%%%...'");
            }
        }

        private void buttonUpdateStock_Click(object sender, RoutedEventArgs e)
        {
            Insert_UpdateInItem(); //Updates no instock
            MessageBox.Show(" Stock Quantity Updated...'");
        }
    }
}
