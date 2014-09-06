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

using System.Windows.Controls.DataVisualization.Charting;

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for Report_Inventory.xaml
    /// </summary>
    public partial class Report_Inventory : Window
    {
        public ObservableCollection<BoolStringClass> TheList { get; set; }
        
        public Report_Inventory()
        {
            InitializeComponent();
            LoadColumnChartData();
        }

        // added for binding checkbox with listbox
        public class BoolStringClass
        {
            public string TheText { get; set; }
            public int TheValue { get; set; }
        }

        public void LoadColumnChartData()
        {

            /*((BarSeries)MyChart.Series[0]).ItemsSource =
                new KeyValuePair<string, int>[]{
                new KeyValuePair<string,int>("Project Manager", 12),
                new KeyValuePair<string,int>("CEO", 25),
                new KeyValuePair<string,int>("Software Engg.", 5),
                new KeyValuePair<string,int>("Team Leader", 6),
                new KeyValuePair<string,int>("Project Leader", 10),
                new KeyValuePair<string,int>("Developer", 4) };*/

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from item";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            TheList = new ObservableCollection<BoolStringClass>();
            BoolStringClass text = new BoolStringClass();

            while (dr.Read())
            {
                string Name = dr.GetString(1);
                int value = Convert.ToInt32(dr.GetString(14));
                TheList.Add(new BoolStringClass { TheText = Name, TheValue = value });
            }

            ((ColumnSeries)MyChart.Series[0]).ItemsSource = TheList;
            con1.Close();

        }

        private void buttonPrintInvoice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
