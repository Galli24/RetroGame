#include "Button.h"
#include <stdexcept>


Rendering::Button::Button(glm::vec2 const& pos, IMenu::Anchor anchor, std::string const& str, Font* font, glm::vec2 const& padding)
	: IMenu(pos, anchor),
	m_font(font), text(str), border_color(1, 1, 1, 1), font_color(1, 1, 1, 1), padding(padding)
{
	std::string fs =
		"#version 330 core\n"
		"in vec2 o_pos;\n"
		"in vec2 o_size;\n"

		"uniform vec4 u_color; \n"

		"out vec4 FragColor; \n"

		"void main(void) {\n"
		"  vec2 pos_in_object = o_pos * o_size;\n"
		"  int border_size = 2;"
		"  if (pos_in_object.x > border_size && pos_in_object.x < o_size.x - border_size &&				\n"
		"      pos_in_object.y > border_size && pos_in_object.y < o_size.y - border_size)				\n"
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


void Rendering::Button::UpdateGraphics(float deltaTime, glm::vec2 const& winSize)
{
}

static glm::vec2 modify_position(glm::vec2 const& position, glm::vec2 size, int vertical, int horizontal)
{
	glm::vec2 pos = position;

	if (vertical == -1)
		pos.y -= size.y;
	else if (vertical == 0)
		pos.y -= size.y / 2;


	if (horizontal == 1)
		pos.x -= size.x;
	else if (horizontal == 0)
		pos.x -= size.x / 2;

	return pos;
}

static glm::vec2 anchored_position(glm::vec2 const& position, glm::vec2 const& size, Rendering::Interface::IMenu::Anchor anchor)
{
	switch (anchor) {
	case Rendering::Interface::IMenu::Anchor::TopLeft:
		return modify_position(position, size, -1, -1);

	case Rendering::Interface::IMenu::Anchor::Top:
		return modify_position(position, size, -1, 0);

	case Rendering::Interface::IMenu::Anchor::TopRight:
		return modify_position(position, size, -1, 1);

	case Rendering::Interface::IMenu::Anchor::Left:
		return modify_position(position, size, 0, -1);

	case Rendering::Interface::IMenu::Anchor::Center:
		return modify_position(position, size, 0, 0);

	case Rendering::Interface::IMenu::Anchor::Right:
		return modify_position(position, size, 0, 1);

	case Rendering::Interface::IMenu::Anchor::BottomLeft:
		return modify_position(position, size, 1, -1);

	case Rendering::Interface::IMenu::Anchor::Bot:
		return modify_position(position, size, 1, 0);

	case Rendering::Interface::IMenu::Anchor::BottomRight:
		return modify_position(position, size, 1, 1);

	default:
		break;
	}

	return position;
}

void Rendering::Button::Render(glm::vec2 const& winSize)
{
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	auto fontOffset = glm::vec2{ 0, m_font->EvaluateYOffset(text) };
	m_shader.use();
	m_shader.setVec2("u_position", GetActualPosition() - fontOffset - (padding / glm::vec2{2, 2}));
	m_shader.setVec2("u_winSize", winSize);
	m_shader.setVec2("u_size", GetObjectSize() + fontOffset + padding);
	m_shader.setVec4("u_color", border_color);
	m_mesh.draw();
	glDisable(GL_BLEND);

	m_font->RenderText(text, GetActualPosition(), winSize, font_color);
}

glm::vec2 Rendering::Button::GetObjectSize() const
{
	return m_font->EvaluateSize(text);
}

void Rendering::Button::OnFocus()
{

}

void Rendering::Button::OnLostFocus()
{

}

void Rendering::Button::OnScroll(double const x, double const y)
{

}

void Rendering::Button::OnMousePress(double const x, double const y)
{

}

void Rendering::Button::OnMouseRelease(double const x, double const y)
{

}

void Rendering::Button::OnMouseMove(double const x, double const y)
{

}

void Rendering::Button::OnKeyPressed(int const key) { }

void Rendering::Button::OnKeyRelease(int const key) { }

