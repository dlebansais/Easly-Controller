using EaslyController.Layout;
using System.IO;
using System.Text;
using System.Windows.Markup;

namespace EaslyEdit
{
    internal class CustomLayoutTemplateSet
    {
        #region Init
        static CustomLayoutTemplateSet()
        {
            NodeTemplateDictionary = LoadTemplate(LayoutTemplateListString);
            ILayoutTemplateReadOnlyDictionary LayoutCustomNodeTemplates = NodeTemplateDictionary.ToReadOnly() as ILayoutTemplateReadOnlyDictionary;
            BlockTemplateDictionary = LoadTemplate(LayoutBlockTemplateString);
            ILayoutTemplateReadOnlyDictionary LayoutCustomBlockTemplates = BlockTemplateDictionary.ToReadOnly() as ILayoutTemplateReadOnlyDictionary;
            LayoutTemplateSet = new LayoutTemplateSet(LayoutCustomNodeTemplates, LayoutCustomBlockTemplates);
        }

        private static ILayoutTemplateDictionary LoadTemplate(string s)
        {
            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                ILayoutTemplateList Templates = XamlReader.Parse(s) as ILayoutTemplateList;

                LayoutTemplateDictionary TemplateDictionary = new LayoutTemplateDictionary();
                foreach (ILayoutTemplate Item in Templates)
                {
                    Item.Root.UpdateParent(Item, LayoutFrame.LayoutRoot);
                    TemplateDictionary.Add(Item.NodeType, Item);
                }

                return TemplateDictionary;
            }
        }

        private CustomLayoutTemplateSet()
        {
        }
        #endregion

        #region Properties
        public static ILayoutTemplateDictionary NodeTemplateDictionary { get; private set; }
        public static ILayoutTemplateDictionary BlockTemplateDictionary { get; private set; }
        public static ILayoutTemplateSet LayoutTemplateSet { get; private set; }
        #endregion

