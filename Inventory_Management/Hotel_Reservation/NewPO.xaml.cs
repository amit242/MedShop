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
    /// Interaction logic for NewPO.xaml
    /// </summary>
    public partial class NewPO : Window
    {
        public int iPONo;
        string temppono = null;
        string sItemNo;
        string sQuantity;
        string sUnitPrice;

        AddOrderItem addorderitem = new AddOrderItem(); // For invoking add order item window
              
        public NewPO()
        {
            InitializeComponent();
            GeneratePONo();
            ShowShippingVia();
            PaymentMethod();
            Fill_Ship_To_Address();
        }

        private void textBoxPOVendorNo_LostFocus(object sender, RoutedEventArgs e)
        {
            Show_Vendor();
        }

        private void Show_Vendor() 
        {
            string code = textBoxPOVendorNo.Text;

            if (code.Length == 0)
            {
                return;
            }

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select * from vendor_table WHERE VId='" + code + "'";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                textBoxVendorBusinessName.Text = dr.GetString(2);
                textBoxVendorAddress1.Text = dr.GetString(3);
                textBoxVendorAddress2.Text = dr.GetString(4);
                textBoxVendorCity.Text = dr.GetString(5);
                textBoxVendorState.Text = dr.GetString(6);
                textBoxVendorPinCode.Text = dr.GetString(7);
                textBoxVendorCountry.Text = dr.GetString(8);
                textBoxVendorPhone.Text = dr.GetString(9);
                textBoxVendorFax.Text = dr.GetString(11);
                textBoxVendorEmail.Text = dr.GetString(10);
            }
            con.Close();
            Get_Vendor_Name();
        }

        /* Generates the next PO number*/
        private void GeneratePONo()
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select PONo from po_table";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                iPONo = Convert.ToInt32(dr.GetString(0));
                temppono = dr.GetString(0);
            }

            if (temppono == null)
            {
                iPONo = 1;
                temppono = Convert.ToString(iPONo);
            }
            else
            {
                iPONo = iPONo + 1;
                temppono = Convert.ToString(iPONo);
            }

            textBoxPONumber.Text = temppono;

            con.Close();
        }

        /* Prints the current view */
        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog prDlg = new PrintDialog();
            if (prDlg.ShowDialog() == true)
            {
                prDlg.PrintVisual(__NewPO, "Print PO");
            }
        }

        /* Gets the last vendor Id from vendor_table */
        private void Get_Vendor_Id()
        {
            string tempvendorno = "";

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select VId from vendor_table";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                tempvendorno = dr.GetString(0);
            }
            textBoxPOVendorNo.Text = tempvendorno;
        }

        /* Gets the last vendor Name from vendor_table */
        private void Get_Vendor_Name()
        {
            string tempvendorno = textBoxPOVendorNo.Text;
            string str1 = "";
            string str2 = "";

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "select * from vendor_table WHERE VId='" + tempvendorno + "'";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                str1 = dr.GetString(0);
                str2 = dr.GetString(1);
            }

            string strRes = String.Concat(str1, str2);
            textBoxVendorContactName.Text = strRes;
        }

        /* Closes the current Window */
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonAddVendor_Click(object sender, RoutedEventArgs e)
        {
            var newVendorAdd = new VendorAdd();
            newVendorAdd.ShowDialog();
            Get_Vendor_Id(); // Get the last vendor Id after adding vendor through add vendor window
            Show_Vendor(); // Show the latest vendor entry in Vendor fields of the window
        }

        private void buttonAddOrderItem_Click(object sender, RoutedEventArgs e)
        {
            string pono = textBoxPONumber.Text;
            addorderitem.textBoxPONo.Text = pono;
            
            addorderitem.ShowDialog();
            Show_Items();
            Show_Total();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = dataGrid1.CurrentCell.Item as DataRowView;
            if (drv != null)
            {
                sItemNo = drv.Row[0].ToString();
                sQuantity = drv.Row[2].ToString();
                sUnitPrice = drv.Row[3].ToString();
            }
        }

        private void Show_Items()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();

            MySqlCommand cmd1 = new MySqlCommand("SELECT ItemNo,ItemName,Quantity,UnitPrice,Amount FROM orderitem_table", con1);
            cmd1.ExecuteNonQuery();

            MySqlDataAdapter dataadp = new MySqlDataAdapter(cmd1);
            DataTable datatbl = new DataTable("orderitem_table");
            dataadp.Fill(datatbl);
            dataGrid1.ItemsSource = datatbl.DefaultView;
            dataadp.Update(datatbl);

            con1.Close();
        }

        private void Show_Total()
        {
            int iTotal = 0;
            string pono = textBoxPONumber.Text;

            MySqlConnection con = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con.Open();
            string Query = "SELECT Amount from orderitem_table WHERE PONo='" + pono + "'";
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                iTotal = iTotal + Convert.ToInt32(dr.GetString(0));
            }
            textBoxTotal.Text = Convert.ToString(iTotal);
        }

        private void textBoxTotal_TextChanged(object sender, RoutedEventArgs e) 
        {
            string total = textBoxTotal.Text;
            
            textBoxGrandTotal.Text = total;
        }

        private void textBoxVATPer_LostFocus(object sender, RoutedEventArgs e)
        {
            string total = textBoxTotal.Text;
            int iTotal;
            int iGrandTotal;
            float fVATPer;
            int iVAT;
            int iSalesTax;

            if (total.Length == 0)
            {
                return;
            }
            iTotal = Convert.ToInt32(total);
            iSalesTax = Convert.ToInt32(textBoxSalesTaxAmount.Text);
            fVATPer = Convert.ToSingle(textBoxVATPer.Text);
            iVAT = (int)((iTotal * fVATPer) / 100);
            textBoxVATAmount.Text = iVAT.ToString();
            iGrandTotal = iTotal + iSalesTax + iVAT;
            textBoxGrandTotal.Text = iGrandTotal.ToString();
        }

        private void textBoxSalesTaxPer_LostFocus(object sender, RoutedEventArgs e)
        {
            string total = textBoxTotal.Text;
            int iTotal;
            int iGrandTotal;
            float fSalesTaxPer;
            int iSalesTax;

            if (total.Length == 0)
            {
                return;
            }
            iTotal = Convert.ToInt32(total);
            fSalesTaxPer = Convert.ToSingle(textBoxSalesTaxPer.Text);
            iSalesTax = (int)((iTotal * fSalesTaxPer) / 100);
            textBoxSalesTaxAmount.Text = iSalesTax.ToString();
            iGrandTotal = iTotal + iSalesTax;
            textBoxGrandTotal.Text = iGrandTotal.ToString();
        }

        private void textBoxShippingCharge_LostFocus(object sender, RoutedEventArgs e)
        {
            int iSCharge;
            int iTotal;
            int iSalesTax;
            int iVAT;
            int iGrandTotal;

            iTotal = Convert.ToInt32(textBoxTotal.Text);
            iSalesTax = Convert.ToInt32(textBoxSalesTaxAmount.Text);
            iVAT = Convert.ToInt32(textBoxVATAmount.Text);
            iSCharge = Convert.ToInt32(textBoxShippingCharge.Text);

            iGrandTotal = iTotal + iSalesTax + iVAT + iSCharge;

            textBoxGrandTotal.Text = iGrandTotal.ToString();
            textBoxGrandPOValue.Text = textBoxGrandTotal.Text;
        }

        private void ShowShippingVia()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from configuration_table WHERE Shipping IS NOT NULL";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                if (dr.GetString(2) != null)
                {
                    string Name = dr.GetString(2);
                    comboBoxShippingMethod.Items.Add(Name);
                }
            }
            con1.Close();
        }

        /* Show payment methods from configuration_table*/
        private void PaymentMethod()
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            string Query = "select * from configuration_table WHERE PaymentMethod IS NOT NULL";
            MySqlCommand cmd1 = new MySqlCommand(Query, con1);
            MySqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                if (dr.GetString(0) != null)
                {
                    string Name = dr.GetString(0);
                    comboBoxPaymentMethod.Items.Add(Name);
                }
            }
            con1.Close();
        }

        /* Delete item(s) from item list*/
        private void buttonDeleteOrderItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteOrderItem(); // Delete selected order item from datagrid
            Show_Items(); // Shows the updated table after deletion
            Show_Total(); // Shows the updated total after deletion
        }
        
        /* Delete selected order item from datagrid */
        private void DeleteOrderItem() 
        {
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd = new MySqlCommand(
                                "DELETE FROM orderitem_table WHERE (ItemNo='" + sItemNo + "') AND (Quantity='" + sQuantity + "') AND (UnitPrice='" + sUnitPrice + "')", con1);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con1.Close();
        }

        private void buttonCompletePO_Click(object sender, RoutedEventArgs e)
        {
            textBoxGrandPOValue.Text = textBoxGrandTotal.Text;
            Add_PO_to_DB();
            Reset_PO_Fields();
            GeneratePONo();
        }

        private void buttonCanclePO_Click(object sender, RoutedEventArgs e)
        {
            string pono = textBoxPONumber.Text;

            if (pono != null)
            {
                MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
                con1.Open();
                MySqlCommand cmd = new MySqlCommand(
                                    "DELETE FROM orderitem_table WHERE PONo='" + pono + "'", con1);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con1.Close();
                Close();
            }
            else
                return;
        }

        private void Add_PO_to_DB() 
        {
            string pono = textBoxPONumber.Text;
            string Vno = textBoxPOVendorNo.Text;
            string podate = datePickerPODate.Text;
            string Sdate = datePickerExpectedShippingDate.Text;
            string Sname = textBoxShipName.Text;
            string Saddress1 = textBoxShipAddress1.Text;
            string Saddress2 = textBoxShipAddress2.Text;
            string Scity = textBoxShipCity.Text;
            string Sstate = textBoxShipState.Text;
            string Spin = textBoxShipPinCode.Text;
            string Scountry = textBoxShipCountry.Text;
            string Svia = comboBoxShippingMethod.Text;
            string terms = textBoxPOTerms.Text;
            string ponotes = textBoxPONotes.Text;
            string Pmethod = comboBoxPaymentMethod.Text;
            string total = textBoxTotal.Text;
            string STper = textBoxSalesTaxPer.Text;
            string VATper = textBoxVATPer.Text;
            string st = textBoxSalesTaxAmount.Text;
            string vat = textBoxVATAmount.Text;
            string Scharge = textBoxShippingCharge.Text;
            string Gtotal = textBoxGrandTotal.Text;
            string Sphone = textBoxShipPhone.Text;
            string Sfax = textBoxShipFax.Text;
            string Semail = textBoxShipEMail.Text;
            string Scontactname= textBoxShipContactName.Text;

            
            MySqlConnection con1 = new MySqlConnection("server=localhost;user id=MNath;password=meenakshi@2014;database=inventory");
            con1.Open();
            MySqlCommand cmd1 = new MySqlCommand(
                                "Insert into po_table (PONo,VendorNo,PODate,ShipDate,ShipBusinessName,ShipAddress1,ShipAddress2,ShipCity,ShipState,ShipPin,ShipCountry,ShipVia,Terms,PONotes,PaymentMethod,Total,SalesTaxPer,VATPer,SalesTax,VAT,ShippingCharge,GrandTotal,ShipPhone,ShipFax,ShipEmail,ShipContactName) values('" + pono + "','" 
                                + Vno + "','" + podate + "','" + Sdate + "','" + Sname + "','" + Saddress1 + "','" + Saddress2 + "','" + Scity + "','" + Sstate + "','" + Spin + "','" + Scountry + "','" + Svia + "','" + terms + "','" 
                                + ponotes + "','" + Pmethod + "','" + total + "','" + STper + "','" + VATper + "','" + st + "','" + vat + "','" + Scharge + "','" + Gtotal + "','" + Sphone + "','" + Sfax + "','" + Semail + "','" + Scontactname + "')", con1);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();
        }

        private void Reset_PO_Fields() 
        {
            //textBoxPONumber.Text = "";
            textBoxPOVendorNo.Text = "";
            datePickerPODate.Text = "";
            datePickerExpectedShippingDate.Text = "";
            textBoxShipName.Text = "";
            textBoxShipAddress1.Text = "";
            textBoxShipAddress2.Text = "";
            textBoxShipCity.Text = "";
            textBoxShipState.Text = "";
            textBoxShipPinCode.Text = "";
            textBoxShipCountry.Text = "";
            comboBoxShippingMethod.Text = "";
            textBoxPOTerms.Text = "";
            textBoxPONotes.Text = "";
            comboBoxPaymentMethod.Text = "";
            textBoxTotal.Text = "";
            textBoxSalesTaxPer.Text = "";
            textBoxVATPer.Text = "";
            textBoxSalesTaxAmount.Text = "";
            textBoxVATAmount.Text = "";
            textBoxShippingCharge.Text = "";
            textBoxGrandTotal.Text = "";
            textBoxShipPhone.Text = "";
            textBoxShipFax.Text = "";
            textBoxShipEMail.Text = "";
            textBoxShipContactName.Text = "";

            textBoxVendorBusinessName.Text = "";
            textBoxVendorAddress1.Text = "";
            textBoxVendorAddress2.Text = "";
            textBoxVendorCity.Text = "";
            textBoxVendorState.Text = "";
            textBoxVendorPinCode.Text = "";
            textBoxVendorCountry.Text = "";
            textBoxVendorPhone.Text = "";
            textBoxVendorFax.Text = "";
            textBoxVendorEmail.Text = "";
            textBoxVendorContactName.Text = "";

            textBoxGrandPOValue.Text = "";
        }

        private void Fill_Ship_To_Address() 
        {
            textBoxShipName.Text = "Ma Tara Hosiery";
            textBoxShipAddress1.Text = "31, Ashutosh Mukherjee Road, ";
            textBoxShipAddress2.Text = "Hazra";
            textBoxShipCity.Text = "Kolkata";
            textBoxShipState.Text = "West Bengal";
            textBoxShipPinCode.Text = "700035";
            textBoxShipCountry.Text = "India";
            textBoxShipPhone.Text = "45261452";
            textBoxShipFax.Text = "45123456";
            textBoxShipEMail.Text = "m.t@gmail.com";
            textBoxShipContactName.Text = "Nalin Sarkar";
        }
    }
}
