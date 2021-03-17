# Builder

## Introduction
The setup of test data, for unit, integration and end to end, is normally quite verbose and makes the tests bigger and distracting.
Builder is here to help quickly and concisely create objects, populating fields and properties, allowing to specify only relevant properties for example, or create a complex object structure, or again create many instances using creation blueprints.

## Features
Builder allows to:
 - Create an instance of an object by specifying the type
 - Create multiple instances of an object by specifying the type
 - Create an instance of an object by specifying the type and creation customization
 - Create multiple instances of an object by specifying the type and creation customization
 - Create one or more objects without generating random values
 - Create one or more objects specifying the list of properties to be excluded from them random value generation

## Usage

### Single entity creation
Single entity creation with properties and fields populated with random data. Object and collections will remain null.
````
T myEntity = Builder<T>.New().Build();
````

### Single entity creation with children object
Single entity creation with properties and fields populated with random data, the integer parameter represents the nesting level to be reached to apply the generation also to objects and collections.
The below example will create a root T instance with all properties and fields generate and also down to 2 level of childer and granchildren objects and collections.
````
T myEntity = Builder<T>.New().Build(2);
````

### Single entity creation with custom generation
Single entity creation with properties and fields populated with random data. Object and collections will remain null.
The action parameter is used to customise the initialization, for examle setting manually the desired value of a property or child object.
This is useful to highlight which values are actually meaningful for the test and which is their value.
````
T myEntity = Builder<T>.New().Build(e => {
                e.DateTimeProperty = DateTime.Now;
                e.StringProperty = "My preferred value";
            });
````
optionally is possible to also ask the builder to only use the setup action, passing the value `false` for the optional parameter `useRandomValues`.
This way all members will be left uninitialized, therefore they will have the default value.
````
T myEntity = Builder<T>.New().Build(e => {
                e.DateTimeProperty = DateTime.Now;
                e.StringProperty = "My preferred value";
                }, false);
````

### Multiple entities creation
Following the same behaviour of the single entity creation, the `BuildMany` method allows to specify the number of entities to be generated.
````
IEnumerable<T> myEntity = Builder<T>.New().BuildMany(10);
````

### Multiple entities creation with custom generation
Exactly like it happens with the `Build` method which accepts an action to customise the object creation, this method allows to specify how each object is created.
The action this time takes as parameter the object to be created and an integer value which corresponds to the number of the current created object.
This index value can be used to customize property values to facilitate their identification within the list but also to add more complex generation logic.
````
IEnumerable<T> myEntity = Builder<T>.New().BuildMany(10, (e,i) =>
            {
                e.IntProperty = i;
                e.StringProperty = $"My preferred value #{i}";
            });
````
optionally is possible to also ask the builder to only use the setup action, passing the value `false` for the optional parameter `useRandomValues`.
This way all members will be left uninitialized, therefore they will have the default value.
````
IEnumerable<T> myEntity = Builder<T>.New().BuildMany(10, (e,i) =>
            {
                e.IntProperty = i;
                e.StringProperty = $"My preferred value #{i}";
            }, false);
````

### Use hierarchy parameter to generate also children Reference Types and IEnumerable
If the object created has members with reference types (objects) or enumerations, these by default will not be populate with random data.
To change this behaviour is it possible to use the custom action to create a child object using another instance of a Builder, or it is possible to pass the _hierarchy_ parameter value, which represents the depth which the builder will reach to generate children objects.

By default enumerations will be populated with 5 items.
````
T myEntity = Builder<T>.New().Build(2);
````
For example given the types below:
````
public class Car {
    public int Id;
    public int OwnerId;
    public Person Owner;
    public Company Make;
    public string RegistrationNumber;
}

public class Person {
    public int Id;
    public string FirstName;
    public string LastName;
}

public class Company {
    public int Id;
    public string Name;
    public int FounderId;
    public Person Founder;
}
````

If a Builder is used to create a Car with depth 2, it will generate a Car, an Owner, a Make and the Founder of the Company.

````
var car = Builder<Car>.New().Build(2);
````

Where car will look like:

|a|b|
|-|-|
|car.Id | Any Interger
|car.OwnerId | Any Integer
|car.Owner | An instance of Person
|car.Owner.Id | Any int
|car.Owner.FirstName | Any string
|car.Owner.LastName | Any string
|car.Make | An instance of Company
|car.Make.Id | Any int
|car.Make.Name | Any string
|car.Make.FounderId | Any int
|car.Make.Founder | An instance of Person
|car.Make.Founder.Id | Any int
|car.Make.Founder.FirstName | Any string
|car.Make.Founder.LastName | Any string
|car.RegistrationNumber | Any string

To further improve the data consistency, for example satisfying the referential integrity, it is possible to use the construction action like shown below.
````
var car = Builder<Car>.New().Build(c =>
{
    c.OwnerId = c.Owner.Id;
    c.Make.FounderId = c.Make.Founder.Id;
}, 2);
````
Because the action to customize the generation is executed after the random generation, it is possible to make references consistent.


### Use Exclusions to prevent properties from being populated with random data
Particularly handy when creating an object which will be saved to a database where the Identity will be generated automatically, it is possible to use the _Exclude_ method to provide one or more exclusions.
````
T myEntity = Builder<T>.New().Exclude(
                e => e.Id,
                e => e.ReferenceId
                ).Build();

Car myEntity = Builder<Car>.New().Exclude(
                c => c.Id,
                c => c.OwnerId,
                c => c.Owner.Id
                ).Build(1);
````

### Set default characters set to be used for strings and characters generation
The strings and characters are now generated by default using Alphanumeric characters. It is possible to change this default behaviour using different sets.
````
Builder.DefaultStringCharSet = CharSet.Alphanumeric;
````

## Builder static configuration using properties
### NumberOfNestedEntitiesInCollections
__NumberOfNestedEntitiesInCollections__ is the default number of entities created for each enumeration populated when the hieratchy is set to a value greater than 1;

### InitializeEmptyCollectionsInsteadOfNull
__InitializeEmptyCollectionsInsteadOfNull__ force the creation of empty collections instead of null when the hierarchy is left by default to zero.

## Builder configuration
It is possible to configure the builder so that the exclusions can be automatically set by default based on the built type and the desired __Operation__.

An __Operation__ can be specified using ```Builder<T>.new().For(Operation.Persistence)```, the available operations are:
- Default (Set by default when not specified)
- Create
- Update
- Delete
- Persistence

In order to configure the Builder, it is necessary to use create an assembly called **BuilderConfiguration** and place it in test project bin folder.
When the builder is created using the __New()__ method, it will search for the assembly and try to load the class implementing the interface __IBuilderExclusionMapping__.

The class should contain the mappings desired for the tested application as shown below in this example:

````
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
````