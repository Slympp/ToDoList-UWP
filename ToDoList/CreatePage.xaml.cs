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

    public sealed partial class CreatePage : Page {

        //private SQLite.Net.SQLiteConnection db;
        private string DBPath;

        public CreatePage() {
            this.InitializeComponent();

            Debug.WriteLine("[CreatePage] Constructor");
            DBPath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Debug.WriteLine("[CreatePage] OnNavigatedTo");

            //db = e.Parameter as SQLite.Net.SQLiteConnection;

            base.OnNavigatedTo(e);
        }

        // Return to main menu
        private void backButtonClick(object sender, RoutedEventArgs e) {
            Debug.WriteLine("[CreatePage] backButtonClick");

            this.Frame.Navigate(typeof(ListPage), null);
        }

        // Create a SQL  request and add the new task in the database
        private void createButtonClick(object sender, RoutedEventArgs e) {
            Debug.WriteLine("[CreatePage] createButtonClick");

            if (!string.IsNullOrWhiteSpace(titleBox.Text)) {
                Debug.WriteLine("--> Insert new task in DB ");

                if (string.IsNullOrWhiteSpace(descBox.Text))
                    descBox.Text = "";

                string formattedDate;
                CultureInfo customCulture = new CultureInfo("fr-FR");
                formattedDate = datePicker.Date.ToString("d", customCulture) + " " + timePicker.Time.ToString("t", customCulture);
                formattedDate = formattedDate.Substring(0, formattedDate.Length - 3);
                Debug.WriteLine("--> formattedDate : " + formattedDate + 
                                "\n CurrentCulture: " + CultureInfo.CurrentCulture +
                                 "\n CustomCulture: " + customCulture);

                using (var db = DbConnection) {
                    db.Insert(new Task() { Title = titleBox.Text, Desc = descBox.Text, Duetime = formattedDate, isDone = false });
                }
                this.Frame.Navigate(typeof(ListPage), null);
            } else
                Debug.WriteLine("--> Can't create: titleBox is empty.");
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
