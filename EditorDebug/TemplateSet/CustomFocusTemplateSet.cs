using EaslyController.Focus;
using System.IO;
using System.Text;
using System.Windows.Markup;

namespace TestDebug
{
    public class CustomFocusTemplateSet
    {
        #region Init
#if !TRAVIS
        static CustomFocusTemplateSet()
        {
            IFocusTemplateReadOnlyDictionary FocusCustomNodeTemplates = LoadTemplate(FocusTemplateListString);
            IFocusTemplateReadOnlyDictionary FocusCustomBlockTemplates = LoadTemplate(FocusBlockTemplateString);
            FocusTemplateSet = new FocusTemplateSet(FocusCustomNodeTemplates, FocusCustomBlockTemplates);
        }

        private static IFocusTemplateReadOnlyDictionary LoadTemplate(string s)
        {
            byte[] ByteArray = Encoding.UTF8.GetBytes(s);
            using (MemoryStream ms = new MemoryStream(ByteArray))
            {
                IFocusTemplateList Templates = XamlReader.Parse(s) as IFocusTemplateList;

                FocusTemplateDictionary TemplateDictionary = new FocusTemplateDictionary();
                foreach (IFocusTemplate Item in Templates)
                {
                    Item.Root.UpdateParent(Item, FocusFrame.FocusRoot);
                    TemplateDictionary.Add(Item.NodeType, Item);
                }

                IFocusTemplateReadOnlyDictionary Result = new FocusTemplateReadOnlyDictionary(TemplateDictionary);
                return Result;
            }
        }

        private CustomFocusTemplateSet()
        {
        }
#endif
        #endregion

        #region Properties
        public static IFocusTemplateSet FocusTemplateSet { get; private set; }
        #endregion

