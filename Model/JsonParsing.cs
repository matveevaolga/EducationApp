﻿using System.Text.Json;

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

        public TestData[] CurrentExerciseTests { get; set; }

        public JsonParsing(string ExerciseId)
        {
            string json = System.IO.File.ReadAllText("..\\..\\Datas\\Tests\\Tests.json");
            Exercises tests = JsonSerializer.Deserialize<Exercises>(json);
            TestData[] exerciseTests = (TestData[])tests.GetType().GetProperty(ExerciseId).GetValue(tests);
            this.CurrentExerciseTests = exerciseTests;
        }
    }
}
