using System.Collections.Generic;
using System.Text.Json;
using System.Reflection;
using System;

namespace FormProject.Model
{
    public class JsonParsing
    {
        class TestData
        {
            public Dictionary<string, object> Test { get; set; }
            public string Answer { get; set; }
        }

        class Exercises
        {
            public TestData[] Exercise4 { get; set; }
            public TestData[] Exercise5 { get; set; }
        }

        public JsonParsing(string ExerciseId)
        {
            string json = System.IO.File.ReadAllText("..\\..\\Datas\\Tests\\Tests.json");
            Exercises tests = JsonSerializer.Deserialize<Exercises>(json);
            TestData[] exerciseTests = (TestData[])tests.GetType().GetProperty(ExerciseId).GetValue(tests);
        }
    }
}
