using Microsoft.Maui.Handlers;

namespace HybridWebView;

public partial class HybridWebViewHandler : WebViewHandler
{
	public static IPropertyMapper<IWebView, IWebViewHandler> HybridWebViewMapper =
		new PropertyMapper<IWebView, IWebViewHandler>(Mapper)
		{
		};

	public HybridWebViewHandler() : base(HybridWebViewMapper, CommandMapper)
	{
	}

	public HybridWebViewHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
		: base(mapper ?? HybridWebViewMapper, commandMapper ?? CommandMapper)
	{
	}
}
