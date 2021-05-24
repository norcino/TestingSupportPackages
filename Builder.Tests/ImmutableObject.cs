namespace Builder.Tests
{
    internal class ImmutableObject
    {
        public readonly string StringValue;
        public readonly int IntValue;
        public readonly SutPoco ObjectValue;

        public ImmutableObject(string stringValue, int intValue, SutPoco objectValue)
        {
            StringValue = stringValue;
            IntValue = intValue;
            ObjectValue = objectValue;
        }
    }
}
