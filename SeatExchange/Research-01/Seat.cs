using System;

namespace SeatExchange
{
    public struct Seat : IComparable<Seat>
    {
        public const byte MIN_ROW = 1;
        public const byte MAX_ROW = 100;
        public const byte MIN_COL = 1;
        public const byte MAX_COL = 100;

        public Seat(byte row, byte col)
        {
            if (row < MIN_ROW || row > MAX_ROW)
                throw new ArgumentOutOfRangeException(nameof(row));

            if (col < MIN_COL || col > MAX_COL)
                throw new ArgumentOutOfRangeException(nameof(col));

            Row = row;
            Col = col;
        }

        public byte Row { get; private set; }
        public byte Col { get; private set; }

        public static bool IsAdjacent(Seat s1, Seat s2) => (s1.Row == s2.Row) && (Math.Abs(s1.Col - s2.Col) <= 1);
        public bool IsAdjacent(Seat other) => IsAdjacent(this, other);

        public int CompareTo(Seat other)
        {
            var rc = Row.CompareTo(other.Row);
            return rc == 0 ? Col.CompareTo(other.Col) : rc;
        }

        public override bool Equals(object obj) => (obj is Seat s) && (s.Row == Row) && (s.Col == Col);
        public override int GetHashCode() => (Row << 8) ^ Col;
        public override string ToString() => $"({Row}:{Col})";
    }
}
