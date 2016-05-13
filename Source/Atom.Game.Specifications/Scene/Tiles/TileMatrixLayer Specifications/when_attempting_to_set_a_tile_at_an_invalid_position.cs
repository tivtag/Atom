
namespace Atom.Scene.Tiles.Specifications.TileMatrixLayerSpecifications
{
    using Machine.Specifications;
    using Machine.Specifications.Contrib.Behaviours;

    [Subject( typeof( TileMatrixLayer ) )]
    public class when_attempting_to_set_a_tile_at_an_invalid_position
        : ArgumentOutOfRangeExceptionThrownBehaviour
    {
        static TileMatrix matrix;
        static TileMatrixLayer layer;

        Establish context = () => {
            matrix = new TileMatrix() {
                Size = new Math.Point2( 2, 3 )
            };

            layer = matrix.AddNewLayer();
        };

        Because of = () =>
            exception = Catch.Exception(
                () => layer[10, 0] = 100
            );

        Behaves_like<ArgumentOutOfRangeExceptionThrownBehaviour> an_exception_thrower;
    }
}
