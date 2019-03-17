using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;
using BaseNode;
using EaslyController.Focus;

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
        }

        public IFocusInner Inner { get; private set; }
        public ObservableCollection<IFocusInsertionChildIndex> IndexList { get; private set; } = new ObservableCollection<IFocusInsertionChildIndex>();

        public INode SelectedNode { get; private set; }

        public void SetReplacement(IFocusInner inner, List<IFocusInsertionChildIndex> indexList)
        {
            Inner = inner;

            IndexList.Clear();
            foreach (IFocusInsertionChildIndex Index in indexList)
                IndexList.Add(Index);

            if (listOptions.SelectedIndex < 0 && IndexList.Count > 0)
                listOptions.SelectedIndex = 0;
        }

        public void SelectPreviousLine()
        {
            if (listOptions.SelectedIndex > 0)
                listOptions.SelectedIndex--;
        }

        public void SelectNextLine()
        {
            if (listOptions.SelectedIndex + 1 < IndexList.Count)
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
