HybridWebView
=============
This project augments MAUI's WebView with two features:

1. A C# message received handler so that C# can receive data sent from JS.
2. A Dev Tools enable/disable flag so that the WebView can be debugged.

The main consideration of this project was to use as much of MAUI's existing code as possible.

A mechanism for receiving messages on the C# side has been requested in the main MAUI repo in the following issues:

- https://github.com/dotnet/maui/issues/1148
- https://github.com/dotnet/maui/issues/4813
- https://github.com/dotnet/maui/issues/6446
- https://github.com/dotnet/maui/discussions/12009

Special thanks to @Eilon's previous work implementing the message received handler and the dev tools enable flag for the different platforms.

- https://github.com/Eilon/MauiHybridWebView

Other implementations are here:

- https://github.com/mahop-net/Maui.HybridWebView
- https://github.com/nmoschkin/MAUIWebViewExample
- https://stackoverflow.com/questions/73217992/js-net-interact-on-maui-webview/75182298#75182298



How To Use
----------
1. Add the following line to MauiProgram.cs

	```C#
	builder.Services.AddHybridWebView();
	```

2. Change MainPage.xaml to be the following.

	```xml
	<?xml version="1.0" encoding="utf-8" ?>
	<ContentPage
		xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
		xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
		xmlns:controls="clr-namespace:HybridWebView;assembly=HybridWebView"
		x:Class="HybridWebViewTestApp.MainPage">

		<controls:HybridWebView x:Name="MainWebView" Source="index.html"/>

	</ContentPage>
	```

3. In MainPage.xaml.cs, add the following code-behind.

	```C#
	namespace HybridWebViewTestApp;

	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			MainWebView.EnableDevTools = true;
			MainWebView.MessageReceived += OnMessageReceived;
		}

		private async void OnMessageReceived(
			object sender,
			HybridWebView.HybridWebViewMessageReceivedEventArgs e)
		{
			await Dispatcher.DispatchAsync(async () =>
			{
				await DisplayAlert("JavaScript message", e.Message, "OK");
			});
		}
	}
	```

4. Add the following javascript function to an HTML page running in the webview to send a message to C#:

	```js
	function SendMessage(content) {
		if(window.chrome && window.chrome.webview) {
			/* Windows WebView2 */
			window.chrome.webview.postMessage(content);
		} else if (window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.webwindowinterop) {
			/* iOS and MacCatalyst WKWebView */
			window.webkit.messageHandlers.webwindowinterop.postMessage(content);
		} else {
			/* Android */
			hybridWebViewHost.sendMessage(content);
		}
	}
	```

	Note: an example HTML file might look like this:

	```html
	<!DOCTYPE html>
	<html lang="en" dir="ltr">
	<head>
		<meta charset="UTF-8"/>
		<title>HybridWebView Test App</title>
		<meta name="viewport" content="viewport-fit=cover, width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no"/>
		<script>
			function SendMessage(message) {
				if(window.chrome && window.chrome.webview) {
					/* Windows WebView2 */
					window.chrome.webview.postMessage(message);
				} else if (window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.webwindowinterop) {
					/* iOS and MacCatalyst WKWebView */
					window.webkit.messageHandlers.webwindowinterop.postMessage(message);
				} else {
					/* Android */
					hybridWebViewHost.sendMessage(message);
				}
			}
		</script>
		<script>
			function sendMessageToCSharp() {
				var message = document.getElementById('messageInput').value;
				SendMessage(message);
			}
		</script>
	</head>
	<body>
		<h1>Hello World</h1>
		<div>
			Your message: <input type="text" id="messageInput"/>
		</div>
		<div>
			<button onclick="sendMessageToCSharp()">Send to C#!</button>
		</div>
	</body>
	</html>
	```



Todo
----
1. Test on Android.
2. Test on Windows.
3. Change EnableDevTools flag to be project wide instead of per instance.
4. Implement Tizen.
