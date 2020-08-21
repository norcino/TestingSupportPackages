# Testing Support Packages
Testing support packages is a repository with the source code of multiple Nuget Packages designed to support testing, from building anonymous data and entities to the usage of fluent assertion with MSTest.

# Introduction
It is more than 20 years that I code in a way or another, more than 14 years as professional and the more I meet new developers and the more I see how much still people struggle with testing (and not just that). I am a strong beliver in testing, a well written, clear and concise test is the most valuable piece of code you can get out of your git repository.

During my carreer I struggled to understand what people was trying to test, from meaningless broad nonsese titles, to spreaded cryptic initializazion of magic numbers and strings, to than arrive to a wide variety of assertions. Of course there are things only, experience, sobriety, common sense and professionalism can fix, but with the libraries I created I tried to help myself and my fellow colleagues to improve and get guided while writing tests.

## Principles
### Small concise tests
When writing a test the developer should keep in mind, if not even focus, on the clarity of the test and make sure the attention of the reader is going to ge in order on _"Setup" "Execution"_ and _"Assertion"_. This is clear and basic but if you start reading a test with 150 lines of code of which more than 100 are only there to setup the initial data and pre conditions, well, the attenntion of the reader will most likely be _"SETUP" "SETUP" "SETUP" "execution" "assertion"_.

Helper methods, refactoring and other tecniques can help the coder shrink the setup section but what I found very conveninet, whenever appliable, was the ability to delegate a data factory, or better a data builder to say, look, I need instances of this Type, a hundred of them, I don't care about the content, and boom you get it, a one clean simple, understandable line.
Most likely altough, the test is focusend on specific data conditions that can be required, so out of 10 properties of a class maybe we are interested in one or two, so what if the builder could take care of all but the specific properties we want? Again one two lines highlighting that we want certain properties to have a certain value and from there you go.

To help have smaller concise setup steps, **[Builder](#Builder)** is the package you might be interested in.

### DRY
Everyone seems to be fully aware that repatition is bad, but especially in tests I often find repetitive code to setup entities needed for the testing, it is frustrating and even the simple extraction of an helper method is often ignored.
With builder, you only need to invoke the build method and specify the specific data you want only for those properties relevant for the test.

### No ambiguity
In tests, assertions over strings and numbers are often reason of confusion, do I really have to expect that ID, is that permission what I really should expect, is that magic number meaningful? To help, **[AnonymousData](#AnonymousData)** allow you to populate variables with values which are hidden from start, with a clear readable statement.

### Clear expectations
When using testing framework I like that *Assert*, being a reference to a stati class, is always styled with a different colour, that at least allows you to pick where you are asserting, useful because I happened to find Assertions spreaded in the test method and this doesn't help to understand what is expected in the test.
Worse is when using libraries like FluentAssertion, you have to repeat yourself getting one by one the properties you need to test from the result object, spreading the verifications in multiple lines which are not even highlighet like with the *Assert*.
For these reasons and more, I decided to create **[Fluent Assertion for MSTest](#Fluent-Assertion-For-MSTest)**, this should help you create clear, fluent and concise expectetions.


## Packages
### Builder
<img src="https://github.com/norcino/TestingSupportPackages/blob/master/Builder/Logo.png" alt="Builder" width="64"/>

[Builder](Builder/Readme.md) allows to create one or more instances of a class with the possibility to specify custom creation to override the random generation of data. Ideal for unit test setup, allow to create test setup where the reader has a more evident view of what data is really needed to create the test conditions.
````
var listOfHundredUsers = Builder<User>.New().BuildMany(100);
````

### AnonymousData

### Fluent Assertion for MSTest
<img src="https://github.com/norcino/TestingSupportPackages/blob/master/FluentAssertion.MSTest/Logo.png" alt="FluentAssertion for MSTest" width="64"/>

[FluentAssertion.MSTest](FluentAssertion.MSTest/Readme.md) allows to write chanined assertions fluently in one statement, including _words_ helpful to make the reading of the assertions more fluent, friendly and clear.
````
var user = sut.GetUserById(userId);
Assert.That.This(user).HasNonNull(u => u.Email).And().Has(u => u.Name == "Bob");
````
To find out more follow the link to get to the dedicated _[readme.md](FluentAssertion.MSTest/Readme.md)_.
