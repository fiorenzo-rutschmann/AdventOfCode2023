using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2023
{
    public class AdventOfCodeTestBase
    {
        protected readonly ITestOutputHelper _log;

        public AdventOfCodeTestBase(ITestOutputHelper testOutputHelper)
        {
            _log = testOutputHelper;
        }
    }
}