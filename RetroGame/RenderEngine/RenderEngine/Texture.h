#pragma once
#include <glad\glad.h>
#include <stb_image.h>
#include <iostream>
#include <string>
namespace Rendering {

	class Texture
	{

	public:
		Texture(std::string const& path, int const textureUnit);

		void Use() const;
		unsigned int GetTextureUnit(void) const noexcept { return this->m_textureUnit; }


	private:
		unsigned int m_textureUnit;
		unsigned int m_textureId;
	};

}