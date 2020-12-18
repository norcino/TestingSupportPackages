namespace FluentAssertion.MSTest.Framework
{
    public class AssertEachItem<T>
    {
        public readonly AssertCollection<T> RootCollection;

        public AssertEachItem(AssertCollection<T> rootCollection)
        {
            RootCollection = rootCollection;
        }
    }
}
