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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory_Management.View
{
    /// <summary>
    /// Interaction logic for BaseMaster.xaml
    /// </summary>
    public partial class BaseMaster : UserControl
    {
        public BaseMaster()
        {
            InitializeComponent();
        }

        private void buttonCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newCustomerWindow = new Customer();
            newCustomerWindow.Show();
            //MessageBox.Show("You clicked Customer Button...'");
        }

        private void buttonInvoice_Click(object sender, RoutedEventArgs e)
        {
            var newInvoiceWindow = new Invoice();
            newInvoiceWindow.Show();

            //MessageBox.Show("You clicked Invoice Button...");
        }

        private void buttonInventory_Click(object sender, RoutedEventArgs e)
        {
            var newInventoryWindow = new Inventory();
            newInventoryWindow.Show();
            //MessageBox.Show("You clicked Inventory Button...'");
        }

        private void buttonReports_Click(object sender, RoutedEventArgs e)
        {
            var newReportsWindow = new Reports();
            newReportsWindow.Show();
            //MessageBox.Show("You clicked Reports Button...'");
        }

        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            var newFindWindow = new Find();
            newFindWindow.Show();
            //MessageBox.Show("You clicked Find Button...'");
        }

        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You clicked Logout Button...'");
            Application.Current.Shutdown();
        }

        private void ExitApplication_Click(object sender, RoutedEventArgs e) 
        {
            MessageBox.Show("Click OK to confirm Application Exit...'");
            Application.Current.Shutdown();
        }

        private void buttonShowInvoice_Click(object sender, RoutedEventArgs e)
        {
            var newShowInvoiceWindow = new InvoiceShow();
            newShowInvoiceWindow.Show();
            //MessageBox.Show("You clicked Show Invoice Button...'");
        }

        private void buttonCustomerAdd_Click(object sender, RoutedEventArgs e)
        {
            var newCustomerAdd = new CustomerAdd();
            newCustomerAdd.Show();
            //MessageBox.Show("You clicked Add Customer Button...'");
        }

        private void buttonCustomerFind_Click(object sender, RoutedEventArgs e)
        {
            var newCustomerFind = new Find();
            newCustomerFind.Show();
            //MessageBox.Show("You clicked Find Customer Button...'");
        }


        private void buttonVendorAdd_Click(object sender, RoutedEventArgs e)
        {
            var newVendorAdd = new VendorAdd();
            newVendorAdd.Show();
            //MessageBox.Show("You clicked Add Vendor Button...'");
        }

        private void buttonPaymentMethodAdd_Click(object sender, RoutedEventArgs e) 
        {
            var newConfigurePaymentMethod = new ConfigurePaymentMethod();
            newConfigurePaymentMethod.Show();
            //MessageBox.Show("You clicked ConfigurePaymentMethod Button...'");
        }

        private void buttonItemCategoryAdd_Click(object sender, RoutedEventArgs e)
        { 
        }

        private void buttonItemLocationsandWarehousesAdd_Click(object sender, RoutedEventArgs e)
        {
            var newAddWarehouse = new ConfigureWarehouse();
            newAddWarehouse.Show();
            //MessageBox.Show("You clicked Add Warehouse Button...'");
        }

        private void buttonAddInventory_Click(object sender, RoutedEventArgs e)
        {
            var newAddInventory = new InventoryAdd();
            newAddInventory.Show();
            //MessageBox.Show("You clicked Add Inventory Button...'");
        }

        private void buttonQuote_Click(object sender, RoutedEventArgs e)
        {
            var newCreateQuotation = new QuotationCreate();
            newCreateQuotation.Show();
            //MessageBox.Show("You clicked New Quotation Button...'");
        }

        private void buttonStock_Receive_Click(object sender, RoutedEventArgs e)
        {
            var newStockReceive = new Stock_Receive();
            newStockReceive.Show();
            //MessageBox.Show("You clicked New Stock Receive Button...'");
        }

        private void buttonStock_Reduce_Click(object sender, RoutedEventArgs e)
        {
            var newStockReduce = new Stock_Reduce();
            newStockReduce.Show();
            //MessageBox.Show("You clicked New Stock Reduce Button...'");
        }

        private void buttonNewReorder_Click(object sender, RoutedEventArgs e)
        {
            var newReorderNew = new ReorderNew();
            newReorderNew.Show();
            //MessageBox.Show("You clicked New Reorder Button...'");
        }

        private void buttonShowReorder_Click(object sender, RoutedEventArgs e)
        {
            var newReorderShow = new ReorderShow();
            newReorderShow.Show();
            //MessageBox.Show("You clicked Show Reorder...'");
        }

        private void buttonVendorView_Click(object sender, RoutedEventArgs e)
        {
            var newVendorView = new VendorView();
            newVendorView.Show();
            //MessageBox.Show("You clicked Show Vendor...'");
        }

        private void buttonNewPO_Click(object sender, RoutedEventArgs e) 
        {
            var newPOView = new NewPO();
            newPOView.Show();
            //MessageBox.Show("You clicked New PO...'");
        }

        private void buttonPurchaseReport_Click(object sender, RoutedEventArgs e)
        {
            //var newPurchaseReport = new ReportPurchase();
            //newPurchaseReport.Show();
        }

        private void buttonInventoryReport_Click(object sender, RoutedEventArgs e)
        {
            var newReportInventory = new Report_Inventory();
            if (newReportInventory != null)
            newReportInventory.Show();
        }

        // Launch Find Vendor window from Vendors MenuItem
        private void buttonVendorFind_Click(object sender, RoutedEventArgs e)
        {
            var newVendorFind = new VendorFind();
            if (newVendorFind != null)
            newVendorFind.Show();
        }

        private void buttonOrderReceive_Click(object sender, RoutedEventArgs e)
        {
            var newOrderReceive = new OrderReceive();
            if (newOrderReceive != null)
            newOrderReceive.Show();
        }

        private void buttonExportInvoice_Click(object sender, RoutedEventArgs e)
        {
            var newExportInvoice = new ExportInvoice();
            if (newExportInvoice != null)
            newExportInvoice.Show();
        }

        private void buttonExportVendor_Click(object sender, RoutedEventArgs e)
        {
            var newExportVendor = new ExportVendor();
            if (newExportVendor != null)
                newExportVendor.Show();
        }

        private void buttonImportVendor_Click(object sender, RoutedEventArgs e)
        {
            var newImportVendor = new ImportVendor();
            if (newImportVendor != null)
                newImportVendor.Show();
        }

        private void buttonFindInvoice_Click (object sender, RoutedEventArgs e)
        {
            var newInvoiceFind = new InvoiceFind();
            if (newInvoiceFind != null)
                newInvoiceFind.Show();
        }
    }
}
