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
            //connecting to mysql db requires 3 steps:
            
            //create connection + open or close during usages
            MySqlConnection connection = new MySqlConnection("Server=4.tcp.eu.ngrok.io;Port=17712;User ID=root;Database=ds_assignment_auction");
            connection.Open();

            //create command
            MySqlCommand command = new MySqlCommand("SELECT auction_id,product_name,auction_start_time,auction_end_time FROM auction", connection);
            
            //execute the command
            Week_Schedule.ItemsSource = command.ExecuteReader();

            MainTitle.Content = "Add or Remove data";
            CollaspAllView();
            Week_Schedule.Visibility = Visibility.Visible;
        }
        private void ButtonThird_Click(object sender, RoutedEventArgs e)
        {
            MainTitle.Content = "Employee Performances";
            CollaspAllView();
        }

        private void CollaspAllView()
        {
            Week_Schedule.Visibility = Visibility.Collapsed;
            Reserve_cal.Visibility = Visibility.Collapsed;
        }

        private void AddReserveTimeBlock()
        {
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

        //was trying out oledb (Access database), but found a way to use implement mysql 
        private OleDbConnection OpenDB()
        {
            string connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = C:\repos\access_db\barberDB.mdb";
            OleDbConnection connection = new OleDbConnection(connectionStr);
            connection.Open();
            return connection;
        }

    }
}
