#include "AnimatedSprite.h"
#include <algorithm>

Rendering::AnimatedSprite::AnimatedSprite(std::vector<std::string> const& sprites, float const frameDuration, glm::vec2 const& position, glm::vec2 const& size, glm::vec2 const& scale)
	: 
	Rendering::Interface::IGraphNode(position),
	m_size(size),
	m_scale(scale),
	m_lifeTime(0),
	m_frameDuration(frameDuration)
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
	this->m_mesh.attribPtr(0, 2, 0, 2 * sizeof(float));
	this->m_shader.init(vs, fs);

	for (auto& path : sprites)
		this->m_textures.emplace_back(Rendering::Texture{ path, 0 });
}

void Rendering::AnimatedSprite::UpdateGraphics(float deltaTime, glm::vec2 const& winSize)
{
	this->m_lifeTime += deltaTime;
}

void Rendering::AnimatedSprite::Render(glm::vec2 const& winSize)
{
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	this->m_shader.use();
	this->m_shader.setVec2("u_position", this->position);
	this->m_shader.setVec2("u_winSize", winSize);
	this->m_shader.setVec2("u_size", this->m_size);
	this->m_shader.setVec2("u_scale", m_scale);

	auto& currentTexture = this->CurrentTexture();

	this->m_shader.setInt("u_texture", currentTexture.GetTextureUnit());
	currentTexture.Use();

	this->m_mesh.draw();

	glDisable(GL_BLEND);
}


glm::vec2 Rendering::AnimatedSprite::GetActualSize() const
{
	return this->m_size * this->m_scale;
}

Rendering::Texture &Rendering::AnimatedSprite::CurrentTexture()
{
	return this->m_textures[(int)(m_lifeTime / m_frameDuration) % this->m_textures.size()];
}
