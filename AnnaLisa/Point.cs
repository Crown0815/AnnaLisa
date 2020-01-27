namespace AnnaLisa
{
    public struct Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }
        
        public static implicit operator Point((double, double) tuple) => new Point(tuple.Item1, tuple.Item2);
    }
}