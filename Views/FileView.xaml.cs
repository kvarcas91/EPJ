﻿using EPJ.Models;
using EPJ.Models.Components;
using EPJ.Models.Interfaces;
using EPJ.Models.Person;
using EPJ.Models.Task;
using EPJ.Utilities;
using EPJ.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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

namespace EPJ.Views
{
    /// <summary>
    /// Interaction logic for FileView.xaml
    /// </summary>
    public partial class FileView : UserControl
    {

        Point startPoint;
        int startIndex = -1;

        public FileView()
        {
            InitializeComponent();
        }

        private void ListViewPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            //Console.WriteLine(startPoint);
        }

        private void FileListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAnchestor.Find<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem == null) return;
                IData component = (IData)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                if (component == null) return;

                startIndex = FileListView.SelectedIndex;
                DataObject dragData = new DataObject("componentItem", component);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void FileListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("componentItem") || sender != e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void FileListView_Drop(object sender, DragEventArgs e)
        {
            /**
             * Checks if item has been droped from within the list or outside
             */
            if (startIndex < 0)
            {
                OuterFileDrop(sender, e);
                return;
            }

            if (e.Data.GetDataPresent("componentItem") && sender == e.Source)
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAnchestor.Find<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem == null)
                {
                    startIndex = -1;
                    e.Effects = DragDropEffects.None;
                    return;
                }

                var component = (IData)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                var source = (IData)FileListView.Items.GetItemAt(startIndex);
                startIndex = -1;
                ((ProjectViewModel)this.DataContext).OnDrop(source, component);
            }
        }

        private void OuterFileDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("OuterFileDrop");
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ((ProjectViewModel)this.DataContext).OnDropOuterFile(files);
        }

        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }
    }
}
