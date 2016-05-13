// <copyright file="HatFactory.cs" company="Microsoft">Copyright © Microsoft 2008</copyright>

namespace Atom.Collections
{
    using System.Linq;
    using Atom.Math;
    using Atom.Math.Moles;
    using Microsoft.Pex.Framework;

    /// <summary>
    /// A factory for Atom.Collections.Hat`1[System.String] instances.
    /// </summary>
    public static partial class HatFactory
    {
        /// <summary>
        /// A factory for Atom.Collections.Hat`1[System.String] instances.
        /// </summary>
        [PexFactoryMethod( typeof( Hat<string> ) )]
        public static Hat<string> Create( [PexAssumeNotNull]string[] items, [PexAssumeNotNull]float[] randomValues )
        {
            // Assume
            PexAssume.AreElementsNotNull( items );
            PexAssume.IsTrue( randomValues.Length > 0 );
            PexAssume.IsTrue( randomValues.All( value => value >= 0.0f && value <= 1.0f ) );
            
            int index = 0;
            IRand rand = new SIRand() {
                RandomSingleGet = () => {
                    ++index;

                    if( index >= randomValues.Length )
                    {
                        index = 0;
                    }

                    return randomValues[index];
                }
            };        

            var hat = new Hat<string>( rand );

            foreach( var item in items )
            {
                hat.Insert( item, rand.RandomSingle * 100.0f );
            }

            return hat;
        }
    }
}
