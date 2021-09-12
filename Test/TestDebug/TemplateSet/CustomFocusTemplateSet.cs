using EaslyController.Focus;
using System.IO;
using System.Text;
using System.Windows.Markup;

namespace TestDebug
{
    public class CustomFocusTemplateSet
    {
        #region Init
        static CustomFocusTemplateSet()
        {
            NodeTemplateDictionary = LoadTemplate(FocusTemplateListString);
            FocusTemplateReadOnlyDictionary FocusCustomNodeTemplates = (FocusTemplateReadOnlyDictionary)NodeTemplateDictionary.ToReadOnly();
            BlockTemplateDictionary = LoadTemplate(FocusBlockTemplateString);
            FocusTemplateReadOnlyDictionary FocusCustomBlockTemplates = (FocusTemplateReadOnlyDictionary)BlockTemplateDictionary.ToReadOnly();
            FocusTemplateSet = new FocusTemplateSet(FocusCustomNodeTemplates, FocusCustomBlockTemplates);
        }

        private static FocusTemplateDictionary LoadTemplate(string s)
        {
            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                FocusTemplateList Templates = (FocusTemplateList)XamlReader.Parse(s);

                FocusTemplateDictionary TemplateDictionary = new FocusTemplateDictionary();
                foreach (IFocusTemplate Item in Templates)
                {
                    Item.Root.UpdateParent(Item, FocusFrame.FocusRoot);
                    TemplateDictionary.Add(Item.NodeType, Item);
                }

                return TemplateDictionary;
            }
        }

        private CustomFocusTemplateSet()
        {
        }
        #endregion

        #region Properties
        public static FocusTemplateDictionary NodeTemplateDictionary { get; private set; }
        public static FocusTemplateDictionary BlockTemplateDictionary { get; private set; }
        public static IFocusTemplateSet FocusTemplateSet { get; private set; }
        #endregion

