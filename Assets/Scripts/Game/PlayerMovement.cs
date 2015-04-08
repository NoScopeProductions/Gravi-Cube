using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using Manager;
using UnityEngine;

namespace Game
{
    [UsedImplicitly]
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField] 
        private Vector2 _input;

        [SerializeField, UsedImplicitly] 
        private float _speed;

        [UsedImplicitly] 
        private GridManager _gridManager;

        private bool CanBeControlled { get; set; }

        private enum PlayerState
        {
            Moving, Stopped, Dead
        }

        [SerializeField]
        private PlayerState _state;

        [UsedImplicitly]
        public void Start()
        {
            CanBeControlled = true;

            _state = PlayerState.Stopped;
            _input = new Vector2(0, 0);
            _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        }

        [UsedImplicitly]
        public void Update()
        {
            if (!CanBeControlled) return;
            GetInput();

            if (_state == PlayerState.Stopped)
            {
                StartCoroutine(MovePlayer());
            }
            
        }

        private void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _input.y = 1;
                _input.x = 0;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _input.y = -1;
                _input.x = 0;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _input.y = 0;
                _input.x = -1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _input.y = 0;
                _input.x = 1;
            }
        }

        private IEnumerator MovePlayer()
        {
            _state = PlayerState.Moving;

            var startPosition = transform.position;
            var endPosition = GetTargetPosition(startPosition);

            float moveTime = 0f;
            while (moveTime < 1f)
            {
                moveTime += Time.deltaTime * _speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, moveTime);
                yield return null;
            }

            _state = PlayerState.Stopped;
            transform.position = endPosition;

            CheckFallen();

            yield return null;
        }

        private void CheckFallen()
        {
            var normalizedPos = transform.position;
            normalizedPos.y = 0f;

            if (!_gridManager.Grid.Find(cube => normalizedPos == cube.transform.position))
            {
                _state = PlayerState.Dead;
            }
        }

        private Vector3 GetTargetPosition(Vector3 startPosition)
        {
            float endPosX = startPosition.x + 3*_input.x;
            float endPosZ = startPosition.z + 3*_input.y;

            var endPosition = new Vector3(endPosX, 1f, endPosZ);

            //TODO: Check bounds and adjust endPosition if needed (do we want to do this?)
            return endPosition;
        }
    }
}
