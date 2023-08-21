using HybridWebView;

namespace HybridWebView;

public class HybridWebView : WebView
{
	public event EventHandler<HybridWebViewMessageReceivedEventArgs>?
		MessageReceived;

	public bool EnableDevTools { get; set; } = false;

	public virtual void OnMessageReceived(string message)
	{
		MessageReceived?.Invoke(this, new HybridWebViewMessageReceivedEventArgs(message));
	}
}
