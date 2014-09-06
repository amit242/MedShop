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
    /// Interaction logic for CustomerSearch.xaml
    /// </summary>
    public partial class CustomerSearch : Window
    {
        public string CustomerNo;
        
        public CustomerSearch()
        {
            InitializeComponent();
        }

        private void Find_Customer()
        {
            string FName = textBoxFirstName.Text;
            string LName = textBoxLastName.Text;
            string CRN = textBoxCustomerNo.Text;
            string BusinessName = textBoxBusinessName.Text;

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM customer_table WHERE ((BFirstName='" + FName + "') OR (BLastName='" + LName + "') OR (BCustomerNo='" + CRN + "') OR (BBusinessName='" + BusinessName + "'))", con1);
            cmd1.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd1);
            DataTable datatbl = new DataTable("customer_table");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);

            con1.Close();
        }

        private void buttonCustomerSearch_Click(object sender, RoutedEventArgs e)
        {
            Find_Customer();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sRowIndex = dataGrid1.SelectedIndex.ToString();

            DataRowView DRowView = dataGrid1.CurrentCell.Item as DataRowView;

            if (DRowView != null)
            {
                CustomerNo = DRowView.Row[2].ToString();
            }

            textBoxSelectedCusNo.Text = CustomerNo;
        }

    }
}
