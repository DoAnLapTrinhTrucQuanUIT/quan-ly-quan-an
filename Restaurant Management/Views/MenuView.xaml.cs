﻿using Restaurant_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant_Management.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }
        private void FoodCard_AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuVM viewModel)
            {
                if (e.OriginalSource is FoodCard foodCard)
                {
                    viewModel.AddToTempMenuCommand.Execute(foodCard);
                }
            }
        }
    }
}
