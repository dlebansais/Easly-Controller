﻿using EaslyController.Layout;
using System.IO;
using System.Text;
using System.Windows.Markup;

namespace TestDebug
{
    public class CoverageLayoutTemplateSet
    {
        #region Init
        static CoverageLayoutTemplateSet()
        {
            NodeTemplateDictionary = LoadTemplate(LayoutTemplateListString);
            LayoutTemplateReadOnlyDictionary LayoutCustomNodeTemplates = (LayoutTemplateReadOnlyDictionary)NodeTemplateDictionary.ToReadOnly();
            BlockTemplateDictionary = LoadTemplate(LayoutBlockTemplateString);
            LayoutTemplateReadOnlyDictionary LayoutCustomBlockTemplates = (LayoutTemplateReadOnlyDictionary)BlockTemplateDictionary.ToReadOnly();
            LayoutTemplateSet = new LayoutTemplateSet(LayoutCustomNodeTemplates, LayoutCustomBlockTemplates);
        }

        private static LayoutTemplateDictionary LoadTemplate(string s)
        {
            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                Templates = (LayoutTemplateList)XamlReader.Parse(s);

                LayoutTemplateDictionary TemplateDictionary = new LayoutTemplateDictionary();
                foreach (ILayoutTemplate Item in Templates)
                {
                    Item.Root.UpdateParent(Item, LayoutFrame.LayoutRoot);
                    RecursivelyCheckParent(Item.Root, Item.Root);
                    TemplateDictionary.Add(Item.NodeType, Item);
                }

                return TemplateDictionary;
            }
        }

        private CoverageLayoutTemplateSet()
        {
        }

        private static void RecursivelyCheckParent(ILayoutFrame rootFrame, ILayoutFrame frame)
        {
            EaslyController.Frame.IFrameFrame AsFrameRootFrame = rootFrame;
            EaslyController.Frame.IFrameFrame AsFrameFrame = frame;
            EaslyController.Focus.IFocusFrame AsFocusRootFrame = rootFrame;
            EaslyController.Focus.IFocusFrame AsFocusFrame = frame;

            //System.Diagnostics.Debug.Assert(false);
            System.Diagnostics.Debug.Assert(rootFrame.ParentTemplate == frame.ParentTemplate);
            System.Diagnostics.Debug.Assert(AsFrameRootFrame.ParentTemplate == AsFrameFrame.ParentTemplate);
            System.Diagnostics.Debug.Assert(AsFocusRootFrame.ParentTemplate == AsFocusFrame.ParentTemplate);

            System.Diagnostics.Debug.Assert(frame.ParentFrame != null || frame == rootFrame);

            if (frame is ILayoutBlockFrameWithVisibility AsBlockFrame)
            {
                ILayoutBlockFrameVisibility BlockVisibility = AsBlockFrame.BlockVisibility;
            }

            if (frame is ILayoutNodeFrameWithVisibility AsNodeFrameWithVisibility)
            {
                ILayoutNodeFrameVisibility Visibility = AsNodeFrameWithVisibility.Visibility;
            }

            if (frame is ILayoutPanelFrame AsPanelFrame)
            {
                foreach (ILayoutFrame Item in AsPanelFrame.Items)
                {
                    EaslyController.Frame.IFrameFrame AsFrameItem = Item;

                    System.Diagnostics.Debug.Assert(Item.ParentFrame == AsPanelFrame);
                    System.Diagnostics.Debug.Assert(AsFrameItem.ParentFrame == AsPanelFrame);

                    RecursivelyCheckParent(rootFrame, Item);
                }
            }

            else if (frame is ILayoutSelectionFrame AsSelectionFrame)
            {
                foreach (ILayoutFrame Item in AsSelectionFrame.Items)
                {
                    EaslyController.Frame.IFrameFrame AsFrameItem = Item;

                    System.Diagnostics.Debug.Assert(Item.ParentFrame == AsSelectionFrame);
                    System.Diagnostics.Debug.Assert(AsFrameItem.ParentFrame == AsSelectionFrame);

                    RecursivelyCheckParent(rootFrame, Item);
                }
            }

            else if (frame is ILayoutDiscreteFrame AsDiscreteFrame)
            {
                foreach (ILayoutKeywordFrame Item in AsDiscreteFrame.Items)
                {
                    EaslyController.Frame.IFrameFrame AsFrameItem = Item;

                    System.Diagnostics.Debug.Assert(Item.ParentFrame == AsDiscreteFrame);
                    System.Diagnostics.Debug.Assert(AsFrameItem.ParentFrame == AsDiscreteFrame);

                    RecursivelyCheckParent(rootFrame, Item);
                }
            }
        }
        #endregion

        #region Properties
        public static LayoutTemplateDictionary NodeTemplateDictionary { get; private set; }
        public static LayoutTemplateDictionary BlockTemplateDictionary { get; private set; }
        public static ILayoutTemplateSet LayoutTemplateSet { get; private set; }
        public static LayoutTemplateList Templates { get; private set; } = null!;
        #endregion

