namespace task18
{
    public interface IScheduler
    {
        bool HasCommand();
        ILongCommand Select();
        void Add(ILongCommand cmd);
    }
}