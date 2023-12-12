using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class Maze
    {
        public (int x, int y) start;
        public char[,] maze;
        public int[,] counts;

        public int width;
        public int height;

        public Maze(string[] lines)
        {
            width = lines[0].Length;
            height = lines.Length;

            maze = new char[height, width];
            counts = new int[height, width];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    char c = lines[i][j];
                    maze[i, j] = lines[i][j];

                    if (lines[i][j] == 'S')
                    {
                        start = (j, i);
                    }

                    counts[i, j] = int.MaxValue;
                }
            }
        }

        public int MaxCount()
        {
            int max = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int count = counts[y, x];

                    if (count != int.MaxValue && count > max)
                    {
                        max = count;
                    }
                }
            }

            return max;
        }

        public List<string> PrintCounts()
        {
            char[] display = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            List<string> ret = new List<string>();

            for (int y = 0; y < height; y++)
            {
                string line = "";
                for (int x = 0; x < width; x++)
                {
                    int count = counts[y, x];

                    if (count < display.Length)
                    {
                        line += display[count];
                    }
                    else
                    {
                        line += ' ';
                    }
                }

                ret.Add(line);
            }

            return ret;
        }

        public List<string> PrintLoop()
        {
            
            List<string> ret = new List<string>();

            for (int y = 0; y < height; y++)
            {
                string line = "";
                for (int x = 0; x < width; x++)
                {
                    if (counts[y, x] != int.MaxValue)
                    {
                        line += "*";
                    }
                    else
                    {
                        line += ' ';
                    }
                }

                ret.Add(line);
            }

            return ret;
        }


        public int GetArea()
        {
            int count = 0;

            for (int y = 0; y < height; y++)
            {
                bool open = false;
                for (int x = 0; x < width; x++)
                {
                    if (counts[y,x] != int.MaxValue)
                    {
                        open = !open;

                        counts[y, x] = 0;
                    }
                    else
                    {
                        counts[y, x] = open ? 1 : 0;
                        count += open ? 1 : 0;
                    }
                }

                open = false;
                for (int x = width - 1; x > 0; x--)
                {
                    if (counts[y, x] == 1)
                    {
                        counts[y, x] = open ? 1 : 0;
                        count -= open ? 0 : 1;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            
            return count;
        }
    }

    public class Day10 : AdventOfCodeTestBase
    {
        public Day10(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData("./test/day10.txt", 4)]
        [InlineData("./test/day10-2.txt", 8)]
        [InlineData("./input/day10.txt", 7086)]
        public void Day10Part1(string input, long expected)
        {
            string[] lines = File.ReadAllLines(input);

            Maze maze = new Maze(lines);

            Stack<(int x, int y, int count, int pX, int py)?> stack = new Stack<(int x, int y, int count, int pX, int py)?>();

            stack.Push((maze.start.x, maze.start.y, 0, 0, 0));

            (int x, int y, int count, int pX, int py)? item;

            while (stack.Any())
            {
                item = stack.Pop();
                findValidPaths(stack, maze, item.Value.x, item.Value.y, item.Value.count, item.Value.pX, item.Value.py);
            }
            
            foreach(string c in maze.PrintCounts())
            {
                _log.WriteLine(c);
            }

            long result = maze.MaxCount(); 

            Assert.Equal(expected, result);
        }

        private void findValidPaths(Stack<(int x, int y, int count, int pX, int py)?> stack, Maze maze, int x, int y, int count, int prevX, int prevY)
        {
            //if position is not out of bounds
            if (x < 0 || y < 0 || x >= maze.width || y >= maze.height )
            {
                return;
            }

            //return if count > count
            if (maze.counts[y, x] < count)
            {
                return;
            }

            //find current symbol
            char currentSymbol = maze.maze[y, x];

            //return if ./S
            if (currentSymbol == '.')
            {
                return;
            }
          
            char[] north = new char[] { '|', 'L', 'J', 'S' };
            char[] south = new char[] { '|', '7', 'F', 'S' };
            char[] east = new char[] { 'L', 'F', '-', 'S' };
            char[] west = new char[] { 'J', '7', '-', 'S' };

            //check if the pipe connects
            if (currentSymbol != 'S' && !((north.Contains(currentSymbol) && y - 1 == prevY) || (south.Contains(currentSymbol) && y + 1 == prevY) || (east.Contains(currentSymbol) && x + 1 == prevX) || (west.Contains(currentSymbol) && x - 1 == prevX)))
            {
                return;
            }

            maze.counts[y, x] = count;

            //north
            if ( north.Contains(currentSymbol) && y - 1 != prevY)
            {
                //findValidPaths(maze, x, y - 1, count + 1, x, y);
                stack.Push((x, y - 1, count + 1, x, y));

            }

            //south
            if (south.Contains(currentSymbol) && y + 1 != prevY)
            {
                //findValidPaths(maze, x, y + 1, count + 1, x, y);
                stack.Push((x, y + 1, count + 1, x, y));
            }

            //east
            if (east.Contains(currentSymbol) && x + 1 != prevX)
            {
                //findValidPaths(maze, x + 1, y, count + 1, x, y);

                stack.Push((x + 1, y, count + 1, x, y));
            }

            //west
            if (west.Contains(currentSymbol) && x - 1 != prevX)
            {
                //findValidPaths(maze, x - 1, y, count + 1, x, y);
                stack.Push((x - 1, y, count + 1, x, y));
            }
        }

        [Theory]
        [InlineData("./test/day10.txt", 1)]
        [InlineData("./test/day10-2.txt", 8)]
        [InlineData("./input/day10.txt", 6917)]
        public void Day10Part2(string input, long expected)
        {
            string[] lines = File.ReadAllLines(input);

            Maze maze = new Maze(lines);

            Stack<(int x, int y, int count, int pX, int py)?> stack = new Stack<(int x, int y, int count, int pX, int py)?>();

            stack.Push((maze.start.x, maze.start.y, 0, 0, 0));

            (int x, int y, int count, int pX, int py)? item;

            while (stack.Any())
            {
                item = stack.Pop();
                findValidPaths(stack, maze, item.Value.x, item.Value.y, item.Value.count, item.Value.pX, item.Value.py);
            }

            foreach (string c in maze.PrintLoop())
            {
                _log.WriteLine(c);
            }

            long result = maze.GetArea();

            Assert.Equal(expected, result);
        }


    }
}