        #region Node Templates
        static string LayoutTemplateListString =
@"<LayoutTemplateList
    xmlns=""clr-namespace:EaslyController.Layout;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:easly=""clr-namespace:BaseNode;assembly=Easly-Language""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAssertion}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""Tag""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutOptionalFrame PropertyName=""Tag"" />
                <LayoutKeywordFrame RightMargin=""Whitespace"">:</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""BooleanExpression"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAttachment}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IClass}"">
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
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
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IClassReplicate}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ReplicateName"" />
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutHorizontalBlockListFrame PropertyName=""PatternBlocks"" Separator=""Comma""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ICommandOverload}"">
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
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IDeferredBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IEffectiveBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IExternBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IPrecursorBody}"" SelectorName=""Overload""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ICommandOverloadType}"">
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutKeywordFrame IsFocusable=""True"">end</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IConditional}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IConstraint}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IContinuation}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame IsFocusable=""True"">step</LayoutKeywordFrame>
            <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IDiscrete}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IEntityDeclaration}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IExceptionHandler}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame>catch</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ExceptionIdentifier"" LeftMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""Instructions"" />
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IExport}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""EntityName"" />
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutHorizontalBlockListFrame PropertyName=""ClassIdentifierBlocks"" Separator=""Comma"">
                <LayoutHorizontalBlockListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""ClassOrExport""/>
                </LayoutHorizontalBlockListFrame.Selectors>
            </LayoutHorizontalBlockListFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IExportChange}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Export""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutInsertFrame CollectionName=""IdentifierBlocks"" />
            <LayoutHorizontalBlockListFrame PropertyName=""IdentifierBlocks"" Separator=""Comma"">
                <LayoutHorizontalBlockListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                </LayoutHorizontalBlockListFrame.Selectors>
            </LayoutHorizontalBlockListFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IGeneric}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IGlobalReplicate}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ReplicateName""/>
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutHorizontalListFrame PropertyName=""Patterns"" Separator=""Comma""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IImport}"">
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
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Library""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""FromIdentifier""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">from</LayoutKeywordFrame>
                    <LayoutOptionalFrame PropertyName=""FromIdentifier"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Source""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IInheritance}"">
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ILibrary}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">library</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""EntityName""/>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""FromIdentifier""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">from</LayoutKeywordFrame>
                    <LayoutOptionalFrame PropertyName=""FromIdentifier"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IName}"" IsSimple=""True"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPattern}"" IsSimple=""True"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutTextValueFrame PropertyName=""Text""/>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IQualifiedName}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalListFrame PropertyName=""Path"" Separator=""Dot"">
                <LayoutHorizontalListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                </LayoutHorizontalListFrame.Selectors>
            </LayoutHorizontalListFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IQueryOverload}"">
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
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                    </LayoutVerticalBlockListFrame.Selectors>
                </LayoutVerticalBlockListFrame>
            </LayoutVerticalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""QueryBody"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IDeferredBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IEffectiveBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IExternBody}"" SelectorName=""Overload""/>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IPrecursorBody}"" SelectorName=""Overload""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""Variant""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">variant</LayoutKeywordFrame>
                <LayoutOptionalFrame PropertyName=""Variant"" />
            </LayoutHorizontalPanelFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IQueryOverloadType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" RightMargin=""ThinSpace""/>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutHorizontalPanelFrame>
                        <LayoutHorizontalPanelFrame.Visibility>
                            <LayoutCountFrameVisibility PropertyName=""ParameterBlocks""/>
                        </LayoutHorizontalPanelFrame.Visibility>
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutKeywordFrame IsFocusable=""True"">end</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IRange}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IRename}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">to</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""DestinationIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IRoot}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IScope}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ITypedef}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""EntityName"" />
            <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""Whitespace"">is</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""DefinedType"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IWith}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAssignmentArgument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"" RightMargin=""ThinSpace"">
                <LayoutSymbolFrame.Visibility>
                    <LayoutCountFrameVisibility PropertyName=""ParameterBlocks"" MaxInvisibleCount=""1""/>
                </LayoutSymbolFrame.Visibility>
            </LayoutSymbolFrame>
            <LayoutHorizontalBlockListFrame PropertyName=""ParameterBlocks"" Separator=""Comma"">
                <LayoutHorizontalBlockListFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPositionalArgument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""Source""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IDeferredBody}"">
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IEffectiveBody}"">
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IExternBody}"">
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPrecursorBody}"">
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame IsFocusable=""true"">precursor</LayoutKeywordFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutHorizontalPanelFrame.Visibility>
                                <LayoutOptionalFrameVisibility PropertyName=""AncestorType""/>
                            </LayoutHorizontalPanelFrame.Visibility>
                            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" LeftMargin=""ThinSpace""/>
                            <LayoutOptionalFrame PropertyName=""AncestorType"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
                        </LayoutHorizontalPanelFrame>
                    </LayoutHorizontalPanelFrame>
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>getter</LayoutKeywordFrame>
                        <LayoutKeywordFrame IsFocusable=""true"" LeftMargin=""Whitespace"">precursor</LayoutKeywordFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutHorizontalPanelFrame.Visibility>
                                <LayoutOptionalFrameVisibility PropertyName=""AncestorType""/>
                            </LayoutHorizontalPanelFrame.Visibility>
                            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" LeftMargin=""ThinSpace""/>
                            <LayoutOptionalFrame PropertyName=""AncestorType"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
                        </LayoutHorizontalPanelFrame>
                    </LayoutHorizontalPanelFrame>
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                    <LayoutHorizontalPanelFrame>
                        <LayoutKeywordFrame>setter</LayoutKeywordFrame>
                        <LayoutKeywordFrame IsFocusable=""true"" LeftMargin=""Whitespace"">precursor</LayoutKeywordFrame>
                        <LayoutHorizontalPanelFrame>
                            <LayoutHorizontalPanelFrame.Visibility>
                                <LayoutOptionalFrameVisibility PropertyName=""AncestorType""/>
                            </LayoutHorizontalPanelFrame.Visibility>
                            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" LeftMargin=""ThinSpace""/>
                            <LayoutOptionalFrame PropertyName=""AncestorType"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
                        </LayoutHorizontalPanelFrame>
                    </LayoutHorizontalPanelFrame>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAgentExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>agent</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""BaseType""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" LeftMargin=""Whitespace""/>
                <LayoutOptionalFrame PropertyName=""BaseType"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </LayoutHorizontalPanelFrame>
            <LayoutPlaceholderFrame PropertyName=""Delegated"" LeftMargin=""Whitespace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAssertionTagExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>tag</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""TagIdentifier"" LeftMargin=""Whitespace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IBinaryConditionalExpression}"" IsComplex=""True"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IBinaryOperatorExpression}"" IsComplex=""True"">
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
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IClassConstantExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
            <LayoutPlaceholderFrame PropertyName=""ClassIdentifier"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Class""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <LayoutPlaceholderFrame PropertyName=""ConstantIdentifier"" LeftMargin=""ThinSpace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ICloneOfExpression}"" IsComplex=""True"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IEntityExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>entity</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""Query"" LeftMargin=""Whitespace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IEqualityExpression}"" IsComplex=""True"">
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
                <LayoutKeywordFrame>≠</LayoutKeywordFrame>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IIndexQueryExpression}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IInitializedObjectExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ClassIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Type""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <LayoutVerticalBlockListFrame PropertyName=""AssignmentBlocks"" />
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
            <LayoutInsertFrame CollectionName=""AssignmentBlocks"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IKeywordEntityExpression}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IKeywordExpression}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IManifestCharacterExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>'</LayoutKeywordFrame>
            <LayoutCharacterFrame PropertyName=""Text""/>
            <LayoutKeywordFrame>'</LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IManifestNumberExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutNumberFrame PropertyName=""Text""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IManifestStringExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>""</LayoutKeywordFrame>
            <LayoutTextValueFrame PropertyName=""Text""/>
            <LayoutKeywordFrame>""</LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:INewExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">new</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""Object"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IOldExpression}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">old</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""Query"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPrecursorExpression}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPrecursorIndexExpression}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPreprocessorExpression}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IQueryExpression}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IResultOfExpression}"" IsComplex=""True"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IUnaryNotExpression}"" IsComplex=""True"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IUnaryOperatorExpression}"" IsComplex=""True"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""Operator"" RightMargin=""Whitespace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAttributeFeature}"">
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Export""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IConstantFeature}"">
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Export""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ICreationFeature}"">
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IFunctionFeature}"">
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
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""Once""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame LeftMargin=""Whitespace"">once per</LayoutKeywordFrame>
                    <LayoutDiscreteFrame PropertyName=""Once"" LeftMargin=""Whitespace"">
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IIndexerFeature}"">
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutOptionalFrame PropertyName=""GetterBody"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IDeferredBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IEffectiveBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IExternBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IPrecursorBody}"" SelectorName=""Getter""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                </LayoutVerticalPanelFrame>
                <LayoutVerticalPanelFrame>
                    <LayoutOptionalFrame PropertyName=""SetterBody"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IDeferredBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IEffectiveBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IExternBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IPrecursorBody}"" SelectorName=""Setter""/>
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IProcedureFeature}"">
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Export""/>
                        </LayoutPlaceholderFrame.Selectors>
                    </LayoutPlaceholderFrame>
                </LayoutHorizontalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPropertyFeature}"">
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
                <LayoutPlaceholderFrame PropertyName=""EntityType"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
                <LayoutDiscreteFrame PropertyName=""PropertyKind"">
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""PropertyKind"" DefaultValue=""2""/>
                    </LayoutDiscreteFrame.Visibility>
                    <LayoutKeywordFrame>read-only</LayoutKeywordFrame>
                    <LayoutKeywordFrame>write-only</LayoutKeywordFrame>
                    <LayoutKeywordFrame>readwrite</LayoutKeywordFrame>
                </LayoutDiscreteFrame>
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
                                <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                            </LayoutHorizontalBlockListFrame.Selectors>
                        </LayoutHorizontalBlockListFrame>
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
                    <LayoutOptionalFrame PropertyName=""GetterBody"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IDeferredBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IEffectiveBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IExternBody}"" SelectorName=""Getter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IPrecursorBody}"" SelectorName=""Getter""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                    <LayoutOptionalFrame PropertyName=""SetterBody"">
                        <LayoutOptionalFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IDeferredBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IEffectiveBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IExternBody}"" SelectorName=""Setter""/>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IPrecursorBody}"" SelectorName=""Setter""/>
                        </LayoutOptionalFrame.Selectors>
                    </LayoutOptionalFrame>
                <LayoutHorizontalPanelFrame>
                    <LayoutHorizontalPanelFrame.Visibility>
                        <LayoutTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </LayoutHorizontalPanelFrame.Visibility>
                    <LayoutKeywordFrame>export to</LayoutKeywordFrame>
                    <LayoutPlaceholderFrame PropertyName=""ExportIdentifier"" LeftMargin=""Whitespace"">
                        <LayoutPlaceholderFrame.Selectors>
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Export""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IIdentifier}"" IsSimple=""True"">
        <LayoutSelectionFrame>
            <LayoutSelectableFrame Name=""Identifier"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text"" TextStyle=""Default"" AutoFormat=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Feature"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text"" TextStyle=""Default"" AutoFormat=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Class"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text"" TextStyle=""Default"" AutoFormat=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""ClassOrExport"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text"" TextStyle=""Default"" AutoFormat=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Export"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text"" TextStyle=""Default"" AutoFormat=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Library"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text"" TextStyle=""Default"" AutoFormat=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Source"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text"" TextStyle=""Default"" AutoFormat=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Type"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text"" TextStyle=""Type"" AutoFormat=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
            <LayoutSelectableFrame Name=""Pattern"">
                <LayoutVerticalPanelFrame>
                    <LayoutCommentFrame/>
                    <LayoutTextValueFrame PropertyName=""Text"" TextStyle=""Default"" AutoFormat=""True""/>
                </LayoutVerticalPanelFrame>
            </LayoutSelectableFrame>
        </LayoutSelectionFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAsLongAsInstruction}"">
        <LayoutVerticalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame>as long as</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ContinueCondition"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame>execute</LayoutKeywordFrame>
                <LayoutInsertFrame CollectionName=""ContinuationBlocks""/>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalPanelFrame>
                <LayoutVerticalBlockListFrame PropertyName=""ContinuationBlocks""/>
                <LayoutVerticalPanelFrame>
                    <LayoutVerticalPanelFrame.Visibility>
                        <LayoutOptionalFrameVisibility PropertyName=""ElseInstructions""/>
                    </LayoutVerticalPanelFrame.Visibility>
                    <LayoutKeywordFrame>else</LayoutKeywordFrame>
                    <LayoutVerticalPanelFrame HasTabulationMargin=""True"">
                        <LayoutOptionalFrame PropertyName=""ElseInstructions"" />
                    </LayoutVerticalPanelFrame>
                </LayoutVerticalPanelFrame>
            </LayoutVerticalPanelFrame>
            <LayoutKeywordFrame>end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAssignmentInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutHorizontalBlockListFrame PropertyName=""DestinationBlocks"" Separator=""Comma""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <LayoutPlaceholderFrame PropertyName=""Source"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAttachmentInstruction}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ICheckInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">check</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""BooleanExpression"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ICommandInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ICreateInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>create</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""EntityIdentifier"" LeftMargin=""Whitespace"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutKeywordFrame LeftMargin=""Whitespace"">with</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""CreationRoutineIdentifier"" LeftMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IDebugInstruction}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IForLoopInstruction}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IIfThenElseInstruction}"">
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
            <LayoutKeywordFrame IsFocusable=""True"">end</LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IIndexAssignmentInstruction}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IInspectInstruction}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IKeywordAssignmentInstruction}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IOverLoopInstruction}"">
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPrecursorIndexAssignmentInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame IsFocusable=""True"">precursor</LayoutKeywordFrame>
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.Visibility>
                    <LayoutOptionalFrameVisibility PropertyName=""AncestorType""/>
                </LayoutHorizontalPanelFrame.Visibility>
                <LayoutKeywordFrame LeftMargin=""Whitespace"" RightMargin=""ThinSpace"">from</LayoutKeywordFrame>
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}"" RightMargin=""ThinSpace""/>
                <LayoutOptionalFrame PropertyName=""AncestorType"" />
                <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}"" LeftMargin=""ThinSpace""/>
            </LayoutHorizontalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"" LeftMargin=""ThinSpace"" RightMargin=""ThinSpace""/>
            <LayoutHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" Separator=""Comma""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <LayoutPlaceholderFrame PropertyName=""Source"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPrecursorInstruction}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IRaiseEventInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">raise</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""QueryIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IReleaseInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame>release</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""EntityName"" LeftMargin=""Whitespace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IThrowInstruction}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame IsFocusable=""True"" RightMargin=""Whitespace"">throw</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""ExceptionType"" RightMargin=""Whitespace""/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">with</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""CreationRoutine"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAnchoredType}"" IsSimple=""True"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IFunctionType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">function</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""BaseType"" RightMargin=""Whitespace""/>
            <LayoutHorizontalBlockListFrame PropertyName=""OverloadBlocks""/>
            <LayoutInsertFrame CollectionName=""OverloadBlocks"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IGenericType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ClassIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IIndexerType}"">
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
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""IndexerKind"" DefaultValue=""2""/>
                    </LayoutDiscreteFrame.Visibility>
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutKeywordFrame>end</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IKeywordAnchoredType}"" IsSimple=""True"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IProcedureType}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutKeywordFrame RightMargin=""Whitespace"">procedure</LayoutKeywordFrame>
            <LayoutPlaceholderFrame PropertyName=""BaseType"" RightMargin=""Whitespace""/>
            <LayoutHorizontalBlockListFrame PropertyName=""OverloadBlocks""/>
            <LayoutInsertFrame CollectionName=""OverloadBlocks""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPropertyType}"">
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
                    <LayoutDiscreteFrame.Visibility>
                        <LayoutDefaultDiscreteFrameVisibility PropertyName=""PropertyKind"" DefaultValue=""2""/>
                    </LayoutDiscreteFrame.Visibility>
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
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
                            <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                        </LayoutVerticalBlockListFrame.Selectors>
                    </LayoutVerticalBlockListFrame>
                </LayoutVerticalPanelFrame>
                <LayoutKeywordFrame>end</LayoutKeywordFrame>
            </LayoutVerticalPanelFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}"" LeftMargin=""ThinSpace""/>
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ISimpleType}"" IsSimple=""True"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ClassIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Type""/>
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:ITupleType}"">
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
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IAssignmentTypeArgument}"">
        <LayoutHorizontalPanelFrame>
            <LayoutCommentFrame/>
            <LayoutPlaceholderFrame PropertyName=""ParameterIdentifier"">
                <LayoutPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Feature""/>
                </LayoutPlaceholderFrame.Selectors>
            </LayoutPlaceholderFrame>
            <LayoutSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}"" LeftMargin=""Whitespace"" RightMargin=""Whitespace""/>
            <LayoutPlaceholderFrame PropertyName=""Source"" />
        </LayoutHorizontalPanelFrame>
    </LayoutNodeTemplate>
    <LayoutNodeTemplate NodeType=""{xaml:Type easly:IPositionalTypeArgument}"">
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
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IArgument,easly:Argument}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IAssertion,easly:Assertion}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IAssignmentArgument,easly:AssignmentArgument}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IAttachment,easly:Attachment}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IClass,easly:Class}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IClassReplicate,easly:ClassReplicate}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ICommandOverload,easly:CommandOverload}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ICommandOverloadType,easly:CommandOverloadType}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IConditional,easly:Conditional}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IConstraint,easly:Constraint}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IContinuation,easly:Continuation}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IDiscrete,easly:Discrete}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IEntityDeclaration,easly:EntityDeclaration}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IExceptionHandler,easly:ExceptionHandler}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IExport,easly:Export}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IExportChange,easly:ExportChange}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IFeature,easly:Feature}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame Separator=""Line""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IGeneric,easly:Generic}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IIdentifier,easly:Identifier}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma"">
                <LayoutHorizontalCollectionPlaceholderFrame.Selectors>
                    <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                </LayoutHorizontalCollectionPlaceholderFrame.Selectors>
            </LayoutHorizontalCollectionPlaceholderFrame>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IImport,easly:Import}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IInheritance,easly:Inheritance}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IInstruction,easly:Instruction}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ILibrary,easly:Library}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IName,easly:Name}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IObjectType,easly:ObjectType}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IPattern,easly:Pattern}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IQualifiedName,easly:QualifiedName}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IQueryOverload,easly:QueryOverload}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IQueryOverloadType,easly:QueryOverloadType}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IRange,easly:Range}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IRename,easly:Rename}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ITypeArgument,easly:TypeArgument}"">
        <LayoutHorizontalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutHorizontalCollectionPlaceholderFrame Separator=""Comma""/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutHorizontalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:ITypedef,easly:Typedef}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
                <LayoutKeywordFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutKeywordFrame.BlockVisibility>
            </LayoutKeywordFrame>
        </LayoutVerticalPanelFrame>
    </LayoutBlockTemplate>
    <LayoutBlockTemplate NodeType=""{xaml:Type easly:IBlock,easly:IWith,easly:With}"">
        <LayoutVerticalPanelFrame HasBlockGeometry=""True"">
            <LayoutHorizontalPanelFrame>
                <LayoutHorizontalPanelFrame.BlockVisibility>
                    <LayoutReplicationFrameVisibility/>
                </LayoutHorizontalPanelFrame.BlockVisibility>
                <LayoutKeywordFrame RightMargin=""Whitespace"">Replicate</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""ReplicationPattern"" RightMargin=""Whitespace""/>
                <LayoutKeywordFrame RightMargin=""Whitespace"">From</LayoutKeywordFrame>
                <LayoutPlaceholderFrame PropertyName=""SourceIdentifier"" RightMargin=""Whitespace"">
                    <LayoutPlaceholderFrame.Selectors>
                        <LayoutFrameSelector SelectorType=""{xaml:Type easly:IIdentifier}"" SelectorName=""Identifier""/>
                    </LayoutPlaceholderFrame.Selectors>
                </LayoutPlaceholderFrame>
                <LayoutKeywordFrame RightMargin=""Whitespace"">All</LayoutKeywordFrame>
            </LayoutHorizontalPanelFrame>
            <LayoutVerticalCollectionPlaceholderFrame/>
            <LayoutKeywordFrame Text=""end"" LeftMargin=""Whitespace"">
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