        #region Node Templates
        static string FocusTemplateListString =
@"<FocusTemplateList
    xmlns=""clr-namespace:EaslyController.Focus;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:easly=""clr-namespace:BaseNode;assembly=Easly-Language""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Assertion}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusOptionalFrame PropertyName=""Tag"" />
                <FocusKeywordFrame>:</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""BooleanExpression"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Attachment}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame Text=""else"">
                    <FocusKeywordFrame.Visibility>
                        <FocusNotFirstItemFrameVisibility/>
                    </FocusKeywordFrame.Visibility>
                </FocusKeywordFrame>
                <FocusKeywordFrame>as</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""AttachTypeBlocks""/>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Class}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""CopySpecification"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""CopySpecification"" DefaultValue=""1""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>any</FocusKeywordFrame>
                    <FocusKeywordFrame>reference</FocusKeywordFrame>
                    <FocusKeywordFrame>value</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""Cloneable"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Cloneable""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>cloneable</FocusKeywordFrame>
                    <FocusKeywordFrame>single</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""Comparable"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Comparable""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>comparable</FocusKeywordFrame>
                    <FocusKeywordFrame>incomparable</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""IsAbstract"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""IsAbstract""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>instanceable</FocusKeywordFrame>
                    <FocusKeywordFrame>abstract</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusKeywordFrame>class</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" IsPreferred=""True""/>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusOptionalFrameVisibility PropertyName=""FromIdentifier""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>from</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""FromIdentifier"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ImportBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>import</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ImportBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ImportBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""GenericBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>generic</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""GenericBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""GenericBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ExportBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>export</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ExportBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ExportBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""TypedefBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>typedef</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""TypedefBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""TypedefBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""InheritanceBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>inheritance</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""InheritanceBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""InheritanceBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""DiscreteBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>discrete</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""DiscreteBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""DiscreteBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ClassReplicateBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>replicate</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ClassReplicateBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ClassReplicateBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""FeatureBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>feature</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""FeatureBlocks"" ItemType=""{xaml:Type easly:AttributeFeature}""/>
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""FeatureBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ConversionBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>conversion</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ConversionBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ConversionBlocks"">
                    <FocusVerticalBlockListFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                    </FocusVerticalBlockListFrame.Selectors>
                </FocusVerticalBlockListFrame>
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""InvariantBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>invariant</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""InvariantBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.Visibility>
                    <FocusMixedFrameVisibility>
                        <FocusCountFrameVisibility PropertyName=""ImportBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""GenericBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""ExportBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""TypedefBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""InheritanceBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""DiscreteBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""ClassReplicateBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""FeatureBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""ConversionBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""InvariantBlocks""/>
                    </FocusMixedFrameVisibility>
                </FocusKeywordFrame.Visibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ClassReplicate}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""ReplicateName"" />
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalBlockListFrame PropertyName=""PatternBlocks""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:CommandOverload}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ParameterBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>parameter</FocusKeywordFrame>
                    <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                        <FocusDiscreteFrame.Visibility>
                            <FocusDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                        </FocusDiscreteFrame.Visibility>
                        <FocusKeywordFrame>closed</FocusKeywordFrame>
                        <FocusKeywordFrame>open</FocusKeywordFrame>
                    </FocusDiscreteFrame>
                    <FocusInsertFrame CollectionName=""ParameterBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""CommandBody"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Overload""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:CommandOverloadType}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                            <FocusDiscreteFrame.Visibility>
                                <FocusDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                            </FocusDiscreteFrame.Visibility>
                            <FocusKeywordFrame>closed</FocusKeywordFrame>
                            <FocusKeywordFrame>open</FocusKeywordFrame>
                        </FocusDiscreteFrame>
                        <FocusInsertFrame CollectionName=""ParameterBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""RequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Conditional}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame Text=""else"">
                    <FocusKeywordFrame.Visibility>
                        <FocusNotFirstItemFrameVisibility/>
                    </FocusKeywordFrame.Visibility>
                </FocusKeywordFrame>
                <FocusKeywordFrame>if</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""BooleanExpression""/>
                <FocusKeywordFrame>then</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Constraint}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""ParentType"" />
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""RenameBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>rename</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""RenameBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""RenameBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Continuation}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>execute</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusPlaceholderFrame PropertyName=""Instructions"" />
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""CleanupBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>cleanup</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""CleanupBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""CleanupBlocks"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Discrete}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""NumericValue""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""NumericValue"" />
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:EntityDeclaration}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusKeywordFrame>:</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""EntityType"" />
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""DefaultValue""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""DefaultValue"" />
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ExceptionHandler}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>catch</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ExceptionIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Export}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalBlockListFrame PropertyName=""ClassIdentifierBlocks"">
                <FocusHorizontalBlockListFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""ClassOrExport""/>
                </FocusHorizontalBlockListFrame.Selectors>
            </FocusHorizontalBlockListFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ExportChange}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalBlockListFrame PropertyName=""IdentifierBlocks"">
                <FocusHorizontalBlockListFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusHorizontalBlockListFrame.Selectors>
            </FocusHorizontalBlockListFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Generic}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusOptionalFrameVisibility PropertyName=""DefaultValue""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>=</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""DefaultValue"" />
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ConstraintBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>conform to</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ConstraintBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ConstraintBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.Visibility>
                    <FocusMixedFrameVisibility>
                        <FocusCountFrameVisibility PropertyName=""ConstraintBlocks""/>
                    </FocusMixedFrameVisibility>
                </FocusKeywordFrame.Visibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:GlobalReplicate}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""ReplicateName""/>
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalListFrame PropertyName=""Patterns""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Import}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Type"">
                    <FocusKeywordFrame>latest</FocusKeywordFrame>
                    <FocusKeywordFrame>strict</FocusKeywordFrame>
                    <FocusKeywordFrame>stable</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""LibraryIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Library""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusOptionalFrameVisibility PropertyName=""FromIdentifier""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>from</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""FromIdentifier"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Source""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""RenameBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>rename</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""RenameBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""RenameBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusMixedFrameVisibility>
                        <FocusCountFrameVisibility PropertyName=""RenameBlocks""/>
                    </FocusMixedFrameVisibility>
                </FocusVerticalPanelFrame.Visibility>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Inheritance}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Conformance"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Conformance""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>conformant</FocusKeywordFrame>
                    <FocusKeywordFrame>non-conformant</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""ParentType"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""RenameBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>rename</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""RenameBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""RenameBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ForgetBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>forget</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ForgetBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ForgetBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""KeepBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>keep</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""KeepBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""KeepBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""DiscontinueBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>discontinue</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""DiscontinueBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""DiscontinueBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ExportChangeBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>export</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ExportChangeBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ExportChangeBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusDiscreteFrame PropertyName=""ForgetIndexer"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""ForgetIndexer""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>ignore indexer</FocusKeywordFrame>
                    <FocusKeywordFrame>forget indexer</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""KeepIndexer"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""KeepIndexer""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>ignore indexer</FocusKeywordFrame>
                    <FocusKeywordFrame>keep indexer</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusDiscreteFrame PropertyName=""DiscontinueIndexer"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""DiscontinueIndexer""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>ignore indexer</FocusKeywordFrame>
                    <FocusKeywordFrame>discontinue indexer</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusKeywordFrame Text=""end"">
                    <FocusKeywordFrame.Visibility>
                        <FocusMixedFrameVisibility>
                            <FocusCountFrameVisibility PropertyName=""RenameBlocks""/>
                            <FocusCountFrameVisibility PropertyName=""ForgetBlocks""/>
                            <FocusCountFrameVisibility PropertyName=""KeepBlocks""/>
                            <FocusCountFrameVisibility PropertyName=""DiscontinueBlocks""/>
                            <FocusDefaultDiscreteFrameVisibility PropertyName=""ForgetIndexer""/>
                            <FocusDefaultDiscreteFrameVisibility PropertyName=""KeepIndexer""/>
                            <FocusDefaultDiscreteFrameVisibility PropertyName=""DiscontinueIndexer""/>
                        </FocusMixedFrameVisibility>
                    </FocusKeywordFrame.Visibility>
                </FocusKeywordFrame>
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Library}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>library</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName""/>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>from</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""FromIdentifier"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ImportBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>import</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ImportBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ImportBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ClassIdentifierBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>class</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ClassIdentifierBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ClassIdentifierBlocks"">
                    <FocusVerticalBlockListFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                    </FocusVerticalBlockListFrame.Selectors>
                </FocusVerticalBlockListFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.Visibility>
                    <FocusMixedFrameVisibility>
                        <FocusCountFrameVisibility PropertyName=""ImportBlocks""/>
                        <FocusCountFrameVisibility PropertyName=""ClassIdentifierBlocks""/>
                    </FocusMixedFrameVisibility>
                </FocusKeywordFrame.Visibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Name}"" IsSimple=""True"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Pattern}"" IsSimple=""True"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusTextValueFrame PropertyName=""Text""/>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:QualifiedName}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalListFrame PropertyName=""Path"">
                <FocusHorizontalListFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusHorizontalListFrame.Selectors>
            </FocusHorizontalListFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:QueryOverload}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ParameterBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>parameter</FocusKeywordFrame>
                    <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                        <FocusKeywordFrame>closed</FocusKeywordFrame>
                        <FocusKeywordFrame>open</FocusKeywordFrame>
                    </FocusDiscreteFrame>
                    <FocusInsertFrame CollectionName=""ParameterBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ResultBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>result</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ResultBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ResultBlocks""/>
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ModifiedQueryBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>modified</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ModifiedQueryBlocks"">
                    <FocusVerticalBlockListFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                    </FocusVerticalBlockListFrame.Selectors>
                </FocusVerticalBlockListFrame>
            </FocusVerticalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""QueryBody"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Overload""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>variant</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""Variant"" />
            </FocusHorizontalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:QueryOverloadType}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                            <FocusDiscreteFrame.Visibility>
                                <FocusDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                            </FocusDiscreteFrame.Visibility>
                            <FocusKeywordFrame>closed</FocusKeywordFrame>
                            <FocusKeywordFrame>open</FocusKeywordFrame>
                        </FocusDiscreteFrame>
                        <FocusInsertFrame CollectionName=""ParameterBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ParameterBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>return</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ResultBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ResultBlocks""/>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""RequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Range}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"">
                <FocusSymbolFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""RightExpression""/>
                </FocusSymbolFrame.Visibility>
            </FocusSymbolFrame>
            <FocusPlaceholderFrame PropertyName=""LeftExpression"" />
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""RightExpression""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusKeywordFrame>to</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Rename}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""DestinationIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Root}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>libraries</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""LibraryBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""LibraryBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>classes</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ClassBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ClassBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>replicates</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""Replicates"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalListFrame PropertyName=""Replicates"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Scope}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>local</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>do</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""InstructionBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Typedef}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusKeywordFrame>is</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""DefinedType"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:With}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>case</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""RangeBlocks""/>
                <FocusInsertFrame CollectionName=""RangeBlocks""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions""/>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:AssignmentArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalBlockListFrame PropertyName=""ParameterBlocks"">
                <FocusHorizontalBlockListFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusHorizontalBlockListFrame.Selectors>
            </FocusHorizontalBlockListFrame>
            <FocusPlaceholderFrame PropertyName=""Source""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PositionalArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""Source""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:DeferredBody}"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Overload"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame IsFocusable=""true"" IsPreferred=""true"">deferred</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Getter"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>getter</FocusKeywordFrame>
                            <FocusKeywordFrame IsFocusable=""true"" IsPreferred=""true"">deferred</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Setter"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>setter</FocusKeywordFrame>
                            <FocusKeywordFrame IsFocusable=""true"" IsPreferred=""true"">deferred</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
        </FocusSelectionFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:EffectiveBody}"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Overload"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>local</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""BodyInstructionBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame IsFocusable=""true"">do</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""BodyInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""BodyInstructionBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionHandlerBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>exception</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionHandlerBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""ExceptionHandlerBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Getter"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>local</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""BodyInstructionBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame IsFocusable=""true"">getter</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""BodyInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""BodyInstructionBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionHandlerBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>exception</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionHandlerBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""ExceptionHandlerBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Setter"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>local</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""BodyInstructionBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame IsFocusable=""true"">setter</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""BodyInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""BodyInstructionBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionHandlerBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>exception</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionHandlerBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""ExceptionHandlerBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
        </FocusSelectionFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ExternBody}"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Overload"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame IsFocusable=""true"">extern</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Getter"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>getter</FocusKeywordFrame>
                            <FocusKeywordFrame IsFocusable=""true"">extern</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Setter"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>setter</FocusKeywordFrame>
                            <FocusKeywordFrame IsFocusable=""true"">extern</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
        </FocusSelectionFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PrecursorBody}"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Overload"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame IsFocusable=""true"">precursor</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Getter"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>getter</FocusKeywordFrame>
                            <FocusKeywordFrame IsFocusable=""true"">precursor</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Setter"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""RequireBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""ExceptionIdentifierBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>setter</FocusKeywordFrame>
                            <FocusKeywordFrame IsFocusable=""true"">precursor</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusVerticalPanelFrame.Visibility>
                            <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        </FocusVerticalPanelFrame.Visibility>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>ensure</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
        </FocusSelectionFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:AgentExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>agent</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""BaseType""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""BaseType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Delegated"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:AssertionTagExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>tag</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""TagIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:BinaryConditionalExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""LeftExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
            <FocusDiscreteFrame PropertyName=""Conditional"">
                <FocusKeywordFrame>and</FocusKeywordFrame>
                <FocusKeywordFrame>or</FocusKeywordFrame>
                <FocusKeywordFrame>xor</FocusKeywordFrame>
                <FocusKeywordFrame>⇒</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:BinaryOperatorExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""LeftExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Operator"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ClassConstantExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Class""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <FocusPlaceholderFrame PropertyName=""ConstantIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:CloneOfExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusDiscreteFrame PropertyName=""Type"">
                <FocusDiscreteFrame.Visibility>
                    <FocusDefaultDiscreteFrameVisibility PropertyName=""Type""/>
                </FocusDiscreteFrame.Visibility>
                <FocusKeywordFrame>shallow</FocusKeywordFrame>
                <FocusKeywordFrame>deep</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusKeywordFrame IsFocusable=""true"">clone of</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""Source""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""Source"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""Source""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:EntityExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>entity</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""Query""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:EqualityExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""LeftExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""LeftExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
            <FocusDiscreteFrame PropertyName=""Comparison"">
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusKeywordFrame>!=</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusDiscreteFrame PropertyName=""Equality"">
                <FocusDiscreteFrame.Visibility>
                    <FocusDefaultDiscreteFrameVisibility PropertyName=""Equality""/>
                </FocusDiscreteFrame.Visibility>
                <FocusKeywordFrame>phys</FocusKeywordFrame>
                <FocusKeywordFrame>deep</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusKeywordFrame Text="" ""/>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:IndexQueryExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""IndexedExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""IndexedExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""IndexedExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:InitializedObjectExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Type""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalBlockListFrame PropertyName=""AssignmentBlocks"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FocusInsertFrame CollectionName=""AssignmentBlocks"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:KeywordEntityExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>entity</FocusKeywordFrame>
            <FocusDiscreteFrame PropertyName=""Value"">
                <FocusKeywordFrame>True</FocusKeywordFrame>
                <FocusKeywordFrame>False</FocusKeywordFrame>
                <FocusKeywordFrame>Current</FocusKeywordFrame>
                <FocusKeywordFrame>Value</FocusKeywordFrame>
                <FocusKeywordFrame>Result</FocusKeywordFrame>
                <FocusKeywordFrame>Retry</FocusKeywordFrame>
                <FocusKeywordFrame>Exception</FocusKeywordFrame>
                <FocusKeywordFrame>Indexer</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:KeywordExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusDiscreteFrame PropertyName=""Value"">
                <FocusKeywordFrame>True</FocusKeywordFrame>
                <FocusKeywordFrame>False</FocusKeywordFrame>
                <FocusKeywordFrame>Current</FocusKeywordFrame>
                <FocusKeywordFrame>Value</FocusKeywordFrame>
                <FocusKeywordFrame>Result</FocusKeywordFrame>
                <FocusKeywordFrame>Retry</FocusKeywordFrame>
                <FocusKeywordFrame>Exception</FocusKeywordFrame>
                <FocusKeywordFrame>Indexer</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ManifestCharacterExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>'</FocusKeywordFrame>
            <FocusCharacterFrame PropertyName=""Text""/>
            <FocusKeywordFrame>'</FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ManifestNumberExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusNumberFrame PropertyName=""Text""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ManifestStringExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>""</FocusKeywordFrame>
            <FocusTextValueFrame PropertyName=""Text""/>
            <FocusKeywordFrame>""</FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:NewExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>new</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""Object"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:OldExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>old</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""Query"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PrecursorExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame IsFocusable=""true"">precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""AncestorType""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PrecursorIndexExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame IsFocusable=""true"">precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""AncestorType""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks""/>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PreprocessorExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusDiscreteFrame PropertyName=""Value"">
                <FocusKeywordFrame>DateAndTime</FocusKeywordFrame>
                <FocusKeywordFrame>CompilationDiscreteIdentifier</FocusKeywordFrame>
                <FocusKeywordFrame>ClassPath</FocusKeywordFrame>
                <FocusKeywordFrame>CompilerVersion</FocusKeywordFrame>
                <FocusKeywordFrame>ConformanceToStandard</FocusKeywordFrame>
                <FocusKeywordFrame>DiscreteClassIdentifier</FocusKeywordFrame>
                <FocusKeywordFrame>Counter</FocusKeywordFrame>
                <FocusKeywordFrame>Debugging</FocusKeywordFrame>
                <FocusKeywordFrame>RandomInteger</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:QueryExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""Query"" />
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ResultOfExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>result of</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""Source""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""Source"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""Source""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:UnaryNotExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>not</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:UnaryOperatorExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""Operator"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusPlaceholderFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusComplexFrameVisibility PropertyName=""RightExpression""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:AttributeFeature}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>:</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                        <FocusPlaceholderFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.Visibility>
                    <FocusMixedFrameVisibility>
                        <FocusCountFrameVisibility PropertyName=""EnsureBlocks""/>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusMixedFrameVisibility>
                </FocusKeywordFrame.Visibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ConstantFeature}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>:</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ConstantValue""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                        <FocusPlaceholderFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.Visibility>
                    <FocusMixedFrameVisibility>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusMixedFrameVisibility>
                </FocusKeywordFrame.Visibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:CreationFeature}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>creation</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""OverloadBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""OverloadBlocks""/>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                        <FocusPlaceholderFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:FunctionFeature}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>once per</FocusKeywordFrame>
                    <FocusDiscreteFrame PropertyName=""Once"">
                        <FocusDiscreteFrame.Visibility>
                            <FocusDefaultDiscreteFrameVisibility PropertyName=""Once""/>
                        </FocusDiscreteFrame.Visibility>
                        <FocusKeywordFrame>normal</FocusKeywordFrame>
                        <FocusKeywordFrame>object</FocusKeywordFrame>
                        <FocusKeywordFrame>processor</FocusKeywordFrame>
                        <FocusKeywordFrame>process</FocusKeywordFrame>
                    </FocusDiscreteFrame>
                </FocusHorizontalPanelFrame>
                <FocusKeywordFrame>function</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""OverloadBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""OverloadBlocks""/>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                        <FocusPlaceholderFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:IndexerFeature}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusKeywordFrame IsFocusable=""true"">indexer</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""IndexParameterBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                            <FocusDiscreteFrame.Visibility>
                                <FocusDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                            </FocusDiscreteFrame.Visibility>
                            <FocusKeywordFrame>closed</FocusKeywordFrame>
                            <FocusKeywordFrame>open</FocusKeywordFrame>
                        </FocusDiscreteFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ModifiedQueryBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>modify</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusOptionalFrame PropertyName=""GetterBody"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Getter""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusOptionalFrame PropertyName=""SetterBody"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Setter""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                        <FocusPlaceholderFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ProcedureFeature}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>procedure</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""OverloadBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""OverloadBlocks""/>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                        <FocusPlaceholderFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PropertyFeature}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Export""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>is</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ModifiedQueryBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>modify</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
                    <FocusOptionalFrame PropertyName=""GetterBody"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Getter""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                    <FocusOptionalFrame PropertyName=""SetterBody"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:DeferredBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:EffectiveBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:ExternBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:PrecursorBody}"" SelectorName=""Setter""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                        <FocusPlaceholderFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.Visibility>
                    <FocusMixedFrameVisibility>
                        <FocusCountFrameVisibility PropertyName=""ModifiedQueryBlocks""/>
                        <FocusOptionalFrameVisibility PropertyName=""GetterBody""/>
                        <FocusOptionalFrameVisibility PropertyName=""SetterBody""/>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusMixedFrameVisibility>
                </FocusKeywordFrame.Visibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:Identifier}"" IsSimple=""True"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Identifier"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Feature"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Class"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""ClassOrExport"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Export"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Library"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Source"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Type"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Pattern"">
                <FocusVerticalPanelFrame>
                    <FocusCommentFrame/>
                    <FocusTextValueFrame PropertyName=""Text"" AutoFormat=""True""/>
                </FocusVerticalPanelFrame>
            </FocusSelectableFrame>
        </FocusSelectionFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:AsLongAsInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>as long as</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ContinueCondition""/>
                <FocusInsertFrame CollectionName=""ContinuationBlocks""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ContinuationBlocks""/>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusOptionalFrameVisibility PropertyName=""ElseInstructions""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>else</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </FocusHorizontalPanelFrame>
                    <FocusOptionalFrame PropertyName=""ElseInstructions"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:AssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalBlockListFrame PropertyName=""DestinationBlocks""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:AttachmentInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>attach</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""Source"" />
                <FocusKeywordFrame>to</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""EntityNameBlocks""/>
                <FocusInsertFrame CollectionName=""AttachmentBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""AttachmentBlocks""/>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusOptionalFrameVisibility PropertyName=""ElseInstructions""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>else</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </FocusHorizontalPanelFrame>
                    <FocusOptionalFrame PropertyName=""ElseInstructions"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:CheckInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>check</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""BooleanExpression"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:CommandInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""Command"" />
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                    <FocusSymbolFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                    </FocusSymbolFrame.Visibility>
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:CreateInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>create</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""EntityIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>with</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""CreationRoutineIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusHorizontalPanelFrame>
                    <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                    <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                    <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
                </FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusOptionalFrameVisibility PropertyName=""Processor""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>same processor as</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""Processor"" />
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:DebugInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>debug</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ForLoopInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>loop</FocusKeywordFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""EntityDeclarationBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>local</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""InitInstructionBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>init</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""InitInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""InitInstructionBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>while</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""WhileCondition""/>
                    <FocusInsertFrame CollectionName=""LoopInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""LoopInstructionBlocks"" />
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""IterationInstructionBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>iterate</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""IterationInstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""IterationInstructionBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""InvariantBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>invariant</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""InvariantBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusOptionalFrameVisibility PropertyName=""Variant""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>variant</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""Variant"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:IfThenElseInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusVerticalBlockListFrame PropertyName=""ConditionalBlocks""/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""ElseInstructions""/>
                </FocusVerticalPanelFrame.Visibility>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>else</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                </FocusHorizontalPanelFrame>
                <FocusOptionalFrame PropertyName=""ElseInstructions"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:IndexAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""Destination"" />
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:InspectInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>inspect</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""Source"" />
                <FocusInsertFrame CollectionName=""WithBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""WithBlocks""/>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusOptionalFrameVisibility PropertyName=""ElseInstructions""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>else</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
                    </FocusHorizontalPanelFrame>
                    <FocusOptionalFrame PropertyName=""ElseInstructions"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:KeywordAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusDiscreteFrame PropertyName=""Destination"">
                <FocusKeywordFrame>True</FocusKeywordFrame>
                <FocusKeywordFrame>False</FocusKeywordFrame>
                <FocusKeywordFrame>Current</FocusKeywordFrame>
                <FocusKeywordFrame>Value</FocusKeywordFrame>
                <FocusKeywordFrame>Result</FocusKeywordFrame>
                <FocusKeywordFrame>Retry</FocusKeywordFrame>
                <FocusKeywordFrame>Exception</FocusKeywordFrame>
                <FocusKeywordFrame>Indexer</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:OverLoopInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusCommentFrame/>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>over</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""OverList"" />
                <FocusKeywordFrame>for each</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""IndexerBlocks""/>
                <FocusDiscreteFrame PropertyName=""Iteration"">
                    <FocusDiscreteFrame.Visibility>
                        <FocusDefaultDiscreteFrameVisibility PropertyName=""Iteration""/>
                    </FocusDiscreteFrame.Visibility>
                    <FocusKeywordFrame>Single</FocusKeywordFrame>
                    <FocusKeywordFrame>Nested</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusInsertFrame CollectionName=""LoopInstructions.InstructionBlocks"" ItemType=""{xaml:Type easly:CommandInstruction}""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusPlaceholderFrame PropertyName=""LoopInstructions"" />
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusOptionalFrameVisibility PropertyName=""ExitEntityName""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>exit if</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""ExitEntityName"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                </FocusHorizontalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""InvariantBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>invariant</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""InvariantBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PrecursorIndexAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""AncestorType""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusKeywordFrame>from</FocusKeywordFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
            </FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PrecursorInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame IsFocusable=""true"">precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusOptionalFrameVisibility PropertyName=""AncestorType""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusKeywordFrame>from</FocusKeywordFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:RaiseEventInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>raise</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""QueryIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusDiscreteFrame PropertyName=""Event"">
                <FocusDiscreteFrame.Visibility>
                    <FocusDefaultDiscreteFrameVisibility PropertyName=""Event""/>
                </FocusDiscreteFrame.Visibility>
                <FocusKeywordFrame>once</FocusKeywordFrame>
                <FocusKeywordFrame>forever</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ReleaseInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>release</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ThrowInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>throw</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""ExceptionType"" />
            <FocusKeywordFrame>with</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""CreationRoutine"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.Visibility>
                    <FocusCountFrameVisibility PropertyName=""ArgumentBlocks""/>
                </FocusHorizontalPanelFrame.Visibility>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:AnchoredType}"" IsSimple=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>like</FocusKeywordFrame>
            <FocusDiscreteFrame PropertyName=""AnchorKind"">
                <FocusDiscreteFrame.Visibility>
                    <FocusDefaultDiscreteFrameVisibility PropertyName=""AnchorKind""/>
                </FocusDiscreteFrame.Visibility>
                <FocusKeywordFrame>declaration</FocusKeywordFrame>
                <FocusKeywordFrame>creation</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusPlaceholderFrame PropertyName=""AnchoredName"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:FunctionType}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>function</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusHorizontalBlockListFrame PropertyName=""OverloadBlocks""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:GenericType}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusHorizontalBlockListFrame PropertyName=""TypeArgumentBlocks""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FocusDiscreteFrame PropertyName=""Sharing"">
                <FocusDiscreteFrame.Visibility>
                    <FocusDefaultDiscreteFrameVisibility PropertyName=""Sharing""/>
                </FocusDiscreteFrame.Visibility>
                <FocusKeywordFrame>not shared</FocusKeywordFrame>
                <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                <FocusKeywordFrame>read-only</FocusKeywordFrame>
                <FocusKeywordFrame>write-only</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:IndexerType}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>indexer</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""EntityType""/>
                </FocusHorizontalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""IndexParameterBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                            <FocusDiscreteFrame.Visibility>
                                <FocusDefaultDiscreteFrameVisibility PropertyName=""ParameterEnd""/>
                            </FocusDiscreteFrame.Visibility>
                            <FocusKeywordFrame>closed</FocusKeywordFrame>
                            <FocusKeywordFrame>open</FocusKeywordFrame>
                        </FocusDiscreteFrame>
                        <FocusInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""IndexParameterBlocks""/>
                </FocusVerticalPanelFrame>
                <FocusDiscreteFrame PropertyName=""IndexerKind"">
                    <FocusKeywordFrame>read-only</FocusKeywordFrame>
                    <FocusKeywordFrame>write-only</FocusKeywordFrame>
                    <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""GetRequireBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetRequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetRequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""GetEnsureBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""GetExceptionIdentifierBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""SetRequireBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""SetEnsureBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetEnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetEnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""SetExceptionIdentifierBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:KeywordAnchoredType}"" IsSimple=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>like</FocusKeywordFrame>
            <FocusDiscreteFrame PropertyName=""Anchor"">
                <FocusKeywordFrame>True</FocusKeywordFrame>
                <FocusKeywordFrame>False</FocusKeywordFrame>
                <FocusKeywordFrame>Current</FocusKeywordFrame>
                <FocusKeywordFrame>Value</FocusKeywordFrame>
                <FocusKeywordFrame>Result</FocusKeywordFrame>
                <FocusKeywordFrame>Retry</FocusKeywordFrame>
                <FocusKeywordFrame>Exception</FocusKeywordFrame>
                <FocusKeywordFrame>Indexer</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:ProcedureType}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>procedure</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusHorizontalBlockListFrame PropertyName=""OverloadBlocks""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PropertyType}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>is</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""EntityType""/>
                </FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""PropertyKind"">
                    <FocusKeywordFrame>read-only</FocusKeywordFrame>
                    <FocusKeywordFrame>write-only</FocusKeywordFrame>
                    <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""GetEnsureBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""GetExceptionIdentifierBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""SetRequireBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame.Visibility>
                        <FocusCountFrameVisibility PropertyName=""SetExceptionIdentifierBlocks""/>
                    </FocusVerticalPanelFrame.Visibility>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:SimpleType}"" IsSimple=""True"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Type""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusDiscreteFrame PropertyName=""Sharing"">
                <FocusDiscreteFrame.Visibility>
                    <FocusDefaultDiscreteFrameVisibility PropertyName=""Sharing""/>
                </FocusDiscreteFrame.Visibility>
                <FocusKeywordFrame>not shared</FocusKeywordFrame>
                <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                <FocusKeywordFrame>read-only</FocusKeywordFrame>
                <FocusKeywordFrame>write-only</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:TupleType}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusKeywordFrame>tuple</FocusKeywordFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FocusDiscreteFrame PropertyName=""Sharing"">
                <FocusDiscreteFrame.Visibility>
                    <FocusDefaultDiscreteFrameVisibility PropertyName=""Sharing""/>
                </FocusDiscreteFrame.Visibility>
                <FocusKeywordFrame>not shared</FocusKeywordFrame>
                <FocusKeywordFrame>readwrite</FocusKeywordFrame>
                <FocusKeywordFrame>read-only</FocusKeywordFrame>
                <FocusKeywordFrame>write-only</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:AssignmentTypeArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""ParameterIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type easly:PositionalTypeArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusCommentFrame/>
            <FocusPlaceholderFrame PropertyName=""Source""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
