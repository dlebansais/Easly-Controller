using EaslyController.Layout;

namespace EditorDebug
{
    public class ReplacementEntry
    {
        public ReplacementEntry(ILayoutInsertionChildNodeIndex index)
        {
            Index = index;

            LayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(index.Node);
            Controller = LayoutController.Create(RootIndex);
        }

        public ILayoutInsertionChildNodeIndex Index { get; set; }
        public ILayoutController Controller { get; set; }
    }
}
