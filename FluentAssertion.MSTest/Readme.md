# Fluent Assertion for MSTest

## MSTest (Microsoft.VisualStudio.TestTools.UnitTesting)
MSTest is the Microsoft Unit Testing library which does not requires third party frameworks.
It has a decent amount of features and is quite mature but for for people like me that prefers a fluent approach in defining the expectations, it does not really offer anything.

## Why fluent assertions
I been using for years FluentAssertions, I love it and I think is the best library out there which allows you to fluently write test assertions.

The benefit of using fluency is that the reader is facilitated in understanding what is actually being asserted, tested.

## Why another fluent assertion then?
An important thing I don't like about FluentAssertion is that you loose the highlight of where an assertion has been placed.


When you write:
```
var result = sut.Do();
result.Should().Be(ExpectedResult);
```
you don't have any visual indication that you are doing an assert, the style of your code shows plain colour and nothing really catches your eyes.

If you use Assert:
```
var result = sut.Do();
Assert.AreEqual(ExpectedResult, result);
```
the Assert class is highlighted and in complex tests (which you should not have, but we all do have), this simple highlight is helping you find all assertions.

That's why I decided to invest some of my time to leverage ```Assert.That``` property to extend MSTest adding flunt assertion extension methods, it is both fun and a good coding excercise.

## What can be tested

### Objects
### Collections
### Strings
### DateTime
### Actions