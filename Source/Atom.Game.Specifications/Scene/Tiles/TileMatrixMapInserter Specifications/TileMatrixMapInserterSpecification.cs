
namespace Atom.Scene.Tiles.Specifications.TileMatrixMapInserterSpecifications
{
    public abstract class TileMatrixMapInserterSpecification
    {
        protected const int MapWidth = 32;
        protected const int MapHeight = 24;

        protected static TileMap tileMap;
        protected static TileMatrixMapInserter inserter;
        protected static TileMatrix matrix;

        protected static TileMap CreateMap( int floorCount, int layerCount)
        {
            var map = new TileMap( MapWidth, MapHeight, 16, floorCount );

            for( int i = 0; i < floorCount; ++i )
            {
                map.AddFloor( initialLayerCapacity: 0 );                
            }

            return map;
        }
    }
}
