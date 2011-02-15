using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace Pippin.Testing
{
    public delegate void MethodThatThrows();

    public static class SpecificationExtensions
    {
        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        public static object ShouldEqual(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
            return expected;
        }

        public static object ShouldNotEqual(this object actual, object expected)
        {
            Assert.AreNotEqual(expected, actual);
            return expected;
        }

        public static void ShouldBeNull(this object anObject)
        {
            Assert.IsNull(anObject);
        }

        public static void ShouldNotBeNull(this object anObject)
        {
            Assert.IsNotNull(anObject);
        }

        public static object ShouldBeTheSameAs(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
            return expected;
        }

        public static object ShouldNotBeTheSameAs(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
            return expected;
        }

        public static void ShouldBeOfType(this object actual, Type expected)
        {
            Assert.IsInstanceOfType(actual, expected);
        }

        public static void ShouldBe(this object actual, Type expected)
        {
            Assert.IsInstanceOfType(actual, expected);
        }

        public static void ShouldNotBeOfType(this object actual, Type expected)
        {
            Assert.IsNotInstanceOfType(actual, expected);
        }

        public static void ShouldContain(this IList actual, object expected)
        {
            Assert.IsTrue(actual.Contains(expected));
        }

        public static void ShouldNotContain(this IList collection, object expected)
        {
            CollectionAssert.DoesNotContain(collection, expected);
        }

        public static IComparable ShouldBeGreaterThan(this IComparable arg1, IComparable arg2)
        {
            Assert.IsTrue(arg1.CompareTo(arg2) == 1);
            return arg2;
        }

        public static IComparable ShouldBeLessThan(this IComparable arg1, IComparable arg2)
        {
            Assert.IsTrue(arg1.CompareTo(arg2) == -1);
            return arg2;
        }

        public static void ShouldBeEmpty(this ICollection collection)
        {
            Assert.IsTrue(collection.Count == 0);
        }

        public static void ShouldBeEmpty(this string aString)
        {
            Assert.IsTrue(aString == string.Empty);
        }

        public static void ShouldNotBeEmpty(this ICollection collection)
        {
            Assert.IsFalse(collection.Count == 0);
        }

        public static void ShouldNotBeEmpty(this string aString)
        {
            Assert.IsFalse(aString == string.Empty);
        }

        public static void ShouldContain(this string fullstring, string substring)
        {
            StringAssert.Contains(fullstring, substring);
        }

        public static void ShouldStartWith(this string fullstring, string substring)
        {
            StringAssert.StartsWith(fullstring, substring);
        }

        public static void ShouldEndWith(this string fullstring, string substring)
        {
            StringAssert.EndsWith(fullstring, substring);
        }

        public static void ShouldMatchRegEx(this string value, Regex pattern)
        {
            StringAssert.Matches(value, pattern);
        }

        public static void ShouldNotMatchRegEx(this string value, Regex pattern)
        {
            StringAssert.DoesNotMatch(value, pattern);
        }

        //public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
        //{
        //    StringAssert.AreEqualIgnoringCase(expected, actual);
        //    return expected;
        //}

        //public static void ShouldNotContain(this string actual, string expected)
        //{
        //    try
        //    {
        //        StringAssert.Contains(expected, actual);
        //    }
        //    catch (AssertionException)
        //    {
        //        return;
        //    }

        //    throw new AssertionException(String.Format("\"{0}\" should not contain \"{1}\".", actual, expected));
        //}

        //public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
        //{
        //    StringAssert.AreEqualIgnoringCase(expected, actual);
        //    return expected;
        //}


        //public static void ShouldBeSurroundedWith(this string actual, string expectedStartDelimiter, string expectedEndDelimiter)
        //{
        //    StringAssert.StartsWith(expectedStartDelimiter, actual);
        //    StringAssert.EndsWith(expectedEndDelimiter, actual);
        //}

        //public static void ShouldBeSurroundedWith(this string actual, string expectedDelimiter)
        //{
        //    StringAssert.StartsWith(expectedDelimiter, actual);
        //    StringAssert.EndsWith(expectedDelimiter, actual);
        //}

        //public static void ShouldContainErrorMessage(this Exception exception, string expected)
        //{
        //    StringAssert.Contains(expected, exception.Message);
        //}

        //public static Exception ShouldBeThrownBy(this Type exceptionType, MethodThatThrows method)
        //{
        //    Exception exception = method.GetException();

        //    Assert.IsNotNull(exception);
        //    Assert.AreEqual(exceptionType, exception.GetType());

        //    return exception;
        //}

        //public static Exception GetException(this MethodThatThrows method)
        //{
        //    Exception exception = null;

        //    try
        //    {
        //        method();
        //    }
        //    catch (Exception e)
        //    {
        //        exception = e;
        //    }

        //    return exception;
        //}
    }
}