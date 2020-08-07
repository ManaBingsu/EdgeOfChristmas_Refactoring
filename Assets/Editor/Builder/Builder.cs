using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Diagnostics;
using UnityEngine;
using System;
using System.Text;
using UnityEditor.Build.Reporting;
using System.IO.Compression;

namespace Editor.Builder
{
    [CreateAssetMenu]
    public class Builder : ScriptableObject
    {
        enum BuildArg
        {
            BuildPath = 0,
            LogPath,
            Platform,
            CompressionMode,
        }

        [SerializeField]
        List<PlatformInfo> platforms = new List<PlatformInfo>()
        {
            new PlatformInfo("win64", BuildTarget.StandaloneWindows),
            new PlatformInfo("mac", BuildTarget.StandaloneOSX),
            new PlatformInfo("linux", BuildTarget.StandaloneLinux64),
            new PlatformInfo("android", BuildTarget.Android)
        };

        [SerializeField]
        List<CompressionInfo> compressions = new List<CompressionInfo>()
        {
            new CompressionInfo("Default", BuildOptions.None),
            new CompressionInfo("LZ4", BuildOptions.CompressWithLz4),
            new CompressionInfo("LZ4HC", BuildOptions.CompressWithLz4HC)
        };


        public void BuildWithCommmandLine()
        {
#if UNITY_EDITOR
            // Parsing arguments
            string[] cmdArgs = System.Environment.GetCommandLineArgs();
            List<string> args = GetValidStrings(cmdArgs, "$");
            BuildInfo buildInfo = new BuildInfo(args);
            // Get building scene  
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            }

            BuildReport report = BuildPipeline.BuildPlayer(
                scenes,
                buildInfo.buildPath,
                buildInfo.platform,
                buildInfo.compressionMode
                );

            BuildSummary summary = report.summary;
            StringBuilder sb = new StringBuilder($"{args[0]}\n{args[1]}\n{args[2]}\n{args[3]}\n");
            if (summary.result == BuildResult.Succeeded)
            {
                sb.Append("Build succeeded: \n");
                sb.Append($"complete time: {DateTime.Now}\n");
                sb.Append($"total time: {summary.totalTime}\n");
            }
            else
            {
                sb.Append("Build failed: \n");
                sb.Append($"complete time: " + DateTime.Now + "\n");
            }
            System.IO.File.WriteAllText(buildInfo.logPath, sb.ToString(), Encoding.UTF8);
#endif
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

        class BuildInfo
        {
            public string buildPath;
            Dictionary<string, BuildTarget> buildTargets;
            Dictionary<string, CompressionMode> compressionModes;
            public BuildTarget platform;
            public BuildOptions compressionMode;
            public string logPath;

            public BuildInfo(List<string> args)
            {
                /*
                args[EBuildInfo.Platform], args[EBuildInfo.LogPath], args[EBuildInfo.CompressionMode]
                this.buildPath = args[EBuildInfo.Path];
                switch (platform)
                {
                    case "win64":
                        this.platform = BuildTarget.StandaloneWindows64;
                        break;
                    case "win32":
                        this.platform = BuildTarget.StandaloneWindows;
                        break;
                    case "linux":
                        this.platform = BuildTarget.StandaloneLinux64;
                        break;
                    case "mac":
                        this.platform = BuildTarget.StandaloneOSX;
                        break;
                    default:
                        this.platform = BuildTarget.StandaloneWindows64;
                        break;
                }

                switch (compressionMode)
                {
                    case "LZ4":
                        this.compressionMode = BuildOptions.CompressWithLz4;
                        break;
                    case "LZ4HC":
                        this.compressionMode = BuildOptions.CompressWithLz4HC;
                        break;
                    default:
                        this.compressionMode = BuildOptions.None;
                        break;
                }
                // Log
                this.logPath = logPath;*/
            }
        }
    }
}


