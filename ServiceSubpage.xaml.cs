using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_barber_proto
{
    /// <summary>
    /// Interaction logic for ServiceSubpage.xaml
    /// </summary>
    public partial class ServiceSubpage : UserControl
    {
        public ServiceSubpage()
        {
            InitializeComponent();
            RenderTable();
        }
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        private void RenderTable()
        {
            int rowIndex = 1;
            List<Service> serviceList = HairdresserProgram.ListService();
            foreach (Service service in serviceList)
            {
                AddRowDef("table");

                if (rowIndex % 2 == 0) AddRowBgColor("green", "table", rowIndex, 5);
                else AddRowBgColor("yellow", "table", rowIndex, 5);

                AddTextBlock(service.Id.ToString(), "table", rowIndex, 0);
                AddTextBlock(service.Name, "table", rowIndex, 1);
                AddTextBlock(service.Duration.ToString(), "table", rowIndex, 2);
                AddCheckBox(service.Sink, "table", rowIndex, 3);
                AddTextBlock(service.PackageName, "table", rowIndex, 4);

                rowIndex++;
            }

        }
        
        protected void AddCollumnContentControl(string gridName, int colIndex)
        {
            var grid = (Grid)this.FindName(gridName);
            ContentControl contentControl = new ContentControl();
            contentControl.Name = "textBlockControl";
            int rowIndex = 1;
            foreach (RowDefinition row in grid.RowDefinitions)
            {
                Grid.SetRow(contentControl, rowIndex);
                Grid.SetColumnSpan(contentControl, colIndex);
            }

            contentControl.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(contentControl_Click));
            
            grid.Children.Add(contentControl);
        }
        private void contentControl_Click(object sender, RoutedEventArgs e)
        {
            string sourceName = ((FrameworkElement)e.Source).Name;
            string senderName = ((FrameworkElement)sender).Name;  
        }

        protected void changeTextBlockBgColor(string color,string gridName,int rowIndex, int colIndex)
        {
            var grid = (Grid)this.FindName(gridName);
            Rectangle rectangle = grid.Children.Cast<Rectangle>().First(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == colIndex);
            rectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)); 
        }
        protected void AddRowDef(string gridName)
        {
            var grid = (Grid)this.FindName(gridName);
            RowDefinition RowDef = new RowDefinition();
            RowDef.Height = new GridLength(50);
            grid.RowDefinitions.Add(RowDef);
        }
        protected void AddRowBgColor(string color, string gridName, int rowIndex, int colSpan)
        {
            Rectangle bgRec = new Rectangle();
            bgRec.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            Grid.SetRow(bgRec, rowIndex);
            Grid.SetColumnSpan(bgRec, colSpan);
            var grid = (Grid)this.FindName(gridName);
            grid.Children.Add(bgRec);
        }
        protected void AddTextBlock(string inpString, string gridName, int rowIndex, int colIndex)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = inpString;
            var grid = (Grid)this.FindName(gridName);
            Grid.SetRow(textBlock, rowIndex);
            Grid.SetColumn(textBlock, colIndex);
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(textBlock);
        }
        protected void AddCheckBox(bool inpBool, string gridName, int rowIndex, int colIndex)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.IsChecked=inpBool;
            var grid = (Grid)this.FindName(gridName);
            Grid.SetRow(checkBox, rowIndex);
            Grid.SetColumn(checkBox, colIndex);
            checkBox.VerticalAlignment = VerticalAlignment.Center;
            checkBox.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(checkBox);
        }
    }
}
