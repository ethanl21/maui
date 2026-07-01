namespace Microsoft.Maui.Handlers
{
	public partial class MenuPickerHandler
	{
		public static PropertyMapper<IPicker, MenuPickerHandler> Mapper = new(ViewMapper)
		{
			[nameof(IPicker.CharacterSpacing)] = MapCharacterSpacing,
			[nameof(IPicker.Font)] = MapFont,
			[nameof(IPicker.SelectedIndex)] = MapSelectedIndex,
			[nameof(IPicker.TextColor)] = MapTextColor,
			[nameof(IPicker.Title)] = MapTitle,
			[nameof(IPicker.TitleColor)] = MapTitleColor,
			[nameof(ITextAlignment.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
			[nameof(ITextAlignment.VerticalTextAlignment)] = MapVerticalTextAlignment,
			[nameof(IPicker.Items)] = MapItems,
			[nameof(IPicker.IsOpen)] = MapIsOpen,
		};

		public static CommandMapper<IPicker, MenuPickerHandler> CommandMapper = new(ViewCommandMapper);

		public MenuPickerHandler() : base(Mapper, CommandMapper)
		{
		}

		public MenuPickerHandler(IPropertyMapper? mapper)
			: base(mapper ?? Mapper, CommandMapper)
		{
		}

		public MenuPickerHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
			: base(mapper ?? Mapper, commandMapper ?? CommandMapper)
		{
		}
	}
}
