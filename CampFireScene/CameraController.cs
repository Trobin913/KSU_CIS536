using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace CampFireScene
{
    class Camera
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3()
            {
                X = (float)(Math.Sin(Orientation.X) * Math.Cos(Orientation.Y)),
                Y = (float)(Math.Sin(Orientation.Y)),
                Z = (float)(Math.Cos(Orientation.X) * Math.Cos(Orientation.Y))
            };

            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }

        public void Move(Vector3 vec, float speed)
        {
            Move(vec.X, vec.Y, vec.Z, speed);
        }

        public void Move(float x, float y, float z, float speed)
        {
            Vector3 forward = new Vector3(
                (float)(Math.Sin(Orientation.X)),
                0,
                (float)(Math.Cos(Orientation.X)));
            Vector3 right = new Vector3(
                -forward.Z,
                0,
                forward.Z);
            Vector3 offset = new Vector3();
            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, speed);

            Position += offset;
        }

        public void Rotate(float x, float y)
        {
            Orientation.X = (Orientation.X + x) % (float)(Math.PI * 2);
            Orientation.Y = Math.Max(
                Math.Min(
                    Orientation.Y + y,
                    (float)(Math.PI / 2 - 0.1)),
                (float)(-Math.PI / 2 + 0.1));
        }
    }

    class CameraController
    {
        private const float ASPECT = 4.0f / 3.0f;
        private const float NEAR_CLIP = 0.1f;
        private const float FAR_CLIP = 100.0f;

        private Camera _camera;
        private GameWindow _window;
        private Vector3 _moveVector;
        private Vector2 _lastMousePos;
        private Vector2 _delta;

        public Matrix4 ProjectionMatrix;
        public Matrix4 ViewMatrix;

        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.01f;
        public float KeyboardSpeed = 0.1f;

        public CameraController(GameWindow window)
        {
            _window = window;
            _window.KeyDown += _window_KeyDown;
            _window.KeyUp += _window_KeyUp;
            _camera = new Camera();
            _moveVector = new Vector3();
            _delta = new Vector2();
        }

        void _window_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    _moveVector.Y -= KeyboardSpeed;
                    break;
                case Key.A:
                    _moveVector.X += KeyboardSpeed;
                    break;
                case Key.S:
                    _moveVector.Y += KeyboardSpeed;
                    break;
                case Key.D:
                    _moveVector.X -= KeyboardSpeed;
                    break;
                case Key.Q:
                    _moveVector.Z -= KeyboardSpeed;
                    break;
                case Key.E:
                    _moveVector.Z += KeyboardSpeed;
                    break;
                case Key.Left:
                    _delta.X += 0.1f;
                    break;
                case Key.Right:
                    _delta.X -= 0.1f;
                    break;
                case Key.Up:
                    _delta.Y -= 0.1f;
                    break;
                case Key.Down:
                    _delta.Y += 0.1f;
                    break;
            }
        }

        void _window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    _moveVector.Y += KeyboardSpeed;
                    break;
                case Key.A:
                    _moveVector.X -= KeyboardSpeed;
                    break;
                case Key.S:
                    _moveVector.Y -= KeyboardSpeed;
                    break;
                case Key.D:
                    _moveVector.X += KeyboardSpeed;
                    break;
                case Key.Q:
                    _moveVector.Z += KeyboardSpeed;
                    break;
                case Key.E:
                    _moveVector.Z -= KeyboardSpeed;
                    break;
                case Key.Left:
                    _delta.X -= 0.1f;
                    break;
                case Key.Right:
                    _delta.X += 0.1f;
                    break;
                case Key.Up:
                    _delta.Y += 0.1f;
                    break;
                case Key.Down:
                    _delta.Y -= 0.1f;
                    break;
            }
        }

        public void Update(double deltaTime)
        {
            Update((float)deltaTime);
        }

        public void Update(float deltaTime)
        {
            _camera.Move(_moveVector * deltaTime, MoveSpeed);

            Vector2 delta = _lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
            delta *= MouseSensitivity;
            _camera.Rotate(delta.X, delta.Y);
            _lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, ASPECT * (_window.Width / _window.Height), NEAR_CLIP, FAR_CLIP);
            ViewMatrix = _camera.GetViewMatrix();
        }
    }
}
