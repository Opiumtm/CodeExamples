﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ReflectionBenchmark.DynamicCall;
using ReflectionBenchmark.NativeCall;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ReflectionBenchmark
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void RunScenarioSet(IScenarioSet set)
        {
            Log($"Scenario set \"{set.Name}\"");
            Log("=======================================================");
            BenchmarkResult baseline = await set.BaselineScenario.DoBenchmark();
            LogResult(set.BaselineScenario.ScenarioName, baseline);
            foreach (var scenario in set.GetScenarios())
            {
                var result = await scenario.DoBenchmark();
                LogResult(scenario.ScenarioName, result, baseline);
            }
            Log("");
        }

        private void ClearLog()
        {
            ConsoleBox.Text = "";
        }

        private void LogResult(string name, BenchmarkResult result)
        {
            double msTotal = result.Milliseconds;
            double msPerRun = msTotal/result.RunCount * 1000;
            Log($"{name}: run count = {result.RunCount}, totalTime(ms) = {result.Milliseconds}, time per run(microsec) = {msPerRun:F4}");
        }

        private void LogResult(string name, BenchmarkResult result, BenchmarkResult baseline)
        {
            double msTotal = result.Milliseconds;
            double msBaseTotal = baseline.Milliseconds;
            double msPerRun = msTotal / result.RunCount * 1000;
            double msBasePerRun = msBaseTotal / baseline.RunCount * 1000;
            double percent = msPerRun/Math.Max(msBasePerRun, 1E-10) * 100.0;
            Log($"{name}: run count = {result.RunCount}, totalTime(ms) = {result.Milliseconds}, time per run(microsec) = {msPerRun:F4}, % to baseline = {percent:F2}%");
        }

        private void Log(string message)
        {
            ConsoleBox.Text += message + "\n";
        }

        private void DynamicCall_OnClick(object sender, RoutedEventArgs e)
        {
            RunScenarioSet(new DynamicCallScenarioSet());
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            RunScenarioSet(new NativeCallScenarioSet());
        }

        private void CppVsNet_OnClick(object sender, RoutedEventArgs e)
        {
            RunScenarioSet(new NativeVsDotnetCallScenarioSet());
        }
    }
}
