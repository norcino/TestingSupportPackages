using Builder;
using System;
using System.Collections.Generic;

namespace BuilderExclusionMappings
{
    /// <summary>
    /// Example of mapping
    /// </summary>
    public class MyCustomExclusions : IBuilderExlusionMapping
    {
        private static Dictionary<Operation, Dictionary<Type, List<string>>> Mappings;

        public MyCustomExclusions ()
        {
            Mappings = new Dictionary<Operation, Dictionary<Type, List<string>>>
            {
                {
                    Operation.Default, new Dictionary<Type, List<string>> {
                        { typeof(Type1), new List<string> { nameof(Type1.ID), nameof(Type1.ModifiedDateTimeUTC), nameof(Type1.CreatedDateTimeUTC) } }
                    }
                },
                {
                    Operation.Persistence, new Dictionary<Type, List<string>> {
                        { typeof(Type1), new List<string> { nameof(Type1.ID) } },
                        { typeof(Type2), new List<string> { nameof(Type2.ID) } }
                    }
                },
                {
                    Operation.Create, new Dictionary<Type, List<string>> {
                        { typeof(Type1), new List<string> { nameof(Type1.ID), nameof(Type1.ModifiedDateTimeUTC) } },
                        { typeof(Type2), new List<string> { nameof(Type2.ID), nameof(Type2.ChildObject) } }
                    }
                },
                {
                    Operation.Update, new Dictionary<Type, List<string>> {
                        { typeof(Type1), new List<string> { nameof(Type1.ID), nameof(Type1.CreatedDateTimeUTC) } },
                        { typeof(Type2), new List<string> { nameof(Type2.ID), nameof(Type2.ChildObject) } }
                    }
                }
            };
        }

        public IEnumerable<string> GetExclusionsFor(Operation operation, Type builtType)
        {
            if (!Mappings.ContainsKey(operation))
                return new List<string>();

            if(!Mappings[operation].ContainsKey(builtType))            
                return new List<string>();
            
            return Mappings[operation][builtType];
        }
    }
}
