using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Text;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

namespace Builder
{
    public static class AutoBuilder
    {
        enum BuildArg
        {
            BuildPath = 0,
            LogPath,
            Platform,
            CompressionMode,
            END
        }

        [SerializeField]
        static Dictionary<string, BuildTarget> platforms = new Dictionary<string, BuildTarget>()
        {
            { "win64", BuildTarget.StandaloneWindows64 },
            { "win32", BuildTarget.StandaloneWindows },
            { "mac", BuildTarget.StandaloneOSX },
            { "linux", BuildTarget.StandaloneLinux64 },
            { "android", BuildTarget.Android }
        };

        [SerializeField]
        static Dictionary<string, BuildOptions> compressions = new Dictionary<string, BuildOptions>()
        {
            { "Default", BuildOptions.None },
            { "LZ4", BuildOptions.CompressWithLz4 },
            { "LZ4HC", BuildOptions.CompressWithLz4HC },
        };

        public static string[] testArgs;

        [MenuItem("Builder/Build")]
        public static bool BuildGame()
        {
#if UNITY_EDITOR
            // Parsing arguments
            string[] cmdArgs = testArgs == null ? System.Environment.GetCommandLineArgs() : testArgs;
            List<string> args = GetValidStrings(cmdArgs, "$");

            if (args.Count != (int)BuildArg.END)
            {
                Debug.Log("Build failed: Argument number exception");
                return MakeLog(false, args, "Invalid Argument number");
            }
            BuildTarget platform = 
                platforms.ContainsKey(args[(int)BuildArg.Platform]) ? platforms[args[(int)BuildArg.Platform]] : 0;
            BuildOptions compressionMode = 
                compressions.ContainsKey(args[(int)BuildArg.CompressionMode]) ? compressions[args[(int)BuildArg.CompressionMode]] : 0;
            if (platform == 0 || compressionMode == 0)
            {
                Debug.Log("Build failed: Invalid platform or compression mode");
                return MakeLog(false, args, "Invalid platform or compression mode");
            }
            else
            {
                return BuildWithBuildInfo(args, platform, compressionMode);
            }
#endif
        }
        public static bool BuildWithBuildInfo(List<string> args, BuildTarget platform, BuildOptions compressionMode)
        {
            BuildReport report = BuildPipeline.BuildPlayer(
                GetBuildingScene(),
                args[(int)BuildArg.BuildPath],
                platform,
                compressionMode
            );
            BuildSummary summary = report.summary;
            return summary.result == BuildResult.Succeeded ? MakeLog(true, args, summary.totalTime.ToString()) : MakeLog(false, args, "");
        }

        public static bool MakeLog(bool isSuceeded, List<string> args, string msg)
        {
            StringBuilder sb = new StringBuilder();
            if (isSuceeded)
            {
                sb.Append($"{args[0]}\n{args[1]}\n{args[2]}\n{args[3]}\n");
                sb.Append($"Build succeeded: \n");
                sb.Append($"complete time: {DateTime.Now}\n");
                sb.Append($"total time: {msg}\n");
            }
            else
            {
                sb.Append($"Build failed: {msg}\n");
                sb.Append($"complete time: {DateTime.Now}\n");
            }
            return isSuceeded;
        }

        public static List<string> GetValidStrings(string[] targets, string separator)
        {
            List<string> strings = new List<string>();
            foreach (string target in targets)
            {
                if (target.Contains(separator))
                {
                    string sepStr = "";
                    sepStr = target.Replace(separator, "");
                    strings.Add(sepStr);
                }
            }
            return strings;
        }

        public static string[] GetBuildingScene()
        {
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            }
            return scenes;
        }
    }
}
