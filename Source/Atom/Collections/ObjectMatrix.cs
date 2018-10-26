// <copyright file="ObjectMatrix.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Collections.ObjectMatrix{T} class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Atom.Diagnostics.Contracts;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a matrix of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the object matrix.
    /// </typeparam>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( System.ComponentModel.ExpandableObjectConverter ) )]
    public class ObjectMatrix<T> : IEnumerable<T>, IEquatable<ObjectMatrix<T>>, ISerializable                                   
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the number of columns this <see cref="ObjectMatrix{T}"/> has.
        /// </summary>
        /// <value>
        /// The number of columns this <see cref="ObjectMatrix{T}"/> has.
        /// </value>
        public int ColumnCount
        {
            [Pure]
            get
            {
                // Contract.Ensures( Contract.Result<int>() >= 1 );

                return this.columnCount;
            }

            set
            {
                Contract.Requires<ArgumentException>( value >= 1 );
                // Contract.Ensures( this.ColumnCount == value );

                this.Resize( this.rowCount, value );
            }
        }

        /// <summary>
        /// Gets or sets the number of rows this <see cref="ObjectMatrix{T}"/> has.
        /// </summary>
        /// <value>
        /// The number of rows this <see cref="ObjectMatrix{T}"/> has.
        /// </value>
        public int RowCount
        {
            [Pure]
            get
            {
                // Contract.Ensures( Contract.Result<int>() >= 1 );

                return this.rowCount;
            }

            set
            {
                Contract.Requires<ArgumentException>( value >= 1 );
                // Contract.Ensures( this.RowCount == value );

                this.Resize( value, this.columnCount );
            }
        }

        /// <summary>
        /// Gets or sets the value at the specified 
        /// <paramref name="row"/> and <paramref name="column"/>.
        /// </summary>
        /// <param name="row">
        /// The zero-based index of the row.
        /// </param>
        /// <param name="column">
        /// The zero-based index of the column.
        /// </param>
        /// <returns>The value at the specified position.</returns>
        public T this[int row, int column]
        {
            [Pure]
            get
            {
                return this.data[GetIndex(row, column)];
            }

            set
            {
                // Contract.Ensures( Object.Equals( this[row, column], value ) );

                this.data[GetIndex(row, column)] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this Matrix is a square Matrix.
        /// </summary>
        /// <remarks>
        /// A <see cref="ObjectMatrix{T}"/> is square if
        /// its <see cref="RowCount"/> is equals to its <see cref="ColumnCount"/>.
        /// </remarks>
        /// <value>
        /// Returns <c>true</c> if this matrix is square; 
        /// otherwise, <c>false</c>.
        /// </value>
        [Pure]
        public bool IsSquare
        {
            get
            {
                return this.rowCount == this.columnCount;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMatrix{T}"/> class.
        /// </summary>
        /// <param name="rowCount">
        /// The number of rows the new ObjectMatrix should have.
        /// </param>
        /// <param name="columnCount">
        /// The number of columns the new ObjectMatrix should have.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="rowCount"/> or <paramref name="columnCount"/> is less than or equal 0.
        /// </exception>
        public ObjectMatrix( int rowCount, int columnCount )
        {
            this.Initialize( rowCount, columnCount );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMatrix{T}"/> class.
        /// </summary>
        /// <param name="elements">
        /// The elements of the new ObjectMatrix{T}.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="elements"/> is null.
        /// </exception>
        public ObjectMatrix( T[,] elements )
        {
            Contract.Requires<ArgumentNullException>( elements != null );
            Contract.Requires( elements.GetLowerBound( 0 ) == 0 );
            Contract.Requires( elements.GetLowerBound( 1 ) == 0 );

            this.Initialize( elements.GetLength( 0 ), elements.GetLength( 1 ) );

            // Copy the data.            
            for( int row = 0; row < this.rowCount; ++row )
            {
                for( int column = 0; column < this.columnCount; ++column )
                {
                    this[row, column] = elements[row, column];
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMatrix{T}"/> class.
        /// </summary>
        /// <param name="matrix">
        /// The existing ObjectMatrix{T} that should be cloned.
        /// </param>
        public ObjectMatrix( ObjectMatrix<T> matrix )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );
            // Contract.Ensures( this.RowCount ==  matrix.RowCount );
            // Contract.Ensures( this.ColumnCount ==  matrix.ColumnCount );

            this.rowCount = matrix.rowCount;
            this.columnCount = matrix.columnCount;
            this.data = (T[])matrix.data.Clone();
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="ObjectMatrix{T}"/> class; and
        /// sets values of the new <see cref="ObjectMatrix{T}"/> to the
        /// values specified by the <see cref="System.Runtime.Serialization.SerializationInfo"/>.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext "/> that 
        /// contains contextual information about the source or destination.
        /// Can be null.
        /// </param>
        protected ObjectMatrix( SerializationInfo info, StreamingContext context )
        {
            this.rowCount = info.GetInt32( "RowCount" );
            this.columnCount = info.GetInt32( "ColumnCount" );

            this.data = (T[])info.GetValue( "Data", typeof( Array ) );
        }

        /// <summary>
        /// Used to initialize a new instance of the ObjectMatrix class.
        /// </summary>
        /// <param name="rowCount">
        /// The number of rows the ObjectMatrix should have.
        /// </param>
        /// <param name="columnCount">
        /// The number of columns the ObjectMatrix should have.
        /// </param>
        private void Initialize( int rowCount, int columnCount )
        {
            Contract.Requires<ArgumentException>( rowCount > 0 );
            Contract.Requires<ArgumentException>( columnCount > 0 );

            // Contract.Ensures( this.rowCount == rowCount );
            // Contract.Ensures( this.columnCount == columnCount );
            // Contract.Ensures( this.data.Length == (rowCount * columnCount ) );

            this.rowCount    = rowCount;
            this.columnCount = columnCount;
            this.data        = new T[rowCount * columnCount];
        }

        #endregion

        #region [ Methods ]

        #region > Interchange <

        /// <summary>
        /// Interchanges/swaps two rows of this ObjectMatrix{T}.
        /// </summary>
        /// <param name="firstRow">
        /// The zero-based index of the first row.
        /// </param>
        /// <param name="secondRow">
        /// The zero-based index of the second row.
        /// </param>
        public void InterchangeRows( int firstRow, int secondRow )
        {
            Contract.Requires<ArgumentException>( firstRow >= 0 );
            Contract.Requires<ArgumentException>( firstRow < this.RowCount );
            Contract.Requires<ArgumentException>( secondRow >= 0 );
            Contract.Requires<ArgumentException>( secondRow < this.RowCount );
            
            if( firstRow == secondRow )
                return;

            for( int column = 0; column < this.columnCount; ++column )
            {
                T temp = this[firstRow, column];

                this[firstRow, column]  = this[secondRow, column];
                this[secondRow, column] = temp;
            }
        }

        /// <summary>
        /// Interchanges/swaps two columns of this ObjectMatrix{T}.
        /// </summary>
        /// <param name="firstColumn">
        /// The zero-based index of the first column.
        /// </param>
        /// <param name="secondColumn">
        /// The zero-based index of the second column.
        /// </param>
        public void InterchangeColumns( int firstColumn, int secondColumn )
        {
            Contract.Requires<ArgumentException>( firstColumn >= 0 );
            Contract.Requires<ArgumentException>( firstColumn < this.ColumnCount );
            Contract.Requires<ArgumentException>( secondColumn >= 0 );
            Contract.Requires<ArgumentException>( secondColumn < this.ColumnCount );

            if( firstColumn == secondColumn )
                return;

            for( int row = 0; row < this.rowCount; ++row )
            {
                T temp = this[row, firstColumn];

                this[row, firstColumn]  = this[row, secondColumn];
                this[row, secondColumn] = temp;
            }
        }

        #endregion

        #region > Get <

        /// <summary>
        /// Gets the index into the private data array
        /// for the given row/column.
        /// </summary>
        /// <param name="row">
        /// The row of the element to get.
        /// </param>
        /// <param name="column">
        /// The column of the element to get.
        /// </param>
        /// <returns>
        /// The corresponding index into the data array.
        /// </returns>
        [Pure]
        private int GetIndex( int row, int column )
        {
            Contract.Requires<ArgumentOutOfRangeException>( row >= 0 );
            Contract.Requires<ArgumentOutOfRangeException>( row < this.RowCount );
            Contract.Requires<ArgumentOutOfRangeException>( column >= 0 );
            Contract.Requires<ArgumentOutOfRangeException>( column < this.ColumnCount );
            // Contract.Ensures( Contract.Result<int>() >= 0 );
            // Contract.Ensures( Contract.Result<int>() < this.data.Length );

            return (row * this.columnCount) + column;
        }

        /// <summary>
        /// Gets a sub matrix of this <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <param name="rowCount">
        /// The row count.
        /// </param>
        /// <param name="columnCount">
        /// The column count.
        /// </param>
        /// <returns>
        /// A sub matrix of the current matrix.
        /// </returns>
        [Pure]
        public ObjectMatrix<T> GetSubMatrix( int rowCount, int columnCount )
        {
            return GetSubMatrix( 0, 0, rowCount, columnCount );
        }

        /// <summary>
        /// Gets a sub matrix of this <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <param name="rowStart">The row start.</param>
        /// <param name="columnStart">The column start.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="columnCount">The column count.</param>
        /// <returns>
        /// A sub matrix of the current matrix.
        /// </returns>
        [Pure]
        public ObjectMatrix<T> GetSubMatrix( int rowStart, int columnStart, int rowCount, int columnCount )
        {
            Contract.Requires<ArgumentException>( rowStart >= 0 );
            Contract.Requires<ArgumentException>( rowStart < this.RowCount );
            Contract.Requires<ArgumentException>( columnStart >= 0 );
            Contract.Requires<ArgumentException>( columnStart < this.ColumnCount );

            Contract.Requires<ArgumentException>( rowCount > 0 );
            Contract.Requires<ArgumentException>( columnCount > 0 );
            Contract.Requires<ArgumentException>( (rowStart + rowCount) <= this.RowCount );
            Contract.Requires<ArgumentException>( (columnStart + columnCount) <= this.ColumnCount );

            // Contract.Ensures( Contract.Result<ObjectMatrix<T>>() != null );
            // Contract.Ensures( Contract.Result<ObjectMatrix<T>>().RowCount == rowCount );
            // Contract.Ensures( Contract.Result<ObjectMatrix<T>>().ColumnCount == columnCount );

            var subMatrix = new ObjectMatrix<T>( rowCount, columnCount );

            int rowEnd = rowStart + rowCount;
            int columnEnd = columnStart + columnCount;

            for( int i = rowStart; i < rowEnd; ++i )
            {
                for( int j = columnStart; j < columnEnd; ++j )
                {
                    subMatrix[i - rowStart, j - columnStart] = this[i, j];
                }
            }

            return subMatrix;
        }

        /// <summary>
        /// Gets the row at the specified zero-based index.
        /// </summary>
        /// <param name="row">
        /// The zero-based index of the row to get.
        /// </param>
        /// <returns>
        /// An array containing the values of the requested row.
        /// </returns>
        [Pure]
        public T[] GetRow( int row )
        {
            Contract.Requires<ArgumentException>( row >= 0 );
            Contract.Requires<ArgumentException>( row < this.RowCount );
            // Contract.Ensures( Contract.Result<T[]>() != null );
            // Contract.Ensures( Contract.Result<T[]>().Length == this.ColumnCount );

            T[] array = new T[this.columnCount];

            for( int column = 0; column < this.columnCount; ++column )
            {
                array[column] = this[row, column];
            }

            return array;
        }

        /// <summary>
        /// Gets the column at the specified zero-based index.
        /// </summary>
        /// <param name="column">
        /// The zero-based index of the column to get.
        /// </param>
        /// <returns>
        /// An array containing the values of the requested column.
        /// </returns>
        [Pure]
        public T[] GetColumn( int column )
        {
            Contract.Requires<ArgumentException>( column >= 0 );
            Contract.Requires<ArgumentException>( column < this.ColumnCount );
            // Contract.Ensures( Contract.Result<T[]>() != null );
            // Contract.Ensures( Contract.Result<T[]>().Length == this.RowCount );

            T[] array = new T[this.rowCount];

            for( int row = 0; row < this.rowCount; ++row )
            {
                array[row] = this[row, column];
            }

            return array;
        }

        #endregion

        #region > Set <

        /// <summary>
        /// Sets the sub matrix at the given position.
        /// </summary>
        /// <param name="rowStart">
        /// The row start.
        /// </param>
        /// <param name="columnStart">
        /// The column start.
        /// </param>
        /// <param name="subMatrix">
        /// The matrix to set at the given position.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="subMatrix"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="rowStart"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="columnStart"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="rowStart"/> plus RowCount of the subMatrix is greater thn RowCount.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="columnStart"/> plus ColumnCount of the subMatrix is greater than ColumnCount.
        /// </exception>
        public void SetSubMatrix( int rowStart, int columnStart, ObjectMatrix<T> subMatrix )
        {
            Contract.Requires<ArgumentNullException>( subMatrix != null );

            this.SetSubMatrix( rowStart, columnStart, subMatrix.RowCount, subMatrix.ColumnCount, subMatrix );
        }

        /// <summary>
        /// Sets the sub matrix at the given position.
        /// </summary>
        /// <param name="rowStart">
        /// The row start.
        /// </param>
        /// <param name="columnStart">
        /// The column start.
        /// </param>
        /// <param name="rowCount">
        /// The number of rows to set.
        /// </param>
        /// <param name="columnCount">
        /// The number of columns to set.
        /// </param>
        /// <param name="subMatrix">
        /// The matrix to set at the given position.
        /// </param>
        public void SetSubMatrix( int rowStart, int columnStart, int rowCount, int columnCount, ObjectMatrix<T> subMatrix )
        {
            Contract.Requires<ArgumentNullException>( subMatrix != null );
            Contract.Requires<ArgumentException>( rowStart >= 0 );
            Contract.Requires<ArgumentException>( columnStart >= 0 );

            Contract.Requires<ArgumentException>( rowCount > 0 );
            Contract.Requires<ArgumentException>( columnCount > 0 );
            Contract.Requires<ArgumentException>( rowCount <= subMatrix.RowCount );
            Contract.Requires<ArgumentException>( columnCount <= subMatrix.ColumnCount );

            Contract.Requires<ArgumentException>( (rowStart + rowCount) <= this.RowCount );
            Contract.Requires<ArgumentException>( (columnStart + columnCount) <= this.ColumnCount );

            int rowEnd = rowStart + rowCount;
            int columnEnd = columnStart + columnCount;

            for( int i = rowStart; i < rowEnd; ++i )
            {
                for( int j = columnStart; j < columnEnd; ++j )
                {
                    this[i, j] = subMatrix[i - rowStart, j - columnStart];
                }
            }
        }
        
        /// <summary>
        /// Sets the to specified row to contain the specified elements.
        /// </summary>
        /// <param name="row">
        /// The zero-based index of the row that should be modified.
        /// </param>
        /// <param name="elements">
        /// The elements to set.
        /// </param>
        public void SetRow( int row, T[] elements )
        {
            Contract.Requires<ArgumentException>( row >= 0 );
            Contract.Requires<ArgumentException>( row < this.RowCount );
            Contract.Requires<ArgumentNullException>( elements != null );
            Contract.Requires<ArgumentException>( elements.Length >= this.ColumnCount );

            for( int column = 0; column < this.ColumnCount; ++column )
            {
                this[row, column] = elements[column];
            }
        }

        /// <summary>
        /// Sets the to specified column to contain the specified elements.
        /// </summary>
        /// <param name="column">
        /// The zero-based index of the column that should be modified.
        /// </param>
        /// <param name="elements">
        /// The elements to set.
        /// </param>
        public void SetColumn( int column, T[] elements )
        {
            Contract.Requires<ArgumentException>( column >= 0 );
            Contract.Requires<ArgumentException>( column < this.ColumnCount );
            Contract.Requires<ArgumentNullException>( elements != null );
            Contract.Requires<ArgumentException>( elements.Length >= this.RowCount );

            for( int row = 0; row < this.RowCount; ++row )
            {
                this[row, column] = elements[row];
            }
        }

        #endregion

        #region > Add <

        /// <summary>
        /// Adds a single row to the matrix.
        /// </summary>
        public void AddRow()
        {
            // Contract.Ensures( this.ColumnCount == Contract.OldValue<int>( this.ColumnCount ) );
            // Contract.Ensures( this.RowCount == (Contract.OldValue<int>( this.RowCount ) + 1) );

            this.AddRows( 1 );
        }
        
        /// <summary>
        /// Adds a single row to this ObjectMatrix{T}, and populates the values
        /// accordingly.
        /// </summary>
        /// <param name="values">
        /// The values to populate the new row with.
        /// </param>
        public void AddRow( params T[] values )
        {
            Contract.Requires<ArgumentNullException>( values != null );

            this.AddRow();

            int columnEnd = System.Math.Min( values.Length, this.columnCount );

            for( int column = 0; column < columnEnd; ++column )
            {
                this[this.rowCount - 1, column] = values[column];
            }
        }

        /// <summary>
        /// Adds the specified number of rows to this ObjectMatrix{T}.
        /// </summary>
        /// <param name="count">
        /// The number of rows to add.
        /// </param>
        public void AddRows( int count )
        {
            Contract.Requires<ArgumentException>( count >= 0 );
            // Contract.Ensures( this.ColumnCount == Contract.OldValue<int>( this.ColumnCount ) );
            // Contract.Ensures( this.RowCount == (Contract.OldValue<int>( this.RowCount ) + count) );

            int newRowCount = this.rowCount + count;
            T[] newData = new T[newRowCount * this.columnCount];            
            Array.Copy( this.data, newData, this.data.Length );

            this.data = newData;
            this.rowCount = newRowCount;
        }

        /// <summary>
        /// Adds a single column to this ObjectMatrix{T}.
        /// </summary>    
        public void AddColumn()
        {
            // Contract.Ensures( this.ColumnCount == (Contract.OldValue<int>( this.ColumnCount ) + 1) );
            // Contract.Ensures( this.RowCount == Contract.OldValue<int>( this.RowCount ) );

            this.AddColumns( 1 );
        }

        /// <summary>
        /// Adds a single row to the matrix, and populates the values
        /// accordingly.
        /// </summary>
        /// <param name="values">The values to populate the row with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The length of <paramref name="values"/> is greater than <see cref="RowCount"/>.</exception>
        public void AddColumn( params T[] values )
        {
            Contract.Requires<ArgumentNullException>( values != null );

            this.AddColumn();

            int rowEnd = System.Math.Min( values.Length, this.rowCount );

            for( int row = 0; row < rowEnd; ++row )
            {
                this[row, this.columnCount - 1] = values[row];
            }
        }

        /// <summary>
        /// Adds the specified number of rows to this ObjectMatrix{T}.
        /// </summary>
        /// <param name="count">
        /// The number of columns to add.
        /// </param>   
        public void AddColumns( int count )
        {
            Contract.Requires<ArgumentException>( count >= 0 );
            // Contract.Ensures( this.ColumnCount == (Contract.OldValue<int>( this.ColumnCount ) + count) );
            // Contract.Ensures( this.RowCount == Contract.OldValue<int>( this.RowCount ) );

            int newColumnCount = this.columnCount + count;
            T[] newData = new T[this.rowCount * newColumnCount];

            // Copy in blocks row by row:
            for( int row = 0; row < this.rowCount; ++row )
            {
                Array.Copy(
                    data,
                    (row * this.columnCount),
                    newData,
                    (row * newColumnCount),
                    this.columnCount
                 );
            }

            this.data        = newData;
            this.columnCount = newColumnCount;
        }

        #endregion

        #region > Resize <

        /// <summary>
        /// Resizes this ObjectMatrix{T} to the specified size.
        /// </summary>
        /// <param name="newRowCount">
        /// The new number of rows this ObjectMatrix{T} should have.
        /// </param>
        /// <param name="newColumnCount">
        /// The new number of columns this ObjectMatrix{T} should have.
        /// </param>
        public void Resize( int newRowCount, int newColumnCount )
        {
            Contract.Requires<ArgumentException>( newRowCount > 0 );
            Contract.Requires<ArgumentException>( newColumnCount > 0 );
            // Contract.Ensures( this.RowCount == newRowCount );
            // Contract.Ensures( this.ColumnCount == newColumnCount );

            if ( newRowCount == this.rowCount && newColumnCount == this.columnCount )
                return;
            
            T[] newData = new T[newRowCount * newColumnCount];

            // Find the minimum of the rows and the columns.
            // Case 1 : Target array is smaller than original - don't cross boundaries of target.
            // Case 2 : Original is smaller than target - don't cross boundaries of original.
            int minRows   = System.Math.Min( this.rowCount, newRowCount );
            int minColumns = System.Math.Min( this.columnCount, newColumnCount );

            for( int row = 0; row < minRows; ++row )
            {
                for( int column = 0; column < minColumns; ++column )
                {
                    newData[(row * newColumnCount) + column] = this[row, column];
                }
            }

            this.data        = newData;
            this.rowCount    = newRowCount;
            this.columnCount = newColumnCount;
        }

        #endregion

        #region > Delete <

        /// <summary>
        /// Deletes the row from this ObjectMatrix{T}.
        /// </summary>
        /// <param name="row">
        /// The index of the row to delete.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the given <paramref name="row"/> index is out of valid range.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If there exists only one row.
        /// </exception>
        public void DeleteRow( int row )
        {
            Contract.Requires<ArgumentException>( row >= 0 );
            Contract.Requires<ArgumentException>( row < this.RowCount );
            Contract.Requires<InvalidOperationException>( this.RowCount > 1, ErrorStrings.MatrixDeleteOnlyRow );

            // Contract.Ensures( this.RowCount == (Contract.OldValue( this.RowCount ) - 1) );

            int newRowCount = this.rowCount - 1;
            T[] newData = new T[newRowCount * this.columnCount];

            // Copy the data before the row
            Array.Copy( data, 0, newData, 0, row * this.columnCount );

            // Copy the data after the row
            Array.Copy( 
                data, 
                ((row + 1) * this.columnCount), 
                newData,
                row * this.columnCount,
                this.columnCount * (newRowCount - row)
            );

            this.data = newData;
            --this.rowCount;
        }

        /// <summary>
        /// Deletes the column from this ObjectMatrix{T}.
        /// </summary>
        /// <param name="column">
        /// The index of the column to delete.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the given <paramref name="column"/> index is out of valid range.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If there exists only one column.
        /// </exception>
        public void DeleteColumn( int column )
        {
            Contract.Requires<ArgumentException>( column >= 0 );
            Contract.Requires<ArgumentException>( column < this.ColumnCount );
            Contract.Requires<InvalidOperationException>( this.ColumnCount > 1, ErrorStrings.MatrixDeleteOnlyRow );

            // Contract.Ensures( this.ColumnCount == (Contract.OldValue( this.ColumnCount ) - 1) );

            T[] newData = new T[rowCount * (columnCount - 1)];

            // Use an exclusion strategy:
            for( int i = 0; i < this.rowCount; ++i )
            {
                int columnIndex = 0;

                for( int j = 0; j < this.columnCount; ++j )
                {
                    if( j != column )
                    {
                        int index = (i * (this.columnCount - 1)) + columnIndex;
                        newData[index] = this[i, j];

                        ++columnIndex;
                    }
                }
            }

            this.data = newData;
            --this.columnCount;
        }

        #endregion

        #region > Find <

        /// <summary>
        /// Tries to find the first occurence of the specified <paramref name="element"/>
        /// using the <see cref="EqualityComparer&lt;T&gt;.Default"/>
        /// </summary>
        /// <param name="element">
        /// The element to find.
        /// </param>
        /// <param name="row">
        /// When this method returns will contain the zero-based row index of the element; if found.
        /// </param>
        /// <param name="column">
        /// When this method returns will contain the zero-based column index of the element; if found.
        /// </param>
        /// <returns>
        /// true if the element was found; 
        /// otherwise false.
        /// </returns>
        [Pure]
        public bool Find( T element, out int row, out int column )
        {
            return Find( element, out row, out column, EqualityComparer<T>.Default );
        }

        /// <summary>
        /// Tries to find the first occurence of the specified <paramref name="element"/>
        /// using the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="element">
        /// The element to find.
        /// </param>
        /// <param name="row">
        /// When this method returns will contain the zero-based row index of the element if found;
        /// otherwise if not found -1.
        /// </param>
        /// <param name="column">
        /// When this method returns will contain the zero-based column index of the element if found;
        /// otherwise if not found -1.
        /// </param>
        /// <param name="comparer">
        /// The comparer that should be used to compare the elements of this
        /// ObjectMatrix{T} with the specified <paramref name="element"/>.
        /// </param>
        /// <returns>
        /// true if the element was found; 
        /// otherwise false.
        /// </returns>
        [Pure]
        public bool Find( T element, out int row, out int column, IEqualityComparer<T> comparer )
        {
            Contract.Requires<ArgumentNullException>( comparer != null );
            // Contract.Ensures( Contract.Result<bool>() || (Contract.ValueAtReturn<int>( out row ) == -1) );
            // Contract.Ensures( Contract.Result<bool>() || (Contract.ValueAtReturn<int>( out column ) == -1) );
            // Contract.Ensures( !Contract.Result<bool>() || (Contract.ValueAtReturn<int>( out row ) >= 0 && Contract.ValueAtReturn<int>( out row ) < this.RowCount ) );
            // Contract.Ensures( !Contract.Result<bool>() || (Contract.ValueAtReturn<int>( out column ) >= 0 && Contract.ValueAtReturn<int>( out column ) < this.ColumnCount ) );

            for ( int x = 0; x < this.rowCount; ++x )
            {
                for( int y = 0; y < this.columnCount; ++y )
                {
                    T thisElement = this[x, y];

                    if( comparer.Equals( element, thisElement ) )
                    {
                        row = x;
                        column = y;
                        return true;
                    }
                }
            }

            row    = -1;
            column = -1;
            return false;
        }

        #endregion

        #region > Overrides / Impls <

        #region Equals

        /// <summary>
        /// Returns whether the specified <see cref="Object"/>
        /// is equal to this <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test against. Can be null.
        /// </param>
        /// <returns>
        /// Returns true if the specified <see cref="Object"/> is equal to this <see cref="ObjectMatrix{T}"/>;
        /// otherwise false.
        /// </returns>
        public override bool Equals( object obj )
        {
            var matrix = obj as ObjectMatrix<T>;
            if( matrix == null )
                return false;

            return this.Equals( matrix );
        }

        /// <summary>
        /// Returns whether the specified <see cref="ObjectMatrix{T}"/>
        /// is equal to this <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="ObjectMatrix{T}"/> to test against. Can be null.
        /// </param>
        /// <returns>
        /// Returns true if the specified <see cref="ObjectMatrix{T}"/> is equal to 
        /// this <see cref="ObjectMatrix{T}"/>; otherwise false.
        /// </returns>
        public bool Equals( ObjectMatrix<T> other )
        {
            if( other == null )
                return false;

            if( this.rowCount != other.rowCount || this.columnCount != other.columnCount )
                return false;

            for( int row = 0; row < rowCount; ++row )
            {
                for( int column = 0; column < columnCount; ++column )
                {
                    if( !this[row, column].Equals( other[row, column] ) )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region GetObjectData

        /// <summary>
        /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// with the data needed to serialize the <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// to populate with data.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// If the given <paramref name="info"/> is null.
        /// </exception>
        /// <param name="context"> 
        /// The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>)
        /// for this serialization. Can be null.
        /// </param>
        [System.Security.Permissions.SecurityPermissionAttribute(
            System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true )]
        public virtual void GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.SetType( typeof( ObjectMatrix<T> ) );

            info.AddValue( "RowCount", this.rowCount );
            info.AddValue( "ColumnCount", this.columnCount );
            info.AddValue( "Data", data, data.GetType() );
        }

        #endregion

        #region GetEnumerator

        /// <summary>
        /// Gets an enumerator that iterates over all elements of the <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for( int i = 0; i < this.data.Length; ++i )
            {
                yield return this.data[i];
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates over all elements of the <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <returns>An enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Returns the hash code of this <see cref="ObjectMatrix{T}"/> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.rowCount );
            hashBuilder.AppendStruct( this.columnCount );

            for( int i = 0; i < this.data.Length; ++i )
            {
                hashBuilder.Append( this.data[i] );
            }

            return hashBuilder.GetHashCode();
        }

        /// <summary>
        /// Returns a humen-readable representation of the <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <returns>A string representing this ObjectMatrix.</returns>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder( rowCount * columnCount * 5 );

            for( int row = 0; row < rowCount; ++row )
            {
                sb.Append( '|' );
                for( int column = 0; column < columnCount; ++column )
                {
                    sb.Append( this[row, column].ToString() );

                    if( column != (columnCount - 1) )
                        sb.Append( ' ' );
                }

                sb.Append( '|' );
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates a clone of this ObjectMatrix{T}.
        /// </summary>
        /// <returns>
        /// The cloned ObjectMatrix{T}.
        /// </returns>
        [Pure]
        public ObjectMatrix<T> Clone()
        {
            // Contract.Ensures( Contract.Result<ObjectMatrix<T>>() != null );
            // Contract.Ensures( Contract.Result<ObjectMatrix<T>>().RowCount == this.RowCount );
            // Contract.Ensures( Contract.Result<ObjectMatrix<T>>().ColumnCount == this.ColumnCount );

            return new ObjectMatrix<T>( this );
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The number of columns in the matrix.
        /// </summary>
        private int columnCount;

        /// <summary>
        /// The number of rows in the matrix.
        /// </summary>
        private int rowCount;

        /// <summary>
        /// The data of the matrix.
        /// </summary>
        private T[] data;

        #endregion
    }
}
