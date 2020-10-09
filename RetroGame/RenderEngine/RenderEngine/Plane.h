#pragma once
#include "IGraphNode.h"
#include "Shader.h"
#include "Camera.h"
#include "Texture.h"

#include <glm/glm.hpp>


namespace rendering {

	class Plane : public IGraphNode
	{

	public:
		Plane(glm::mat4 const& normalize_basis, glm::vec2 const& size, std::string const& texturePath);

		virtual void Render(rendering::Camera const& camera, int framebuffer = 0) override;


	private:
		glm::vec2 m_size;
		float *m_planeVertices = new float[30] {
			 1.f, 0.f,  1.f, 1.0f, 0.0f,
			-1.f, 0.f,  1.f, 0.0f, 0.0f,
			-1.f, 0.f, -1.f, 0.0f, 1.0f,

			 1.f, 0.f,  1.f, 1.0f, 0.0f,
			-1.f, 0.f, -1.f, 0.0f, 1.0f,
			 1.f, 0.f, -1.f, 1.0f, 1.0f
		};
		Shader m_shader;
		GLuint m_vao;
		GLuint m_vbo;

		Texture m_texture;

	};

}

