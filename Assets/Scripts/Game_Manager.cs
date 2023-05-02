using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    private static Game_Manager _gameManager;
    public delegate void DelVoid();
    public static DelVoid GameEnd;

    [SerializeField]
    private Canvas _canvasWorld;
    [SerializeField]
    private Text _prefabEntityHealthText;
    [SerializeField]
    private LayerMask _platformMask;
    public static LayerMask PlatformMask;
    [SerializeField]
    private Text _levelProgressText;

    public static readonly float DebugRayLifeTime = 0.02f;
    [Tooltip("When calculating distance of ray for raycast, use this to account for small gaps between colliders for better physics performance")]
    public static readonly float RayCastRayOffset = 0.2f;
    /// <summary>
    /// Returns an angle in degrees
    /// </summary>
    public static Func<Vector2, float> GetAngleFromVector2 { get; } = vector2 => Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
    public static Func<float, Vector2> GetVector2FromAngle { get; } = angleInDegrees => new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    public static Func<Vector2, Vector2, float> GetAngleBetweenVector2s { get; } =
     (vector1, vector2) => Mathf.Acos(GetDotProductFromVector2s(vector1, vector2) / GetMagnitudeProductFromVector2s(vector1, vector2)) * Mathf.Rad2Deg;
    private static Func<Vector2, Vector2, float> GetDotProductFromVector2s { get; } = (vector1, vector2) => Vector2.Dot(vector1, vector2);
    private static Func<Vector2, Vector2, float> GetMagnitudeProductFromVector2s { get; } = (vector1, vector2) => vector1.magnitude * vector2.magnitude;
    public static Func<Ray, float, RaycastHit2D> GetRaycastHit { get; } = (ray, distance) => Physics2D.Raycast(ray.origin, ray.direction, distance, PlatformMask);
    public static Func<GameObject, bool> IsObjectAPlatform { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "Platform");
    public static Func<GameObject, bool> IsObjectAnInvisiblePlatform { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "VoidCheck");
    public static Func<GameObject, bool> IsObjectAnEnemy { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "Enemy");
    public static Func<GameObject, bool> IsObjectAPlayer { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "Player");
    public static Func<GameObject, bool> IsObjectALevelEnd { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "LevelEnd");
    public static Func<GameObject, bool> IsObjectALevelCheckpoint { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "SpawnPoint");
    private static Func<GameObject, string, bool> IsObjectAGeneric { get; } = (objectToCheck, tagToCheck) => objectToCheck.CompareTag(tagToCheck);

    private void Awake()
    {
        SetStatics();
    }
    private void SetStatics()
    {
        PlatformMask = _platformMask;
    }
    private Game_Manager()
    {

    }
    public static Game_Manager GetSingleton()
    {
        if (!_gameManager)
        {
            if (FindObjectsOfType<Game_Manager>().Length != 1)
            {
                throw new Exception("Either no GameManager or more than one GameManager object exists in the scene");
            }
            _gameManager = FindObjectOfType<Game_Manager>();
        }
        return _gameManager;
    }
    public Text CreateAndGetHealthText()
    {
        GameObject healthText = Instantiate(_prefabEntityHealthText.gameObject);
        healthText.transform.parent = _canvasWorld.transform;
        return healthText.GetComponent<Text>();
    }
    public void WinGame(PlayerMovement player)
    {
        // If there is at least 1 enemy alive, do not end the game
        int count = FindObjectsOfType<EnemyMovement>().Length;
        if (count > 0)
        {
            SetLevelProgressText($"You cannot win yet!\nThere are still {count} enemies alive.", player.transform.position);
        }
        else
        {
            GameEnd?.Invoke();
            SetLevelProgressText("You win!\n:)", player.transform.position);
            StartCoroutine(SwitchScene());
        }
    }
    public void SetLevelProgressText(String message, Vector2 position)
    {
        _levelProgressText.gameObject.SetActive(true);
        _levelProgressText.text = message;
        _levelProgressText.transform.position = position + 17.5f * Vector2.up;
        StartCoroutine(HideLevelProgress());
    }
    private IEnumerator HideLevelProgress()
    {
        yield return new WaitForSeconds(3.5f);
        _levelProgressText.gameObject.SetActive(false);
    }
    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }
    public void AutoWinGame()
    {
        Debug.Log($"{name}: Initiated cheating");
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach (EnemyHealth e in enemies)
        {
            e.OnDeath();
        }
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        player.transform.position = new Vector2(520f, -3f);
    }
}
