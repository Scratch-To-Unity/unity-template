using System;
using System.Collections;
using UnityEngine;

namespace Scratch
{
    public partial class ScratchSprite : MonoBehaviour
    {
        /// <summary>
        /// Called on each move of the sprite.
        /// </summary>
        private void OnMove(Vector2 nextPosition)
        {
            if (!isPenDown)
            {
                return;
            }

            DrawLineShader(transform.position, nextPosition, penColor, (float)penWidth);
        }
        /// <summary>
        /// Move the sprite to (x, y).
        /// </summary>
        public void GoToXY(double x, double y)
        {
            float valueX = ClampPosition(ToFloat(x));
            float valueY = ClampPosition(ToFloat(y));
            OnMove(new Vector2(valueX, valueY));
            transform.position = new Vector2(valueX, valueY);
        }
        /// <summary>
        /// Move the sprite to (x, y).
        /// </summary>
        public void GoToXY(object x, object y)
        {
            float valueX = ClampPosition(ToFloat(x));
            float valueY = ClampPosition(ToFloat(y));
            OnMove(new Vector2(valueX, valueY));
            transform.position = new Vector2(valueX, valueY);
        }
        /// <summary>
        /// <para>Move the sprite to another object position. </para>
        /// <paramref name="target"></paramref> can be "mouse", "random" or "sprite". 
        /// In this last case, set <paramref name="name"></paramref> to the sprite's name.
        /// </summary>
        public void GoTo(Target target, string name = "")
        {
            switch (target)
            {
                case Target.mouse:
                    OnMove(GetMousePosition());
                    transform.position = GetMousePosition();
                    break;
                case Target.random:
                    Vector2 pos = RandomPosition();
                    OnMove(pos);
                    transform.position = pos;
                    break;
                case Target.sprite:
                    Transform sprite = GameObject.Find(name).transform;
                    OnMove(sprite.position);
                    transform.position = sprite.position;
                    break;
                default:
                    Debug.LogError("Unknown GoTo Command");
                    break;
            }
        }
        public Vector2 RandomPosition()
        {
            return new Vector2(UnityEngine.Random.Range(-240, 240), UnityEngine.Random.Range(-180, 180));
        }
        /// <summary>
        /// <para>Glide the sprite to another <paramref name="target"/> position in <paramref name="seconds"/>.</para>
        /// <paramref name="target"></paramref> can be "mouse", "random" or "sprite". 
        /// </summary>
        public IEnumerator GlideToTarget(float seconds, Target target)
        {
            switch (target)
            {
                case Target.sprite:
                    Transform sprite = GameObject.Find(name).transform;
                    yield return StartCoroutine(GlideToPosition(seconds, sprite.position));
                    break;
                case Target.mouse:
                    yield return StartCoroutine(GlideToPosition(seconds, GetMousePosition()));
                    break;
                case Target.random:
                    yield return StartCoroutine(GlideToPosition(seconds, RandomPosition()));
                    break;
                default:
                    break;
            }
            yield return null;
        }
        /// <summary>
        /// <para>Glide the sprite to another position in <paramref name="seconds"/>.</para>
        /// </summary>
        public IEnumerator GlideToXY(float seconds, float X, float Y)
        {
            yield return StartCoroutine(GlideToPosition(seconds, new Vector2(X, Y)));
        }

        private IEnumerator GlideToPosition(float seconds, Vector2 targetPosition)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0;

