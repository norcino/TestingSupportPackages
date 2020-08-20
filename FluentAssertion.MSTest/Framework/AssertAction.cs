using System;

namespace FluentAssertion.MSTest.Framework
{
    /// <summary>
    /// Represent an action being subject of an assertion
    /// </summary>
    public class AssertAction
    {
        public AssertAction And => this;
        private readonly Action _rootAction;

        /// <summary>
        /// Creates an AssertAction which get the action under test
        /// </summary>
        /// <example>
        /// This constructor is used when usin the following code
        /// </example>
        /// <code>
        /// Assert.That.Invoking(() => sut.Increment(i))
        /// </code>
        /// <param name="_rootAction">Action under test</param>
        public AssertAction(Action _rootAction)
        {
            this._rootAction = _rootAction;
        }

        /// <summary>
        /// Invoke the action for testing
        /// </summary>
        public void Invoke()
        {
            var x = _rootAction.Target;
            var y = _rootAction.Method;
            var k = _rootAction.DynamicInvoke();
            var m = _rootAction.GetInvocationList();

            _rootAction.Invoke();
        }
    }
}