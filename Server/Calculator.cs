namespace Server
{
    class Calculator
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Calculator(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int SumCount(int x, int y)
        {
            return x + y;
        }
    }
}
