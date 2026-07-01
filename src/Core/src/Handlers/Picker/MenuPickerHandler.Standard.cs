using System;

namespace Microsoft.Maui.Handlers
{
	public partial class MenuPickerHandler : ViewHandler<IPicker, object>
	{
		protected override object CreatePlatformView() => throw new NotImplementedException();

		internal static void MapItems(MenuPickerHandler handler, IPicker picker) { }

		public static void MapTitle(MenuPickerHandler handler, IPicker view) { }
		public static void MapTitleColor(MenuPickerHandler handler, IPicker view) { }
		public static void MapSelectedIndex(MenuPickerHandler handler, IPicker view) { }
		public static void MapCharacterSpacing(MenuPickerHandler handler, IPicker view) { }
		public static void MapFont(MenuPickerHandler handler, IPicker view) { }
		public static void MapTextColor(MenuPickerHandler handler, IPicker view) { }
		public static void MapHorizontalTextAlignment(MenuPickerHandler handler, IPicker view) { }
		public static void MapVerticalTextAlignment(MenuPickerHandler handler, IPicker view) { }
		internal static void MapIsOpen(MenuPickerHandler handler, IPicker picker) { }
	}
}
