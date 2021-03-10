using BuilderExclusionMappings;
using FluentAssertion.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Builder.Tests
{
    public class Car
    {
        public int Id;
        public int OwnerId;
        public Person Owner;
        public Company Make;
        public string RegistrationNumber;
    }

    public class Person
    {
        public int Id;
        public string FirstName;
        public string LastName;
    }

    public class Company
    {
        public int Id;
        public string Name;
        public int FounderId;
        public Person Founder;
    }

    [TestClass]
    public class Examples
    {
        [TestMethod]
        public void Builder_should_allow_to_setup_referential_integrity_generation_and_depth()
        {
            var car = Builder<Car>.New().Build(c =>
            {
                c.OwnerId = c.Owner.Id;
                c.Make.FounderId = c.Make.Founder.Id;
            }, 2);

            Assert.That.This(car).Has(c => c.OwnerId == c.Owner.Id).And()
                .Has(c => c.Make.FounderId == c.Make.Founder.Id);
        }
    }
}
