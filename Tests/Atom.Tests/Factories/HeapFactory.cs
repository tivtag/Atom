// <copyright file="HeapFactory.cs" company="Microsoft">Copyright © Microsoft 2008</copyright>

namespace Atom.Collections
{
    using Microsoft.Pex.Framework;

    /// <summary>A factory for Atom.Collections.Heap`1[System.Int32] instances</summary>
    public static partial class HeapFactory
    {
        [PexFactoryMethod( typeof( Heap<int> ) )]
        public static Heap<int> Create( HeapType heapType, [PexAssumeNotNull]int[] items )
        {
            PexAssume.IsTrue( items.Length < 10 );
            PexAssume.IsTrue( items.HasDistinctElements() );

            var heap = new Heap<int>( heapType );

            for( int i = 0; i < items.Length; ++i )
            {
                heap.Add( items[i] );
            }

            return heap;
        }

        [PexFactoryMethod( typeof( Heap<string> ) )]
        public static Heap<string> Create( HeapType heapType, [PexAssumeNotNull]string[] items )
        {
            PexAssume.IsTrue( items.Length < 10 );
            PexAssume.IsTrue( items.HasDistinctElements() );

            var heap = new Heap<string>( heapType );

            for( int i = 0; i < items.Length; ++i )
            {
                heap.Add( items[i] );
            }

            return heap;
        }
    }
}
