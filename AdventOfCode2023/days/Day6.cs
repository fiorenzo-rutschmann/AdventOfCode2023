using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Day6 : AdventOfCodeTestBase
    {
        public Day6(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }
        
        [Theory]
        [InlineData("./test/day6.txt", 288)]
        [InlineData("./input/day6.txt", 1660968)]
        public void Day6Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            //read durations
            int[] durations = lines[0][(lines[0].IndexOf(':') + 1)..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            int[] distance = lines[1][(lines[1].IndexOf(':') + 1)..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            List<int> beats = new List<int>();

            for (int dur = 0; dur < durations.Length; dur++)
            {
                int duration = durations[dur];
                int beat = 0;
                for (int i = 0; i <= duration; i++)
                {
                    int dist = i * (duration - i);

                    if (dist > distance[dur])
                    {
                        beat++;
                    }
                }

                beats.Add(beat);
            }

            int result = 1;
            foreach (int b in beats)
            {
                result *= b;
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day6.txt", 71503)]
        [InlineData("./input/day6.txt", 26499773)]
        public void Day6Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            //read durations
            long durations = long.Parse(lines[0][(lines[0].IndexOf(':') + 1)..].Replace(" ", ""));
            long distance = long.Parse(lines[1][(lines[1].IndexOf(':') + 1)..].Replace(" ", ""));
            List<long> beats = new List<long>();

            long duration = durations;
            int beat = 0;
            for (long i = 0; i <= duration; i++)
            {
                long dist = i * (duration - i);

                if (dist > distance)
                {
                    beat++;
                }
            }

            beats.Add(beat);

            long result = 1;
            foreach (long b in beats)
            {
                result *= b;
            }

            Assert.Equal(expected, result);
        }


    }
}
