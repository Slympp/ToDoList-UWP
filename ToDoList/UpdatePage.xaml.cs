using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

namespace ToDoList {

    public sealed partial class UpdatePage : Page {

        CultureInfo customCulture;
        private Task task;

        public UpdatePage() {
            this.InitializeComponent();

            Debug.WriteLine("[UpdatePage] Constructor");
            customCulture = new CultureInfo("fr-FR");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Debug.WriteLine("[UpdatePage] OnNavigatedTo");

            if (e.Parameter is Task) {
                task = e.Parameter as Task;
                titleBox.Text = task.Title;
                descBox.Text = task.Desc;

                Debug.WriteLine("--> DueTime: " + task.Duetime);
                string time = task.Duetime.Substring(task.Duetime.Length - 5);
                Debug.WriteLine("--> Time: " + time);
                datePicker.Date = DateTime.ParseExact(task.Duetime, "dd/MM/yyyy HH:mm", customCulture);
                timePicker.Time = TimeSpan.ParseExact(time, "hh\\:mm", customCulture);
            }

            base.OnNavigatedTo(e);
        }

        // Return to main menu on Click on "Cancel"
        private void cancelButtonClick(object sender, RoutedEventArgs e) {
            Debug.WriteLine("[UpdatePage] cancelButtonClick");

            this.Frame.Navigate(typeof(ListPage), null);
        }

        // Update the opened Task in the database
        private void updateButtonClick(object sender, RoutedEventArgs e) {
            Debug.WriteLine("[UpdatePage] updateButtonClick");

            if (!string.IsNullOrWhiteSpace(titleBox.Text)) {
                Debug.WriteLine("--> Update task in DB ");

                if (string.IsNullOrWhiteSpace(descBox.Text))
                    descBox.Text = "";

                using (var db = DbConnection) {
                    Task newTask = new ToDoList.Task();
                    newTask.Id = task.Id;
                    newTask.Title = titleBox.Text;
                    newTask.Desc = descBox.Text;
                    newTask.isDone = task.isDone;

                    string formattedDate;
                    formattedDate = datePicker.Date.ToString("d", customCulture) + " " + timePicker.Time.ToString("t", customCulture);
                    formattedDate = formattedDate.Substring(0, formattedDate.Length - 3);

                    newTask.Duetime = formattedDate;
                    db.InsertOrReplace(newTask);
                }
                this.Frame.Navigate(typeof(ListPage), null);
            } else
                Debug.WriteLine("--> Can't update: titleBox is empty.");
        }

        // Delete the opened Task and return to main menu
        private void deleteButtonClick(object sender, RoutedEventArgs e) {
            Debug.WriteLine("[UpdatePage] Delete");

            using (var db = DbConnection) {
                db.Execute("DELETE FROM Task WHERE Id = ?", task.Id);
            }
            this.Frame.Navigate(typeof(ListPage), null);
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
