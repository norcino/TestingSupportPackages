using System;
using System.Collections.Generic;

namespace Builder
{
    /// <summary>
    /// Descript a class which maps types to exclusions which will not be generated on Build or BuildMany
    /// </summary>
    public interface IBuilderExlusionMapping
    {
        IEnumerable<string> GetExclusionsFor(Operation operation, Type builtType);
    }
}
