namespace HybridWebView;

public class HybridWebViewMessageReceivedEventArgs : EventArgs
{
	public string? Message { get; }

	public HybridWebViewMessageReceivedEventArgs(string? message)
	{
		Message = message;
	}
}