        #region Node Templates
        static string LayoutTemplateListString =
@"<LayoutTemplateList
    xmlns=""clr-namespace:EaslyController.Layout;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:easly=""clr-namespace:BaseNode;assembly=Easly-Language""
    xmlns:cov=""clr-namespace:Coverage;assembly=Test-Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <LayoutNodeTemplate NodeType=""{xaml:Type cov:Leaf}"" IsComplex=""True"" IsSimple=""True"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutTextValueFrame PropertyName=""Text""/>
            <LayoutKeywordFrame Text=""first"">
                <LayoutKeywordFrame.Visibility>
                    <LayoutNotFirstItemFrameVisibility/>
                </LayoutKeywordFrame.Visibility>
            </LayoutKeywordFrame>
            <LayoutKeywordFrame Text=""not first"">
                <LayoutKeywordFrame.Visibility>
                    <LayoutNotFirstItemFrameVisibility/>
                </LayoutKeywordFrame.Visibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type cov:Tree}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""Placeholder""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.Dot}"">
                <LayoutSymbolFrame.Visibility>
                    <LayoutComplexFrameVisibility PropertyName=""Placeholder""/>
                </LayoutSymbolFrame.Visibility>
            </LayoutSymbolFrame>
            <LayoutDiscreteFrame PropertyName=""ValueBoolean"">
                <LayoutKeywordFrame>True</LayoutKeywordFrame>
                <LayoutKeywordFrame>False</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <LayoutDiscreteFrame PropertyName=""ValueEnum"">
                <LayoutKeywordFrame>Any</LayoutKeywordFrame>
                <LayoutKeywordFrame>Reference</LayoutKeywordFrame>
                <LayoutKeywordFrame>Value</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type cov:Main}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.HorizontalLine}""/>
            <LayoutVerticalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.HorizontalLine}""/>
            </LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <LayoutHorizontalPanelFrame>
                    <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                </LayoutHorizontalPanelFrame>
                <LayoutPlaceholderFrame PropertyName=""PlaceholderTree""/>
                <LayoutPlaceholderFrame PropertyName=""PlaceholderLeaf""/>
                <LayoutOptionalFrame PropertyName=""UnassignedOptionalLeaf"" />
                <LayoutOptionalFrame PropertyName=""EmptyOptionalLeaf"" />
                <LayoutOptionalFrame PropertyName=""AssignedOptionalTree"" />
                <LayoutOptionalFrame PropertyName=""AssignedOptionalLeaf"" />
                <LayoutInsertFrame CollectionName=""LeafBlocks"">
                    <LayoutInsertFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""LeafBlocks""/>
                    </LayoutInsertFrame.Visibility>
                </LayoutInsertFrame>
                <LayoutHorizontalBlockListFrame PropertyName=""LeafBlocks"" Separator=""Comma"">
                    <LayoutHorizontalBlockListFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""LeafPath""/>
                    </LayoutHorizontalBlockListFrame.Visibility>
                    <LayoutHorizontalBlockListFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutHorizontalBlockListFrame.Selectors>
                </LayoutHorizontalBlockListFrame>
                <LayoutVerticalListFrame PropertyName=""LeafPath"" IsPreferred=""True"">
                    <LayoutVerticalListFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""LeafBlocks""/>
                    </LayoutVerticalListFrame.Visibility>
                </LayoutVerticalListFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""LeafBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutDiscreteFrame PropertyName=""ValueBoolean"">
                        <LayoutKeywordFrame>True</LayoutKeywordFrame>
                        <LayoutKeywordFrame>False</LayoutKeywordFrame>
                    </LayoutDiscreteFrame>
                </LayoutVerticalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""ValueEnum"">
                    <LayoutKeywordFrame>Any</LayoutKeywordFrame>
                    <LayoutKeywordFrame>Reference</LayoutKeywordFrame>
                    <LayoutKeywordFrame>Value</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutTextValueFrame PropertyName=""ValueString"">
                    <LayoutTextValueFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""PlaceholderTree""/>
                    </LayoutTextValueFrame.Visibility>
                </LayoutTextValueFrame>
                <LayoutCharacterFrame PropertyName=""ValueString"">
                    <LayoutCharacterFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""PlaceholderTree""/>
                    </LayoutCharacterFrame.Visibility>
                </LayoutCharacterFrame>
                <LayoutCharacterFrame PropertyName=""ValueString""/>
                <LayoutNumberFrame PropertyName=""ValueString"">
                    <LayoutNumberFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""PlaceholderTree""/>
                    </LayoutNumberFrame.Visibility>
                </LayoutNumberFrame>
                <LayoutNumberFrame PropertyName=""ValueString""/>
                <LayoutKeywordFrame Text=""end"">
                    <LayoutKeywordFrame.Visibility>
                        <LayoutMixedFrameVisibility>
                            <LayoutCountFrameVisibility PropertyName=""LeafBlocks""/>
                            <LayoutCountFrameVisibility PropertyName=""LeafPath""/>
                            <LayoutOptionalFrameVisibility PropertyName=""AssignedOptionalTree""/>
                            <LayoutOptionalFrameVisibility PropertyName=""UnassignedOptionalLeaf""/>
                        </LayoutMixedFrameVisibility>
                    </LayoutKeywordFrame.Visibility>
                </LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type cov:Root}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalBlockListFrame PropertyName=""MainBlocksH"">
                <LayoutHorizontalBlockListFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""LeafPathH""/>
                </LayoutHorizontalBlockListFrame.Visibility>
                <LayoutHorizontalBlockListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Overload""/>
                </LayoutHorizontalBlockListFrame.Selectors>
            </LayoutHorizontalBlockListFrame>
            <LayoutVerticalBlockListFrame PropertyName=""MainBlocksV"" HasTabulationMargin=""True"" Separator=""Line"">
                <LayoutVerticalBlockListFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""LeafPathV""/>
                </LayoutVerticalBlockListFrame.Visibility>
                <LayoutVerticalBlockListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Overload""/>
                </LayoutVerticalBlockListFrame.Selectors>
            </LayoutVerticalBlockListFrame>
            <LayoutInsertFrame CollectionName=""UnassignedOptionalMain.LeafBlocks""/>
            <LayoutOptionalFrame PropertyName=""UnassignedOptionalMain"" />
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""LeafPathH""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutTextValueFrame PropertyName=""ValueString""/>
            </LayoutVerticalPanelFrame>
            <LayoutHorizontalListFrame PropertyName=""LeafPathH"" Separator=""Comma"">
                <LayoutHorizontalListFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""MainBlocksH""/>
                </LayoutHorizontalListFrame.Visibility>
                <LayoutHorizontalListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Overload""/>
                </LayoutHorizontalListFrame.Selectors>
            </LayoutHorizontalListFrame>
            <LayoutVerticalListFrame PropertyName=""LeafPathV"" Separator=""Line"">
                <LayoutVerticalListFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""MainBlocksV""/>
                </LayoutVerticalListFrame.Visibility>
                <LayoutVerticalListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Overload""/>
                </LayoutVerticalListFrame.Selectors>
            </LayoutVerticalListFrame >
            <LayoutOptionalFrame PropertyName=""UnassignedOptionalLeaf"">
                <LayoutOptionalFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""MainBlocksV""/>
                </LayoutOptionalFrame.Visibility>
            </LayoutOptionalFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Assertion}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutOptionalFrame PropertyName=""Tag"" />
                <LayoutKeywordFrame RightMargin=""Whitespace"">:</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""BooleanExpression"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Attachment}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"" Text=""else"">
                    <LayoutKeywordFrame.Visibility>
                        <LayoutNotFirstItemFrameVisibility/>
                    </LayoutKeywordFrame.Visibility>
                </LayoutKeywordFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">as</LayoutKeywordFrame>
                <LayoutHorizontalBlockListFrame PropertyName=""AttachTypeBlocks"" Separator=""Comma""/>
                <LayoutInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""Instructions"" />
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Class}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""CopySpecification"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""CopySpecification"" DefaultValue=""1""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>any</LayoutKeywordFrame>
                    <LayoutKeywordFrame>reference</LayoutKeywordFrame>
                    <LayoutKeywordFrame>value</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutDiscreteFrame PropertyName=""Cloneable"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Cloneable""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>cloneable</LayoutKeywordFrame>
                    <LayoutKeywordFrame>single</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutDiscreteFrame PropertyName=""Comparable"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Comparable""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>comparable</LayoutKeywordFrame>
                    <LayoutKeywordFrame>incomparable</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutDiscreteFrame PropertyName=""IsAbstract"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""IsAbstract""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>instanceable</LayoutKeywordFrame>
                    <LayoutKeywordFrame>abstract</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutKeywordFrame>class</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName"" IsPreferred=""True"" LeftMargin=""Whitespace""/>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""FromIdentifier""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">from</LayoutKeywordFrame>
                    <LayoutOptionalFrame PropertyName=""FromIdentifier"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ImportBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>import</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ImportBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ImportBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""GenericBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>generic</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""GenericBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""GenericBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ExportBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>export</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ExportBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ExportBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""TypedefBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>typedef</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""TypedefBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""TypedefBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""InheritanceBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>inheritance</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""InheritanceBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""InheritanceBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""DiscreteBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>discrete</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""DiscreteBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""DiscreteBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ClassReplicateBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>replicate</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ClassReplicateBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ClassReplicateBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""FeatureBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>feature</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""FeatureBlocks"" ItemType=""{xaml:Type easly:AttributeFeature}""/>
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""FeatureBlocks"" HasTabulationMargin=""True"" Separator=""Line""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ConversionBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>conversion</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ConversionBlocks""/>
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ConversionBlocks"" HasTabulationMargin=""True"">
                    <LayoutVerticalBlockListFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                    </LayoutVerticalBlockListFrame.Selectors>
                </LayoutVerticalBlockListFrame>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""InvariantBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>invariant</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""InvariantBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""InvariantBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.Visibility>
                    <LayoutMixedFrameVisibility>
                        <LayoutCountFrameVisibility PropertyName=""ImportBlocks""/>
                        <LayoutCountFrameVisibility PropertyName=""GenericBlocks""/>
                        <LayoutCountFrameVisibility PropertyName=""ExportBlocks""/>
                        <LayoutCountFrameVisibility PropertyName=""TypedefBlocks""/>
                        <LayoutCountFrameVisibility PropertyName=""InheritanceBlocks""/>
                        <LayoutCountFrameVisibility PropertyName=""DiscreteBlocks""/>
                        <LayoutCountFrameVisibility PropertyName=""ClassReplicateBlocks""/>
                        <LayoutCountFrameVisibility PropertyName=""FeatureBlocks""/>
                        <LayoutCountFrameVisibility PropertyName=""ConversionBlocks""/>
                        <LayoutCountFrameVisibility PropertyName=""InvariantBlocks""/>
                    </LayoutMixedFrameVisibility>
                </LayoutKeywordFrame.Visibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ClassReplicate}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ReplicateName"" />
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutHorizontalBlockListFrame PropertyName=""PatternBlocks"" Separator=""Comma""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:CommandOverload}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ParameterBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>parameter</LayoutKeywordFrame>
                    <LayoutDiscreteFrame PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                        <LayoutDiscreteFrame.Visibility>
                            <LayoutDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                        </LayoutDiscreteFrame.Visibility>
                        <LayoutKeywordFrame>closed</LayoutKeywordFrame>
                        <LayoutKeywordFrame>open</LayoutKeywordFrame>
                    </LayoutDiscreteFrame>
                    <LayoutInsertFrame CollectionName=""ParameterBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ParameterBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""CommandBody"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Overload""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:CommandOverloadType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" RightMargin=""ThinSpace""/>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>parameter</LayoutKeywordFrame>
                        <LayoutDiscreteFrame PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                            <LayoutDiscreteFrame.Visibility>
                                <LayoutDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                            </LayoutDiscreteFrame.Visibility>
                            <LayoutKeywordFrame>closed</LayoutKeywordFrame>
                            <LayoutKeywordFrame>open</LayoutKeywordFrame>
                        </LayoutDiscreteFrame>
                        <LayoutInsertFrame CollectionName=""ParameterBlocks""/>
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""ParameterBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>require</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>exception</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" HasTabulationMargin=""True"">
                        <LayoutVerticalBlockListFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutKeywordFrame>end</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Conditional}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame Text=""else"" RightMargin=""Whitespace"">
                    <LayoutKeywordFrame.Visibility>
                        <LayoutNotFirstItemFrameVisibility/>
                    </LayoutKeywordFrame.Visibility>
                </LayoutKeywordFrame>
                <LayoutKeywordFrame>if</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""BooleanExpression"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame>then</LayoutKeywordFrame>
                <LayoutInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""Instructions"" />
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Constraint}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ParentType"" />
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""RenameBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>rename</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""RenameBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""RenameBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Continuation}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame>execute</LayoutKeywordFrame>
                <LayoutInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutPlaceholderFrame PropertyName=""Instructions"" />
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""CleanupBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>cleanup</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""CleanupBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""CleanupBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutVerticalPanelFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Discrete}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""EntityName"" />
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""NumericValue""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">=</LayoutKeywordFrame>
                <LayoutOptionalFrame PropertyName=""NumericValue"" />
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:EntityDeclaration}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""EntityName"" />
            <LayoutKeywordFrame RightMargin=""Whitespace"">:</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""EntityType"" />
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""DefaultValue""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">=</LayoutKeywordFrame>
                <LayoutOptionalFrame PropertyName=""DefaultValue"" />
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ExceptionHandler}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame>catch</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ExceptionIdentifier"" LeftMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""Instructions"" />
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Export}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""EntityName"" />
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutHorizontalBlockListFrame PropertyName=""ClassIdentifierBlocks"" Separator=""Comma"">
                <LayoutHorizontalBlockListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""ClassOrExport""/>
                </LayoutHorizontalBlockListFrame.Selectors>
            </LayoutHorizontalBlockListFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ExportChange}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutHorizontalBlockListFrame PropertyName=""IdentifierBlocks"" Separator=""Comma"">
                <LayoutHorizontalBlockListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutHorizontalBlockListFrame.Selectors>
            </LayoutHorizontalBlockListFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Generic}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName"" />
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""DefaultValue""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">=</LayoutKeywordFrame>
                    <LayoutOptionalFrame PropertyName=""DefaultValue"" />
                </LayoutHorizontalPanelFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ConstraintBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>conform to</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ConstraintBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ConstraintBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.Visibility>
                    <LayoutMixedFrameVisibility>
                        <LayoutCountFrameVisibility PropertyName=""ConstraintBlocks""/>
                    </LayoutMixedFrameVisibility>
                </LayoutKeywordFrame.Visibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:GlobalReplicate}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ReplicateName""/>
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutHorizontalListFrame PropertyName=""Patterns"" Separator=""Comma""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Import}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""Type"">
                    <LayoutKeywordFrame>latest</LayoutKeywordFrame>
                    <LayoutKeywordFrame>strict</LayoutKeywordFrame>
                    <LayoutKeywordFrame>stable</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutPlaceholderFrame PropertyName=""LibraryIdentifier"" LeftMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Library""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""FromIdentifier""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">from</LayoutKeywordFrame>
                    <LayoutOptionalFrame PropertyName=""FromIdentifier"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Source""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""RenameBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>rename</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""RenameBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""RenameBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutMixedFrameVisibility>
                        <LayoutCountFrameVisibility PropertyName=""RenameBlocks""/>
                    </LayoutMixedFrameVisibility>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutKeywordFrame>end</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Inheritance}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""Conformance"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Conformance""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>conformant</LayoutKeywordFrame>
                    <LayoutKeywordFrame>non-conformant</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutPlaceholderFrame PropertyName=""ParentType""/>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""RenameBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>rename</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""RenameBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""RenameBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ForgetBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>forget</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ForgetBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""ForgetBlocks"" HasTabulationMargin=""True"">
                        <LayoutVerticalBlockListFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""KeepBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>keep</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""KeepBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""KeepBlocks"" HasTabulationMargin=""True"">
                        <LayoutVerticalBlockListFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""DiscontinueBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>discontinue</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""DiscontinueBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""DiscontinueBlocks"" HasTabulationMargin=""True"">
                        <LayoutVerticalBlockListFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ExportChangeBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>export</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ExportChangeBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""ExportChangeBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""ForgetIndexer"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""ForgetIndexer""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>ignore indexer</LayoutKeywordFrame>
                    <LayoutKeywordFrame>forget indexer</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutDiscreteFrame PropertyName=""KeepIndexer"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""KeepIndexer""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>ignore indexer</LayoutKeywordFrame>
                    <LayoutKeywordFrame>keep indexer</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutDiscreteFrame PropertyName=""DiscontinueIndexer"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""DiscontinueIndexer""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>ignore indexer</LayoutKeywordFrame>
                    <LayoutKeywordFrame>discontinue indexer</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutKeywordFrame Text=""end"">
                    <LayoutKeywordFrame.Visibility>
                        <LayoutMixedFrameVisibility>
                            <LayoutCountFrameVisibility PropertyName=""RenameBlocks""/>
                            <LayoutCountFrameVisibility PropertyName=""ForgetBlocks""/>
                            <LayoutCountFrameVisibility PropertyName=""KeepBlocks""/>
                            <LayoutCountFrameVisibility PropertyName=""DiscontinueBlocks""/>
                            <LayoutDefaultDiscreteFrameVisibility PropertyName=""ForgetIndexer""/>
                            <LayoutDefaultDiscreteFrameVisibility PropertyName=""KeepIndexer""/>
                            <LayoutDefaultDiscreteFrameVisibility PropertyName=""DiscontinueIndexer""/>
                        </LayoutMixedFrameVisibility>
                    </LayoutKeywordFrame.Visibility>
                </LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Library}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">library</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName""/>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">from</LayoutKeywordFrame>
                    <LayoutOptionalFrame PropertyName=""FromIdentifier"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ImportBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>import</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ImportBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ImportBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ClassIdentifierBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>class</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ClassIdentifierBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ClassIdentifierBlocks"" HasTabulationMargin=""True"">
                    <LayoutVerticalBlockListFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                    </LayoutVerticalBlockListFrame.Selectors>
                </LayoutVerticalBlockListFrame>
            </LayoutVerticalPanelFrame>
                <LayoutKeywordFrame Text=""end"">
                    <LayoutKeywordFrame.Visibility>
                        <LayoutMixedFrameVisibility>
                            <LayoutCountFrameVisibility PropertyName=""ImportBlocks""/>
                            <LayoutCountFrameVisibility PropertyName=""ClassIdentifierBlocks""/>
                        </LayoutMixedFrameVisibility>
                    </LayoutKeywordFrame.Visibility>
                </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Name}"" IsSimple=""True"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutTextValueFrame PropertyName=""Text""/>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Pattern}"" IsSimple=""True"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutTextValueFrame PropertyName=""Text""/>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:QualifiedName}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalListFrame PropertyName=""Path"" Separator=""Dot"">
                <LayoutHorizontalListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutHorizontalListFrame.Selectors>
            </LayoutHorizontalListFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:QueryOverload}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ParameterBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>parameter</LayoutKeywordFrame>
                    <LayoutDiscreteFrame PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                        <LayoutDiscreteFrame.Visibility>
                            <LayoutDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                        </LayoutDiscreteFrame.Visibility>
                        <LayoutKeywordFrame>closed</LayoutKeywordFrame>
                        <LayoutKeywordFrame>open</LayoutKeywordFrame>
                    </LayoutDiscreteFrame>
                    <LayoutInsertFrame CollectionName=""ParameterBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ParameterBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ResultBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>result</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ResultBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ResultBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ModifiedQueryBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>modified</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ModifiedQueryBlocks"" HasTabulationMargin=""True"">
                    <LayoutVerticalBlockListFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                    </LayoutVerticalBlockListFrame.Selectors>
                </LayoutVerticalBlockListFrame>
            </LayoutVerticalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""QueryBody"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Overload""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">variant</LayoutKeywordFrame>
                <LayoutOptionalFrame PropertyName=""Variant"" />
            </LayoutHorizontalPanelFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:QueryOverloadType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" RightMargin=""ThinSpace""/>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>parameter</LayoutKeywordFrame>
                        <LayoutDiscreteFrame PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                            <LayoutDiscreteFrame.Visibility>
                                <LayoutDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                            </LayoutDiscreteFrame.Visibility>
                            <LayoutKeywordFrame>closed</LayoutKeywordFrame>
                            <LayoutKeywordFrame>open</LayoutKeywordFrame>
                        </LayoutDiscreteFrame>
                        <LayoutInsertFrame CollectionName=""ParameterBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""ParameterBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>return</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ResultBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""ResultBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>require</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>exception</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" HasTabulationMargin=""True"">
                        <LayoutVerticalBlockListFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutKeywordFrame>end</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Range}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" RightMargin=""ThinSpace"">
                <LayoutSymbolFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""RightExpression""/>
                </LayoutSymbolFrame.Visibility>
            </LayoutSymbolFrame>
            <LayoutPlaceholderFrame PropertyName=""LeftExpression"" />
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""RightExpression""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
                <LayoutOptionalFrame PropertyName=""RightExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Rename}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""DestinationIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Root}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>libraries</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""LibraryBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""LibraryBlocks"" />
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>classes</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ClassBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ClassBlocks"" />
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>replicates</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""Replicates"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalListFrame PropertyName=""Replicates"" />
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Scope}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>local</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>do</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""InstructionBlocks"" HasTabulationMargin=""True""/>
            </LayoutVerticalPanelFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Typedef}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""EntityName"" />
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">is</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""DefinedType"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:With}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">case</LayoutKeywordFrame>
                <LayoutHorizontalBlockListFrame PropertyName=""RangeBlocks"" Separator=""Comma""/>
                <LayoutInsertFrame CollectionName=""RangeBlocks""/>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""Instructions""/>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:AssignmentArgument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                <LayoutSymbolFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ParameterBlocks"" MaxInvisibleCount=""1""/>
                </LayoutSymbolFrame.Visibility>
            </LayoutSymbolFrame>
            <LayoutHorizontalBlockListFrame PropertyName=""ParameterBlocks"" Separator=""Comma"">
                <LayoutHorizontalBlockListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutHorizontalBlockListFrame.Selectors>
            </LayoutHorizontalBlockListFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                <LayoutSymbolFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ParameterBlocks"" MaxInvisibleCount=""1""/>
                </LayoutSymbolFrame.Visibility>
            </LayoutSymbolFrame>
            <LayoutSymbolFrame Symbol=""LeftArrow"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <LayoutPlaceholderFrame PropertyName=""Source""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PositionalArgument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""Source""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:DeferredBody}"">
        <LayoutSelectionFrame>
            <LayoutSelectableFrame Name=""Overload"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame IsFocusable=""true"" IsPreferred=""true"">deferred</LayoutKeywordFrame>
                        </LayoutHorizontalPanelFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Getter"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>getter</LayoutKeywordFrame>
                            <LayoutKeywordFrame IsFocusable=""true"" IsPreferred=""true"" LeftMargin=""Whitespace"">deferred</LayoutKeywordFrame>
                        </LayoutHorizontalPanelFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Setter"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>setter</LayoutKeywordFrame>
                            <LayoutKeywordFrame IsFocusable=""true"" IsPreferred=""true"" LeftMargin=""Whitespace"">deferred</LayoutKeywordFrame>
                        </LayoutHorizontalPanelFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
        </LayoutSelectionFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:EffectiveBody}"">
        <LayoutSelectionFrame>
            <LayoutSelectableFrame Name=""Overload"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>local</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame IsFocusable=""true"">do</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""BodyInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""BodyInstructionBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionHandlerBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>exception</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionHandlerBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""ExceptionHandlerBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Getter"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>local</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""BodyInstructionBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame IsFocusable=""true"">getter</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""BodyInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""BodyInstructionBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionHandlerBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>exception</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionHandlerBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""ExceptionHandlerBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Setter"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>local</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""BodyInstructionBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame IsFocusable=""true"">setter</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""BodyInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""BodyInstructionBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionHandlerBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>exception</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionHandlerBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""ExceptionHandlerBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
        </LayoutSelectionFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ExternBody}"">
        <LayoutSelectionFrame>
            <LayoutSelectableFrame Name=""Overload"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame IsFocusable=""true"">extern</LayoutKeywordFrame>
                        </LayoutHorizontalPanelFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Getter"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>getter</LayoutKeywordFrame>
                            <LayoutKeywordFrame IsFocusable=""true"" LeftMargin=""Whitespace"">extern</LayoutKeywordFrame>
                        </LayoutHorizontalPanelFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Setter"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>setter</LayoutKeywordFrame>
                            <LayoutKeywordFrame IsFocusable=""true"" LeftMargin=""Whitespace"">extern</LayoutKeywordFrame>
                        </LayoutHorizontalPanelFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
        </LayoutSelectionFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PrecursorBody}"">
        <LayoutSelectionFrame>
            <LayoutSelectableFrame Name=""Overload"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame IsFocusable=""true"">precursor</LayoutKeywordFrame>
                        </LayoutHorizontalPanelFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Getter"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>getter</LayoutKeywordFrame>
                            <LayoutKeywordFrame IsFocusable=""true"" LeftMargin=""Whitespace"">precursor</LayoutKeywordFrame>
                        </LayoutHorizontalPanelFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Setter"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>require</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""RequireBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""RequireBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>throw</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>setter</LayoutKeywordFrame>
                            <LayoutKeywordFrame IsFocusable=""true"" LeftMargin=""Whitespace"">precursor</LayoutKeywordFrame>
                        </LayoutHorizontalPanelFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame>
                        <LayoutVerticalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </LayoutVerticalPanelFrame.Visibility>
                        <LayoutHorizontalPanelFrame>
                            <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                            <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                        </LayoutHorizontalPanelFrame>
                        <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
        </LayoutSelectionFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:AgentExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>agent</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""BaseType""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" LeftMargin=""ThinSpace""/>
                <LayoutOptionalFrame PropertyName=""BaseType"" LeftMargin=""ThinSpace"" RightMargin=""Whitespace""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""Delegated"" LeftMargin=""Whitespace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:AssertionTagExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>tag</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""TagIdentifier"" LeftMargin=""Whitespace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:BinaryConditionalExpression}"" IsComplex=""True"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""LeftExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutDiscreteFrame PropertyName=""Conditional"" LeftMargin=""Whitespace"" RightMargin=""Whitespace"">
                <LayoutKeywordFrame>and</LayoutKeywordFrame>
                <LayoutKeywordFrame>or</LayoutKeywordFrame>
                <LayoutKeywordFrame>xor</LayoutKeywordFrame>
                <LayoutKeywordFrame>⇒</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""RightExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:BinaryOperatorExpression}"" IsComplex=""True"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""LeftExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""Operator"" LeftMargin=""Whitespace"" RightMargin=""Whitespace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""RightExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ClassConstantExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
            <LayoutPlaceholderFrame PropertyName=""ClassIdentifier"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <LayoutPlaceholderFrame PropertyName=""ConstantIdentifier"" LeftMargin=""ThinSpace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:CloneOfExpression}"" IsComplex=""True"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutDiscreteFrame PropertyName=""Type"" RightMargin=""Whitespace"">
                <LayoutDiscreteFrame.Visibility>
                    <LayoutDefaultDiscreteFrameVisibility PropertyName=""Type""/>
                </LayoutDiscreteFrame.Visibility>
                <LayoutKeywordFrame>shallow</LayoutKeywordFrame>
                <LayoutKeywordFrame>deep</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
            <LayoutKeywordFrame IsFocusable=""true"" RightMargin=""Whitespace"">clone of</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""Source""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""Source"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""Source""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:EntityExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>entity</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""Query"" LeftMargin=""Whitespace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:EqualityExpression}"" IsComplex=""True"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""LeftExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutDiscreteFrame PropertyName=""Comparison"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame>=</LayoutKeywordFrame>
                <LayoutKeywordFrame>!=</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
            <LayoutDiscreteFrame PropertyName=""Equality"">
                <LayoutDiscreteFrame.Visibility>
                    <LayoutDefaultDiscreteFrameVisibility PropertyName=""Equality""/>
                </LayoutDiscreteFrame.Visibility>
                <LayoutKeywordFrame>phys</LayoutKeywordFrame>
                <LayoutKeywordFrame>deep</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
            <LayoutKeywordFrame Text="" ""/>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""RightExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IndexQueryExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""IndexedExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""IndexedExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""IndexedExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:InitializedObjectExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ClassIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Type""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <LayoutVerticalBlockListFrame PropertyName=""AssignmentBlocks"" />
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
            <LayoutInsertFrame CollectionName=""AssignmentBlocks"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:KeywordEntityExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>entity</LayoutKeywordFrame>
            <LayoutDiscreteFrame PropertyName=""Value"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame>True</LayoutKeywordFrame>
                <LayoutKeywordFrame>False</LayoutKeywordFrame>
                <LayoutKeywordFrame>Current</LayoutKeywordFrame>
                <LayoutKeywordFrame>Value</LayoutKeywordFrame>
                <LayoutKeywordFrame>Result</LayoutKeywordFrame>
                <LayoutKeywordFrame>Retry</LayoutKeywordFrame>
                <LayoutKeywordFrame>Exception</LayoutKeywordFrame>
                <LayoutKeywordFrame>Indexer</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:KeywordExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutDiscreteFrame PropertyName=""Value"">
                <LayoutKeywordFrame>True</LayoutKeywordFrame>
                <LayoutKeywordFrame>False</LayoutKeywordFrame>
                <LayoutKeywordFrame>Current</LayoutKeywordFrame>
                <LayoutKeywordFrame>Value</LayoutKeywordFrame>
                <LayoutKeywordFrame>Result</LayoutKeywordFrame>
                <LayoutKeywordFrame>Retry</LayoutKeywordFrame>
                <LayoutKeywordFrame>Exception</LayoutKeywordFrame>
                <LayoutKeywordFrame>Indexer</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ManifestCharacterExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>'</LayoutKeywordFrame>
            <LayoutCharacterFrame PropertyName=""Text""/>
            <LayoutKeywordFrame>'</LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ManifestNumberExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutNumberFrame PropertyName=""Text""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ManifestStringExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>""</LayoutKeywordFrame>
            <LayoutTextValueFrame PropertyName=""Text""/>
            <LayoutKeywordFrame>""</LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:NewExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">new</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""Object"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:OldExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">old</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""Query"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PrecursorExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame IsFocusable=""true"">precursor</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""AncestorType""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" LeftMargin=""ThinSpace""/>
                <LayoutOptionalFrame PropertyName=""AncestorType"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" LeftMargin=""Whitespace"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PrecursorIndexExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame IsFocusable=""true"">precursor</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""AncestorType""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" LeftMargin=""ThinSpace""/>
                <LayoutOptionalFrame PropertyName=""AncestorType"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""Whitespace"" RightMargin=""ThinSpace""/>
                <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PreprocessorExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutDiscreteFrame PropertyName=""Value"">
                <LayoutKeywordFrame>DateAndTime</LayoutKeywordFrame>
                <LayoutKeywordFrame>CompilationDiscreteIdentifier</LayoutKeywordFrame>
                <LayoutKeywordFrame>ClassPath</LayoutKeywordFrame>
                <LayoutKeywordFrame>CompilerVersion</LayoutKeywordFrame>
                <LayoutKeywordFrame>ConformanceToStandard</LayoutKeywordFrame>
                <LayoutKeywordFrame>DiscreteClassIdentifier</LayoutKeywordFrame>
                <LayoutKeywordFrame>Counter</LayoutKeywordFrame>
                <LayoutKeywordFrame>Debugging</LayoutKeywordFrame>
                <LayoutKeywordFrame>RandomInteger</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:QueryExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""Query"" />
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ResultOfExpression}"" IsComplex=""True"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">result of</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""Source""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""Source"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""Source""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:UnaryNotExpression}"" IsComplex=""True"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">not</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""RightExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:UnaryOperatorExpression}"" IsComplex=""True"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""Operator"" RightMargin=""Whitespace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutPlaceholderFrame PropertyName=""RightExpression"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutComplexFrameVisibility PropertyName=""RightExpression""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:AttributeFeature}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""Export"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>exported</LayoutKeywordFrame>
                    <LayoutKeywordFrame>private</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName"" />
                <LayoutKeywordFrame>:</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace""/>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>ensure</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""EnsureBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""EnsureBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame>export to</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"" LeftMargin=""Whitespace"">
                        <LayoutPlaceholderFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.Visibility>
                    <LayoutMixedFrameVisibility>
                        <LayoutCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutMixedFrameVisibility>
                </LayoutKeywordFrame.Visibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ConstantFeature}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""Export"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>exported</LayoutKeywordFrame>
                    <LayoutKeywordFrame>private</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName"" />
                <LayoutKeywordFrame>:</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame>=</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ConstantValue"" LeftMargin=""Whitespace""/>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame>export to</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"" LeftMargin=""Whitespace"">
                        <LayoutPlaceholderFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.Visibility>
                    <LayoutMixedFrameVisibility>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutMixedFrameVisibility>
                </LayoutKeywordFrame.Visibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:CreationFeature}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""Export"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>exported</LayoutKeywordFrame>
                    <LayoutKeywordFrame>private</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame>creation</LayoutKeywordFrame>
                <LayoutInsertFrame CollectionName=""OverloadBlocks"" />
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalBlockListFrame PropertyName=""OverloadBlocks""/>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame>export to</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"" LeftMargin=""Whitespace"">
                        <LayoutPlaceholderFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:FunctionFeature}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""Export"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>exported</LayoutKeywordFrame>
                    <LayoutKeywordFrame>private</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName"" />
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame LeftMargin=""Whitespace"">once per</LayoutKeywordFrame>
                    <LayoutDiscreteFrame PropertyName=""Once"" LeftMargin=""Whitespace"">
                        <LayoutDiscreteFrame.Visibility>
                            <LayoutDefaultDiscreteFrameVisibility PropertyName=""Once""/>
                        </LayoutDiscreteFrame.Visibility>
                        <LayoutKeywordFrame>normal</LayoutKeywordFrame>
                        <LayoutKeywordFrame>object</LayoutKeywordFrame>
                        <LayoutKeywordFrame>processor</LayoutKeywordFrame>
                        <LayoutKeywordFrame>process</LayoutKeywordFrame>
                    </LayoutDiscreteFrame>
                </LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame LeftMargin=""Whitespace"">function</LayoutKeywordFrame>
                <LayoutInsertFrame CollectionName=""OverloadBlocks"" />
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalBlockListFrame PropertyName=""OverloadBlocks""/>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame>export to</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"" LeftMargin=""Whitespace"">
                        <LayoutPlaceholderFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IndexerFeature}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""Export"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>exported</LayoutKeywordFrame>
                    <LayoutKeywordFrame>private</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutKeywordFrame IsFocusable=""true"">indexer</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace""/>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""IndexParameterBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutDiscreteFrame PropertyName=""ParameterEnd"" RightMargin=""Whitespace"">
                            <LayoutDiscreteFrame.Visibility>
                                <LayoutDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                            </LayoutDiscreteFrame.Visibility>
                            <LayoutKeywordFrame>closed</LayoutKeywordFrame>
                            <LayoutKeywordFrame>open</LayoutKeywordFrame>
                        </LayoutDiscreteFrame>
                        <LayoutKeywordFrame>parameter</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ModifiedQueryBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>modify</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                        <LayoutHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutOptionalFrame PropertyName=""GetterBody"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Getter""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutOptionalFrame PropertyName=""SetterBody"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Setter""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                </LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame>export to</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"" LeftMargin=""Whitespace"">
                        <LayoutPlaceholderFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ProcedureFeature}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""Export"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>exported</LayoutKeywordFrame>
                    <LayoutKeywordFrame>private</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName"" />
                <LayoutKeywordFrame LeftMargin=""Whitespace"">procedure</LayoutKeywordFrame>
                <LayoutInsertFrame CollectionName=""OverloadBlocks"" />
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalBlockListFrame PropertyName=""OverloadBlocks""/>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame>export to</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"" LeftMargin=""Whitespace"">
                        <LayoutPlaceholderFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PropertyFeature}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""Export"" RightMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>exported</LayoutKeywordFrame>
                    <LayoutKeywordFrame>private</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame>is</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace""/>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ModifiedQueryBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>modify</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                        <LayoutHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"" Separator=""Comma"">
                            <LayoutHorizontalBlockListFrame.Selectors>
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
                    <LayoutOptionalFrame PropertyName=""GetterBody"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Getter""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                    <LayoutOptionalFrame PropertyName=""SetterBody"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Setter""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame>export to</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"" LeftMargin=""Whitespace"">
                        <LayoutPlaceholderFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.Visibility>
                    <LayoutMixedFrameVisibility>
                        <LayoutCountFrameVisibility PropertyName=""ModifiedQueryBlocks""/>
                        <LayoutOptionalFrameVisibility PropertyName=""GetterBody""/>
                        <LayoutOptionalFrameVisibility PropertyName=""SetterBody""/>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutMixedFrameVisibility>
                </LayoutKeywordFrame.Visibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:Identifier}"" IsSimple=""True"">
        <LayoutSelectionFrame>
            <LayoutSelectableFrame Name=""Identifier"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Feature"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Class"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""ClassOrExport"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Export"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Library"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Source"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Type"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Pattern"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
        </LayoutSelectionFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:AsLongAsInstruction}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame>as long as</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ContinueCondition"" LeftMargin=""Whitespace""/>
                <LayoutInsertFrame CollectionName=""ContinuationBlocks""/>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalBlockListFrame PropertyName=""ContinuationBlocks""/>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""ElseInstructions""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>else</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </LayoutHorizontalPanelFrame>
                    <LayoutOptionalFrame PropertyName=""ElseInstructions"" />
                </LayoutVerticalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:AssignmentInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalBlockListFrame PropertyName=""DestinationBlocks"" Separator=""Comma""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <LayoutPlaceholderFrame PropertyName=""Source"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:AttachmentInstruction}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">attach</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""Source"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">to</LayoutKeywordFrame>
                <LayoutHorizontalBlockListFrame PropertyName=""EntityNameBlocks"" Separator=""Comma""/>
                <LayoutInsertFrame CollectionName=""AttachmentBlocks"" />
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalBlockListFrame PropertyName=""AttachmentBlocks""/>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""ElseInstructions""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>else</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </LayoutHorizontalPanelFrame>
                    <LayoutOptionalFrame PropertyName=""ElseInstructions"" />
                </LayoutVerticalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:CheckInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">check</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""BooleanExpression"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:CommandInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutInsertFrame CollectionName=""Command.Path""/>
            <LayoutPlaceholderFrame PropertyName=""Command"" />
            <LayoutHorizontalPanelFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
                <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace"">
                    <LayoutSymbolFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </LayoutSymbolFrame.Visibility>
                </LayoutSymbolFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:CreateInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>create</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""EntityIdentifier"" LeftMargin=""Whitespace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame LeftMargin=""Whitespace"">with</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""CreationRoutineIdentifier"" LeftMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                    <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
                    <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace""/>
                </LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""Processor""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">same processor as</LayoutKeywordFrame>
                    <LayoutOptionalFrame PropertyName=""Processor"" />
                </LayoutHorizontalPanelFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:DebugInstruction}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame>debug</LayoutKeywordFrame>
                <LayoutInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""Instructions"" />
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame>end</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ForLoopInstruction}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>loop</LayoutKeywordFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>local</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""InitInstructionBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>init</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""InitInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""InitInstructionBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>while</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""WhileCondition"" LeftMargin=""Whitespace""/>
                    <LayoutInsertFrame CollectionName=""LoopInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""LoopInstructionBlocks"" HasTabulationMargin=""True""/>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""IterationInstructionBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>iterate</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""IterationInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""IterationInstructionBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""InvariantBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>invariant</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""InvariantBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""InvariantBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""Variant""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame RightMargin=""Whitespace"">variant</LayoutKeywordFrame>
                    <LayoutOptionalFrame PropertyName=""Variant"" />
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IfThenElseInstruction}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutVerticalBlockListFrame PropertyName=""ConditionalBlocks""/>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""ElseInstructions""/>
                </LayoutVerticalPanelFrame.Visibility>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>else</LayoutKeywordFrame>
                    <LayoutInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                </LayoutHorizontalPanelFrame>
                <LayoutOptionalFrame PropertyName=""ElseInstructions"" />
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IndexAssignmentInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""Destination"" RightMargin=""ThinSpace""/>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
            </LayoutHorizontalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <LayoutPlaceholderFrame PropertyName=""Source"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:InspectInstruction}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">inspect</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""Source"" />
                <LayoutInsertFrame CollectionName=""WithBlocks"" />
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutVerticalBlockListFrame PropertyName=""WithBlocks""/>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""ElseInstructions""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>else</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </LayoutHorizontalPanelFrame>
                    <LayoutOptionalFrame PropertyName=""ElseInstructions"" />
                </LayoutVerticalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:KeywordAssignmentInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutDiscreteFrame PropertyName=""Destination"">
                <LayoutKeywordFrame>True</LayoutKeywordFrame>
                <LayoutKeywordFrame>False</LayoutKeywordFrame>
                <LayoutKeywordFrame>Current</LayoutKeywordFrame>
                <LayoutKeywordFrame>Value</LayoutKeywordFrame>
                <LayoutKeywordFrame>Result</LayoutKeywordFrame>
                <LayoutKeywordFrame>Retry</LayoutKeywordFrame>
                <LayoutKeywordFrame>Exception</LayoutKeywordFrame>
                <LayoutKeywordFrame>Indexer</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <LayoutPlaceholderFrame PropertyName=""Source"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:OverLoopInstruction}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">over</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""OverList"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">for each</LayoutKeywordFrame>
                <LayoutHorizontalBlockListFrame PropertyName=""IndexerBlocks"" Separator=""Comma""/>
                <LayoutDiscreteFrame PropertyName=""Iteration"" LeftMargin=""Whitespace"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Iteration""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>Single</LayoutKeywordFrame>
                    <LayoutKeywordFrame>Nested</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutInsertFrame CollectionName=""LoopInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                <LayoutPlaceholderFrame PropertyName=""LoopInstructions"" />
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""ExitEntityName""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame>exit if</LayoutKeywordFrame>
                    <LayoutOptionalFrame PropertyName=""ExitEntityName"" LeftMargin=""Whitespace"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""InvariantBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>invariant</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""InvariantBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""InvariantBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PrecursorIndexAssignmentInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>precursor</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""AncestorType""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""ThinSpace"">from</LayoutKeywordFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" RightMargin=""ThinSpace""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" RightMargin=""Whitespace""/>
                <LayoutOptionalFrame PropertyName=""AncestorType"" />
            </LayoutHorizontalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <LayoutPlaceholderFrame PropertyName=""Source"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PrecursorInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame IsFocusable=""true"">precursor</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""AncestorType""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""ThinSpace"">from</LayoutKeywordFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" RightMargin=""ThinSpace""/>
                <LayoutOptionalFrame PropertyName=""AncestorType"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}"" RightMargin=""Whitespace""/>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace""/>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:RaiseEventInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">raise</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""QueryIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutDiscreteFrame PropertyName=""Event"" LeftMargin=""Whitespace"">
                <LayoutDiscreteFrame.Visibility>
                    <LayoutDefaultDiscreteFrameVisibility PropertyName=""Event""/>
                </LayoutDiscreteFrame.Visibility>
                <LayoutKeywordFrame>once</LayoutKeywordFrame>
                <LayoutKeywordFrame>forever</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ReleaseInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>release</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""EntityName"" LeftMargin=""Whitespace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ThrowInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">throw</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""ExceptionType"" RightMargin=""Whitespace""/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">with</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""CreationRoutine"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"" LeftMargin=""ThinSpace""/>
            </LayoutHorizontalPanelFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:AnchoredType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">like</LayoutKeywordFrame>
            <LayoutDiscreteFrame PropertyName=""AnchorKind"" RightMargin=""Whitespace"">
                <LayoutDiscreteFrame.Visibility>
                    <LayoutDefaultDiscreteFrameVisibility PropertyName=""AnchorKind""/>
                </LayoutDiscreteFrame.Visibility>
                <LayoutKeywordFrame>declaration</LayoutKeywordFrame>
                <LayoutKeywordFrame>creation</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
            <LayoutPlaceholderFrame PropertyName=""AnchoredName"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:FunctionType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">function</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""BaseType"" RightMargin=""Whitespace""/>
            <LayoutHorizontalBlockListFrame PropertyName=""OverloadBlocks""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:GenericType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ClassIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <LayoutHorizontalBlockListFrame PropertyName=""TypeArgumentBlocks"" Separator=""Comma""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
            <LayoutDiscreteFrame PropertyName=""Sharing"" LeftMargin=""Whitespace"">
                <LayoutDiscreteFrame.Visibility>
                    <LayoutDefaultDiscreteFrameVisibility PropertyName=""Sharing""/>
                </LayoutDiscreteFrame.Visibility>
                <LayoutKeywordFrame>not shared</LayoutKeywordFrame>
                <LayoutKeywordFrame>readwrite</LayoutKeywordFrame>
                <LayoutKeywordFrame>read-only</LayoutKeywordFrame>
                <LayoutKeywordFrame>write-only</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IndexerType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""BaseType"" />
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""Whitespace"" RightMargin=""ThinSpace""/>
            <LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>indexer</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace""/>
                </LayoutHorizontalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""IndexParameterBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>parameter</LayoutKeywordFrame>
                        <LayoutDiscreteFrame PropertyName=""ParameterEnd"" LeftMargin=""Whitespace"">
                            <LayoutDiscreteFrame.Visibility>
                                <LayoutDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                            </LayoutDiscreteFrame.Visibility>
                            <LayoutKeywordFrame>closed</LayoutKeywordFrame>
                            <LayoutKeywordFrame>open</LayoutKeywordFrame>
                        </LayoutDiscreteFrame>
                        <LayoutInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""IndexerKind"">
                    <LayoutKeywordFrame>read-only</LayoutKeywordFrame>
                    <LayoutKeywordFrame>write-only</LayoutKeywordFrame>
                    <LayoutKeywordFrame>readwrite</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""GetRequireBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>getter require</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""GetRequireBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""GetRequireBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""GetEnsureBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>getter ensure</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""GetExceptionIdentifierBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>getter exception</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"" HasTabulationMargin=""True"">
                        <LayoutVerticalBlockListFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""SetRequireBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>setter require</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""SetRequireBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""SetRequireBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""SetEnsureBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>setter ensure</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""SetEnsureBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""SetEnsureBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""SetExceptionIdentifierBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>setter exception</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"" HasTabulationMargin=""True"">
                        <LayoutVerticalBlockListFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutKeywordFrame>end</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:KeywordAnchoredType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">like</LayoutKeywordFrame>
            <LayoutDiscreteFrame PropertyName=""Anchor"">
                <LayoutKeywordFrame>True</LayoutKeywordFrame>
                <LayoutKeywordFrame>False</LayoutKeywordFrame>
                <LayoutKeywordFrame>Current</LayoutKeywordFrame>
                <LayoutKeywordFrame>Value</LayoutKeywordFrame>
                <LayoutKeywordFrame>Result</LayoutKeywordFrame>
                <LayoutKeywordFrame>Retry</LayoutKeywordFrame>
                <LayoutKeywordFrame>Exception</LayoutKeywordFrame>
                <LayoutKeywordFrame>Indexer</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ProcedureType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">procedure</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""BaseType"" RightMargin=""Whitespace""/>
            <LayoutHorizontalBlockListFrame PropertyName=""OverloadBlocks""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PropertyType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""BaseType"" />
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""Whitespace"" RightMargin=""ThinSpace""/>
            <LayoutVerticalPanelFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutKeywordFrame>is</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace""/>
                </LayoutHorizontalPanelFrame>
                <LayoutDiscreteFrame PropertyName=""PropertyKind"">
                    <LayoutKeywordFrame>read-only</LayoutKeywordFrame>
                    <LayoutKeywordFrame>write-only</LayoutKeywordFrame>
                    <LayoutKeywordFrame>readwrite</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""GetEnsureBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>getter ensure</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""GetExceptionIdentifierBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>getter exception</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"" HasTabulationMargin=""True"">
                        <LayoutVerticalBlockListFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""SetRequireBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>setter require</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""SetRequireBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""SetRequireBlocks"" HasTabulationMargin=""True""/>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutCountFrameVisibility PropertyName=""SetExceptionIdentifierBlocks""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>setter exception</LayoutKeywordFrame>
                        <LayoutInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </LayoutHorizontalPanelFrame>
                    <LayoutVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"" HasTabulationMargin=""True"">
                        <LayoutVerticalBlockListFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutKeywordFrame>end</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:SimpleType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ClassIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Type""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutDiscreteFrame PropertyName=""Sharing"" LeftMargin=""Whitespace"">
                <LayoutDiscreteFrame.Visibility>
                    <LayoutDefaultDiscreteFrameVisibility PropertyName=""Sharing""/>
                </LayoutDiscreteFrame.Visibility>
                <LayoutKeywordFrame>not shared</LayoutKeywordFrame>
                <LayoutKeywordFrame>readwrite</LayoutKeywordFrame>
                <LayoutKeywordFrame>read-only</LayoutKeywordFrame>
                <LayoutKeywordFrame>write-only</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:TupleType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>tuple</LayoutKeywordFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <LayoutVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
            <LayoutDiscreteFrame PropertyName=""Sharing"" LeftMargin=""Whitespace"">
                <LayoutDiscreteFrame.Visibility>
                    <LayoutDefaultDiscreteFrameVisibility PropertyName=""Sharing""/>
                </LayoutDiscreteFrame.Visibility>
                <LayoutKeywordFrame>not shared</LayoutKeywordFrame>
                <LayoutKeywordFrame>readwrite</LayoutKeywordFrame>
                <LayoutKeywordFrame>read-only</LayoutKeywordFrame>
                <LayoutKeywordFrame>write-only</LayoutKeywordFrame>
            </LayoutDiscreteFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:AssignmentTypeArgument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ParameterIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <LayoutPlaceholderFrame PropertyName=""Source"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:PositionalTypeArgument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""Source""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
