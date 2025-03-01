namespace Noc7c9.TheDigitalFrontier {

    public struct Empty : ITile {

        public TileType type {
            get {
                return TileType.Empty;
            }
        }

        public Coord pos { get; }

        public Empty(int x, int y) : this(new Coord(x, y)) {}
        public Empty(Coord pos) {
            this.pos = pos;
        }

    }

}
