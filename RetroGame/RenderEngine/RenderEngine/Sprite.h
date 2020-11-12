#pragma once

#include <string>
#include <glm/glm.hpp>

#include "IGraphNode.h"
#include "IRenderable.h"
#include "Shader.h"
#include "Mesh.h"

namespace Rendering {


	class Sprite : public Rendering::Interface::IGraphNode
	{

	public:
		Sprite(glm::vec2 const& position, glm::vec2 const& size);


		void UpdateGraphics(float deltaTime, glm::vec2 const& winSize) override;
		void Render(glm::vec2 const& winSize) override;

	private:
		Rendering::Shader	m_shader;
		Rendering::Mesh		m_mesh;
		glm::vec2			m_size;
	};

}
