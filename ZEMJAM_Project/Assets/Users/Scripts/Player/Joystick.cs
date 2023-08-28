using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    Movement m_Movement;
    Collison m_Collison;

    [SerializeField] RectTransform joyPanel;
    [SerializeField] RectTransform joyStick;
    [SerializeField] RectTransform powerBar;
    [SerializeField] Image powerFill;
    [SerializeField] Text powerText;
    [SerializeField] Transform directionArrow;

    Camera mainCamera;

    Touch touch;
    Vector2 inputTouchPos;
    Vector2 startTouchPos;
    Vector2 endTouchPos;

    bool isTouchDown;

    public float maxPower;
    float angle, power, fill;

    void Start()
    {
        m_Movement = GetComponent<Movement>();
        m_Collison = GetComponent<Collison>();
        mainCamera = Camera.main;
    }
    void Update()
    {
        var isTouch = (Input.GetMouseButton(0) || (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary));

        if (m_Movement.m_Count < 0)
        {
            if (Input.GetMouseButton(0) || (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
            {
#if (UNITY_EDITOR)
                inputTouchPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
#elif (UNITY_ANDROID)
                inputTouchPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, -Camera.main.transform.position.z));
#endif

                if (!isTouchDown)
                {
                    SetUIActive(true);
                    isTouchDown = true;
                    startTouchPos = inputTouchPos;
                }


                SetDistance();
                angle = Mathf.Atan2(endTouchPos.y - startTouchPos.y, endTouchPos.x - startTouchPos.x) * Mathf.Rad2Deg;
                directionArrow.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

                powerBar.transform.position = transform.position + new Vector3(transform.position.x > 0 ? -0.7f : 0.7f, transform.position.y > 0 ? -0.4f : 0.4f);
                fill = Mathf.Lerp(fill, power / maxPower, Time.deltaTime * 12);
                powerFill.fillAmount = fill;
                powerText.text = power.ToString();

                joyPanel.position = startTouchPos;
                joyStick.position = endTouchPos;

                var inputDir = endTouchPos - startTouchPos;
                var inputMag = inputDir.magnitude;
                var clampedDir = inputMag <= 1.5f ? inputDir : inputDir.normalized * 1.51f;
                directionArrow.transform.localScale = new Vector3(1, (inputMag <= 1.51f ? inputMag : 1.5f) * 2);

                joyStick.position = clampedDir + startTouchPos;
            }
            if (Input.GetMouseButtonUp(0) || touch.phase == TouchPhase.Ended)
            {
                m_Movement.Dash(power, angle);
                SetUIActive(false);
                isTouchDown = false;
            }
        }
    }
    void SetDistance()
    {
        endTouchPos = inputTouchPos;

        Vector3 offset = startTouchPos - endTouchPos;
        float sqrlen = offset.sqrMagnitude;
        var len = Mathf.Sqrt(sqrlen);
        power = (int)((len + 0.5f) * maxPower / 1.5f);
        if (power > maxPower)
            power = maxPower;

        SetAngle();
    }
    private void SetAngle()
    {
        angle = Mathf.Atan2(endTouchPos.y - startTouchPos.y, endTouchPos.x - startTouchPos.x) * Mathf.Rad2Deg;

        if (m_Collison.onDown && m_Collison.onLeft)
        {
            angle = angle <= -177 && angle >= 0 ? -177 : angle;
            angle = angle >= -93 && angle < 0 ? -93 : angle;
        }
        else if (m_Collison.onDown && m_Collison.onRight)
        {
            angle = angle >= -3 && angle <= 180 ? -3 : angle;
            angle = angle <= -87 && angle > -180 ? -87 : angle;
        }
        else if (m_Collison.onUp && m_Collison.onLeft)
        {
            angle = angle <= 93 && angle >= -90 ? 93 : angle;
            angle = angle >= 177 || angle < -90 ? 177 : angle;
        }
        else if (m_Collison.onUp && m_Collison.onRight)
        {
            angle = angle >= 87 && angle <= -135 ? 87 : angle;
            angle = angle <= 3 && angle > -135 ? 3 : angle;
        }
        else if (m_Collison.onUp)
        {
            angle = angle <= 3 && angle >= -90 ? 3 : angle;
            angle = angle >= 177 || angle < -90 ? 177 : angle;
        }
        else if (m_Collison.onDown)
        {
            angle = angle >= -3 && angle <= 90 ? -3 : angle;
            angle = angle <= -177 || angle > 90 ? -177 : angle;
        }
        else if (m_Collison.onLeft)
        {
            angle = angle <= 93 && angle >= 0 ? 93 : angle;
            angle = angle >= -93 && angle < 0 ? -93 : angle;
        }
        else if (m_Collison.onRight)
        {
            angle = angle >= 87 && angle <= 180 ? 87 : angle;
            angle = angle <= -87 && angle > -180 ? -87 : angle;
        }
    }
    void SetUIActive(bool value)
    {
        joyPanel.gameObject.SetActive(value);
        powerBar.gameObject.SetActive(value);
        directionArrow.gameObject.SetActive(value);
    }
}
