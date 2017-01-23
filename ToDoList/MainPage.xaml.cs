using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

    public sealed partial class MainPage : Page
    {
        private List<Task> listOfTasks = new List<Task>();

        public MainPage()
        {
            this.InitializeComponent();

            listOfTasks.Add(new Task { Title = "Sample1", Desc = "This is an example of description",
                            Duetime = "27/11/2016", isDone = false });
            // listOfTasks.Add(new Task { Title = "", Desc = "", Duetime = "", isDone = false });

            //taskList.ItemsSource = listOfTasks;
        }

        //// Start Update Page
        //private void updateTask(object sender, SelectionChangedEventArgs e)
        //{
        //    Debug.WriteLine("Selected: {0}", e.AddedItems[0]);
        //}

        //// Start Create Page

        private void createTask(object sender, RoutedEventArgs e) {
            Debug.WriteLine("Button click");

        }
    }
}
