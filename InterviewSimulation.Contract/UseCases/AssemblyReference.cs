using System.Reflection;

namespace InterviewSimulation.Core
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
