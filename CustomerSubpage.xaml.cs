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
        public CustomerSubpage()
        {
            InitializeComponent();
            data.ItemsSource = HairdresserProgram.ListCustomers();
        }
        HairdresserProgram HairdresserProgram = new HairdresserProgram();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Staff staff;
            string value = AddPhoneBox.Text;
            List<Staff> AlteredStaffList = new List<Staff>();

            var InsertRecord = MessageBox.Show("Do you want to add " + value + " as a new staff?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (InsertRecord == MessageBoxResult.Yes)
            {
                staff = new Staff(null, value);
                AlteredStaffList = HairdresserProgram.ListStaff();
                AlteredStaffList.Add(staff);
                if (HairdresserProgram.SaveStaffChanges(AlteredStaffList))
                {
                    data.ItemsSource = HairdresserProgram.ListStaff();
                    MessageBox.Show("\"" + staff.Name + "\"" + " has being added!", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    data.ItemsSource = HairdresserProgram.ListStaff();
                    MessageBox.Show("Something went wrong", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            AddPhoneBox.Text = "";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int countSelected = data.SelectedItems.Count;
            if (data.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + data.SelectedItems.Count + " Employees?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    foreach (var row in data.SelectedItems)
                    {
                        Staff staff = row as Staff;
                        HairdresserProgram.DeleteStaff(staff);
                    }
                    MessageBox.Show(countSelected + " Employees have being deleted!");
                    data.ItemsSource = HairdresserProgram.ListStaff();
                }
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
