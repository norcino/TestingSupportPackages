# AnonymousData
[![.NET Core](https://github.com/norcino/TestingSupportPackages/actions/workflows/netcore-build-test.yml/badge.svg)](https://github.com/norcino/TestingSupportPackages/actions/workflows/netcore-build-test.yml)

## Why AnonymousData
Have you have come across a unit test where a huge object is initialized so that it could be fed to the method under test?
Have you wondered if all these property values were intended to be exactly with these values or none was actually needed?
Well I did often, and despite this could be normally prevented using good coding standards and conventions, but real life is hard.

This library through the class _Any_, aims to help you and your team write clearer tests, but it can also be used in production code when random data is needed.

One last reason to take into consideration, with it's pros and cons, is that the use of random values, introduces a flavour of fuzz testing in the unit tests.
You might encounter rare failures when the generated value goes outside of the comfort zone.
This is a good discovery as there might be a loose validation in the subject under test, or the generated ranges are not configured correctly.

## Supported types
The following data types are supported for generation:

|Type|Length Limit|Unique|Other options|
|---|:---:|:---:|---|
|Base64String|✔||Prefix, CharSet|
|Bool|||
|Byte|||
|Char|||CharSet|
|DateTime|||Range, Future|
|Decimal|✔|||
|Double|✔|||
|Email||||
|Enum||||
|Float|✔|||
|Guid||||
|Int|✔|✔|Allow Zero, Positive, Range|
|In|||Enums, IEnumerable, Params[]|
|Long|||Positive|
|Of|||Object Type|
|SByte|||
|Short||Range|
|String|✔||Prefix, CharSet|
|TimeSpan|||Range|
|Uri|||Protocol|
|Url|||Protocol|
|In||||
|NotIn||||

For all types it is possible to configure the exclusion of the default value of the given type.

## Extra features

### Exclude defaults
It is possible to statically disable the selection of default values, for example when disabled, an randomly generated integer will never be 0;
````C#
// Any.Int() will not return 0, Any.Bool() will never return false
Any.ExcludeDefaultValues = false;
````

### Unique values
When working in a test you could need to enforce that certain values are yes anonymous, but also unique.
For example when setting up a mocked repository which is returning different entities for different IDs.
In order to force the generation of unique values you can use:
````c#
var uniqueId = Any.Unique.Int();

// Execute reset at test teardown to free memory and help generation speed
Any.ResetUniqueValues();
````
The use of random generation has implications which can affect performance. When requesting for unique values, always make sure that the set from which the values needs to be generated, is big enough to get the first available value in a timely manner.

For example generating a unique integer from a range of 100000, is going to be safe and quick, but if you require 10000 unique integers, you might start experiencing delays and even failures.
The unique generation mechanism has a failsafe which aborts the generation and throws an exception if no random value was found in 10000 iterations.

### An element in a IEnumerable
Any supports the random selection of an option from a given list of options. These can be passed as an _IEnumberable_ or as a list of parameters.
````c#
// Possible results: [Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday ]
var weekDays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
var aWeekDay = Any.In(weekDays);
````

### A random value not in a list of exclusions
Any supports the generation of a random value which won't match any of the values contained in the exclusion list. These can be passed as an _IEnumberable_ or as a list of parameters.
````
// Possible results: [Sunday ]
var excludeFals = new List<bool> { false };
var alwaysTrue = Any.NotIn(exclusions);
````

### An element of a list of parameters
Similar but alternative option is to provide a list of parameters as follows:
````c#
// Possible results: [Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday ]
var aWeekDay = Any.Of("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday");
````

### Multiple values
It is possible to generate a list or array of random value using the related methods _ListOf_ and _Array_Of_.
````C#
// Get an array with a random number of int elements (between 1 and 30)
Any.ArrayOf().Int();

// Get an array of 100 randomly generated int elements
Any.ArrayOf(100).Int();

// Get a list of random number of int elements (between 1 and 30)
Any.ListOf().Int();

// Get a list of 100 randomly generated int elements
Any.ListOf(100).Int();
````

## Usage

### Base64String
Generate a Base64 string of 15 characters
````c#
// Possible results: Any Base64 string from a string with 15 characters length
var aString = Any.Base64String();

// Possible results: Any Base64 string from a string with a length of 15 UTF-16 characters
var aUTFString = Any.Base64String(charSet: CharSet.UTF16);
````
Generate a Base64 string from a string with the given length
````c#
// Possible results: Any Base64 string from a string with 6 characters length
var aString = Any.Base64String(6);
````
Generate a Base64 string, from a string with the given length and prefix
````c#
// Possible results: Any string of a length of 10 characters starting with "Name_"
var aString = Any.Base64String("Name_", 10);
````
Strings can be generated using one the following __CharSet__:
- ASCII
- UTF16
- Alphanumeric
The default is Alphanumeric

### Bool
Get a random boolean
````c#
// Possible results: [true, false]
var aBoolean = Any.Bool();
````
Setting _Any_ to exclude default values, will affect _Bool()_ making it return always _true_
````c#
Any.ExcludeDefaultValues(true);

// Possible results: [true]
var alwaysTrue = Any.Bool();
````

### Byte
Get a random _Byte_ value between 0 and 255
````c#
// Possible results: [0, 1, ... 255]
var aByte = Any.Byte();
````
Setting _Any_ to exclude default values, will affect _Byte()_ which will never return _0_
````c#
Any.ExcludeDefaultValues(true);

// Possible results: [1, ... 255]
var aByte = Any.Byte();
````

### Char
Generates a random _Char_ between 0 and 65535 in *UTF-16**
````c#
// Possible results: [0, 1, ... 65535]
var aChar = Any.Char();
````
Setting _Any_ to exclude default values, will affect _Char()_ which will never return _'0'_
````c#
Any.ExcludeDefaultValues(true);

// Possible results: [1, ... 65535]
var aChar = Any.Char();
````

### DateTime
Generate a random date time in the future, it is possible to request a date time in the past.
Get a random datetime in the future
````c#
// Possible results: [DateTime from current UTC to 31/12/9999 23:59:99]
var aDateTimeInTheFuture = Any.DateTime();
````
Get a random datetime in the past
````c#
// Possible results: [01/01/0001 00:00 to DateTime current UTC]
var expectedResult = Any.DateTime(false);
````
Get a random datetime in the future but before 2090/12/31
````c#
// Possible results: [DateTime from current UTC to 31/12/2090 00:00]
var expectedResult = Any.DateTime(new DateTime(2090,12,31));
````
Setting _Any_ to exclude default values, will affect _DateTime()_ which will never return _'1/1/1 0:00'_
````c#
Any.ExcludeDefaultValues(true);

// Possible results: All supported DateTime excluding 1st January of the year 1 at midnight
var aDateTime = Any.DateTime(false);
````

### Decimal
Get a random decimal value with by default up to 4 integer digits and 2 decimals
````c#
// Possible results: [0 ... 9999].[0 ... 99]
var aLimitedDecimal = Any.Decimal();
````
Get a decimal with maximum 2 integer digits and 4 decimal digits
````c#
// Possible results: [0 ... 99].[0 ... 9999]
var aLimitedDecimal = Any.Decimal(2,4);
````

### Double
Get a random double value with by default up to 4 integer digits and 2 decimals
````c#
// Possible results: [0 ... 9999].[0 ... 99]
var aLimitedDouble = Any.Double();
````
Get a decimal with maximum 2 integer digits and 4 decimal digits
````c#
// Possible results: [0 ... 99].[0 ... 9999]
var aLimitedDouble = Any.Double(2,4);
````

### Email
Extending the capability of _Any.String()_ it is possible to have a valid formatted email address.
````c#
// Possible results: any email address with the format {any_string}@{any_string}.any
var email = Any.Email();
````

### Enumertions
Get a random enumeration value
````c#
// Possible results: Any of the values in MyEnum
MyEnum anyValue = Any.In<MyEnum>();
````

### Float
Get a random float value with by default up to 4 integer digits and 2 decimals
````c#
// Possible results: [0 ... 9999].[0 ... 99]
var aLimitedFloat = Any.Float();
````
Get a decimal with maximum 2 integer digits and 4 decimal digits
````c#
// Possible results: [0 ... 99].[0 ... 9999]
var aLimitedFloat = Any.Float(2,4);
````

### Int
Get a random integer including zero
````c#
// Possible results: [0, 1, ... Int.MaxValue]
var aPositiveInt = Any.Int();
````
Get a random integer of maximum 4 digits, excluding zero
````c#
// Possible results: [1, 2, ... 9998, 9999]
var aNonZeroPositiveInt = Any.Int(4, false);
````
Get a random integer with sign
````c#
// Possible results: [Int.MinValue ... -1, 0, 1 ... Int.MaxValue]
var anInt = Any.Int(onlyPositive: false);
````
Get a random integer between 600 and 1200
````c#
// Possible results: [600 ... 1200]
var anInt = Any.Int(minValue: 600, maxValue: 1200);
````
Setting _Any_ to exclude default values, will affect _Int()_ so that it will never return _0_
````c#
Any.ExcludeDefaultValues(true);

// Possible results: [1, ... Int.MaxValue]
var aPositiveIntExcludingZero = Any.Int();
````

### Long
Get a random positive long
````c#
// Possible results: 0 or [Int.MaxValue ... Int.MaxValue * 2 - 1]
// Possible results: A positive long value greater than Int.MaxValue
var aPositiveLong = Any.Long();
````
Get a random negative long
````c#
// Possible results: 0 or [Int.MinValue * 2 + 1 ... Int.MinValue] or [Int.MaxValue ... Int.MaxValue * 2 - 1]
// A long value greater than Int.MaxValue or lessere than Int.MinValue
var aNegativeLong = Any.Long(false);
````
Setting _Any_ to exclude default values, will affect _Long()_ which will never return _0_
````c#
Any.ExcludeDefaultValues(true);

// Possible results: [Int.MaxValue ... Int.MaxValue * 2 - 1]
var aPositiveLong = Any.Long();
````

### Of (With default parameter less constructor)
Generate an instance of type T with random properties and fields values
````c#
// Possible results: An instance of MyType with random properties and fields
var randomObject = Any.Of<MyType>();

// Possible results: An instance of MyType with random properties and fields
var randomObject = Any.Of(typeof(MyType));
````
NOTE: for a proper Object generation, take a look at the package [*Object.Builder*](https://www.nuget.org/packages/Object.Builder).

### SByte
Generates a random signed byte with value between -128 and 127
````c#
// Possible results: [-128, ... -1, 0, 1, ... 127]
var aSByte = Any.SByte();
````
Setting _Any_ to exclude default values, will affect _SByte()_ which will never return _0_
````c#
Any.ExcludeDefaultValues(true);

// Possible results: [-128, ... -1, 1, ... 127]
var aSByte = Any.SByte();
````

### String
Generate a string of 15 characters
````c#
// Possible results: Any string of a length of 15 characters
var aString = Any.String();

// Possible results: Any string with a length of 15 UTF-16 characters
var aUTFString = Any.String(charSet: CharSet.UTF16);
````
Generate a string with the given length
````c#
// Possible results: Any string of a length of 6 characters
var aString = Any.String(6);
````
Generate a string with the given length and prefix
````c#
// Possible results: Any string of a length of 10 characters starting with "Name_"
var aString = Any.String("Name_", 10);
````
Strings can be generated using one the following __CharSet__:
- ASCII
- UTF16
- Alphanumeric
The default is Alphanumeric

### TimeSpan
Generates a random TimeSpan of a default maximum value of 10 days
````c#
// Possible results: Any timespan between 0 and 10 days
var aTimeSpan = Any.TimeSpan();
````
Generates a random TimeSpan of a maximum value of 1 hour
````c#
// Possible results: Any timespan between 0 and 1 hour
var aTimeSpan = Any.TimeSpan(0,1);
````

### Uri
Generates a random _Uri_ using as default protocol _http://_ and default TLD _.any_
````c#
// Possible results: Any URL in the form http://{any_string}.any
Uri uri = Any.Uri()
````
but is also possible to specify a custom protocol using:
````c#
// Possible results: Any URL in the form ftp://{any_string}.any
Uri uri = Any.Uri("ftp")
````

### Url
Generates a random _Url_ using as default protocol _http://_ and default TLD _.any_
````c#
// Possible results: Any URL in the form http://{any_string}.any
string url = Any.Url()
````
but is also possible to specify a custom protocol using:
````c#
// Possible results: Any URL in the form ftp://{any_string}.any
string url = Any.Url("ftp")
````

# Changelog

## Version 1.3.0 - 24/01/2023

### Bugfix
`NotIn` was not working properly and has been fixed.
Fixed `Any.Of<enum>` where zero index was never choosen.

### Breaking changes
None

### Added features
Added `Any.ListOf().` and `Any.ArrayOf().` methods to facilitate the creation of rispectively lists and arrays of random values.

## Version 1.2.0 - 18/01/2023

### Bugfix
Using `Any.Unique.Int()` was causing any subsequent request implicitly using `Any.Int()`, to also use the _Unique_ contraint, which could have lead to exceptions being thrown if the library was running out of valid random integers not previously generated.

### Breaking changes
None

### Added features
NotIn options as been added to let you generate a random value which is not contained in the list of exclusions.
The returned type is the same generic type of the list of exclusions passed to the method.
For example if you have a set of strings or integers you don't want to randomly generate, you can use this method.
