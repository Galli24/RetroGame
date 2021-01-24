#pragma once
#include "glm/glm.hpp"
#include "IMenu.h"
#include "../RenderEngine/IMenu.h"
#include "../RenderEngine/Button.h"
#include "../RenderEngine/Font.h"
#include "Font.h"
#include <string>
#include <vcclr.h> //required for gcroot

namespace RenderEngine {


	class ButtonWrapper : public Rendering::Button 
	{
	private:
		gcroot<RenderEngine::IMenu^> _interop;

	public:
		ButtonWrapper(gcroot<RenderEngine::IMenu^> interop, glm::vec2 const& pos, Rendering::Interface::IMenu::Anchor anchor, std::string const& str, Rendering::Font* font, glm::vec2 const& padding) 
			: Button(pos, anchor, str, font, padding),
			_interop(interop)
		{}

		void OnScroll(double const x, double const y) override {
			Rendering::Button::OnScroll(x, y);
			_interop->RaiseScroll(x, y);
		}

		void OnMousePress(int const key, double const x, double const y) override {
			Rendering::Button::OnMousePress(key, x, y);
			_interop->RaiseMousePress(key, x, y);
		}

		void OnMouseRelease(int const key, double const x, double const y) override {
			Rendering::Button::OnMouseRelease(key, x, y);
			_interop->RaiseMouseRelease(key, x, y);
		}

		void OnMouseMove(double const x, double const y) override {
			Rendering::Button::OnMouseMove(x, y);
			_interop->RaiseMouseMove(x, y);
		}

		void OnKeyPressed(int const key, int const mods) override {
			Rendering::Button::OnKeyPressed(key, mods);
			_interop->RaiseKeyPressed(key, mods);
		}

		void OnKeyRelease(int const key, int const mods) override {
			Rendering::Button::OnKeyRelease(key, mods);
			_interop->RaiseKeyRelease(key, mods);
		}

		void OnCharReceived(char const c) override {
			Rendering::Button::OnCharReceived(c);
			_interop->RaiseCharReceived(c);
		}

	};




	public ref class Button : public IMenu
	{
	private:
		Rendering::Button* buttonResources;

	public:

		Button(Vector2 position, String^ str, IMenu::Anchor^ anchor, Font^ font, Vector2 padding)
		{
			SetResources(new ButtonWrapper(this,
				{ position.X, position.Y },
				static_cast<Rendering::Interface::IMenu::Anchor>((int)*anchor),
				msclr::interop::marshal_as<std::string>(str),
				font->nativeResources,
				{ padding.X, padding.Y }
			));

			buttonResources = dynamic_cast<ButtonWrapper*>(this->menuResources);
		}



		property String^ Text {

			String^ get() {
				return gcnew String(buttonResources->text.c_str());
			}

			void set(String^ value) {
				std::string str = msclr::interop::marshal_as<std::string>(value);
				buttonResources->text = str;
			}
		}

		property Vector4 TextColor {

			Vector4 get() {
				return Vector4(buttonResources->font_color.x, buttonResources->font_color.y, buttonResources->font_color.z, buttonResources->font_color.w);
			}

			void set(Vector4 value)
			{
				buttonResources->font_color = { value.X, value.Y, value.Z, value.W };
			}

		}

		property Vector4 BorderColor {

			Vector4 get() {
				return Vector4(buttonResources->border_color.x, buttonResources->border_color.y, buttonResources->border_color.z, buttonResources->border_color.w);
			}

			void set(Vector4 value)
			{
				buttonResources->border_color = { value.X, value.Y, value.Z, value.W };
			}

		}

		property Vector2 Padding {

			Vector2 get() {
				return Vector2(buttonResources->padding.x, buttonResources->padding.y);
			}

			void set(Vector2 value)
			{
				buttonResources->padding = { value.X, value.Y };
			}

		}

		property int BorderSize {

			int get() {
				return buttonResources->border_size;
			}

			void set(int value)
			{
				buttonResources->border_size = value;
			}

		}


	};

}
