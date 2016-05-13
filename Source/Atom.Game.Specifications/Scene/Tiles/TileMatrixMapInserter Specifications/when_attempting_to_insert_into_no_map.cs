
namespace Atom.Scene.Tiles.Specifications.TileMatrixMapInserterSpecifications
{
    using Atom.Math;
    using Machine.Specifications;
    using Machine.Specifications.Contrib.Behaviours;
    
    [Subject( typeof( TileMatrixMapInserter ) )]
    public class when_attempting_to_insert_into_no_map : ArgumentNullExceptionThrownBehaviour
    {
        static TileMatrixMapInserter inserter;

        Establish context = () =>
            inserter = new TileMatrixMapInserter();


        Because of = () =>
            exception = Catch.Exception( 
                () => inserter.InsertAt( new Point2( 2, 2 ), new TileMatrix(), null )
            );

        Behaves_like<ArgumentNullExceptionThrownBehaviour> exception_thrower;
    }
}
