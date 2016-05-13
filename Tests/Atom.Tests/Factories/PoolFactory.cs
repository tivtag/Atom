// <copyright file="PoolFactory.cs" company="Microsoft">Copyright © Microsoft 2008</copyright>

namespace Atom.Collections.Pooling
{
    using System.Linq;
    using Microsoft.Pex.Framework;

    public static partial class PoolFactory
    {
        [PexFactoryMethod(typeof(Pool<int>))]
        public static Pool<int> Create( [PexAssumeNotNull]int[] items ) // int initialSize )
        {
            PexSymbolicValue.Minimize( items.Length );
            PexAssume.IsTrue( items.All( item => item < items.Length ) );
            PexAssume.AreDistinctValues( items );

            int index = 0;
            return Pool<int>.Create( 
                items.Length, 
                () => {
                    if( index < items.Length )
                    {
                        return items[index++];
                    }
                    else
                    {
                        return index++;
                    }
                }
            );
        }
    }
}
