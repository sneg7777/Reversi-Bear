using UnityEngine;

public enum AlarmKind
{
    AutoPassTurn,
    GameEnd,
}

public class InGameAlarm : MonoBehaviour
{
    public const float AlarmOnPosX = 2.737921f;
    public const float AlarmOffPosX = 6.5f;
    public const float AlarmDelay = 1f;

    [SerializeField] private RectTransform rect;
    [SerializeField] private GameObject textsAlarm;

    private bool isAlarm;
    private bool isAlarmShowComplete;
    private float xVelocity = 2.0f;
    private float alarmTick;


    private void Update()
    {
        ProcessAlarm();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetAlarm(AlarmKind.AutoPassTurn);
        }
    }

    public void SetAlarm(AlarmKind kind)
    {
        isAlarm = true;
        isAlarmShowComplete = false;
        rect.position = new Vector3(AlarmOffPosX, rect.position.y, rect.position.z);

        int alarmCount = textsAlarm.transform.childCount;
        for (int i = 0; i < alarmCount; i++)
        {
            textsAlarm.transform.GetChild(i).gameObject.SetActive(false);
        }
        textsAlarm.transform.GetChild((int)kind).gameObject.SetActive(true);
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
            posX = Mathf.SmoothDamp(rect.position.x, AlarmOnPosX, ref xVelocity, 0.4f);
            rect.position = new Vector3(posX, rect.position.y, rect.position.z);

            if (Mathf.Abs(AlarmOnPosX - rect.position.x) < 0.01f)
            {
                isAlarmShowComplete = true;
                alarmTick = AlarmDelay;
                rect.position = new Vector3(AlarmOnPosX, rect.position.y, rect.position.z);
            }
        }
        else
        {
            alarmTick -= Time.deltaTime;
            if (alarmTick < 0f)
            {

                posX = Mathf.SmoothDamp(rect.position.x, AlarmOffPosX, ref xVelocity, 0.4f);
                rect.position = new Vector3(posX, rect.position.y, rect.position.z);
                if (Mathf.Abs(AlarmOffPosX - rect.position.x) < 0.01f)
                {
                    isAlarm = false;
                    isAlarmShowComplete = false;
                    rect.position = new Vector3(AlarmOffPosX, rect.position.y, rect.position.z);
                }
            }

        }
    }
}
