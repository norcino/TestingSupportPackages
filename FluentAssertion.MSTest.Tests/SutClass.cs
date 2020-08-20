using System;
using System.Threading;

namespace FluentAssertion.MSTest.Tests
{
    internal class SutClass
    {
        public SutPoco Add(SutPoco poco1, SutPoco poco2)
        {
            var result = new SutPoco
            {
                BoolField = poco1.BoolField && poco2.BoolField,
                IntField = poco1.IntField + poco2.IntField,
                LongField = poco1.LongField + poco2.LongField,
                StringField = poco1.StringField + poco2.StringField
            };
            return result;
        }

        public int Increment(int input, int sleepFor = 0, bool throwException = false)
        {
            if(throwException) throw new Exception("Requested to throw Exception");
            Thread.Sleep(sleepFor);
            return ++input;
        }
    }
}
