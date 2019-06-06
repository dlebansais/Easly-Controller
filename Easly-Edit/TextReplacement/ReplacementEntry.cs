using EaslyController.Layout;

namespace EaslyEdit
{
    internal class ReplacementEntry
    {
        public ReplacementEntry(ILayoutInner inner, ILayoutInsertionChildNodeIndex insertionIndex)
        {
            Inner = inner;
            InsertionIndex = insertionIndex;

            LayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(insertionIndex.Node);
            Controller = LayoutController.Create(RootIndex);
        }

        public ILayoutInner Inner { get; }
        public ILayoutInsertionChildNodeIndex InsertionIndex { get; }
        public ILayoutController Controller { get; }
    }
}
