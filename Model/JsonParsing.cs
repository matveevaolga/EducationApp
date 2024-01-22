using FormProject.View.UserControls;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Text;
using System.Text.Json;

namespace FormProject.Model
{
    public class JsonParsing
    {
        public static void RunTestsFromJson(string inputScript, string creatorLogin,
            int idExercise, string functionName, ref string message)
        {
            try
            {
                string tests = System.IO.File.ReadAllText("..\\..\\Datas\\Tests\\Tests.txt",
                Encoding.GetEncoding("windows-1251"));
                string testScript = System.IO.File.ReadAllText("..\\..\\Datas\\" +
                "Tests\\TestUserInputScript.txt", Encoding.GetEncoding("windows-1251"));
                ScriptEngine engine = Python.CreateEngine();
                string fullScript = $"function_name=\"{functionName.Split('(')[0].Trim()}\"\n" +
                    $"all_tests={tests}\ntests_by_creator=all_tests[\"{creatorLogin}\"]\n" +
                    $"tests=tests_by_creator[{idExercise}]\n{inputScript}\n{testScript}";
                engine.Execute(fullScript);
            }
            catch (Exception ex) { message = ex.Message; }
        }

        public static void WriteExerciseToJson(string exerciseTests,
            string creatorLogin, int exerciseId)
        {
            string json = System.IO.File.ReadAllText("..\\..\\Datas\\Tests\\Tests.txt",
                Encoding.GetEncoding("windows-1251"));
            string updateJsonScript = System.IO.File.ReadAllText("..\\..\\Datas\\" +
                "Tests\\UpdateJsonScript.txt",
            Encoding.GetEncoding("windows-1251"));
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            string script = $"exerciseId={exerciseId}\nfull_json={json}\ncreator_login=\"{creatorLogin}\"" +
                $"\nexerciseTests={exerciseTests}\n{updateJsonScript}\n" +
                $"result=create_exercise(exerciseId, exerciseTests, full_json, creator_login)";
            engine.Execute(script, scope);
            string updated_json = scope.GetVariable("result");
            System.IO.File.WriteAllText("..\\..\\Datas\\Tests\\Tests.txt", updated_json,
                Encoding.GetEncoding("windows-1251"));
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
