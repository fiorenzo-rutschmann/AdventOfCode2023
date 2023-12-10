using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Day8 : AdventOfCodeTestBase
    {
        public Day8(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("./test/day8.txt", 2)]
        [InlineData("./test/day8-2.txt", 6)]
        [InlineData("./input/day8.txt", 11567)]
        public void Day8Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            string directions = lines[0];

            Dictionary<string, (string, string)> nodes = new Dictionary<string, (string, string)>();

            foreach (string line in lines.Skip(2))
            {
                string key = line[0..3];
                string left = line[7..10];
                string right = line[12..15];
                nodes.Add(key, new(left, right));
            }

            var currentNode = "AAA";

            var found = false;
            int result = 0;
            while (!found)
            {
                foreach (var c in directions)
                {
                    if (c == 'R')
                    {
                        currentNode = nodes[currentNode].Item2;
                    }
                    else
                    {
                        currentNode = nodes[currentNode].Item1;
                    }

                    result++;

                    if (currentNode == "ZZZ")
                    {
                        found = true;
                        break;
                    }
                }
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day8.txt", 2)]
        [InlineData("./test/day8-2.txt", 6)]
        [InlineData("./input/day8.txt", 9858474970153)]
        public void Day8Part2(string input, long expected)
        {
            string[] lines = File.ReadAllLines(input);

            string directions = lines[0];

            Dictionary<string, (string, string)> nodes = new Dictionary<string, (string, string)>();

            var currentNodes = new List<string>();

            foreach (string line in lines.Skip(2))
            {
                string key = line[0..3];
                string left = line[7..10];
                string right = line[12..15];
                nodes.Add(key, new(left, right));
                if (key[2] == 'A')
                {
                    currentNodes.Add(key);
                }
            }

            List<int> ZNumbers = new List<int>();

            foreach (string cn in currentNodes)
            {
                string currentNode = cn;

                var found = false;
                int currentZ = 0;
                while (!found)
                {
                    foreach (var c in directions)
                    {
                        if (c == 'R')
                        {
                            currentNode = nodes[currentNode].Item2;
                        }
                        else
                        {
                            currentNode = nodes[currentNode].Item1;
                        }

                        currentZ++;

                        if (currentNode[2] == 'Z')
                        {

                            ZNumbers.Add(currentZ);
                            found = true;
                            break;
                        }
                    }
                }
            }

            long lcmN = ZNumbers[0];

            foreach (var number in ZNumbers.Skip(1))
            {
                lcmN = lcm(lcmN, number);
            }

            long result = lcmN;

            Assert.Equal(expected, result);
        }

        private long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }
    }
}
