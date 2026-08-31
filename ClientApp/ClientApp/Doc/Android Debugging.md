## Debugging Android with Visual Studio

We have a setup to that you can use, "<chrome/edge>://inspect/#devices". Replace <chrome/edge> with your browser of choice. 
This will allow you to debug your Android application using the browser's developer tools. It can take upwards of 30 seconds for the device to show up in the list of devices.

# How did you do that?

In the platform folder of the project, there is a MainApplication.cs file. In that file, there is a method called OnCreate. In that method, there is a line of code that looks like this:

```csharp
#if DEBUG
            Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#endif
```

This allows us to enable debugging for the WebView component of the Android application when the application is built in Debug mode.