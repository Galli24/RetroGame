#include "Mesh.h"


void Rendering::Mesh::init(float const* vertices, int const bufferSize, int const nbVertex)
{
	m_size = nbVertex;
	glGenVertexArrays(1, &m_vao);
	glGenBuffers(1, &m_vbo);

	glBindVertexArray(m_vao);

	glBindBuffer(GL_ARRAY_BUFFER, m_vbo);
	glBufferData(GL_ARRAY_BUFFER, bufferSize, vertices, GL_STATIC_DRAW);
}

void Rendering::Mesh::attribPtr(int index, int size, int ptr, int stride)
{
	glVertexAttribPointer(index, size, GL_FLOAT, GL_FALSE, stride, (void*)ptr);
	glEnableVertexAttribArray(index);

}

void Rendering::Mesh::updateBuffer(int const size, const void* vertices)
{
	glBindBuffer(GL_ARRAY_BUFFER, m_vbo);
	glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(vertices), vertices);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
}

void Rendering::Mesh::draw()
{
	glBindVertexArray(m_vao);
	glDrawArrays(GL_TRIANGLES, 0, m_size);
}
