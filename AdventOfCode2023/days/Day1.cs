using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Day1 : AdventOfCodeTestBase
    {
        public Day1(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("./input/day1.txt", 55712)]
        public void Day1Part1(string input, int expected)
        {
            int result = File.ReadAllLines(input).Select(l => l.Where(a => Char.IsDigit(a))).Select(d => string.Concat(d.First(), d.Last())).Select(c => int.Parse(c)).Sum();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./input/day1.txt", 55413)]
        public void Day1Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            int result = 0;

            foreach (string line in lines)
            {
                char[] values = new char[2] { (char)0, (char)0 };

                ReadOnlySpan<char> span = line.AsSpan();

                for (int i = 0; i < span.Length; i++)
                {
                    char value = Day1Part1GetValue(span.Slice(i));

                    if (value != 0)
                    {
                        if (values[0] == 0)
                        {
                            values[0] = value;
                        }
                        else
                        {
                            values[1] = value;
                        }
                    }
                }

                if (values[1] == 0)
                {
                    values[1] = values[0];
                }

                result += int.Parse(values);
            }

            Assert.Equal(expected, result);
        }

        private char Day1Part1GetValue(ReadOnlySpan<char> slice)
        {
            if (char.IsDigit(slice[0]))
                return slice[0];

            switch (slice)
            {
                case var s when s.StartsWith("one", StringComparison.OrdinalIgnoreCase):
                    return '1';
                case var s when s.StartsWith("two", StringComparison.OrdinalIgnoreCase):
                    return '2';
                case var s when s.StartsWith("three", StringComparison.OrdinalIgnoreCase):
                    return '3';
                case var s when s.StartsWith("four", StringComparison.OrdinalIgnoreCase):
                    return '4';
                case var s when s.StartsWith("five", StringComparison.OrdinalIgnoreCase):
                    return '5';
                case var s when s.StartsWith("six", StringComparison.OrdinalIgnoreCase):
                    return '6';
                case var s when s.StartsWith("seven", StringComparison.OrdinalIgnoreCase):
                    return '7';
                case var s when s.StartsWith("eight", StringComparison.OrdinalIgnoreCase):
                    return '8';
                case var s when s.StartsWith("nine", StringComparison.OrdinalIgnoreCase):
                    return '9';
            }

            return (char)0;
        }

    }
}
