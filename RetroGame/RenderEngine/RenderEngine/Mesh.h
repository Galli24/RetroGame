#pragma once

#include <glad/glad.h>



namespace Rendering {

	class Mesh
	{
	public:
		Mesh() : m_vbo(-1), m_vao(-1), m_size(-1) {}

		void init(float const* vertices, int const bufferSize, int const nbVertex);
		void draw();

	private:
		unsigned m_vbo;
		unsigned m_vao;
		unsigned m_size;
	};

}
