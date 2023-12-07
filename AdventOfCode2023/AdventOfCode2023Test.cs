using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class AdventOfCode2023Test
    {
        private readonly ITestOutputHelper _log;

        public AdventOfCode2023Test(ITestOutputHelper testOutputHelper)
        {
            _log = testOutputHelper;
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

        [Theory]
        [InlineData("./input/day2test.txt", 8)]
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
        [InlineData("./input/day2test.txt", 2286)]
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

        [Theory]
        [InlineData("./input/day3test.txt", 4361)]
        [InlineData("./input/day3.txt", 537732)]
        public void Day3Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            int result = 0;
            int lineIndex = -1;

            int NoParts = 0;
            int NoNotParts = 0;

            foreach (var line in lines)
            {
                lineIndex++;

                int startPosition = -1;
                int index = -1;
                bool isPart = false;
                foreach (char character in line)
                {
                    index++;
                    if (Is0to9(character))
                    {
                        if (!isPart)
                        {
                            isPart = CheckForNeighbouringSymbols(lines, lineIndex, index);
                        }

                        if (startPosition == -1)
                        {
                            startPosition = index;
                        }
                    }
                    else
                    {
                        if (startPosition > -1 && isPart)
                        {
                            var number = int.Parse(line.Substring(startPosition, index - startPosition));
                            result += number;

                            NoParts++;

                        }
                        else if (startPosition > -1)
                        {
                            NoNotParts++;
                        }

                        //reset
                        startPosition = -1;
                        isPart = false;
                    }
                }

                //if number reaches the end of the line
                if (startPosition > -1 && isPart)
                {
                    var number = int.Parse(line.Substring(startPosition, index + 1 - startPosition));
                    result += number;
                }
            }

            //count parts
            string inputText = File.ReadAllText(input);
            Regex rgx = new Regex("[^0-9][0-9]+[^0-9]");
            var matches = rgx.Matches(inputText);

            _log.WriteLine($"parts: {NoParts} not: {NoNotParts} count: {matches.Count}");
            Assert.Equal(expected, result);
        }

        private bool CheckForNeighbouringSymbols(string[] lines, int index, int position)
        {
            return

            //topleft
            HasSymbol(lines, index - 1, position - 1)

            //top

            || HasSymbol(lines, index - 1, position)

            //bottomleft

            || HasSymbol(lines, index + 1, position - 1)

            //bottom

            || HasSymbol(lines, index + 1, position)

            //left 

            || HasSymbol(lines, index, position - 1)

            //top right

            || HasSymbol(lines, index - 1, position + 1)

            //bottomright

            || HasSymbol(lines, index + 1, position + 1)

            //right

            || HasSymbol(lines, index, position + 1);

        }

        private bool HasSymbol(string[] lines, int index, int position)
        {
            if (index > 0 && position > 0 && index < lines.Length && position < lines[0].Length)
            {
                var ret = !Char.IsLetterOrDigit(lines[index][position]) && lines[index][position] != '.';
                return ret;
            }

            return false;
        }

        private bool Is0to9(char character)
        {
            return character >= '0' && character <= '9';
        }

        [Theory]
        [InlineData("./input/day3test.txt", 467835)]
        [InlineData("./input/day3.txt", 84883664)]
        public void Day3Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            int result = 0;

            for (int index = 0; index < lines.Length; index++)
            {
                string line = lines[index];
                for (int position = 0; position < line.Length; position++)
                {
                    //check for candidate
                    if (line[position] == '*')
                    {
                        _log.WriteLine($"* {index}:{position}");

                        List<int> numbers = new List<int>();

                        //line above
                        if (HasNumber(lines, index - 1, position - 1) || HasNumber(lines, index - 1, position) || HasNumber(lines, index - 1, position + 1))
                        {
                            if (HasNumber(lines, index - 1, position - 1))
                            {
                                numbers.Add(GetLineNumber(lines, index - 1, position - 1));
                            }
                            if (HasNumber(lines, index - 1, position))
                            {
                                numbers.Add(GetLineNumber(lines, index - 1, position));
                            }
                            if (HasNumber(lines, index - 1, position + 1))
                            {
                                numbers.Add(GetLineNumber(lines, index - 1, position + 1));
                            }
                        }

                        //currentline
                        if (HasNumber(lines, index, position - 1) || HasNumber(lines, index, position + 1))
                        {
                            if (HasNumber(lines, index, position - 1))
                            {
                                numbers.Add(GetLineNumber(lines, index, position - 1));
                            }
                            if (HasNumber(lines, index, position + 1))
                            {
                                numbers.Add(GetLineNumber(lines, index, position + 1));
                            }
                        }

                        //line below
                        if (HasNumber(lines, index + 1, position - 1) || HasNumber(lines, index + 1, position) || HasNumber(lines, index + 1, position + 1))
                        {
                            if (HasNumber(lines, index + 1, position - 1))
                            {
                                numbers.Add(GetLineNumber(lines, index + 1, position - 1));
                            }
                            if (HasNumber(lines, index + 1, position))
                            {
                                numbers.Add(GetLineNumber(lines, index + 1, position));
                            }
                            if (HasNumber(lines, index + 1, position + 1))
                            {
                                numbers.Add(GetLineNumber(lines, index + 1, position + 1));
                            }
                        }


                        if (numbers.Distinct().Count() == 2)
                        {
                            var x = numbers.Distinct().ToArray();

                            result += x[0] * x[1];
                        }

                    }
                }
            }

            Assert.Equal(expected, result);
        }

        private bool HasNumber(string[] lines, int index, int position)
        {
            if (index >= 0 && position >= 0 && index < lines.Length && position < lines[0].Length)
            {
                var ret = Char.IsDigit(lines[index][position]) && lines[index][position] != '.';
                return ret;
            }

            return false;
        }

        private int GetLineNumber(string[] lines, int index, int position)
        {
            string line = lines[index];

            //start from known number position
            int middle = position;

            int left = position;
            int right = position;

            while (left > 0 && char.IsDigit(line[left - 1]))
            {
                left--;
            }

            while (right < line.Length - 1 && char.IsDigit(line[right + 1]))
            {
                right++;
            }

            int result = int.Parse(line[left..(right + 1)]);

            _log.WriteLine($" {result} ");
            return result;
        }


        [Theory]
        [InlineData("./input/day4test.txt", 13)]
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

                var lottoNumbers = line[(startIndex+1)..middleIndex].Split(" ").Where(n => n.Length > 0).Select(n => int.Parse(n)).ToList();
                var numbers = line[(middleIndex+1)..].Split(" ").Where(n => n.Length > 0).Select(n => int.Parse(n)).ToList();

                int count = 0;
                foreach(var number in numbers)
                {
                    if (lottoNumbers.Contains(number))
                    {
                        count++;
                    }
                }

                if (count > 0)
                {
                    result += 1 << count-1;
                }
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./input/day4test.txt", 30)]
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

                for (int i = index+1; i < copies.Length && i < index+count+1 ; i++)
                {
                    copies[i] += copies[index];
                }
            }

            result = copies.Sum();

            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("./input/day5test.txt", 35)]
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
                    foreach(DestSrcRge destsrc in arr)
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

            _log.WriteLine(String.Join(",",seedline));

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
                if (number >= source && number < source+range)
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
        [InlineData("./input/day5test.txt", 46)]
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

        [Theory]
        [InlineData("./input/day6test.txt", 288)]
        [InlineData("./input/day6.txt", 1660968)]
        public void Day6Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);

            //read durations
            int[] durations = lines[0][(lines[0].IndexOf(':')+1)..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            int[] distance = lines[1][(lines[1].IndexOf(':')+1)..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
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
            foreach(int b in beats)
            {
                result *= b;
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./input/day6test.txt", 71503)]
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