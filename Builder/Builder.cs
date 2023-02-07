using AnonymousData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Builder
{
    public abstract class Builder
    {
        public static int NumberOfNestedEntitiesInCollections { get; set; } = 5;
        public static bool InitializeNullCollectionsInsteadOfEmpty { get; set; } = false;
        public static bool SetPropertiesPrivateSetters { get; set; } = false;
    }

    /// <inheritdoc cref="IBuilder{TE}"/>
    public class Builder<TE> : Builder, IBuilder<TE> where TE : class
    {
        public static IBuilderExlusionMapping BuilderExclusionMapping;
        public CharSet DefaultStringCharSet = CharSet.Alphanumeric;
        internal Operation CurrentOperation = Operation.Default;
        internal IEnumerable<string> Exclusions;
        internal TE CopyFrom = null;

        public static Builder<TE> New()
        {
            try
            {
                // Try to load custom configurations
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var assembly = Assembly.LoadFrom(Path.Combine(path, "BuilderConfiguration.dll"));

                if (BuilderExclusionMapping == null)
                {
                    var exclusionMappingType = assembly.GetTypes().FirstOrDefault(t => t.GetInterface(nameof(IBuilderExlusionMapping)) != null);
                    if (exclusionMappingType != null)
                        BuilderExclusionMapping = (IBuilderExlusionMapping)Activator.CreateInstance(exclusionMappingType);
                }
            }
            catch { }

            //if (typeof(TE).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) != null)
            //{
                return (Builder<TE>)Activator.CreateInstance(typeof(Builder<TE>));
            //}
            //else
            //{
            //    var constructors = typeof(TE).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //    var constructorWithHigherNumberOfParameters = constructors.OrderByDescending(c => c.GetParameters()?.Count()).First();
            //    var parameterInfos = constructorWithHigherNumberOfParameters.GetParameters();

            //    var parameters = new List<object>();

            //    foreach(var parameterInfo in parameterInfos)
            //    {
            //        parameters.Add(GenerateAnonymousData(null, parameterInfo.ParameterType, string.Empty, 0, Enumerable.Empty<string>(), CharSet.Alphanumeric));
            //    }

            //    return (Builder<TE>)Activator.CreateInstance(typeof(Builder<TE>), parameters);
            //}
        }

        public Builder<TE> For(Operation operation)
        {
            CurrentOperation = operation;
            return this;
        }

        // Feature not ready
        //public static BuildFrom<TE> From(TE source)
        //{
        //    return new BuildFrom<TE>(source);
        //}

        protected Builder<TE> Exclude(List<string> exclusions)
        {
            Exclusions = exclusions;
            return this;
        }

        public Builder<TE> Exclude(params Expression<Func<TE, object>>[] exclusions)
        {
            var propertiesToBeExcluded = new List<string>();

            foreach (var exclusion in exclusions)
            {
                var memberExpression = exclusion.Body as MemberExpression ?? ((UnaryExpression)exclusion.Body).Operand as MemberExpression;

                string propertyName = GetPropertyName(memberExpression);
                if (propertyName != null) propertiesToBeExcluded.Add(propertyName.ToLower());
            }

            this.Exclusions = propertiesToBeExcluded;
            return this;
        }

        #region Build
        /// <inheritdoc cref="IBuilder{TE}.Build()"/>
        public virtual TE Build(int hierarchyDepth = 0, bool useRandomValues = true)
        {
            if (this.Exclusions == null && BuilderExclusionMapping != null && BuilderExclusionMapping.GetExclusionsFor(CurrentOperation, typeof(TE)).Any())
            {
                this.Exclusions = BuilderExclusionMapping.GetExclusionsFor(CurrentOperation, typeof(TE));
            }

            var e = (TE)CreateInstance(typeof(TE), hierarchyDepth, this.Exclusions);

            //TE e = default;

            //// Handle objects with no parameterless constructors such immutable
            //if (typeof(TE).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null)
            //{
            //    var constructors = typeof(TE).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //    var constructorWithHigherNumberOfParameters = constructors.OrderByDescending(c => c.GetParameters()?.Count()).First();
            //    var parameterInfos = constructorWithHigherNumberOfParameters.GetParameters();

            //    var parameters = new List<object>();

            //    foreach (var parameterInfo in parameterInfos)
            //    {
            //        parameters.Add(GenerateAnonymousData(null, parameterInfo.ParameterType, string.Empty, 0, Enumerable.Empty<string>(), CharSet.Alphanumeric));
            //    }

            //    var a = parameters.ToArray();
            //    return (TE)constructorWithHigherNumberOfParameters.Invoke(a);
            //}

            //e = (TE)Activator.CreateInstance(typeof(TE));

            if (!useRandomValues)
                return e;

            var properties = e.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var propertyInfo in properties)
            {
                // Property is ignored
                if (IsMemberInExludeList(propertyInfo.Name))
                    continue;

                // Property has not setter
                if (propertyInfo.GetSetMethod(true) == null)
                    continue;

                // Property has private setter and should not be set
                if (propertyInfo.GetSetMethod(true).IsPrivate && !SetPropertiesPrivateSetters)
                    continue;

                propertyInfo.GetSetMethod(true).Invoke(e, new object[] { GenerateAnonymousData(e, propertyInfo.PropertyType, propertyInfo.Name, hierarchyDepth, Exclusions) });
            }

            var fields = e.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (var fieldInfo in fields)
            {
                if (IsMemberInExludeList(fieldInfo.Name))
                    continue;
                fieldInfo.SetValue(e, GenerateAnonymousData(e, fieldInfo.FieldType, fieldInfo.Name, hierarchyDepth, Exclusions));
            }
            return e;
        }

        /// <inheritdoc cref="IBuilder{TE}.Build(Action{TE})"/>
        public virtual TE Build(Action<TE> entitySetupAction, int hierarchyDepth = 0, bool useRandomValues = true)
        {
            if (entitySetupAction == null)
                throw new ArgumentNullException($"{nameof(entitySetupAction)}");

            var entity = Build(hierarchyDepth, useRandomValues);
            entitySetupAction(entity);
            return entity;
        }
        #endregion

        #region BuildMany
        /// <inheritdoc cref="IBuilder{TE}.BuildMany(int, Action<TE, int>)"/>
        public virtual List<TE> BuildMany(int numberOfEntities, Action<TE, int> entitySetupAction = null, bool useRandomValues = true)
        {
            if (numberOfEntities < 1)
                throw new ArgumentOutOfRangeException($"{nameof(numberOfEntities)} must be greater than zero");

            var result = new List<TE>();
            for (var i = 1; i <= numberOfEntities; i++)
            {
                TE entity;
                if (useRandomValues)
                {
                    entity = Build();
                }
                else
                {
                    entity = (TE)Activator.CreateInstance(typeof(TE));
                }

                entitySetupAction?.Invoke(entity, i);
                result.Add(entity);
            }
            return result;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Return true if the member is in the exclusion list
        /// </summary>
        /// <param name="memberName">Name of the property or field</param>
        /// <returns>True if the member has been excluded</returns>
        private bool IsMemberInExludeList(string memberName)
        {
            return Exclusions?.Any(ex => !ex.Contains('.') && string.Equals(ex, memberName, StringComparison.InvariantCultureIgnoreCase)) ?? false;
        }

        /// <summary>
        /// Get the fully qualified name of the property passed in as member expression
        /// </summary>
        /// <param name="memberExpression">Expression containing the property</param>
        /// <param name="namePrefix">Pre defined prefix to appent to the property, also use by recursion to generate the name</param>
        /// <returns>The fully qualified name of the member expression property</returns>
        private static string GetPropertyName(MemberExpression memberExpression, string namePrefix = "")
        {
            var expession = memberExpression?.Expression;
            if (expession == null) return namePrefix;

            var me = (expession as MemberExpression);
            var propertyName = "";

            // First iteration always read property name
            if(string.IsNullOrEmpty(namePrefix))
                propertyName = (memberExpression.Member as MemberInfo)?.Name;

            // If member has name it means the iteration needs to recurse
            if (!string.IsNullOrEmpty(me?.Member?.Name))
                namePrefix = GetPropertyName(me, $"{me?.Member?.Name}.{namePrefix}");

            // If parent object was found add dot between obj and property
            if (!string.IsNullOrEmpty(namePrefix) && !namePrefix.EndsWith(".")) namePrefix = $"{namePrefix}.";

            return $"{namePrefix}{propertyName}";
        }

        /// <summary>
        /// Get random data for a given property to be set to the given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="memberType"></param>
        /// <param name="propertyName"></param>
        /// <param name="hierarchyDepth"></param>
        /// <param name="exclusions"></param>
        /// <returns>The entity with the property populated with random value</returns>
        internal virtual object GenerateAnonymousData(object entity, Type type, string propertyName, int hierarchyDepth, IEnumerable<string> exclusions)
        {
            return GenerateAnonymousData(entity, type, propertyName, hierarchyDepth, exclusions, DefaultStringCharSet);
        }

        internal virtual object CreateInstance(Type type, int hierarchyDepth, IEnumerable<string> exclusions)
        {
            if (type == typeof(int) || type == typeof(uint) || type == typeof(Int32) || type == typeof(UInt32) || type == typeof(Nullable<int>) || type == typeof(Nullable<uint>))
                return (object)Any.Int(3, false);

            if (type == typeof(string))
                return Any.String();

            if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(Nullable<sbyte>) || type == typeof(Nullable<byte>))
                return (object)Any.Byte();

            if (type == typeof(short) || type == typeof(ushort) || type == typeof(Int16) || type == typeof(UInt16) || type == typeof(Nullable<short>) || type == typeof(Nullable<ushort>))
                return (object)Any.Short();

            if (type == typeof(long) || type == typeof(ulong) || type == typeof(Int64) || type == typeof(UInt64) || type == typeof(Nullable<long>) || type == typeof(Nullable<ulong>))
                return (object)Any.Long();

            if (type == typeof(double) || type == typeof(Nullable<double>))
                return (object)Any.Double();

            if (type == typeof(decimal) || type == typeof(Nullable<decimal>))
                return (object)Any.Decimal();

            if (type == typeof(float) || type == typeof(Nullable<float>))
                return (object)Any.Float();

            if (type == typeof(char) || type == typeof(Nullable<char>))
                return (object)Any.Char();

            if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
                return (object)Any.DateTime();

            if (type == typeof(TimeSpan) || type == typeof(Nullable<TimeSpan>))
                return (object)Any.TimeSpan();

            if (type == typeof(Guid) || type == typeof(Nullable<Guid>))
                return (object)Any.Guid();

            if (type == typeof(Uri))
                return (object)Any.Uri();

            if (type == typeof(MailAddress))
                return (object)new MailAddress(Any.Email());

            if (type == typeof(object))
                return new object();

            if (type == typeof(StringBuilder))
                return new StringBuilder(Any.String());

            if (type?.BaseType == typeof(Enum))
            {
                var randomIndex = Any.Int(minValue: 0, maxValue: Enum.GetNames(type).Length - 1);
                return (object)Enum.GetValues(type).GetValue(randomIndex);
            }

            // Nullable Enums
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments()[0].IsEnum)
            {
                var enumType = type.GenericTypeArguments.First();
                var randomIndex = Any.Int(minValue: 0, maxValue: Enum.GetNames(enumType).Length - 1);
                return (object)Enum.GetValues(enumType).GetValue(randomIndex);
            }

            // If impropertly using builder to create enumerables or generic types throw
            if (type.IsGenericType || typeof(IEnumerable).IsAssignableFrom(type))
                throw new InvalidOperationException("To construct IEnumerables use the generic type directly in combination with the method BuildMany");


            object e = default;

            // Handle objects with no parameterless constructors such immutable
            if (typeof(TE).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null)
            {
                var constructors = typeof(TE).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var constructorWithHigherNumberOfParameters = constructors.OrderByDescending(c => c.GetParameters()?.Count()).First();
                var parameterInfos = constructorWithHigherNumberOfParameters.GetParameters();

                var parameters = new List<object>();

                foreach (var parameterInfo in parameterInfos)
                {
                    parameters.Add(GenerateAnonymousData(null, parameterInfo.ParameterType, string.Empty, 0, Enumerable.Empty<string>(), CharSet.Alphanumeric));
                }

                var a = parameters.ToArray();
                return (TE)constructorWithHigherNumberOfParameters.Invoke(a);
            }

            return (TE)Activator.CreateInstance(typeof(TE));
        }

        private static object GenerateAnonymousData(object entity, Type type, string propertyName, int hierarchyDepth, IEnumerable<string> exclusions, CharSet charSet)
        {
            if (type == typeof(string))
            {
                var propertyNameLowered = propertyName.ToLower();
                if (propertyName.Contains("email") || propertyName.Contains("mailaddress"))
                {
                    return $"{propertyName}_{Any.Email()}";
                }

                if (propertyName.Contains("url") || propertyName.Contains("website"))
                {
                    return $"{Any.Url()}";
                }

                return Any.String(propertyName, 15 + propertyName.Length, charSet);
            }

            if (type == typeof(int) || type == typeof(uint) || type == typeof(Int32) || type == typeof(UInt32) || type == typeof(Nullable<int>) || type == typeof(Nullable<uint>))
                return (object)Any.Int(3, false);

            if (type == typeof(string))
                return Any.String(length: 15, charSet: charSet);

            if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(Nullable<sbyte>) || type == typeof(Nullable<byte>))
                return (object)Any.Byte();

            if (type == typeof(short) || type == typeof(ushort) || type == typeof(Int16) || type == typeof(UInt16) || type == typeof(Nullable<short>) || type == typeof(Nullable<ushort>))
                return (object)Any.Short();

            if (type == typeof(long) || type == typeof(ulong) || type == typeof(Int64) || type == typeof(UInt64) || type == typeof(Nullable<long>) || type == typeof(Nullable<ulong>))
                return (object)Any.Long();

            if (type == typeof(double) || type == typeof(Nullable<double>))
                return (object)Any.Double();

            if (type == typeof(decimal) || type == typeof(Nullable<decimal>))
                return (object)Any.Decimal();

            if (type == typeof(float) || type == typeof(Nullable<float>))
                return (object)Any.Float();

            if (type == typeof(char) || type == typeof(Nullable<char>))
                return (object)Any.Char(charSet);

            if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
                return (object)Any.DateTime();

            if (type == typeof(TimeSpan) || type == typeof(Nullable<TimeSpan>))
                return (object)Any.TimeSpan();

            if (type == typeof(Guid) || type == typeof(Nullable<Guid>))
                return (object)Any.Guid();

            if (type == typeof(Uri))
                return (object)Any.Uri();

            if (type == typeof(MailAddress))
                return (object)new MailAddress(Any.Email());

            if (type == typeof(object))
                return new object();

            if (type == typeof(StringBuilder))
                return new StringBuilder(Any.String());

            if (type?.BaseType == typeof(Enum))
            {
                var randomIndex = Any.Int(minValue: 0, maxValue: Enum.GetNames(type).Length - 1);
                return (object)Enum.GetValues(type).GetValue(randomIndex);
            }

            // Nullable Enums
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments()[0].IsEnum)
            {
                var enumType = type.GenericTypeArguments.First();
                var randomIndex = Any.Int(minValue: 0, maxValue: Enum.GetNames(enumType).Length - 1);
                return (object)Enum.GetValues(enumType).GetValue(randomIndex);
            }