        #region Node Templates
        static string FocusTemplateListString =
@"<FocusTemplateList
    xmlns=""clr-namespace:EaslyController.Focus;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FocusNodeTemplate NodeType=""{xaml:Type IAssertion}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusOptionalFrame PropertyName=""Tag"" />
                <FocusKeywordFrame>:</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""BooleanExpression"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAttachment}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame Text=""else"">
                    <FocusKeywordFrame.Visibility>
                        <FocusNotFirstItemFrameVisibility/>
                    </FocusKeywordFrame.Visibility>
                </FocusKeywordFrame>
                <FocusKeywordFrame>as</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""AttachTypeBlocks""/>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IClass}"">
        <FocusVerticalPanelFrame>
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
                <FocusPlaceholderFrame PropertyName=""EntityName""/>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>from</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""FromIdentifier"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
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
                    <FocusInsertFrame CollectionName=""FeatureBlocks"" />
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IClassReplicate}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ReplicateName"" />
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalBlockListFrame PropertyName=""PatternBlocks""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICommandOverload}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
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
            <FocusPlaceholderFrame PropertyName=""CommandBody"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IDeferredBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type IEffectiveBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type IExternBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type IPrecursorBody}"" SelectorName=""Overload""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICommandOverloadType}"">
        <FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
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
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""RequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IConditional}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame Text=""else"">
                    <FocusKeywordFrame.Visibility>
                        <FocusNotFirstItemFrameVisibility/>
                    </FocusKeywordFrame.Visibility>
                </FocusKeywordFrame>
                <FocusKeywordFrame>if</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""BooleanExpression""/>
                <FocusKeywordFrame>then</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IConstraint}"">
        <FocusVerticalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ParentType"" />
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>rename</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""RenameBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""RenameBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IContinuation}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>execute</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusPlaceholderFrame PropertyName=""Instructions"" />
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>cleanup</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""CleanupBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""CleanupBlocks"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IDiscrete}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""NumericValue"" />
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IEntityDeclaration}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusKeywordFrame>:</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""EntityType"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>=</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""DefaultValue"" />
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IExceptionHandler}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>catch</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ExceptionIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IExport}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalBlockListFrame PropertyName=""ClassIdentifierBlocks"">
                <FocusHorizontalBlockListFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""ClassOrExport""/>
                </FocusHorizontalBlockListFrame.Selectors>
            </FocusHorizontalBlockListFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IExportChange}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Export""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalBlockListFrame PropertyName=""IdentifierBlocks"">
                <FocusHorizontalBlockListFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusHorizontalBlockListFrame.Selectors>
            </FocusHorizontalBlockListFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IGeneric}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>=</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""DefaultValue"" />
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>conform to</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ConstraintBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ConstraintBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IGlobalReplicate}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ReplicateName""/>
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusHorizontalListFrame PropertyName=""Patterns""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IImport}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Type"">
                    <FocusKeywordFrame>latest</FocusKeywordFrame>
                    <FocusKeywordFrame>strict</FocusKeywordFrame>
                    <FocusKeywordFrame>stable</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""LibraryIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Library""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>from</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""FromIdentifier"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Source""/>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IInheritance}"">
        <FocusVerticalPanelFrame>
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
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
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
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
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
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
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
    <FocusNodeTemplate NodeType=""{xaml:Type ILibrary}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>library</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName""/>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>from</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""FromIdentifier"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>import</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ImportBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ImportBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>class</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ClassIdentifierBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ClassIdentifierBlocks"">
                    <FocusVerticalBlockListFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                    </FocusVerticalBlockListFrame.Selectors>
                </FocusVerticalBlockListFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IName}"" IsSimple=""True"">
        <FocusTextValueFrame PropertyName=""Text""/>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPattern}"" IsSimple=""True"">
        <FocusTextValueFrame PropertyName=""Text""/>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IQualifiedName}"">
        <FocusHorizontalListFrame PropertyName=""Path"">
            <FocusHorizontalListFrame.Selectors>
                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
            </FocusHorizontalListFrame.Selectors>
        </FocusHorizontalListFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IQueryOverload}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
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
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>result</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ResultBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ResultBlocks""/>
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>modified</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ModifiedQueryBlocks"">
                    <FocusVerticalBlockListFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                    </FocusVerticalBlockListFrame.Selectors>
                </FocusVerticalBlockListFrame>
            </FocusVerticalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""QueryBody"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IDeferredBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type IEffectiveBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type IExternBody}"" SelectorName=""Overload""/>
                    <FocusFrameSelector SelectorType=""{xaml:Type IPrecursorBody}"" SelectorName=""Overload""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>variant</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""Variant"" />
            </FocusHorizontalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IQueryOverloadType}"">
        <FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
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
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>return</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ResultBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ResultBlocks""/>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""RequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IRange}"">
        <FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}"">
            </FocusSymbolFrame>
            <FocusPlaceholderFrame PropertyName=""LeftExpression"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>to</FocusKeywordFrame>
                <FocusOptionalFrame PropertyName=""RightExpression"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IRename}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""SourceIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusKeywordFrame>to</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""DestinationIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IRoot}"">
        <FocusVerticalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IScope}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>local</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
            </FocusVerticalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>do</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""InstructionBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""InstructionBlocks"" />
            </FocusVerticalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ITypedef}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName"" />
            <FocusKeywordFrame>is</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""DefinedType"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAssignmentArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalBlockListFrame PropertyName=""ParameterBlocks"">
                <FocusHorizontalBlockListFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusHorizontalBlockListFrame.Selectors>
            </FocusHorizontalBlockListFrame>
            <FocusPlaceholderFrame PropertyName=""Source""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IWith}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>case</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""RangeBlocks""/>
                <FocusInsertFrame CollectionName=""RangeBlocks""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions""/>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPositionalArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Source""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IDeferredBody}"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Overload"">
                <FocusVerticalPanelFrame>
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
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>deferred</FocusKeywordFrame>
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
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>getter</FocusKeywordFrame>
                            <FocusKeywordFrame>deferred</FocusKeywordFrame>
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
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>setter</FocusKeywordFrame>
                            <FocusKeywordFrame>deferred</FocusKeywordFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IEffectiveBody}"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Overload"">
                <FocusVerticalPanelFrame>
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
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
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
                            <FocusKeywordFrame>do</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""BodyInstructionBlocks"" />
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
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
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
                            <FocusKeywordFrame>getter</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""BodyInstructionBlocks"" />
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
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
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
                            <FocusKeywordFrame>setter</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""BodyInstructionBlocks"" />
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
    <FocusNodeTemplate NodeType=""{xaml:Type IExternBody}"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Overload"">
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>extern</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
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
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>getter</FocusKeywordFrame>
                            <FocusKeywordFrame>extern</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
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
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>setter</FocusKeywordFrame>
                            <FocusKeywordFrame>extern</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorBody}"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Overload"">
                <FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>precursor</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
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
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>getter</FocusKeywordFrame>
                            <FocusKeywordFrame>precursor</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
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
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>require</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""RequireBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusVerticalBlockListFrame PropertyName=""RequireBlocks"" />
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>throw</FocusKeywordFrame>
                            <FocusInsertFrame CollectionName=""ExceptionIdentifierBlocks"" />
                        </FocusHorizontalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ExceptionIdentifierBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalPanelFrame>
                            <FocusKeywordFrame>setter</FocusKeywordFrame>
                            <FocusKeywordFrame>precursor</FocusKeywordFrame>
                        </FocusHorizontalPanelFrame>
                    </FocusVerticalPanelFrame>
                    <FocusVerticalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IAgentExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>agent</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""BaseType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Delegated"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAssertionTagExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>tag</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""TagIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IBinaryConditionalExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IBinaryOperatorExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
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
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IClassConstantExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Class""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.Dot}""/>
            <FocusPlaceholderFrame PropertyName=""ConstantIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICloneOfExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusDiscreteFrame PropertyName=""Type"">
                <FocusKeywordFrame>shallow</FocusKeywordFrame>
                <FocusKeywordFrame>deep</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusKeywordFrame>clone of</FocusKeywordFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IEntityExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>entity</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""Query""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IEqualityExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IIndexQueryExpression}"">
        <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IInitializedObjectExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Type""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalBlockListFrame PropertyName=""AssignmentBlocks"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            <FocusInsertFrame CollectionName=""AssignmentBlocks"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IKeywordExpression}"">
        <FocusDiscreteFrame PropertyName=""Value"">
            <FocusKeywordFrame>True</FocusKeywordFrame>
            <FocusKeywordFrame>False</FocusKeywordFrame>
            <FocusKeywordFrame>Current</FocusKeywordFrame>
            <FocusKeywordFrame>Value</FocusKeywordFrame>
            <FocusKeywordFrame>Result</FocusKeywordFrame>
            <FocusKeywordFrame>Retry</FocusKeywordFrame>
            <FocusKeywordFrame>Exception</FocusKeywordFrame>
        </FocusDiscreteFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IManifestCharacterExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>'</FocusKeywordFrame>
            <FocusCharacterFrame PropertyName=""Text""/>
            <FocusKeywordFrame>'</FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IManifestNumberExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusNumberFrame PropertyName=""Text""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IManifestStringExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>""</FocusKeywordFrame>
            <FocusTextValueFrame PropertyName=""Text""/>
            <FocusKeywordFrame>""</FocusKeywordFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type INewExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>new</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""Object"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IOldExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>old</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""Query"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}"">
                </FocusSymbolFrame>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}"">
                </FocusSymbolFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorIndexExpression}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IPreprocessorExpression}"">
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
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IQueryExpression}"">
        <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IResultOfExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IUnaryNotExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IUnaryOperatorExpression}"" IsComplex=""True"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Operator"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IAttributeFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>:</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
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
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IConstantFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
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
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICreationFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
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
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IFunctionFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>once per</FocusKeywordFrame>
                    <FocusDiscreteFrame PropertyName=""Once"">
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
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIndexerFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusKeywordFrame>indexer</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
                            <FocusKeywordFrame>closed</FocusKeywordFrame>
                            <FocusKeywordFrame>open</FocusKeywordFrame>
                        </FocusDiscreteFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""IndexParameterBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""IndexParameterBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>modify</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusOptionalFrame PropertyName=""GetterBody"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IDeferredBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IEffectiveBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IExternBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IPrecursorBody}"" SelectorName=""Getter""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusOptionalFrame PropertyName=""SetterBody"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IDeferredBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IEffectiveBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IExternBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IPrecursorBody}"" SelectorName=""Setter""/>
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
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IProcedureFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
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
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPropertyFeature}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusDiscreteFrame PropertyName=""Export"">
                    <FocusKeywordFrame>exported</FocusKeywordFrame>
                    <FocusKeywordFrame>private</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusPlaceholderFrame PropertyName=""EntityName"" />
                <FocusKeywordFrame>is</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""EntityType""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>modify</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ModifiedQueryBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalPanelFrame>
                        <FocusHorizontalBlockListFrame PropertyName=""ModifiedQueryBlocks"">
                            <FocusHorizontalBlockListFrame.Selectors>
                                <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                            </FocusHorizontalBlockListFrame.Selectors>
                        </FocusHorizontalBlockListFrame>
                    </FocusVerticalPanelFrame>
                </FocusVerticalPanelFrame>
                    <FocusOptionalFrame PropertyName=""GetterBody"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IDeferredBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IEffectiveBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IExternBody}"" SelectorName=""Getter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IPrecursorBody}"" SelectorName=""Getter""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                    <FocusOptionalFrame PropertyName=""SetterBody"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IDeferredBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IEffectiveBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IExternBody}"" SelectorName=""Setter""/>
                            <FocusFrameSelector SelectorType=""{xaml:Type IPrecursorBody}"" SelectorName=""Setter""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                <FocusHorizontalPanelFrame>
                    <FocusHorizontalPanelFrame.Visibility>
                        <FocusTextMatchFrameVisibility PropertyName=""ExportIdentifier"" TextPattern=""All""/>
                    </FocusHorizontalPanelFrame.Visibility>
                    <FocusKeywordFrame>export to</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""ExportIdentifier"">
                        <FocusPlaceholderFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Export""/>
                        </FocusPlaceholderFrame.Selectors>
                    </FocusPlaceholderFrame>
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame Text=""end"">
            </FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIdentifier}"" IsSimple=""True"">
        <FocusSelectionFrame>
            <FocusSelectableFrame Name=""Identifier"">
                <FocusTextValueFrame PropertyName=""Text""/>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Feature"">
                <FocusTextValueFrame PropertyName=""Text""/>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Class"">
                <FocusTextValueFrame PropertyName=""Text""/>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""ClassOrExport"">
                <FocusTextValueFrame PropertyName=""Text""/>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Export"">
                <FocusTextValueFrame PropertyName=""Text""/>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Library"">
                <FocusTextValueFrame PropertyName=""Text""/>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Source"">
                <FocusTextValueFrame PropertyName=""Text""/>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Type"">
                <FocusTextValueFrame PropertyName=""Text""/>
            </FocusSelectableFrame>
            <FocusSelectableFrame Name=""Pattern"">
                <FocusTextValueFrame PropertyName=""Text""/>
            </FocusSelectableFrame>
        </FocusSelectionFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAsLongAsInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>as long as</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""ContinueCondition""/>
                <FocusInsertFrame CollectionName=""ContinuationBlocks""/>
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""ContinuationBlocks""/>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>else</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusOptionalFrame PropertyName=""ElseInstructions"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusHorizontalBlockListFrame PropertyName=""DestinationBlocks""/>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAttachmentInstruction}"">
        <FocusVerticalPanelFrame>
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
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>else</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusOptionalFrame PropertyName=""ElseInstructions"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICheckInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>check</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""BooleanExpression"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ICommandInstruction}"">
        <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type ICreateInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>create</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""EntityIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>with</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""CreationRoutineIdentifier"">
                    <FocusPlaceholderFrame.Selectors>
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusHorizontalPanelFrame>
                    <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                    <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                    <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
                </FocusHorizontalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>same processor as</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""Processor"" />
                </FocusHorizontalPanelFrame>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IDebugInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>debug</FocusKeywordFrame>
                <FocusInsertFrame CollectionName=""Instructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Instructions"" />
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IForLoopInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusKeywordFrame>loop</FocusKeywordFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>local</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""EntityDeclarationBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""EntityDeclarationBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>init</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""InitInstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""InitInstructionBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>while</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""WhileCondition""/>
                    <FocusInsertFrame CollectionName=""LoopInstructionBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""LoopInstructionBlocks"" />
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>iterate</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""IterationInstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""IterationInstructionBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>invariant</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""InvariantBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""InvariantBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>variant</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""Variant"" />
                </FocusHorizontalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIfThenElseInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusVerticalBlockListFrame PropertyName=""ConditionalBlocks""/>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>else</FocusKeywordFrame>
                    <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                </FocusHorizontalPanelFrame>
                <FocusOptionalFrame PropertyName=""ElseInstructions"" />
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IIndexAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""Destination"" />
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IInspectInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>inspect</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""Source"" />
                <FocusInsertFrame CollectionName=""WithBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusVerticalBlockListFrame PropertyName=""WithBlocks""/>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>else</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""ElseInstructions.InstructionBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusOptionalFrame PropertyName=""ElseInstructions"" />
                </FocusVerticalPanelFrame>
            </FocusVerticalPanelFrame>
            <FocusKeywordFrame>end</FocusKeywordFrame>
        </FocusVerticalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IKeywordAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusDiscreteFrame PropertyName=""Destination"">
                <FocusKeywordFrame>True</FocusKeywordFrame>
                <FocusKeywordFrame>False</FocusKeywordFrame>
                <FocusKeywordFrame>Current</FocusKeywordFrame>
                <FocusKeywordFrame>Value</FocusKeywordFrame>
                <FocusKeywordFrame>Result</FocusKeywordFrame>
                <FocusKeywordFrame>Retry</FocusKeywordFrame>
                <FocusKeywordFrame>Exception</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IOverLoopInstruction}"">
        <FocusVerticalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>over</FocusKeywordFrame>
                <FocusPlaceholderFrame PropertyName=""OverList"" />
                <FocusKeywordFrame>for each</FocusKeywordFrame>
                <FocusHorizontalBlockListFrame PropertyName=""IndexerBlocks""/>
                <FocusDiscreteFrame PropertyName=""Iteration"">
                    <FocusKeywordFrame>Single</FocusKeywordFrame>
                    <FocusKeywordFrame>Nested</FocusKeywordFrame>
                </FocusDiscreteFrame>
                <FocusInsertFrame CollectionName=""LoopInstructions.InstructionBlocks"" />
            </FocusHorizontalPanelFrame>
            <FocusVerticalPanelFrame>
                <FocusPlaceholderFrame PropertyName=""LoopInstructions"" />
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>exit if</FocusKeywordFrame>
                    <FocusOptionalFrame PropertyName=""ExitEntityName"">
                        <FocusOptionalFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                        </FocusOptionalFrame.Selectors>
                    </FocusOptionalFrame>
                </FocusHorizontalPanelFrame>
                <FocusVerticalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorIndexAssignmentInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IPrecursorInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>precursor</FocusKeywordFrame>
            <FocusHorizontalPanelFrame>
                <FocusKeywordFrame>from</FocusKeywordFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftCurlyBracket}""/>
                <FocusOptionalFrame PropertyName=""AncestorType"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightCurlyBracket}""/>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IRaiseEventInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>raise</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""QueryIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusDiscreteFrame PropertyName=""Event"">
                <FocusKeywordFrame>once</FocusKeywordFrame>
                <FocusKeywordFrame>forever</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IReleaseInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>release</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""EntityName""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IThrowInstruction}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>throw</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""ExceptionType"" />
            <FocusKeywordFrame>with</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""CreationRoutine"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusHorizontalPanelFrame>
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftParenthesis}""/>
                <FocusHorizontalBlockListFrame PropertyName=""ArgumentBlocks"" />
                <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightParenthesis}""/>
            </FocusHorizontalPanelFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IAnchoredType}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>like</FocusKeywordFrame>
            <FocusDiscreteFrame PropertyName=""AnchorKind"">
                <FocusKeywordFrame>declaration</FocusKeywordFrame>
                <FocusKeywordFrame>creation</FocusKeywordFrame>
            </FocusDiscreteFrame>
            <FocusPlaceholderFrame PropertyName=""AnchoredName"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IFunctionType}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>function</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusHorizontalBlockListFrame PropertyName=""OverloadBlocks""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IGenericType}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IIndexerType}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftBracket}""/>
            <FocusVerticalPanelFrame>
                <FocusHorizontalPanelFrame>
                    <FocusKeywordFrame>indexer</FocusKeywordFrame>
                    <FocusPlaceholderFrame PropertyName=""EntityType""/>
                </FocusHorizontalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>parameter</FocusKeywordFrame>
                        <FocusDiscreteFrame PropertyName=""ParameterEnd"">
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
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetRequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetRequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetEnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetEnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IKeywordAnchoredType}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>like</FocusKeywordFrame>
            <FocusDiscreteFrame PropertyName=""Anchor"">
                <FocusKeywordFrame>True</FocusKeywordFrame>
                <FocusKeywordFrame>False</FocusKeywordFrame>
                <FocusKeywordFrame>Current</FocusKeywordFrame>
                <FocusKeywordFrame>Value</FocusKeywordFrame>
                <FocusKeywordFrame>Result</FocusKeywordFrame>
                <FocusKeywordFrame>Retry</FocusKeywordFrame>
                <FocusKeywordFrame>Exception</FocusKeywordFrame>
            </FocusDiscreteFrame>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IProcedureType}"">
        <FocusHorizontalPanelFrame>
            <FocusKeywordFrame>procedure</FocusKeywordFrame>
            <FocusPlaceholderFrame PropertyName=""BaseType"" />
            <FocusHorizontalBlockListFrame PropertyName=""OverloadBlocks""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPropertyType}"">
        <FocusHorizontalPanelFrame>
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
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter ensure</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetEnsureBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetEnsureBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>getter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""GetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""GetExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter require</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetRequireBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetRequireBlocks"" />
                </FocusVerticalPanelFrame>
                <FocusVerticalPanelFrame>
                    <FocusHorizontalPanelFrame>
                        <FocusKeywordFrame>setter exception</FocusKeywordFrame>
                        <FocusInsertFrame CollectionName=""SetExceptionIdentifierBlocks"" />
                    </FocusHorizontalPanelFrame>
                    <FocusVerticalBlockListFrame PropertyName=""SetExceptionIdentifierBlocks"">
                        <FocusVerticalBlockListFrame.Selectors>
                            <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                        </FocusVerticalBlockListFrame.Selectors>
                    </FocusVerticalBlockListFrame>
                </FocusVerticalPanelFrame>
                <FocusKeywordFrame>end</FocusKeywordFrame>
            </FocusVerticalPanelFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.RightBracket}""/>
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type ISimpleType}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ClassIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Type""/>
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
    <FocusNodeTemplate NodeType=""{xaml:Type ITupleType}"">
        <FocusHorizontalPanelFrame>
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
    <FocusNodeTemplate NodeType=""{xaml:Type IAssignmentTypeArgument}"">
        <FocusHorizontalPanelFrame>
            <FocusPlaceholderFrame PropertyName=""ParameterIdentifier"">
                <FocusPlaceholderFrame.Selectors>
                    <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Feature""/>
                </FocusPlaceholderFrame.Selectors>
            </FocusPlaceholderFrame>
            <FocusSymbolFrame Symbol=""{x:Static const:Symbols.LeftArrow}""/>
            <FocusPlaceholderFrame PropertyName=""Source"" />
        </FocusHorizontalPanelFrame>
    </FocusNodeTemplate>
    <FocusNodeTemplate NodeType=""{xaml:Type IPositionalTypeArgument}"">
        <FocusPlaceholderFrame PropertyName=""Source"" />
    </FocusNodeTemplate>
