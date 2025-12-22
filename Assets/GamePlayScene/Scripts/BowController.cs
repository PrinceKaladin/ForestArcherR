using UnityEngine;

public class BowController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float rotationSpeed = 30f; 
    public float maxAngle = 45f;

    private bool shooting = false;
    private float angleDirection = 1f;
    private GameObject currentArrow;

    void Start()
    {
        SpawnArrow();
    }

    void Update()
    {
        if (!shooting)
        {
            float currentZ = transform.eulerAngles.z;
            if (currentZ > 180f) currentZ -= 360f;

            if (currentZ >= maxAngle) angleDirection = -1f;
            if (currentZ <= -maxAngle) angleDirection = 1f;

            float rotationThisFrame = angleDirection * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationThisFrame);
        }
    }

    void SpawnArrow()
    {
        currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation, transform);
    }

    public void ShootArrow()
    {
        if (currentArrow == null || shooting) return;

        shooting = true;
        GameManager.Instance.ArrowFired();
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayArrowShoot();
        ArrowController arrowCtrl = currentArrow.GetComponent<ArrowController>();
        if (arrowCtrl != null)
        {
            Vector2 shootDirection = arrowSpawnPoint.up;

            arrowCtrl.OnArrowHit += OnArrowLanded;

            arrowCtrl.Launch(shootDirection);
        }

        currentArrow = null;

        Invoke(nameof(SpawnArrow), 0.5f);
    }

    private void OnArrowLanded()
    {
        shooting = false;

        ArrowController arrowCtrl = currentArrow?.GetComponent<ArrowController>();
        if (arrowCtrl != null)
            arrowCtrl.OnArrowHit -= OnArrowLanded;
    }
}