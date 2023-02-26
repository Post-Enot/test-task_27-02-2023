using System;
using System.Collections.Generic;
using Unigine;

namespace TestTask
{
    [Component(PropertyGuid = "f9f9dbe9f57ef434224212030f420f86d90f212d")]
    public sealed class CommandQueueExecutor : Component
    {
        public bool IsExecuted { get; private set; }

        public event Action ExecutionStarted;
        public event Action ExecutionStopped;
        public event Action ExecutionCompleted;

        private Command ExecutionCommand => _commandQueue[_executionCommandIndex];

        private IReadOnlyList<Command> _commandQueue;
        private int _executionCommandIndex;

        public void StartExecution(IReadOnlyList<Command> commandQueue)
        {
            if (IsExecuted)
            {
                throw new InvalidOperationException();
            }

            _executionCommandIndex = 0;
            _commandQueue = commandQueue;
            IsExecuted = true;
            ExecutionStarted?.Invoke();

            if (_commandQueue.Count == 0)
            {
                IsExecuted = false;
                ExecutionCompleted?.Invoke();
            }
            else
            {
                InitExecutionCommand();
            }
        }

        public void StopExecution()
        {
            _executionCommandIndex = 0;
            _commandQueue = null;
            IsExecuted = false;
            ExecutionStopped?.Invoke();
        }

        private void Update()
        {
            if (IsExecuted)
            {
                bool isExecuteComplete = ExecutionCommand.UpdateExecution();
                if (isExecuteComplete)
                {
                    ExecutionCommand.CompleteExecution();
                    _executionCommandIndex += 1;
                    if (_executionCommandIndex >= _commandQueue.Count)
                    {
                        CompleteExecution();
                    }
                    else
                    {
                        InitExecutionCommand();
                    }
                }
            }
        }

        private void CompleteExecution()
        {
            _executionCommandIndex = 0;
            _commandQueue = null;
            IsExecuted = false;
            ExecutionCompleted?.Invoke();
        }

        private void InitExecutionCommand()
        {
            if (ExecutionCommand == null)
            {
                throw new NullReferenceException();
            }
            ExecutionCommand.StartExecution();
        }
    }
}
