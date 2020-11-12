#include "Sprite.h"

Rendering::Sprite::Sprite(glm::vec2 const& position, glm::vec2 const& size)
	: Rendering::Interface::IGraphNode(position),
	m_size(size)
{
	std::string fs =
		"#version 330 core\n"
		"out vec4 FragColor; \n"

		"void main(void) {\n"
		"  FragColor = vec4(1, 0, 0, 1); \n"
		"} \n";

	const auto vs =
		"#version 330 core\n"
		"layout(location = 0) in vec2 l_pos;   \n"
		"uniform vec2 u_position; \n"
		"uniform vec2 u_winSize; \n"
		"uniform vec2 u_size; \n"

		"void main()   \n"
		"{   \n"
		"  vec2 resized = ((l_pos * u_size) + u_position) * 2 / u_winSize;  \n"
		"  gl_Position = vec4(resized - 1, 0, 1.0);   \n"
		"}   \n";

	const float vertices[] = {
		0, 0,
		1, 0,
		1, 1,
		0, 0,
		1, 1,
		0, 1,
	};

	this->m_mesh.init(vertices, sizeof(vertices), 6);
	this->m_shader.init(vs, fs);
}

void Rendering::Sprite::UpdateGraphics(float deltaTime, glm::vec2 const& winSize)
{

}

void Rendering::Sprite::Render(glm::vec2 const& winSize)
{
	this->m_shader.use();
	this->m_shader.setVec2("u_position", this->m_position);
	this->m_shader.setVec2("u_winSize", winSize);
	this->m_shader.setVec2("u_size", this->m_size);
	this->m_mesh.draw();
}
