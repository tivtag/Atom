
namespace Atom.Scene.Tiles.Specifications.TileMatrixSpecifications
{
    using Machine.Specifications;

    [Subject( typeof( TileMatrix ) )]
    public class when_a_new_layer_is_added
    {
        static TileMatrix matrix;
        static TileMatrixLayer layer;

        Establish context = () => {
            matrix = new TileMatrix();
        };

        Because of = () =>
            layer = matrix.AddNewLayer();

        It should_contain_the_layer = () =>
            matrix.Layers.ShouldContain( layer );
    }
}
