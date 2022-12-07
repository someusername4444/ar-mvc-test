public class App
{
    public static App Instance
    {
        get;
        private set;
    }

    public static void Startup(IUpdateManager updateManager)
    {
        if (Instance == null)
        {
            Instance = new App();
        }
        Instance.Initialize(updateManager);
    }

    public IFactory factory
    {
        get;
        private set;
    }

    public ILogger logger
    {
        get;
        set;
    }

    public IConfigManager configManager
    {
        get;
        set;
    }

    private App()
    {
        var factory = new Factory();
        this.factory = factory;
    }

    public void Initialize(IUpdateManager updateManager)
    {
        var appController = new AppController();
        factory.AddDependency<IUpdateManager>(updateManager);
        factory.AddDependency<ILogger>(new Logger());
        factory.AddDependency<IConfigManager>(new ConfigManager());
        factory.AddDependency<IARController>(new ARController());
        appController.Initialize();
        appController.GoToMainMenu();
        factory.AddDependency<IAppController>(appController);
        factory.ResolveDependencies(this);
        logger.Info("App Initiated");
    }
}

