using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace Roguelike.Core
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [Header("Event")]
        [SerializeField] private GameEventVoid _saveGameSettingsEvent;
        [SerializeField] private GameEventVoid _loadGameSettingsEvent;

        [Header("Variable")]
        [SerializeField] private Variable<SystemLanguage> _language;

        [Header("Account")]
        [SerializeField] private Account _account;

        [Header("Player")]
        [SerializeField] private PlayerInfo _playerInfo;

        [Header("Settings")]
        [SerializeField] private GameSettings _gameSettings;

        [SerializeField] private UserSettings _userSettings;

        [SerializeField] private StageSettings _stageSettings;
        [SerializeField] private ActorSettings _actorSettings;
        [SerializeField] private SkillSettings _skillSettings;
        [SerializeField] private ItemSettings _itemSettings;
        [SerializeField] private DropItemSettings _dropItemSettings;
        [SerializeField] private EffectSettings _effectSettings;
        [SerializeField] private DamageFontSettings _damageFontSettings;
        [SerializeField] private DamageAreaSettings _damageAreaSettings;
        [SerializeField] private LocalizationSettings _localizationSettings;

        [Header("Excel")]
        [SerializeField] private ExcelTableCollection _actorStatTableCollection;
        [SerializeField] private ExcelTableCollection _skillStatTableCollection;
        [SerializeField] private ExcelTableCollection _skillCombinationTableCollection;
        [SerializeField] private ExcelTableCollection _localizationTableCollection;

        [SerializeField] private DataStorage _storage;


        public Account Account { get { return _account; } }
        public GameSettings GameSettings { get { return _gameSettings; } }
        public UserSettings UserSettings { get { return _userSettings; } }
        public PlayerInfo PlayerInfo { get { return _playerInfo; } }
        public StageSettings StageSettings { get { return _stageSettings; } }
        public ActorSettings ActorSettings { get { return _actorSettings; } }
        public SkillSettings SkillSettings { get { return _skillSettings; } }
        public ItemSettings ItemSettings { get { return _itemSettings; } }
        public DropItemSettings DropItemSettings { get { return _dropItemSettings; } }
        public EffectSettings EffectSettings { get { return _effectSettings; } }
        public DamageFontSettings DamageFontSettings { get { return _damageFontSettings; } }
        public DamageAreaSettings DamageAreaSettings { get { return _damageAreaSettings; } }
        public LocalizationSettings LocalizationSettings { get { return _localizationSettings; } }
        public ExcelTableCollection ActorStatTableCollection { get { return _actorStatTableCollection; } }
        public ExcelTableCollection SkillStatsTableCollection { get { return _skillStatTableCollection; } }
        public ExcelTableCollection SkillCombinationTableCollection { get { return _skillCombinationTableCollection; } }
        public ExcelTableCollection LocalizationTableCollection { get { return _localizationTableCollection; } }
        public DataStorage Storage { get { return _storage; } }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            if (_userSettings == null)
            {
                Debug.LogError("[DataManager] You must create [UserSettings].");
            }
        }

        private void OnEnable()
        {
            _saveGameSettingsEvent.AddListener(SaveUserSettings);
            _loadGameSettingsEvent.AddListener(LoadUserSettings);

            _language.OnValueChanged.AddListener(OnLanguageChanged);
        }

        private void OnDisable()
        {
            _saveGameSettingsEvent.RemoveListener(SaveUserSettings);
            _loadGameSettingsEvent.RemoveListener(LoadUserSettings);

            _language.OnValueChanged.RemoveListener(OnLanguageChanged);
        }

        public void OnLanguageChanged(SystemLanguage language)
        {
            _userSettings.Language = language;
        }

        [DebugButton]
        public void SaveUserSettings()
        {
            _userSettings.Save();
        }

        [DebugButton]
        public void LoadUserSettings()
        {
            _userSettings.Load();
        }

        [DebugButton]
        public void PullAllData()
        {
#if UNITY_EDITOR
            ActorStatTableCollection.Pull();
            SkillStatsTableCollection.Pull();
            SkillCombinationTableCollection.Pull();
            LocalizationTableCollection.Pull();
#endif
        }

    }
}

