using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampFireScene
{
    class InputManager
    {
        public void Update(object sender, FrameEventArgs e)
        {
            GameWindow game = sender as GameWindow;
            if (game.Keyboard[Key.Escape])
            {
                game.Exit();
            }
        }
    }
}