</FocusTemplateList>";
        #endregion

        #region Block Templates
        static string FocusBlockTemplateString =
@"<FocusTemplateList
    xmlns=""clr-namespace:EaslyController.Focus;assembly=Easly-Controller""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:xaml=""clr-namespace:EaslyController.Xaml;assembly=Easly-Controller""
    xmlns:const=""clr-namespace:EaslyController.Constants;assembly=Easly-Controller"">
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IArgument,Argument}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IAssertion,Assertion}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IAssignmentArgument,AssignmentArgument}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IAttachment,Attachment}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IClass,Class}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IClassReplicate,ClassReplicate}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ICommandOverload,CommandOverload}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ICommandOverloadType,CommandOverloadType}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IConditional,Conditional}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IConstraint,Constraint}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IContinuation,Continuation}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IDiscrete,Discrete}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IEntityDeclaration,EntityDeclaration}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IExceptionHandler,ExceptionHandler}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IExport,Export}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IExportChange,ExportChange}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IFeature,Feature}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IGeneric,Generic}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IIdentifier,Identifier}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IImport,Import}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IInheritance,Inheritance}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IInstruction,Instruction}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ILibrary,Library}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IName,Name}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IObjectType,ObjectType}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IPattern,Pattern}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IQualifiedName,QualifiedName}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IQueryOverload,QueryOverload}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IQueryOverloadType,QueryOverloadType}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IRange,Range}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IRename,Rename}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ITypeArgument,TypeArgument}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusHorizontalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusHorizontalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,ITypedef,Typedef}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
    <FocusBlockTemplate NodeType=""{xaml:Type IBlock,IWith,With}"">
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
                        <FocusFrameSelector SelectorType=""{xaml:Type IIdentifier}"" SelectorName=""Identifier""/>
                    </FocusPlaceholderFrame.Selectors>
                </FocusPlaceholderFrame>
                <FocusKeywordFrame>All</FocusKeywordFrame>
            </FocusHorizontalPanelFrame>
            <FocusVerticalCollectionPlaceholderFrame/>
            <FocusKeywordFrame Text=""end""/>
        </FocusVerticalPanelFrame>
    </FocusBlockTemplate>
</FocusTemplateList>
";
        #endregion
    }
}
