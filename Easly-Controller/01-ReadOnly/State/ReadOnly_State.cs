namespace EaslyController.ReadOnly
{
    public interface IReadOnlyState
    {
        IReadOnlyInner PropertyToInner(string propertyName);
    }

    public abstract class ReadOnlyState : IReadOnlyState
    {
        public abstract IReadOnlyInner PropertyToInner(string propertyName);
    }
}
