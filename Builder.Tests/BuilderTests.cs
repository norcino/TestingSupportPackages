using FluentAssertion.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnonymousData;
using System;
using System.Linq;
using BuilderExclusionMappings;

namespace Builder.Tests
{
    [TestClass]
    public class BuilderTests
    {
        [TestMethod]
        public void BuildMany_should_generate_the_expected_number_of_results_with_the_same_default_behaviour_and_applies_the_customization_action()
        {
            var builtObjects = Builder<SutPoco>.New().BuildMany(100, (o,i) =>
            {
                o.StringField = $"Element#{i}";
                o.IntField = i;
                o.IntProperty = 0;
                o.StringProperty = null;
            });
            
            Assert.That.These(builtObjects)
                .HaveCount(100)
                .Contains(e => e.StringField == "Element#1").And().Contains(e => e.StringField == "Element#100")
                .And()
                .DoesNotContain(e => e.StringField == "Element#0").And().DoesNotContain(e => e.StringField == "Element#101")
                .And()
                .Contains(e => e.IntField == 1).And().Contains(e => e.IntField == 100)
                .And()
                .DoesNotContain(e => e.IntField == 0).And().DoesNotContain(e => e.IntField == 101)

                .AndEachElement()
                    .IsOfType(typeof(SutPoco))
                    .HasNonDefault(i => i.TimeSpanProperty)
                    .HasNonDefault(i => i.LongField)
                    .HasNonDefault(i => i.DateTimeProperty)
                    .HasNonDefault(i => i.GuidProperty)
                    .HasProperty(i => i.StringField).WhichValueStartsWith("Element#").And()
                    .HasProperty(i => i.StringProperty).WithValue(null).And()                    
                    .HasProperty(i => i.IntProperty).WithValue(0);

        }

        [TestMethod]
        public void BuildMany_should_generate_the_expected_number_of_results_with_properties_setup_as_specified_without_using_reandom_values()
        {
            var builtObjects = Builder<SutPoco>.New().BuildMany(100, (o, i) =>
            {
                o.StringField = $"Element#{i}";
                o.IntField = i;
                o.IntProperty = 0;
                o.StringProperty = null;
            }, false);

            Assert.That.These(builtObjects)
                .HaveCount(100)
                .Contains(e => e.StringField == "Element#1").And().Contains(e => e.StringField == "Element#100")
                .And()
                .DoesNotContain(e => e.StringField == "Element#0").And().DoesNotContain(e => e.StringField == "Element#101")
                .And()
                .Contains(e => e.IntField == 1).And().Contains(e => e.IntField == 100).And()
                .And()
                .DoesNotContain(e => e.IntField == 0).And().DoesNotContain(e => e.IntField == 101)

                .AndEachElement()
                    .IsOfType(typeof(SutPoco))
                    .HasDefault(i => i.TimeSpanProperty)
                    .HasDefault(i => i.LongField)
                    .HasDefault(i => i.DateTimeProperty)
                    .HasProperty(i => i.StringField).WhichValueStartsWith("Element#").And()
                    .HasProperty(i => i.StringProperty).WithValue(null).And()
                    .HasProperty(i => i.IntProperty).WithValue(0);
        }

        [TestMethod]
        public void BuildMany_should_generate_the_expected_number_of_results_with_the_same_default_behaviour_expected_for_Build()
        {
            var builtObjects = Builder<SutPoco>.New().BuildMany(100);

            Assert.That.These(builtObjects)
                .HaveCount(100)
                .AndEachElement()
                    .IsOfType(typeof(SutPoco))
                    .HasNull(i => i.AnotherPocoField)
                    .HasNull(i => i.AnotherPocoProperty)
                    .HasEmpty(i => i.MorePocosField)
                    .HasEmpty(i => i.MorePocosProperty)
                    .HasNonDefault(i => i.CharField)
                    .HasNonDefault(o => o.CharProperty);
        }

