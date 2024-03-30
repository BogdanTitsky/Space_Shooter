using System;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using InputClasses;
using Interfaces;
using MyScreen;
using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour, IMovable
    {
        private static readonly int AnimatorMove = Animator.StringToHash("Move");

        private IAnimatable _animator;
        private float _bottomBorder;

        private Camera _camera;
        private IGetSizeable _gameObjectSize;

        private IInput _iInput;
        private IInputService _iInputService;
        private float _lastYPosition;

        private float _leftBorder;
        private Vector3 _offsetDistance;
        private float _rightBorder;
        private IGetSizeable _screenBounds;
        private float _topBorder;

        private float movementSpeed = 4;

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_iInputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = _iInputService.Axis;
            }

            Vector3 newPosition = transform.position + movementVector * (movementSpeed * Time.deltaTime);

            newPosition.x = Mathf.Clamp(newPosition.x, _leftBorder, _rightBorder);
            newPosition.y = Mathf.Clamp(newPosition.y, _bottomBorder, _topBorder);

            transform.position = newPosition;
        }

        public void OnMouseDown()
        {
            _offsetDistance = MousePositionInWorld() - transform.position;
        }

        private void OnMouseDrag()
        {
            Move();
        }

        public void Init()
        {
            _camera = Camera.main;

            _iInputService = Game.InputService;
            _iInput = AllServices.Container.Single<IInput>();

            _gameObjectSize = GetComponent<IGetSizeable>();
            _animator = GetComponent<IAnimatable>();

            CurrentScreen currentScreen = AllServices.Container.Single<CurrentScreen>();
            ScreenBounds borders = currentScreen.GetBoundsForObject(_gameObjectSize);

            _leftBorder = borders.Left;
            _rightBorder = borders.Right;
            _topBorder = borders.Top;
            _bottomBorder = borders.Bottom;
        }

        public void Move()
        {
            Vector2 positionWithinScreen = LimitByScreen();
            transform.position = positionWithinScreen;
            // ForwardChecker();
        }

        private Vector3 LimitByScreen()
        {
            float limitByX = Mathf.Clamp(MousePositionInWorld().x - _offsetDistance.x, _leftBorder, _rightBorder);
            float limitByY = Mathf.Clamp(MousePositionInWorld().y - _offsetDistance.y, _bottomBorder, _topBorder);

            return new Vector3(limitByX, limitByY);
        }

        // private void ForwardChecker()
        // {
        //     float currentPosition = transform.position.y;
        //
        //     if (currentPosition > _lastYPosition)
        //     {
        //         _animator.Animator.SetTrigger(AnimatorMove);
        //     }
        //
        //     _lastYPosition = currentPosition;
        // }

        private Vector3 MousePositionInWorld()
        {
            return _camera.ScreenToWorldPoint(new Vector3(
                _iInput.Horizontal,
                _iInput.Vertical,
                _camera.WorldToScreenPoint(transform.position).z)
            );
        }
    }
}