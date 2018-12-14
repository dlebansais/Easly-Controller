namespace EaslyController.ReadOnly
{
    public interface IReadOnlyState
    {
        IReadOnlyInner<IReadOnlyBrowsingChildIndex> PropertyToInner(string propertyName);
    }
}
