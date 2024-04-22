using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region GameEvents
        [Header("System")]
        [SerializeField] private GameEventVoid _pauseEvent;
        [SerializeField] private GameEventVoid _resumeEvent;
        [SerializeField] private GameEvent<MoveStartEventArgs> _moveStartEvent;
        [SerializeField] private GameEvent<MoveEndEventArgs> _moveEndEvent;

        [SerializeField] private GameEvent<string> _sceneChangedEvent;
        [Header("Stage")]
        [SerializeField] private GameEvent<StageKind> _selectStageEvent;
        [SerializeField] private GameEventVoid _enterStageEvent;
        [SerializeField] private GameEvent<float> _changeSpawnDelayEvent;
        [SerializeField] private GameEvent<float> _showBossAlertEvent;

        [Header("Player")]
        [SerializeField] private GameEvent<SkillKind> _selectSkillEvent;
        #endregion

        #region Variables
        [SerializeField] private Variable<float> _playTime;
        [Header("Stage")]
        [SerializeField] private Variable<int> _selectedStageIndex;
        public Variable<float> PlayTime { get { return _playTime; } }
        #endregion

        #region Global
        private bool _isPaused;
        public bool IsPaused { get { return _isPaused; } }

        [Header("Canvas")]
        [SerializeField] private Canvas _canvas;
        [Header("Joystick")]
        [SerializeField] private GameObject _joystickPrefab;
        private Joystick _joystick;
        #endregion

        #region Player
        private Actor _player;
        public Actor Player { get { return _player; } }
        #endregion

        #region Camera
        private Camera _mainCamera;
        private GameCamera _camera;
        #endregion

        #region Monster
        public Actor NearestMonster
        {
            get
            {
                if (_currentStage == null) return null;
                return _currentStage.NearestMonster;
            }
        }

        public List<Actor> Monsters
        {
            get
            {
                if (_currentStage == null) return null;
                return _currentStage.Monsters;
            }
        }
        #endregion

        #region Stage
        [SerializeField] private StageKind _currentStageKind = StageKind.End;
        [SerializeField] private StageKind _nextStageKind = StageKind.End;
        private IStage _currentStage;

        public IStage CurrentStage { get { return _currentStage; } }

        private bool _isGameScene = false;
        #endregion


        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            _joystick?.OnMove.AddListener(OnJoystickMove);
            _joystick?.OnExit.AddListener(OnJoystickExit);

            _sceneChangedEvent.AddListener(OnSceneChanged);

            _selectStageEvent.AddListener(SelectStage);
            _enterStageEvent.AddListener(EnterStage);

            _pauseEvent.AddListener(Pause);
            _resumeEvent.AddListener(Resume);

            _changeSpawnDelayEvent.AddListener(ChangeSpawnDelay);
        }

        private void OnDisable()
        {
            _joystick?.OnMove.RemoveListener(OnJoystickMove);
            _joystick?.OnExit.RemoveListener(OnJoystickExit);

            _sceneChangedEvent.RemoveListener(OnSceneChanged);

            _selectStageEvent.RemoveListener(SelectStage);
            _enterStageEvent.RemoveListener(EnterStage);

            _pauseEvent.RemoveListener(Pause);
            _resumeEvent.RemoveListener(Resume);

            _changeSpawnDelayEvent.RemoveListener(ChangeSpawnDelay);
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (_isGameScene == false) return;
            ProcessStage();
        }

        #region System

        public void Initilaize()
        {
            _mainCamera = Camera.main;
            _camera = _mainCamera.GetComponent<GameCamera>();
            _canvas = FindObjectOfType<Canvas>();

            var joystickObject = Instantiate(_joystickPrefab, _canvas.transform);
            _joystick = joystickObject.GetComponent<Joystick>();

            _joystick.OnMove.AddListener(OnJoystickMove);
            _joystick.OnExit.AddListener(OnJoystickExit);
        }

        public void Release()
        {
            _joystick.OnMove.RemoveListener(OnJoystickMove);
            _joystick.OnExit.RemoveListener(OnJoystickExit);
            Destroy(_joystick);
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            _isPaused = true;
            _joystick?.Pause();
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            _isPaused = false;
            _joystick?.Resume();
        }
        #endregion

        #region Stage

        public void SelectStage(StageKind stageKind)
        {
            Instance._nextStageKind = stageKind;
        }

        public void EnterStage()
        {
            _nextStageKind = (StageKind)_selectedStageIndex.Value;
            SceneController.LoadScene("InGame");
        }

        private void ProcessStage()
        {
            if (_nextStageKind != _currentStageKind)
            {
                if (_currentStage != null)
                {
                    _currentStage.Release();
                }
                _currentStageKind = _nextStageKind;

                if (_currentStageKind != StageKind.End)
                {
                    StageInfo info = DataManager.Instance.StageSettings.StageInfos[(int)_currentStageKind];
                    _currentStage = IStage.Create(info.Type, _currentStageKind);
                    _currentStage.Initialize();
                }
                else
                {
                    _currentStage = null;
                }
                return;
            }

            if (_currentStage != null)
            {
                _currentStage.Update();
            }
        }

        public void ShowBossAlert(float second)
        {
            _showBossAlertEvent.Invoke(second);
        }
        #endregion

        #region Entity
        public void CreatePlayer()
        {
            GameObject playerObj = ObjectPool.Instance.Allocate(EntityType.Actor);
            _player = playerObj.GetComponent<Actor>();
            _player.Stat = new ActorStats(DataManager.Instance.ActorSettings.ActorInfos[(int)ActorKind.Yumi01].Stats);
            _player.Team = Team.Alliance;
            _player.AddSkill(DataManager.Instance.PlayerInfo.InitialSkillKind);
            _player.SetActorKind(ActorKind.Yumi01);
            _player.ShowHpBar();
            _player.ShowDirectionIndicator();
            _camera.SetTarget(_player.gameObject);
            //CollisionManager.Instance.Target = playerObj;
        }

        #endregion

        #region Game

        public void UpdateSkills(SkillKind kind)
        {
            _selectSkillEvent.Invoke(kind);
        }

        public void GiveUp()
        {

        }

        #endregion

        #region Callback
        private void OnSceneChanged(string sceneName)
        {
            _isGameScene = SceneController.Instance.IsGameScene;
            if (_isGameScene == false)
            {
                _currentStageKind = StageKind.End;
            }
            else
            {
                Resume();
            }

        }

        public void OnPlayerDie()
        {
            _player = null;
        }

        private void ChangeSpawnDelay(float delay)
        {
            if (_currentStage == null) return;
        }

        #endregion


        #region InputAction

        public void OnJoystickMove(Vector2 input)
        {
            if (_player == null) return;
            var args = new MoveStartEventArgs() { EntityID = _player.Id, Direction = input };
            _player.OnMoveStart(args);
        }

        public void OnJoystickExit()
        {
            if (_player == null) return;
            var args = new MoveEndEventArgs() { EntityID = _player.Id };
            _player.OnMoveEnd(args);
        }

        #endregion

    }
}
