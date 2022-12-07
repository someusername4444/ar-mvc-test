using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IAppController
{
    public void Initialize();
    public void GoToMainMenu();
    public void GoToGame();
}

public class AppController : IAppController
{
    public IConfigManager configManager
    {
        get;
        set;
    }

    public ILogger logger
    {
        get;
        set;
    }

    public IARController arController
    {
        get;
        set;
    }

    private AppControllerConfig appControllerConfig;
    private Canvas canvas;
    EventSystem eventSystem;
    GameObject mainMenuView;
    GameObject inGameView;

    public void Initialize()
    {
        App.Instance.factory.ResolveDependencies(this);
        appControllerConfig = configManager.GetConfig<AppControllerConfig>();
        canvas = UnityEngine.Object.Instantiate(appControllerConfig.canvasPrefab);
        eventSystem = UnityEngine.Object.Instantiate(appControllerConfig.eventSystemPrefab);
        mainMenuView = UnityEngine.Object.Instantiate(appControllerConfig.mainMenuViewPrefab, canvas.transform);
        inGameView = UnityEngine.Object.Instantiate(appControllerConfig.inGameViewPrefab, canvas.transform);
        var mainMenuButton = mainMenuView.transform.Find("StartButton").GetComponent<Button>();
        mainMenuButton.onClick.AddListener(GoToGame);
        var goBackButton = inGameView.transform.Find("BackButton").GetComponent<Button>();
        goBackButton.onClick.AddListener(GoToMainMenu);
        arController.Initialize();
        logger.Info("App Controller Initiated");
    }

    public void GoToMainMenu()
    {
        logger.Info("Go to main menu");
        arController.Deactivate();
        inGameView.SetActive(false);
        mainMenuView.SetActive(true);
    }

    public void GoToGame()
    {
        logger.Info("Go to game");
        mainMenuView.SetActive(false);
        inGameView.SetActive(true);
        arController.Activate();
    }
}

