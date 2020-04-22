using System.IO;
using System.Text;
using Xunit;

namespace OpenEventSerializer.Tests
{
    public class ActionTests
    {
        StringBuilder consoleOutput = new StringBuilder();

        EventSerializer eventSerializer;

        public ActionTests()
        {
            eventSerializer = new EventSerializer();

            eventSerializer.AddAction("print", (string text) => {
                consoleOutput.AppendLine(text);
            });
        }

        [Fact]
        public void PrintHelloTest()
        {
            eventSerializer.CallAction("print", "hello world");
            Assert.Equal("hello world\n", consoleOutput.ToString());
        }
    }
}
