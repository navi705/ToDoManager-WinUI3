// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using ToDoManager.IoC;
using ToDoManager.Models;
using ToDoManager.ViewModels;

namespace ToDoManager.Pages
{
    public sealed partial class AddOrEditTasksPage : Page
    {
        public AddOrEditTasksPage()
        {
            this.InitializeComponent();
        }

        public AddOrEditTasksViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = DIHelper.Resolve<AddOrEditTasksViewModel>();
            var task = (ToDoTask) e.Parameter;
            ViewModel.Task =task;
            ViewModel.TitleTask = task.Name;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var task = (ToDoTask) e.ClickedItem;
            ViewModel.SelectedSubTask(task);
        }
    }
}
