#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>

#include <guiddef.h>
#include <combaseapi.h>
#include <vector>
#include <string>
#include <map>
#include "Font.h"




class TextManager
{

public:
	static Rendering::Font LoadFont(std::string const& path, int const fontSize);
	static glm::vec2 RenderText(int font, std::string const& str, int fontSize, glm::vec2 position);


};

