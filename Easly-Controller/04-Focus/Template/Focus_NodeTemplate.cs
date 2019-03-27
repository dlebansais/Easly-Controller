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
        IFocusNamedFrame PropertyToFrame(string propertyName, IList<IFocusFrameSelectorList> selectorStack);

        /// <summary>
        /// Gets the frame associated to the comment.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        IFocusCommentFrame GetCommentFrame(IList<IFocusFrameSelectorList> selectorStack);
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

        /// <summary></summary>
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

            if (Root is IFocusSelectorPropertyFrame AsSelectorPropertyFrame)
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
        public virtual IFocusNamedFrame PropertyToFrame(string propertyName, IList<IFocusFrameSelectorList> selectorStack)
        {
            int ValueFrameIndex = 0;
            bool Found = GetFirstNamedFrame(Root, propertyName, selectorStack, false, ref ValueFrameIndex, out IFocusNamedFrame Result);
            Debug.Assert(Found);
            Debug.Assert(ValueFrameIndex > 0);
            Debug.Assert(Result != null);

            return Result;
        }

        private protected bool GetFirstNamedFrame(IFocusFrame root, string propertyName, IList<IFocusFrameSelectorList> selectorStack, bool reverseSearch, ref int valueFrameIndex, out IFocusNamedFrame frame)
        {
            frame = null;
            bool Found = false;

            if (root is IFocusNamedFrame AsNamedFrame)
            {
                valueFrameIndex++;

                if (AsNamedFrame.PropertyName == propertyName)
                {
                    frame = AsNamedFrame;
                    Found = true;
                }
            }

            if (root is IFocusPanelFrame AsPanelFrame)
            {
                int Count = AsPanelFrame.Items.Count;
                for (int i = 0; i < Count; i++)
                {
                    IFocusFrame Item = AsPanelFrame.Items[reverseSearch ? Count - 1 - i : i];

                    if (GetFirstNamedFrame(Item, propertyName, selectorStack, reverseSearch, ref valueFrameIndex, out frame))
                    {
                        Found = true;
                        break;
                    }
                }
            }

            else if (root is IFocusSelectionFrame AsSelectionFrame)
            {
                IFocusSelectableFrame SelectedFrame = null;

                foreach (IFocusSelectableFrame Item in AsSelectionFrame.Items)
                {
                    foreach (IFocusFrameSelectorList SelectorList in selectorStack)
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
                        Found = true;
            }

            return Found;
        }

        /// <summary>
        /// Gets the frame associated to the comment.
        /// This overload uses selectors to choose the correct frame.
        /// </summary>
        public virtual IFocusCommentFrame GetCommentFrame(IList<IFocusFrameSelectorList> selectorStack)
        {
            bool Found = GetFirstCommentFrame(Root, selectorStack, out IFocusCommentFrame Result);
            Debug.Assert(Found);
            Debug.Assert(Result != null);

            return Result;
        }

        /// <summary></summary>
        private protected virtual bool GetFirstCommentFrame(IFocusFrame root, IList<IFocusFrameSelectorList> selectorStack, out IFocusCommentFrame frame)
        {
            bool Found = false;
            frame = null;

            if (root is IFocusCommentFrame AsCommentFrame)
            {
                frame = AsCommentFrame;
                Found = true;
            }

            if (!Found && root is IFocusPanelFrame AsPanelFrame)
            {
                foreach (IFocusFrame Item in AsPanelFrame.Items)
                    if (GetFirstCommentFrame(Item, selectorStack, out frame))
                    {
                        Found = true;
                        break;
                    }
            }

            else if (root is IFocusSelectionFrame AsSelectionFrame)
            {
                IFocusSelectableFrame SelectedFrame = null;

                foreach (IFocusSelectableFrame Item in AsSelectionFrame.Items)
                {
                    foreach (IFocusFrameSelectorList SelectorList in selectorStack)
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
                        Found = true;
            }

            return Found;
        }
        #endregion
    }
}