            while (elapsedTime < seconds)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / seconds);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }

        /// <summary>
        /// <para>Turn the sprite towards another object. </para>
        /// In this last case, set <paramref name="name"></paramref> to the sprite's name.
        /// </summary>
        public void PointTowards(Target target, string name = "")
        {
            switch (target)
            {
                case Target.mouse:
                    transform.rotation = LookAt2D(transform.position, GetMousePosition());
                    break;
                case Target.sprite:
                    Transform sprite = GameObject.Find(name).transform;
                    transform.rotation = LookAt2D(transform.position, sprite.position);
                    break;
                default:
                    Debug.LogError("Unknown PointTowards Command");
                    break;
            }
        }

        private float ClampPosition(float positionAxis)
        {
            positionAxis = Mathf.Clamp(positionAxis, -10000, 10000);
            positionAxis = float.IsNaN(positionAxis) ? 0 : positionAxis;
            return positionAxis;
        }

        private Vector2 ClampPosition(Vector2 position)
        {
            float pX = Mathf.Clamp(position.x, -10000, 10000);
            float pY = Mathf.Clamp(position.y, -10000, 10000);
            pX = float.IsNaN(pX) ? 0 : pX;
            pY = float.IsNaN(pY) ? 0 : pY;
            return new Vector2(pX, pY);
        }

        Quaternion LookAt2D(Vector3 position, Vector3 targetPosition)
        {
            Vector3 directionToTarget = targetPosition - position;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }
        /// <summary>
        /// <para>Change sprite's position along the x axis.</para>
        /// </summary>
        public void ChangeX(object x)
        {
            float value = ToFloat(x);
            OnMove(transform.position + Vector3.right * value);
            transform.Translate(Vector2.right * value, Space.World);
        }
        /// <summary>
        /// <para>Change sprite's position along the y axis.</para>
        /// </summary>
        public void ChangeY(object y)
        {
            float value = ToFloat(y);
            OnMove(transform.position + Vector3.up * value);
            transform.Translate(Vector2.up * value, Space.World);
        }
        /// <summary>
        /// <para>Change sprite's position along the x axis.</para>
        /// </summary>
        public void ChangeX(double x)
        {
            float value = ToFloat(x);
            OnMove(transform.position + Vector3.right * value);
            transform.Translate(Vector2.right * value, Space.World);
        }
        /// <summary>
        /// <para>Change sprite's position along the y axis.</para>
        /// </summary>
        public void ChangeY(double y)
        {
            float value = ToFloat(y);
            OnMove(transform.position + Vector3.up * value);
            transform.Translate(Vector2.up * value, Space.World);
        }
        /// <summary>
        /// <para>Change sprite's position along its forward axis.</para>
        /// </summary>
        public void MoveSteps(object steps)
        {
            float value = ToFloat(steps);
            Vector2 beforePos = transform.position;
            transform.Translate(Vector2.right * value, Space.Self);
            OnMove(beforePos);
        }
        /// <summary>
        /// <para>Rotate the sprite to the right by <paramref name="degrees"/>.</para>
        /// </summary>
        public void TurnRight(object degrees)
        {
            float value = ToFloat(degrees);
            transform.Rotate(Vector3.forward, value);
        }
        /// <summary>
        /// <para>Rotate the sprite to the left by <paramref name="degrees"/>.</para>
        /// </summary>
        public void TurnLeft(object degrees)
        {
            float value = ToFloat(degrees);
            transform.Rotate(Vector3.forward, -value);
        }
        /// <summary>
        /// <para>Set the sprite's z axis rotation to <paramref name="degrees"/>. </para>
        /// </summary>
        public void SetRotation(object degrees)
        {
            float value = ToFloat(degrees);
            transform.rotation = Quaternion.Euler(Vector3.forward * (90 - value));
        }
        /// <summary>
        /// <para>Set the sprite's x position to <paramref name="x"/>. </para>
        /// </summary>
        public void SetX(object x)
        {
            float value = ToFloat(x);
            OnMove(new Vector2(value, transform.position.y));
            transform.position = new Vector2(value, transform.position.y);
        }
        /// <summary>
        /// <para>Set the sprite's y position to <paramref name="y"/>. </para>
        /// </summary>
        public void SetY(object y)
        {
            float value = ToFloat(y);
            OnMove(new Vector2(transform.position.x, value));
            transform.position = new Vector2(transform.position.x, value);
        }
        /// <summary>
        /// <para>Set the sprite's x position to <paramref name="x"/>. </para>
        /// </summary>
        public void SetX(double x)
        {
            float value = ToFloat(x);
            OnMove(new Vector2(value, transform.position.y));
            transform.position = new Vector2(value, transform.position.y);
        }
        /// <summary>
        /// <para>Set the sprite's y position to <paramref name="y"/>. </para>
        /// </summary>
        public void SetY(double y)
        {
            float value = ToFloat(y);
            OnMove(new Vector2(transform.position.x, value));
            transform.position = new Vector2(transform.position.x, value);
        }
    }
}