</LayoutTemplateList>";
        #endregion

        #region Block Templates
        static string LayoutBlockTemplateString =
@"<LayoutTemplateList
    xmlns=""clr-namespace:EaslyController.Layout;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:easly=""clr-namespace:BaseNode;assembly=Easly-Language""
    xmlns:cov=""clr-namespace:Coverage;assembly=Test-Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,cov:Leaf}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,cov:Tree}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame Separator=""Line"">
                <LayoutVerticalCollectionPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                </LayoutVerticalCollectionPlaceholderFrame.Selectors>
            </LayoutVerticalCollectionPlaceholderFrame>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,cov:Main}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutCommentFrame/>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutVerticalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Argument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Assertion}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:AssignmentArgument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Attachment}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Class}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ClassReplicate}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:CommandOverload}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:CommandOverloadType}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Conditional}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Constraint}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Continuation}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Discrete}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:EntityDeclaration}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ExceptionHandler}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Export}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ExportChange}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Feature}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Generic}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Identifier}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame>
                <LayoutHorizontalCollectionPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                </LayoutHorizontalCollectionPlaceholderFrame.Selectors>
            </LayoutHorizontalCollectionPlaceholderFrame>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Import}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Inheritance}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Instruction}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Library}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Name}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ObjectType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Pattern}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:QualifiedName}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:QueryOverload}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:QueryOverloadType}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Range}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Rename}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:TypeArgument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:Typedef}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:With}"">
        <LayoutVerticalPanelFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame>Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <LayoutKeywordFrame>From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame>All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
</LayoutTemplateList>
";
        #endregion
    }
}
