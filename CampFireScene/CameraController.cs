using OpenTK;
using OpenTK.Input;
using System;

namespace CampFireScene
{
    /// <summary>
    /// Represents the camera in the scene.
    /// </summary>
    /// <Author>Devin Kelly-Collins</Author>
    internal class Camera
    {
        /// <summary>
        /// The orientations of the camera.
        /// </summary>
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        
        /// <summary>
        /// The position of the camera in the scene.
        /// </summary>
        public Vector3 Position = Vector3.Zero;

        /// <summary>
        /// Returns the view matrix.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Moves the camera in the scene.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="speed"></param>
        public void Move(Vector3 vec, float speed)
        {
            Move(vec.X, vec.Y, vec.Z, speed);
        }

        /// <summary>
        /// Moves the camera in the scene.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="speed"></param>
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

        /// <summary>
        /// Resets the camera to its starting values.
        /// </summary>
        public void Reset()
        {
            Position = new Vector3(0f, 0f, 5f);
            Orientation = new Vector3((float)Math.PI, 0f, 0f);
        }

        /// <summary>
        /// Rotates the camera.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
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

    /// <summary>
    /// Handles camera logic.
    /// </summary>
    /// <Author>Devin Kelly-Collins</Author>
    internal class CameraController
    {
        /// <summary>
        /// Sensitivity of the keyboard.
        /// </summary>
        public float KeyboardSensitivity = 0.1f;

        /// <summary>
        /// Sensitivity of the mouse.
        /// </summary>
        public float MouseSensitivity = 0.01f;
        
        /// <summary>
        /// The speed the camera moves.
        /// </summary>
        public float MoveSpeed = 0.2f;

        /// <summary>
        /// The calculated Projection Matrix. This is updated on every call to update.
        /// </summary>
        public Matrix4 ProjectionMatrix;

        /// <summary>
        /// The calculated View Matrix. This is updated on every call to update.
        /// </summary>
        public Matrix4 ViewMatrix;

        /// <summary>
        /// The aspect ratio.
        /// </summary>
        private const float ASPECT = 4.0f / 3.0f;

        /// <summary>
        /// The far clip plane.
        /// </summary>
        private const float FAR_CLIP = 1000.0f;

        /// <summary>
        /// The near clip plane.
        /// </summary>
        private const float NEAR_CLIP = 0.1f;

        private Camera _camera;
        private Vector2 _lastMousePos;
        private GameWindow _window;

        public CameraController(GameWindow window)
        {
            _window = window;
            _camera = new Camera();
        }

        /// <summary>
        /// Resets the camera.
        /// </summary>
        public void Reset()
        {
            _camera.Reset();
        }

        /// <summary>
        /// Updates the camera based on input.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(double deltaTime)
        {
            Update((float)deltaTime);
        }

        /// <summary>
        /// Updates the camera based on input.
        /// </summary>
        /// <param name="deltaTime"></param>
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

        /// <summary>
        /// Returns the move vector.
        /// </summary>
        /// <returns></returns>
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