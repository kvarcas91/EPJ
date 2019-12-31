using EPJ.Models;
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
    /// Interaction logic for ProjectView.xaml
    /// </summary>
    public partial class ProjectView : UserControl
    {

        Point startPoint;
        int startIndex = -1;

        public ProjectView()
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

        private void OuterFileDrop (object sender, DragEventArgs e)
        {
            Console.WriteLine("OuterFileDrop");
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ((ProjectViewModel)this.DataContext).OnDropOuterFile(files);
        }

        private void TreeView_MouseDown (object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void TreeView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                //TreeView treeView = sender as TreeView;
                TreeViewItem treeviewItem = FindAnchestor.Find<TreeViewItem>((DependencyObject)e.OriginalSource);
                if (treeviewItem == null) return;
                IElement component = (IElement)projectTaskList.ItemContainerGenerator.ItemFromContainer(treeviewItem);
                if (component == null) return;

                //startIndex = FileListView.SelectedIndex;
                DataObject dragData = new DataObject("taskItem", component);
                DragDrop.DoDragDrop(treeviewItem, dragData, DragDropEffects.Copy | DragDropEffects.Move);
            }
            else
            {
                //Console.WriteLine("TreeView_MouseMove failed");
            }
        }

        private void TreeView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("taskItem") || sender != e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                //Console.WriteLine("TreeView_DragEnter failed");
            }
        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            var index = -1;

            //if (startIndex < 0) return;

            if (e.Data.GetDataPresent("taskItem"))
            {
                //ListView listView = sender as ListView;
                TreeViewItem treeViewItem = FindAnchestor.Find<TreeViewItem>((DependencyObject)e.OriginalSource);
                if (treeViewItem == null)
                {
                    e.Effects = DragDropEffects.None;
                    return;
                }
                var item = ((ITask)projectTaskList.SelectedItem);
                startIndex = ((ProjectViewModel)DataContext).ProjectTasks.IndexOf(item);

                IElement component = (IElement)projectTaskList.ItemContainerGenerator.ItemFromContainer(treeViewItem);
                IElement source = (IElement)projectTaskList.Items.GetItemAt(startIndex);
                e.Effects = DragDropEffects.Move;
               

                ((ProjectViewModel)DataContext).OnDropTask(source, component);

                startIndex = -1;
                index = -1;
            }
            else
            {
                //Console.WriteLine("TreeView_Drop failed");
            }
        }

        /*
        private void projectTaskList_MouseMove(object sender, MouseEventArgs e)
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
                ITask component = (ITask)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                if (component == null) return;
                Console.WriteLine($"Mouse move: {component.Content}");
                //startIndex = projectTaskList.SelectedIndex;
                
                DataObject dragData = new DataObject("taskItem", component);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void projectTaskList_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("taskItem") || sender != e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void projectTaskList_Drop(object sender, DragEventArgs e)
        {
            var index = -1;

            if (startIndex < 0) return;

            if (e.Data.GetDataPresent("taskItem") && sender == e.Source)
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAnchestor.Find<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem == null)
                {
                    e.Effects = DragDropEffects.None;
                    return;
                }

                ITask component = (ITask)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                ITask source = (ITask)projectTaskList.Items.GetItemAt(startIndex);
                e.Effects = DragDropEffects.Move;
                index = projectTaskList.Items.IndexOf(component);

                ((ProjectViewModel)this.DataContext).OnDropTask(source, component);

                startIndex = -1;
                index = -1;
            }
        }

      */
    }
}
