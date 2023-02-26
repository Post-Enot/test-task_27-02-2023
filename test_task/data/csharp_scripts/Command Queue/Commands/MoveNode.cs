using Unigine;

namespace TestTask
{
    public sealed class MoveNode : Command
    {
        public MoveNode(Node target, dvec3 movement, float duration)
        {
            _target = target;
            _movement = movement;
            _movementDuration = duration;
        }

        private readonly Node _target;
        private readonly dvec3 _movement;
        private readonly float _movementDuration;

        private dvec3 _sourcePosition;
        private dvec3 _destinationPosition;
        private float _startTime;

        public override void OnExecutionStart()
        {
            _startTime = Game.Time;
            _sourcePosition = _target.Position;
            _destinationPosition = _sourcePosition + _movement;
            Log.MessageLine($"Start command execution: move target: \"{_target.Name}\" on {_movement}.");
        }

        public override bool OnExecutionUpdate()
        {
            float timeLeft = Game.Time - _startTime;

            if (timeLeft >= _movementDuration)
            {
                _target.Position = _destinationPosition;
                return true;
            }

            float normalizedTime = timeLeft / _movementDuration;
            dvec3 deltaPosition = MathLib.Lerp(_sourcePosition, _destinationPosition, normalizedTime);
            _target.Position = deltaPosition;
            return false;
        }

        public override void OnExecutionComplete()
        {
            _startTime = 0;
            _destinationPosition = vec3.ZERO;
            _sourcePosition = vec3.ZERO;
            Log.MessageLine($"Complete command execution: move target: \"{_target.Name}\" on {_movement}.");
        }

        public override string ToString()
        {
            return $"Command type: {nameof(MoveNode)} (" +
                $"target: {_target.Name}; " +
                $"movement: {_movement}; " +
                $"duration: {_movementDuration})";
        }
    }
}