        [TestMethod]
        public void Build_should_allow_to_customize_creation_to_highlight_relevant_properties_and_fields()
        {
            var expectedBoolField = Any.Bool();
            var expectedIntField = Any.Int();
            var expectedLongField = Any.Long();
            var expectedStringField = Any.String();
            var expectedDecimalField = Any.Decimal();
            var expectedDoubleField = Any.Double();
            var expectedFloatField = Any.Float();
            var expectedCharField = Any.Char();
            var expectedDateTimeField = Any.DateTime();
            var expectedTimeSpanField = Any.TimeSpan();
            var expectedEnumField = Any.In<SutEnum>();
            var expectedBoolProperty = Any.Bool();
            var expectedIntProperty = Any.Int();
            var expectedLongProperty = Any.Long();
            var expectedStringProperty = Any.String();
            var expectedDecimalProperty = Any.Decimal();
            var expectedDoubleProperty = Any.Double();
            var expectedFloatProperty = Any.Float();
            var expectedCharProperty = Any.Char();
            var expectedDateTimeProperty = Any.DateTime();
            var expectedTimeSpanProperty = Any.TimeSpan();
            var expectedEnumProperty = Any.In<SutEnum>();

            var builtObject = Builder<SutPoco>.New().Build(o => {
                o.BoolField = expectedBoolField;
                o.IntField = expectedIntField;
                o.LongField = expectedLongField;
                o.StringField = expectedStringField;
                o.DecimalField = expectedDecimalField;
                o.DoubleField = expectedDoubleField;
                o.FloatField = expectedFloatField;
                o.CharField = expectedCharField;
                o.DateTimeField = expectedDateTimeField;
                o.TimeSpanField = expectedTimeSpanField;
                o.EnumField = expectedEnumField;
                o.BoolProperty = expectedBoolProperty;
                o.IntProperty = expectedIntProperty;
                o.LongProperty = expectedLongProperty;
                o.StringProperty = expectedStringProperty;
                o.DecimalProperty = expectedDecimalProperty;
                o.DoubleProperty = expectedDoubleProperty;
                o.FloatProperty = expectedFloatProperty;
                o.CharProperty = expectedCharProperty;
                o.DateTimeProperty = expectedDateTimeProperty;
                o.TimeSpanProperty = expectedTimeSpanProperty;
                o.EnumProperty = expectedEnumProperty;
            });

            Assert.That.The(builtObject)
                .HasProperty(o => o.BoolProperty)
                    .WithValue(expectedBoolProperty)
                .And()
                .HasProperty(o => o.IntField)
                    .WithValue(expectedIntField)
                .And()
                .HasProperty(o => o.CharField)
                    .WithValue(expectedCharField)
                .And()
                .HasProperty(o => o.DateTimeField)
                    .WithValue(expectedDateTimeField)
                .And()
                .HasProperty(o => o.DecimalField)
                    .WithValue(expectedDecimalField)
                .And()
                .HasProperty(o => o.DoubleField)
                    .WithValue(expectedDoubleField)
                .And()
                .HasProperty(o => o.EnumField)
                    .WithValue(expectedEnumField)
                .And()
                .HasProperty(o => o.FloatField)
                    .WithValue(expectedFloatField)
                .And()
                .HasProperty(o => o.LongField)
                    .WithValue(expectedLongField)
                .And()
                .HasProperty(o => o.StringField)
                    .WithValue(expectedStringField)
                .And()
                .HasProperty(o => o.TimeSpanField)
                    .WithValue(expectedTimeSpanField);
        }

        [TestMethod]
        public void Build_should_return_the_object_with_all_properties_and_fields_populated_but_not_children_objects()
        {
            Any.ExcludeDefaultValues(true);
            var builtObject = Builder<SutPoco>.New().Build();

            Assert.That.The(builtObject)
                .IsNotNull().And()
                .HasNull(o => o.AnotherPocoField).And()
                .HasNull(o => o.AnotherPocoProperty).And()
                .HasEmpty(o => o.MorePocosField).And()
                .HasEmpty(o => o.MorePocosProperty).And()
                .HasNonDefault(o => o.CharField).And()
                .HasNonDefault(o => o.CharProperty).And()
                .HasNonDefault(o => o.DateTimeField).And()
                .HasNonDefault(o => o.DateTimeProperty).And()
                .HasNonDefault(o => o.DecimalField).And()
                .HasNonDefault(o => o.DecimalProperty).And()
                .HasNonDefault(o => o.DoubleField).And()
                .HasNonDefault(o => o.DoubleProperty).And()
                .HasNonDefault(o => o.FloatField).And()
                .HasNonDefault(o => o.FloatProperty).And()
                .HasNonDefault(o => o.IntField).And()
                .HasNonDefault(o => o.IntProperty).And()
                .HasNonDefault(o => o.LongField).And()
                .HasNonDefault(o => o.LongProperty).And()
                .HasNonDefault(o => o.StringField).And()
                .HasNonDefault(o => o.StringProperty).And()
                .HasNonDefault(o => o.TimeSpanField).And()
                .HasNonDefault(o => o.TimeSpanProperty).And()                
                .HasNonDefault(o => o.EnumField).And()
                .HasNonDefault(o => o.EnumProperty).And()
                .HasNonDefault(o => o.EmailProperty).And()
                .HasNonDefault(o => o.UriProperty);
        }

