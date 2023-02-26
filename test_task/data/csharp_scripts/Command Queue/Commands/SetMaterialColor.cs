using Unigine;

namespace TestTask
{
    public sealed class SetMaterialColor : Command
    {
        public SetMaterialColor(ObjectMeshDynamic target, vec4 color)
        {
            _target = target;
            _color = color;
        }

        private readonly ObjectMeshDynamic _target;
        private readonly vec4 _color;

        public override void OnExecutionStart()
        {
            Log.MessageLine($"Start execute command: set material color: ({_color}) on: {_target.Name}");
        }

        public override bool OnExecutionUpdate()
        {
            Material material = _target.GetMaterial(0);
            Material customMaterial = material.Inherit();
            customMaterial.SetParameterFloat4("albedo_color", _color);
            _target.SetMaterial(customMaterial, 0);
            return true;
        }

        public override void OnExecutionComplete()
        {
            Log.MessageLine($"Execute command complete: set material color: ({_color}) on: {_target.Name}");
        }

        public override string ToString()
        {
            return $"Command type: {nameof(SetMaterialColor)} (" +
                $"target: {_target.Name}; " +
                $"color: {_color})";
        }
    }
}
