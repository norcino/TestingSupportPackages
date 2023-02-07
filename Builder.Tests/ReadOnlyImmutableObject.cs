namespace Builder.Tests
{
    internal class ReadOnlyImmutableObject
    {
        public readonly string StringValue;
        public readonly int IntValue;
        public readonly SutPoco ObjectValue;

        public ReadOnlyImmutableObject(string stringValue)
        {
            StringValue = stringValue;
        }

        public ReadOnlyImmutableObject(string stringValue, int intValue)
        {
            StringValue = stringValue;
            IntValue = intValue;
        }

        public ReadOnlyImmutableObject(string stringValue, int intValue, SutPoco objectValue)
        {
            StringValue = stringValue;
            IntValue = intValue;
            ObjectValue = objectValue;
        }
    }
}
