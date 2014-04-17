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
        private const float FAR_CLIP = 100.0f;

        public Matrix4 ProjectionMatrix { get; private set; }
        public Matrix4 ViewMatrix { get; private set; }

        private Vector3 _position = new Vector3(0, 0, 5);
        private float _horizontalAngle = (float)Math.PI;
        private float _verticalAngle = 0.0f;
        private float _initialFoV = 45.0f;
        private float _speed = 3.0f;
        private float _mouseSpeed = 0.005f;
        private double _lastTime = -1;
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
        }

        void _window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.A:
                case OpenTK.Input.Key.Left:
                    _isLeftPressed = false;
                    break;
                case OpenTK.Input.Key.D:
                case OpenTK.Input.Key.Right:
                    _isRightPressed = false;
                    break;
                case OpenTK.Input.Key.W:
                case OpenTK.Input.Key.Up:
                    _isUpPressed = false;
                    break;
                case OpenTK.Input.Key.S:
                case OpenTK.Input.Key.Down:
                    _isDownPressed = false;
                    break;
            }
        }

        void _window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.A:
                case OpenTK.Input.Key.Left:
                    _isLeftPressed = true;
                    break;
                case OpenTK.Input.Key.D:
                case OpenTK.Input.Key.Right:
                    _isRightPressed = true;
                    break;
                case OpenTK.Input.Key.W:
                case OpenTK.Input.Key.Up:
                    _isUpPressed = true;
                    break;
                case OpenTK.Input.Key.S:
                case OpenTK.Input.Key.Down:
                    _isDownPressed = true;
                    break;
            }
        }

        public void Update()
        {
            if (_lastTime == -1)
                _lastTime = _window.RenderTime;
            double currentTime = _window.RenderTime;
            float deltaTime = (float)(currentTime - _lastTime);

            double xPos = _window.Mouse.X;
            double yPos = _window.Mouse.Y;

            _horizontalAngle += _mouseSpeed * (float)(_window.Size.Width / 2 - xPos);
            _verticalAngle += _mouseSpeed * (float)(_window.Size.Height / 2 - yPos);

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
                _position += direction * deltaTime * _speed;
            }

            if (_isDownPressed)
            {
                _position -= direction * deltaTime * _speed;
            }

            if (_isRightPressed)
            {
                _position += right * deltaTime * _speed;
            }

            if (_isLeftPressed)
            {
                _position -= right * deltaTime * _speed;
            }

            //float FoV = _initialFoV;
            float FoV = _initialFoV - 5 * _window.Mouse.WheelPrecise;

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FoV, ASPECT, NEAR_CLIP, FAR_CLIP);
            ViewMatrix = Matrix4.LookAt(_position, _position + direction, up);

            _lastTime = currentTime;
        }
    }
}
