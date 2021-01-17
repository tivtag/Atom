// <copyright file="CustomAssert.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.CustomAssert class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;
    using System.Collections.Generic;
    using System.Collections;

    /// <summary>
    /// Provides custom assert extensions.
    /// </summary>
    public static class CustomAssert
    {
        /// <summary>
        /// Verifies that a collection contains a given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be verified</typeparam>
        /// <param name="expected">The object expected to be in the collection</param>
        /// <param name="collection">The collection to be inspected</param>
        /// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
        public static void Contains<T>( T expected, IEnumerable<T> collection )
        {
            Contains( expected, collection, GetComparer<T>() );
        }

        /// <summary>
        /// Verifies that a collection contains a given object, using a comparer.
        /// </summary>
        /// <typeparam name="T">The type of the object to be verified</typeparam>
        /// <param name="expected">The object expected to be in the collection</param>
        /// <param name="collection">The collection to be inspected</param>
        /// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
        /// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
        public static void Contains<T>( T expected,
                                       IEnumerable<T> collection,
                                       IComparer<T> comparer )
        {
            foreach( T item in collection )
                if( comparer.Compare( expected, item ) == 0 )
                    return;

            throw new ContainsException( expected );
        }

        /// <summary>
        /// Verifies that a string contains a given sub-string, using the current culture.
        /// </summary>
        /// <param name="expectedSubString">The sub-string expected to be in the string</param>
        /// <param name="actualString">The string to be inspected</param>
        /// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
        public static void Contains( string expectedSubString,
                                    string actualString )
        {
            Contains( expectedSubString, actualString, StringComparison.CurrentCulture );
        }

        /// <summary>
        /// Verifies that a string contains a given sub-string, using the given comparison type.
        /// </summary>
        /// <param name="expectedSubString">The sub-string expected to be in the string</param>
        /// <param name="actualString">The string to be inspected</param>
        /// <param name="comparisonType">The type of string comparison to perform</param>
        /// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
        public static void Contains( string expectedSubString,
                                     string actualString,
                                     StringComparison comparisonType )
        {
            if( actualString.IndexOf( expectedSubString, comparisonType ) < 0 )
                throw new ContainsException( expectedSubString );
        }

        /// <summary>
        /// Verifies that a collection does not contain a given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be compared</typeparam>
        /// <param name="expected">The object that is expected not to be in the collection</param>
        /// <param name="collection">The collection to be inspected</param>
        /// <exception cref="DoesNotContainException">Thrown when the object is present inside the container</exception>
        public static void DoesNotContain<T>( T expected,
                                             IEnumerable<T> collection )
        {
            DoesNotContain( expected, collection, GetComparer<T>() );
        }

        /// <summary>
        /// Verifies that a collection does not contain a given object, using a comparer.
        /// </summary>
        /// <typeparam name="T">The type of the object to be compared</typeparam>
        /// <param name="expected">The object that is expected not to be in the collection</param>
        /// <param name="collection">The collection to be inspected</param>
        /// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
        /// <exception cref="DoesNotContainException">Thrown when the object is present inside the container</exception>
        public static void DoesNotContain<T>( T expected,
                                             IEnumerable<T> collection,
                                             IComparer<T> comparer )
        {
            foreach( T item in collection )
                if( comparer.Compare( expected, item ) == 0 )
                    throw new DoesNotContainException( expected );
        }

        /// <summary>
        /// Verifies that a string does not contain a given sub-string, using the current culture.
        /// </summary>
        /// <param name="expectedSubString">The sub-string which is expected not to be in the string</param>
        /// <param name="actualString">The string to be inspected</param>
        /// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
        public static void DoesNotContain( string expectedSubString,
                                          string actualString )
        {
            DoesNotContain( expectedSubString, actualString, StringComparison.CurrentCulture );
        }

        /// <summary>
        /// Verifies that a string does not contain a given sub-string, using the current culture.
        /// </summary>
        /// <param name="expectedSubString">The sub-string which is expected not to be in the string</param>
        /// <param name="actualString">The string to be inspected</param>
        /// <param name="comparisonType">The type of string comparison to perform</param>
        /// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the given string</exception>
        public static void DoesNotContain( string expectedSubString,
                                          string actualString,
                                          StringComparison comparisonType )
        {
            if( actualString.IndexOf( expectedSubString, comparisonType ) >= 0 )
                throw new DoesNotContainException( expectedSubString );
        }

        /// <summary>
        /// Verifies that a block of code does not throw any exceptions.
        /// </summary>
        /// <param name="testCode">A delegate to the code to be tested</param>
        public static void DoesNotThrow( Action testCode )
        {
            Exception ex = RecordException( testCode );

            if( ex != null )
                throw new DoesNotThrowException( ex );
        }
        
        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type).
        /// </summary>
        /// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
        /// <param name="testCode">A delegate to the code to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown</exception>
        public static T Throws<T>( Action testCode )
            where T : Exception
        {
            return (T)Throws( typeof( T ), testCode );
        }

        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type).
        /// Generally used to test property accessors.
        /// </summary>
        /// <exception cref="ThrowsException">
        /// Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.
        /// </exception>
        /// <param name="exceptionType">The type of the exception expected to be thrown</param>
        /// <param name="testCode">A delegate to the code to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        public static Exception Throws( Type exceptionType, Action testCode )
        {
            Exception exception = RecordException( testCode );

            if( exception == null )
                throw new ThrowsException( exceptionType );

            if( !exceptionType.Equals( exception.GetType() ) )
                throw new ThrowsException( exceptionType, exception );

            return exception;
        }

        /// <summary>
        /// Verifies that any exception is thrown.
        /// </summary>
        /// <exception cref="ThrowsException">
        /// Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.
        /// </exception>
        /// <param name="testCode">A delegate to the code to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        public static Exception Throws( Action testCode )
        {
            Exception exception = RecordException( testCode );

            if( exception == null )
                throw new ThrowsException();

            return exception;
        }

        /// <summary>
        /// Records any exception which is thrown by the given code.
        /// </summary>
        /// <param name="code">The code which may thrown an exception.</param>
        /// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
        private static Exception RecordException(Action code)
        {
            try
            {
                code();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private static IComparer<T> GetComparer<T>()
        {
            return AssertComparer<T>.Instance;
        }

        private sealed class AssertComparer<T> : IComparer<T>
        {
            public static AssertComparer<T> Instance = new AssertComparer<T>();

            public int Compare( T x,
                               T y )
            {
                Type type = typeof( T );

                // Null?
                if( !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom( typeof( Nullable<> ) )) )
                {
                    if( Equals( x, default( T ) ) )
                    {
                        if( Equals( y, default( T ) ) )
                            return 0;
                        return -1;
                    }

                    if( Equals( y, default( T ) ) )
                        return -1;
                }

                // Same type?
                if( x.GetType() != y.GetType() )
                    return -1;

                // Implements IComparable<T>?
                IComparable<T> comparable1 = x as IComparable<T>;

                if( comparable1 != null )
                    return comparable1.CompareTo( y );

                // Implements IComparable?
                IComparable comparable2 = x as IComparable;

                if( comparable2 != null )
                    return comparable2.CompareTo( y );

                // Implements IEquatable<T>?
                IEquatable<T> equatable = x as IEquatable<T>;

                if( equatable != null )
                    return equatable.Equals( y ) ? 0 : -1;

                // Enumerable?
                IEnumerable enumerableX = x as IEnumerable;
                IEnumerable enumerableY = y as IEnumerable;

                if( enumerableX != null && enumerableY != null )
                {
                    IEnumerator enumeratorX = enumerableX.GetEnumerator();
                    IEnumerator enumeratorY = enumerableY.GetEnumerator();

                    while( true )
                    {
                        bool hasNextX = enumeratorX.MoveNext();
                        bool hasNextY = enumeratorY.MoveNext();

                        if( !hasNextX || !hasNextY )
                            return (hasNextX == hasNextY ? 0 : -1);

                        if( !Equals( enumeratorX.Current, enumeratorY.Current ) )
                            return -1;
                    }
                }

                // Last case, rely on Object.Equals
                return Equals( x, y ) ? 0 : -1;
            }
        }
    }
}
