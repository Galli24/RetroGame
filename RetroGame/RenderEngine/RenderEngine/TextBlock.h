#pragma once
#include "IMenu.h"
#include "glm/glm.hpp"
#include <string>
#include "Font.h"

namespace Rendering
{

	class TextBlock : public Interface::IMenu
	{

	public:
		TextBlock(glm::vec2 const& pos, IMenu::Anchor anchor, std::string const& str, Font* font, glm::vec2 const& padding);


		void UpdateGraphics(float deltaTime, glm::vec2 const& winSize) override;
		void Render(glm::vec2 const& winSize) override;


		void OnFocus() override;
		void OnLostFocus() override;

		void OnScroll(double const x, double const y) override;
		void OnMousePress(int const key, double const x, double const y) override;
		void OnMouseRelease(int const key, double const x, double const y) override;
		void OnMouseMove(double const x, double const y) override;
		void OnKeyPressed(int const key, int const mods) override;
		void OnKeyRelease(int const key, int const mods) override;
		void OnCharReceived(char const c) override;


		glm::vec2 GetObjectSize() const override;

		glm::vec4			font_color;
		glm::vec4			border_color;
		glm::vec2			padding;
		std::string			text;
		int					border_size;

	private:
		Rendering::Font* m_font;

		Rendering::Mesh		m_mesh;
		Rendering::Shader	m_shader;

	};


}

