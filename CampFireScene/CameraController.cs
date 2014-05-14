using OpenTK;
using OpenTK.Input;
using System;

namespace CampFireScene
{
    internal class Camera
    {
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public Vector3 Position = Vector3.Zero;

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

        public void Reset()
        {
            Position = new Vector3(0f, 0f, 5f);
            Orientation = new Vector3((float)Math.PI, 0f, 0f);
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

    internal class CameraController
    {
        public float KeyboardSensitivity = 0.1f;
        public float MouseSensitivity = 0.01f;
        public float MoveSpeed = 0.2f;
        public Matrix4 ProjectionMatrix;
        public Matrix4 ViewMatrix;
        private const float ASPECT = 4.0f / 3.0f;
        private const float FAR_CLIP = 1000.0f;
        private const float NEAR_CLIP = 0.1f;
        private Camera _camera;
        private Vector2 _lastMousePos;
        private GameWindow _window;

        public CameraController(GameWindow window)
        {
            _window = window;
            _camera = new Camera();
        }

        public void Reset()
        {
            _camera.Reset();
        }

        public void Update(double deltaTime)
        {
            Update((float)deltaTime);
        }

        public void Update(float deltaTime)
        {
            _camera.Move(getMoveVector() * deltaTime, MoveSpeed);

            Vector2 delta = _lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
            delta *= MouseSensitivity;
            _camera.Rotate(delta.X, delta.Y);
            _lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, ASPECT * (_window.Width / _window.Height), NEAR_CLIP, FAR_CLIP);
            ViewMatrix = _camera.GetViewMatrix();
        }

        private Vector3 getMoveVector()
        {
            Vector3 moveVector = Vector3.Zero;
            if (_window.Keyboard[Key.W])
                moveVector.Y += KeyboardSensitivity;
            if (_window.Keyboard[Key.D])
                moveVector.X += KeyboardSensitivity;
            if (_window.Keyboard[Key.S])
                moveVector.Y -= KeyboardSensitivity;
            if (_window.Keyboard[Key.A])
                moveVector.X -= KeyboardSensitivity;
            if (_window.Keyboard[Key.Q])
                moveVector.Z -= KeyboardSensitivity;
            if (_window.Keyboard[Key.E])
                moveVector.Z += KeyboardSensitivity;
            return moveVector;
        }
    }
}