</FocusTemplateList>";
        #endregion

        #region Block Templates
        static string FocusBlockTemplateString =
@"<FocusTemplateList
    xmlns=""clr-namespace:EaslyController.Focus;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:easly=""clr-namespace:BaseNode;assembly=Easly-Language""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Argument}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Assertion}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:AssignmentArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Attachment}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Class}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:ClassReplicate}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:CommandOverload}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:CommandOverloadType}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Conditional}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Constraint}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Continuation}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Discrete}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:EntityDeclaration}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:ExceptionHandler}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Export}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:ExportChange}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Feature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Generic}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Identifier}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame>
                <FocusHorizontalCollectionPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                </FocusHorizontalCollectionPlaceholderFrame.Selectors>
            </FocusHorizontalCollectionPlaceholderFrame>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Import}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Inheritance}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Instruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Library}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Name}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:ObjectType}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Pattern}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:QualifiedName}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:QueryOverload}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:QueryOverloadType}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Range}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Rename}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:TypeArgument
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:Typedef}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type easly:Block,easly:With}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusHorizontalPanelFrame.BlockVisibility>
                <FocusKeywordFrame>Replicate</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ReplicationPattern""/>
                <FocusKeywordFrame>From</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type easly:Identifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end"">
                <FocusKeywordFrame.BlockVisibility>
                    <FocusReplicationFrameVisibility/>
                </FocusKeywordFrame.BlockVisibility>
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
</FocusTemplateList>
";
        #endregion
    }
}
