using System;
using UnityEngine;

namespace Scratch
{
    public partial class ScratchSprite : MonoBehaviour
    {
        /// <summary>
        /// <para>Returns the x position of the mouse.</para>
        /// </summary>
        public float GetMousePositionX()
        {
            return GetMousePosition().x;
        }
        /// <summary>
        /// <para>Returns the y position of the mouse.</para>
        /// </summary>
        public float GetMousePositionY()
        {
            return GetMousePosition().y;
        }

        private Vector2 GetMousePosition()
        {
            return new Vector2(Input.mousePosition.x / Screen.width * 480, Input.mousePosition.y / Screen.height * 360) - new Vector2(240, 180);
        }

        /// <summary>
        /// Gets the distance to another object. The <paramref name="target"/> can be "mouse" or "sprite".
        /// </summary>
        public float getDistanceTo(Target target, string name = "")
        {
            switch (target)
            {
                case Target.mouse:
                    return Vector2.Distance(transform.position, GetMousePosition());
                case Target.sprite:
                    Transform sprite = GameObject.Find(name).transform;
                    return Vector2.Distance(transform.position, sprite.position);
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Detects if the sprite touches another object. The <paramref name="target"/> can be "mouse", "edge" or "sprite".
        /// </summary>
        public bool Touching(Target target, string name = "")
        {
            switch (target)
            {
                case Target.mouse:
                    return touchedSprites.Contains(mouseCollider);
                case Target.edge:
                    return touchedSprites.Contains(edges);
                case Target.sprite:
                    GameObject sprite = GameObject.Find(name);
                    return touchedSprites.Contains(sprite);
                default:
                    return false;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            touchedSprites.Add(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            touchedSprites.Remove(collision.gameObject);
        }
        public double NumberOfDaysSince2000()
        {
            DateTime startDate = new DateTime(2000, 1, 1);
            TimeSpan elapsedTime = DateTime.Now - startDate;
            return elapsedTime.TotalDays;
        }
        /// <summary>
        /// Translation of scratch's touch key block.
        /// </summary>
        public bool GetKey(string key)
        {
            if (key.Length == 0)
            {
                return false;
            }
            char keyChar = key[0];

            switch (key)
            {
                case "any":
                    return Input.anyKey && !Input.GetMouseButton(0);
                case "space":
                    return Input.GetKey(KeyCode.Space);
                case "enter":
                    return Input.GetKey(KeyCode.Return);
                case "left arrow":
                    return Input.GetKey(KeyCode.LeftArrow);
                case "right arrow":
                    return Input.GetKey(KeyCode.RightArrow);
                case "up arrow":
                    return Input.GetKey(KeyCode.UpArrow);
                case "down arrow":
                    return Input.GetKey(KeyCode.DownArrow);
                case "'":
                    return Input.GetKey(KeyCode.Quote);
                case ",":
                    return Input.GetKey(KeyCode.Comma);
                case ";":
                    return Input.GetKey(KeyCode.Semicolon);
                case ":":
                    return Input.GetKey(KeyCode.Colon);
                case "!":
                    return Input.GetKey(KeyCode.Exclaim);
                case "?":
                    return Input.GetKey(KeyCode.Question);
                case ".":
                    return Input.GetKey(KeyCode.Period);
                case "/":
                    return Input.GetKey(KeyCode.Slash);
                case "~":
                    return Input.GetKey(KeyCode.Tilde);
                case "*":
                    return Input.GetKey(KeyCode.KeypadMultiply);
                case "-":
                    return Input.GetKey(KeyCode.Minus);
                case "(":
                    return Input.GetKey(KeyCode.LeftParen);
                case ")":
                    return Input.GetKey(KeyCode.RightParen);
                case "+":
                    return Input.GetKey(KeyCode.KeypadPlus);
                case "&":
                    return Input.GetKey(KeyCode.Ampersand);
                case "_":
                    return Input.GetKey(KeyCode.Underscore);
                case "=":
                    return Input.GetKey(KeyCode.Equals);
                case "0":
                    return Input.GetKey(KeyCode.Keypad0);
                case "1":
                    return Input.GetKey(KeyCode.Keypad1);
                case "2":
                    return Input.GetKey(KeyCode.Keypad2);
                case "3":
                    return Input.GetKey(KeyCode.Keypad3);
                case "4":
                    return Input.GetKey(KeyCode.Keypad4);
                case "5":
                    return Input.GetKey(KeyCode.Keypad5);
                case "6":
                    return Input.GetKey(KeyCode.Keypad6);
                case "7":
                    return Input.GetKey(KeyCode.Keypad7);
                case "8":
                    return Input.GetKey(KeyCode.Keypad8);
                case "9":
                    return Input.GetKey(KeyCode.Keypad9);
                default:
                    if (char.IsLetter(keyChar))
                    {
                        char uppercaseInput = char.ToUpper(keyChar);
                        KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), uppercaseInput.ToString());
                        return Input.GetKey(keyCode);
                    }
                    else
                    {
                        Debug.LogError("Unknown key : " + key);
                        return false;
                    }
            }
        }
    }
}

