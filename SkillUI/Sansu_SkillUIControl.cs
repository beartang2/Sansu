using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sansu_SkillUIControl : MonoBehaviour
{
    public GameObject[] hideSkillButton;
    public GameObject[] textPros;
    public List<TextMeshProUGUI> hideSkillTimeTexts;
    public Image[] hideSkillImage;
    private bool[] isHideSkills = { false, false };
    private float[] skillTimes = { 5, 3 };
    private float[] getSkillTimes = { 0, 0 };

    // Update is called once per frame
    void Update()
    {
        HideSkillChk();

        CheckKeyboardInput();
    }

    void CheckKeyboardInput()
    {        
        if (Input.GetKeyDown(KeyCode.Q) && !isHideSkills[0])
        {
            HideSkillSetting(0);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isHideSkills[1])
        {
            HideSkillSetting(1);
        }
    }

    public void HideSkillSetting(int skillNum) 
    {
        hideSkillButton[skillNum].SetActive(true);
        getSkillTimes[skillNum] = skillTimes[skillNum];
        isHideSkills[skillNum] = true;
    }

    void HideSkillChk()
    {
        for (int i = 0; i < isHideSkills.Length; i++)
        {
            if (isHideSkills[i])
            {
                if (getSkillTimes[i] <= 0)
                {
                    isHideSkills[i] = false;
                    hideSkillButton[i].SetActive(false);
                    getSkillTimes[i] = 0;
                }
                else
                {
                    getSkillTimes[i] -= Time.deltaTime;
                    hideSkillTimeTexts[i].text = Mathf.CeilToInt(getSkillTimes[i]).ToString("0");
                    hideSkillImage[i].fillAmount = getSkillTimes[i] / skillTimes[i];
                }
            }
        }
    }
}