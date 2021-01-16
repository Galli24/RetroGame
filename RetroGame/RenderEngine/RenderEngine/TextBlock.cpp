#include "TextBlock.h"
#include <stdexcept>


Rendering::TextBlock::TextBlock(glm::vec2 const& pos, IMenu::Anchor anchor, std::string const& str, Font* font, glm::vec2 const& padding)
	: IMenu(pos, anchor),
	m_font(font), text(str), border_color(0.5, 0.5, 0.5, 0), font_color(1, 1, 1, 1), padding(padding), border_size(2)
{
	std::string fs =
		"#version 330 core\n"
		"in vec2 o_pos;\n"
		"in vec2 o_size;\n"

		"uniform vec4 u_color; \n"
		"uniform int u_borderSize;"
		"out vec4 FragColor; \n"

		"void main(void) {\n"
		"  vec2 pos_in_object = o_pos * o_size;\n"
		"  if (pos_in_object.x > u_borderSize && pos_in_object.x < o_size.x - u_borderSize &&				\n"
		"      pos_in_object.y > u_borderSize && pos_in_object.y < o_size.y - u_borderSize)				\n"
		"    discard; \n"
		"  FragColor = u_color; \n"
		"} \n";

	const auto vs =
		"#version 330 core\n"
		"layout(location = 0) in vec2 l_pos;   \n"

		"uniform vec2 u_position; \n"
		"uniform vec2 u_winSize; \n"
		"uniform vec2 u_size; \n"

		"out vec2 o_pos;\n"
		"out vec2 o_size;\n"

		"void main()   \n"
		"{   \n"
		"  vec2 resized = ((l_pos * u_size) + u_position) * 2 / u_winSize;  \n"
		"  o_pos = l_pos;	\n"
		"  o_size = u_size;	\n"
		"  gl_Position = vec4(resized - 1, 0, 1.0);   \n"
		"}   \n";

	const float vertices[] = {
		//  X, Y,
			0, 0,
			1, 0,
			1, 1,
			0, 0,
			1, 1,
			0, 1,
	};

	m_mesh.init(vertices, sizeof(vertices), 6);
	m_mesh.attribPtr(0, 2, 0, 2 * sizeof(float));
	m_shader.init(vs, fs);

}


void Rendering::TextBlock::UpdateGraphics(float deltaTime, glm::vec2 const& winSize)
{
}

void Rendering::TextBlock::Render(glm::vec2 const& winSize)
{
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	auto fontOffset = glm::vec2{ 0, m_font->EvaluateYOffset(text) };
	m_shader.use();
	m_shader.setVec2("u_position", GetActualPosition() - fontOffset);
	m_shader.setVec2("u_winSize", winSize);
	m_shader.setVec2("u_size", GetObjectSize() + fontOffset);
	m_shader.setVec4("u_color", border_color);
	m_shader.setInt("u_borderSize", border_size);
	m_mesh.draw();
	glDisable(GL_BLEND);

	m_font->RenderText(text, GetActualPosition() + padding, winSize, font_color);
}

glm::vec2 Rendering::TextBlock::GetObjectSize() const
{
	return m_font->EvaluateSize(text) + padding * 2.0f;
}


void Rendering::TextBlock::OnFocus() { }

void Rendering::TextBlock::OnLostFocus() { }

void Rendering::TextBlock::OnScroll(double const x, double const y) { }

void Rendering::TextBlock::OnMousePress(int const key, double const x, double const y)
{
}

void Rendering::TextBlock::OnMouseRelease(int const key, double const x, double const y)
{
}

void Rendering::TextBlock::OnMouseMove(double const x, double const y)
{

}

void Rendering::TextBlock::OnKeyPressed(int const key, int const mods) { }

void Rendering::TextBlock::OnKeyRelease(int const key, int const mods) { }

void Rendering::TextBlock::OnCharReceived(char const c) { }

