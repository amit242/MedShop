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
    /// Interaction logic for VendorView.xaml
    /// </summary>
    public partial class VendorView : Window
    {
        string sBusinessName = "";
        string sRowIndex = "";
        string sHeaderName = "";
        string sUpdatedText = "";

        public VendorView()
        {
            InitializeComponent();
            Show_Vendor();
        }

        private void Show_Vendor()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT VFirstName,VLastName,VBusinessName,VAddress1,VAddress2,VCity,VState,VPin,VCountry,VPhone,VEmail,VFax,VMobile,VNotes FROM vendor_table", con1);
            cmd1.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd1);
            DataTable datatbl = new DataTable("vendor_table");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);

            con1.Close();
        }



        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sRowIndex = dataGrid1.SelectedIndex.ToString();
            
            DataRowView drv = dataGrid1.CurrentCell.Item as DataRowView;
            if (drv != null)
            {
                sBusinessName = drv.Row[2].ToString();
            }
            
        }

        private void buttonDeleteVendor_Click(object sender, RoutedEventArgs e)
        {
            string vendorbusinessname = sBusinessName;

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "DELETE FROM vendor_table WHERE VBusinessName='" + vendorbusinessname + "'", con1);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con1.Close();
            Show_Vendor();
        }

        private void buttonAddVendor_Click(object sender, RoutedEventArgs e)
        {
            var newVendorAdd = new VendorAdd();
            newVendorAdd.ShowDialog();
            Show_Vendor();
        }

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) 
        {
            var element = e.EditingElement as TextBox;

            sUpdatedText = element.Text;
            sHeaderName = e.Column.Header.ToString();

            Insert_UpdatedValue();
            Show_Vendor();
        }

        private void Insert_UpdatedValue()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand(
                                "UPDATE vendor_table SET " + sHeaderName + "='" + sUpdatedText + "' WHERE VBusinessName='" + sBusinessName + "'", con1);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();
        }

    }
}
