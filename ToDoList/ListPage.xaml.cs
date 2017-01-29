using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToDoList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class ListPage : Page {

        private List<Task> listOfTasks;
        static private bool isAlphabetical;

        public ListPage() {
            this.InitializeComponent();

            Debug.WriteLine("[ListPage] Constructor");

            if (isAlphabetical)
                orderButton.Content = "Order: Alpha";
            else
                orderButton.Content = "Order: Time";

            using (var db = DbConnection) {
                db.CreateTable<Task>();
                db.GetMapping(typeof(Task));
            }
            updateList();
        }

        private void updateList() {
            Debug.WriteLine("[ListPage] UpdateDB");

            using (var db = DbConnection) {
                Debug.WriteLine("--> On récupère les données et update la list");

                listOfTasks = (from p in db.Table<Task>() select p).ToList();

                if (isAlphabetical)
                    listOfTasks = listOfTasks.OrderBy(o => o.isDone).ThenBy(o => o.Title).ToList();
                else
                    listOfTasks = listOfTasks.OrderBy(o => o.isDone).ThenBy(o => DateTime.Parse(o.Duetime)).ToList();

                taskList.ItemsSource = listOfTasks;
            }
        }

        // Start Update Page
        private void updateTask(object sender, ItemClickEventArgs e) {
            Debug.WriteLine("[ListPage] updateTask");

            Task task = (Task)e.ClickedItem;
            this.Frame.Navigate(typeof(UpdatePage), task);
        }

        // Start Create Page
        private void createTask(object sender, RoutedEventArgs e) {
            Debug.WriteLine("[ListPage] createTask");

            this.Frame.Navigate(typeof(CreatePage), null);
        }

        private void swapOrderHandler(object sender, RoutedEventArgs e) {
            Debug.WriteLine("[ListPage] swapOrder");

            isAlphabetical = !isAlphabetical;
            if (isAlphabetical)
                orderButton.Content = "OrderBy: Alpha";
            else
                orderButton.Content = "OrderBy: Time";
            updateList();
        }

        // Set to done status selected items
        private void setDone(object sender, RoutedEventArgs e) {
            Debug.WriteLine("[ListPage] setDone");

            using (var db = DbConnection) {
                var checkbox = sender as CheckBox;
                if (checkbox != null) {
                    Task task = (Task)checkbox.DataContext;
                    Debug.WriteLine("--> [" + task.Id + "] isDone: " + task.isDone);
                    db.InsertOrReplace(task);
                }
            }
        }

        private static SQLiteConnection DbConnection {
            get {
                return new SQLiteConnection(
                    new SQLitePlatformWinRT(),
                    Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite"));
            }
        }

    }

    
}