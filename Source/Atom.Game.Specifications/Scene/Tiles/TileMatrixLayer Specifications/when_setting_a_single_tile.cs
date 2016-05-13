
namespace Atom.Scene.Tiles.Specifications.TileMatrixLayerSpecifications
{
    using System.Linq;
    using Machine.Specifications;

    [Subject( typeof( TileMatrixLayer ) )]
    public class when_setting_a_single_tile
    {
        const int X = 1;
        const int Y = 2;
        const int TileValue = 120;

        static TileMatrix matrix;
        static TileMatrixLayer layer;

        Establish context = () => {
            matrix = new TileMatrix() {
                Size = new Math.Point2( 2, 3 )
            };

            layer = matrix.AddNewLayer();
        };

        Because of = () =>
            layer[X, Y] = TileValue;

        It should_have_the_modified_tile_at_the_specified_position = () =>
            layer[X, Y].ShouldEqual( TileValue );

        It should_still_have_the_old_tiles_at_the_other_positions = () => {
            for( int x = 0; x < layer.Size.X; ++x )
            {       
                for( int y = 0; y < layer.Size.Y; ++y )
                {
                    if( x != X && y != Y )
                    {
                        layer[x, y].ShouldEqual( null );
                    }
                }         
            }
        };            
    }
}
