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
            //MaxWidth = ActualWidth;
            //MaxHeight = ActualHeight;
        }

        IFocusControllerView ControllerView;

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Up:
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                            MoveExistingBlock(-1);
                        else
                            MoveExistingItem(-1);
                        break;

                    case Key.Down:
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                            MoveExistingBlock(+1);
                        else
                            MoveExistingItem(+1);
                        break;

                    case Key.E:
                        ToggleUserVisible();
                        break;

                    case Key.Y:
                        RemoveExistingItem();
                        break;

                    case Key.S:
                        SplitExistingItem();
                        break;

                    case Key.M:
                        MergeExistingItem();
                        break;

                    case Key.T:
                        CycleThroughExistingItem();
                        break;

                    case Key.I:
                        SimplifyExistingItem();
                        break;

                    case Key.R: // toggle replicate
                        ToggleReplicate();
                        break;

                    case Key.D: // expand/reduce selection
                    case Key.Z: // undo/redo
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Subtract:
                        ChangeDiscreteValue(-1);
                        break;

                    case Key.Add:
                        ChangeDiscreteValue(+1);
                        break;

                    case Key.Up:
                        MoveFocus(-1);
                        break;

                    case Key.Down:
                        MoveFocus(+1);
                        break;

                    case Key.Enter:
                        InsertNewItem();
                        break;

                    case Key.OemPeriod:
                        SplitIdentifier();
                        break;

                    case Key.Back:
                    case Key.Delete:
                    case Key.Insert:
                    case Key.Tab: // Comments
                    case Key.Left:
                    case Key.Right:
                    case Key.Home:
                    case Key.End:
                    case Key.PageUp: // Focus up
                    case Key.PageDown: // Focus down
                    case Key.Space: // Compact
                        break;
                }
            }
        }

        private void InsertNewItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index))
                return;

            ControllerView.Controller.Insert(inner, index, out IWriteableBrowsingCollectionNodeIndex nodeIndex);
            UpdateFocusView();
        }

        private void RemoveExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                return;

            ControllerView.Controller.Remove(inner, index);
            UpdateFocusView();
        }

        private void MoveExistingBlock(int direction)
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsBlockMoveable(direction, out IFocusBlockListInner inner, out int blockIndex))
                return;

            ControllerView.Controller.MoveBlock(inner, blockIndex, direction);
            UpdateFocusView();
        }

        private void MoveExistingItem(int direction)
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemMoveable(direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                return;

            ControllerView.Controller.Move(inner, index, direction);
            UpdateFocusView();
        }

        private void SplitExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                return;

            ControllerView.Controller.SplitBlock(inner, index);
            UpdateFocusView();
        }

        private void MergeExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                return;

            ControllerView.Controller.MergeBlocks(inner, index);
            UpdateFocusView();
        }

        private void CycleThroughExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition))
                return;

            cyclePosition = (cyclePosition + 1) % state.CycleIndexList.Count;
            ControllerView.Controller.Replace(state.ParentInner, state.CycleIndexList, cyclePosition, out IFocusBrowsingChildIndex nodeIndex);
            UpdateFocusView();
        }

        private void SimplifyExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemSimplifiable(out IFocusInner Inner, out IFocusInsertionChildIndex Index))
                return;

            ControllerView.Controller.Replace(Inner, Index, out IWriteableBrowsingChildIndex nodeIndex);
            UpdateFocusView();
        }

        private void ToggleReplicate()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsReplicationModifiable(out IFocusBlockListInner Inner, out int BlockIndex, out ReplicationStatus Replication))
                return;

            switch (Replication)
            {
                case ReplicationStatus.Normal:
                    Replication = ReplicationStatus.Replicated;
                    break;
                case ReplicationStatus.Replicated:
                    Replication = ReplicationStatus.Normal;
                    break;
            }

            ControllerView.Controller.ChangeReplication(Inner, BlockIndex, Replication);
            UpdateFocusView();
        }

        private void ToggleUserVisible()
        {
            if (ControllerView == null)
                return;

            ControllerView.SetUserVisible(!ControllerView.IsUserVisible);
            UpdateFocusView();
        }

        private void MoveFocus(int direction)
        {
            if (ControllerView == null)
                return;

            foreach (TextBlock Child in gridMain.Children)
                if (Child.DataContext == ControllerView.FocusedCellView)
                {
                    Child.Background = Brushes.Transparent;
                    break;
                }

            ControllerView.MoveFocus(direction);

            foreach (TextBlock Child in gridMain.Children)
                if (Child.DataContext == ControllerView.FocusedCellView)
                {
                    Child.Background = Brushes.LightCyan;
                    break;
                }
        }

        private void ChangeDiscreteValue(int change)
        {
            if (ControllerView == null)
                return;

            if (ControllerView.FocusedCellView is IFocusDiscreteContentFocusableCellView AsContentCellView)
            {
                IFocusIndex Index = AsContentCellView.StateView.State.ParentIndex;

                int Value = ControllerView.Controller.GetDiscreteValue(Index, AsContentCellView.PropertyName);
                ControllerView.Controller.ChangeDiscreteValue(Index, AsContentCellView.PropertyName, Value + change);

                UpdateFocusView();
            }
        }

        private void SplitIdentifier()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsIdentifierSplittable(out IFocusListInner Inner, out IFocusInsertionListNodeIndex ReplaceIndex, out IFocusInsertionListNodeIndex InsertIndex))
                return;

            ControllerView.Controller.Replace(Inner, ReplaceIndex, out IWriteableBrowsingChildIndex FirstIndex);
            ControllerView.Controller.Insert(Inner, InsertIndex, out IWriteableBrowsingCollectionNodeIndex SecondIndex);
            UpdateFocusView();
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
                LoadFileFocus(RootNode);
            }
        }

        #region Frame
        private void LoadFileFrame(INode rootNode)
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

            IFrameVisibleCellView[,] Assigned = new IFrameVisibleCellView[MaxRow, MaxColumn];

            IFrameVisibleCellViewList CellList = new FrameVisibleCellViewList();
            ControllerView.EnumerateVisibleCellViews(CellList);

            foreach (IFrameVisibleCellView CellView in CellList)
            {
                int Row = CellView.LineNumber - 1;
                int Column = CellView.ColumnNumber - 1;
                IFrameFrame Frame = CellView.Frame;
                INode ChildNode = CellView.StateView.State.Node;
                string PropertyName;
                TextBlock Child = new TextBlock();
                IFrameVisibleCellView OldCellView = Assigned[Row, Column];
                Debug.Assert(OldCellView == null);

                bool IsHandled = false;

                switch (CellView)
                {
                    case IFrameDiscreteContentFocusableCellView AsDiscreteContentFocusable: // Enum, bool
                        PropertyName = AsDiscreteContentFocusable.PropertyName;
                        if (NodeTreeHelper.IsEnumProperty(ChildNode, PropertyName))
                        {
                            Child.Text = $"{PropertyName}: {NodeTreeHelper.GetEnumValue(ChildNode, PropertyName)}";
                            IsHandled = true;
                        }
                        else if (NodeTreeHelper.IsBooleanProperty(ChildNode, PropertyName))
                        {
                            if (NodeTreeHelper.GetEnumValue(ChildNode, PropertyName) != 0)
                                Child.Text = $"{PropertyName}: True";
                            else
                                Child.Text = $"{PropertyName}: False";
                            IsHandled = true;
                        }
                        break;

                    case IFrameTextFocusableCellView AsTextFocusable: // String
                        Child.Text = NodeTreeHelper.GetString(ChildNode, AsTextFocusable.PropertyName);
                        IsHandled = true;
                        break;

                    case IFrameFocusableCellView AsFocusable: // Insert
                        Child.Foreground = Brushes.Blue;
                        Child.FontWeight = FontWeights.Bold;
                        if (CellView.Frame is IFrameKeywordFrame AsFocusableKeywordFrame)
                            Child.Text = AsFocusableKeywordFrame.Text;
                        else
                            Child.Text = "◄";
                        IsHandled = true;
                        break;

                    case IFrameVisibleCellView AsVisible: // Others
                        if (Frame is IFrameKeywordFrame AsKeywordFrame)
                        {
                            Child.FontWeight = FontWeights.Bold;
                            Child.Text = AsKeywordFrame.Text;
                            IsHandled = true;
                        }
                        else if (Frame is IFrameSymbolFrame AsSymbolFrame)
                        {
                            Child.Foreground = Brushes.Blue;

                            Symbols Symbol = AsSymbolFrame.Symbol;
                            switch (Symbol)
                            {
                                case Symbols.LeftArrow:
                                    Child.Text = "←";
                                    IsHandled = true;
                                    break;
                                case Symbols.Dot:
                                    Child.Text = ".";
                                    IsHandled = true;
                                    break;
                                case Symbols.LeftBracket:
                                    Child.Text = "[";
                                    IsHandled = true;
                                    break;
                                case Symbols.RightBracket:
                                    Child.Text = "]";
                                    IsHandled = true;
                                    break;
                                case Symbols.LeftCurlyBracket:
                                    Child.Text = "{";
                                    IsHandled = true;
                                    break;
                                case Symbols.RightCurlyBracket:
                                    Child.Text = "}";
                                    IsHandled = true;
                                    break;
                                case Symbols.LeftParenthesis:
                                    Child.Text = "(";
                                    IsHandled = true;
                                    break;
                                case Symbols.RightParenthesis:
                                    Child.Text = ")";
                                    IsHandled = true;
                                    break;
                            }
                        }
                        break;
                }

                Debug.Assert(IsHandled);

                Child.Margin = new Thickness(0, 0, 5, 0);
                Grid.SetRow(Child, Row);
                Grid.SetColumn(Child, Column);
                Assigned[Row, Column] = CellView;

                gridMain.Children.Add(Child);
            }
        }
        #endregion

        #region Focus
        private void LoadFileFocus(INode rootNode)
        {
            IFocusRootNodeIndex RootIndex = new FocusRootNodeIndex(rootNode);
            IFocusController Controller = FocusController.Create(RootIndex);
            ControllerView = FocusControllerView.Create(Controller, CustomFocusTemplateSet.FocusTemplateSet);

            UpdateFocusView();
        }

        private void UpdateFocusView()
        {
            gridMain.RowDefinitions.Clear();
            gridMain.ColumnDefinitions.Clear();
            gridMain.Children.Clear();

            int MaxRow = ControllerView.LastLineNumber;
            int MaxColumn = ControllerView.LastColumnNumber;

            for (int i = 0; i < MaxRow; i++)
                gridMain.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < MaxColumn; i++)
                gridMain.ColumnDefinitions.Add(new ColumnDefinition());

            IFocusVisibleCellView[,] Assigned = new IFocusVisibleCellView[MaxRow, MaxColumn];

            IFocusVisibleCellViewList CellList = new FocusVisibleCellViewList();
            ControllerView.EnumerateVisibleCellViews(CellList);

            foreach (IFocusVisibleCellView CellView in CellList)
            {
                int Row = CellView.LineNumber - 1;
                int Column = CellView.ColumnNumber - 1;
                IFocusFrame Frame = CellView.Frame;
                INode ChildNode = CellView.StateView.State.Node;
                TextBlock Child = new TextBlock();
                IFocusVisibleCellView OldCellView = Assigned[Row, Column];
                Debug.Assert(OldCellView == null);

                bool IsHandled = false;

                switch (CellView)
                {
                    case IFocusDiscreteContentFocusableCellView AsDiscreteContentFocusable: // Enum, bool
                        Child.Foreground = Brushes.Purple;
                        Child.Text = AsDiscreteContentFocusable.KeywordFrame.Text;
                        IsHandled = true;
                        break;

                    case IFocusTextFocusableCellView AsTextFocusable: // String
                        Child.Text = NodeTreeHelper.GetString(ChildNode, AsTextFocusable.PropertyName);
                        IsHandled = true;
                        break;

                    case IFocusFocusableCellView AsFocusable: // Insert or focusable keyword
                        Child.Foreground = Brushes.Blue;
                        Child.FontWeight = FontWeights.Bold;
                        if (CellView.Frame is IFrameKeywordFrame AsFocusableKeywordFrame)
                            Child.Text = AsFocusableKeywordFrame.Text;
                        else
                            Child.Text = "◄";
                        IsHandled = true;
                        break;

                    case IFocusVisibleCellView AsVisible: // Others
                        if (Frame is IFocusKeywordFrame AsKeywordFocus)
                        {
                            Child.FontWeight = FontWeights.Bold;
                            Child.Text = AsKeywordFocus.Text;
                            IsHandled = true;
                        }
                        else if (Frame is IFocusSymbolFrame AsSymbolFocus)
                        {
                            Child.Foreground = Brushes.Blue;

                            Symbols Symbol = AsSymbolFocus.Symbol;
                            switch (Symbol)
                            {
                                case Symbols.LeftArrow:
                                    Child.Text = "←";
                                    IsHandled = true;
                                    break;
                                case Symbols.Dot:
                                    Child.Text = ".";
                                    IsHandled = true;
                                    break;
                                case Symbols.LeftBracket:
                                    Child.Text = "[";
                                    IsHandled = true;
                                    break;
                                case Symbols.RightBracket:
                                    Child.Text = "]";
                                    IsHandled = true;
                                    break;
                                case Symbols.LeftCurlyBracket:
                                    Child.Text = "{";
                                    IsHandled = true;
                                    break;
                                case Symbols.RightCurlyBracket:
                                    Child.Text = "}";
                                    IsHandled = true;
                                    break;
                                case Symbols.LeftParenthesis:
                                    Child.Text = "(";
                                    IsHandled = true;
                                    break;
                                case Symbols.RightParenthesis:
                                    Child.Text = ")";
                                    IsHandled = true;
                                    break;
                            }
                        }
                        break;
                }

                Debug.Assert(IsHandled);

                Child.Margin = new Thickness(0, 0, 5, 0);
                Child.DataContext = CellView;

                Grid.SetRow(Child, Row);
                Grid.SetColumn(Child, Column);
                Assigned[Row, Column] = CellView;

                if (CellView == ControllerView.FocusedCellView)
                    Child.Background = Brushes.LightCyan;
                else
                    Child.Background = Brushes.Transparent;

                gridMain.Children.Add(Child);
            }
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
