#include "Texture.h"


Rendering::Texture::Texture(std::string const& path, int const textureUnit) : m_textureUnit(textureUnit)
{
	glGenTextures(1, &this->m_textureId);

	int width, height, nrComponents;
	unsigned char* data = stbi_load(path.c_str(), &width, &height, &nrComponents, 0);
	m_size = { width, height };
	if (data)
	{
		GLenum format = 0;
		if (nrComponents == 1)
			format = GL_RED;
		else if (nrComponents == 3)
			format = GL_RGB;
		else if (nrComponents == 4)
			format = GL_RGBA;

		glActiveTexture(GL_TEXTURE0 + this->m_textureUnit);
		glBindTexture(GL_TEXTURE_2D, this->m_textureId);
		glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, GL_UNSIGNED_BYTE, data);
		glGenerateMipmap(GL_TEXTURE_2D);

		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

	}
	else
		std::cout << "Texture failed to load at path: " << path << std::endl;
	stbi_image_free(data);
}


Rendering::Texture::Texture(unsigned char* ptr, int const textureUnit, GLenum format, glm::ivec2 const &size) : m_textureUnit(textureUnit), m_size(size)
{
	glGenTextures(1, &this->m_textureId);
	glActiveTexture(GL_TEXTURE0 + this->m_textureUnit);
	glBindTexture(GL_TEXTURE_2D, this->m_textureId);
	glTexImage2D(GL_TEXTURE_2D, 0, format, size.x, size.y, 0, format, GL_UNSIGNED_BYTE, ptr);
	glGenerateMipmap(GL_TEXTURE_2D);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
}

void Rendering::Texture::Use() const
{
	glActiveTexture(GL_TEXTURE0 + this->m_textureUnit);
	glBindTexture(GL_TEXTURE_2D, this->m_textureId);
}

glm::ivec2 const& Rendering::Texture::GetSize(void) const
{
	return this->m_size;
}