        [TestMethod]
        public void Build_should_set_Empty_enumerations_by_default_when_there_is_no_hierarchy()
        {
            var builtObjects = Builder<SutPoco>.New().Build();

            Assert.That.This(builtObjects)
                .HasEmpty(o => o.MorePocosProperty).And()
                .HasEmpty(o => o.MorePocosField);
        }

        [TestMethod]
        public void Build_should_set_Null_enumerations_when_so_configured_with_no_hierarchy()
        {
            Builder.InitializeNullCollectionsInsteadOfEmpty = true;

            var builtObjects = Builder<SutPoco>.New().Build();

            Assert.That.This(builtObjects)
                .HasNull(o => o.MorePocosProperty).And()
                .HasNull(o => o.MorePocosField);

            Builder.InitializeNullCollectionsInsteadOfEmpty = false;
        }

        [TestMethod]
        public void Build_should_return_the_object_with_all_properties_fields_and_first_level_children_objects_populated_when_specified()
        {
            Any.ExcludeDefaultValues(true);
            var builtObject = Builder<SutPoco>.New().Build(1);

            Assert.That.The(builtObject)
                .IsNotNull().And()
                .HasNonNull(o => o.AnotherPocoField, "With depth 1 the child object is expected to be populated")
                    .HasNonDefault(o => o.AnotherPocoField.CharField).And()
                    .HasNonDefault(o => o.AnotherPocoField.CharProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.DateTimeField).And()
                    .HasNonDefault(o => o.AnotherPocoField.DateTimeProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.DecimalField).And()
                    .HasNonDefault(o => o.AnotherPocoField.DecimalProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.DoubleField).And()
                    .HasNonDefault(o => o.AnotherPocoField.DoubleProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.FloatField).And()
                    .HasNonDefault(o => o.AnotherPocoField.FloatProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.IntField).And()
                    .HasNonDefault(o => o.AnotherPocoField.IntProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.LongField).And()
                    .HasNonDefault(o => o.AnotherPocoField.LongProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.StringField).And()
                    .HasNonDefault(o => o.AnotherPocoField.StringProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.TimeSpanField).And()
                    .HasNonDefault(o => o.AnotherPocoField.TimeSpanProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.EnumField).And()
                    .HasNonDefault(o => o.AnotherPocoField.EnumProperty).And()
                    .HasNull(o => o.AnotherPocoField.AnotherPocoField, "With depth 1 the grandchild object is expected to be null has it has depth 2").And()
                
                .HasNonNull(o => o.AnotherPocoProperty, "With depth 1 the child object is expected to be populated").And()
                    .HasNonDefault(o => o.AnotherPocoProperty.CharField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.CharProperty).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.DateTimeField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.DateTimeProperty).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.DecimalField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.DecimalProperty).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.DoubleField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.DoubleProperty).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.FloatField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.FloatProperty).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.IntField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.IntProperty).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.LongField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.LongProperty).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.StringField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.StringProperty).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.TimeSpanField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.TimeSpanProperty).And()
                    .HasNonDefault(o => o.AnotherPocoField.EnumField).And()
                    .HasNonDefault(o => o.AnotherPocoField.EnumProperty).And()
                    .HasNull(o => o.AnotherPocoProperty.AnotherPocoProperty, "With depth 1 the grandchild object is expected to be null has it has depth 2").And()
               
                .HasNonEmpty(o => o.MorePocosField).And()
                .HasNonEmpty(o => o.MorePocosProperty).And()
                .HasNonDefault(o => o.CharField).And()
                .HasNonDefault(o => o.CharProperty).And()
                .HasNonDefault(o => o.DateTimeField).And()
                .HasNonDefault(o => o.DateTimeProperty).And()
                .HasNonDefault(o => o.DecimalField).And()
                .HasNonDefault(o => o.DecimalProperty).And()
                .HasNonDefault(o => o.DoubleField).And()
                .HasNonDefault(o => o.DoubleProperty).And()
                .HasNonDefault(o => o.FloatField).And()
                .HasNonDefault(o => o.FloatProperty).And()
                .HasNonDefault(o => o.IntField).And()
                .HasNonDefault(o => o.IntProperty).And()
                .HasNonDefault(o => o.LongField).And()
                .HasNonDefault(o => o.LongProperty).And()
                .HasNonDefault(o => o.StringField).And()
                .HasNonDefault(o => o.StringProperty).And()
                .HasNonDefault(o => o.TimeSpanField).And()
                .HasNonDefault(o => o.TimeSpanProperty);

            Assert.That.These(builtObject.MorePocosField)
                .HaveCount(Builder<SutPoco>.NumberOfNestedEntitiesInCollections)
                .AndEachElement()
                    .HasNonDefault(o => o.CharField).And()
                    .HasNonDefault(o => o.CharProperty).And()
                    .HasNonDefault(o => o.DateTimeField).And()
                    .HasNonDefault(o => o.DateTimeProperty).And()
                    .HasNonDefault(o => o.DecimalField).And()
                    .HasNonDefault(o => o.DecimalProperty).And()
                    .HasNonDefault(o => o.DoubleField).And()
                    .HasNonDefault(o => o.DoubleProperty).And()
                    .HasNonDefault(o => o.FloatField).And()
                    .HasNonDefault(o => o.FloatProperty).And()
                    .HasNonDefault(o => o.IntField).And()
                    .HasNonDefault(o => o.IntProperty).And()
                    .HasNonDefault(o => o.LongField).And()
                    .HasNonDefault(o => o.LongProperty).And()
                    .HasNonDefault(o => o.StringField).And()
                    .HasNonDefault(o => o.StringProperty).And()
                    .HasNonDefault(o => o.TimeSpanField).And()
                    .HasNonDefault(o => o.TimeSpanProperty).And()
                    .HasNonDefault(o => o.EnumField).And()
                    .HasNonDefault(o => o.EnumProperty).And()
                    .HasNull(o => o.AnotherPocoProperty, "Expected child of a child to be null as depth is 1").And()
                    .HasNull(o => o.AnotherPocoField, "Expected child of a child to be null as depth is 1").And()
                    .HasEmpty(o => o.MorePocosField, "Expected child of a child to be null as depth is 1").And()
                    .HasEmpty(o => o.MorePocosProperty, "Expected child of a child to be null as depth is 1");

            Assert.That.These(builtObject.MorePocosProperty)
                .HaveCount(Builder<SutPoco>.NumberOfNestedEntitiesInCollections)
                .AndEachElement()
                    .HasNonDefault(o => o.CharField).And()
                    .HasNonDefault(o => o.CharProperty).And()
                    .HasNonDefault(o => o.DateTimeField).And()
                    .HasNonDefault(o => o.DateTimeProperty).And()
                    .HasNonDefault(o => o.DecimalField).And()
                    .HasNonDefault(o => o.DecimalProperty).And()
                    .HasNonDefault(o => o.DoubleField).And()
                    .HasNonDefault(o => o.DoubleProperty).And()
                    .HasNonDefault(o => o.FloatField).And()
                    .HasNonDefault(o => o.FloatProperty).And()
                    .HasNonDefault(o => o.IntField).And()
                    .HasNonDefault(o => o.IntProperty).And()
                    .HasNonDefault(o => o.LongField).And()
                    .HasNonDefault(o => o.LongProperty).And()
                    .HasNonDefault(o => o.StringField).And()
                    .HasNonDefault(o => o.StringProperty).And()
                    .HasNonDefault(o => o.TimeSpanField).And()
                    .HasNonDefault(o => o.TimeSpanProperty).And()
                    .HasNonDefault(o => o.EnumField).And()
                    .HasNonDefault(o => o.EnumProperty).And()
                    .HasNull(o => o.AnotherPocoProperty, "Expected child of a child to be null as depth is 1").And()
                    .HasNull(o => o.AnotherPocoField, "Expected child of a child to be null as depth is 1").And()
                    .HasEmpty(o => o.MorePocosField, "Expected child of a child to be null as depth is 1").And()
                    .HasEmpty(o => o.MorePocosProperty, "Expected child of a child to be null as depth is 1");
        }

        [TestMethod]
        public void Build_should_return_the_object_with_all_properties_fields_and_two_levels_of_children_objects_populated_when_specified()
        {
            Any.ExcludeDefaultValues(true);
            var builtObject = Builder<SutPoco>.New().Build(hierarchyDepth: 2);

            Assert.That.This(builtObject)
                .IsNotNull().And()
                .HasNonNull(o => o.AnotherPocoField, "Child of the build entity has depth 1 and expected to be non-null")
                    .HasNonDefault(o => o.AnotherPocoField.CharField).And()
                    .HasNonDefault(o => o.AnotherPocoField.CharProperty).And()
                    .HasNonNull(o => o.AnotherPocoField.AnotherPocoField, "Grandchild of the build entity has depth 2 and expected to be non-null").And()
                        .HasNonDefault(o => o.AnotherPocoField.AnotherPocoField.CharField).And()
                        .HasNonDefault(o => o.AnotherPocoField.AnotherPocoField.CharProperty).And()
                        .HasNull(o => o.AnotherPocoField.AnotherPocoField.AnotherPocoField, "Great-grandchild of the build entity has depth 3 and expected to be null").And()
                .HasNonNull(o => o.AnotherPocoProperty, "Child of the build entity has depth 1 and expected to be non-null").And()
                    .HasNonDefault(o => o.AnotherPocoProperty.CharField).And()
                    .HasNonDefault(o => o.AnotherPocoProperty.CharProperty).And()
                    .HasNonNull(o => o.AnotherPocoProperty.AnotherPocoProperty, "Grandchild of the build entity has depth 2 and expected to be non-null").And()
                        .HasNonDefault(o => o.AnotherPocoProperty.AnotherPocoProperty.CharField).And()
                        .HasNonDefault(o => o.AnotherPocoProperty.AnotherPocoProperty.CharProperty).And()
                        .HasNull(o => o.AnotherPocoProperty.AnotherPocoProperty.AnotherPocoProperty, "Great-grandchild of the build entity has depth 3 and expected to be null").And()
                .HasNonEmpty(o => o.MorePocosField).And()
                .HasNonEmpty(o => o.MorePocosProperty).And()
                .HasNonDefault(o => o.CharField).And()
                .HasNonDefault(o => o.CharProperty);

            Assert.That.These(builtObject.MorePocosField)
                .HaveCount(Builder<SutPoco>.NumberOfNestedEntitiesInCollections)
                .AndEachElement()
                    .HasNonDefault(o => o.CharField).And()
                    .HasNonDefault(o => o.CharProperty).And()
                    .HasNonNull(o => o.AnotherPocoField, "Child of an item of the collection has depth 2 and expected to be not null").And()
                        .HasNonDefault(o => o.AnotherPocoField.CharField).And()
                        .HasNonDefault(o => o.AnotherPocoField.CharProperty).And()
                        .HasNull(o => o.AnotherPocoField.AnotherPocoField, "Grandchild of an item of the collection has depth 3 and expected to be null").And()        
                    .HasNonNull(o => o.AnotherPocoProperty, "Child of an item of the collection has depth 2 and expected to be not null").And()
                        .HasNonDefault(o => o.AnotherPocoProperty.CharField).And()
                        .HasNonDefault(o => o.AnotherPocoProperty.CharProperty).And()
                        .HasNull(o => o.AnotherPocoProperty.AnotherPocoProperty, "Grandchild of an item of the collection has depth 3 and expected to be null");

            Assert.That.These(builtObject.MorePocosProperty)
                .HaveCount(Builder<SutPoco>.NumberOfNestedEntitiesInCollections)
                .AndEachElement()
                    .HasNonDefault(o => o.CharField).And()
                    .HasNonDefault(o => o.CharProperty).And()
                    .HasNonNull(o => o.AnotherPocoField, "Child of an item of the collection has depth 2 and expected to be not null").And()
                        .HasNonDefault(o => o.AnotherPocoField.CharField).And()
                        .HasNonDefault(o => o.AnotherPocoField.CharProperty).And()
                        .HasNull(o => o.AnotherPocoField.AnotherPocoField, "Grandchild of an item of the collection has depth 3 and expected to be null").And()
                    .HasNonNull(o => o.AnotherPocoProperty, "Child of an item of the collection has depth 2 and expected to be not null").And()
                        .HasNonDefault(o => o.AnotherPocoProperty.CharField).And()
                        .HasNonDefault(o => o.AnotherPocoProperty.CharProperty).And()
                        .HasNull(o => o.AnotherPocoProperty.AnotherPocoProperty, "Grandchild of an item of the collection has depth 3 and expected to be null");
        }

        [TestMethod]
        public void Exclude_should_prevent_random_value_generation_for_the_excluded_members()
        {
            var entity = Builder<SutPoco>.New().Exclude(
                p => p.IntField,
                p => p.IntProperty
                ).Build();

            Assert.That.This(entity)
                .IsNotNull().And()
                .HasDefault(e => e.IntField).And()
                .HasDefault(e => e.IntProperty).And()
                .Has(e => e.LongField != 0).And()
                .Has(e => e.LongProperty != 0);
        }

        [TestMethod]
        public void Exclude_should_prevent_random_value_generation_for_the_excluded_members_on_hierarchy_objects()
        {
            var entity = Builder<SutPoco>.New().Exclude(
                p => p.IntField,
                p => p.IntProperty,
                p => p.AnotherPocoProperty.IntField,
                p => p.AnotherPocoProperty.AnotherPocoField.StringProperty
                ).Build(2);


            Assert.That.This(entity)
                .IsNotNull().And()
                // Make sure all properties excluded are using default values
                .HasDefault(e => e.IntField).And()
                .HasDefault(e => e.IntProperty).And()
                .HasDefault(e => e.AnotherPocoProperty.IntField).And()
                .HasDefault(e => e.AnotherPocoProperty.AnotherPocoField.StringProperty).And()
                // Then just make sure we are still generating random values
                .Has(e => e.LongField != 0).And()
                .Has(e => e.LongProperty != 0).And()                
                .Has(e => e.AnotherPocoProperty.LongProperty != 0).And()
                .Has(e => e.AnotherPocoProperty.StringProperty != null).And()               
                .Has(e => e.AnotherPocoProperty.AnotherPocoField.IntField != 0);            
        }

        [TestMethod]
        public void Builder_should_use_exclusion_mappings_from_Assembly_BuilderConfiguration_and_use_generic_operation()
        {
            // See MyCustomExclusions class
            var generatedType1 = Builder<Type1>.New().Build();

            Assert.That.This(generatedType1)
                .HasDefault(t => t.ID)
                .HasDefault(t => t.CreatedDateTimeUTC)
                .HasDefault(t => t.ModifiedDateTimeUTC)
                .HasNonDefault(t => t.Name)
                .HasNonDefault(t => t.Surname);

            var generatedType2 = Builder<Type2>.New().Build(1);

            Assert.That.This(generatedType2)
                .HasNonDefault(t => t.ID)
                .HasNonDefault(t => t.ChildObject)
                .HasNonDefault(t => t.Name)
                .HasNonDefault(t => t.Surname);
        }

        [TestMethod]
        public void Builder_should_use_exclusion_mappings_from_Assembly_BuilderConfiguration()
        {
            // See MyCustomExclusions class
            var generatedType1 = Builder<Type1>.New().For(Operation.Create).Build();

            Assert.That.This(generatedType1)
                .HasDefault(t => t.ID)
                .HasNonDefault(t => t.CreatedDateTimeUTC)
                .HasDefault(t => t.ModifiedDateTimeUTC)
                .HasNonDefault(t => t.Name)
                .HasNonDefault(t => t.Surname);

            var generatedType2 = Builder<Type2>.New().For(Operation.Create).Build(1);

            Assert.That.This(generatedType2)
                .HasDefault(t => t.ID)
                .HasDefault(t => t.ChildObject)
                .HasNonDefault(t => t.Name)
                .HasNonDefault(t => t.Surname);
        }

        [TestMethod]
        public void Builder_should_not_use_exclusion_mappings_from_Assembly_BuilderConfiguration_when_explicit_exclude_is_used()
        {
            // See MyCustomExclusions class
            var generatedType1 = Builder<Type1>.New().Exclude(t => t.ID).Build();

            Assert.That.This(generatedType1)
                .HasDefault(t => t.ID)
                .HasNonDefault(t => t.CreatedDateTimeUTC)
                .HasNonDefault(t => t.ModifiedDateTimeUTC)
                .HasNonDefault(t => t.Name)
                .HasNonDefault(t => t.Surname);
        }

        [TestMethod]
        public void Builder_should_use_exclusion_mappings_from_Assembly_BuilderConfiguration_ignoring_child_objects()
        {
            // See MyCustomExclusions class
            var generatedType1 = Builder<Type2>.New().For(Operation.Create).Build(1);

            Assert.That.This(generatedType1)
                .HasDefault(t => t.ID)
                .HasDefault(t => t.ChildObject)
                .HasNonDefault(t => t.Name)
                .HasNonDefault(t => t.Surname);
        }
    }
}