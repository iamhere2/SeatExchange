using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SeatExchange
{
    public class Program
    {
        static void Main(string[] args)
        {
            TestGetAllAdjacentPairs();

            Тест_на_количество_групп(fixedRandom : true);
        }

        private static void TestGetAllAdjacentPairs()
        {
            var seats = new[] { new Seat(1, 1), new Seat(1, 2), new Seat(1, 3), new Seat(1, 4) };
            var pairs = GetAllAdjacentPairs(seats);
            Debug.Assert(pairs.Count == 3, $"Должно быть 3 пары, а найдено {pairs.Count}");
        }

        /// <summary>
        /// Проверяем, сколько будет групп размещения при случайном расположении мест.
        /// Самый простой вариант - запросы по двое.
        /// </summary>
        private static void Тест_на_количество_групп(bool fixedRandom)
        {
            // Тестовый набор запросов при рассадке 43 x 7
            var requests = CreateRandomPairsRequestSet(43, 7, 150, fixedRandom ? 0 : (int)DateTime.Now.Ticks);

            // Выведем все запросы
            Console.WriteLine($"Requests generated ({requests.Count}):");
            Print(requests);
            Console.WriteLine();

            // Определим все группы смежности по два в этих запросах
            var pairs = GetAllAdjacentPairs(GetAllSeats(requests));
            Console.WriteLine($"Adjacent pairs ({pairs.Count}):");
            Print(pairs);

            Console.WriteLine();
            Console.WriteLine("Press Enter...");
            Console.ReadLine();
        }

        private static IList<Seat> GetAllSeats(List<SeatSet> requests)
        {
            return
                requests.SelectMany(ss => ss.Seats).ToList();
        }

        /// <summary>Вычисляет все возможные группы смежности по два в заданном наборе мест</summary>
        private static IList<SeatSet> GetAllAdjacentPairs(IList<Seat> sourceSeats)
        {
            // Отсортируем по порядку
            var seats = new List<Seat>(sourceSeats);
            seats.Sort();

            // Берем места по очереди, если образуется смежная пара - добавляем в список
            var adjacentPairs = new List<SeatSet>();
            if (seats.Count >= 2)
            {
                Seat s1, s2;
                s2 = seats[0];
                var ndx = 1;

                while (ndx < seats.Count)
                {
                    s1 = s2;
                    s2 = seats[ndx];

                    if (s1.IsAdjacent(s2))
                        adjacentPairs.Add(new SeatSet(s1, s2));

                    ndx++;
                }
            }

            return adjacentPairs;
        }

        private static void Print(IList<SeatSet> requests)
        {
            foreach (var r in requests)
                Console.WriteLine(r.ToString());
        }

        private static List<SeatSet> CreateRandomPairsRequestSet(byte rows, byte cols, int requestCount, int seed = 0)
        {
            var rnd = new Random(seed);

            var freeSeats = new List<Seat>();
            for (byte r = 1; r <= rows; r++)
            {
                for (byte c = 1; c <= cols; c++)
                    freeSeats.Add(new Seat(r, c));
            }

            Seat TakeRandomFreeSeat()
            {
                var ndx = rnd.Next(0, freeSeats.Count);
                var s = freeSeats[ndx];
                freeSeats.RemoveAt(ndx);
                return s;
            }

            var requests = new List<SeatSet>();
            for (int i = 0; i < requestCount; i++)
                requests.Add(new SeatSet(TakeRandomFreeSeat(), TakeRandomFreeSeat()));

            return requests;
        }
    }
}
