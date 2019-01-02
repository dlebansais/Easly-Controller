namespace EaslyController.Frame
{
    public interface IFrameTemplate
    {
        string NodeName { get; set; } // Set in Xaml
        IFrameFrame Root { get; set; } // Set in Xaml
        bool IsValid { get; }
    }

    public class FrameTemplate : IFrameTemplate
    {
        #region Properties
        public string NodeName { get; set; } // Set in Xaml
        public IFrameFrame Root { get; set; } // Set in Xaml

        public virtual bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(NodeName))
                    return false;

                if (Root == null)
                    return false;

                return true;
            }
        }
        #endregion

        #region Ancestor Interface
        public override string ToString()
        {
            return "Template {" + NodeName + "}";
        }
        #endregion
    }
}
