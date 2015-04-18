using System.Collections;
using Game.Manager;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    [UsedImplicitly]
    public class Player : MonoBehaviour
    {

        [SerializeField] 
        private Vector2 _input;

        [SerializeField, UsedImplicitly] 
        private float _speed;

        [UsedImplicitly] 
        private GridManager _gridManager;

        public Vector2 TilePosition;


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
            //_input = new Vector2(0, 0);
            _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();

            _gridManager.Players.Add(this);
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
                transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(moveTime) );
                yield return null;
            }

            _state = PlayerState.Stopped;

            var previousCube = _gridManager.GetCubeAtPosition(TilePosition);

            // Update the player's TilePosition before calling DecreaseHealth on the previous cube
            // Otherwise the FindIslands algorithm will start from the dead cube instead of the new one
            TilePosition = _gridManager.GetTileCoordinatesFromWorldCoordinates(transform.position);
            previousCube.DecreaseHealth();

            transform.position = endPosition;

            CheckFallen();
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
            float endPosX = startPosition.x + GridManager.CubeScale*_input.x;
            float endPosZ = startPosition.z + GridManager.CubeScale*_input.y;

            var endPosition = new Vector3(endPosX, 1f, endPosZ);

            //TODO: Check bounds and adjust endPosition if needed (do we want to do this?)
            return endPosition;
        }
    }
}
