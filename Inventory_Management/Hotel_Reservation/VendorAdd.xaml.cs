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
    /// Interaction logic for VendorAdd.xaml
    /// </summary>
    public partial class VendorAdd : Window
    {
        int iVendorId;
        string tempvendorid = null;
        
        public VendorAdd()
        {
            InitializeComponent();
            GenerateVendorId();
        }

        public void Reset_Vendor_Fields() 
        {
            textBoxVendorFirstName.Text = "";
            textBoxVendorLastName.Text = "";
            textBoxVendorBusinessName.Text = "";
            textBoxVendorAddress1.Text = "";
            textBoxVendorAddress2.Text = "";
            textBoxVendorCity.Text = "";
            textBoxVendorState.Text = "";
            textBoxVendorPinCode.Text = "";
            textBoxVendorCountry.Text = "";
            textBoxVendorPhone.Text = "";
            textBoxVendorEmail.Text = "";
            textBoxVendorFax.Text = "";
            textBoxVendorMobile.Text = "";
            textBoxVendorNotes.Text = "";
            textBoxVendorId.Text = "";
        }

        private void Fill_VendorDetails() 
        {
            string Vfirstname = textBoxVendorFirstName.Text;
            string Vlastname = textBoxVendorLastName.Text;
            string Vbusinessname = textBoxVendorBusinessName.Text;
            string Vaddress1 = textBoxVendorAddress1.Text;
            string Vaddress2 = textBoxVendorAddress2.Text;
            string Vcity = textBoxVendorCity.Text;
            string Vstate = textBoxVendorState.Text;
            string Vpostcode = textBoxVendorPinCode.Text;
            string Vcountry = textBoxVendorCountry.Text;
            string Vphone = textBoxVendorPhone.Text;
            string Vemail = textBoxVendorEmail.Text;
            string Vfax = textBoxVendorFax.Text;
            string Vmobile = textBoxVendorMobile.Text;
            string Vnotes = textBoxVendorNotes.Text;
            string Vid = textBoxVendorId.Text;

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand(
                                "Insert into vendor_table (VId,VFirstName,VLastName,VBusinessName,VAddress1,VAddress2,VCity,VState,VPin,VCountry,VPhone,VEmail,VFax,VMobile,VNotes) values('" + Vid + "','" + Vfirstname + "','" + Vlastname + "','" + Vbusinessname + "','" + Vaddress1 + "','" + Vaddress2 + "','" + Vcity + "','" + Vstate + "','" + Vpostcode + "','" + Vcountry + "','" + Vphone + "','" + Vemail + "','" + Vfax + "','" + Vmobile + "','" + Vnotes + "')", con1);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();
        }

        private void GenerateVendorId()
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select VId from vendor_table";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                iVendorId = Convert.ToInt32(dr.GetString(0));
                tempvendorid = dr.GetString(0);
            }

            if (tempvendorid == null)
            {
                iVendorId = 1;
                tempvendorid = Convert.ToString(iVendorId);
            }
            else
            {
                iVendorId = iVendorId + 1;
                tempvendorid = Convert.ToString(iVendorId);
            }

            textBoxVendorId.Text = tempvendorid;

            con.Close();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            Fill_VendorDetails();
            Reset_Vendor_Fields();
            GenerateVendorId();
        }

        private void buttonCancle_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