//--------------------------------------------------------
            // Handle IEnumerable members if the hierarchy depth has been set
            if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
            {
                var genericTypeArgument = type.GenericTypeArguments.FirstOrDefault();
                if (!genericTypeArgument.IsClass || genericTypeArgument.IsGenericType)
                {
                    return null;
                }

                // If hierarchy does not need to be populated return null or empty list according to configuration
                if (hierarchyDepth <= 0)
                {
                    if (InitializeNullCollectionsInsteadOfEmpty)
                        return null;
                    else
                        return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericTypeArgument));
                }

                var listOfChildEntities = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericTypeArgument));

                try
                {
                    for (var i = 1; i <= NumberOfNestedEntitiesInCollections; i++)
                    {
                        var childEntity = GenerateAnonymousDateForChildEntityObjectImpl(null, genericTypeArgument, propertyName, hierarchyDepth, exclusions?.Where(e => e.Contains('.')));
                        listOfChildEntities.Add(childEntity);
                    }
                    return listOfChildEntities;
                }
                catch (Exception)
                {
                    return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericTypeArgument));
                }
            }

            // Handle Reference types members if the hierarchy depth has been set
            if (hierarchyDepth > 0 && type.IsClass && !type.IsGenericType)
            {
                return GenerateAnonymousDateForChildEntityObjectImpl(entity, type, propertyName, hierarchyDepth, exclusions?.Where(e => e.Contains('.')));
            }
