#include "Sprite.h"

Rendering::Sprite::Sprite(std::string const& path, glm::vec2 const& position, glm::vec2 const& size, glm::vec2 const& scale)
	: Rendering::Interface::IGraphNode(position),
	m_size(size),
	m_scale(scale),
	m_texture(path, 0)

{
	std::string fs =
		"#version 330 core\n"
		"in vec2 o_texCoords;\n"

		"uniform sampler2D u_texture;\n"

		"out vec4 FragColor; \n"

		"void main(void) {\n"
		"  FragColor = texture(u_texture, vec2(o_texCoords.x, o_texCoords.y * -1)); \n"
		"} \n";

	const auto vs =
		"#version 330 core\n"
		"layout(location = 0) in vec2 l_pos;   \n"

		"uniform vec2 u_position; \n"
		"uniform vec2 u_winSize; \n"
		"uniform vec2 u_size; \n"
		"uniform vec2 u_scale; \n"

		"out vec2 o_texCoords;\n"

		"void main()   \n"
		"{   \n"
		"  vec2 resized = ((l_pos * u_size * u_scale) + u_position) * 2 / u_winSize;  \n"
		"  o_texCoords = l_pos;	\n"
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
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	this->m_shader.use();
	this->m_shader.setVec2("u_position", this->position);
	this->m_shader.setVec2("u_winSize", winSize);
	this->m_shader.setVec2("u_size", this->m_size);
	this->m_shader.setVec2("u_scale", m_scale);
	this->m_shader.setInt("u_texture", this->m_texture.GetTextureUnit());

	this->m_texture.Use();

	this->m_mesh.draw();

	glDisable(GL_BLEND);
}

void Rendering::Sprite::UpdatePosition(float deltaTime, glm::vec2 const& winSize)
{
	static glm::vec2 dir = glm::vec2{ 300, 300 };

	if (this->position.x + this->GetActualSize().x > winSize.x || this->position.x < 0)
		dir.x *= -1;

	if (this->position.y + this->GetActualSize().y > winSize.y || this->position.y < 0)
		dir.y *= -1;

	this->position += dir * deltaTime;
}

glm::vec2 Rendering::Sprite::GetActualSize() const
{
	return this->m_size * this->m_scale;
}
