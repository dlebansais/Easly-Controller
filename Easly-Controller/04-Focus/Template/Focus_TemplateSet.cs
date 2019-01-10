using BaseNode;
using BaseNodeHelper;
using EaslyController.Frame;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public interface IFocusTemplateSet : IFrameTemplateSet
    {
        /// <summary>
        /// Templates for nodes by their type.
        /// </summary>
        new IFocusTemplateReadOnlyDictionary NodeTemplateTable { get; }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        new IFocusTemplateReadOnlyDictionary BlockTemplateTable { get; }
    }

    /// <summary>
    /// Set of templates used to describe all possible nodes in the tree.
    /// </summary>
    public class FocusTemplateSet : FrameTemplateSet, IFocusTemplateSet
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusTemplateSet"/>.
        /// </summary>
        /// <param name="nodeTemplateTable">Templates for nodes by their type.</param>
        /// <param name="blockTemplateTable">Templates for blocks of nodes.</param>
        public FocusTemplateSet(IFocusTemplateReadOnlyDictionary nodeTemplateTable, IFocusTemplateReadOnlyDictionary blockTemplateTable)
            : base(nodeTemplateTable, blockTemplateTable)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Templates for nodes by their type.
        /// </summary>
        public new IFocusTemplateReadOnlyDictionary NodeTemplateTable { get { return (IFocusTemplateReadOnlyDictionary)base.NodeTemplateTable; } }

        /// <summary>
        /// Templates for blocks of nodes.
        /// </summary>
        public new IFocusTemplateReadOnlyDictionary BlockTemplateTable { get { return (IFocusTemplateReadOnlyDictionary)base.BlockTemplateTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTemplateDictionary object.
        /// </summary>
        protected override IFrameTemplateDictionary CreateTemplateDictionary(IDictionary<Type, IFrameTemplate> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusTemplateDictionary((IDictionary<Type, IFocusTemplate>)dictionary);
        }

        /// <summary>
        /// Creates a IxxxTemplateReadOnlyDictionary object.
        /// </summary>
        protected override IFrameTemplateReadOnlyDictionary CreateTemplateReadOnlyDictionary(IFrameTemplateDictionary dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusTemplateReadOnlyDictionary((IFocusTemplateDictionary)dictionary);
        }

        /// <summary>
        /// Creates a IxxxHorizontalPanelFrame object.
        /// </summary>
        protected override IFrameHorizontalPanelFrame CreateHorizontalPanelFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusHorizontalPanelFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalCollectionPlaceholderFrame object.
        /// </summary>
        protected override IFrameHorizontalCollectionPlaceholderFrame CreateHorizontalCollectionPlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusHorizontalCollectionPlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderFrame object.
        /// </summary>
        protected override IFramePlaceholderFrame CreatePlaceholderFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusPlaceholderFrame();
        }

        /// <summary>
        /// Creates a IxxxOptionalFrame object.
        /// </summary>
        protected override IFrameOptionalFrame CreateOptionalFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusOptionalFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalListFrame object.
        /// </summary>
        protected override IFrameHorizontalListFrame CreateHorizontalListFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusHorizontalListFrame();
        }

        /// <summary>
        /// Creates a IxxxHorizontalBlockListFrame object.
        /// </summary>
        protected override IFrameHorizontalBlockListFrame CreateHorizontalBlockListFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusHorizontalBlockListFrame();
        }

        /// <summary>
        /// Creates a IxxxValueFrame object.
        /// </summary>
        protected override IFrameValueFrame CreateValueFrame()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusValueFrame();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        protected override IFrameNodeTemplate CreateNodeTemplate()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusNodeTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplate object.
        /// </summary>
        protected override IFrameBlockTemplate CreateBlockTemplate()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusBlockTemplate();
        }

        /// <summary>
        /// Creates a IxxxTemplateSet object.
        /// </summary>
        protected override IFrameTemplateSet CreateDefaultTemplateSet(IFrameTemplateReadOnlyDictionary nodeTemplateTable, IFrameTemplateReadOnlyDictionary blockTemplateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusTemplateSet));
            return new FocusTemplateSet((IFocusTemplateReadOnlyDictionary)nodeTemplateTable, (IFocusTemplateReadOnlyDictionary)blockTemplateTable);
        }
        #endregion
    }
}
