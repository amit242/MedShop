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
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {
        string sCustomerNo;
        
        public Customer()
        {
            InitializeComponent();
            Show_Customer();
        }

        private void Show_Customer() 
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT BCustomerNo,BFirstName,BLastName,BBusinessName,BAddress1,BAddress2,BCity,BState,BPin,ADPhone FROM customer_table", con1);
            cmd1.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd1);
            DataTable datatbl = new DataTable("customer_table");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);

            con1.Close();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newCustomerAdd = new CustomerAdd();
            newCustomerAdd.Show();
        }

        private void buttonDeleteCustomers_Click(object sender, RoutedEventArgs e)
        {
            string customerno = sCustomerNo;

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "DELETE FROM customer_table WHERE BCustomerNo='" + customerno + "'", con1);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con1.Close();
            Show_Customer();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataRowView drv = dataGrid1.CurrentCell.Item as DataRowView;
            if (drv != null)
            {
                sCustomerNo = drv.Row[2].ToString();
            }
        }
    }
}
