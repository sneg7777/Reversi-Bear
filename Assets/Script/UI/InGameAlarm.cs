using UnityEngine;

public enum AlarmKind
{
    AutoPassTurn,
    GameEnd,
}

public class InGameAlarm : MonoBehaviour
{
    private float alarmOnPosX;
    private float alarmOffPosX;
    private const float alarmDelay = 1f;

    [SerializeField] private RectTransform rect;
    [SerializeField] private GameObject textsAlarm;

    private bool isAlarm;
    private bool isAlarmShowComplete;
    private float xVelocity = 2.0f;
    private float alarmTick;

    private void Start()
    {
        alarmOnPosX = Screen.width - 10f;
        alarmOffPosX = Screen.width * 2f;
        rect.position = new Vector3(alarmOffPosX, rect.position.y, rect.position.z);
    }

    private void Update()
    {
        ProcessAlarm();
    }

    public void SetAlarm(AlarmKind kind)
    {
        isAlarm = true;
        isAlarmShowComplete = false;
        rect.position = new Vector3(alarmOffPosX, rect.position.y, rect.position.z);

        int alarmCount = textsAlarm.transform.childCount;
        for (int i = 0; i < alarmCount; i++)
        {
            textsAlarm.transform.GetChild(i).gameObject.SetActive(false);
        }
        textsAlarm.transform.GetChild((int)kind).gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    private void ProcessAlarm()
    {
        if (!isAlarm)
        {
            return;
        }

        float posX;
        if (!isAlarmShowComplete)
        {
            posX = Mathf.SmoothDamp(rect.position.x, alarmOnPosX, ref xVelocity, 0.4f);
            rect.position = new Vector3(posX, rect.position.y, rect.position.z);

            if (Mathf.Abs(alarmOnPosX - rect.position.x) < 0.1f)
            {
                isAlarmShowComplete = true;
                alarmTick = alarmDelay;
                rect.position = new Vector3(alarmOnPosX, rect.position.y, rect.position.z);
            }
        }
        else
        {
            alarmTick -= Time.deltaTime;
            if (alarmTick < 0f)
            {

                posX = Mathf.SmoothDamp(rect.position.x, alarmOffPosX, ref xVelocity, 0.4f);
                rect.position = new Vector3(posX, rect.position.y, rect.position.z);
                if (Mathf.Abs(alarmOffPosX - rect.position.x) < 0.1f)
                {
                    isAlarm = false;
                    isAlarmShowComplete = false;
                    rect.position = new Vector3(alarmOffPosX, rect.position.y, rect.position.z);
                    gameObject.SetActive(false);
                }
            }

        }
    }
}
