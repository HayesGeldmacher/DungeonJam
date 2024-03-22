using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementCommand
{
    public abstract IEnumerator Execute(PlayerController player);
}

public class MoveCommand : MovementCommand
{
    private Vector3 _direction;

    public MoveCommand(Vector3 direction)
    {
        _direction = direction;
    }

    public override IEnumerator Execute(PlayerController player)
    {
        yield return player.Move(_direction);
    }
}

public class RotateCommand : MovementCommand
{
    private float _amount;

    public RotateCommand(float amount)
    {
        _amount = amount;
    }

    public override IEnumerator Execute(PlayerController player)
    {
        yield return player.Rotate(_amount);
    }
}


public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private float _moveDistance = 1;
    [SerializeField] private float _bumpDistance = .1f;
    [SerializeField] private float _moveTime = .15f;
    [SerializeField] private float _rotateTime = .15f;
    [SerializeField] private float _repeatDelay = .4f;
    [SerializeField] private float _repeatRate = .2f;

    private List<Object> _locks = new List<Object>();
    private bool _isBusy = false;
    private Queue<MovementCommand> _commandQueue = new Queue<MovementCommand>();

    private KeyCode _lastKey = KeyCode.None;
    private float _lastKeyTime = 0;
    private bool _repeating = false;
    private Dictionary<KeyCode, MovementCommand> _keyBindings = new Dictionary<KeyCode, MovementCommand>()
    {
        { KeyCode.W, new MoveCommand(Vector3.forward) },
        { KeyCode.A, new MoveCommand(Vector3.left) },
        { KeyCode.S, new MoveCommand(Vector3.back) },
        { KeyCode.D, new MoveCommand(Vector3.right) },
        { KeyCode.Q, new RotateCommand(-90) },
        { KeyCode.E, new RotateCommand(90) }
    };

    private bool CanMove(Vector3 direction)
    {
        return !Physics.Raycast(transform.position, transform.TransformVector(direction), direction.magnitude, _obstacleLayer);
    }

    public IEnumerator Move(Vector3 direction)
    {
        if (!CanMove(direction))
        {
            yield return Bump(direction);
            yield break;
        }

        Vector3 startingPos = transform.position;
        Vector3 targetPos = startingPos + transform.TransformVector(direction) * _moveDistance;
        for (float elapsedTime = 0; elapsedTime < _moveTime; elapsedTime += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / _moveTime));
            yield return new WaitForEndOfFrame();
        }
        transform.position = targetPos;
    }

    public IEnumerator Rotate(float amount)
    {
        Quaternion startingRot = transform.rotation;
        Quaternion targetRot = startingRot * Quaternion.Euler(0, amount, 0);
        for (float elapsedTime = 0; elapsedTime < _rotateTime; elapsedTime += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, (elapsedTime / _rotateTime));
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = targetRot;
    }

    private IEnumerator Bump(Vector3 direction)
    {
        Vector3 startingPos = transform.position;
        Vector3 targetPos = startingPos + transform.TransformVector(direction) * _bumpDistance;
        for (float elapsedTime = 0; elapsedTime < _moveTime/2; elapsedTime += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, (2 * elapsedTime / _moveTime));
            yield return new WaitForEndOfFrame();
        }
        for (float elapsedTime = 0; elapsedTime < _moveTime/2; elapsedTime += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(targetPos, startingPos, (2 * elapsedTime / _moveTime));
            yield return new WaitForEndOfFrame();
        }
        transform.position = startingPos;
    }

    private bool TryGetCommand(out MovementCommand triggeredCommand)
    {
        if (!Input.GetKey(_lastKey))
        {
            _repeating = false;
        }
        foreach (var key in _keyBindings.Keys)
        {
            var command = _keyBindings[key];
            if (Input.GetKeyDown(key))
            {
                triggeredCommand = command;
                _lastKeyTime = Time.time;
                if (!_repeating) {
                    _lastKey = key;
                }
                return true;
            }
            else if (Input.GetKey(key) && _lastKey == key)
            {
                if (!_repeating && Time.time - _lastKeyTime > _repeatDelay)
                {
                    _lastKeyTime = Time.time;
                    _repeating = true;
                    triggeredCommand = command;
                    return true;
                }
                else if (_repeating && Time.time - _lastKeyTime > _repeatRate)
                {
                    _lastKeyTime = Time.time;
                    triggeredCommand = command;
                    return true;
                }
            }
        }
        triggeredCommand = null;
        return false;
    }

    private void Update()
    {
        if (_locks.Count > 0)
        {
            return;
        }

        if (TryGetCommand(out MovementCommand command))
        {
            _commandQueue.Enqueue(command);
        }

        if (!_isBusy && _commandQueue.Count > 0)
        {
            StartCoroutine(ExecuteCommand(_commandQueue.Dequeue()));
        }
    }

    private IEnumerator ExecuteCommand(MovementCommand command)
    {
        _isBusy = true;
        yield return command.Execute(this);
        _isBusy = false;
    }

    public void Lock(Object lockObject)
    {
        _locks.Add(lockObject);
    }

    public void Unlock(Object lockObject)
    {
        _locks.Remove(lockObject);
    }
}
