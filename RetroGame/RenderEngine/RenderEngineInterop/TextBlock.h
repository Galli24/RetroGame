#pragma once
#include "glm/glm.hpp"
#include "IMenu.h"
#include "Font.h"
#include "../RenderEngine/TextBlock.h"
#include "../RenderEngine/IMenu.h"
#include <string>


namespace RenderEngine {

	public ref class TextBlock : IMenu
	{
	private:
		Rendering::TextBlock* textblockResources;

	public:

		TextBlock(Vector2 position, String^ str, IMenu::Anchor^ anchor, Font^ font, Vector2 padding)
		{
			SetResources(new Rendering::TextBlock(
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				msclr::interop::marshal_as<std::string>(str),
				font->nativeResources,
				{ padding.X, padding.Y }
			));

			textblockResources = dynamic_cast<Rendering::TextBlock*>(this->menuResources);
		}



		property String^ Text {

			String^ get() {
				return gcnew String(textblockResources->text.c_str());
			}

			void set(String^ value) {
				std::string str = msclr::interop::marshal_as<std::string>(value);
				textblockResources->text = str;
			}
		}

		property Vector4 TextColor {

			Vector4 get() {
				return Vector4(textblockResources->font_color.x, textblockResources->font_color.y, textblockResources->font_color.z, textblockResources->font_color.w);
			}

			void set(Vector4 value)
			{
				textblockResources->font_color = { value.X, value.Y, value.Z, value.W };
			}

		}

		property Vector4 BorderColor {

			Vector4 get() {
				return Vector4(textblockResources->border_color.x, textblockResources->border_color.y, textblockResources->border_color.z, textblockResources->border_color.w);
			}

			void set(Vector4 value)
			{
				textblockResources->border_color = { value.X, value.Y, value.Z, value.W };
			}

		}

		property Vector2 Padding {

			Vector2 get() {
				return Vector2(textblockResources->padding.x, textblockResources->padding.y);
			}

			void set(Vector2 value)
			{
				textblockResources->padding = { value.X, value.Y };
			}

		}

		property int BorderSize {

			int get() {
				return textblockResources->border_size;
			}

			void set(int value)
			{
				textblockResources->border_size = value;
			}

		}


	};

}
