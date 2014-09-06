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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Collections.ObjectModel; // added for binding checkbox with listbox

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for ConfigureWarehouse.xaml
    /// </summary>
    public partial class ConfigureWarehouse : Window
    {
        public ObservableCollection<BoolStringClass> TheList { get; set; }
        public ObservableCollection<BoolStringClass> TheList1 { get; set; }
        int id = 0;
        int tempid = 0;
        int parentid = 0;
        int lastid = 0;

        public ConfigureWarehouse()
        {
            InitializeComponent();
            ShowWarehouse();
        }

        private void ResetAddWarehouse()
        {
            textBoxAddWarehouse.Text = "";
        }

        private void ResetRemoveWarehouse()
        {
            textBoxRemoveWarehouse.Text = "";
        }

        // added for binding checkbox with listbox
        public class BoolStringClass
        {
            public string TheText { get; set; }
            public int TheValue { get; set; }
        }

        private void ShowWarehouse()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from warehouse_location WHERE ParentId IS NULL";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            TheList = new ObservableCollection<BoolStringClass>();
            BoolStringClass text = new BoolStringClass();

            while (dr.Read())
            {
                //id = Convert.ToInt32(dr.GetString(0));
                string Name = dr.GetString(1);
                TheList.Add(new BoolStringClass { TheText = Name, TheValue = 1 });
            }

            listBoxAddWarehouse.ItemsSource = TheList;
            con1.Close();
        }

        private void buttonAddWarehouse_Click(object sender, RoutedEventArgs e)
        {
            string warehouse = textBoxAddWarehouse.Text;
            ShowLastId();
            id = lastid + 1;

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "Insert into warehouse_location (Id,Name) values('" + id + "','" + warehouse + "')", con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            ResetAddWarehouse();
            ShowWarehouse();            
        }

        private void buttonRemoveWarehouse_Click(object sender, RoutedEventArgs e)
        {
            string warehouse = textBoxRemoveWarehouse.Text;
            string removeparentid = "";

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();

            string Query = "select * from warehouse_location WHERE Name='" + warehouse + "'";
            MySqlCommand cmd1 = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                //removeparentid = Convert.ToInt32(dr.GetString(0));
                removeparentid = dr.GetString(0);
            }
            con.Close();

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "DELETE FROM warehouse_location WHERE Name='" + warehouse + "' OR ParentId='" + removeparentid + "'", con1);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con1.Close();
            ResetRemoveWarehouse();
            ShowWarehouse();
            ShowLocation();
        }

        private void list_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ShowLocation();
        }

        private void ResetAddLocation()
        {
            textBoxAddLocation.Text = "";
        }

        private void ShowLocation()
        {
            getparentid();

            string tempidstring = Convert.ToString(tempid);
            
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM warehouse_location WHERE ParentId='" + tempidstring + "'", con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            TheList1 = new ObservableCollection<BoolStringClass>();
            BoolStringClass text = new BoolStringClass();

            while (dr.Read())
            {
                //parentid = Convert.ToInt32(dr.GetString(2));
                string Name = dr.GetString(1);
                TheList1.Add(new BoolStringClass { TheText = Name, TheValue = 1 });
            }

            listBoxAddLocation.ItemsSource = TheList1;

            con1.Close();
        }

        private void getparentid()
        {
            string warehouse = "";
            //string location = textBoxAddLocation.Text;
            BoolStringClass tempwarehouse = (BoolStringClass)listBoxAddWarehouse.SelectedItem;
            if (tempwarehouse != null)
            {
                warehouse = tempwarehouse.TheText;
            }
            //MessageBox.Show(warehouse);
            
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM warehouse_location WHERE Name='" + warehouse + "'", con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            if (tempwarehouse != null)
            {
                dr.Read();
                tempid = Convert.ToInt32(dr.GetString(0));
            }
            con1.Close();
        }

        private void buttonAddLocation_Click(object sender, RoutedEventArgs e)
        {
            string location = textBoxAddLocation.Text;
            ShowLastId();
            id = lastid + 1;
            parentid = tempid;
            
            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "Insert into warehouse_location (Id,Name,ParentId) values('" + id + "','" + location + "','" + parentid + "')", con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            ResetAddLocation();

            ShowLocation();
        }

        private void ShowLastId()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from warehouse_location";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                lastid = Convert.ToInt32(dr.GetString(0));
            }
            con1.Close();
        }

        private void buttonCloseCfgWarehouse_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonRemoveLocation_Click(object sender, RoutedEventArgs e)
        {
            string location = textBoxRemoveLocation.Text;
           
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "DELETE FROM warehouse_location WHERE Name='" + location + "'", con1);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con1.Close();
            ResetRemoveLocation();
            ShowLocation();
        }

        private void ResetRemoveLocation()
        {
            textBoxRemoveLocation.Text = "";
        }
    }
}
