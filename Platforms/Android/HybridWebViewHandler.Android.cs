/* Todo: untested */

namespace HybridWebView;

/* Note: MAUI's Android WebViewHandler is located in src/Core/src/Handlers/WebView/WebViewHandler.Android.cs */
public partial class HybridWebViewHandler : WebViewHandler
{
	protected override AWebView CreatePlatformView()
	{
		var enable_dev_tools = ((HybridWebView)VirtualView).EnableDevTools;

		/* MauiWebView for Android is located in src/Core/src/Platform/Android/MauiWebView.cs */
		var platform_view = base.CreatePlatformView();

		/* Note: this setting is app-wide. If enabled, it is enabled for all Android WebViews in the
		 * app. */
		AWebView.SetWebContentsDebuggingEnabled(enable_dev_tools);

		platform_view.AddJavascriptInterface(
			new HybridWebViewJavaScriptInterface(this),
			"hybridWebViewHost");

		return platform_view;
	}

	private sealed class HybridWebViewJavaScriptInterface : Java.Lang.Object
	{
		private readonly HybridWebView hybridWebView;

		public HybridWebViewJavaScriptInterface(HybridWebView hybridWebView)
		{
			this.hybridWebView = hybridWebView;;
		}

		[JavascriptInterface]
		[Export("sendMessage")]
		public void SendMessage(string message)
		{
			hybridWebView.OnMessageReceived(message);
		}
	}
}
