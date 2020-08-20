using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertion.MSTest
{
    /// <summary>
    /// Add fluent assertion extensions to MSTest Assert.That for HttpResponseMessage
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        #region HttpResponse Status Codes
        public static void IsOk(this Assert assert, HttpResponseMessage response)
        {
            Assert.IsNotNull(response);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new AssertFailedException($"Expected Ok (200) status code, but was {response.StatusCode} ({(int)response.StatusCode})");
        }

        public static void IsCreated(this Assert assert, HttpResponseMessage response)
        {
            Assert.IsNotNull(response);
            if (response.StatusCode != HttpStatusCode.Created)
                throw new AssertFailedException($"Expected Created (201) status code, but was {response.StatusCode} ({(int)response.StatusCode})");
        }

        public static void IsNotFound(this Assert assert, HttpResponseMessage response)
        {
            Assert.IsNotNull(response);
            if (response.StatusCode != HttpStatusCode.NotFound)
                throw new AssertFailedException($"Expected NotFound (404) status code, but was {response.StatusCode} ({(int)response.StatusCode})");
        }

        public static void IsBadRequest(this Assert assert, HttpResponseMessage response)
        {
            Assert.IsNotNull(response);
            if (response.StatusCode != HttpStatusCode.BadRequest)
                throw new AssertFailedException($"Expected BadRequest (400) status code, but was {response.StatusCode} ({(int)response.StatusCode})");
        }

        public static void HasStatusCode
            (this Assert assert, HttpResponseMessage response, HttpStatusCode statuscode)
        {
            Assert.IsNotNull(response);
            if (response.StatusCode != statuscode)
                throw new AssertFailedException($"Expected {statuscode} ({(int)statuscode}) status code, but was {response.StatusCode} ({(int)response.StatusCode})");
        }
        #endregion
    }
}