
namespace Atom.Scene.Tiles.Specifications.TileMatrixSpecifications
{    
    using System.Linq;
    using Machine.Specifications;

    [Subject( typeof( TileMatrix ) )]
    public class when_newly_created
    {
        static TileMatrix matrix;

        Because of = () =>
            matrix = new TileMatrix();

        It should_have_no_layers = () =>
            matrix.Layers.Count().ShouldEqual( 0 );

        It should_have_a_width_greater_than_0 = () =>
            matrix.Size.X.ShouldBeGreaterThan( 0 );

        It should_have_a_height_greater_than_0 = () =>
            matrix.Size.Y.ShouldBeGreaterThan( 0 );
    }
}
