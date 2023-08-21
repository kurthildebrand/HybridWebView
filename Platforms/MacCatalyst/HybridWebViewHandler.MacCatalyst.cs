using System.Drawing;
using Foundation;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using WebKit;

namespace HybridWebView;

/* Note: MAUI's iOS/MacCatalyst WebViewHandler is located in src/Core/src/Handlers/WebView/WebViewHandler.iOS.cs */
public partial class HybridWebViewHandler : WebViewHandler
{
	protected override WKWebView CreatePlatformView()
	{
		var enable_dev_tools = ((HybridWebView)VirtualView).EnableDevTools;

		/* MauiWKWebView is located in src/Core/src/Platform/iOS/MauiWKWebView.cs */
		var config = MauiWKWebView.CreateConfiguration();

		config.UserContentController.AddScriptMessageHandler(
			new HybridWebViewScriptMessageHandler(MessageReceived),
			"webwindowinterop");

		/* Legacy devtools */
		config.Preferences.SetValueForKey(NSObject.FromObject(enable_dev_tools),
			new NSString("developerExtrasEnabled"));

		var platform_view = new MauiWKWebView(RectangleF.Empty, this, config);

		/* Enable devtools for MacCatalyst / iOS builds for 16.4+ */
		if(OperatingSystem.IsMacCatalystVersionAtLeast(major: 13, minor: 3) ||
		   OperatingSystem.IsIOSVersionAtLeast(major: 16, minor: 4))
		{
			platform_view.SetValueForKey(NSObject.FromObject(enable_dev_tools),
				new NSString("inspectable"));
		}

		return platform_view;
	}

	private void MessageReceived(string message)
	{
		((HybridWebView)VirtualView).OnMessageReceived(message);
	}

	private class HybridWebViewScriptMessageHandler : NSObject, IWKScriptMessageHandler
	{
		private Action<string> message_received_action;

		public HybridWebViewScriptMessageHandler(Action<string> message_received_action)
		{
			this.message_received_action = message_received_action ??
				throw new ArgumentNullException(nameof(message_received_action));
		}

		public void DidReceiveScriptMessage(
			WKUserContentController user_content_controller,
			WKScriptMessage message)
		{
			if(message is null)
			{
				throw new ArgumentNullException(nameof(message));
			}

			message_received_action(((NSString)message.Body).ToString());
		}
	}
}
