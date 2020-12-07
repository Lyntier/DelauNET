using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace DelauNET.Tests
{
    public class OutputConverter : TextWriter
    {
        private ITestOutputHelper _helper;

        public OutputConverter(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        public override Encoding Encoding => Encoding.Default;

        public override void WriteLine(string message)
        {
            _helper.WriteLine(message);
        }
    }
}