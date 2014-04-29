//using OpenTK;
//using OpenTK.Input;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace CampFireScene
//{
//    class InputManager
//    {
//        public void Update(object sender, FrameEventArgs e)
//        {
//            GameWindow game = sender as GameWindow;
//            if (game.Keyboard[Key.Escape])
//            {
//                game.Exit();
//            }
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
// Last Edited: 11-16-13

using System;
using OpenTK.Input;

namespace CampFireScene
{
    /// <summary>InputManager is used for input management (keyboard and mouse).</summary>
    public static class InputManager
    {
        //// Reference to the keyboard input
        //private static Keyboard _keyboard;
        ////private static Mouse _mouse;

        ///// <summary>Gets a reference to the keyboard device to check inputs.</summary>
        //public static Keyboard Keyboard { get { return _keyboard; } }
        ///// <summary>Gets a reference to the mouse device to check inputs.</summary>
        ////public static Mouse Mouse { get { return _mouse; } }

        ///// <summary>This initializes the reference to OpenTK's keyboard.</summary>
        ///// <param name="keyboardDevice">Reference to OpenTK's KeyboardDevice class within their GameWindow class.</param>
        //internal static void InitializeKeyboard(KeyboardDevice keyboardDevice)
        //{ _keyboard = new Keyboard(keyboardDevice); }
        ///// <summary>This initializes the reference to OpenTK's mouse.</summary>
        ///// <param name="mouseDevice">Reference to OpenTK's MouseDevice class within their GameWindow class.</param>
        ////internal static void InitializeMouse(MouseDevice mouseDevice)
        ////{ _mouse = new Mouse(mouseDevice); }

        ///// <summary>Update the input states.</summary>
        //public static void Update()
        //{
        //    _keyboard.Update();
        //    //_mouse.Update();
        //}
    }
}