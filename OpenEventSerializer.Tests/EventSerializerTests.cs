using System;
using System.IO;
using System.Text;
using Xunit;

namespace OpenEventSerializer.Tests
{
    public class EventSerializerTests
    {
        StringBuilder consoleOutput = new StringBuilder();

        EventSerializer eventSerializer;

        public EventSerializerTests()
        {
            eventSerializer = new EventSerializer();

            eventSerializer.AddAction("print", (string text) => {
                consoleOutput.AppendLine(text);
            });
        }

        [Fact]
        public void PrintTest()
        {
            string json = @"
            [
                {
                    ""type"": ""action"",
                    ""target"": ""print"",
                    ""arguments"": [
                        ""hello world""
                    ],
                },
            ]
            ";
            eventSerializer.LoadScript("main", json);
            for(var enumerator = eventSerializer.StartScript("main"); enumerator.MoveNext();)
                eventSerializer.Handle(enumerator);

            Assert.Equal("hello world\n", consoleOutput.ToString());
        }

        [Fact]
        public void JumpTest()
        {
            string json = @"
            [
                {
                    ""type"": ""jump"",
                    ""target"": ""hello_world"",
                },
                {
                    ""id"": ""hello_another"",
                    ""type"": ""action"",
                    ""target"": ""print"",
                    ""arguments"": [
                        ""hello another""
                    ],
                },
                {
                    ""type"": ""end"",
                },
                {
                    ""id"": ""hello_world"",
                    ""type"": ""action"",
                    ""target"": ""print"",
                    ""arguments"": [
                        ""hello world""
                    ],
                },
                {
                    ""type"": ""jump"",
                    ""target"": ""hello_another"",
                },
            ]
            ";
            eventSerializer.LoadScript("main", json);
            for(var enumerator = eventSerializer.StartScript("main"); enumerator.MoveNext();)
            {
                eventSerializer.Handle(enumerator);
            }

            Assert.Equal("hello world\nhello another\n", consoleOutput.ToString());
        }

        [Fact]
        public void MultiScriptTest()
        {
            string json = @"
            [
                {
                    ""type"": ""jump"",
                    ""target"": ""hello_world"",
                    ""script"": ""other"",
                },
                {
                    ""id"": ""hello_another"",
                    ""type"": ""action"",
                    ""target"": ""print"",
                    ""arguments"": [
                        ""hello another"",
                    ],
                },
            ]";
            string json2 = @"
            [
                {
                    ""type"": ""end"",
                },
                {
                    ""id"": ""hello_world"",
                    ""type"": ""action"",
                    ""target"": ""print"",
                    ""arguments"": [
                        ""hello world"",
                    ],
                },
                {
                    ""type"": ""jump"",
                    ""target"": ""hello_another"",
                    ""script"": ""main"",
                },
            ]
            ";
            eventSerializer.LoadScript("main", json);
            eventSerializer.LoadScript("other", json2);
            for(var enumerator = eventSerializer.StartScript("main"); enumerator.MoveNext();)
            {
                eventSerializer.Handle(enumerator);
            }

            Assert.Equal("hello world\nhello another\n", consoleOutput.ToString());
        }
    }
}