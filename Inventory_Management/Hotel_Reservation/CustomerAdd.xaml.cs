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
    /// Interaction logic for CustomerAdd.xaml
    /// </summary>
    public partial class CustomerAdd : Window
    {
        int icustomerno;
        string tempcustomerno = null;
        
        public CustomerAdd()
        {
            InitializeComponent();
            GenerateCustomerNo();
        }

       /* Resets all customer input fields in Customer Add UI*/
        public void Reset_Customer_Fields() 
        {
            textBoxBillingFirstName.Text = "";
            textBoxBillingLastName.Text = "";
            textBoxBillingCustomerNo.Text = "";
            textBoxBillingBusinessName.Text = "";
            textBoxBillingAddress1.Text = "";
            textBoxBillingAddress2.Text = "";
            textBoxBillingCity.Text = "";
            textBoxBillingState.Text = "";
            textBoxBillingPinCode.Text = "";
            textBoxBillingCountry.Text = "";

            textBoxShippingFirstName.Text = "";
            textBoxShippingLastName.Text = "";
            textBoxShippingBusinessName.Text = "";
            textBoxShippingAddress1.Text = "";
            textBoxShippingAddress2.Text = "";
            textBoxShippingCity.Text = "";
            textBoxShippingState.Text = "";
            textBoxShippingPinCode.Text = "";
            textBoxShippingCountry.Text = "";

            textBoxPhone.Text = "";
            textBoxEmail.Text = "";
            textBoxFax.Text = "";
            textBoxDiscount.Text = "";
            textBoxMobile.Text = "";
            textBoxNotes.Text = "";
        }

        /*Reads all data from Customer Add UI inputs and updates DB (customer_table)*/
        private void Fill_CustomerDetails() 
        {
            string Bfirstname = textBoxBillingFirstName.Text;
            string Blastname = textBoxBillingLastName.Text;
            string Bcustomerno = textBoxBillingCustomerNo.Text;
            string Bbusinessname = textBoxBillingBusinessName.Text;
            string Baddress1 = textBoxBillingAddress1.Text;
            string Baddress2 = textBoxBillingAddress2.Text;
            string Bcity = textBoxBillingCity.Text;
            string Bstate = textBoxBillingState.Text;
            string Bpostcode = textBoxBillingPinCode.Text;
            string Bcountry = textBoxBillingCountry.Text;

            string Sfirstname = textBoxShippingFirstName.Text;
            string Slastname = textBoxShippingLastName.Text;
            string Sbusinessname = textBoxShippingBusinessName.Text;
            string Saddress1 = textBoxShippingAddress1.Text;
            string Saddress2 = textBoxShippingAddress2.Text;
            string Scity = textBoxShippingCity.Text;
            string Sstate = textBoxShippingState.Text;
            string Spostcode = textBoxShippingPinCode.Text;
            string Scountry = textBoxShippingCountry.Text;

            string Aphone = textBoxPhone.Text;
            string Aemail = textBoxEmail.Text;
            string Afax = textBoxFax.Text;
            string Adiscount = textBoxDiscount.Text;
            string Amobile = textBoxMobile.Text;
            string Anotes = textBoxNotes.Text;
            

            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand(
                                "Insert into customer_table (BFirstName,BLastName,BCustomerNo,BBusinessName,BAddress1,BAddress2,BCity,BState,BPin,BCountry,SHFirstName,SHLastName,SHBusinessName,SHAddress1,SHAddress2,SHCity,SHState,SHPin,SHCountry,ADPhone,ADEmail,ADFax,ADDiscount,ADMobile,ADNotes) values('" + Bfirstname + "','" + Blastname + "','" + Bcustomerno + "','" + Bbusinessname + "','" + Baddress1 + "','" + Baddress2 + "','" + Bcity + "','" + Bstate + "','" + Bpostcode + "','" + Bcountry + "','" + Sfirstname + "','" + Slastname + "','" + Sbusinessname + "','" + Saddress1 + "','" + Saddress2 + "','" + Scity + "','" + Sstate + "','" + Spostcode + "','" + Scountry + "','" + Aphone + "','" + Aemail + "','" + Afax + "','" + Adiscount + "','" + Amobile + "','" + Anotes + "')", con1);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();
  
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            Fill_CustomerDetails();
            Reset_Customer_Fields();
            GenerateCustomerNo();
        }

        /* Gets the last Customer Id from vendor_table */
        private void GenerateCustomerNo()
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select * from customer_table";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                icustomerno = Convert.ToInt32(dr.GetString(2));
                tempcustomerno = dr.GetString(2);
            }

            if (tempcustomerno == null)
            {
                icustomerno = 1;
                tempcustomerno = Convert.ToString(icustomerno);
            }
            else
            {
                icustomerno = icustomerno + 1;
                tempcustomerno = Convert.ToString(icustomerno);
            }

            textBoxBillingCustomerNo.Text = tempcustomerno;

            con.Close();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
