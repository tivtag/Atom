
namespace Atom.Scene.Tiles.Specifications.TileMatrixMapInserterSpecifications
{
    using System;
    using Atom.Math;
    using Machine.Specifications;
    using Machine.Specifications.Contrib.Behaviours;

    [Subject( typeof( TileMatrixMapInserter ) )]
    public class when_inserting_at_a_y_position_greater_than_the_map_height
        : TileMatrixMapInserterSpecification
    {
        static Exception exception;

        Establish context = () => {
            tileMap = CreateMap( floorCount: 0, layerCount: 0 );
            inserter = new TileMatrixMapInserter();
            matrix = new TileMatrix();
        };

        Because of = () =>
            exception = Catch.Exception(
                () => inserter.InsertAt( new Point2( 0, MapHeight ), matrix, tileMap )
            );

        It should_have_thrown_an_exception = () =>
            exception.ShouldNotBeNull();

        It should_have_thrown_an_argument_out_of_range_exception = () =>
            exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }
}
