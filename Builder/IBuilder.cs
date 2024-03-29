﻿using System;
using System.Collections.Generic;

namespace Builder
{
    /// <summary>
    /// Generic builder used to create custom testing model set.
    /// Each entity is generated creating random values for reference types, strings and dates.
    /// </summary>
    /// <typeparam name="TE">Type of the entity to be created</typeparam>
    public interface IBuilder<TE> where TE : class
    {
        /// <summary>
        /// Generates a default valid entity, by default does not create child entities, but this can be changed specifying the depth level.
        /// Depth one will generate child entities, if any. Depth two will also generate grandchild entities, and so on.
        /// </summary>
        /// <param name="hierarchyDepth">Depth lever to be reached when generating child entity objects</param>
        /// <param name="useRandomValues">False will leave members with their default value as not initialized</param>
        /// <returns>Entity generated</returns>
        TE Build(int hierarchyDepth = 0, bool useRandomValues = true);

        /// <summary>
        /// Generates the entity with default values and customise it applying the provided action.
        /// To create the entity without generating random values but only using the customization action, it is possible
        /// to pass the boolean flag.
        /// </summary>
        /// <param name="hierarchyDepth">Depth lever to be reached when generating child entity objects</param>
        /// <param name="useRandomValues">False will leave members with their default value as not initialized</param>
        /// <param name="entitySetupAction">Action used to customise the created entity</param>
        /// <returns>The created entity</returns>
        TE Build(Action<TE> entitySetupAction, int hierarchyDepth = 0, bool useRandomValues = true);

        /// <summary>
        /// Generates a specific amount of entities as per the number specified.
        /// Uses the standard build method to generate each instance and uses the optional action
        /// to customize the entity based on the creation index.
        /// To create the entities without generating random values but only using the customization action, it is possible
        /// to pass the boolean flag.
        /// </summary>
        /// <param name="numberOfEntities">Number of entities to be created</param>
        /// <param name="useRandomValues">False will leave members with their default value as not initialized</param>
        /// <param name="entitySetupAction">Override action based on the index of the created element</param>
        /// <returns>List of the entities created</returns>
        List<TE> BuildMany(int numberOfEntities, Action<TE, int> entitySetupAction = null, bool useRandomValues = false);
    }
}
