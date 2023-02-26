using Unigine;

namespace TestTask
{
    public sealed class ScaleNode : Command
    {
        public ScaleNode(Node target, vec3 scale, float duration)
        {
            _target = target;
            _scale = scale;
            _duration = duration;
        }

        private readonly Node _target;
        private readonly vec3 _scale;
        private readonly float _duration;

        private vec3 _sourceScale;
        private vec3 _destinationScale;
        private float _startTime;

        public override void OnExecutionStart()
        {
            _startTime = Game.Time;
            _sourceScale = _target.Scale;
            _destinationScale = _sourceScale + _scale;
            Log.MessageLine($"Start command execution: {nameof(ScaleNode)}: \"{_target.Name}\" on {_scale}.");
        }

        public override bool OnExecutionUpdate()
        {
            float timeLeft = Game.Time - _startTime;

            if (timeLeft >= _duration)
            {
                _target.Scale = _destinationScale;
                return true;
            }

            float normalizedTime = timeLeft / _duration;
            vec3 deltaScale = MathLib.Lerp(_sourceScale, _destinationScale, normalizedTime);
            _target.Scale = deltaScale;
            return false;
        }

        public override void OnExecutionComplete()
        {
            _startTime = 0;
            _destinationScale = vec3.ZERO;
            _sourceScale = vec3.ZERO;
            Log.MessageLine(_target.Scale);
            Log.MessageLine($"Complete command execution: {nameof(ScaleNode)}: \"{_target.Name}\" on {_scale}.");
        }

        public override string ToString()
        {
            return $"Command type: {nameof(ScaleNode)} (" +
                $"target: {_target.Name}; " +
                $"scale: {_scale}; " +
                $"duration: {_duration})";
        }
    }
}
