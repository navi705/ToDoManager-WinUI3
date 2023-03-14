// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ToDoManager.IoC;
using ToDoManager.Models;
using ToDoManager.ViewModels;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoManager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimeMangerPage : Page
    {
        public TimeMangerPage()
        {
            this.InitializeComponent();

        }
        public TimeMangerViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = DIHelper.Resolve<TimeMangerViewModel>();
            ViewModel.Intilization();
             ViewModel.PropertyChanged += DrawChart;
        }

        private void Button_Click_1(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            var time = (Time)button.DataContext;
            ViewModel.DeleteTimeNote(time);
        }

        private void DrawChart(object? sender, EventArgs e) 
        {
            if (ViewModel.TimeTable == null)
            {
                chart.Children.Clear();
                return;
            }
            if (ViewModel.TimeTable.Count == 0)
            {
                chart.Children.Clear();
                return;
            }
                chart.Children.Clear();
            Grid grid = new() {ColumnSpacing = 15 };
            chart.Children.Add(grid);

            var timetable = ViewModel.TimeTable;
            timetable = new ObservableCollection<Time>(timetable.OrderBy(x => x.Of).ToList());
            int row = timetable.Count + 3, column = timetable.Count;

            

            for (int i = 0; i < row; ++i)
            {
                RowDefinition rowDefinition = new();
                rowDefinition.Height = new GridLength(2, GridUnitType.Star);
                grid.RowDefinitions.Add(rowDefinition);

            }

            int[] arr = new int[timetable.Count];

            int coefficient = 2,coff = 1;
           // int coefficient = column;
            Dictionary<Time, TimeSpan> times1 = new();
            for (int i = 0; i < timetable.Count; ++i)
            {
                TimeSpan timesNext = DateTimeOffset.Parse(timetable[i].To) - DateTimeOffset.Parse(timetable[i].Of);
                times1.Add(timetable[i], timesNext);
            }
            Dictionary<Time, TimeSpan> times2 = times1.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            // чинить
            for (int i = 0; i < timetable.Count-1; ++i)
            {
                var timez = times2.Values.ElementAt(i + 1) - times2.Values.ElementAt(i);
                if (timez > TimeSpan.Parse("00:50") || times2.Values.ElementAt(i) > TimeSpan.Parse("00:50"))
                {
                    coefficient *= 2;
                    arr[timetable.IndexOf(times2.Keys.ElementAt(i))] = coefficient;
                }
                else
                {
                    if(timez < TimeSpan.Parse("00:50") || times2.Values.ElementAt(i) < TimeSpan.Parse("00:50"))
                    {
                        arr[timetable.IndexOf(times2.Keys.ElementAt(i))] = coff;
                        continue;
                    }
                    arr[timetable.IndexOf(times2.Keys.ElementAt(i))] = coefficient;
                }

                //arr[timetable.IndexOf(times2.Keys.ElementAt(i))] = coefficient;
                //coefficient++;
            }
            //починить
            if (times2.Count >= 3)
            {
                if (times2.Values.ElementAt(timetable.Count - 1) - times2.Values.ElementAt(timetable.Count - 2) > TimeSpan.Parse("00:50"))
                {
                    coefficient++;
                    arr[timetable.IndexOf(times2.Keys.ElementAt(timetable.Count - 1))] = coefficient;
                }
                else
                {
                    arr[timetable.IndexOf(times2.Keys.ElementAt(timetable.Count - 1))] = coefficient;
                }
            }
            else
            {
                arr[timetable.IndexOf(times2.Keys.ElementAt(timetable.Count - 1))] = coefficient;
            }

            var uiSettings = new UISettings();
            var color = uiSettings.GetColorValue(UIColorType.Accent);
            SolidColorBrush myBrush = new SolidColorBrush(color);
            for (int i = 0; i < column; ++i)
            {
                ColumnDefinition columnDefinition = new();
               Microsoft.UI.Xaml.Shapes.Rectangle rectangle = new() { Height = 50, Fill =myBrush , Opacity = 0.3f, HorizontalAlignment = HorizontalAlignment.Stretch
                   , RadiusX =15, RadiusY = 15, };
                columnDefinition.Width = new GridLength(arr[i], GridUnitType.Star);
                grid.ColumnDefinitions.Add(columnDefinition);
                grid.Children.Add(rectangle);
                Grid.SetColumn(rectangle, i);
                Grid.SetRow(rectangle, i);
            }

            for (int i = 0; i < column; i++)
            {
                TextBlock textBlockOf = new() { Text = timetable[i].Of, HorizontalAlignment = HorizontalAlignment.Center };
                TextBlock textBlockTo = new() { Text = timetable[i].To, HorizontalAlignment = HorizontalAlignment.Center };
                TextBlock textBlockNameTAsk = new() { Text = timetable[i].NameTask, HorizontalAlignment = HorizontalAlignment.Center };

                grid.Children.Add(textBlockOf);
                Grid.SetColumn(textBlockOf, i);
                Grid.SetRow(textBlockOf, i+1);

                grid.Children.Add(textBlockTo);
                Grid.SetColumn(textBlockTo, i);
                Grid.SetRow(textBlockTo, i+3);

                grid.Children.Add(textBlockNameTAsk);
                Grid.SetColumn(textBlockNameTAsk, i);
                Grid.SetRow(textBlockNameTAsk, i+2);
            }

        }
          //for (int i = 0; i<timetable.Count-1; ++i)
          //  {
          //      var timez = times2.Values.ElementAt(i) - times2.Values.ElementAt(i + 1);
          //      if (timez< -TimeSpan.Parse("00:50"))
          //      {
          //          coefficient *= 2;
          //          arr[timetable.IndexOf(times2.Keys.ElementAt(i))] = coefficient;
          //      }
          //      else
          //      {
          //          arr[timetable.IndexOf(times2.Keys.ElementAt(i))] = coefficient;
          //      }

          //      //arr[timetable.IndexOf(times2.Keys.ElementAt(i))] = coefficient;
          //      //coefficient++;
          //  }
    }
}
