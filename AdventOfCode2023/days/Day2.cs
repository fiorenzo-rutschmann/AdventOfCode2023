using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Day2 : AdventOfCodeTestBase
    {
        public Day2(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("./test/day2.txt", 8)]
        [InlineData("./input/day2.txt", 2727)]
        public void Day2Part1(string input, int expected)
        {
            var colours = new Dictionary<string, int>()
            {
                { "red", 12 },
                { "green", 13},
                { "blue", 14 }
            };

            string[] lines = File.ReadAllLines(input);

            Regex rgx = new Regex("(?<number>[0-9]{2}) (?<colour>[a-z]*)");

            List<int> PossibleGames = new List<int>();

            foreach (var line in lines)
            {
                bool Possible = true;
                foreach (Match match in rgx.Matches(line))
                {
                    if (int.Parse(match.Groups["number"].Value) > colours[match.Groups["colour"].Value])
                    {
                        Possible = false;
                        break;
                    }
                }

                if (Possible)
                {
                    int GameNumber = int.Parse(line.Substring("Game ".Length, line.IndexOf(':') - "Game ".Length));
                    PossibleGames.Add(GameNumber);
                }
            }

            int result = PossibleGames.Sum();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day2.txt", 2286)]
        [InlineData("./input/day2.txt", 56580)]
        public void Day2Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            Regex rgx = new Regex("(?<number>[0-9]+) (?<colour>[a-z]*)");

            int result = 0;

            foreach (var line in lines)
            {
                var colours = new Dictionary<string, int>()
                {
                    { "red", 0 },
                    { "green", 0},
                    { "blue", 0 }
                };

                foreach (Match match in rgx.Matches(line))
                {
                    int value = int.Parse(match.Groups["number"].Value);
                    string colour = match.Groups["colour"].Value;

                    if (value > colours[colour])
                    {
                        colours[colour] = value;
                    }
                }

                result += colours["red"] * colours["green"] * colours["blue"];
            }

            Assert.Equal(expected, result);
        }

    }
}
