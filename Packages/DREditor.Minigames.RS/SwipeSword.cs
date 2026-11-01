//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using DREditor.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISword) +
        "\n\n")]
    public class SwipeSword : MonoBehaviour, ISword
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool logObjectsHit = false;
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float cutDistance = 9.35f;


        [Header("")]
        [SerializeField] protected UnityEvent<GameObject, Vector3> onHitDetected;


        public UnityEvent<GameObject, Vector3> OnHitDetected { get => onHitDetected; }

        public bool RecentlyLandedHit { get => recentlyLandedHit; }


        /// <summary>
        ///
        /// </summary>
        protected bool detectingCuts = false;

        /// <summary>
        ///
        /// </summary>
        protected bool recentlyLandedHit = false;

        /// <summary>
        ///
        /// </summary>
        protected BoxCollider boxCollider;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 startPosition = Vector3.zero;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 endPosition = Vector3.zero;

        /// <summary>
        ///
        /// </summary>
        protected ISwordHitListener[] hitListeners;


        protected virtual void Awake()
        {
            hitListeners = transform.root.GetComponentsInChildren<ISwordHitListener>();

            boxCollider = GetComponent<BoxCollider>();

            boxCollider.size = new Vector3(cutDistance, boxCollider.size.y, boxCollider.size.z);
        }

        protected virtual void OnEnable()
        {
            if (hitListeners == null)
                return;

            foreach (var listener in hitListeners)
                onHitDetected.AddListener(listener.OnSwordHit);
        }

        protected virtual void OnDisable()
        {
            foreach (var listener in hitListeners)
                onHitDetected.RemoveListener(listener.OnSwordHit);
        }

        protected virtual void OnTriggerEnter(Collider other) => DetectCut(other.gameObject);

        protected virtual void OnTriggerExit(Collider other) => DetectCut(other.gameObject);

        protected virtual void OnCollisionEnter(Collision collision) => DetectCut(collision.gameObject);

        protected virtual void OnCollisionExit(Collision collision) => DetectCut(collision.gameObject);

        /// <summary>
        ///
        /// </summary>
        /// <param name="collidedObject"></param>
        protected virtual void DetectCut(GameObject collidedObject)
        {
            if (detectingCuts && collidedObject.GetComponent<Collider>() is Collider objectsCollider)
            {
                if (logObjectsHit)
                    UnityEngine.Debug.Log("REPORT: Object Hit: " + collidedObject.name);

                if (collidedObject.transform.parent != null && 
                    collidedObject.transform.parent.TryGetComponent(out PhraseTextBase phraseText) && 
                    phraseText.textPhrase is IPhraseHealth health &&
                    health?.GetHealth() != 0)
                    recentlyLandedHit = true;

                onHitDetected?.Invoke(collidedObject, objectsCollider.ClosestPointOnBounds(transform.position));
            }
        }


        /// <inheritdoc/>
        public virtual Vector3 AngleSword(Vector3 direction)
        {
            float halfCut = cutDistance / 2;

            float nhalfCut = halfCut * -1;

            endPosition = GetAnglePosition(direction, halfCut, nhalfCut);

            startPosition = GetAnglePosition(direction, nhalfCut, halfCut);

            AngleCollider(direction);

            return startPosition;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="offsetOne"></param>
        /// <param name="offsetTwo"></param>
        /// <returns></returns>
        protected virtual Vector3 GetAnglePosition(Vector3 direction, float offsetOne, float offsetTwo)
        {
            float xOffset = Mathf.Round(direction.x) switch
            {
                0 => 0,
                1 => offsetOne,
                _ => offsetTwo
            };

            float yOffset = Mathf.Round(direction.y) switch
            {
                0 => 0,
                1 => offsetOne,
                _ => offsetTwo
            };

            return new(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        protected virtual void AngleCollider(Vector3 direction)
        {
            bool isHorizontal = startPosition.y == endPosition.y;

            bool isVertical = startPosition.x == endPosition.x;

            if (isHorizontal)
                boxCollider.transform.eulerAngles = new Vector3(-90, 0, 0);
            else if (isVertical)
                boxCollider.transform.eulerAngles = new Vector3(0, -90, 90);
            else if ((direction.x < 0 && direction.y > 0) || (direction.y < 0 && direction.x > 0))
                boxCollider.transform.eulerAngles = new Vector3(315, 90, 270);
            else if ((direction.x > 0 && direction.y > 0) || (direction.y < 0 && direction.x < 0))
                boxCollider.transform.eulerAngles = new Vector3(45, 90, 270);
        }

        /// <inheritdoc/>
        public virtual void StartDetectingCuts()
        {
            recentlyLandedHit = false;

            detectingCuts = true;

            if (boxCollider == null)
                boxCollider = GetComponent<BoxCollider>();

            boxCollider.enabled = true;
        }

        /// <inheritdoc/>
        public virtual void StopDetectingCuts()
        {
            detectingCuts = false;

            if (boxCollider == null)
                boxCollider = GetComponent<BoxCollider>();

            boxCollider.enabled = false;
        }

        /// <inheritdoc/>
        public virtual bool IsDetectingCuts() => detectingCuts;


        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector3 position = boxCollider != null ? boxCollider.transform.position : transform.position;

            float halfCut = cutDistance / 2;

            (Vector3 from, Vector3 to)[] lines =
            {
                (new(0, -halfCut), new(0,  halfCut)), // Vertical
                (new(-halfCut, 0), new(halfCut, 0)), // Horizontal
                (new(halfCut, -halfCut), new(-halfCut, halfCut)), // Diagonal One
                (new(-halfCut, -halfCut), new(halfCut, halfCut)), // Diagonal Two
            };

            foreach (var (from, to) in lines)
                GizmosUtility.DrawSwipeLineGizmo(position + from, position + to);

            if (Application.isPlaying)
                GizmosUtility.DrawSphereLine(startPosition, endPosition);
        }
    }
}