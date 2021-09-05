namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Frame;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFocusNodeTemplate : IFrameNodeTemplate, IFocusTemplate
    {
        /// <summary>
        /// True if the associated expression should be surrounded with parenthesis.
        /// (Set in Xaml)
        /// </summary>
        bool IsComplex { get; }

        /// <summary>
        /// True if the parent template rather than this template should be fully displayed when visibility is enforced.
        /// (Set in Xaml)
        /// </summary>
        bool IsSimple { get; }

        /// <summary>
        /// Returns the frame associated to a property if it can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        bool FrameSelectorForProperty(string propertyName, out IFocusFrameWithSelector frame);

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        void GetPreferredFrame(out IFocusNodeFrame firstPreferredFrame, out IFocusNodeFrame lastPreferredFrame);

        /// <summary>
        /// Gets the frame that associated to a given property.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="selectorStack">A list of selectors to choose the correct frame.</param>
        IFocusNamedFrame PropertyToFrame(string propertyName, IList<FocusFrameSelectorList> selectorStack);

        /// <summary>
        /// Gets the frame associated to the comment.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        IFocusCommentFrame GetCommentFrame(IList<FocusFrameSelectorList> selectorStack);
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    [ContentProperty("Root")]
    public class FocusNodeTemplate : FrameNodeTemplate, IFocusNodeTemplate
    {
        #region Properties
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        public new IFocusFrame Root { get { return (IFocusFrame)base.Root; } set { base.Root = value; } }

        /// <summary>
        /// True if the associated expression should be surrounded with parenthesis.
        /// (Set in Xaml)
        /// </summary>
        public bool IsComplex { get; set; }

        /// <summary>
        /// True if the parent template rather than this template should be fully displayed when visibility is enforced.
        /// (Set in Xaml)
        /// </summary>
        public bool IsSimple { get; set; }

        private protected override bool IsRootValid { get { return Root.ParentFrame == FocusFrame.FocusRoot; } }

        /// <summary>
        /// Checks that a template and all its frames are valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                bool IsValid = true;

                IsValid &= base.IsValid;

                GetPreferredFrame(out IFocusNodeFrame FirstPreferredFrame, out IFocusNodeFrame LastPreferredFrame);
                IsValid &= FirstPreferredFrame != null && LastPreferredFrame != null;

                return IsValid;
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Returns the frame associated to a property if it can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        public virtual bool FrameSelectorForProperty(string propertyName, out IFocusFrameWithSelector frame)
        {
            frame = null;
            bool Found = false;

            if (Root is IFocusSelectionFrame AsSelectionFrame)
            {
                List<IFocusFrameWithSelector> FrameSelectorList = new List<IFocusFrameWithSelector>();
                foreach (IFocusSelectableFrame Item in AsSelectionFrame.Items)
                    if (FrameSelectorForPropertySingle(Item.Content, propertyName, out frame))
                        FrameSelectorList.Add(frame);

                if (FrameSelectorList.Count > 0)
                {
                    IFocusFrameWithSelector FirstFrame = FrameSelectorList[0];

                    CompareEqual Comparer = CompareEqual.New();
                    for (int i = 1; i < FrameSelectorList.Count; i++)
                        Debug.Assert(FrameSelectorList[i].Selectors.IsEqual(Comparer, FirstFrame.Selectors));

                    frame = FirstFrame;
                    Found = true;
                }
            }
            else
                Found = FrameSelectorForPropertySingle(Root, propertyName, out frame);

            return Found;
        }

        /// <summary></summary>
        protected virtual bool FrameSelectorForPropertySingle(IFocusFrame root, string propertyName, out IFocusFrameWithSelector frame)
        {
            frame = null;
            bool Found = false;

            if (root is IFocusSelectorPropertyFrame AsSelectorPropertyFrame)
                Found = AsSelectorPropertyFrame.FrameSelectorForProperty(propertyName, out frame);

            return Found;
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        public virtual void GetPreferredFrame(out IFocusNodeFrame firstPreferredFrame, out IFocusNodeFrame lastPreferredFrame)
        {
            firstPreferredFrame = null;
            lastPreferredFrame = null;
            ((IFocusNodeFrame)Root).GetPreferredFrame(ref firstPreferredFrame, ref lastPreferredFrame);
        }

        /// <summary>
        /// Gets the frame that associated to a given property.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="selectorStack">A list of selectors to choose the correct frame.</param>
        public virtual IFocusNamedFrame PropertyToFrame(string propertyName, IList<FocusFrameSelectorList> selectorStack)
        {
            int ValueFrameIndex = 0;
            bool Found = GetFirstNamedFrame(Root, propertyName, selectorStack, false, ref ValueFrameIndex, out IFocusNamedFrame Result);
            Debug.Assert(Found);
            Debug.Assert(ValueFrameIndex > 0);
            Debug.Assert(Result != null);

            return Result;
        }

        private protected bool GetFirstNamedFrame(IFocusFrame root, string propertyName, IList<FocusFrameSelectorList> selectorStack, bool reverseSearch, ref int valueFrameIndex, out IFocusNamedFrame frame)
        {
            frame = null;
            bool Found = false;

            switch (root)
            {
                case IFocusNamedFrame AsNamedFrame:
                    Found = GetFirstNamedFrameNamed(AsNamedFrame, propertyName, ref valueFrameIndex, out frame);
                    break;
                case IFocusPanelFrame AsPanelFrame:
                    Found = GetFirstNamedFramePanel(AsPanelFrame, propertyName, selectorStack, reverseSearch, ref valueFrameIndex, out frame);
                    break;
                case IFocusSelectionFrame AsSelectionFrame:
                    Found = GetFirstNamedFrameSelection(AsSelectionFrame, propertyName, selectorStack, reverseSearch, ref valueFrameIndex, out frame);
                    break;
            }

            return Found;
        }

        private bool GetFirstNamedFrameNamed(IFocusNamedFrame root, string propertyName, ref int valueFrameIndex, out IFocusNamedFrame frame)
        {
            frame = null;

            valueFrameIndex++;

            if (root.PropertyName == propertyName)
            {
                frame = root;
                return true;
            }
            else
                return false;
        }

        private bool GetFirstNamedFramePanel(IFocusPanelFrame root, string propertyName, IList<FocusFrameSelectorList> selectorStack, bool reverseSearch, ref int valueFrameIndex, out IFocusNamedFrame frame)
        {
            frame = null;

            int Count = root.Items.Count;
            for (int i = 0; i < Count; i++)
            {
                IFocusFrame Item = (IFocusFrame)root.Items[reverseSearch ? Count - 1 - i : i];

                if (GetFirstNamedFrame(Item, propertyName, selectorStack, reverseSearch, ref valueFrameIndex, out frame))
                    return true;
            }

            return false;
        }

        private bool GetFirstNamedFrameSelection(IFocusSelectionFrame root, string propertyName, IList<FocusFrameSelectorList> selectorStack, bool reverseSearch, ref int valueFrameIndex, out IFocusNamedFrame frame)
        {
            frame = null;

            IFocusSelectableFrame SelectedFrame = null;

            foreach (IFocusSelectableFrame Item in root.Items)
            {
                foreach (FocusFrameSelectorList SelectorList in selectorStack)
                {
                    foreach (IFocusFrameSelector Selector in SelectorList)
                    {
                        if (Selector.SelectorType == NodeType)
                            if (Selector.SelectorName == Item.Name)
                            {
                                SelectedFrame = Item;
                                break;
                            }
                    }
                    if (SelectedFrame != null)
                        break;
                }
                if (SelectedFrame != null)
                    break;
            }

            if (SelectedFrame != null)
                if (GetFirstNamedFrame(SelectedFrame.Content, propertyName, selectorStack, reverseSearch, ref valueFrameIndex, out frame))
                    return true;

            return false;
        }

        /// <summary>
        /// Gets the frame associated to the comment.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        public virtual IFocusCommentFrame GetCommentFrame(IList<FocusFrameSelectorList> selectorStack)
        {
            bool Found = GetFirstCommentFrame(Root, selectorStack, out IFocusCommentFrame Result);
            Debug.Assert(Found);
            Debug.Assert(Result != null);

            return Result;
        }

        private protected virtual bool GetFirstCommentFrame(IFocusFrame root, IList<FocusFrameSelectorList> selectorStack, out IFocusCommentFrame frame)
        {
            bool Found = false;
            frame = null;

            switch (root)
            {
                case IFocusCommentFrame AsCommentFrame:
                    Found = GetFirstCommentFrameComment(AsCommentFrame, out frame);
                    break;
                case IFocusPanelFrame AsPanelFrame:
                    Found = GetFirstCommentFramePanel(AsPanelFrame, selectorStack, out frame);
                    break;
                case IFocusSelectionFrame AsSelectionFrame:
                    Found = GetFirstCommentFrameSelection(AsSelectionFrame, selectorStack, out frame);
                    break;
            }

            return Found;
        }

        private bool GetFirstCommentFrameComment(IFocusCommentFrame root, out IFocusCommentFrame frame)
        {
            frame = root;
            return true;
        }

        private bool GetFirstCommentFramePanel(IFocusPanelFrame root, IList<FocusFrameSelectorList> selectorStack, out IFocusCommentFrame frame)
        {
            frame = null;

            foreach (IFocusFrame Item in root.Items)
                if (GetFirstCommentFrame(Item, selectorStack, out frame))
                    return true;

            return false;
        }

        private bool GetFirstCommentFrameSelection(IFocusSelectionFrame root, IList<FocusFrameSelectorList> selectorStack, out IFocusCommentFrame frame)
        {
            frame = null;

            IFocusSelectableFrame SelectedFrame = null;

            foreach (IFocusSelectableFrame Item in root.Items)
            {
                foreach (FocusFrameSelectorList SelectorList in selectorStack)
                {
                    foreach (IFocusFrameSelector Selector in SelectorList)
                    {
                        if (Selector.SelectorType == NodeType)
                            if (Selector.SelectorName == Item.Name)
                            {
                                SelectedFrame = Item;
                                break;
                            }
                    }
                    if (SelectedFrame != null)
                        break;
                }
                if (SelectedFrame != null)
                    break;
            }

            if (SelectedFrame != null)
                if (GetFirstCommentFrame(SelectedFrame.Content, selectorStack, out frame))
                    return true;

            return false;
        }
        #endregion
    }
}
