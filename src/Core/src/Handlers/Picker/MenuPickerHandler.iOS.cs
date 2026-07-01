using System;
using UIKit;

namespace Microsoft.Maui.Handlers
{
	public partial class MenuPickerHandler : ViewHandler<IPicker, UIButton>
	{
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

			var weakHandler = new WeakReference<MenuPickerHandler>(this);
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
				picker.IsOpen = false;
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

			if (_fontManager != null)
			{
				PlatformView.TitleLabel.UpdateFont(VirtualView, _fontManager, UIFont.LabelFontSize);
			}

			var textColor = isTitle ? VirtualView.TitleColor : VirtualView.TextColor;
			if (textColor != null)
			{
				PlatformView.SetTitleColor(textColor.ToPlatform(), UIControlState.Normal);
			}

			var characterSpacing = VirtualView.CharacterSpacing;
			if (characterSpacing != 0)
			{
				var currentText = PlatformView.TitleLabel.AttributedText;
				var newText = currentText?.WithCharacterSpacing(characterSpacing);
				if (textColor != null)
				{
					newText = newText?.WithTextColor(textColor);
				}
				if (newText != null)
				{
					PlatformView.SetAttributedTitle(newText, UIControlState.Normal);
				}
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

		static void Reload(MenuPickerHandler handler)
		{
			handler.UpdateMenu();
			handler.UpdateSelectedText();
		}

		internal static void MapItems(MenuPickerHandler handler, IPicker picker) => Reload(handler);

		public static void MapTitle(MenuPickerHandler handler, IPicker picker)
		{
			handler.UpdateSelectedText();
		}

		public static void MapTitleColor(MenuPickerHandler handler, IPicker picker)
		{
			handler.UpdateSelectedText();
		}

		public static void MapSelectedIndex(MenuPickerHandler handler, IPicker picker)
		{
			handler.UpdateSelectedText();
		}

		public static void MapCharacterSpacing(MenuPickerHandler handler, IPicker picker)
		{
			handler.UpdateSelectedText();
		}

		public static void MapFont(MenuPickerHandler handler, IPicker picker)
		{
			handler.UpdateSelectedText();
		}

		public static void MapHorizontalTextAlignment(MenuPickerHandler handler, IPicker picker)
		{
			if (handler.PlatformView != null)
				handler.PlatformView.TitleLabel.TextAlignment = picker.HorizontalTextAlignment.ToPlatformHorizontal(handler.PlatformView.EffectiveUserInterfaceLayoutDirection);
		}

		public static void MapTextColor(MenuPickerHandler handler, IPicker picker)
		{
			handler.UpdateSelectedText();
		}

		public static void MapVerticalTextAlignment(MenuPickerHandler handler, IPicker picker)
		{
			if (handler.PlatformView != null)
				handler.PlatformView.ContentVerticalAlignment = picker.VerticalTextAlignment.ToPlatformVertical();
		}

		internal static void MapIsOpen(MenuPickerHandler handler, IPicker picker)
		{
		}
	}
}
