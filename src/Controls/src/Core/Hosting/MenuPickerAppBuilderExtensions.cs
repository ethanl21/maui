#if IOS || MACCATALYST
using Microsoft.Maui.Handlers;
#endif

namespace Microsoft.Maui.Controls.Hosting
{
	public static class MenuPickerAppBuilderExtensions
	{
		public static MauiAppBuilder UseMenuPicker(this MauiAppBuilder builder)
		{
#if IOS || MACCATALYST
			builder.ConfigureMauiHandlers(handlers =>
			{
				handlers.AddHandler<Picker, MenuPickerHandler>();
			});
#endif
			return builder;
		}
	}
}
