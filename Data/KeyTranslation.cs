using Microsoft.Xna.Framework.Input;
using System;

namespace d4lilah.Data
{
    static class KeyTranslation
    {            

        static public Keys? Translate(string key)
        {
            Keys[] values = (Keys[])Enum.GetValues(typeof(Keys));

            foreach(Keys k in values)
            {
                if(key == k.ToString())
                {
                    return k;
                }   
            }
            return null;
            /*switch (key.ToLower())
                {
                case "a":
                    {
                        return Keys.A;
                    }
                case "b":
                    {
                        return Keys.B;
                    }
                case "c":
                    {
                        return Keys.C;
                    }
                case "d":
                    {
                        return Keys.D;
                    }
                case "e":
                    {
                        return Keys.E;
                    }
                case "f":
                    {
                        return Keys.F;
                    }
                case "g":
                    {
                        return Keys.G;
                    }
                case "h":
                    {
                        return Keys.H;
                    }
                case "i":
                    {
                        return Keys.I;
                    }
                case "J":
                    {
                        return Keys.J;
                    }
                case "k":
                    {
                        return Keys.K;
                    }
                case "l":
                    {
                        return Keys.L;
                    }
                case "m":
                    {
                        return Keys.M;
                    }
                case "n":
                    {
                        return Keys.N;
                    }
                case "o":
                    {
                        return Keys.O;
                    }
                case "p":
                    {
                        return Keys.P;
                    }
                case "q":
                    {
                        return Keys.Q;
                    }
                case "r":
                    {
                        return Keys.R;
                    }
                case "s":
                    {
                        return Keys.S;
                    }
                case "t":
                    {
                        return Keys.T;
                    }
                case "u":
                    {
                        return Keys.U;
                    }
                case "v":
                    {
                        return Keys.V;
                    }
                case "w":
                    {
                        return Keys.W;
                    }
                case "x":
                    {
                        return Keys.X;
                    }
                case "y":
                    {
                        return Keys.Y;
                    }
                case "z":
                    {
                        return Keys.Z;
                    }
                case "f1":
                    {
                        return Keys.F1;
                    }
                case "f2":
                    {
                        return Keys.F2;
                    }
                case "f3":
                    {
                        return Keys.F3;
                    }
                case "f4":
                    {
                        return Keys.F4;
                    }
                case "f5":
                    {
                        return Keys.F5;
                    }
                case "f6":
                    {
                        return Keys.F6;
                    }
                case "f7":
                    {
                        return Keys.F7;
                    }
                case "f8":
                    {
                        return Keys.F8;
                    }
                case "f9":
                    {
                        return Keys.F9;
                    }
                case "f10":
                    {
                        return Keys.F10;
                    }
                case "f11":
                    {
                        return Keys.F11;
                    }
                case "f12":
                    {
                        return Keys.F12;
                    }
                case "space":
                    {
                        return Keys.Space;
                    }
                case "1":
                    {
                        return Keys.D1;
                    }
                case "2":
                    {
                        return Keys.D2;
                    }
                case "3":
                    {
                        return Keys.D3;
                    }
                case "4":
                    {
                        return Keys.D4;
                    }
                case "5":
                    {
                        return Keys.D5;
                    }
                case "6":
                    {
                        return Keys.D6;
                    }
                case "7":
                    {
                        return Keys.D7;
                    }
                case "8":
                    {
                        return Keys.D8;
                    }
                case "9":
                    {
                        return Keys.D9;
                    }
                case "0":
                    {
                        return Keys.D0;
                    }
                case "tab":
                    {
                        return Keys.Tab;
                    }
                case "caps":
                    {
                        return Keys.CapsLock;
                    }
                case "lshift":
                    {
                        return Keys.LeftShift;
                    }
                case "rshift":
                    {
                        return Keys.RightShift;
                    }
                case "ctrl":
                    {
                        return Keys.LeftControl;
                    }
                case "lalt":
                    {
                        return Keys.LeftAlt;
                    }
                case "ralt":
                    {
                        return Keys.RightAlt;
                    }
                case "right":
                    {
                        return Keys.Right;
                    }
                case "left":
                    {
                        return Keys.Left;
                    }
                case "up":
                    {
                        return Keys.Up;
                    }
                case "down":
                    {
                        return Keys.Down;
                    }
                case "tilde":
                    {
                        return Keys.OemTilde;
                    }
                case "pipe":
                    {
                        return Keys.OemPipe;
                    }
                case "escape":
                    {
                        return Keys.Escape;
                    }
                default:
                    {
                        return null;
                    }
            }  */
        }
    }
}
