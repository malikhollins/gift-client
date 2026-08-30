using Android.App;
using Android.Runtime;
using Android.Webkit;

namespace ClientApp
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override void OnCreate()
        {
            base.OnCreate();

#if DEBUG
            Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#endif
        }
    }
}
