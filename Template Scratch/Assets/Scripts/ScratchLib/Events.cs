using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Scratch
{
    public partial class ScratchSprite : MonoBehaviour
    {
        /// <summary>
        /// Sends a message to all ScratchLib inherited classes.
        /// Set <paramref name="wait"/>  to true to make it synchronous.
        /// </summary>
        public IEnumerator SendMessageToAll(string message, bool wait = false)
        {
            foreach (var sprite in spriteScriptInstances)
            {
                if (CoroutineChecker.ContainsCoroutine(sprite, message))
                {
                    if (wait)
                    {
                        yield return sprite.StartCoroutine(message);
                    }
                    else
                    {
                        sprite.StartCoroutine(message);
                    }

                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// A class used for handling scratch messages.
    /// </summary>
    public static class CoroutineChecker
    {
        public static bool ContainsCoroutine(MonoBehaviour script, string coroutineName)
        {
            foreach (MethodInfo method in script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (method.ReturnType == typeof(IEnumerator) && method.Name == coroutineName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
