using System.Collections.Generic;

namespace ReflectionBenchmark.BranchHypo
{
    public class BranchHypoScenarioSet : IScenarioSet
    {
        public string Name => "Branch hypothesis (if vs delegate)";

        public IScenario BaselineScenario => new IfBranchCallScenario();

        public IEnumerable<IScenario> GetScenarios()
        {
            yield return new DelegateBranchCallScenario();
        }
    }
}