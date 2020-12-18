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
                BoolProperty = poco1.BoolProperty && poco2.BoolProperty,
                IntProperty = poco1.IntProperty + poco2.IntProperty,
                LongProperty = poco1.LongProperty + poco2.LongProperty,
                StringProperty = poco1.StringProperty + poco2.StringProperty
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
