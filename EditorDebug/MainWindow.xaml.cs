using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using BaseNode;
using BaseNodeHelper;
using EaslyController.Layout;
using Microsoft.Win32;
using PolySerializer;

namespace EditorDebug
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Init
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //TextFormattedNumber();

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
                INode? RootNode = Serializer.Deserialize(fs) as INode;
                if (RootNode != null)
                    LoadFileLayout(RootNode);
            }
        }

        private void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                string? Content = Clipboard.GetData(DataFormats.UnicodeText) as string;

                if (Content != null)
                {
                    Serializer Serializer = new Serializer();
                    Serializer.Format = SerializationFormat.TextOnly;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] Bytes = Encoding.UTF8.GetBytes(Content);
                        ms.Write(Bytes, 0, Bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);

                        INode? RootNode = Serializer.Deserialize(ms) as INode;
                        if (RootNode != null)
                            LoadFileLayout(RootNode);
                    }
                }
            }
            catch
            {
            }
        }

        public string CurrentDirectory { get; private set; }
        public string CurrentFileName { get; private set; } = string.Empty;

        private string GenerateNumber(string charset, long pattern)
        {
            if (pattern == 0)
                return "";

            string s = charset.Substring((int)(pattern % charset.Length), 1);
            s += GenerateNumber(charset, pattern / charset.Length);

            return s;
        }
        #endregion

        #region Layout
        private void LoadFileLayout(INode rootNode)
        {
            LayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(rootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);
            layoutControl.Controller = Controller;
        }

        private void UpdateView()
        {
            layoutControl.InvalidateVisual();
        }
        #endregion

        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

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
