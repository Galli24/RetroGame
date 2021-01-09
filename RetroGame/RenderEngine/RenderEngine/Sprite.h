#pragma once

#include <string>
#include <glm/glm.hpp>

#include "IGraphNode.h"
#include "IRenderable.h"
#include "Shader.h"
#include "Mesh.h"
#include "Texture.h"

namespace Rendering {


	class Sprite : public Rendering::Interface::IGraphNode
	{

	public:
		Sprite(std::string const& path, glm::vec2 const& position, glm::vec2 const& size, glm::vec2 const& scale = glm::vec2{ 1, 1 });


		void UpdateGraphics(float deltaTime, glm::vec2 const& winSize) override;
		void Render(glm::vec2 const& winSize) override;


		glm::vec2 GetActualSize() const;

	private:
		Rendering::Shader	m_shader;
		Rendering::Mesh		m_mesh;
		glm::vec2			m_size;
		glm::vec2			m_scale;
		Rendering::Texture	m_texture;
	};

}
