namespace BD.WTTS.UI;

public sealed partial class App : Application
{
    const string TAG = "AvaloniaApp";

    public Window? MainWindow { get; set; }

    public override void Initialize()
    {
        try
        {
            AvaloniaXamlLoader.Load(this);
        }
        catch (Exception ex)
        {
            Log.Error(TAG, ex, "load Xaml fail.");
        }

        IApplication.IDesktopProgramHost desktopProgramHost = Program.Host.Instance;
        desktopProgramHost.OnCreateAppExecuted(handlerViewModelManager: vmService =>
        {
            //            switch (vmService.MainWindow)
            //            {
            //                case CloudArchiveWindowViewModel:
            //                    ProgramHost.IsMinimize = false;
            //                    MainWindow = new CloudArchiveWindow();
            //                    break;

            //                case AchievementWindowViewModel:
            //                    ProgramHost.IsMinimize = false;
            //                    MainWindow = new AchievementWindow();
            //                    break;

            //                default:
            //                    #region 主窗口启动时加载的资源
            //#if !UI_DEMO
            //                    compositeDisposable.Add(SettingsHost.Save);
            //                    compositeDisposable.Add(ProxyService.Current.Exit);
            //                    compositeDisposable.Add(SteamConnectService.Current.Dispose);
            //                    compositeDisposable.Add(ASFService.Current.StopASF);
            //#pragma warning disable CA1416 // 验证平台兼容性
            //                    if (GeneralSettings.IsStartupAppMinimized.Value)
            //                        ProgramHost.IsMinimize = true;
            //#pragma warning restore CA1416 // 验证平台兼容性
            //#endif
            //                    #endregion
            //                    MainWindow = new MainWindow();
            //                    break;
            //            }
            //            MainWindow.DataContext = vmService.MainWindow;
        });
    }

    public override void RegisterServices()
    {
        if (!Design.IsDesignMode)
        {
            AvaloniaLocator.CurrentMutable
                .Bind<IFontManagerImpl>().ToConstant(Ioc.Get<IFontManagerImpl>());
        }

        base.RegisterServices();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = MainWindow = new MainWindow();

        base.OnFrameworkInitializationCompleted();
    }

#if WINDOWS
    /// <inheritdoc cref="IPlatformService.SetDesktopBackgroundToWindow(nint, int, int)"/>
    public void SetDesktopBackgroundWindow()
    {
        //try
        //{
        //    if (MainWindow is MainWindow window)
        //    {
        //        IPlatformService.Instance.SetDesktopBackgroundToWindow(
        //            window.BackHandle,
        //            Convert.ToInt32(window.Width),
        //            Convert.ToInt32(window.Height));
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Log.Error(TAG, ex, "SetDesktopBackgroundToWindow fail.");
        //}
    }
#endif

    /// <summary>
    /// 设置当前打开窗口的 AvaloniaWindow 背景透明材质
    /// </summary>
    /// <param name="level"></param>
    public void SetAllWindowransparencyMateria(int level)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            foreach (var window in desktop.Windows)
            {
                window.TransparencyLevelHint = (WindowTransparencyLevel)level;

                if (window.TransparencyLevelHint == WindowTransparencyLevel.Transparent ||
                    window.TransparencyLevelHint == WindowTransparencyLevel.Blur)
                {
                    ((IPseudoClasses)window.Classes).Set(":transparent", true);
                }
                else
                {
                    ((IPseudoClasses)window.Classes).Set(":transparent", false);
                }
            }
        }
    }

    /// <summary>
    /// Exits the app by calling <c>Shutdown()</c> on the <c>IClassicDesktopStyleApplicationLifetime</c>.
    /// </summary>
    public bool Shutdown(int exitCode = 0)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainThread2.BeginInvokeOnMainThread(() =>
            {
                desktop.Shutdown(exitCode);
            });
            return true;
        }
        return false;
    }
}
