#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>

#include <guiddef.h>
#include <combaseapi.h>
#include <vector>
#include <string>
#include <map>

#include "Mesh.h"
#include "Shader.h"
#include "Texture.h"
#include <xutility>
namespace Rendering {


	class Font
	{
	public:
		class Character {
		public:
			Rendering::Texture texture;
			glm::ivec2 bearing;
			unsigned int advance;

			Character(const Rendering::Texture &t, const glm::ivec2 b, unsigned int a) : texture(t), bearing(b), advance(a) { }
		};


	public:
		Font(std::string const& path, int fontSize);
		~Font();
		void RenderText(std::string const& str, glm::vec2 const& position, glm::ivec2 const& winSize, glm::vec4 const& color);
		void RenderChar(unsigned char const c, glm::vec2 const& position, glm::ivec2 const& winSize, glm::vec4 const& color);
		glm::vec2 EvaluateSize(std::string const& str);
		glm::vec2 GetCharMaxSize();
		float EvaluateYOffset(std::string const& str);


	private:
		std::map<unsigned char, class Rendering::Font::Character*>	m_charTextures;
		Rendering::Mesh				m_mesh;
		Rendering::Shader			m_shader;

	};


}