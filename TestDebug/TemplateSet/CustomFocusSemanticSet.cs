using BaseNode;
using BaseNodeHelper;
using EaslyController.Focus;
using System;
using System.Collections.Generic;

namespace TestDebug
{
    public class CustomFocusSemanticSet
    {
        #region Init
        static CustomFocusSemanticSet()
        {
            IFocusNodeSemanticDictionary<Type> FocusCustomNodeSemantics = new FocusNodeSemanticDictionary<Type>(NodeHelper.CreateNodeDictionary<IFocusNodeSemantic>());

            FocusCustomNodeSemantics[typeof(IAttachment)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IAttachment.AttachTypeBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IClassReplicate)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IClassReplicate.PatternBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IExport)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IExport.ClassIdentifierBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IGlobalReplicate)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IGlobalReplicate.Patterns), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IQualifiedName)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IQualifiedName.Path), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IQueryOverload)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IQueryOverload.ResultBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IQueryOverloadType)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IQueryOverloadType.ResultBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IAssignmentArgument)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IAssignmentArgument.ParameterBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IWith)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IWith.RangeBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IIndexQueryExpression)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IIndexQueryExpression.ArgumentBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IPrecursorIndexExpression)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IPrecursorIndexExpression.ArgumentBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(ICreationFeature)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(ICreationFeature.OverloadBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IFunctionFeature)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IFunctionFeature.OverloadBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IProcedureFeature)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IProcedureFeature.OverloadBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IAsLongAsInstruction)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IAsLongAsInstruction.ContinuationBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IAssignmentInstruction)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IAssignmentInstruction.DestinationBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IAttachmentInstruction)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IAttachmentInstruction.EntityNameBlocks), IsNeverEmpty = true }, new FocusPropertySemantic() { PropertyName = nameof(IAttachmentInstruction.AttachmentBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IIfThenElseInstruction)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IIfThenElseInstruction.ConditionalBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IInspectInstruction)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IInspectInstruction.WithBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IOverLoopInstruction)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IOverLoopInstruction.IndexerBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IPrecursorIndexAssignmentInstruction)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IPrecursorIndexAssignmentInstruction.ArgumentBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IFunctionType)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IFunctionType.OverloadBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IGenericType)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IGenericType.TypeArgumentBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IIndexerType)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IIndexerType.IndexParameterBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(IProcedureType)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(IProcedureType.OverloadBlocks), IsNeverEmpty = true } });
            FocusCustomNodeSemantics[typeof(ITupleType)] = new FocusNodeSemantic(new FocusPropertySemanticList() { new FocusPropertySemantic() { PropertyName = nameof(ITupleType.EntityDeclarationBlocks), IsNeverEmpty = true } });

            List<Type> KeyList = new List<Type>(FocusCustomNodeSemantics.Keys);
            foreach(Type Key in KeyList)
                if (FocusCustomNodeSemantics[Key] == null)
                    FocusCustomNodeSemantics[Key] = new FocusNodeSemantic();

            FocusSemanticSet = new FocusSemanticSet(FocusCustomNodeSemantics);
        }
        #endregion

        #region Properties
        public static IFocusSemanticSet FocusSemanticSet { get; private set; }
        #endregion
    }
}
