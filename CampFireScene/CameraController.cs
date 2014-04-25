using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace CampFireScene
{
    /// <summary>
    /// Manages camera logic
    /// </summary>
    class CameraController
    {
        private const float ASPECT = 4.0f / 3.0f;
        private const float NEAR_CLIP = 0.1f;
        private const float FAR_CLIP = 10000.0f;

        public Matrix4 ProjectionMatrix { get; private set; }
        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 ModelMatrix = Matrix4.Identity;

        private Vector3 _position = new Vector3(1, 1, 1);
        private float _horizontalAngle = (float)Math.PI;
        private float _verticalAngle = 0.0f;
        private float _initialFoV = (float)(45.0f * (Math.PI/180.0));
        private float _speed = 2.0f;
        private float _mouseSpeed = 0.01f;
        private double _xDelta = 0.0f;
        private double _yDelta = 0.0f;
        private GameWindow _window;

        private bool _isUpPressed = false;
        private bool _isDownPressed = false;
        private bool _isRightPressed = false;
        private bool _isLeftPressed = false;

        public CameraController(GameWindow window)
        {
            _window = window;
            _window.KeyDown += _window_KeyDown;
            _window.KeyUp += _window_KeyUp;
            ProjectionMatrix = Matrix4.Identity;
            ViewMatrix = Matrix4.Identity;
        }

        private void _window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.Left:
                    _isLeftPressed = false;
                    break;
                case OpenTK.Input.Key.A:
                case OpenTK.Input.Key.D:
                    _xDelta = 0;
                    break;
                case OpenTK.Input.Key.Right:
                    _isRightPressed = false;
                    break;
                case OpenTK.Input.Key.Up:
                    _isUpPressed = false;
                    break;
                case OpenTK.Input.Key.W:
                case OpenTK.Input.Key.S:
                    _yDelta = 0;
                    break;
                case OpenTK.Input.Key.Down:
                    _isDownPressed = false;
                    break;
            }
        }

        private void _window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.A:
                    _xDelta = -1;
                    break;
                case OpenTK.Input.Key.Left:
                    _isLeftPressed = true;
                    break;
                case OpenTK.Input.Key.D:
                    _xDelta = 1;
                    break;
                case OpenTK.Input.Key.Right:
                    _isRightPressed = true;
                    break;
                case OpenTK.Input.Key.W:
                    _yDelta = 1;
                    break;
                case OpenTK.Input.Key.Up:
                    _isUpPressed = true;
                    break;
                case OpenTK.Input.Key.S:
                    _yDelta = -1;
                    break;
                case OpenTK.Input.Key.Down:
                    _isDownPressed = true;
                    break;
            }
        }

        public void Update(double deltaTime)
        {
            float deltaTimeF = (float)deltaTime;

            _horizontalAngle += (float)(_mouseSpeed * _xDelta);
            _verticalAngle += (float)(_mouseSpeed * _yDelta);

            Vector3 direction = new Vector3(
                (float)(Math.Cos(_verticalAngle) * Math.Sin(_horizontalAngle)),
                (float)Math.Sin(_verticalAngle),
                (float)(Math.Cos(_verticalAngle) * Math.Cos(_horizontalAngle))
            );

            Vector3 right = new Vector3(
                (float)(Math.Sin(_horizontalAngle - Math.PI / 2)),
                0,
                (float)(Math.Sin(_horizontalAngle - Math.PI / 2))
            );

            Vector3 up = Vector3.Cross(right, direction);

            if (_isUpPressed)
            {
                _position += direction * deltaTimeF * _speed;
            }

            if (_isDownPressed)
            {
                _position -= direction * deltaTimeF * _speed;
            }

            if (_isRightPressed)
            {
                _position += right * deltaTimeF * _speed;
            }

            if (_isLeftPressed)
            {
                _position -= right * deltaTimeF * _speed;
            }

            float FoV = _initialFoV;
            //float FoV = _initialFoV - 5 * _window.Mouse.WheelPrecise;

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FoV, ASPECT, NEAR_CLIP, FAR_CLIP);
            ViewMatrix = Matrix4.LookAt(_position, _position + direction, up);

#if DEBUG
            Console.Out.Write(string.Format("{0}, {1}, {2}\r", _verticalAngle, _horizontalAngle, _position));
#endif
        }

        public void Reset()
        {
            ProjectionMatrix = Matrix4.Identity;
            ViewMatrix = Matrix4.Identity;
            _position = new Vector3(1, 1, 1);
            _horizontalAngle = (float)Math.PI;
            _verticalAngle = 0.0f;
        }
    }
}
