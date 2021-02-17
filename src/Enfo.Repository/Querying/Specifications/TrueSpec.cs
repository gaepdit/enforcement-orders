namespace Enfo.Repository.Querying
{
    public class TrueSpec<T> : Specification<T>
    {
        public TrueSpec()
        {
            ApplyCriteria(e => true);
        }
    }
}
