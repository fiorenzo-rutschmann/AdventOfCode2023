using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Day7 : AdventOfCodeTestBase
    {
        public Day7(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("./test/day7.txt", 6440)]
        [InlineData("./input/day7.txt", 250370104)]
        public void Day7Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            //hands
            var hands = new List<Hand>();
            foreach (var line in lines)
            {
                hands.Add(new Hand(line));
            }

            //points
            hands.Sort((a, b) => a.CompareTo(b));

            Hand[] handsArray = hands.ToArray();

            var result = 0;

            for (int i = 0; i < handsArray.Length; i++)
            {
                result += handsArray[i].bid * (i + 1);
            }

            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("./test/day7.txt", 5905)]
        [InlineData("./input/day7.txt", 251735672)]
        public void Day7Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            //hands
            var hands = new List<Hand>();
            foreach (var line in lines)
            {
                hands.Add(new Hand(line));
            }

            //points
            hands.Sort((a, b) => a.CompareTo2(b));

            Hand[] handsArray = hands.ToArray();

            var result = 0;

            for (int i = 0; i < handsArray.Length; i++)
            {
                result += handsArray[i].bid * (i + 1);
            }

            Assert.Equal(expected, result);
        }

        public struct Hand
        {
            readonly char[] Cards = new char[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

            public Hand(string line)
            {
                //first 5 are hand
                this.hand = new int[5];
                this.hand[0] = Array.IndexOf(Cards, line[0]);
                this.hand[1] = Array.IndexOf(Cards, line[1]);
                this.hand[2] = Array.IndexOf(Cards, line[2]);
                this.hand[3] = Array.IndexOf(Cards, line[3]);
                this.hand[4] = Array.IndexOf(Cards, line[4]);

                this.bid = int.Parse(line[6..]);
            }

            public int[] hand;
            public int bid;

            public int GethandStrength()
            {
                Hand self = this;
                //5 of a kind
                if (hand.All(a => a == self.hand[0]))
                {
                    return 6;
                }

                var counts = new List<int>();
                foreach (var dist in hand.Distinct())
                {
                    counts.Add(hand.Count(a => a == dist));
                }

                //4 of a kind
                if (counts.Contains(4))
                {
                    return 5;
                }

                //full house
                if (counts.Contains(3) && counts.Contains(2))
                {
                    return 4;
                }

                //three of a kind
                if (counts.Contains(3))
                {
                    return 3;
                }

                //two pair
                if (counts.Where(a => a == 2).Count() == 2)
                {
                    return 2;
                }

                if (counts.Contains(2))
                {
                    return 1;
                }

                return 0;
            }

            public int GetHandStrengthWithJoker()
            {
                var NewHand = hand.Where(a => a != 9);

                Hand self = this;

                var counts = new List<int>();
                foreach (var dist in NewHand.Distinct())
                {
                    counts.Add(hand.Count(a => a == dist));
                }
                counts = counts.OrderByDescending(a => a).ToList();

                //5 Jokers
                if (!counts.Any())
                    return 6;

                counts[0] += 5 - NewHand.Count();

                //5 of a kind
                if (counts.Contains(5))
                {
                    return 6;
                }

                //4 of a kind
                if (counts.Contains(4))
                {
                    return 5;
                }

                //full house
                if (counts.Contains(3) && counts.Contains(2))
                {
                    return 4;
                }

                //three of a kind
                if (counts.Contains(3))
                {
                    return 3;
                }

                //two pair
                if (counts.Where(a => a == 2).Count() == 2)
                {
                    return 2;
                }

                if (counts.Contains(2))
                {
                    return 1;
                }

                return 0;
            }


            public int CompareTo(Hand b)
            {
                if (this.GethandStrength() > b.GethandStrength())
                {
                    return 1;
                }

                if (this.GethandStrength() < b.GethandStrength())
                {
                    return -1;
                }

                for (int i = 0; i < this.hand.Length; i++)
                {
                    if (this.hand[i] > b.hand[i])
                    {
                        return 1;
                    }

                    if (this.hand[i] < b.hand[i])
                    {
                        return -1;
                    }
                }

                return 0;
            }

            public int CompareTo2(Hand b)
            {
                if (this.GetHandStrengthWithJoker() > b.GetHandStrengthWithJoker())
                {
                    return 1;
                }

                if (this.GetHandStrengthWithJoker() < b.GetHandStrengthWithJoker())
                {
                    return -1;
                }

                for (int i = 0; i < this.hand.Length; i++)
                {
                    //joker is weakest individual card now
                    if (!(b.hand[i] == 9 && this.hand[i] == 9))
                    {
                        if (b.hand[i] == 9)
                        {
                            return 1;
                        }

                        if (this.hand[i] == 9)
                        {
                            return -1;
                        }
                    }

                    if (this.hand[i] > b.hand[i])
                    {
                        return 1;
                    }

                    if (this.hand[i] < b.hand[i])
                    {
                        return -1;
                    }
                }

                return 0;
            }

        }
    }
}
