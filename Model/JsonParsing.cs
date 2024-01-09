using System.Text.Json;

namespace FormProject.Model
{
    public class JsonParsing
    {
        public class TestData
        {
            public string[] Test { get; set; }
            public string Answer { get; set; }
        }

        class Exercises
        {
            public TestData[] Exercise4 { get; set; }
            public TestData[] Exercise5 { get; set; }
            public TestData[] Exercise6 { get; set; }

        }

        public static TestData[] ParseExercise(string ExerciseId)
        {
            string json = System.IO.File.ReadAllText("..\\..\\Datas\\Tests\\Tests.json");
            Exercises tests = JsonSerializer.Deserialize<Exercises>(json);
            TestData[] exerciseTests = (TestData[])tests.GetType().GetProperty(ExerciseId).GetValue(tests);
            return exerciseTests;
        }

        public class ServerData
        {
            public string Server { get; set; }
            public string Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Database { get; set; }
        }

        public static ServerData ParseServer()
        {
            string json = System.IO.File.ReadAllText("..\\..\\Datas\\Access\\ServerData.json");
            ServerData serverData = JsonSerializer.Deserialize<ServerData>(json);
            return serverData;
        }
    }
}
