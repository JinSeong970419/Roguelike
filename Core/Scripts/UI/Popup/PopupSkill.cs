using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class PopupSkill : Popup
    {
        [SerializeField] private Color attackSkillColor;
        [SerializeField] private Color buffSkillColor;
        [SerializeField] private Color combinedSkillColor;

        [SerializeField] private List<Image> iconAttackSkills;
        [SerializeField] private List<Image> iconBuffSkills;



        protected override void OnEnable()
        {
            base.OnEnable();

            var player = GameManager.Instance.Player;
            if (player != null)
            {
                int attackSkillCount = player.AttackSkills.Count;
                for (int i = 0; i < attackSkillCount; i++)
                {
                    Skill skill = player.AttackSkills[i];
                    iconAttackSkills[i].sprite = skill.SkillInfo.Icon;
                    iconAttackSkills[i].color = Color.white;
                }

                int buffSkillCount = player.BuffSkills.Count;
                for (int i = 0; i < buffSkillCount; i++)
                {
                    Skill skill = player.BuffSkills[i];
                    iconBuffSkills[i].sprite = skill.SkillInfo.Icon;
                    iconBuffSkills[i].color = Color.white;
                }
            }
        }

        public void OnBackButtonClick()
        {
            Close();
        }
    }
}
