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
using System.Windows.Controls.Primitives;

namespace WPF_barber_proto
{    public partial class MainWindow : Window
    {
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ButtonFirst_Click(object sender, RoutedEventArgs e)
        {
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
            ContentControl.Content = new StaffSubpage();
        }

        private void Package_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new PackageSubpage();
        }

        private void Service_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new ServiceSubpage(); 
        }
        private void OnButtonClick(object sender, MouseButtonEventArgs e)
        {
            // Create a new instance of the page that you want to navigate to.
            popup newPage = new popup();

            newPage.Show();
        }
        private void Reserve_cal_Loaded(object sender, RoutedEventArgs e)
        {
            int numRows = 25;
            int numColumns = 8;

            // Loop through each row in the grid
            for (int row = 1; row < numRows; row++)
            {
                // Loop through each column in the grid
                for (int col = 1; col < numColumns; col++)
                {
                    // Create a new Border (cell) element
                    Border cell = new Border();
                    cell.Background = Brushes.White;
                    Rectangle background = new Rectangle();
                    background.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("blue"));
                    TextBlock textblock = new TextBlock();


                    // Attach the MouseLeftButtonDown event handler to the cell
                    cell.MouseLeftButtonDown += GridCell_MouseLeftButtonDown;

                    // Add the cell to the grid at the current row and column
                    Grid.SetRow(cell, row);
                    Grid.SetColumn(cell, col);  
                    Reserve_cal.Children.Add(cell);
                }
            }
        }

        private void GridCell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get the clicked cell
            Border clickedCell = sender as Border;

            // Create a new pop-up window
            popup popupWindow = new popup();

            // Show the pop-up window
            popupWindow.ShowDialog();
        }
    }
}
