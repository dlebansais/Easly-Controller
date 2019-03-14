using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;
using BaseNode;

namespace EditorDebug
{
    /// <summary>
    /// Interaction logic for TextReplacement.xaml
    /// </summary>
    public partial class TextReplacement : Popup, INotifyPropertyChanged
    {
        public TextReplacement()
        {
            InitializeComponent();
            DataContext = this;

            OptionNodeList.Add("X");
            OptionNodeList.Add("Y");
            OptionNodeList.Add("Z");
            listOptions.SelectedIndex = 0;
        }

        public string Text { get; private set; }

        public bool HasReplacementOptions { get { return Text != null && Text.Length > 1; } }

        public ObservableCollection<string> OptionNodeList { get; } = new ObservableCollection<string>();

        public INode SelectedNode { get; private set; }

        public void SetText(string text)
        {
            Text = text;
        }

        public void SelectPreviousLine()
        {
            if (listOptions.SelectedIndex > 0)
                listOptions.SelectedIndex--;
        }

        public void SelectNextLine()
        {
            if (listOptions.SelectedIndex + 1 < OptionNodeList.Count)
                listOptions.SelectedIndex++;
        }

        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public void NotifyThisPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion
    }
}
