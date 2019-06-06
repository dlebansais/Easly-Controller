using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;

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

        public void SetReplacement(IList<ReplacementEntry> indexList)
        {
            EntryList.Clear();
            foreach (ReplacementEntry Index in indexList)
                EntryList.Add(Index);

            if (listOptions.SelectedIndex < 0 && EntryList.Count > 0)
                listOptions.SelectedIndex = 0;
        }

        public void SelectPreviousLine(out bool isHandled)
        {
            isHandled = false;

            if (listOptions.SelectedIndex > 0)
            {
                listOptions.SelectedIndex--;
                isHandled = true;
            }
        }

        public void SelectNextLine(out bool isHandled)
        {
            isHandled = false;

            if (listOptions.SelectedIndex + 1 < EntryList.Count)
            {
                listOptions.SelectedIndex++;
                isHandled = true;
            }
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
