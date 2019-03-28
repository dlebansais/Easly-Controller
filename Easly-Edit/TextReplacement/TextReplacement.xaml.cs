using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;
using EaslyController.Layout;

namespace EaslyEdit
{
    /// <summary>
    /// Interaction logic for TextReplacement.xaml
    /// </summary>
    internal partial class TextReplacement : Popup, INotifyPropertyChanged
    {
        public TextReplacement()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ILayoutInner Inner { get; private set; }
        public ObservableCollection<ReplacementEntry> EntryList { get; private set; } = new ObservableCollection<ReplacementEntry>();
        public ReplacementEntry SelectedEntry
        {
            get
            {
                if (listOptions.SelectedIndex < 0 || listOptions.SelectedIndex >= EntryList.Count)
                    return null;
                else
                    return EntryList[listOptions.SelectedIndex];
            }
        }

        public void SetReplacement(ILayoutInner inner, List<ReplacementEntry> indexList)
        {
            Inner = inner;

            EntryList.Clear();
            foreach (ReplacementEntry Index in indexList)
                EntryList.Add(Index);

            if (listOptions.SelectedIndex < 0 && EntryList.Count > 0)
                listOptions.SelectedIndex = 0;
        }

        public void SelectPreviousLine()
        {
            if (listOptions.SelectedIndex > 0)
                listOptions.SelectedIndex--;
        }

        public void SelectNextLine()
        {
            if (listOptions.SelectedIndex + 1 < EntryList.Count)
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
