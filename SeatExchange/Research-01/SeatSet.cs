using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeatExchange
{
    public class SeatSet
    {
        public SeatSet(IList<Seat> seats)
        {
            if (seats == null)
                throw new ArgumentNullException(nameof(seats));

            Seats = new List<Seat>(seats).AsReadOnly();
        }

        public SeatSet(params Seat[] seats) : this((IList<Seat>)seats)
        {
        }

        public IList<Seat> Seats { get; private set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[ ");
            sb.Append(String.Join(", ", Seats.Select(s => s.ToString())));
            sb.Append(" ]");
            return sb.ToString();
        }
    }
}
