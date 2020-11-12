#pragma once

#include <glm/glm.hpp>
#include <string>

#include "IGraphNode.h"
#include "Texture.h"
#include "Mesh.h"
#include "Shader.h"


namespace Rendering {

	class AnimatedSprite : public Rendering::Interface::IGraphNode
	{
	public:
		AnimatedSprite(std::vector<std::string> const& sprites, float const frameDuration, glm::vec2 const& position, glm::vec2 const& size, glm::vec2 const& scale = glm::vec2{ 1, 1 });

		void UpdateGraphics(float deltaTime, glm::vec2 const& winSize) override;
		void Render(glm::vec2 const& winSize) override;
		void UpdatePosition(float deltaTime, glm::vec2 const& winSize) override;

		glm::vec2 GetActualSize() const;

		Rendering::Texture &CurrentTexture();

	private:
		Rendering::Shader				m_shader;
		Rendering::Mesh					m_mesh;
		glm::vec2						m_size;
		glm::vec2						m_scale;
		std::vector<Rendering::Texture>	m_textures;
		float							m_frameDuration;
		float							m_lifeTime;

	};

}
