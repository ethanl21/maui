using System;
using UIKit;

namespace Microsoft.Maui.Handlers
{
	public partial class PickerHandler : ViewHandler<IPicker, UIButton>
	{
		internal bool UpdateImmediately { get; set; }
		IFontManager? _fontManager;

		protected override UIButton CreatePlatformView()
		{
			var button = new UIButton(UIButtonType.System);

			button.ShowsMenuAsPrimaryAction = true;
			button.ChangesSelectionAsPrimaryAction = true;

			button.BackgroundColor = UIColor.SystemBackground;
			button.Layer.BorderWidth = 0.5f;
			button.Layer.BorderColor = UIColor.Separator.CGColor;
			button.Layer.CornerRadius = 5f;
			button.ContentEdgeInsets = new UIEdgeInsets(8, 12, 8, 12);

			button.AccessibilityTraits = UIAccessibilityTrait.Button;

			return button;
		}

		void UpdateMenu()
		{
			if (PlatformView == null || VirtualView == null)
				return;

			var count = VirtualView.GetCount();

			if (count == 0)
			{
				PlatformView.Menu = null;
				return;
			}

			var weakHandler = new WeakReference<PickerHandler>(this);
			var menuElements = new UIMenuElement[count];

			for (int i = 0; i < count; i++)
			{
				var index = i;
				var title = VirtualView.GetItem(index);
				var action = UIAction.Create(title, null, null, _ =>
				{
					if (weakHandler.TryGetTarget(out var h))
						h.OnMenuItemSelected(index);
				});
				action.State = (i == VirtualView.SelectedIndex) ? UIMenuElementState.On : UIMenuElementState.Off;
				menuElements[i] = action;
			}

			PlatformView.Menu = UIMenu.Create(string.Empty, menuElements);
		}

		void OnMenuItemSelected(int index)
		{
			if (VirtualView == null)
				return;

			VirtualView.SelectedIndex = index;

			if (VirtualView is IPicker picker)
			{
				picker.IsFocused = false;
			}
		}

		void UpdateSelectedText()
		{
			if (PlatformView == null || VirtualView == null)
				return;

			var selectedIndex = VirtualView.SelectedIndex;
			bool isTitle = selectedIndex < 0 || selectedIndex >= VirtualView.GetCount();
			var text = isTitle ? (VirtualView.Title ?? string.Empty) : VirtualView.GetItem(selectedIndex);

			PlatformView.SetTitle(text, UIControlState.Normal);
			ApplyTextStyle(isTitle);
		}

		void ApplyTextStyle(bool isTitle)
		{
			if (PlatformView == null || VirtualView == null)
				return;

			var font = VirtualView.Font;
			var fontManager = _fontManager;
			if (font.Size > 0 && fontManager != null)
			{
				PlatformView.TitleLabel.Font = fontManager.GetFont(font);
			}

			var textColor = isTitle ? VirtualView.TitleColor : VirtualView.TextColor;
			if (textColor != null)
			{
				PlatformView.SetTitleColor(textColor.ToPlatform(), UIControlState.Normal);
			}
		}

		protected override void ConnectHandler(UIButton platformView)
		{
			_fontManager = ((IElementHandler)this).GetRequiredService<IFontManager>();
			UpdateMenu();
			UpdateSelectedText();

			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(UIButton platformView)
		{
			platformView.Menu = null;
			platformView.SetTitle(null, UIControlState.Normal);
			_fontManager = null;
			base.DisconnectHandler(platformView);
		}

		static void Reload(IPickerHandler handler)
		{
			if (handler is PickerHandler pickerHandler)
			{
				pickerHandler.UpdateMenu();
				pickerHandler.UpdateSelectedText();
			}
		}

		[Obsolete("Use Microsoft.Maui.Handlers.PickerHandler.MapItems instead")]
		public static void MapReload(IPickerHandler handler, IPicker picker, object? args) => Reload(handler);

		internal static void MapItems(IPickerHandler handler, IPicker picker) => Reload(handler);

		public static void MapTitle(IPickerHandler handler, IPicker picker)
		{
			if (handler is PickerHandler pickerHandler)
				pickerHandler.UpdateSelectedText();
		}

		public static void MapTitleColor(IPickerHandler handler, IPicker picker)
		{
			if (handler is PickerHandler pickerHandler)
				pickerHandler.UpdateSelectedText();
		}

		public static void MapSelectedIndex(IPickerHandler handler, IPicker picker)
		{
			if (handler is PickerHandler pickerHandler)
				pickerHandler.UpdateSelectedText();
		}

		public static void MapCharacterSpacing(IPickerHandler handler, IPicker picker)
		{
			if (handler is PickerHandler pickerHandler)
				pickerHandler.UpdateSelectedText();
		}

		public static void MapFont(IPickerHandler handler, IPicker picker)
		{
			if (handler is PickerHandler pickerHandler)
				pickerHandler.UpdateSelectedText();
		}

		public static void MapHorizontalTextAlignment(IPickerHandler handler, IPicker picker)
		{
		}

		public static void MapTextColor(IPickerHandler handler, IPicker picker)
		{
			if (handler is PickerHandler pickerHandler)
				pickerHandler.UpdateSelectedText();
		}

		public static void MapVerticalTextAlignment(IPickerHandler handler, IPicker picker)
		{
		}

		internal static void MapIsOpen(IPickerHandler handler, IPicker picker)
		{
		}
	}

