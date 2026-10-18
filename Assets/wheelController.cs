using System.Collections;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [Header("旋轉時間設定")]
    [SerializeField] private float maxSpeed = 1000f;       // 最高旋轉速度 (度/秒)
    [SerializeField] private float accelerateTime = 2f;    // 加速到最高速所需時間 (秒)
    [SerializeField] private float decelerateTime = 4f;    // 從最高速到停止所需時間 (秒)

    [Header("基礎音效設定")]
    [SerializeField] private AudioSource audioSource;      // 聲音來源組件
    [SerializeField] private AudioClip tickSound;         // 轉動時的「噠噠」單音效果
    [SerializeField] private float anglePerTick = 30f;     // 每轉多少度播一次音效 (依輪盤格子數調整)

    [Header("最終停止音效")]
    [SerializeField] private AudioClip stopSound;         // 輪盤完全停下時的慶祝/中獎音效

    private bool isSpinning = false;
    private float currentSpeed = 0f;
    private float lastTickAngle = 0f;

    void Start()
    {
        // 如果 Inspector 沒有手動拖曳 AudioSource，自動從同個物件上取得
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // 安全機制：確保 Play On Awake 是關閉的
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // 功能 1：完美相容新版與舊版 Input System 的空白鍵偵測
        #if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null && 
            UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame && 
            !isSpinning)
        {
            StartSpinning();
        }
        #else
        if (Input.GetKeyDown(KeyCode.Space) && !isSpinning)
        {
            StartSpinning();
        }
        #endif
    }

    public void StartSpinning()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinRoutine());
        }
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        lastTickAngle = transform.eulerAngles.z;

        // --- 1. 加速階段 ---
        float timer = 0f;
        while (timer < accelerateTime)
        {
            timer += Time.deltaTime;
            currentSpeed = Mathf.Lerp(0f, maxSpeed, timer / accelerateTime);
            RotateWheel();
            yield return null;
        }

        // --- 2. 減速階段 ---
        timer = 0f;
        float startDecelSpeed = currentSpeed;
        while (timer < decelerateTime)
        {
            timer += Time.deltaTime;
            float t = timer / decelerateTime;
            float smoothT = Mathf.SmoothStep(0f, 1f, t); // 使用 SmoothStep 讓快停下的物理感更逼真
            currentSpeed = Mathf.Lerp(startDecelSpeed, 0f, smoothT);
            
            RotateWheel();
            yield return null;
        }

        // --- 3. 確保完全停止並觸發功能 3（停止音效） ---
        currentSpeed = 0f;
        isSpinning = false;

        if (audioSource != null)
        {
            // 雙重保險：先強制切斷正在播放或殘留的「噠噠」轉動聲，避免聲音互蓋
            audioSource.Stop(); 

            if (stopSound != null)
            {
                // 功能 3：播放最終停止音效
                audioSource.PlayOneShot(stopSound);
                Debug.Log("【控制台提示】輪盤已完全靜止，成功播放停止中獎音效！");
            }
            else
            {
                // 如果你忘記拉音效進欄位，這裡會跳出顯眼的警告
                Debug.LogWarning("【未填寫警告】輪盤停下了，但是你的 Stop Sound 欄位是空的！請記得把音效檔案拉進去！");
            }
        }
    }

    private void RotateWheel()
    {
        float angleToRotate = currentSpeed * Time.deltaTime;
        transform.Rotate(0, 0, angleToRotate);

        // 功能 2：轉動音效邏輯
        if (audioSource != null && tickSound != null)
        {
            float currentAngle = transform.eulerAngles.z;
            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(lastTickAngle, currentAngle));

            if (angleDiff >= anglePerTick)
            {
                audioSource.PlayOneShot(tickSound);
                lastTickAngle = currentAngle;
            }
        }
    }
}