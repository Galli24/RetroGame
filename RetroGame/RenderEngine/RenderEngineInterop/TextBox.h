#pragma once
#include "glm/glm.hpp"
#include "IMenu.h"
#include "Font.h"
#include "../RenderEngine/TextBox.h"
#include "../RenderEngine/IMenu.h"
#include <string>


namespace RenderEngine {

	public ref class TextBox : IMenu
	{
	private:
		Rendering::TextBox* textboxResources;

	public:


		TextBox(Vector2 position, IMenu::Anchor^ anchor, Font^ font, Vector2 padding)
		{
			SetResources(new Rendering::TextBox(
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				font->nativeResources,
				{ padding.X, padding.Y }
			));

			textboxResources = dynamic_cast<Rendering::TextBox*>(this->menuResources);
		}


		TextBox(Vector2 position, IMenu::Anchor^ anchor, Font^ font, Vector2 padding, int maxCharDisplayed) {
			SetResources(new Rendering::TextBox(
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				font->nativeResources,
				{ padding.X, padding.Y },
				maxCharDisplayed
			));

			textboxResources = dynamic_cast<Rendering::TextBox*>(this->menuResources);
		}

		TextBox(Vector2 position, IMenu::Anchor^ anchor, Font^ font, Vector2 padding, int maxCharDisplayed, int minWidth)
		{
			SetResources(new Rendering::TextBox(
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				font->nativeResources,
				{ padding.X, padding.Y },
				maxCharDisplayed,
				minWidth
			));

			textboxResources = dynamic_cast<Rendering::TextBox*>(this->menuResources);
		}

		TextBox(Vector2 position, IMenu::Anchor^ anchor, Font^ font, Vector2 padding, int maxCharDisplayed, int minWidth, int maxCharsInBox)
		{
			SetResources(new Rendering::TextBox(
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				font->nativeResources,
				{ padding.X, padding.Y },
				maxCharDisplayed,
				minWidth,
				maxCharsInBox
			));

			textboxResources = dynamic_cast<Rendering::TextBox*>(this->menuResources);
		}


		property String^ Text {

			String^ get() {
				return gcnew String(textboxResources->text.c_str());
			}

			void set(String^ value) {
				std::string str = msclr::interop::marshal_as<std::string>(value);
				textboxResources->text = str;
			}
		}

		property Vector4 TextColor {

			Vector4 get() {
				return Vector4(textboxResources->font_color.x, textboxResources->font_color.y, textboxResources->font_color.z, textboxResources->font_color.w);
			}

			void set(Vector4 value)
			{
				textboxResources->font_color = { value.X, value.Y, value.Z, value.W };
			}

		}

		property Vector4 BorderColor {

			Vector4 get() {
				return Vector4(textboxResources->border_color.x, textboxResources->border_color.y, textboxResources->border_color.z, textboxResources->border_color.w);
			}

			void set(Vector4 value)
			{
				textboxResources->border_color = { value.X, value.Y, value.Z, value.W };
			}

		}

		property Vector2 Padding {

			Vector2 get() {
				return Vector2(textboxResources->padding.x, textboxResources->padding.y);
			}

			void set(Vector2 value)
			{
				textboxResources->padding = { value.X, value.Y };
			}

		}

		property int BorderSize {

			int get() {
				return textboxResources->border_size;
			}

			void set(int value)
			{
				textboxResources->border_size = value;
			}

		}

		property int MinWidth {

			int get() {
				return textboxResources->min_width;
			}

			void set(int value)
			{
				textboxResources->min_width = value;
			}

		}

		property int MaxCharDisplayed {

			int get() {
				return textboxResources->max_char_displayed;
			}

			void set(int value)
			{
				textboxResources->max_char_displayed = value;
			}

		}

	};

}
