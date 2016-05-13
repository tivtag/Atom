// <copyright file="ObjectMatrixFactory.cs" company="Microsoft">Copyright © Microsoft 2008</copyright>

namespace Atom.Collections
{
    using Microsoft.Pex.Framework;

    public static partial class ObjectMatrixFactory
    {
        [PexFactoryMethod( typeof( ObjectMatrix<int> ) )]
        public static ObjectMatrix<int> Create( int rows, int columns )
        {
            PexSymbolicValue.Minimize( rows );
            PexSymbolicValue.Minimize( columns );

            // Assume
            PexAssume.IsTrue( rows > 1 );
            PexAssume.IsTrue( rows <= 8 );
            PexAssume.IsTrue( columns > 1 );
            PexAssume.IsTrue( columns <= 8 );

            // Setup
            var matrix = new ObjectMatrix<int>( rows, columns );

            int value = 0;
            for( int r = 0; r < matrix.RowCount; ++r )
            {
                for( int c = 0; c < matrix.ColumnCount; ++c )
                {
                    matrix[r, c] = ++value;
                }
            }

            return matrix;
        }

        [PexFactoryMethod( typeof( ObjectMatrix<string> ) )]
        public static ObjectMatrix<string> CreateString( int rows, int columns )
        {
            PexSymbolicValue.Minimize( rows );
            PexSymbolicValue.Minimize( columns );

            // Assume
            PexAssume.IsTrue( rows > 1 );
            PexAssume.IsTrue( rows <= 8 );
            PexAssume.IsTrue( columns > 1 );
            PexAssume.IsTrue( columns <= 8 );

            // Setup
            var matrix = new ObjectMatrix<string>( rows, columns );
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            int value = 0;
            for( int r = 0; r < matrix.RowCount; ++r )
            {
                for( int c = 0; c < matrix.ColumnCount; ++c )
                {
                    matrix[r, c] = (++value).ToString( culture );
                }
            }

            return matrix;
        }
    }
}
