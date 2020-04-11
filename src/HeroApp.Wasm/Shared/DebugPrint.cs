using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HeroApp.Wasm.Shared
{
    public static class DebugPrint
    {
#if DEBUG
        private static string env = "Debug";
        private static string _scopeName = string.Empty;
#endif
#if !DEBUG
        private static string env = "Release";
        private static string _scopeName = "R";
#endif
        private static string Format(string text)
        {
            var date = DateTime.Now.ToLocalTime();
            var scope = _scopeName.Length > 0 ? $"{_scopeName} " : string.Empty;
            // var timeZone = $"[{date:dd/MM/yyyy}: Utc:{TimeZoneInfo.Local.BaseUtcOffset}]\n";

            return $"[{scope}{date:HH:mm:ss} {env}] {text}";
        }
        public static void Log(string text)
        {
            Task.Run(() => Console.WriteLine(Format(text)));
        }


        public static void Log(object data)
        {

            Task.Run(() =>
            {
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });
                var text = $"\n{json}";
                Console.WriteLine(Format(text));
            });
            Console.WriteLine("FIM");
        }

        public static void CreateScope(string scopeName)
        {
            _scopeName = scopeName;
            
        }
        public static void EndScope()
        {
            _scopeName = string.Empty;
        }


    }
}
