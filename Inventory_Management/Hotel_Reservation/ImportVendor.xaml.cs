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

using Microsoft.Win32; // For OpenFileDialogue
using System.Data.OleDb;

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for ImportVendor.xaml
    /// </summary>
    public partial class ImportVendor : Window
    {
        public ImportVendor()
        {
            InitializeComponent();
        }

        private void buttonChooseFile_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter for file extension and default file extension 
            //openFileDialog1.DefaultExt = ".png";
            //openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"; 


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = openFileDialog1.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true) 
            {
                // Open document 
                string filename = openFileDialog1.FileName;
                textBox_path.Text = filename;
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonLoadFile_Click(object sender, RoutedEventArgs e)
        {
            string PathConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBox_path.Text + ";Extended Properties=\"Excel 8.0;HDR=Yes;\";";
            OleDbConnection conn = new OleDbConnection(PathConn);

            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter("Select * from [" + textBox_sheet.Text + "$]", conn);
            DataTable dt = new DataTable();
            myDataAdapter.Fill(dt);

            dataGrid1.ItemsSource = dt.DefaultView;
            myDataAdapter.Update(dt);
        }
    }
}
