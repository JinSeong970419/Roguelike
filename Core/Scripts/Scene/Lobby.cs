using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Roguelike.Core
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private Account account;

        [Header("Account")]
        [SerializeField] private TMP_Text textNickname;
        [SerializeField] private TMP_Text textLevel;
        [SerializeField] private Image imageExpGauge;
        [SerializeField] private TMP_Text textStamina;
        [SerializeField] private TMP_Text textDiamond;
        [SerializeField] private TMP_Text textGold;
        [Header("Stage")]
        [SerializeField] private Image imageStage;
        [SerializeField] private Variable<int> selectedStageIndex;
        [SerializeField] private Button buttonPrevStage;
        [SerializeField] private Button buttonNextStage;
        [Header("Navigation")]
        //[SerializeField] private HorizontalScrollSnap horizontalScrollSnap;
        [SerializeField] private ScrollPanel horizontalScrollPanel;

        private void Start()
        {
            //Debug.Log("Lobby Start"); 
            //horizontalScrollSnap.UpdateLayout();
            Time.timeScale = 1.0f;
        }

        private void OnEnable()
        {
            Refresh();

            account.Nickname.OnValueChanged.AddListener(OnNicknameChanged);
            account.Level.OnValueChanged.AddListener(OnLevelChanged);
            account.MaxExp.OnValueChanged.AddListener(OnMaxExpChanged);
            account.Exp.OnValueChanged.AddListener(OnExpChanged);
            account.MaxStamina.OnValueChanged.AddListener(OnMaxStaminaChanged);
            account.Stamina.OnValueChanged.AddListener(OnStaminaChanged);
            account.Diamond.OnValueChanged.AddListener(OnDiamondChanged);
            account.Gold.OnValueChanged.AddListener(OnGoldChanged);

            selectedStageIndex.OnValueChanged.AddListener(OnSelectedStageIndexChanged);
        }

        private void OnDisable()
        {
            account.Nickname.OnValueChanged.RemoveListener(OnNicknameChanged);
            account.Level.OnValueChanged.RemoveListener(OnLevelChanged);
            account.MaxExp.OnValueChanged.RemoveListener(OnMaxExpChanged);
            account.Exp.OnValueChanged.RemoveListener(OnExpChanged);
            account.MaxStamina.OnValueChanged.RemoveListener(OnMaxStaminaChanged);
            account.Stamina.OnValueChanged.RemoveListener(OnStaminaChanged);
            account.Diamond.OnValueChanged.RemoveListener(OnDiamondChanged);
            account.Gold.OnValueChanged.RemoveListener(OnGoldChanged);

            selectedStageIndex.OnValueChanged.RemoveListener(OnSelectedStageIndexChanged);
        }

        public void Refresh()
        {
            OnNicknameChanged(account.Nickname.Value);
            OnLevelChanged(account.Level.Value);
            OnMaxExpChanged(account.MaxExp.Value);
            OnExpChanged(account.Exp.Value);
            OnMaxStaminaChanged(account.MaxStamina.Value);
            OnStaminaChanged(account.Stamina.Value);
            OnDiamondChanged(account.Diamond.Value);
            OnGoldChanged(account.Gold.Value);

            OnSelectedStageIndexChanged(selectedStageIndex.Value);
        }

        private void OnNicknameChanged(string nickname)
        {
            textNickname.text = nickname;
        } 

        private void OnLevelChanged(int level)
        {
            textLevel.text = "Lv." + level.ToString();
        }

        private void OnMaxExpChanged(float maxExp)
        {
            if (maxExp != 0f)
            {
                float ratio = account.Exp.Value / maxExp;
                imageExpGauge.fillAmount = ratio;
            }
            else
            {
                imageExpGauge.fillAmount = 0f;
            }
        }

        private void OnExpChanged(float exp)
        {
            if (account.MaxExp.Value != 0f)
            {
                float ratio = exp / account.MaxExp.Value;
                imageExpGauge.fillAmount = ratio;
            }
            else 
            {
                imageExpGauge.fillAmount = 0f;
            }
        }

        private void OnMaxStaminaChanged(float maxStamina)
        {
            textStamina.text = account.Stamina.Value.ToString() + " / " + maxStamina.ToString();
        }

        private void OnStaminaChanged(float stamina)
        {
            textStamina.text = stamina.ToString() + " / " + account.MaxStamina.Value.ToString();
        }

        private void OnDiamondChanged(int diamond)
        {
            textDiamond.text = diamond.ToString();
        }

        private void OnGoldChanged(int gold)
        {
            textGold.text = gold.ToString();
        }

        private void OnSelectedStageIndexChanged(int index)
        {
            imageStage.sprite = DataManager.Instance.StageSettings.StageInfos[index].Icon;
        }

        public void OnPrevStageButtonClick()
        {
            int count = (int)StageKind.End;
            int index = selectedStageIndex.Value == 0 ? count - 1 : selectedStageIndex.Value - 1;

            account.SelectedStageIndex.Value = index;
        }

        public void OnNextStageButtonClick()
        {
            int count = (int)StageKind.End;
            int index = selectedStageIndex.Value == count-1 ? 0 : selectedStageIndex.Value + 1;

            account.SelectedStageIndex.Value = index;
        }

        public void OnProfileClick()
        {
            PopupManager.Instance.OpenPopup((int)PopupKind.PopupProfile);
        }

        public void OnEnterStageButtonClick()
        {
            FirestoreManager.Instance.SetUserData(account.ToUserData());
            GameManager.Instance.EnterStage();
        }

        public void ShowLeaderboard()
        {
            PopupManager.Instance.OpenPopup(PopupKind.PopupRanking);
        }

        public void OnPage01Click()
        {
            //horizontalScrollSnap.ChangePage(0);
            horizontalScrollPanel.ChangePage(0);
        }

        public void OnPage02Click()
        {
            //horizontalScrollSnap.ChangePage(1);
            horizontalScrollPanel.ChangePage(1);
        }

        public void OnPage03Click()
        {
            //horizontalScrollSnap.ChangePage(2);
            horizontalScrollPanel.ChangePage(2);
        }

        public void OnPage04Click()
        {
            //horizontalScrollSnap.ChangePage(3);
            horizontalScrollPanel.ChangePage(3);
        }

        public void OnPage05Click()
        {
            //horizontalScrollSnap.ChangePage(4);
            horizontalScrollPanel.ChangePage(4);
        }
    }
}
