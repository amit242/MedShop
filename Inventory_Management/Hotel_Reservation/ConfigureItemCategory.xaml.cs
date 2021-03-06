﻿using System;
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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Collections.ObjectModel; // added for binding checkbox with listbox

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for ItemCategoryConfig.xaml
    /// </summary>
    public partial class ItemCategoryConfig : Window
    {
        public ObservableCollection<BoolStringClass> TheList { get; set; } // added for binding checkbox with listbox 
        
        public ItemCategoryConfig()
        {
            InitializeComponent();
            ShowCategory();
        }

        private void ResetAddPaymentMethod()
        {
            textBoxAddCategory.Text = "";
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
            string Query = "select * from configuration_table";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            TheList = new ObservableCollection<BoolStringClass>();
            BoolStringClass text = new BoolStringClass();

            while (dr.Read())
            {
                string PaymentMethod = dr.GetString(0);
                TheList.Add(new BoolStringClass { TheText = PaymentMethod, TheValue = 1 });
            }

            listBoxAddCategory.ItemsSource = TheList;

            con1.Close();
        }

        private void buttonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            string category = textBoxAddCategory.Text;
            //listBoxPayMethod.Items.Add(textBoxAddPayMethod.Text);
            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "Insert into configuration_table (PaymentMethod) values('" + category + "')", con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            ResetAddPaymentMethod();

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from configuration_table";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            TheList = new ObservableCollection<BoolStringClass>();
            BoolStringClass text = new BoolStringClass();

            while (dr.Read())
            {
                string PaymentMethod = dr.GetString(0);
                TheList.Add(new BoolStringClass { TheText = PaymentMethod, TheValue = 1 });
            }

            listBoxAddCategory.ItemsSource = TheList;

            con1.Close(); 
        }
    }
}
