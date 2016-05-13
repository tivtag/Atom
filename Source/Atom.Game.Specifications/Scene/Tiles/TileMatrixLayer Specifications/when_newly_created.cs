
namespace Atom.Scene.Tiles.Specifications.TileMatrixLayerSpecifications
{
    using System.Linq;
    using Machine.Specifications;

    [Subject( typeof( TileMatrixLayer ) )]
    public class when_newly_created
    {
        static TileMatrix matrix;
        static TileMatrixLayer layer;

        Establish context = () =>
            matrix = new TileMatrix() {
                Size = new Math.Point2( 2, 3 )
            };

        Because of = () =>
            layer = matrix.AddNewLayer();

        It should_have_the_same_size_as_the_matrix_that_owns_it = () =>
            layer.Size.ShouldEqual( matrix.Size );

        It should_have_a_tile_floor_number_of_0 = () =>
            layer.TileFloorNumber.ShouldEqual( 0 );

        It should_have_a_tile_layer_number_of_0 = () =>
            layer.TileLayerNumber.ShouldEqual( 0 );

        It should_have_every_tile_unset = () =>
            layer.Data
                .All( tile => tile == null )
                .ShouldBeTrue();
    }
}
