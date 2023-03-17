using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        
 
   
        
 
        private void dgEmp_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
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
        }
        
        //private void dgEmp_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Delete && !isBeingEdited)
        //    {
        //        var grid = (DataGrid)sender;
        //        if (grid.SelectedItems.Count > 0)
        //        {
        //            var Res = MessageBox.Show("Are you sure you want to delete " + grid.SelectedItems.Count + " Employees?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
        //            if (Res == MessageBoxResult.Yes)
        //            {
        //                foreach (var row in grid.SelectedItems)
        //                {
        //                    Employee employee = row as Employee;
        //                    context.Employees.Remove(employee);
        //                
        //                context.SaveChanges();
        //                MessageBox.Show(grid.SelectedItems.Count + " Employees have being deleted!");
        //            }
        //            else
        //                dgEmp.ItemsSource = GetEmployeeList();
        //        }
        //    }
        //}

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
