using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Builder
{
    /// <inheritdoc cref="IBuilder{TE}"/>
    public class Builder<TE> : IBuilder<TE> where TE : class, new()
    {
        public static int NumberOfNestedEntitiesInCollections { get; set; } = 5;

        public static Builder<TE> New()
        {
            return (Builder<TE>)Activator.CreateInstance(typeof(Builder<TE>));
        }

        /// <inheritdoc cref="IBuilder{TE}.Build()"/>
        public virtual TE Build(int hierarchyDepth = 0)
        {
            var e = (TE)Activator.CreateInstance(typeof(TE));
            var properties = e.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            
            foreach (var propertyInfo in properties)
            {
                propertyInfo.SetValue(e, GenerateAnonymousData(e, propertyInfo.PropertyType, propertyInfo.Name, hierarchyDepth));
            }

            var fields = e.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (var fieldInfo in fields)
            {
                fieldInfo.SetValue(e, GenerateAnonymousData(e, fieldInfo.FieldType, fieldInfo.Name, hierarchyDepth));
            }
            return e;
        }
        
        /// <inheritdoc cref="IBuilder{TE}.BuildMany(int, Action<TE, int>)"/>
        public virtual List<TE> BuildMany(int numberOfEntities, Action<TE, int> entitySetupAction = null)
        {
            if (numberOfEntities < 1)
                throw new ArgumentOutOfRangeException($"{nameof(numberOfEntities)} must be greater than zero");

            var result = new List<TE>();
            for (var i = 1; i <= numberOfEntities; i++)
            {
                var entity = Build();
                entitySetupAction?.Invoke(entity, i);
                result.Add(entity);
            }
            return result;
        }

        /// <inheritdoc cref="IBuilder{TE}.Build(Action{TE})"/>
        public virtual TE Build(Action<TE> entitySetupAction)
        {
            if (entitySetupAction == null)
                throw new ArgumentNullException($"{nameof(entitySetupAction)}");

            var entity = Build();
            entitySetupAction(entity);
            return entity;
        }

        protected virtual object GenerateAnonymousData(object entity, Type propertyType, string propertyName, int hierarchyDepth)
        {
            if (propertyType == typeof(string))
                return Any.String(propertyName);

            if (propertyType == typeof(sbyte) || propertyType == typeof(byte) || propertyType == typeof(Byte) || propertyType == typeof(SByte))
                return Any.Byte();

            if (propertyType == typeof(short) || propertyType == typeof(ushort) || propertyType == typeof(Int16) || propertyType == typeof(UInt16))
                return Any.Short();

            if (propertyType == typeof(int) || propertyType == typeof(uint) || propertyType == typeof(Int32) || propertyType == typeof(UInt32))
                return Any.Int(3, false);

            if (propertyType == typeof(long) || propertyType == typeof(ulong) || propertyType == typeof(Int64) || propertyType == typeof(UInt64))
                return Any.Long();

            if (propertyType == typeof(double) || propertyType == typeof(Double))
                return Any.Double();

            if (propertyType == typeof(decimal) || propertyType == typeof(Decimal))
                return Any.Decimal();

            if (propertyType == typeof(float) || propertyType == typeof(Single))
                return Any.Float();

            if (propertyType == typeof(char) || propertyType == typeof(Char))
                return Any.Char();

            if (propertyType == typeof(DateTime))
                return Any.DateTime();

            if (propertyType == typeof(TimeSpan))
                return Any.TimeSpan();

            if (propertyType.IsValueType)
            {
                return Activator.CreateInstance(propertyType);
            }

            if (hierarchyDepth > 0 && propertyType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(propertyType))
            {
                var genericTypeArgument = propertyType.GenericTypeArguments.FirstOrDefault();                
                if (!genericTypeArgument.IsClass || genericTypeArgument.IsGenericType)
                {
                    return null;
                }
                var listOfChildEntities = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericTypeArgument));

                try
                {
                    for (var i = 1; i <= NumberOfNestedEntitiesInCollections; i++)
                    {                        
                        var childEntity = GenerateAnonymousDateForChildEntityObject(null, genericTypeArgument, propertyName, hierarchyDepth);
                        listOfChildEntities.Add(childEntity);                        
                    }
                    return listOfChildEntities;
                }
                catch (Exception)
                {
                    return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericTypeArgument));
                }
            }

            if (hierarchyDepth > 0 && propertyType.IsClass && !propertyType.IsGenericType)
            {
                return GenerateAnonymousDateForChildEntityObject(entity, propertyType, propertyName, hierarchyDepth);
            }

            return null;
        }

        protected object GenerateAnonymousDateForChildEntityObject(object entity, Type propertyType, string propertyName, int hierarchyDepth)
        {
            try
            {
                var validIdTypes = new[]
                {
                    typeof(Int16),typeof(Int32),typeof(Int64)
                };

                var genericBuilderType = typeof(Builder<>);
                var builderType = genericBuilderType.MakeGenericType(propertyType);
                var methodInfo = builderType.GetMethod(nameof(this.Build), new[] { typeof(int) });
                var builderObject = Activator.CreateInstance(builderType);
                var childEntity = methodInfo.Invoke(builderObject, new object[] { hierarchyDepth - 1 });

                // Search for an Id property in the child entity type
                var childEntityIdProperty = childEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop =>
                        prop.CanRead &&
                        prop.CanWrite &&
                        prop.Name.ToLower() == "id" &&
                        validIdTypes.Contains(prop.PropertyType)
                    ).FirstOrDefault();

                // Unable to find the Id property in the child entity
                if (childEntityIdProperty == null) return childEntity;

                // Search for a reference child entity id in the property's parent entity
                var entityChildEntityIdProperty = entity?.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop =>
                        prop.CanRead &&
                        prop.CanWrite &&
                        // By convention the reference keys are named using the format: {EntityName}Id
                        prop.Name.ToLower() == $"{propertyName}Id".ToLower() &&
                        // Use the child Id property type to filter the reference Id property
                        childEntityIdProperty.PropertyType == prop.PropertyType
                    ).FirstOrDefault();

                // If the parent entity has a matching property with the id of the child entity, set it
                if (entityChildEntityIdProperty != null)
                {
                    entityChildEntityIdProperty.SetValue(entity, childEntityIdProperty.GetValue(childEntity));
                }

                return childEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
