using BaseNode;
using BaseNodeHelper;
using PolySerializer;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using EaslyController.Frame;
using System.Windows.Controls;
using TestDebug;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using EaslyController.Constants;
using System.Windows.Media;
using EaslyController.Focus;
using EaslyController.Writeable;
using EaslyController.Layout;
using EaslyController.Controller;

namespace EditorDebug
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Init
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            CurrentDirectory = Environment.CurrentDirectory;
            if (CurrentDirectory.Contains("Debug") || CurrentDirectory.Contains("Release"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            if (CurrentDirectory.Contains("x64"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            if (CurrentDirectory.Contains("bin"))
                CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);

            CurrentDirectory = Path.GetDirectoryName(CurrentDirectory);
            CurrentDirectory = Path.Combine(CurrentDirectory, "Test");

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //MaxWidth = ActualWidth;
            //MaxHeight = ActualHeight;
        }

        private void OnBrowse(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.FileName = CurrentDirectory;

            bool? Result = Dlg.ShowDialog(this);
            if (Result.HasValue && Result.Value)
            {
                LoadFile(Dlg.FileName);
                bool b = layoutControl.Focus();
            }
        }

        private void LoadFile(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                CurrentFileName = fileName;
                NotifyPropertyChanged(nameof(CurrentFileName));

                Serializer Serializer = new Serializer();
                INode RootNode = Serializer.Deserialize(fs) as INode;
                LoadFileLayout(RootNode);
            }
        }

        public string CurrentDirectory { get; private set; }
        public string CurrentFileName { get; private set; }
        ILayoutController Controller;
        #endregion

        #region Events
        public void OnActivated()
        {
            layoutControl.OnActivated();
        }

        public void OnDeactivated()
        {
            layoutControl.OnDeactivated();
        }

        public void OnToggleInsert(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnToggleInsert(sender, e);
        }

        private void OnDelete(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnDelete(sender, e);
        }

        private void OnBackspace(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnBackspace(sender, e);
        }

        private void OnTabForward(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnTabForward(sender, e);
        }

        private void OnEnterParagraphBreak(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.InsertNewItem(sender, e);
        }

        private void OnToggleUserVisible(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.ToggleUserVisible(sender, e);
        }

        private void OnRemoveExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.RemoveExistingItem(sender, e);
        }

        private void OnSplitExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.SplitExistingItem(sender, e);
        }

        private void OnMergeExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.MergeExistingItem(sender, e);
        }

        private void OnCycleThroughExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.CycleThroughExistingItem(sender, e);
        }

        private void OnSimplifyThroughExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.SimplifyExistingItem(sender, e);
        }

        private void OnToggleReplicate(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.ToggleReplicate(sender, e);
        }

        private void OnRedo(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.Redo(sender, e);
        }

        private void OnUndo(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.Undo(sender, e);
        }

        private void OnExpand(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.Expand(sender, e);
        }

        private void OnReduce(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.Reduce(sender, e);
        }

        public void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnCopy(sender, e);
        }

        public void OnCut(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnCut(sender, e);
        }

        public void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
            layoutControl.OnPaste(sender, e);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            layoutControl.OnMouseDown(sender, e);
        }
        #endregion

        #region Layout
        private void LoadFileLayout(INode rootNode)
        {
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            Controller = LayoutController.Create(RootIndex);
            layoutControl.SetController(Controller);
        }

        private void UpdateView()
        {
            layoutControl.InvalidateVisual();
        }
        #endregion

        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        public void NotifyThisPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion
    }
}
