#include "TextBox.h"

Rendering::TextBox::TextBox(glm::vec2 const& pos, IMenu::Anchor anchor, Font* font, glm::vec2 const& padding, int minWidth, unsigned int maxChar)
	: IMenu(pos, anchor), m_maxChar(maxChar), m_font(font), font_color(1, 1, 1, 1), border_color(0.5, 0.5, 0.5, 1), padding(padding), border_size(2), min_width(minWidth)
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

	m_fontOffset = glm::vec2{ 0, m_font->EvaluateYOffset("[") };
	m_fontMaxCharSize = m_font->GetCharMaxSize();
}

void Rendering::TextBox::UpdateGraphics(float deltaTime, glm::vec2 const& winSize)
{
}

void Rendering::TextBox::Render(glm::vec2 const& winSize)
{
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	auto maxChars = m_font->GetFitNumChars(text, min_width - padding.x * 2.f);
	auto digits = max((int)text.size() - maxChars.x, 0);
	auto value = text.substr(digits);

	m_shader.use();
	m_shader.setVec2("u_position", GetActualPosition());
	m_shader.setVec2("u_winSize", winSize);
	m_shader.setVec2("u_size", GetObjectSize() - m_fontOffset);
	m_shader.setVec4("u_color", border_color);
	m_shader.setInt("u_borderSize", border_size);
	m_mesh.draw();
	glDisable(GL_BLEND);

	m_font->RenderText(value, GetActualPosition() + padding + m_fontOffset, winSize, font_color);

}

glm::vec2 Rendering::TextBox::GetObjectSize() const
{
	return { min_width, m_fontMaxCharSize.y };
}

void Rendering::TextBox::OnFocus()
{
	border_color = { 1, 0, 0, 1 };
}

void Rendering::TextBox::OnLostFocus()
{
	border_color = { .5f, .5f, .5f, 1 };
}

void Rendering::TextBox::OnKeyPressed(int const key, int const mods)
{
	if (key == GLFW_KEY_BACKSPACE && !text.empty())
		text.pop_back();
}

void Rendering::TextBox::OnCharReceived(char const c)
{
	if (text.size() >= m_maxChar)
		return;
	text.push_back(c);
}

void Rendering::TextBox::OnScroll(double const x, double const y) { }

void Rendering::TextBox::OnMousePress(int const key, double const x, double const y) { }

void Rendering::TextBox::OnMouseRelease(int const key, double const x, double const y) { }

void Rendering::TextBox::OnMouseMove(double const x, double const y) { }

void Rendering::TextBox::OnKeyRelease(int const key, int const mods) { }