//--------------------------------------------------------
            return null;
        }

        /// <summary>
        /// When hierarchy is set the children objects are also generate with random values
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyType"></param>
        /// <param name="propertyName"></param>
        /// <param name="hierarchyDepth"></param>
        /// <param name="exclusions"></param>
        /// <returns></returns>
        internal object GenerateAnonymousDateForChildEntityObject(object entity, Type propertyType, string propertyName, int hierarchyDepth, IEnumerable<string> exclusions)
        {
            return GenerateAnonymousDateForChildEntityObjectImpl(entity, propertyType, propertyName, hierarchyDepth, exclusions);
        }

        private static object GenerateAnonymousDateForChildEntityObjectImpl(object entity, Type propertyType, string propertyName, int hierarchyDepth, IEnumerable<string> exclusions)
        {
            try
            {
                var genericBuilderType = typeof(Builder<>);
                var builderType = genericBuilderType.MakeGenericType(propertyType);
                var buildMethodInfo = builderType.GetMethod("Build", new[] { typeof(int), typeof(bool) });
                var builderObject = Activator.CreateInstance(builderType);

                if (exclusions != null && exclusions.Count() > 0)
                {
                    var excludeFieldInfo = builderType.GetField("Exclusions", BindingFlags.Instance | BindingFlags.NonPublic);
                    excludeFieldInfo.SetValue(builderObject, GetExclusionsRemovingFirstLevel(exclusions));
                }

                var childEntity = buildMethodInfo.Invoke(builderObject, new object[] { hierarchyDepth - 1, true });

                // ********** Prototype to try to set reference keys using ID conventions
                //var validIdTypes = new[]
                //{
                //    typeof(Int16),typeof(Int32),typeof(Int64)
                //};

                //// Search for an Id property in the child entity type
                //var childEntityIdProperty = childEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                //    .Where(prop =>
                //        prop.CanRead &&
                //        prop.CanWrite &&
                //        prop.Name.ToLower() == "id" &&
                //        validIdTypes.Contains(prop.PropertyType)
                //    ).FirstOrDefault();

                // Unable to find the Id property in the child entity
                //if (childEntityIdProperty == null) return childEntity;

                // Search for a reference child entity id in the property's parent entity
                //var entityChildEntityIdProperty = entity?.GetType()
                //    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                //    .Where(prop =>
                //        prop.CanRead &&
                //        prop.CanWrite &&
                //        // By convention the reference keys are named using the format: {EntityName}Id
                //        prop.Name.ToLower() == $"{propertyName}Id".ToLower() &&
                //        // Use the child Id property type to filter the reference Id property
                //        childEntityIdProperty.PropertyType == prop.PropertyType
                //    ).FirstOrDefault();

                // If the parent entity has a matching property with the id of the child entity, set it
                //if (entityChildEntityIdProperty != null)
                //{
                //    entityChildEntityIdProperty.SetValue(entity, childEntityIdProperty.GetValue(childEntity));
                //}
                //***************
                return childEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Remove the first level of an exclusion, which represent the root object/type.
        /// This method uses . delimiter to remove the first part and return a new list.
        /// If an element of the list has no parent object, it means that was already a property name and will be removed.
        /// </summary>
        /// <param name="exclusions"></param>
        /// <returns></returns>
        private static List<string> GetExclusionsRemovingFirstLevel(IEnumerable<string> exclusions)
        {
            if (exclusions == null) return null;

            var result = new List<string>();
            foreach (var exclusion in exclusions)
            {
                var dotPosition = exclusion.IndexOf('.');
                if (dotPosition < 0) continue;

                result.Add(exclusion.Substring(dotPosition+1));
            }

            return result;
        }
        #endregion
    }
}
