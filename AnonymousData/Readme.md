# AnonymousData

## Why AnonymousData
Often reading tests, I need to think weather a variable used for testing, is the expected input value or it is just a value.
This could be normally prevented using good coding standards and conventions, but real life is hard.

This library through the class _Any_, aims to help you and your team write clearer tests, but it can also be used in production code when rando data is needed.

## Supported types
The following types are supported for generation:

|Type|Lenght Limit|Other options|
|---|:---:|---|
|Bool||
|Byte||
|Char||UTF|
|DateTime|✔|Future|
|Decimal|✔||
|Double|✔||
|Enum|||
|Float|✔||
|Int|✔|Allow Zero, Positive, Range|
|Long||Positive|
|Object||
|SByte||
|Short||
|String|✔|Prefix, UTF|
|TimeSpan|✔||

For all types it is possible to configure the exclusion of the default value of the given type.

## Usage

### Bool
Get a random boolean
````
// Possible results: [true, false]
var aBoolean = Any.Bool();
````
Setting _Any_ to exclude default values, will affect _Bool()_ making it return always _true_
````
Any.ExcludeDefaultValues(true);

// Possible results: [true]
var alwaysTrue = Any.Bool();
````

### Byte
Get a random _Byte_ value between 0 and 255
````
// Possible results: [0, 1, ... 255]
var aByte = Any.Byte();
````
Setting _Any_ to exclude default values, will affect _Byte()_ which will never return _0_
````
Any.ExcludeDefaultValues(true);

// Possible results: [1, ... 255]
var aByte = Any.Byte();
````

### Char
Generates a random _Char_ between 0 and 65535 in *UTF-16**
````
// Possible results: [0, 1, ... 65535]
var aChar = Any.Char();
````
Setting _Any_ to exclude default values, will affect _Char()_ which will never return _'0'_
````
Any.ExcludeDefaultValues(true);

// Possible results: [1, ... 65535]
var aChar = Any.Char();
````

### DateTime
Generate a random date time in the future, it is possible to request a date time in the past.
Get a random datetime in the future
````
// Possible results: [DateTime from current UTC to 31/12/9999 23:59:99]
var aDateTimeInTheFuture = Any.DateTime();
````
Get a random datetime in the past
````
// Possible results: [01/01/0001 00:00 to DateTime current UTC]
var expectedResult = Any.DateTime(false);
````
Get a random datetime in the future but before 2090/12/31
````
// Possible results: [DateTime from current UTC to 31/12/2090 00:00]
var expectedResult = Any.DateTime(new DateTime(2090,12,31));
````
Setting _Any_ to exclude default values, will affect _DateTime()_ which will never return _'1/1/1 0:00'_
````
Any.ExcludeDefaultValues(true);

// Possible results: All supported DateTime excluding 1st January of the year 1 at midnight
var aDateTime = Any.DateTime(false);
````

### Decimal
Get a dandom decimal value with by default up to 4 integer digits and 2 decimals
````
// Possible results: [0 ... 9999].[0 ... 99]
var aLimitedDecimal = Any.Decimal();
````
Get a decimal with maximum 2 integer digits and 4 decimal digits
````
// Possible results: [0 ... 99].[0 ... 9999]
var aLimitedDecimal = Any.Decimal(2,4);
````

### Double
Get a random double value with by default up to 4 integer digits and 2 decimals
````
// Possible results: [0 ... 9999].[0 ... 99]
var aLimitedDouble = Any.Double();
````
Get a decimal with maximum 2 integer digits and 4 decimal digits
````
// Possible results: [0 ... 99].[0 ... 9999]
var aLimitedDouble = Any.Double(2,4);
````

### Float
Get a random float value with by default up to 4 integer digits and 2 decimals
````
// Possible results: [0 ... 9999].[0 ... 99]
var aLimitedFloat = Any.Float();
````
Get a decimal with maximum 2 integer digits and 4 decimal digits
````
// Possible results: [0 ... 99].[0 ... 9999]
var aLimitedFloat = Any.Float(2,4);
````

### Int
Get a random integer including zero
````
// Possible results: [0, 1, ... Int.MaxValue]
var aPositiveInt = Any.Int();
````
Get a random integer of maximum 4 digits, excluding zero
````
// Possible results: [1, 2, ... 9998, 9999]
var aNonZeroPositiveInt = Any.Int(4, false);
````
Get a random integer with sign
````
// Possible results: [Int.MinValue ... -1, 0, 1 ... Int.MaxValue]
var anInt = Any.Int(onlyPositive: false);
````
Get a random integer between 600 and 1200
````
// Possible results: [600 ... 1200]
var anInt = Any.Int(minValue: 600, maxValue: 1200);
````
Setting _Any_ to exclude default values, will affect _Int()_ so that it will never return _0_
````
Any.ExcludeDefaultValues(true);

// Possible results: [1, ... Int.MaxValue]
var aPositiveIntExcludingZero = Any.Int();
````

### Long
Get a random positive long
````
// Possible results: 0 or [Int.MaxValue ... Int.MaxValue * 2 - 1]
// Possible results: A positive long value greater than Int.MaxValue
var aPositiveLong = Any.Long();
````
Get a random negative long
````
// Possible results: 0 or [Int.MinValue * 2 + 1 ... Int.MinValue] or [Int.MaxValue ... Int.MaxValue * 2 - 1]
// A long value greater than Int.MaxValue or lessere than Int.MinValue
var aNegativeLong = Any.Long(false);
````
Setting _Any_ to exclude default values, will affect _Long()_ which will never return _0_
````
Any.ExcludeDefaultValues(true);

// Possible results: [Int.MaxValue ... Int.MaxValue * 2 - 1]
var aPositiveLong = Any.Long();
````

### SByte
Generates a random signed byte with value between -128 and 127
````
// Possible results: [-128, ... -1, 0, 1, ... 127]
var aSByte = Any.SByte();
````
Setting _Any_ to exclude default values, will affect _SByte()_ which will never return _0_
````
Any.ExcludeDefaultValues(true);

// Possible results: [-128, ... -1, 1, ... 127]
var aSByte = Any.SByte();
````

### String
Generate a string of 15 characters
````
// Possible results: Any string of a lenght of 15 characters
var aString = Any.String();

// Possible results: Any string with a lenght of 15 UTF-16 characters
var aUTFString = Any.String(utf: true);
````
Generate a string with the given lenght
````
// Possible results: Any string of a lenght of 6 characters
var aString = Any.String(6);
````
Generate a string with the given lenght and prefix
````
// Possible results: Any string of a lenght of 10 characters starting with "Name_"
var aString = Any.String("Name_", 10);
````

### TimeSpan
Generates a random TimeSpan of a default maximum value of 10 days
````
// Possible results: Any timespan between 0 and 10 days
var aTimeSpan = Any.TimeSpan();
````
Generates a random TimeSpan of a maximum value of 1 hour
````
// Possible results: Any timespan between 0 and 1 hour
var aTimeSpan = Any.TimeSpan(0,1);
````

### Enumertions
Get a random enumeration value
````
// Possible results: Any of the values in MyEnum
MyEnum anyValue = Any.In<MyEnum>();
````

### Objects (With default parameterless constructor)
Generate an instance of type T with random properties and fields values
````
// Possible results: An instance of MyType with random properties and fields
var randomObject = Any.Of<MyType>();
````
NOTE: for a proper Object generation, take a look at the package "*Object.Builder*".