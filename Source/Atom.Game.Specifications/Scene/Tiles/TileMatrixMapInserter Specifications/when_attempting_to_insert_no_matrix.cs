
namespace Atom.Scene.Tiles.Specifications.TileMatrixMapInserterSpecifications
{
    using Atom.Math;
    using Machine.Specifications;
    using Machine.Specifications.Contrib.Behaviours;

    [Subject( typeof( TileMatrixMapInserter ) )]
    public class when_attempting_to_insert_no_matrix : ArgumentNullExceptionThrownBehaviour
    {
        static TileMatrixMapInserter inserter;

        Establish context = () =>
            inserter = new TileMatrixMapInserter();


        Because of = () =>
            exception = Catch.Exception(
                () => inserter.InsertAt( new Point2( 2, 2 ), null, new TileMap() )
            );

        Behaves_like<ArgumentNullExceptionThrownBehaviour> exception_thrower;
    }
}
