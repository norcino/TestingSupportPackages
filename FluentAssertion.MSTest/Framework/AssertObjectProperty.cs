namespace FluentAssertion.MSTest.Framework
{
    public class AssertObjectProperty<T,P>
    {
        public readonly AssertObject<T> AssertObject;
        public readonly P Property;

        public AssertObjectProperty(AssertObject<T> assertObject, P property)
        {
            AssertObject = assertObject;
            Property = property;
        }
    }
}
