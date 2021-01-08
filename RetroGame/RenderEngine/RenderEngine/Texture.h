#pragma once
#include <glad\glad.h>
#include <stb_image.h>
#include <glm/glm.hpp>
#include <iostream>
#include <string>
namespace Rendering {

	class Texture
	{

	public:
		Texture(std::string const& path, int const textureUnit);
		Texture(unsigned char* ptr, int const textureUnit, GLenum format, glm::ivec2 const& size);

		void Use() const;
		unsigned int GetTextureUnit(void) const noexcept { return this->m_textureUnit; }
		glm::ivec2 const& GetSize(void) const;

	private:
		unsigned int	m_textureUnit;
		unsigned int	m_textureId;
		glm::ivec2		m_size;
	};

}