using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AutoBuilder
{
    public class Builder : MonoBehaviour
    {
        [MenuItem("Build/Build")]
        public static void BuildGame()
        {
            // Update Resource
            Resource resource = (Resource)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Resource.asset", typeof(Resource));
            resource.Load();
            
            // Gather values from args
            var options = ArgumentsParser.GetValidatedOptions();

            // Gather values from project
            var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(s => s.path).ToArray();

            
            // Define BuildPlayer Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = options["customBuildPath"],
                target = (BuildTarget)Enum.Parse(typeof(BuildTarget), options["buildTarget"]),
            };

            // Perform build
            BuildReport buildReport = BuildPipeline.BuildPlayer(buildOptions);

            // Summary
            BuildSummary summary = buildReport.summary;
            StdOutReporter.ReportSummary(summary);

            // Result
            BuildResult result = summary.result;
            StdOutReporter.ExitWithResult(result);
        }
    }
}

