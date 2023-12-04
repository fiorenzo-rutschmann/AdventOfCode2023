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
            
            foreach(string line in lines)
            {
                char[] values = new char[2] { (char)0, (char)0 };

                ReadOnlySpan<char> span = line.AsSpan();

                for(int i = 0; i < span.Length; i++)
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

            foreach(var line in lines)
            {
                bool Possible = true ;
                foreach(Match match in rgx.Matches(line))
                {
                    if (int.Parse(match.Groups["number"].Value) > colours[match.Groups["colour"].Value] )
                    {
                        Possible = false ;
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
                        else if(startPosition > -1)
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
                    var number = int.Parse(line.Substring(startPosition, index+1 - startPosition));
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
                        if (HasNumber(lines, index-1, position-1) || HasNumber(lines, index-1, position) || HasNumber(lines, index-1, position+1))
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
                        if (HasNumber(lines, index, position - 1) || HasNumber(lines, index, position+1))
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

            while (left > 0 && char.IsDigit(line[left-1]))
            {
                left--;
            }

            while (right < line.Length-1 && char.IsDigit(line[right+1]))
            {
                right++;
            }

            int result = int.Parse(line[left..(right+1)]);

            _log.WriteLine($" {result} ");
            return result;
        }


    }
}