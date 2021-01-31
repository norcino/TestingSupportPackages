using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Builder
{
    // TODO Handled Immutable Objects
    // Maybe assume Builder is only used to create DTOs and Entities so ref objects are other DTOs or Entities?
    // Add BindingFlags.NonPublic when getting properties

    /// <inheritdoc cref="IBuilder{TE}"/>
    public class Builder<TE> : IBuilder<TE> where TE : class, new()
    {
        internal List<string> Exclusions;
        public static int NumberOfNestedEntitiesInCollections { get; set; } = 5;

        public static Builder<TE> New()
        {
            return (Builder<TE>)Activator.CreateInstance(typeof(Builder<TE>));
        }

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
            var e = (TE)Activator.CreateInstance(typeof(TE));

            if (!useRandomValues) return e;
            
            var properties = e.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            
            foreach (var propertyInfo in properties)
            {
                if (IsMemberInExludeList(propertyInfo.Name)) continue;
                propertyInfo.SetValue(e, GenerateAnonymousData(e, propertyInfo.PropertyType, propertyInfo.Name, hierarchyDepth, Exclusions));
            }

            var fields = e.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (var fieldInfo in fields)
            {
                if (IsMemberInExludeList(fieldInfo.Name)) continue;
                fieldInfo.SetValue(e, GenerateAnonymousData(e, fieldInfo.FieldType, fieldInfo.Name, hierarchyDepth, Exclusions));
            }
            return e;
        }

        /// <inheritdoc cref="IBuilder{TE}.Build(Action{TE})"/>
        public virtual TE Build(Action<TE> entitySetupAction, bool useRandomValues = true)
        {
            if (entitySetupAction == null)
                throw new ArgumentNullException($"{nameof(entitySetupAction)}");

            var entity = Build();
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
            return Exclusions.Any(ex => !ex.Contains('.') && ex == memberName.ToLower());
        }


        private static string GetPropertyName(MemberExpression memberExpression, string soFar = "")
        {
            var expession = memberExpression?.Expression;
            if (expession == null) return soFar;

            var me = (expession as MemberExpression);
            var propertyName = "";
            
            // First iteration always read property name
            if(string.IsNullOrEmpty(soFar))
                propertyName = (memberExpression.Member as MemberInfo)?.Name;

            // If member has name it means the iteration needs to recurse
            if (!string.IsNullOrEmpty(me?.Member?.Name))
                soFar = GetPropertyName(me, $"{me?.Member?.Name}.{soFar}");

            // If parent object was found add dot between obj and property
            if (!string.IsNullOrEmpty(soFar) && !soFar.EndsWith(".")) soFar = $"{soFar}.";

            return $"{soFar}{propertyName}";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="memberType"></param>
        /// <param name="propertyName"></param>
        /// <param name="hierarchyDepth"></param>
        /// <param name="exclusions"></param>
        /// <returns></returns>
        internal virtual object GenerateAnonymousData(object entity, Type memberType, string propertyName, int hierarchyDepth, List<string> exclusions)
        {
            if (memberType == typeof(string))
                return Any.String(propertyName);

            if (memberType == typeof(sbyte) || memberType == typeof(byte) || memberType == typeof(Byte) || memberType == typeof(SByte))
                return Any.Byte();

            if (memberType == typeof(short) || memberType == typeof(ushort) || memberType == typeof(Int16) || memberType == typeof(UInt16))
                return Any.Short();

            if (memberType == typeof(int) || memberType == typeof(uint) || memberType == typeof(Int32) || memberType == typeof(UInt32))
                return Any.Int(3, false);

            if (memberType == typeof(long) || memberType == typeof(ulong) || memberType == typeof(Int64) || memberType == typeof(UInt64))
                return Any.Long();

            if (memberType == typeof(double) || memberType == typeof(Double))
                return Any.Double();

            if (memberType == typeof(decimal) || memberType == typeof(Decimal))
                return Any.Decimal();

            if (memberType == typeof(float) || memberType == typeof(Single))
                return Any.Float();

            if (memberType == typeof(char) || memberType == typeof(Char))
                return Any.Char();

            if (memberType == typeof(DateTime))
                return Any.DateTime();

            if (memberType == typeof(TimeSpan))
                return Any.TimeSpan();
            
            if (memberType?.BaseType == typeof(Enum))
            {
                var randomIndex = Any.Int(minValue: 0, maxValue: Enum.GetNames(memberType).Length - 1);
                return Enum.GetValues(memberType).GetValue(randomIndex);
            }

            // Handle all value types not handled above
            if (memberType.IsValueType)
            {
                return Activator.CreateInstance(memberType);
            }

            // Handle IEnumerable members if the hierarchy depth has been set
            if (hierarchyDepth > 0 && memberType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(memberType))
            {
                var genericTypeArgument = memberType.GenericTypeArguments.FirstOrDefault();                
                if (!genericTypeArgument.IsClass || genericTypeArgument.IsGenericType)
                {
                    return null;
                }
                var listOfChildEntities = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericTypeArgument));

                try
                {
                    for (var i = 1; i <= NumberOfNestedEntitiesInCollections; i++)
                    {                        
                        var childEntity = GenerateAnonymousDateForChildEntityObject(null, genericTypeArgument, propertyName, hierarchyDepth, exclusions.Where(e => e.Contains('.')).ToList());
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
            if (hierarchyDepth > 0 && memberType.IsClass && !memberType.IsGenericType)
            {
                return GenerateAnonymousDateForChildEntityObject(entity, memberType, propertyName, hierarchyDepth, exclusions.Where(e => e.Contains('.')).ToList());
            }

            return null;
        }

        internal object GenerateAnonymousDateForChildEntityObject(object entity, Type propertyType, string propertyName, int hierarchyDepth, List<string> exclusions)
        {
            try
            {
                var genericBuilderType = typeof(Builder<>);
                var builderType = genericBuilderType.MakeGenericType(propertyType);
                var buildMethodInfo = builderType.GetMethod(nameof(this.Build), new[] { typeof(int), typeof(bool) });
                var excludeFieldInfo = builderType.GetField(nameof(this.Exclusions), BindingFlags.Instance | BindingFlags.NonPublic);

                var builderObject = Activator.CreateInstance(builderType);
                excludeFieldInfo.SetValue(builderObject, GetExclusionsRemovingFirstLevel(exclusions));
                
                var childEntity = buildMethodInfo.Invoke(builderObject, new object[] { hierarchyDepth - 1, true });

                // ********** Prototype to try to ser reference keys using ID conventions
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

        private List<string> GetExclusionsRemovingFirstLevel(List<string> exclusions)
        {
            var result = new List<string>();
            foreach(var exclusion in exclusions)
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
