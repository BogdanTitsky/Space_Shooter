using Data;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISavedLoadService _saveLoadService;
        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISavedLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }
        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.worldData.levelToLoad); 
        }
        public void Exit()
        {
        }
        private void LoadProgressOrInitNew() => 
            _progressService.Progress = 
                _saveLoadService.LoadProgress() 
                ?? NewProgress();
        private PlayerProgress NewProgress() => 
            new PlayerProgress("Level 1");
    }
}
