using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Day9 : AdventOfCodeTestBase
    {
        public Day9(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("./test/day9.txt", 114)]
        [InlineData("./input/day9.txt", 1939607039)]
        public void Day9Part1(string input, long expected)
        {
            long result = 0;

            string[] lines = File.ReadAllLines(input);

            foreach (var line in lines)
            {
                List<List<int>> triangle = new List<List<int>>();

                var triangleLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();

                //generate lines
                while (!triangleLine.All(a => a == 0))
                {
                    result += triangleLine.Last();

                    List<int> nextLine = new List<int>();

                    int last = triangleLine.First();
                    foreach(int i in triangleLine.Skip(1))
                    {
                        nextLine.Add(i - last);
                        last = i;
                    }

                    triangleLine = nextLine;
                }
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day9.txt", 2)]
        [InlineData("./input/day9.txt", 1041)]
        public void Day9Part2(string input, long expected)
        {
            long result = 0;

            string[] lines = File.ReadAllLines(input);

            foreach (var line in lines)
            {
                List<List<int>> triangle = new List<List<int>>();

                var triangleLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();

                List<int> firsts = new List<int>();

                //generate lines
                while (!triangleLine.All(a => a == 0))
                {
                    firsts.Add(triangleLine.First());

                    List<int> nextLine = new List<int>();
                    
                    int last = triangleLine.First();
                    foreach (int i in triangleLine.Skip(1))
                    {
                        nextLine.Add(i - last);
                        last = i;
                    }

                    
                    triangleLine = nextLine;
                }

                int lastFirstNumber = 0;
                firsts.Reverse();
                foreach (int i in firsts)
                {
                    lastFirstNumber = i - lastFirstNumber;
                }

                result += lastFirstNumber;
            }

            Assert.Equal(expected, result);
        }
    }
}
