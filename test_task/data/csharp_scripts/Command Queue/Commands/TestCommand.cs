using Unigine;

namespace TestTask
{
    public sealed class TestCommand : Command
    {
        public override void OnExecutionStart()
        {
            Log.MessageLine($"Start Command Execution: {nameof(TestCommand)}.");
        }

        public override bool OnExecutionUpdate()
        {
            Log.MessageLine($"Update Command Execution: {nameof(TestCommand)}.");
            return true;
        }

        public override void OnExecutionComplete()
        {
            Log.MessageLine($"Complete Command Execution: {nameof(TestCommand)}.");
        }

        public override string ToString()
        {
            return $"Command type: {nameof(TestCommand)}";
        }
    }
}
