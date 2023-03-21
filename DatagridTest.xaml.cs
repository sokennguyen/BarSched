using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WPF_barber_proto
{
    /// <summary>
    /// Interaction logic for DatagridTest.xaml
    /// </summary>
    public partial class DatagridTest : UserControl
    {
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        bool isInsertMode = false;
        bool isBeingEdited = false;
        public DatagridTest()
        {
            InitializeComponent();
            data.ItemsSource = HairdresserProgram.ListStaff();
        }       
        
        

        private void Add_Click(object sender, RoutedEventArgs e)
        {  
            Staff staff;
            string value = AddBox.Text;
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
            AddBox.Text = "";
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




        private void dgEmp_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            isInsertMode = true;
        }
 
        private void dgEmp_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isBeingEdited = true;
        }

        
    }
}
