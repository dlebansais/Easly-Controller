using EaslyController.Layout;

namespace Coverage
{
    public class CoverageLayoutFrame : LayoutFrame
    {
        public CoverageLayoutFrame()
        {
            ILayoutTemplate ParentTemplate = this.ParentTemplate;
            ILayoutFrame ParentFrame = this.ParentFrame;
        }
    }
}
