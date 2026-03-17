using UnityEngine;
using UnityEngine.InputSystem;

//AI가 시간 배속하는거 만들어줌
public class TimeManager : MonoBehaviour
{
    [Header("시간 배속 설정")]
    public float[] timeSteps = { 1f, 2f, 4f, 8f };
    private int currentStepIndex = 0; 

    [Header("부드러운 전환")]
    public bool useLerp = true;
    public float lerpSpeed = 10f;

    private float targetScale = 1f;

    public void OnPressLM(InputValue value)
    {
        if (value.isPressed)
        {
            currentStepIndex++;

            if (currentStepIndex >= timeSteps.Length)
            {
                currentStepIndex = 0;
            }

            targetScale = timeSteps[currentStepIndex];
        }
    }

    void Update()
    {
        if (useLerp)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetScale, Time.unscaledDeltaTime * lerpSpeed);
        }
        else
        {
            Time.timeScale = targetScale;
        }

        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}