//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;

//namespace CampFireScene
//{
//    /// <summary>
//    /// Manages camera logic
//    /// </summary>
//    class CameraController
//    {
//        private const float ASPECT = 4.0f / 3.0f;
//        private const float NEAR_CLIP = 0.1f;
//        private const float FAR_CLIP = 10000.0f;

//        public Matrix4 ProjectionMatrix { get; private set; }
//        public Matrix4 ViewMatrix { get; private set; }
//        public Matrix4 ModelMatrix = Matrix4.Identity;

//        private Vector3 _position = new Vector3(1, 1, 1);
//        private float _horizontalAngle = (float)Math.PI;
//        private float _verticalAngle = 0.0f;
//        private float _initialFoV = (float)(45.0f * (Math.PI/180.0));
//        private float _speed = 2.0f;
//        private float _mouseSpeed = 0.01f;
//        private double _xDelta = 0.0f;
//        private double _yDelta = 0.0f;
//        private GameWindow _window;

//        private bool _isUpPressed = false;
//        private bool _isDownPressed = false;
//        private bool _isRightPressed = false;
//        private bool _isLeftPressed = false;

//        public CameraController(GameWindow window)
//        {
//            _window = window;
//            _window.KeyDown += _window_KeyDown;
//            _window.KeyUp += _window_KeyUp;
//            ProjectionMatrix = Matrix4.Identity;
//            ViewMatrix = Matrix4.Identity;
//        }

//        private void _window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
//        {
//            switch (e.Key)
//            {
//                case OpenTK.Input.Key.Left:
//                    _isLeftPressed = false;
//                    break;
//                case OpenTK.Input.Key.A:
//                case OpenTK.Input.Key.D:
//                    _xDelta = 0;
//                    break;
//                case OpenTK.Input.Key.Right:
//                    _isRightPressed = false;
//                    break;
//                case OpenTK.Input.Key.Up:
//                    _isUpPressed = false;
//                    break;
//                case OpenTK.Input.Key.W:
//                case OpenTK.Input.Key.S:
//                    _yDelta = 0;
//                    break;
//                case OpenTK.Input.Key.Down:
//                    _isDownPressed = false;
//                    break;
//            }
//        }

//        private void _window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
//        {
//            switch (e.Key)
//            {
//                case OpenTK.Input.Key.A:
//                    _xDelta = -1;
//                    break;
//                case OpenTK.Input.Key.Left:
//                    _isLeftPressed = true;
//                    break;
//                case OpenTK.Input.Key.D:
//                    _xDelta = 1;
//                    break;
//                case OpenTK.Input.Key.Right:
//                    _isRightPressed = true;
//                    break;
//                case OpenTK.Input.Key.W:
//                    _yDelta = 1;
//                    break;
//                case OpenTK.Input.Key.Up:
//                    _isUpPressed = true;
//                    break;
//                case OpenTK.Input.Key.S:
//                    _yDelta = -1;
//                    break;
//                case OpenTK.Input.Key.Down:
//                    _isDownPressed = true;
//                    break;
//            }
//        }

//        public void Update(double deltaTime)
//        {
//            float deltaTimeF = (float)deltaTime;

//            _horizontalAngle += (float)(_mouseSpeed * _xDelta);
//            _verticalAngle += (float)(_mouseSpeed * _yDelta);

//            Vector3 direction = new Vector3(
//                (float)(Math.Cos(_verticalAngle) * Math.Sin(_horizontalAngle)),
//                (float)Math.Sin(_verticalAngle),
//                (float)(Math.Cos(_verticalAngle) * Math.Cos(_horizontalAngle))
//            );

//            Vector3 right = new Vector3(
//                (float)(Math.Sin(_horizontalAngle - Math.PI / 2)),
//                0,
//                (float)(Math.Sin(_horizontalAngle - Math.PI / 2))
//            );

//            Vector3 up = Vector3.Cross(right, direction);

//            if (_isUpPressed)
//            {
//                _position += direction * deltaTimeF * _speed;
//            }

//            if (_isDownPressed)
//            {
//                _position -= direction * deltaTimeF * _speed;
//            }

//            if (_isRightPressed)
//            {
//                _position += right * deltaTimeF * _speed;
//            }

//            if (_isLeftPressed)
//            {
//                _position -= right * deltaTimeF * _speed;
//            }

//            float FoV = _initialFoV;
//            //float FoV = _initialFoV - 5 * _window.Mouse.WheelPrecise;

//            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FoV, ASPECT, NEAR_CLIP, FAR_CLIP);
//            ViewMatrix = Matrix4.LookAt(_position, _position + direction, up);

//#if DEBUG
//            Console.Out.Write(string.Format("{0}, {1}, {2}\r", _verticalAngle, _horizontalAngle, _position));
//#endif
//        }
//    }
//}
// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com

