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
using System.Data;
using System.Data.OleDb;
using System.Windows.Media.Animation;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Messaging;

namespace WPF_barber_proto
{    public partial class MainWindow : Window
    {
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        public MainWindow()
        {
            InitializeComponent();
            AddReserveTimeBlock();            
        }
        private void ButtonFirst_Click(object sender, RoutedEventArgs e)
        {
            AddReserveTimeBlock();
            MainTitle.Content = "Reservations";
            CollaspAllView();
            Reserve_cal.Visibility = Visibility.Visible;
        }        

        private void ButtonSecond_Click(object sender, RoutedEventArgs e)
        {           


            MainTitle.Content = "Add or Remove data";
            CollaspAllView();
            Editor.Visibility = Visibility.Visible;
        }
        private void ButtonThird_Click(object sender, RoutedEventArgs e)
        {
            MainTitle.Content = "Employee Performances";
            CollaspAllView();
        }

        private void CollaspAllView()
        {
            Editor.Visibility = Visibility.Collapsed;
            Reserve_cal.Visibility = Visibility.Collapsed;
        }
        //private void IniTable()
        //{
        //    List<Customer> custList = HairdresserProgram.ListCustomers();
        //    foreach (Customer customer in custList)
        //    {

        //    }
        //}

        //private void AddTextBlock(string inpString, string gridName)
        //{
        //    TextBlock textBlock = new TextBlock();
        //    textBlock.Text = inpString;
        //    var grid = (Grid)this.FindName(gridName);
        //    grid.Children.Add(textBlock);
        //}


        private void AddReserveTimeBlock()
        {
            if (Reserve_cal.Children.Count>9) return;            
            DateTime datetimeTimeBlock = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 30, 0);
            int indexAddRow = 1;
            foreach (RowDefinition row in Reserve_cal.RowDefinitions)
            {
                TextBlock outTimeBlock = new TextBlock();
                string dateTimeBlockString="";
                if (datetimeTimeBlock.Minute==30)
                    dateTimeBlockString = datetimeTimeBlock.Hour.ToString() + ":" + datetimeTimeBlock.Minute.ToString();
                else dateTimeBlockString = datetimeTimeBlock.Hour.ToString() + ":" + datetimeTimeBlock.Minute.ToString() + "0";
                outTimeBlock.Text = dateTimeBlockString;
                
                //set row and add into Reserve_cal grid
                Grid.SetRow(outTimeBlock, indexAddRow);
                Reserve_cal.Children.Add(outTimeBlock);

                //increase time and row for next loop
                if (indexAddRow <23)
                {
                    datetimeTimeBlock = datetimeTimeBlock.AddMinutes(30);
                    indexAddRow++;
                }
            }
        }

        private void Customer_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new CustomerSubpage();
        }

        private void Staff_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new CustomerSubpage();
        }
    }
}
