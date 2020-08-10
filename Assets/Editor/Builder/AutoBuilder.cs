using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Diagnostics;
using UnityEngine;
using System;
using System.Text;
using UnityEditor.Build.Reporting;
using System.IO.Compression;
using Debug = UnityEngine.Debug;

namespace Editor.Builder
{
    [CreateAssetMenu]
    public class AutoBuilder
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
        Dictionary<string, BuildTarget> platforms = new Dictionary<string, BuildTarget>()
        {
            { "win64", BuildTarget.StandaloneWindows64 },
            { "win32", BuildTarget.StandaloneWindows },
            { "mac", BuildTarget.StandaloneOSX },
            { "linux", BuildTarget.StandaloneLinux64 },
            { "android", BuildTarget.Android }
        };

        [SerializeField]
        Dictionary<string, BuildOptions> compressions = new Dictionary<string, BuildOptions>()
        {
            { "Default", BuildOptions.None },
            { "LZ4", BuildOptions.CompressWithLz4 },
            { "LZ4HC", BuildOptions.CompressWithLz4HC },
        };

        private string[] testArgs;

        public AutoBuilder()
        {

        }

        public AutoBuilder(string[] args)
        {
            testArgs = args;
        }

        public bool BuildGame()
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

        public bool BuildWithBuildInfo(List<string> args, BuildTarget platform, BuildOptions compressionMode)
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

        public bool MakeLog(bool isSuceeded, List<string> args, string msg)
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

        public List<string> GetValidStrings(string[] targets, string separator)
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

        public string[] GetBuildingScene()
        {
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            }
            return scenes;
        }

        [Serializable]
        class PlatformInfo
        {
            [SerializeField]
            string name;
            public string Name => name;
            [SerializeField]
            BuildTarget platform;
            public BuildTarget Platform => platform;

            public PlatformInfo(string name, BuildTarget platform)
            {
                this.name = name;
                this.platform = platform;
            }
        }

        [Serializable]
        class CompressionInfo
        {
            [SerializeField]
            string name;
            public string Name => name;
            [SerializeField]
            BuildOptions mode;
            public BuildOptions Mode => mode;

            public CompressionInfo(string name, BuildOptions mode)
            {
                this.name = name;
                this.mode = mode;
            }
        }
    }
}


