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

### Use Exclusions to prevent properties from being populated with random data