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

### No ambiguity

### Clear expectations

## Packages
### Builder
<img src="https://github.com/norcino/TestingSupportPackages/blob/master/Builder/Logo.png" alt="Builder" width="64"/>
[Builder](Builder/Readme.md) allows to create one or more instances of a class with the possibility to specify custom creation to override the random generation of data. Ideal for unit test setup, allow to create test setup where the reader has a more evident view of what data is really needed to create the test conditions.
```
var listOfHundredUsers = Builder<User>.New().BuildMany(100);
```
### AnonymousData

### Fluent Assertion for MSTest
<img src="https://github.com/norcino/TestingSupportPackages/blob/master/FluentAssertion.MSTest/Logo.png" alt="FluentAssertion for MSTest" width="64"/>
[FluentAssertion.MSTest](FluentAssertion.MSTest/Readme.md) allows to write chanined assertions fluently in one statement, including _words_ helpful to make the reading of the assertions more fluent, friendly and clear.
```
var user = sut.GetUserById(userId);
Assert.That.This(user).HasNonNull(u => u.Email).And().Has(u => u.Name == "Bob");
```
To find out more follow the link to get to the dedicated _[readme.md](FluentAssertion.MSTest/Readme.md)_.
