using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class PopupSkillSelect : Popup
    {
        [SerializeField] private SkillSlotItem[] skillSlotItems = new SkillSlotItem[3];

        private UnityAction[] onClickList = new UnityAction[3];

        private int activeCount = 0;

        public void Initialize()
        {
            for (int i = 0; i < skillSlotItems.Length; i++)
            {
                var skillSlotItem = skillSlotItems[i];
                skillSlotItem.Slot.onClick.RemoveAllListeners();
                skillSlotItem.Slot.gameObject.SetActive(false);
            }

            onClickList[0] = OnClickSlot01;
            onClickList[1] = OnClickSlot02;
            onClickList[2] = OnClickSlot03;

            activeCount = 0;
        }

        public void SetRandomSkills()
        {
            var player = GameManager.Instance.Player;
            List<SkillInfo> skills = player.GetRandomSkills(3);

            if (skills == null) return;
            if (skills.Count == 0) return;


            player.MoveStop();
            GameManager.Instance.Pause();

            int count = skills.Count;
            for (int i = 0; i < count; i++)
            {
                SkillInfo info = skills[i];
                Skill skill = player.FindSkill(info.Kind);
                Level skillLevel = Level._01;
                if (skill == null)
                {
                    skillLevel = Level._01;
                }
                else
                {
                    skillLevel = skill.Level + 1;
                }
                SetSkill(i, info, skillLevel, player.OnSkillSelect);
            }
        }

        public void SetSkill(int index, SkillInfo skillInfo, Level skillLevel, UnityAction<SkillInfo> callback)
        {
            var skillSlotItem = skillSlotItems[index];
            var skillStat = skillInfo.Stats[(int)skillLevel];
            skillSlotItem.Slot.onClick.AddListener(onClickList[index]);
            skillSlotItem.Icon.sprite = skillInfo.Icon;
            skillSlotItem.Name.text = skillInfo.Name;
            skillSlotItem.Description.text = skillStat.Description;
            skillSlotItem.SkillInfo = skillInfo;
            skillSlotItem.Callback = callback;
            skillSlotItem.Slot.gameObject.SetActive(true);
            
             activeCount++;
        }

        public void Refresh()
        {
            if (activeCount == 3)
            {
                var rt = skillSlotItems[0].Slot.GetComponent<RectTransform>();
                var prt = rt.parent as RectTransform;
                prt.offsetMin = new Vector2(50f, prt.offsetMin.y);
                prt.offsetMax = new Vector2(-750f, prt.offsetMax.y);
                var rt2 = skillSlotItems[1].Slot.GetComponent<RectTransform>();
                var prt2 = rt2.parent as RectTransform;
                prt2.offsetMin = new Vector2(400f, prt2.offsetMin.y);
                prt2.offsetMax = new Vector2(-400f, prt2.offsetMax.y);
                var rt3 = skillSlotItems[2].Slot.GetComponent<RectTransform>();
                var prt3 = rt3.parent as RectTransform;
                prt3.offsetMin = new Vector2(750f, prt3.offsetMin.y);
                prt3.offsetMax = new Vector2(-50f, prt3.offsetMax.y);
            }
            else if(activeCount == 2)
            {
                var rt = skillSlotItems[0].Slot.GetComponent<RectTransform>();
                var prt = rt.parent as RectTransform;
                prt.offsetMin = new Vector2(50f, prt.offsetMin.y);
                prt.offsetMax = new Vector2(-560f, prt.offsetMax.y);
                var rt2 = skillSlotItems[1].Slot.GetComponent<RectTransform>();
                var prt2 = rt2.parent as RectTransform;
                prt2.offsetMin = new Vector2(560f, prt2.offsetMin.y);
                prt2.offsetMax = new Vector2(-50f, prt2.offsetMax.y);
            }
            else if(activeCount == 1)
            {
                var rt = skillSlotItems[0].Slot.GetComponent<RectTransform>();
                var prt = rt.parent as RectTransform;
                prt.offsetMin = new Vector2(300f, prt.offsetMin.y);
                prt.offsetMax = new Vector2(-300f, prt.offsetMax.y);
            }
            
        }

        private void OnClickSlot01()
        {
            skillSlotItems[0].Callback.Invoke(skillSlotItems[0].SkillInfo);
        }
        private void OnClickSlot02()
        {
            skillSlotItems[1].Callback.Invoke(skillSlotItems[1].SkillInfo);
        }
        private void OnClickSlot03()
        {
            skillSlotItems[2].Callback.Invoke(skillSlotItems[2].SkillInfo);
        }

        public void OnClickRefreshButton()
        {
            Initialize();
            SetRandomSkills();
            Refresh();
        }
    }

    [System.Serializable]
    public class SkillSlotItem
    {
        [SerializeField] private Button slot;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text description;

        public Button Slot { get { return slot; } }
        public Image Icon { get { return icon; } set { icon = value; } }
        public TMP_Text Name { get { return name; } set { name = value; } }
        public TMP_Text Description { get { return description; } set { description = value; } }
        public SkillInfo SkillInfo { get; set; }
        public UnityAction<SkillInfo> Callback { get; set; }
    }
}
