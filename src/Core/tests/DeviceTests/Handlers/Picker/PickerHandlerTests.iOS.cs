using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using ObjCRuntime;
using UIKit;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class PickerHandlerTests
	{
		[Fact(DisplayName = "Title Color Initializes Correctly")]
		public async Task TitleColorInitializesCorrectly()
		{
			var xplatTitleColor = Colors.CadetBlue;

			var picker = new PickerStub
			{
				Title = "Select an Item",
				TitleColor = xplatTitleColor
			};

			var expectedValue = xplatTitleColor.ToPlatform();

			var values = await GetValueAsync(picker, (handler) =>
			{
				return new
				{
					ViewValue = picker.TitleColor,
					PlatformViewValue = GetNativeTitleColor(handler)
				};
			});

			Assert.Equal(xplatTitleColor, values.ViewValue);
			Assert.Equal(expectedValue, values.PlatformViewValue);
		}

		[Fact(DisplayName = "Text Color Initializes Correctly")]
		public async Task TextColorInitializesCorrectly()
		{
			var xplatTitleColor = Colors.CadetBlue;

			var picker = new PickerStub
			{
				Title = "Select an Item",
				TextColor = xplatTitleColor,
				Items = new[] { "Item 1", "Item2", "Item3" },
				SelectedIndex = 1
			};

			var expectedValue = xplatTitleColor.ToPlatform();

			var values = await GetValueAsync(picker, (handler) =>
			{
				return new
				{
					ViewValue = picker.TextColor,
					PlatformViewValue = GetNativeTextColor(handler)
				};
			});

			Assert.Equal(xplatTitleColor, values.ViewValue);
			Assert.Equal(expectedValue, values.PlatformViewValue);
		}

		UIButton GetNativePicker(PickerHandler pickerHandler) =>
			pickerHandler.PlatformView;

		string GetNativeTitle(PickerHandler pickerHandler) =>
			GetNativePicker(pickerHandler).Title(UIControlState.Normal) ?? string.Empty;

		string GetNativeText(PickerHandler pickerHandler) =>
			 GetNativePicker(pickerHandler).Title(UIControlState.Normal) ?? string.Empty;

		async Task ValidateNativeItemsSource(IPicker picker, int itemsCount)
		{
			var expected = await GetValueAsync(picker, handler =>
			{
				var button = GetNativePicker(handler);
				return (int)(button.Menu?.Children?.Length ?? 0);
			});
			Assert.Equal(expected, itemsCount);
		}

		async Task ValidateNativeSelectedIndex(IPicker slider, int selectedIndex)
		{
			var expected = await GetValueAsync(slider, handler =>
			{
				var button = GetNativePicker(handler);
				var menu = button.Menu;
				if (menu != null)
				{
					for (nuint i = 0; i < menu.Children.Length; i++)
					{
						if (menu.Children[i] is UIAction action && action.State == UIMenuElementState.On)
							return (int)i;
					}
				}
				return -1;
			});
			Assert.Equal(expected, selectedIndex);
		}

		UITextAlignment GetNativeHorizontalTextAlignment(PickerHandler pickerHandler) =>
			GetNativePicker(pickerHandler).TitleLabel.TextAlignment;

		UIColor GetNativeTitleColor(PickerHandler pickerHandler)
		{
			var button = GetNativePicker(pickerHandler);
			return button.TitleColor(UIControlState.Normal);
		}

		UIColor GetNativeTextColor(PickerHandler pickerHandler)
		{
			var button = GetNativePicker(pickerHandler);
			return button.TitleColor(UIControlState.Normal);
		}

		UIControlContentVerticalAlignment GetNativeVerticalTextAlignment(PickerHandler pickerHandler) =>
			GetNativePicker(pickerHandler).ContentVerticalAlignment;
	}
}
