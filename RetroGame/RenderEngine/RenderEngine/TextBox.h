#pragma once
#include "IMenu.h"
#include <glm/glm.hpp>
#include "Font.h"
#include <memory>
#include "Mesh.h"
#include "Shader.h"

namespace Rendering {


	class TextBox : public Interface::IMenu
	{
	public:

		TextBox(glm::vec2 const& pos, IMenu::Anchor anchor, Font* font, glm::vec2 const& padding, int minWidth = 500, unsigned int maxChar = -1);

		void UpdateGraphics(float deltaTime, glm::vec2 const& winSize) override;
		void Render(glm::vec2 const& winSize) override;

		glm::vec2 GetObjectSize() const override;
		void ReevaluateSize() override { }

		void OnFocus() override;
		void OnLostFocus() override;

		void OnScroll(double const x, double const y) override;
		void OnMousePress(int const key, double const x, double const y) override;
		void OnMouseRelease(int const key, double const x, double const y) override;
		void OnMouseMove(double const x, double const y) override;
		void OnKeyPressed(int const key, int const mods) override;
		void OnKeyRelease(int const key, int const mods) override;
		void OnCharReceived(char const c) override;


		glm::vec4			font_color;
		glm::vec4			border_color;
		glm::vec2			padding;
		std::string			text;
		int					border_size;
		int					min_width;

	private:
		unsigned int		m_maxChar;
		Rendering::Font*	m_font;

		Rendering::Mesh		m_mesh;
		Rendering::Shader	m_shader;

		glm::vec2			m_fontOffset;
		glm::vec2			m_fontMaxCharSize;

	};

}

