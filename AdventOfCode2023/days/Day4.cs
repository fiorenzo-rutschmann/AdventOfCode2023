using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Day4 : AdventOfCodeTestBase
    {
        public Day4(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("./test/day4.txt", 13)]
        [InlineData("./input/day4.txt", 22897)]
        public void Day4Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            int result = 0;

            for (int index = 0; index < lines.Length; index++)
            {
                string line = lines[index].Replace("  ", " ");
                int startIndex = line.IndexOf(':');
                int middleIndex = line.IndexOf('|');

                var lottoNumbers = line[(startIndex + 1)..middleIndex].Split(" ").Where(n => n.Length > 0).Select(n => int.Parse(n)).ToList();
                var numbers = line[(middleIndex + 1)..].Split(" ").Where(n => n.Length > 0).Select(n => int.Parse(n)).ToList();

                int count = 0;
                foreach (var number in numbers)
                {
                    if (lottoNumbers.Contains(number))
                    {
                        count++;
                    }
                }

                if (count > 0)
                {
                    result += 1 << count - 1;
                }
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day4.txt", 30)]
        [InlineData("./input/day4.txt", 5095824)]
        public void Day4Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);
            int[] copies = new int[lines.Length];

            //populate original cards
            for (int i = 0; i < copies.Length; i++)
            {
                copies[i] = 1;
            }

            int result = 0;

            for (int index = 0; index < lines.Length; index++)
            {
                string line = lines[index].Replace("  ", " ");
                int startIndex = line.IndexOf(':');
                int middleIndex = line.IndexOf('|');

                var lottoNumbers = line[(startIndex + 1)..middleIndex].Split(" ").Where(n => n.Length > 0).Select(n => int.Parse(n)).ToList();
                var numbers = line[(middleIndex + 1)..].Split(" ").Where(n => n.Length > 0).Select(n => int.Parse(n)).ToList();

                int count = 0;
                foreach (var number in numbers)
                {
                    if (lottoNumbers.Contains(number))
                    {
                        count++;
                    }
                }

                for (int i = index + 1; i < copies.Length && i < index + count + 1; i++)
                {
                    copies[i] += copies[index];
                }
            }

            result = copies.Sum();

            Assert.Equal(expected, result);
        }
    }
}