using System;
using OpenTK;
using OpenTK.Input;

namespace CampFireScene
{
    /// <summary>Represents a camera to assist a game by generating a view matrix transformation.</summary>
    public class Camera
    {
        private static Vector yAxis = new Vector(0, 1, 0);

        private float _fieldOfView;

        public float _nearClipPlane = 1f;
        public float _farClipPlane = 1000000f;

        private float _positionSpeed;
        private float _lookSpeed;

        private Vector _position;
        private Vector _forward;
        private Vector _up;

        public float NearClipPlane { get { return _nearClipPlane; } set { _nearClipPlane = value; } }
        public float FarClipPlane { get { return _farClipPlane; } set { _farClipPlane = value; } }

        /// <summary>The field of view applied to the projection matrix during rendering transformations.</summary>
        public float FieldOfView { get { return _fieldOfView; } set { _fieldOfView = value; } }

        /// <summary>The speed at which the camera's position moves (camera movement sensitivity).</summary>
        public float PositionSpeed { get { return _positionSpeed; } set { _positionSpeed = value; } }
        public float LookSpeed { get { return _lookSpeed; } set { _lookSpeed = value; } }

        public Vector Position { get { return _position; } set { _position = value; } }
        public Vector Forward { get { return _forward; } set { _forward = value; } }
        public Vector Up { get { return _up; } set { _up = value; } }

        public Vector Backward { get { return -_forward; } }
        public Vector Right { get { return _forward.CrossProduct(_up).Normalize(); } }
        public Vector Left { get { return _up.CrossProduct(_forward).Normalize(); } }
        public Vector Down { get { return -_up; } }

        public Camera()
        {
            _position = new Vector(0, 0, 0);
            _forward = new Vector(0, 0, 1);
            _up = new Vector(0, 1, 0);

            _fieldOfView = .5f;
        }

        public Camera(Vector pos, Vector forward, Vector up, float fieldOfView)
        {
            _position = pos;
            _forward = forward.Normalize();
            _up = up.Normalize();
            _fieldOfView = fieldOfView;
        }

        public void Move(Vector direction, float ammount)
        {
            _position = _position + (direction * ammount);
        }

        public void RotateY(float angle)
        {
            Vector Haxis = yAxis.CrossProduct(_forward.Normalize());
            _forward = _forward.RotateBy(angle, 0, 1, 0).Normalize();
            _up = _forward.CrossProduct(Haxis.Normalize());
        }

        public void RotateX(float angle)
        {
            Vector Haxis = yAxis.CrossProduct(_forward.Normalize());
            _forward = _forward.RotateBy(angle, Haxis.X, Haxis.Y, Haxis.Z).Normalize();
            _up = _forward.CrossProduct(Haxis.Normalize());
        }

        public Matrix4 GetMatrix()
        {
            Matrix4 camera = Matrix4.LookAt(
              _position.X, _position.Y, _position.Z,
              _position.X + _forward.X, _position.Y + _forward.Y, _position.Z + _forward.Z,
              _up.X, _up.Y, _up.Z);
            return camera;
        }
        public void CameraControls()
        {
            // Camera position movement
            if (InputManager.Keyboard.Qdown)
                if (InputManager.Keyboard.ShiftLeftdown)
                    Move(Down, PositionSpeed * 100);
                else
                    Move(Down, PositionSpeed);

            if (InputManager.Keyboard.Edown)
                if (InputManager.Keyboard.ShiftLeftdown)
                    Move(Up, PositionSpeed * 100);
                else
                    Move(Up, PositionSpeed);

            if (InputManager.Keyboard.Adown)
                if (InputManager.Keyboard.ShiftLeftdown)
                    Move(Left, PositionSpeed * 100);
                else
                    Move(Left, PositionSpeed);

            if (InputManager.Keyboard.Wdown)
                if (InputManager.Keyboard.ShiftLeftdown)
                    Move(Forward, PositionSpeed * 100);
                else
                    Move(Forward, PositionSpeed);

            if (InputManager.Keyboard.Sdown)
                if (InputManager.Keyboard.ShiftLeftdown)
                    Move(Backward, PositionSpeed * 100);
                else
                    Move(Backward, PositionSpeed);

            if (InputManager.Keyboard.Ddown)
                if (InputManager.Keyboard.ShiftLeftdown)
                    Move(Right, PositionSpeed * 100);
                else
                    Move(Right, PositionSpeed);

            // Camera look angle adjustment
            if (InputManager.Keyboard.Kdown)
                RotateX(.05f);
            if (InputManager.Keyboard.Idown)
                RotateX(-.05f);
            if (InputManager.Keyboard.Jdown)
                RotateY(.05f);
            if (InputManager.Keyboard.Ldown)
                RotateY(-.05f);
        }
    }
}