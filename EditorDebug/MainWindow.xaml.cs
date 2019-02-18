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
        ILayoutControllerView ControllerView = null;
        #endregion

        #region Events
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            ControllerView = layoutControl.ControllerView;

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                OnKeyDownCtrl(e.Key);
            else
                OnKeyDown(e.Key);
        }

        private void OnKeyDown(Key key)
        {
            switch (key)
            {
                case Key.Subtract:
                    ChangeDiscreteValue(-1);
                    break;

                case Key.Add:
                    ChangeDiscreteValue(+1);
                    break;

                case Key.Up:
                    MoveFocusVertically(-1);
                    break;

                case Key.Down:
                    MoveFocusVertically(+1);
                    break;

                case Key.Enter:
                    InsertNewItem();
                    break;

                case Key.OemPeriod:
                    SplitIdentifier();
                    break;

                case Key.Back:
                case Key.Delete:
                case Key.Tab: // Comments
                case Key.Home:
                case Key.End:
                case Key.PageUp: // Focus up
                case Key.PageDown: // Focus down
                case Key.Space: // Compact
                    break;

                case Key.Left:
                    MoveCaretLeft();
                    break;
                case Key.Right:
                    MoveCaretRight();
                    break;
                case Key.Insert:
                    ToggleCaretMode();
                    break;
            }
        }

        private void OnKeyDownCtrl(Key key)
        {
            switch (key)
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

                case Key.Left:
                    MoveFocus(-1);
                    break;

                case Key.Right:
                    MoveFocus(+1);
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

        private void InsertNewItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index))
                return;

            ControllerView.Controller.Insert(inner, index, out IWriteableBrowsingCollectionNodeIndex nodeIndex);
            UpdateView();
        }

        private void RemoveExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                return;

            ControllerView.Controller.Remove(inner, index);
            UpdateView();
        }

        private void MoveExistingBlock(int direction)
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsBlockMoveable(direction, out IFocusBlockListInner inner, out int blockIndex))
                return;

            ControllerView.Controller.MoveBlock(inner, blockIndex, direction);
            UpdateView();
        }

        private void MoveExistingItem(int direction)
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemMoveable(direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                return;

            ControllerView.Controller.Move(inner, index, direction);
            UpdateView();
        }

        private void SplitExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                return;

            ControllerView.Controller.SplitBlock(inner, index);
            UpdateView();
        }

        private void MergeExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
                return;

            ControllerView.Controller.MergeBlocks(inner, index);
            UpdateView();
        }

        private void CycleThroughExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition))
                return;

            cyclePosition = (cyclePosition + 1) % state.CycleIndexList.Count;
            ControllerView.Controller.Replace(state.ParentInner, state.CycleIndexList, cyclePosition, out IFocusBrowsingChildIndex nodeIndex);
            UpdateView();
        }

        private void SimplifyExistingItem()
        {
            if (ControllerView == null)
                return;
            if (!ControllerView.IsItemSimplifiable(out IFocusInner Inner, out IFocusInsertionChildIndex Index))
                return;

            ControllerView.Controller.Replace(Inner, Index, out IWriteableBrowsingChildIndex nodeIndex);
            UpdateView();
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
            UpdateView();
        }

        private void ToggleUserVisible()
        {
            if (ControllerView == null)
                return;

            ControllerView.SetUserVisible(!ControllerView.IsUserVisible);
            UpdateView();
        }

        private void MoveFocus(int direction)
        {
            if (ControllerView == null)
                return;

            bool IsMoved;

            if (ControllerView.MinFocusMove <= direction && direction <= ControllerView.MaxFocusMove)
                ControllerView.MoveFocus(direction, out IsMoved);
            else
            {
                if (direction < 0 && ControllerView.CaretPosition > 0)
                    ControllerView.SetCaretPosition(0, out IsMoved);
                else if (direction > 0 && ControllerView.CaretPosition < ControllerView.MaxCaretPosition)
                    ControllerView.SetCaretPosition(ControllerView.MaxCaretPosition, out IsMoved);
                else
                    IsMoved = false;
            }

            if (IsMoved)
                layoutControl.InvalidateVisual();
        }

        private void MoveFocusVertically(int direction)
        {
            if (ControllerView == null)
                return;

            ControllerView.MoveFocusVertically(ControllerView.DrawContext.LineHeight * direction, out bool IsMoved);
            if (IsMoved)
                layoutControl.InvalidateVisual();
        }

        private void ChangeDiscreteValue(int change)
        {
            if (ControllerView == null)
                return;

            if (ControllerView.Focus is ILayoutDiscreteContentCellFocus AsDiscreteContentCellFocus)
            {
                ILayoutDiscreteContentFocusableCellView CellView = AsDiscreteContentCellFocus.CellView;
                IFocusIndex Index = CellView.StateView.State.ParentIndex;

                int Value = ControllerView.Controller.GetDiscreteValue(Index, CellView.PropertyName);
                ControllerView.Controller.ChangeDiscreteValue(Index, CellView.PropertyName, Value + change);

                UpdateView();
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
            UpdateView();
        }

        private void MoveCaretRight()
        {
            bool IsMoved;

            if (ControllerView.CaretPosition < ControllerView.MaxCaretPosition)
                ControllerView.SetCaretPosition(ControllerView.CaretPosition + 1, out IsMoved);
            else
                ControllerView.MoveFocus(+1, out IsMoved);

            if (IsMoved)
                layoutControl.InvalidateVisual();
        }

        private void MoveCaretLeft()
        {
            bool IsMoved;

            if (ControllerView.CaretPosition > 0)
                ControllerView.SetCaretPosition(ControllerView.CaretPosition - 1, out IsMoved);
            else
                ControllerView.MoveFocus(-1, out IsMoved);

            if (IsMoved)
                layoutControl.InvalidateVisual();
        }

        private void ToggleCaretMode()
        {
            if (ControllerView.CaretMode == CaretModes.Insertion && ControllerView.CaretPosition < ControllerView.MaxCaretPosition)
                ControllerView.SetCaretMode(CaretModes.Override);
            else
                ControllerView.SetCaretMode(CaretModes.Insertion);

            layoutControl.InvalidateVisual();
        }

        public void OnActivated()
        {
            layoutControl.OnActivated();
        }

        public void OnDeactivated()
        {
            layoutControl.OnDeactivated();
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
