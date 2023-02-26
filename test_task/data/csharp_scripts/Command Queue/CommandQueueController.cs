using System;
using System.Collections.Generic;
using Unigine;

namespace TestTask
{
    [Component(PropertyGuid = "8f1f7805c35709a0c650307ffe3b03d20875d109")]
    public sealed class CommandQueueController : Component
    {
        [ShowInEditor] private ObjectMeshDynamic _target;

        private readonly List<Command> _commandQueue = new();
        private CommandQueueExecutor _commandQueueExecutor;

        private void Init()
        {
            _commandQueueExecutor = node.GetComponent<CommandQueueExecutor>();
            _commandQueueExecutor.ExecutionStarted += OnExecutionStarted;
            _commandQueueExecutor.ExecutionStopped += OnExecutionStopped;
            _commandQueueExecutor.ExecutionCompleted += OnExecutionCompleted;

            Unigine.Console.AddCommand(
                "cq_help",
                "Displays auxiliary information about all console commands for working with the command queue.",
                ConsoleCommandHelp);
            Unigine.Console.AddCommand(
                "cq_start",
                "Starts the execution of the command queue. Clears the queue when execution completes.",
                ConsoleCommandStartExecution);
            Unigine.Console.AddCommand(
                "cq_stop",
                "Stops the execution of the command queue.",
                ConsoleCommandStopExecution);
            Unigine.Console.AddCommand(
                "cq_add_cmd",
                "Adds a command to the queue.",
                ConsoleCommandAddCommand);
            Unigine.Console.AddCommand(
                "cq_remove_cmd",
                "Removes a command from the queue by index.",
                ConsoleCommandAddCommand);
            Unigine.Console.AddCommand(
                "cq_clear",
                "Clears the command queue.",
                ConsoleCommandClearCommandQueue);
            Unigine.Console.AddCommand(
                "cq_print",
                "Outputs a command queue.",
                ConsoleCommandDisplayCommandQueue);
        }

        private void OnExecutionStarted()
        {
            Log.MessageLine("Command queue execution started.");
        }

        private void OnExecutionStopped()
        {
            _commandQueue.Clear();
            Log.MessageLine("Command queue execution stopped.");
        }

        private void OnExecutionCompleted()
        {
            _commandQueue.Clear();
            Log.MessageLine("Command queue execution completed.");
        }

        private static void ConsoleCommandHelp(int argc, string[] argv)
        {
            Log.MessageLine("===\n" +
                "Command queue console commands:\n\n" +
                "cq_help\n" +
                "Displays auxiliary information about all console commands for working with the command queue.\n" +
                "\n" +
                "cq_start\n" +
                "Starts the execution of the command queue. Clears the queue when execution completes.\n" +
                "\n" +
                "cq_stop\n" +
                "Stops the execution of the command queue.\n" +
                "\n" +
                "cq_add_cmd <command name>\n" +
                "Adds a command to the queue.\n" +
                "args:\n" +
                "<command_name> : string ( move_node, scale_node, set_material_color, test )\n" +
                "\n" +
                "cq_remove_cmd <command index>\n" +
                "Removes a command from the queue by index.\n" +
                "args:\n" +
                "<command_index> : int\n" +
                "\n" +
                "cq_print\n" +
                "Outputs a command queue.");
        }

        private void ConsoleCommandStartExecution(int argc, string[] argv)
        {
            _commandQueueExecutor.StartExecution(_commandQueue);
        }

        private void ConsoleCommandStopExecution(int argc, string[] argv)
        {
            _commandQueueExecutor.StopExecution();
        }

        private void ConsoleCommandDisplayCommandQueue(int argc, string[] argv)
        {
            if (_commandQueue.Count == 0)
            {
                Log.MessageLine($"Command Queue is empty ({_commandQueue.Count}).");
            }
            else
            {
                try
                {
                    string output = $"Command Queue ({_commandQueue.Count}): [\n";
                    for (int i = 0; i < _commandQueue.Count - 1; i += 1)
                    {
                        output += $"    {i}: {_commandQueue[i]},\n";
                    }
                    output += $"    {_commandQueue.Count - 1}: {_commandQueue[_commandQueue.Count - 1]}\n]";
                    Log.MessageLine(output);
                }
                catch (Exception exception)
                {
                    Log.MessageLine(exception.Message);
                }
            }
        }

        private void ConsoleCommandClearCommandQueue(int argc, string[] argv)
        {
            _commandQueue.Clear();
        }

        private void ConsoleCommandAddCommand(int argc, string[] argv)
        {
            try
            {
                if (argv.Length == 0)
                {
                    Log.ErrorLine("Invalid console command: you should pass args to \"cq_clear\" command.");
                }
                else
                {
                    string commandName = argv[1];
                    switch (commandName)
                    {
                        case "move_node":
                            dvec3 movement = new(
                                vx: Convert.ToDouble(argv[2]),
                                vy: Convert.ToDouble(argv[3]),
                                vz: Convert.ToDouble(argv[4]));
                            float movementDuration = Convert.ToSingle(argv[5]);
                            _commandQueue.Add(new MoveNode(_target, movement, movementDuration));
                            break;

                        case "test":
                            _commandQueue.Add(new TestCommand());
                            break;

                        case "scale_node":
                            vec3 scale = new(
                                vx: Convert.ToSingle(argv[2]),
                                vy: Convert.ToSingle(argv[3]),
                                vz: Convert.ToSingle(argv[4]));
                            float scaleDuration = Convert.ToSingle(argv[5]);
                            _commandQueue.Add(new ScaleNode(_target, scale, scaleDuration));
                            break;

                        case "set_material_color":
                            vec4 color = new(
                                vx: Convert.ToSingle(argv[2]),
                                vy: Convert.ToSingle(argv[3]),
                                vz: Convert.ToSingle(argv[4]),
                                vw: Convert.ToSingle(argv[5]));
                            _commandQueue.Add(new SetMaterialColor(_target, color));
                            break;

                        default:
                            Log.ErrorLine(
                                $"Invalid console command argument value: \"{commandName}\" command doesn't exist.");
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.ErrorLine(exception.Message);
            }
        }
    }
}
