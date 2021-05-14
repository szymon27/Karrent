using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karrent.Objects
{
    class ReservationPeriod
    {
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }

        public int TotalDays { get; set; }
        public ReservationPeriod(DateTime? begin, DateTime? end)
        {
            this.Begin = begin;
            this.End = end;
            this.TotalDays = (End - Begin).GetValueOrDefault().Days + 1;
        }
        public override string ToString()
        {
            return $"{Begin:dd-MM-yyyy} - {End:dd-MM-yyyy} ({TotalDays})";
        }

        public static bool CheckReservation(ReservationPeriod reservationPeriod, List<ReservationPeriod> list)
        {
            foreach (ReservationPeriod e in list)
            {
                if (reservationPeriod.Begin >= e.Begin && reservationPeriod.End <= e.End)
                    return false;
                if (reservationPeriod.Begin < e.Begin)
                    if (reservationPeriod.End < e.Begin) continue;
                    else return false;
                if (reservationPeriod.Begin > e.End) continue;
            }
            return true;
        }
    }
}
