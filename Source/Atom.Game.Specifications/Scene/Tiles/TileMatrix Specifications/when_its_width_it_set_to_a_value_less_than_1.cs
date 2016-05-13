
namespace Atom.Scene.Tiles.Specifications.TileMatrixSpecifications
{
    using Atom.Math;
    using Machine.Specifications;
    using Machine.Specifications.Contrib.Behaviours;

    [Subject( typeof( TileMatrix ) )]
    public class when_its_width_it_set_to_a_value_less_than_1
        : ArgumentExceptionThrownBehaviour
    {
        static TileMatrix matrix;

        Establish context = () =>
            matrix = new TileMatrix();

        Because of = () =>
            exception = Catch.Exception(
                () => matrix.Size = new Point2( 0, matrix.Size.Y ) 
            );

        Behaves_like<ArgumentExceptionThrownBehaviour> an_exception_thrower;
    }
}
