using MrJobProject.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for ListsUC.xaml
    /// </summary>
    public partial class ListsUC : UserControl
    {
        private ObservableCollection<Worker> workers;
        private List<Worker> selectedWorkers;

        public ListsUC()
        {
            InitializeComponent();
            ListOfYears.ItemsSource = Enumerable.Range(2000, DateTime.Today.Year + 3 - 2000).ToList().Reverse<int>();
            ListOfMonths.ItemsSource = Enumerable.Range(1, 12).ToList();
            ListOfYears.SelectedValue = DateTime.Today.Year;
            ListOfMonths.SelectedValue = DateTime.Today.Month;

            workers = new ObservableCollection<Worker>();
            ReadDatabase();
        }

        private void ReadDatabase()
        {
            //TODO:
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Worker>(); //workers list
                workers = new ObservableCollection<Worker>
                    (connection.Table<Worker>().Where(c => c.Status == true).ToList().OrderBy(c => c.Name).ToList().OrderByDescending(c => c.Status));
                if (workers != null)
                {
                    workersListView.ItemsSource = workers;
                }
            }
        }

        private void ListOfYears_SelectionChanged(object sender, SelectionChangedEventArgs e) // when year changed
        {
            //TODO:
        }

        private void ListOfMonths_SelectionChanged(object sender, SelectionChangedEventArgs e) // when month changed
        {
            //TODO:
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;
            var filteredList = workers.Where(c => c.Name.ToLower().Contains(searchTextBox.Text)).ToList();

            workersListView.ItemsSource = filteredList;
        }

        private void WorkersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // var sselectWorkers = workersListView.SelectedItems;
        }

        private void NoneButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in workers)
            {
                workersListView.SelectedItems.Clear();
            }
            Keyboard.Focus(workersListView);
        }

        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in workers)
            {
                workersListView.SelectedItems.Add(item);
            }
            Keyboard.Focus(workersListView);
        }
    }
}