#include <ft2build.h>
#include <freetype/freetype.h>

#include "Font.h"
#include <exception>
#include <xutility>


namespace Rendering {

	Font::Font(std::string const& path, int fontSize, int id) : ID(id), m_charTextures()
	{
		FT_Library ft = nullptr;
		if (FT_Init_FreeType(&ft))
			throw std::exception("Can't init freetype");

		FT_Face face = nullptr;
		if (FT_New_Face(ft, path.c_str(), 0, &face))
			throw std::exception("Can't load font");
		FT_Set_Pixel_Sizes(face, 0, fontSize);

		glPixelStorei(GL_UNPACK_ALIGNMENT, 1); // disable byte-alignment restriction

		for (unsigned char c = 0; c < 128; c++)
		{
			if (FT_Load_Char(face, c, FT_LOAD_RENDER))
				throw std::exception("Can't load Glyph");

			auto ch = new Rendering::Font::Character {
				{
					face->glyph->bitmap.buffer,
					0,
					GL_RED,
					glm::ivec2(face->glyph->bitmap.width, face->glyph->bitmap.rows)
				},
				glm::ivec2(face->glyph->bitmap_left, face->glyph->bitmap_top),
				(unsigned int)face->glyph->advance.x };

			this->m_charTextures[c] = ch;
		}
		glBindTexture(GL_TEXTURE_2D, 0);
		FT_Done_Face(face);
		FT_Done_FreeType(ft);

		std::string fs =
			"#version 330 core\n"
			"in vec2 o_texCoords;\n"

			"uniform sampler2D u_texture;\n"
			"uniform vec3 u_color;\n"

			"out vec4 FragColor; \n"

			"void main(void) {\n"
			"  FragColor = vec4(texture(u_texture, vec2(o_texCoords.x, o_texCoords.y * -1)).x) * vec4(u_color, 1); \n"
			"} \n";

		const auto vs =
			"#version 330 core\n"
			"layout(location = 0) in vec2 l_pos;   \n"

			"uniform vec2 u_position; \n"
			"uniform vec2 u_winSize; \n"
			"uniform vec2 u_size; \n"
			"uniform vec2 u_scale; \n"

			"out vec2 o_texCoords;\n"

			"void main()   \n"
			"{   \n"
			"  vec2 resized = ((l_pos * u_size * u_scale) + u_position) * 2 / u_winSize;  \n"
			"  o_texCoords = l_pos;	\n"
			"  gl_Position = vec4(resized - 1, 0, 1.0);   \n"
			"}   \n";

		const float vertices[] = {
			//  X, Y,
				0, 0,
				1, 0,
				1, 1,
				0, 0,
				1, 1,
				0, 1,
		};

		m_mesh.init(vertices, sizeof(vertices), 6);
		m_mesh.attribPtr(0, 2, 0, 2 * sizeof(float));
		m_shader.init(vs, fs);
	}





	Font::~Font()
	{
		for (auto& elt : this->m_charTextures)
			delete elt.second;
	}

	void Font::RenderText(std::string const& text, glm::vec2 const& position, glm::ivec2 const& winSize, glm::vec3 const& color)
	{
		m_shader.use();
		glActiveTexture(GL_TEXTURE0);

		std::string::const_iterator c;
		float x = 0;
		for (c = text.begin(); c != text.end(); c++)
		{
			auto ch = m_charTextures[*c];
			this->RenderChar(*c, {position.x + x, position.y}, winSize, color);
			x += (ch->advance >> 6);
		}
		glBindVertexArray(0);
		glBindTexture(GL_TEXTURE_2D, 0);
	}

	void Font::RenderChar(unsigned char const c, glm::vec2 const& position, glm::ivec2 const& winSize, glm::vec3 const& color)
	{
		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
		auto ch = m_charTextures[c];

		float xpos = position.x + ch->bearing.x;
		glm::ivec2 const& size = ch->texture.GetSize();
		float ypos = position.y - (size.y - ch->bearing.y);

		float w = size.x;
		float h = size.y;

		ch->texture.Use();
		m_shader.setInt("u_texture", 0);
		m_shader.setVec2("u_position", position);
		m_shader.setVec3("u_color", color);
		m_shader.setVec2("u_winSize", winSize);
		m_shader.setVec2("u_size", ch->texture.GetSize());
		m_shader.setVec2("u_scale", { 1, 1 });
		this->m_mesh.draw();
		glDisable(GL_BLEND);
	}

}
