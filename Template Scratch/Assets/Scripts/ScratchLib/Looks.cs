using System.Collections.Generic;
using UnityEngine;

namespace Scratch
{
    public partial class ScratchSprite : MonoBehaviour
    {
        [Header("Costumes")]
        [SerializeField]
        private List<Costume> costumes;
        public int currentCostumeIndex;
        public string currentCostumeName;
        public SpriteRenderer spriteRenderer;


        #region Looks
        /// <summary>
        /// Costume struct used by the sprite.
        /// </summary>
        [System.Serializable]
        private struct Costume
        {
            public string name;
            public Sprite sprite;
            public int index;
        }
        /// <summary>
        /// Sets the costume by its name, or index.
        /// </summary>
        public void SetCostume(object costumeName)
        {
            string costumeNameStr = (string)costumeName;
            char costumeChar = costumeNameStr[0];
            if (char.IsDigit(costumeChar))
            {
                spriteRenderer.sprite = costumes[int.Parse(costumeNameStr) - 1].sprite;
                currentCostumeIndex = int.Parse(costumeNameStr) - 1;
                currentCostumeName = costumes[int.Parse(costumeNameStr) - 1].name;
            }
            else
            {
                Costume costume = costumes.Find(costume => costume.name == (string)costumeName);
                spriteRenderer.sprite = costume.sprite;
                currentCostumeIndex = costume.index;
                currentCostumeName = costume.name;
                if (spriteRenderer.sprite == null)
                {
                    spriteRenderer.sprite = costumes[0].sprite;
                    currentCostumeIndex = costumes[0].index;
                    currentCostumeName = costumes[0].name;
                }
            }
        }
        /// <summary>
        /// Switches to the next costume.
        /// </summary>
        public void NextCostume()
        {
            spriteRenderer.sprite = costumes.Find(costume => costume.index == (currentCostumeIndex + 1) % costumes.Count).sprite;
            currentCostumeIndex += 1;
            currentCostumeIndex %= costumes.Count;
            currentCostumeName = costumes[currentCostumeIndex].name;
        }
        /// <summary>
        /// <para>Disable the sprite renderer.</para>
        /// </summary>
        public void Hide()
        {
            spriteRenderer.enabled = false;
        }
        /// <summary>
        /// <para>Enable the sprite renderer.</para>
        /// </summary>
        public void Show()
        {
            spriteRenderer.enabled = true;
        }
        /// <summary>
        /// <para>Set the scale to <paramref name="size"/>.</para>
        /// </summary>
        public void SetSize(object size)
        {
            float value = ToFloat(size);
            transform.localScale = Vector3.one * value;
        }
        /// <summary>
        /// <para>Set the scale to <paramref name="size"/>.</para>
        /// </summary>
        public void SetSize(double size)
        {
            float value = ToFloat(size);
            transform.localScale = Vector3.one * value;
        }
        /// <summary>
        /// <para>Change the scale by <paramref name="size"/>.</para>
        /// </summary>
        public void ChangeSize(object size)
        {
            float value = ToFloat(size);
            transform.localScale = transform.localScale - Vector3.one * value;
        }
        /// <summary>
        /// <para>Change the scale by <paramref name="size"/>.</para>
        /// </summary>
        public void ChangeSize(double size)
        {
            float value = ToFloat(size);
            transform.localScale = transform.localScale - Vector3.one * value;
        }
        /// <summary>
        /// <para>Set the <paramref name="layer"/> of the sprite.</para>
        /// </summary>
        public void SetLayer(string layer)
        {
            spriteRenderer.sortingLayerName = layer;
        }
        /// <summary>
        /// <para>Change the <paramref name="layer"/> of the sprite.</para>
        /// <para>Set the <paramref name="direction"/> to "backward" to substract a layer.</para>
        /// </summary>
        public void ChangeLayer(string direction, int layer)
        {
            if (direction == "backward")
            {
                layer = -layer;
            }
            spriteRenderer.sortingOrder += layer;
        }
        #endregion

    }
}
