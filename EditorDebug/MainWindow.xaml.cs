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

namespace EditorDebug
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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
            MaxWidth = ActualWidth;
            MaxHeight = ActualHeight;
        }

        public string CurrentDirectory { get; private set; }
        public string CurrentFileName { get; private set; }

        private void OnBrowse(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.FileName = CurrentDirectory;

            bool? Result = Dlg.ShowDialog(this);
            if (Result.HasValue && Result.Value)
            {
                LoadFile(Dlg.FileName);
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
                LoadFile(RootNode);
            }
        }

        private void LoadFile(INode rootNode)
        {
            IFrameRootNodeIndex RootIndex = new FrameRootNodeIndex(rootNode);
            IFrameController Controller = FrameController.Create(RootIndex);
            IFrameControllerView ControllerView = FrameControllerView.Create(Controller, CustomFrameTemplateSet.FrameTemplateSet);

            int MaxRow = ControllerView.LastLineNumber;
            int MaxColumn = ControllerView.LastColumnNumber;

            for (int i = 0; i < MaxRow; i++)
                gridMain.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < MaxColumn; i++)
                gridMain.ColumnDefinitions.Add(new ColumnDefinition());

            IFrameVisibleCellViewList CellList = new FrameVisibleCellViewList();
            ControllerView.EnumerateVisibleCellViews(CellList);

            foreach (IFrameVisibleCellView CellView in CellList)
            {
                IFrameFrame Frame = CellView.Frame;
                INode ChildNode = CellView.StateView.State.Node;
                string PropertyName;
                TextBlock Child = new TextBlock();

                switch (CellView)
                {
                    case IFrameDiscreteContentFocusableCellView AsDiscreteContentFocusable: // Enum, bool
                        PropertyName = AsDiscreteContentFocusable.PropertyName;
                        if (NodeTreeHelper.IsEnumProperty(ChildNode, PropertyName))
                            Child.Text = $"{PropertyName}: {NodeTreeHelper.GetEnumValue(ChildNode, PropertyName)}";
                        else if (NodeTreeHelper.IsBooleanProperty(ChildNode, PropertyName))
                            if (NodeTreeHelper.GetEnumValue(ChildNode, PropertyName) != 0)
                                Child.Text = $"{PropertyName}: True";
                            else
                                Child.Text = $"{PropertyName}: False";
                        else
                            throw new ArgumentOutOfRangeException(nameof(CellView));
                        break;
                    case IFrameTextFocusableCellView AsTextFocusable: // String
                        Child.Text = NodeTreeHelper.GetText(ChildNode);
                        break;
                    case IFrameFocusableCellView AsFocusable: // Insert
                        Child.Text = "+";
                        break;
                    case IFrameVisibleCellView AsVisible: // Others
                        if (Frame is IFrameKeywordFrame AsKeywordFrame)
                            Child.Text = AsKeywordFrame.Text;
                        else if (Frame is IFrameSymbolFrame AsSymbolFrame)
                            Child.Text = AsSymbolFrame.Symbol.ToString();
                        else
                            throw new ArgumentOutOfRangeException(nameof(CellView));
                        break;
                }

                Grid.SetRow(Child, CellView.LineNumber);
                Grid.SetColumn(Child, CellView.ColumnNumber);

                gridMain.Children.Add(Child);
            }
        }

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
