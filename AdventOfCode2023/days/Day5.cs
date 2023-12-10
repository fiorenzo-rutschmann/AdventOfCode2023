using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Day5 : AdventOfCodeTestBase
    {
        public Day5(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("./test/day5.txt", 35)]
        [InlineData("./input/day5.txt", 251346198)]
        public void Day5Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            //read seeds
            string seedline = lines[0].Replace("  ", " ");
            int startIndex = seedline.IndexOf(':');
            List<long> seeds = seedline[(startIndex + 1)..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n)).ToList();
            DestSrcRge[][] destSrcRges = new DestSrcRge[][] {
                ReadValuesInto(lines, lines.ToList().IndexOf("seed-to-soil map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("soil-to-fertilizer map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("fertilizer-to-water map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("water-to-light map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("light-to-temperature map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("temperature-to-humidity map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("humidity-to-location map:"))
            };

            var results = new List<long>();

            foreach (long seed in seeds)
            {
                long current = seed;

                foreach (var arr in destSrcRges)
                {
                    foreach (DestSrcRge destsrc in arr)
                    {
                        if (destsrc.InRange(current, out current))
                        {
                            break;
                        }
                    }
                }

                results.Add(current);
                _log.WriteLine($"{seed}:{current}");
            }

            _log.WriteLine(String.Join(",", seedline));

            long result = results.Min();

            Assert.Equal(expected, result);
        }

        public struct DestSrcRge
        {
            public DestSrcRge(long[] longs)
            {
                this.dest = longs[0];
                this.source = longs[1];
                this.range = longs[2];
            }

            long dest;
            long source;
            long range;

            public bool InRange(long number, out long output)
            {
                if (number >= source && number < source + range)
                {
                    output = dest + (number - source);
                    return true;
                }
                output = number;
                return false;
            }
        }

        public DestSrcRge[] ReadValuesInto(string[] lines, int index)
        {
            List<DestSrcRge> ret = new List<DestSrcRge>();
            index++;
            while (index < lines.Length && lines[index].Length > 1)
            {
                string line = lines[index];

                ret.Add(new DestSrcRge(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n)).ToArray()));

                index++;
            };

            return ret.ToArray();
        }


        [Theory]
        [InlineData("./test/day5.txt", 46)]
        [InlineData("./input/day5.txt", 72263011)]
        public void Day5Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            //read seeds
            string seedline = lines[0].Replace("  ", " ");
            int startIndex = seedline.IndexOf(':');
            long[] SeedlineNumbers = seedline[(startIndex + 1)..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n)).ToArray();


            long result = 1000 * 1000 * 1000;

            List<long> seeds = new List<long>();

            DestSrcRge[][] destSrcRges = new DestSrcRge[][] {
                ReadValuesInto(lines, lines.ToList().IndexOf("seed-to-soil map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("soil-to-fertilizer map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("fertilizer-to-water map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("water-to-light map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("light-to-temperature map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("temperature-to-humidity map:"))
                ,ReadValuesInto(lines, lines.ToList().IndexOf("humidity-to-location map:"))
            };

            var results = new List<long>();

            foreach (long seed in GetNextSeed(SeedlineNumbers))
            {
                long current = seed;

                foreach (var arr in destSrcRges)
                {
                    foreach (DestSrcRge destsrc in arr)
                    {
                        if (destsrc.InRange(current, out current))
                        {
                            break;
                        }
                    }
                }

                if (current < result)
                {
                    result = current;
                }
                //_log.WriteLine($"{seed}:{current}");
            }

            //_log.WriteLine(String.Join(",", seedline));

            Assert.Equal(expected, result);
        }

        public IEnumerable<long> GetNextSeed(long[] SeedlineNumbers)
        {
            for (int i = 0; i < SeedlineNumbers.Length; i += 2)
            {
                for (long j = SeedlineNumbers[i]; j < SeedlineNumbers[i] + SeedlineNumbers[i + 1]; j++)
                {
                    yield return j;
                }
            }

            yield break;
        }
    }
}
