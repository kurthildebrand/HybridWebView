/* Todo: untested */

namespace HybridWebView;

/* Note: MAUI's Windows WebViewHandler is located in src/Core/src/Handlers/WebView/WebViewHandler.Windows.cs */
public partial class HybridWebViewHandler : WebViewHandler
{
	protected override async WebView2 CreatePlatformView()
	{
		var enable_dev_tools = ((HybridWebView)VirtualView).EnableDevTools;

		/* MauiWebView for Windows is located in src/Core/src/Platform/Windows/MauiWebView.cs */
		var platform_view = base.CreatePlatformView();

		platform_view.WebMessageReceived += WebMessageReceived;
		platform_view.CoreWebView2.Settings.IsWebMessageEnabled = true;
		platform_view.CoreWebView2.Settings.AreDevToolsEnabled = enable_dev_tools;
	}

	private void WebMessageReceived(
		Microsoft.UI.Xaml.Controls.WebView2 sender,
		CoreWebView2WebMessageReceivedEventArgs args)
	{
		((HybridWebView)VirtualView).OnMessageReceived(message);
	}
}
