using Scratch.PenTools;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Scratch
{
    /// <summary>
    /// <para>
    /// A class that implements scratch blocks in a human-readable format.
    /// </para>
    /// <para>
    /// It contains various useful functions inspired by scratch's blocks.
    /// Use it like this : 
    /// </para> 
    /// <code>
    /// public class MySpriteScript : ScratchSprite
    /// </code>
    /// </summary>
    public partial class ScratchSprite : MonoBehaviour
    {
        private static List<ScratchSprite> spriteScriptInstances = new List<ScratchSprite>();

        [Header("Clones")]
        public bool isClone = false;
        private Transform cloneContainer;

        [Header("Collision")]
        [SerializeField]
        public List<GameObject> touchedSprites = new List<GameObject>();
        private GameObject edges;
        private GameObject mouseCollider;

        /// <summary>
        /// Initialization of references
        /// </summary>
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteScriptInstances.Add(this);
            pen = FindObjectOfType<PenDrawer>();
            mouseCollider = GameObject.Find("MouseCollider");
            cloneContainer = GameObject.Find("Clone Container").transform;
            if (gameObject.GetComponent<PolygonCollider2D>() == null)
            {
                PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
                collider.isTrigger = true;
                Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
                rb.isKinematic = true;
            }
            isClone = transform.parent == cloneContainer;
        }

        /// <summary>
        /// Used to track the script instances.
        /// </summary>
        protected virtual void OnDestroy()
        {
            // Remove the instance from the list when it's destroyed
            spriteScriptInstances.Remove(this);
        }

        
        #region Clones
        /// <summary>
        /// Duplicate the given GameObject. Set <paramref name="spriteName"/> to "_myself_" to clone this GameObject.
        /// </summary>
        public void CreateClone(string spriteName)
        {
            GameObject sprite;
            if (spriteName == "_myself_")
            {
                sprite = gameObject;
            }
            else
            {
                sprite = GameObject.Find(spriteName);
            }
            Instantiate(sprite, cloneContainer);
        }
        /// <summary>
        /// Delete the GameObject if it's a clone
        /// </summary>
        public void DeleteClone()
        {
            if (isClone)
            {
                Destroy(gameObject);
            }
        }
        #endregion
    }
    /// <summary>
    /// Target options for the scene.
    /// </summary>
    public enum Target
    {
        sprite,
        mouse,
        random,
        edge,
        stage
    }

    public static class Timing
    {
        static Stopwatch stopwatch = new Stopwatch();
        public static void StartWatch()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public static void StopWatch(string message)
        {
            stopwatch.Stop();
            //TimeSpan elapsedTime = stopwatch.Elapsed;
            //UnityEngine.Debug.Log($"Function {message} took {elapsedTime.TotalMilliseconds} milliseconds");
        }
    }
}