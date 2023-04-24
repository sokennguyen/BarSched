using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_barber_proto
{
    public partial class CustomerSubpage : UserControl
    {
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        public CustomerSubpage()
        {
            InitializeComponent();
            data.ItemsSource = HairdresserProgram.ListCustomers();
        }
        

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Customer customer;
            if (AddNameBox.Text == "" || AddPhoneBox.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string value = AddNameBox.Text;
            List<Customer> AlteredCustomerList = new List<Customer>();

            var InsertRecord = MessageBox.Show("Do you want to add " + value + " as a new customer?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (InsertRecord == MessageBoxResult.Yes)
            {
                customer = new Customer(null, value, AddPhoneBox.Text);
                AlteredCustomerList = HairdresserProgram.ListCustomers();
                AlteredCustomerList.Add(customer);
                if (HairdresserProgram.SaveCustomerChanges(AlteredCustomerList))
                    data.ItemsSource = HairdresserProgram.ListCustomers();
                else
                {
                    data.ItemsSource = HairdresserProgram.ListCustomers();
                    MessageBox.Show("Something went wrong", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            AddNameBox.Text = "";
            AddPhoneBox.Text = "";
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            bool isComplete = false;
            int countSelected = data.SelectedItems.Count;
            if (data.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + data.SelectedItems.Count + " Customers?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    foreach (var row in data.SelectedItems)
                    {
                        Customer customer = row as Customer;
                        isComplete = HairdresserProgram.DeleteCustomer(customer);
                    }
                    if (isComplete == false)
                        MessageBox.Show("Unable to execute query, remove linked data first to proceed.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    data.ItemsSource = HairdresserProgram.ListCustomers();
                }
            }


        }
        private void data_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Customer cust = e.Row.DataContext as Customer;
            if (HairdresserProgram.UpdateCustomer(cust) == false)
                MessageBox.Show("Unable to execute query, remove linked data first to proceed.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (IdSearchBox.Text == "" && NameSearchBox.Text == "" && PhoneSearchBox.Text == "")
                data.ItemsSource = HairdresserProgram.ListCustomers();
            else
            {
                Customer cust = new Customer(IdSearchBox.Text, NameSearchBox.Text, PhoneSearchBox.Text);
                data.ItemsSource = HairdresserProgram.SearchCustomer(cust);
            }
        }


        //TextBlock solution

        //private void RenderTable()
        //{
        //    int rowIndex = 1;
        //    List<Customer> custList = HairdresserProgram.ListCustomers();
        //    foreach (Customer customer in custList)
        //    {
        //        AddRowDef("table");

        //        if (rowIndex % 2 == 0) AddRowBgColor("green", "table", rowIndex, 3);
        //        else AddRowBgColor("yellow", "table", rowIndex, 3);

        //        AddTextBlock(customer.Id.ToString(), "table", rowIndex, 0);
        //        AddTextBlock(customer.Name.ToString(), "table", rowIndex, 1);
        //        AddTextBlock(customer.Phone.ToString(), "table", rowIndex, 2);
        //        rowIndex++;
        //    }

        //}

        //protected void AddRowDef(string gridName)
        //{
        //    var grid = (Grid)this.FindName(gridName);
        //    RowDefinition RowDef = new RowDefinition();
        //    RowDef.Height = new GridLength(50);
        //    grid.RowDefinitions.Add(RowDef);
        //}
        //protected void AddRowBgColor(string color, string gridName, int rowIndex, int colSpan)
        //{
        //    Rectangle bgRec = new Rectangle();
        //    bgRec.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        //    Grid.SetRow(bgRec, rowIndex);
        //    Grid.SetColumnSpan(bgRec, colSpan);
        //    var grid = (Grid)this.FindName(gridName);
        //    grid.Children.Add(bgRec);
        //}
        //protected void AddTextBlock(string inpString, string gridName, int rowIndex, int colIndex)
        //{
        //    TextBlock textBlock = new TextBlock();
        //    textBlock.Text = inpString;
        //    var grid = (Grid)this.FindName(gridName);
        //    Grid.SetRow(textBlock, rowIndex);
        //    Grid.SetColumn(textBlock, colIndex);
        //    textBlock.VerticalAlignment = VerticalAlignment.Center;
        //    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
        //    grid.Children.Add(textBlock);
        //}
    }
}
