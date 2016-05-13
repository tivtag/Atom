// <copyright file="ObjectMatrixTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Tests.ObjectMatrixTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections.Tests
{
    using System;
    using System.Linq;
    using Microsoft.Pex.Framework;
    using Microsoft.Pex.Framework.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="ObjectMatrix{T}"/> class.
    /// </summary>
    [TestClass]
    [PexClass( typeof( ObjectMatrix<> ) )]
    [PexGenericArguments( typeof( int ) )]
    [PexGenericArguments( typeof( string ) )]
    public sealed partial class ObjectMatrixTests
    {
        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( OverflowException ) )]
        public void HasCorrectSizeAfterCreation<T>( int rows, int columns )
        {
            var matrix = new ObjectMatrix<T>( rows, columns );

            Assert.AreEqual( rows, matrix.RowCount );
            Assert.AreEqual( columns, matrix.ColumnCount );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( OverflowException ) )]
        public void IsSquareWhenRowsAndColumsSame<T>( int size )
        {
            var matrix = new ObjectMatrix<T>( size, size );

            Assert.IsTrue( matrix.IsSquare );
            Assert.AreEqual( size, matrix.RowCount );
            Assert.AreEqual( size, matrix.ColumnCount );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( OverflowException ) )]
        public void IsNotSquareWhenRowsAndColumsNotTheSame<T>( int rows, int columns )
        {
            // Assume
            PexAssume.AreNotEqual( rows, columns );

            // Handle
            var matrix = new ObjectMatrix<T>( rows, columns );

            // Assert
            Assert.IsFalse( matrix.IsSquare );
            Assert.AreEqual( rows, matrix.RowCount );
            Assert.AreEqual( columns, matrix.ColumnCount );
        }

        [PexMethod]
        public void CreateUsingMultiDimensionalArray_WithDistinctElements<T>( int rows, int columns, [PexAssumeNotNull]T[] elements )
        {
            // Assume
            PexAssume.IsTrue( rows <= 100 );
            PexAssume.IsTrue( columns <= 100 );
            PexAssume.IsTrue( rows > 1 );
            PexAssume.IsTrue( columns > 1 );
            PexAssume.AreNotEqual( rows, columns );
            PexAssume.AreEqual( rows * columns, elements.Length );
            PexAssume.AreDistinct( elements, ( x, y ) => {
                    if( object.ReferenceEquals( x, y ) )
                        return true;

                    return x.Equals( y );
                }
            );

            // Setup
            var multiArray = ArrayUtilities.CreateMultiDimensional( rows, columns, elements );

            // Assert
            CreateUsingMultiDimensionalArray( multiArray );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentNullException ) ), PexAllowedException( typeof( ArgumentException ) )]
        public void CreateUsingMultiDimensionalArray<T>( T[,] elements )
        {
            // Assumes
            PexAssume.IsTrue( elements == null || elements.GetLowerBound( 0 ) == 0 );
            PexAssume.IsTrue( elements == null || elements.GetLowerBound( 1 ) == 0 );
            
            // Setup
            var matrix = new ObjectMatrix<T>( elements );
            int rows = elements.GetLength( 0 );
            int columns = elements.GetLength( 1 );

            // Assert
            Assert.AreEqual( rows, matrix.RowCount );
            Assert.AreEqual( columns, matrix.ColumnCount );

            for( int row = 0; row < rows; ++row )
            {
                for( int column = 0; column < columns; ++column )
                {
                    Assert.AreEqual( 
                        elements[row, column],
                        matrix[row, column] 
                    );
                }
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void ResizingChangesSizeAccordingly<T>( 
            [PexAssumeUnderTest]ObjectMatrix<T> matrix, 
            int newRowCount,
            int newColumnCount )
        {
            // Assume
            PexAssume.IsTrue( newRowCount < 100 );
            PexAssume.IsTrue( newColumnCount < 100 );

            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.Resize( newRowCount, newColumnCount );

            // Assert
            Assert.AreEqual( newRowCount, matrix.RowCount );
            Assert.AreEqual( newColumnCount, matrix.ColumnCount );

            int originalRows = Math.Min( newRowCount, originalMatrix.RowCount );
            int originalColumns = Math.Min( newColumnCount, originalMatrix.ColumnCount );

            Assert.AreEqual(
                originalMatrix.GetSubMatrix( originalRows, originalColumns ),
                matrix.GetSubMatrix( originalRows, originalColumns )
            );
        }

        [PexMethod( MaxConstraintSolverTime=2 ), PexAllowedException( typeof( ArgumentException ) )]
        public void ResizingChangesSizeAccordingly_MakeLarger<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int newRowCount,
            int newColumnCount )
        {
            PexAssume.IsTrue( newRowCount > matrix.RowCount );
            PexAssume.IsTrue( newColumnCount > matrix.ColumnCount );

            ResizingChangesSizeAccordingly( matrix, newRowCount, newColumnCount );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( ArgumentOutOfRangeException ) )]
        public void GetSubMatrixReturnsExpectedMatrix<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix, 
            int subMatrixRowCount,
            int subMatrixColumnCount )
        {
            // Handle
            var subMatrix = matrix.GetSubMatrix( subMatrixRowCount, subMatrixColumnCount );

            // Assert
            Assert.IsNotNull( subMatrix );
            Assert.AreEqual( subMatrixRowCount, subMatrix.RowCount );
            Assert.AreEqual( subMatrixColumnCount, subMatrix.ColumnCount );

            for( int row = 0; row < subMatrixRowCount; ++row )
            {
                for( int column = 0; column < subMatrixColumnCount; ++column )
                {
                    Assert.AreEqual( matrix[row, column], subMatrix[row, column] );
                }
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void GetSubMatrixWithStartIndexReturnsExpectedMatrix<T>(
           [PexAssumeUnderTest]ObjectMatrix<T> matrix,
           int rowStart,
           int columnStart,
           int rowCount,
           int columnCount )
        {
            // Assume
            PexAssume.IsTrue( rowCount <= 100 );
            PexAssume.IsTrue( columnCount <= 100 );

            // Handle
            var subMatrix = matrix.GetSubMatrix( rowStart, columnStart, rowCount, columnCount );

            // Assert
            Assert.IsNotNull( subMatrix );
            Assert.AreEqual( rowCount, subMatrix.RowCount );
            Assert.AreEqual( columnCount, subMatrix.ColumnCount );

            for( int row = 0; row < rowCount; ++row )
            {
                for( int column = 0; column < columnCount; ++column )
                {
                    Assert.AreEqual( matrix[rowStart + row, columnStart + column], subMatrix[row, column] );
                }
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void GetSubMatrixWithStartIndexGreaterOneReturnsExpectedMatrix<T>(
           [PexAssumeUnderTest]ObjectMatrix<T> matrix,
           int rowStart,
           int columnStart,
           int rowCount,
           int columnCount )
        {
            // Assume
            PexAssume.IsTrue( rowStart > 1 );
            PexAssume.IsTrue( columnStart > 1 );
            PexAssume.IsTrue( rowCount <= 100 );
            PexAssume.IsTrue( columnCount <= 100 );

            GetSubMatrixWithStartIndexReturnsExpectedMatrix(
                matrix,
                rowStart,
                columnStart,
                rowCount,
                columnCount
            );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( ArgumentNullException ) )]
        public void SetSubMatrixWorksAsExpected<T>(
           [PexAssumeUnderTest]ObjectMatrix<T> matrix,
           ObjectMatrix<T> subMatrix,
           int rowStart,
           int columnStart,
           int rowCount,
           int columnCount )
        {
            // Assume
            PexAssume.IsTrue( rowStart <= 100 );
            PexAssume.IsTrue( columnStart <= 100 );
            PexAssume.IsTrue( rowCount <= 100 );
            PexAssume.IsTrue( columnCount <= 100 );

            // Handle
            var originalMatrix = matrix.Clone();
            matrix.SetSubMatrix( rowStart, columnStart, rowCount, columnCount, subMatrix );

            // Assert
            int rowEnd = rowStart + rowCount;
            int columnEnd = columnStart + columnCount;

            for( int i = rowStart; i < rowEnd; ++i )
            {
                for( int j = columnStart; j < columnEnd; ++j )
                {
                    Assert.AreEqual( subMatrix[i - rowStart, j - columnStart], matrix[i, j] );
                }
            }

            // The rest of the matrix shouldn't have been affected:
            for( int i = 0; i < rowStart; ++i )
            {
                for( int j = 0; j < columnStart; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }

            for( int i = rowEnd; i < matrix.RowCount; ++i )
            {
                for( int j = columnEnd; j < matrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( ArgumentNullException ) )]
        public void SetSubMatrixCopiesExactSubMatrix_WhenNotSpecifyingSize<T>(
           [PexAssumeUnderTest]ObjectMatrix<T> matrix,
           ObjectMatrix<T> subMatrix,
           int rowStart,
           int columnStart )
        {
            // Assume
            PexAssume.IsTrue( rowStart <= 100 );
            PexAssume.IsTrue( columnStart <= 100 );

            // Handle
            var originalMatrix = matrix.Clone();
            matrix.SetSubMatrix( rowStart, columnStart, subMatrix );

            // Assert
            int rowEnd = rowStart + subMatrix.RowCount;
            int columnEnd = columnStart + subMatrix.ColumnCount;

            for( int i = rowStart; i < rowEnd; ++i )
            {
                for( int j = columnStart; j < columnEnd; ++j )
                {
                    Assert.AreEqual( subMatrix[i - rowStart, j - columnStart], matrix[i, j] );
                }
            }

            // The rest of the matrix shouldn't have been affected:
            for( int i = 0; i < rowStart; ++i )
            {
                for( int j = 0; j < columnStart; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }

            for( int i = rowEnd; i < matrix.RowCount; ++i )
            {
                for( int j = columnEnd; j < matrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( ArgumentNullException ) )]
        public void SetSubMatrixWorksAsExpected_WithPositiveStartIndices<T>(
           [PexAssumeUnderTest]ObjectMatrix<T> matrix,
           ObjectMatrix<T> subMatrix,
           int rowStart,
           int columnStart,
           int rowCount,
           int columnCount )
        {
            // Assume
            PexAssume.IsTrue( rowStart > 1 );
            PexAssume.IsTrue( columnStart > 1 );

            // Assert
            SetSubMatrixWorksAsExpected( matrix, subMatrix, rowStart, columnStart, rowCount, columnCount );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( ArgumentNullException ) )]
        public void SetSubMatrixWorksAsExpected_WithPositiveStartIndices_2<T>(
           [PexAssumeUnderTest]ObjectMatrix<T> matrix,
           ObjectMatrix<T> subMatrix,
           int rowStart,
           int columnStart,
           int rowCount,
           int columnCount )
        {
            // Assume
            PexAssume.IsTrue( rowStart > 0 );
            PexAssume.IsTrue( columnStart > 0 );

            // Assert
            SetSubMatrixWorksAsExpected( matrix, subMatrix, rowStart, columnStart, rowCount, columnCount );
        }

        [PexMethod]
        public void CloningReturnsExpectedMatrix<T>( [PexAssumeNotNull]ObjectMatrix<T> matrix )
        {
            var clone = matrix.Clone();

            Assert.IsNotNull( clone );
            Assert.IsTrue( clone.Equals( matrix ) );
        }

        [PexMethod]
        public void EqualsReturnsFalseWhenMatricesDifferInSize<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix, 
            int rowSizeDifference,
            int columnSizeDifference )
        {
            PexSymbolicValue.Minimize( rowSizeDifference );
            PexSymbolicValue.Minimize( columnSizeDifference );

            // Assume
            PexAssume.IsTrue( rowSizeDifference >= 0 );
            PexAssume.IsTrue( columnSizeDifference >= 0 );
            PexAssume.IsTrue( rowSizeDifference > 0 || columnSizeDifference > 0 );

            // Setup
            var otherMatrix = matrix.Clone();
            otherMatrix.RowCount += rowSizeDifference;
            otherMatrix.RowCount += columnSizeDifference;

            // Handle & Assert
            Assert.IsFalse( matrix.Equals( otherMatrix ) );
            Assert.IsFalse( matrix.Equals( (object)otherMatrix ) );
            Assert.IsFalse( otherMatrix.Equals( matrix ) );
            Assert.IsFalse( otherMatrix.Equals( (object)matrix ) );
        }

        [PexMethod]
        public void EqualsReturnsFalseWhenOtherMatrixIsNull<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix )
        {
            // Handle & Assert
            Assert.IsFalse( matrix.Equals( (ObjectMatrix<T>)null ) );
            Assert.IsFalse( matrix.Equals( (object)null ) );
        }

        [PexMethod]
        public void EqualsReturnsTrueWhenPassedSame<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix )
        {
            // Handle & Assert
            Assert.IsTrue( matrix.Equals( matrix ) );
            Assert.IsTrue( matrix.Equals( (object)matrix ) );
        }

        [PexMethod]
        public void EqualsReturnsExpectedResult<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            [PexAssumeUnderTest]ObjectMatrix<T> otherMatrix )
        {
            // Handle
            bool areEqual = matrix.Equals( otherMatrix );

            // Assert
            bool sizeEqual = matrix.ColumnCount == otherMatrix.ColumnCount && matrix.RowCount == otherMatrix.RowCount;
            bool elementsEqual = matrix.ToArray().ElementsEqual( otherMatrix.ToArray() );
            bool areActuallyEqual = sizeEqual && elementsEqual;
            Assert.AreEqual( areActuallyEqual, areEqual  );
        }

        [PexMethod()]
        public void EqualsReturnsExpectedResult_WhenPassedSameMatrices<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix )
        {
            EqualsReturnsExpectedResult( matrix, matrix.Clone() );
        }

        [PexMethod( MaxConstraintSolverTime=4 )]
        public void EqualsReturnsExpectedResult_WhenPassedSameSizedMatricesWithDifferentValues<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            T value,
            int row,
            int column )
        {
            // Assume
            PexAssume.IsTrue( row >= 0 && row < matrix.RowCount );
            PexAssume.IsTrue( column >= 0 && column < matrix.ColumnCount );
            PexAssume.IsFalse( matrix[row, column].Equals( value ) );

            // Handle
            var otherMatrix = matrix.Clone();
            otherMatrix[row, column] = value;

            // Assert
            EqualsReturnsExpectedResult( matrix, otherMatrix );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void ResizesCorrectlyWhenSettingColumnCount<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix, 
            int newColumnCount )
        {
            // Assume
            PexSymbolicValue.Minimize( newColumnCount );

            // Setup
            int oldRowCount = matrix.RowCount;

            // Handle
            matrix.ColumnCount = newColumnCount;

            // Assert
            Assert.AreEqual( oldRowCount, matrix.RowCount );
            Assert.AreEqual( newColumnCount, matrix.ColumnCount );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void ResizesCorrectlyWhenSettingRowCount<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int newRowCount )
        {
            // Assume
            PexSymbolicValue.Minimize( newRowCount );

            // Setup
            int oldColumnCount = matrix.ColumnCount;

            // Handle
            matrix.RowCount = newRowCount;

            // Assert
            Assert.AreEqual( newRowCount, matrix.RowCount );
            Assert.AreEqual( oldColumnCount, matrix.ColumnCount );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void SetIndexer_SpecificElement_WorksAsExpected<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int row,
            int column,
            T value )
        {
            // Assume
            PexSymbolicValue.Minimize( row );
            PexSymbolicValue.Minimize( column );
            PexAssume.AreNotEqual( row, column );

            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix[row, column] = value;

            // Assert
            for( int r = 0; r < matrix.RowCount; ++r )
            {
                for( int c = 0; c < matrix.ColumnCount; ++c )
                {
                    T element = matrix[r, c];

                    if( r == row && c == column )
                    {
                        Assert.AreEqual( value, element );
                    }
                    else
                    {
                        Assert.AreEqual( originalMatrix[r, c], element );
                    }
                }
            }
        }

        [PexMethod]
        public void GetHashCodeReturnsSameValuesForEqualMatrices<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            [PexAssumeUnderTest]ObjectMatrix<T> otherMatrix )
        {
            // Assume
            PexAssume.AreEqual( matrix, otherMatrix );

            // Handle
            int hashA = matrix.GetHashCode();
            int hashB = otherMatrix.GetHashCode();

            // Assert
            Assert.AreEqual( hashA, hashB );
        }

        [PexMethod]
        public void Matrizes_WithDifferent_HashCodes_AreNot_Equal<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            [PexAssumeUnderTest]ObjectMatrix<T> otherMatrix )
        {
            // Assume
            PexAssume.IsTrue( matrix.GetHashCode() != otherMatrix.GetHashCode() );

            // Act
            bool areEqual = matrix.Equals( otherMatrix );

            // Assert
            Assert.IsFalse( areEqual );
        }

        [PexMethod]
        public void TheStringReturnedByToStringContainsAllElements<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix )
        {
            // Handle
            string str = matrix.ToString();

            // Assert
            foreach( T element in matrix )
            {
                CustomAssert.Contains( element.ToString(), str );
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void GetRowReturnsExpectedElements<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix, int row )
        {
            // Handle
            T[] elements = matrix.GetRow( row );

            // Assert
            Assert.IsNotNull( elements );
            Assert.AreEqual( matrix.ColumnCount, elements.Length );

            for( int i = 0; i < elements.Length; ++i )
            {
                Assert.AreEqual( matrix[row, i], elements[i] );
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void GetColumnReturnsExpectedElements<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix, int column )
        {
            // Handle
            T[] elements = matrix.GetColumn( column );

            // Assert
            Assert.IsNotNull( elements );
            Assert.AreEqual( matrix.RowCount, elements.Length );

            for( int i = 0; i < elements.Length; ++i )
            {
                Assert.AreEqual( matrix[i, column], elements[i] );
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( ArgumentNullException ) )]
        public void SetRowChangesExpectedElements<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix, T[] elements, int row )
        {
            // Assume
            if( elements != null )
            {
                PexAssume.AreDistinct( elements, ( x, y ) => { return Object.Equals( x, y ); } );
            }

            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.SetRow( row, elements );

            // Assert
            for( int i = 0; i < matrix.RowCount; ++i )
            {
                T[] array = matrix.GetRow( i );

                if( i == row )
                {
                    Assert.IsTrue( array.ElementsEqual( elements.Take( matrix.ColumnCount ) ) );
                }
                else
                {
                    Assert.IsTrue( array.ElementsEqual( originalMatrix.GetRow( i ) ) );
                }
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( ArgumentNullException ) )]
        public void SetColumnChangesExpectedElements<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix, T[] elements, int column )
        {
            // Assume
            if( elements != null )
            {
                PexAssume.AreDistinct( elements, ( x, y ) => { return Object.Equals( x, y ); } );
            }

            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.SetColumn( column, elements );

            // Assert
            for( int i = 0; i < matrix.ColumnCount; ++i )
            {
                T[] array = matrix.GetColumn( i );

                if( i == column )
                {
                    Assert.IsTrue( array.ElementsEqual( elements.Take( matrix.RowCount ) ) );
                }
                else
                {
                    Assert.IsTrue( array.ElementsEqual( originalMatrix.GetColumn( i ) ) );
                }
            }
        }

        [PexMethod]
        public void AddEmptyRowWorksAsExpected<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix )
        {
            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.AddRow();

            // Assert
            Assert.AreEqual( originalMatrix.RowCount + 1, matrix.RowCount );
            Assert.AreEqual( originalMatrix.ColumnCount, matrix.ColumnCount );

            for( int i = 0; i < originalMatrix.RowCount; ++i )
            {
                for( int j = 0; j < matrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }

            for( int j = 0; j < matrix.ColumnCount; ++j )
            {
                Assert.AreEqual( default( T ), matrix[(matrix.RowCount - 1), j] );
            }
        }

        [PexMethod]
        public void AddEmptyColumnWorksAsExpected<T>( [PexAssumeUnderTest]ObjectMatrix<T> matrix )
        {
            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.AddColumn();

            // Assert
            Assert.AreEqual( originalMatrix.RowCount, matrix.RowCount );
            Assert.AreEqual( originalMatrix.ColumnCount + 1, matrix.ColumnCount );

            for( int i = 0; i < matrix.RowCount; ++i )
            {
                for( int j = 0; j < originalMatrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }

            for( int i = 0; i < matrix.RowCount; ++i )
            {
                Assert.AreEqual( default( T ), matrix[i, (matrix.ColumnCount - 1)] );
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void AddingMultipleRowsAtTheSameTimeWorksAsExpected<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix, 
            int rowsToAdd )
        {
            // Assume
            PexSymbolicValue.Minimize( rowsToAdd );

            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.AddRows( rowsToAdd );

            // Asset
            Assert.AreEqual( originalMatrix.RowCount + rowsToAdd, matrix.RowCount );
            Assert.AreEqual( originalMatrix.ColumnCount, matrix.ColumnCount );

            for( int i = 0; i < originalMatrix.RowCount; ++i )
            {
                for( int j = 0; j < matrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }

            for( int i = originalMatrix.RowCount; i < matrix.RowCount; ++i )
            {
                for( int j = 0; j < matrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( default( T ), matrix[i, j] );
                }
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void AddingMultipleColumnsAtTheSameTimeWorksAsExpected<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int columnsToAdd )
        {
            // Assume
            PexSymbolicValue.Minimize( columnsToAdd );

            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.AddColumns( columnsToAdd );

            // Asset
            Assert.AreEqual( originalMatrix.RowCount, matrix.RowCount );
            Assert.AreEqual( originalMatrix.ColumnCount + columnsToAdd, matrix.ColumnCount );

            for( int i = 0; i < matrix.RowCount; ++i )
            {
                for( int j = 0; j < originalMatrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }

            for( int i = 0; i < matrix.RowCount; ++i )
            {
                for( int j = originalMatrix.ColumnCount; j < matrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( default( T ), matrix[i, j] );
                }
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( ArgumentNullException ) )]
        public void AddingRowWithSpecificValuesWorkAsExpected<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            params T[] values )
        {
            // Assume
            if( values != null )
            {
                PexAssume.AreDistinct( values, ( x, y ) => { return Object.Equals( x, y ); } );
            }

            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.AddRow( values );
            
            // Assert
            Assert.AreEqual( originalMatrix.RowCount + 1, matrix.RowCount );
            Assert.AreEqual( originalMatrix.ColumnCount, matrix.ColumnCount );

            for( int i = 0; i < originalMatrix.RowCount; ++i )
            {
                for( int j = 0; j < matrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }

            for( int j = 0; j < System.Math.Min( values.Length, matrix.ColumnCount ); ++j )
            {
                Assert.AreEqual( values[j], matrix[(matrix.RowCount - 1), j] );
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) ), PexAllowedException( typeof( ArgumentNullException ) )]
        public void AddingColumnWithSpecificValuesWorkAsExpected<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            params T[] values )
        {
            // Assume
            if( values != null )
            {
                PexAssume.AreDistinct( values, ( x, y ) => { return Object.Equals( x, y ); } );
            }

            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.AddColumn( values );

            // Assert
            Assert.AreEqual( originalMatrix.RowCount, matrix.RowCount );
            Assert.AreEqual( originalMatrix.ColumnCount + 1, matrix.ColumnCount );

            for( int i = 0; i < matrix.RowCount; ++i )
            {
                for( int j = 0; j < originalMatrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( originalMatrix[i, j], matrix[i, j] );
                }
            }

            for( int i = 0; i < System.Math.Min( values.Length, matrix.RowCount ); ++i )
            {
                Assert.AreEqual( values[i], matrix[i, (matrix.ColumnCount - 1)] );
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void DeleteRowWorksAsExpected<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int row )
        {
            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.DeleteRow( row );

            // Assert
            Assert.AreEqual( originalMatrix.ColumnCount, matrix.ColumnCount );
            Assert.AreEqual( originalMatrix.RowCount - 1, matrix.RowCount );

            for( int i = 0, i2 = 0; i < matrix.RowCount; ++i, ++i2 )
            {
                if( i == row )
                {
                    ++i2;
                }

                for( int j = 0; j < matrix.ColumnCount; ++j )
                {
                    Assert.AreEqual( originalMatrix[i2, j], matrix[i, j] );
                }
            }
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void DeleteColumnWorkAsExpected<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int column )
        {
            // Setup
            var originalMatrix = matrix.Clone();

            // Handle
            matrix.DeleteColumn( column );

            // Assert
            Assert.AreEqual( originalMatrix.ColumnCount - 1, matrix.ColumnCount );
            Assert.AreEqual( originalMatrix.RowCount, matrix.RowCount );

            for( int i = 0; i < matrix.RowCount; ++i )
            {
                for( int j = 0, j2 = 0; j < matrix.ColumnCount; ++j, ++j2 )
                {
                    if( j == column )
                    {
                        ++j2;
                    }

                    Assert.AreEqual( originalMatrix[i, j2], matrix[i, j] );
                }
            }
        }

        [PexMethod]
        public void InterchangingTheSameRowHasNoEffect<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int row )
        {
            // Assume
            PexAssume.IsTrue( row >= 0 && row < matrix.RowCount );

            // Arrange
            var originalMatrix = matrix.Clone();

            // Act
            matrix.InterchangeRows( row, row );

            // Assert
            Assert.AreEqual( originalMatrix, matrix );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void InterchangingTwoDistinctRowsWorksAsExpected<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int firstRow,
            int secondRow )
        {
            // Assume
            PexAssume.AreNotEqual( firstRow, secondRow );

            // Arrange
            var originalMatrix = matrix.Clone();

            // Act
            matrix.InterchangeRows( firstRow, secondRow );

            // Assert
            Assert.IsTrue( matrix.GetRow( firstRow ).ElementsEqual( originalMatrix.GetRow( secondRow ) ) );
            Assert.IsTrue( matrix.GetRow( secondRow ).ElementsEqual( originalMatrix.GetRow( firstRow ) ) );
        }

        [PexMethod]
        public void InterchangingTheSameColumnHasNoEffect<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int column )
        {
            // Assume
            PexAssume.IsTrue( column >= 0 && column < matrix.ColumnCount );

            // Arrange
            var originalMatrix = matrix.Clone();

            // Act
            matrix.InterchangeColumns( column, column );

            // Assert
            Assert.AreEqual( originalMatrix, matrix );
        }

        [PexMethod, PexAllowedException( typeof( ArgumentException ) )]
        public void InterchangingTwoDistinctColumnsWorksAsExpected<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int firstColumn,
            int secondColumn )
        {
            // Assume
            PexAssume.AreNotEqual( firstColumn, secondColumn );

            // Arrange
            var originalMatrix = matrix.Clone();

            // Act
            matrix.InterchangeColumns( firstColumn, secondColumn );

            // Assert
            Assert.IsTrue( matrix.GetColumn( firstColumn ).ElementsEqual( originalMatrix.GetColumn( secondColumn ) ) );
            Assert.IsTrue( matrix.GetColumn( secondColumn ).ElementsEqual( originalMatrix.GetColumn( firstColumn ) ) );
        }

        [PexMethod]
        public void FindingExistingElementReturnsExpectedMatrixIndicesAndResult<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix,
            int row,
            int column )
        {
            // Assume
            PexAssume.IsTrue( row >= 0 && row < matrix.RowCount );
            PexAssume.IsTrue( column >= 0 && column < matrix.ColumnCount );

            // Arrange
            T elementToFind = matrix[row, column];

            // Act
            int foundRow, foundColumn;
            bool found = matrix.Find( elementToFind, out foundRow, out foundColumn );

            // Assert
            Assert.IsTrue( found );
            Assert.AreEqual( row, foundRow );
            Assert.AreEqual( column, foundColumn );
        }

        [PexMethod]
        public void FindingNonExistingElementReturnsFalse<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix, 
            T elementToFind )
        {
            // Assume
            PexAssume.IsFalse( matrix.Contains( elementToFind ) );

            // Act
            int foundRow, foundColumn;
            bool found = matrix.Find( elementToFind, out foundRow, out foundColumn );

            // Assert
            Assert.IsFalse( found );
            Assert.AreEqual( -1, foundRow );
            Assert.AreEqual( -1, foundColumn );
        }

        [PexMethod]
        public void GetObjectData_WithArabitaryMatrix_SetsRowCount<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix )
        {
            // Arrange
            var context = new System.Runtime.Serialization.StreamingContext();
            var info = new System.Runtime.Serialization.SerializationInfo( 
                typeof( ObjectMatrix<T> ),
                new System.Runtime.Serialization.FormatterConverter()
            );
            
            // Act
            matrix.GetObjectData( info, context );

            // Assert
            int rowCount = info.GetInt32( "RowCount" );
            Assert.AreEqual( matrix.RowCount, rowCount );
        }

        [PexMethod]
        public void GetObjectData_WithArabitaryMatrix_SetsColumnCount<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix )
        {
            // Arrange
            var context = new System.Runtime.Serialization.StreamingContext();
            var info = new System.Runtime.Serialization.SerializationInfo(
                typeof( ObjectMatrix<T> ),
                new System.Runtime.Serialization.FormatterConverter()
            );

            // Act
            matrix.GetObjectData( info, context );

            // Assert
            int columnCount = info.GetInt32( "ColumnCount" );
            Assert.AreEqual( matrix.ColumnCount, columnCount );
        }

        [PexMethod]
        public void GetObjectData_WithArabitaryMatrix_SetsData<T>(
            [PexAssumeUnderTest]ObjectMatrix<T> matrix )
        {
            // Arrange
            var context = new System.Runtime.Serialization.StreamingContext();
            var info = new System.Runtime.Serialization.SerializationInfo(
                typeof( ObjectMatrix<T> ),
                new System.Runtime.Serialization.FormatterConverter()
            );

            // Act
            matrix.GetObjectData( info, context );

            // Assert
            T[] data = (T[])info.GetValue( "Data", typeof( T[] ) );
          
            Assert.AreEqual( data.Length, matrix.RowCount * matrix.ColumnCount );

            foreach( T element in matrix )
            {
                CustomAssert.Contains( element, data );
            }
        }

        [PexMethod]
        public void AddColumns_NegativeCount_Throws( int count )
        {
            // Assume
            PexAssume.IsTrue( count < 0 );

            // Arrange
            var matrix = new ObjectMatrix<int>( 1, 1 );

            // Act & Assert
            CustomAssert.Throws<ArgumentException>(
                () => matrix.AddColumns( count )
            );
        }

        [PexMethod]
        public void AddRows_NegativeCount_Throws( int count )
        {
            // Assume
            PexAssume.IsTrue( count < 0 );

            // Arrange
            var matrix = new ObjectMatrix<int>( 1, 1 );

            // Act & Assert
            CustomAssert.Throws<ArgumentException>(
                () => matrix.AddRows( count )
            );
        }

        [PexMethod]
        public void GetEnumerator_WhenMatrixConvertedToEnumerable_IteratesOverAllElements<T>( [PexAssumeNotNull]ObjectMatrix<T> matrix )
        {
            // Arrange
            System.Collections.IEnumerable enumerable = matrix;

            // Assert
            foreach( var item in enumerable.Cast<T>() )
            {
                Assert.IsTrue( matrix.Contains( item ) );                
            }
        }
    }
}
