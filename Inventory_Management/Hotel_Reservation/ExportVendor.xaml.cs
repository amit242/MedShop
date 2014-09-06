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

using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.SpreadSheet;
using ExcelLibrary.BinaryDrawingFormat;
using ExcelLibrary.BinaryFileFormat;

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for ExportVendor.xaml
    /// </summary>
    public partial class ExportVendor : Window
    {
        public ExportVendor()
        {
            InitializeComponent();
        }

        private void buttonLoadVendor_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM vendor_table", con1);
            cmd1.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd1);
            DataTable datatbl = new DataTable("vendor_table");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);

            DataSet ds = new DataSet("Vendor_Dataset");
            ds.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
            dataadp.Fill(datatbl);
            ds.Tables.Add(datatbl);
            ExcelLibrary.DataSetHelper.CreateWorkbook("Vendor.xls", ds);

            con1.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
