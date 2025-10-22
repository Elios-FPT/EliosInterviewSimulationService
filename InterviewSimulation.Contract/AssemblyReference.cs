using System.Reflection;

namespace InterviewSimulation.Contract
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
