using System;

namespace TestTask
{
    public abstract class Command
    {
        public bool IsExecute { get; private set; }

        public void StartExecution()
        {
            if (IsExecute)
            {
                throw new InvalidOperationException("Невозможно выполнить операцию: команда уже выполняется.");
            }
            IsExecute = true;
            OnExecutionStart();
        }

        public bool UpdateExecution()
        {
            bool isExecuteCompleted = OnExecutionUpdate();
            return isExecuteCompleted;
        }

        public void CompleteExecution()
        {

            IsExecute = false;
            OnExecutionComplete();
        }

        public abstract void OnExecutionStart();

        public abstract bool OnExecutionUpdate();

        public abstract void OnExecutionComplete();
    }
}
