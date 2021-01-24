#pragma once
#include "glm/glm.hpp"
#include "IMenu.h"
#include "Font.h"
#include "../RenderEngine/TextBox.h"
#include "../RenderEngine/IMenu.h"
#include "../RenderEngine/Font.h"
#include <string>


namespace RenderEngine {

	class TextBoxWrapper : public Rendering::TextBox
	{
	private:
		gcroot<RenderEngine::IMenu^> _interop;

	public:
		TextBoxWrapper(gcroot<RenderEngine::IMenu^> interop, glm::vec2 const& pos, IMenu::Anchor anchor, Rendering::Font* font, glm::vec2 const& padding, int maxCharDisplayed = 20, int minWidth = 500, unsigned int maxChar = -1)
			: Rendering::TextBox(pos, anchor, font, padding, maxCharDisplayed, minWidth, maxChar),
			_interop(interop)
		{}

		void OnScroll(double const x, double const y) override {
			Rendering::TextBox::OnScroll(x, y);
			_interop->RaiseScroll(x, y);
		}

		void OnMousePress(int const key, double const x, double const y) override {
			Rendering::TextBox::OnMousePress(key, x, y);
			_interop->RaiseMousePress(key, x, y);
		}

		void OnMouseRelease(int const key, double const x, double const y) override {
			Rendering::TextBox::OnMouseRelease(key, x, y);
			_interop->RaiseMouseRelease(key, x, y);
		}

		void OnMouseMove(double const x, double const y) override {
			Rendering::TextBox::OnMouseMove(x, y);
			_interop->RaiseMouseMove(x, y);
		}

		void OnKeyPressed(int const key, int const mods) override {
			Rendering::TextBox::OnKeyPressed(key, mods);
			_interop->RaiseKeyPressed(key, mods);
		}

		void OnKeyRelease(int const key, int const mods) override {
			Rendering::TextBox::OnKeyRelease(key, mods);
			_interop->RaiseKeyRelease(key, mods);
		}

		void OnCharReceived(char const c) override {
			Rendering::TextBox::OnCharReceived(c);
			_interop->RaiseCharReceived(c);
		}

	};

	public ref class TextBox : IMenu
	{
	private:
		TextBoxWrapper* textboxResources;

	public:


		TextBox(Vector2 position, IMenu::Anchor^ anchor, Font^ font, Vector2 padding)
		{
			SetResources(new TextBoxWrapper(this,
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				font->nativeResources,
				{ padding.X, padding.Y }
			));

			textboxResources = dynamic_cast<TextBoxWrapper*>(this->menuResources);
		}


		TextBox(Vector2 position, IMenu::Anchor^ anchor, Font^ font, Vector2 padding, int maxCharDisplayed) {
			SetResources(new TextBoxWrapper(this,
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				font->nativeResources,
				{ padding.X, padding.Y },
				maxCharDisplayed
			));

			textboxResources = dynamic_cast<TextBoxWrapper*>(this->menuResources);
		}

		TextBox(Vector2 position, IMenu::Anchor^ anchor, Font^ font, Vector2 padding, int maxCharDisplayed, int minWidth)
		{
			SetResources(new TextBoxWrapper(this,
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				font->nativeResources,
				{ padding.X, padding.Y },
				maxCharDisplayed,
				minWidth
			));

			textboxResources = dynamic_cast<TextBoxWrapper*>(this->menuResources);
		}

		TextBox(Vector2 position, IMenu::Anchor^ anchor, Font^ font, Vector2 padding, int maxCharDisplayed, int minWidth, int maxCharsInBox)
		{
			SetResources(new TextBoxWrapper(this,
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				font->nativeResources,
				{ padding.X, padding.Y },
				maxCharDisplayed,
				minWidth,
				maxCharsInBox
			));

			textboxResources = dynamic_cast<TextBoxWrapper*>(this->menuResources);
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
