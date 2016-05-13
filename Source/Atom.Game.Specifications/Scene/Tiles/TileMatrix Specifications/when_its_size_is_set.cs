
namespace Atom.Scene.Tiles.Specifications.TileMatrixSpecifications
{
    using System.Linq;
    using Atom.Math;
    using Machine.Specifications;

    [Subject( typeof( TileMatrix ) )]
    public class when_its_size_is_set
    {   
        static TileMatrix matrix;
        static readonly Point2 SizeToSet = new Point2( 10, 10 );

        Establish context = () => {
            matrix = new TileMatrix();
            matrix.AddNewLayer();
            matrix.AddNewLayer();
            matrix.AddNewLayer();
        };

        Because of = () =>
            matrix.Size = SizeToSet;

        It should_have_the_new_size = () =>
            matrix.Size.ShouldEqual( SizeToSet );

        It should_only_have_layers_with_the_new_size = () =>
            matrix.Layers.All( layer => layer.Size == SizeToSet ).ShouldBeTrue();
    }
}
