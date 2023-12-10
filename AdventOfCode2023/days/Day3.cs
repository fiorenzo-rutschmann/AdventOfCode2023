using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Day3 : AdventOfCodeTestBase
    {
        public Day3(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("./test/day3.txt", 4361)]
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
        [InlineData("./test/day3.txt", 467835)]
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

    }
}
