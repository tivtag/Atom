// <copyright file="EnumerableExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.EnumerableExtensions class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Defines extension methods for the IEnumerable{T} interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Concatenates two sequences.
        /// </summary>
        /// <typeparam name="TDestination">
        /// The type of the elements in the destination sequence.
        /// </typeparam>
        /// <typeparam name="TFirst">
        /// The type of the elements in the first sequence.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// The type of the elements in the second sequence.
        /// </typeparam>
        /// <param name="first">
        /// The first sequence to concatenate.
        /// </param>
        /// <param name="second">
        /// The sequence to concatenate to the first sequence.
        /// </param>
        /// <returns>
        /// An System.Collections.Generic.IEnumerable{TDestination} that contains the
        /// concatenated elements of the two input sequences.
        /// </returns>
        public static IEnumerable<TDestination> Concat<TDestination, TFirst, TSecond>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second )
        {
            Contract.Requires<ArgumentNullException>( first != null );
            Contract.Requires<ArgumentNullException>( second != null );
            Contract.Ensures( Contract.Result<IEnumerable<TDestination>>() != null );
            Contract.Ensures( Contract.Result<IEnumerable<TDestination>>().Count() == (first.Count() + second.Count()) );

            var result = new List<TDestination>();

            foreach( var item in first.Cast<TDestination>() )
            {
                result.Add( item );
            }

            foreach( var item in second.Cast<TDestination>() )
            {
                result.Add( item );
            }

            return result;
        }

        /// <summary>
        /// Concatenates two sequences.
        /// </summary>
        /// <typeparam name="TDestination">
        /// The type of the elements in the destination sequence.
        /// </typeparam>
        /// <typeparam name="TFirst">
        /// The type of the elements in the first sequence.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// The type of the elements in the second sequence.
        /// </typeparam>
        /// <param name="first">
        /// The first sequence to concatenate.
        /// </param>
        /// <param name="second">
        /// The sequence to concatenate to the first sequence.
        /// </param>
        /// <param name="firstConverter">
        /// The converter that should be used to convert items of type TFirst to TDestination.
        /// </param>
        /// <param name="secondConverter">
        /// The converter that should be used to convert items of type TSecond to TDestination.
        /// </param>
        /// <returns>
        /// An System.Collections.Generic.IEnumerable{TDestination} that contains the
        /// concatenated elements of the two input sequences.
        /// </returns>
        public static IEnumerable<TDestination> Concat<TDestination, TFirst, TSecond>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Converter<TFirst, TDestination> firstConverter,
            Converter<TSecond, TDestination> secondConverter )
        {
            Contract.Requires<ArgumentNullException>( first != null );
            Contract.Requires<ArgumentNullException>( second != null );
            Contract.Requires<ArgumentNullException>( firstConverter != null );
            Contract.Requires<ArgumentNullException>( secondConverter != null );
            Contract.Ensures( Contract.Result<IEnumerable<TDestination>>() != null );
            Contract.Ensures( Contract.Result<IEnumerable<TDestination>>().Count() == (first.Count() + second.Count()) );

            var result = new List<TDestination>();

            foreach( var item in first )
            {
                result.Add( firstConverter( item ) );
            }

            foreach( var item in second )
            {
                result.Add( secondConverter( item ) );
            }

            return result;
        }

        /// <summary>
        /// Conveters the elements of the specified sequence into another sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements in the resulting sequence.
        /// </typeparam>
        /// <param name="sequence">
        /// The input sequence.
        /// </param>
        /// <param name="converter">
        /// The conversion method used to convert every single element.
        /// </param>
        /// <returns>
        /// A new sequence containing the converted elements of the source sequence.
        /// </returns>
        public static IEnumerable<TResult> Cast<TSource, TResult>(
            this IEnumerable<TSource> sequence,
            Converter<TSource, TResult> converter )
        {
            Contract.Requires<ArgumentNullException>( sequence != null );
            Contract.Requires<ArgumentNullException>( converter != null );
            Contract.Ensures( Contract.Result<IEnumerable<TResult>>() != null );
            Contract.Ensures( Contract.Result<IEnumerable<TResult>>().Count() == sequence.Count() );

            List<TResult> result = CreateFittingList<TSource, TResult>( sequence );

            foreach( var item in sequence )
            {
                result.Add( converter( item ) );
            }

            return result;
        }

        /// <summary>
        /// Creates a new List{T} that can hold all elements of the given sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements in the resulting list.
        /// </typeparam>
        /// <param name="sequence">
        /// The input sequence.
        /// </param>
        /// <returns>
        /// A new, empty, List{TResult}.
        /// </returns>
        private static List<TResult> CreateFittingList<TSource, TResult>( IEnumerable<TSource> sequence )
        {
            var sourceList = sequence as ICollection<TSource>;

            if( sourceList != null )
            {
                return new List<TResult>( sourceList.Count );
            }
            else
            {
                return new List<TResult>();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified sequence contains
        /// only distinct elements using the <see cref="EqualityComparer&lt;T&gt;.Default"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements.
        /// </typeparam>
        /// <param name="sequence">
        /// The input sequence of elements.
        /// </param>
        /// <returns>
        /// true if all values in the specified sequence are different;
        /// otherwise false.
        /// </returns>
        public static bool HasDistinctElements<T>( this IEnumerable<T> sequence )
        {
            return HasDistinctElements( sequence, EqualityComparer<T>.Default );
        }

        /// <summary>
        /// Gets a value indicating whether the specified sequence contains
        /// only distinct elements.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements.
        /// </typeparam>
        /// <param name="sequence">
        /// The input sequence of elements.
        /// </param>
        /// <param name="comparer">
        /// The IEqualityComparer{T} that should be used.
        /// </param>
        /// <returns>
        /// true if all values in the specified sequence are different;
        /// otherwise false.
        /// </returns>
        public static bool HasDistinctElements<T>( this IEnumerable<T> sequence, IEqualityComparer<T> comparer )
        {
            Contract.Requires<ArgumentNullException>( sequence != null );
            Contract.Requires<ArgumentNullException>( comparer != null );

            int index = 1;

            foreach( var element in sequence )
            {
                var otherSequence = sequence.Skip( index );

                foreach( var otherElement in otherSequence )
                {
                    if( comparer.Equals( element, otherElement ) )
                    {
                        return false;
                    }
                }

                ++index;
            }

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the specified sequences are equal; 
        /// using the default EqualityComparer{T}.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements in the sequences.
        /// </typeparam>
        /// <param name="sequence">
        /// The first sequence.
        /// </param>
        /// <param name="otherSequence">
        /// The second sequence.
        /// </param>
        /// <returns>
        /// true if the elements are equal;
        /// otherwise false.
        /// </returns>
        public static bool ElementsEqual<T>( this IEnumerable<T> sequence, IEnumerable<T> otherSequence )
        {
            Contract.Requires<ArgumentNullException>( sequence != null );
            Contract.Requires<ArgumentNullException>( otherSequence != null );

            return ElementsEqual<T>( sequence, otherSequence, EqualityComparer<T>.Default );
        }

        /// <summary>
        /// Gets a value indicating whether the specified sequences are equal; 
        /// using the specified IEqualityComparer{T}.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements in the sequences.
        /// </typeparam>
        /// <param name="sequence">
        /// The first sequence.
        /// </param>
        /// <param name="otherSequence">
        /// The second sequence.
        /// </param>
        /// <param name="comparer">
        /// The comparer to use.
        /// </param>
        /// <returns>
        /// true if the elements are equal;
        /// otherwise false.
        /// </returns>
        public static bool ElementsEqual<T>(
            this IEnumerable<T> sequence,
            IEnumerable<T> otherSequence,
            IEqualityComparer<T> comparer )
        {
            Contract.Requires<ArgumentNullException>( sequence != null );
            Contract.Requires<ArgumentNullException>( otherSequence != null );
            Contract.Requires<ArgumentNullException>( comparer != null );

            var enumerator = sequence.GetEnumerator();
            var otherEnumerator = otherSequence.GetEnumerator();

            while( enumerator.MoveNext() )
            {
                if( !otherEnumerator.MoveNext() )
                {
                    return false;
                }

                T current = enumerator.Current;
                T otherCurrent = otherEnumerator.Current;

                if( !comparer.Equals( current, otherCurrent ) )
                {
                    return false;
                }
            }

            if( otherEnumerator.MoveNext() )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether a sequence contains all elements of another sequence
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the sequence.
        /// </typeparam>
        /// <param name="source">
        /// A sequence in which locate the values.
        /// </param>
        /// <param name="elements">
        /// A sequence of values to locate.
        /// </param>
        /// <returns>
        /// true if the specified source sequence contains all elements of the specified sequence;
        /// otherwise false.
        /// </returns>
        public static bool Contains<T>( this IEnumerable<T> source, IEnumerable<T> elements )
        {
            Contract.Requires<ArgumentNullException>( source != null );
            Contract.Requires<ArgumentNullException>( elements != null );

            foreach( var element in elements )
            {
                if( !source.Contains( element ) )
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether a sequence contains any of the elements of another sequence
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the sequence.
        /// </typeparam>
        /// <param name="source">
        /// A sequence in which locate the values.
        /// </param>
        /// <param name="elements">
        /// A sequence of values to locate.
        /// </param>
        /// <returns>
        /// true if the specified source sequence contains all elements of the specified sequence;
        /// otherwise false.
        /// </returns>
        public static bool ContainsAny<T>( this IEnumerable<T> source, IEnumerable<T> elements )
        {
            Contract.Requires<ArgumentNullException>( source != null );
            Contract.Requires<ArgumentNullException>( elements != null );

            foreach( var element in elements )
            {
                if( source.Contains( element ) )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the zero-based index of element that fulfills the specified <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements in the sequence.
        /// </typeparam>
        /// <param name="sequence">
        /// The input sequence.
        /// </param>
        /// <param name="predicate">
        /// The predicate to check for.
        /// </param>
        /// <returns>
        /// The zero-based index of the element that first fulfills the specified predicate;
        /// -or- -1 if no element has fulfilled the predicate.
        /// </returns>
        public static int IndexOf<T>( this IEnumerable<T> sequence, Predicate<T> predicate )
        {
            Contract.Requires<ArgumentNullException>( sequence != null );
            Contract.Requires<ArgumentNullException>( predicate != null );
            Contract.Ensures( Contract.Result<int>() == -1 || (Contract.Result<int>() >= 0 && Contract.Result<int>() < sequence.Count()) );

            int index = 0;

            foreach( T element in sequence )
            {
                if( predicate( element ) )
                {
                    return index;
                }

                ++index;
            }

            return -1;
        }

        /// <summary>
        /// Verifies that the given predicate holds atleast <paramref name="count"/> times over this <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements to query over.
        /// </typeparam>
        /// <param name="sequence">
        /// The sequence to query.
        /// </param>
        /// <param name="count">
        /// The minimum number of times the predicate must hold.
        /// </param>
        /// <param name="predicate">
        /// The predicate to verify.
        /// </param>
        /// <returns>
        /// True if the predicate holds atleast count times; -or- otherwise false.
        /// </returns>
        public static bool AtLeast<T>( this IEnumerable<T> sequence, int count, Predicate<T> predicate )
        {
            Contract.Requires<ArgumentNullException>( sequence != null );
            Contract.Requires<ArgumentException>( count >= 0 );
            Contract.Requires<ArgumentNullException>( predicate != null );

            if( count == 0 )
            {
                return true;
            }

            foreach( var item in sequence )
            {
                if( predicate( item ) )
                {
                    --count;

                    if( count == 0 )
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
