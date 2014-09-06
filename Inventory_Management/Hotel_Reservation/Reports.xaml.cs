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

using System.IO;
using Microsoft.Research.DynamicDataDisplay; // Core functionality
using Microsoft.Research.DynamicDataDisplay.DataSources; // EnumerableDataSource
using Microsoft.Research.DynamicDataDisplay.PointMarkers; // CirclePointMarker

using Xceed.Wpf.Toolkit;


namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        public ObservableCollection<BoolStringClass> TheList { get; set; }
        
        public Reports()
        {
            InitializeComponent();
            Show_Report_List();
            
        }

        // added for binding checkbox with listbox
        public class BoolStringClass
        {
            public string TheText { get; set; }
            public int TheValue { get; set; }
        }

        private void Show_Report_List()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from configuration_table WHERE ReportList IS NOT NULL";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            TheList = new ObservableCollection<BoolStringClass>();
            BoolStringClass text = new BoolStringClass();

            while (dr.Read())
            {
                //id = Convert.ToInt32(dr.GetString(0));
                string Name = dr.GetString(4);
                TheList.Add(new BoolStringClass { TheText = Name, TheValue = 1 });
            }

            listBoxReportList.ItemsSource = TheList;
            con1.Close();
        }

        private void Window1_Loaded()
        {
            List<BugInfo> bugInfoList = LoadBugInfo("..\\..\\BugInfo.txt");

            DateTime[] dates = new DateTime[bugInfoList.Count];
            int[] numberOpen = new int[bugInfoList.Count];
            int[] numberClosed = new int[bugInfoList.Count];

            for (int i = 0; i < bugInfoList.Count; ++i)
            {
                dates[i] = bugInfoList[i].date;
                numberOpen[i] = bugInfoList[i].numberOpen;
                numberClosed[i] = bugInfoList[i].numberClosed;
            }

            var datesDataSource = new EnumerableDataSource<DateTime>(dates);
            datesDataSource.SetXMapping(x => dateAxis.ConvertToDouble(x));

            var numberOpenDataSource = new EnumerableDataSource<int>(numberOpen);
            numberOpenDataSource.SetYMapping(y => y);

            var numberClosedDataSource = new EnumerableDataSource<int>(numberClosed);
            numberClosedDataSource.SetYMapping(y => y);

            CompositeDataSource compositeDataSource1 = new
              CompositeDataSource(datesDataSource, numberOpenDataSource);
            CompositeDataSource compositeDataSource2 = new
              CompositeDataSource(datesDataSource, numberClosedDataSource);

            plotter.AddLineGraph(compositeDataSource1,
              new Pen(Brushes.Blue, 2),
              new CirclePointMarker { Size = 10.0, Fill = Brushes.Red },
              new PenDescription("Number bugs open"));


            plotter.AddLineGraph(compositeDataSource2,
              new Pen(Brushes.Green, 2),
              new TrianglePointMarker
              {
                  Size = 10.0,
                  Pen = new Pen(Brushes.Black, 2.0),
                  Fill = Brushes.GreenYellow
              },
              new PenDescription("Number bugs closed"));

            plotter.Viewport.FitToView();

        } // Window1_Loaded()

        private static List<BugInfo> LoadBugInfo(string fileName)
        {
            var result = new List<BugInfo>();
            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] pieces = line.Split(':');
                DateTime d = DateTime.Parse(pieces[0]);
                int numopen = int.Parse(pieces[1]);
                int numclosed = int.Parse(pieces[2]);
                BugInfo bi = new BugInfo(d, numopen, numclosed);
                result.Add(bi);
            }
            sr.Close();
            fs.Close();
            return result;
        }

        private void listBoxReportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoolStringClass selecteditem = (BoolStringClass)listBoxReportList.SelectedItem;
            string tempselitem = "";
            if (selecteditem != null)
            {
                tempselitem = selecteditem.TheText;
            }

            switch (tempselitem) 
            {
                case "Purchase Report":
                    Window1_Loaded();
                    break;
                case "Sales Report":
                    Window1_Loaded();
                    break;
                case "Invoicing Report":
                    Window1_Loaded();
                    break;
                case "Itemwise Stock Report":
                    //Report_Inventory reportinventory = new Report_Inventory();
                    //reportinventory.LoadColumnChartData();
                    break;
            }
        }

    } // class Window1

    public class BugInfo
    {
        public DateTime date;
        public int numberOpen;
        public int numberClosed;

        public BugInfo(DateTime date, int numberOpen, int numberClosed)
        {
            this.date = date;
            this.numberOpen = numberOpen;
            this.numberClosed = numberClosed;
        }
    }
}
