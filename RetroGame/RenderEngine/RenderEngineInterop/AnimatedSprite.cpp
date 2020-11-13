#include "pch.h"
#include "AnimatedSprite.h"
#include <cliext/adapter>
#include <cliext/algorithm>
#include <cliext/vector>
#include <cstring>
#include <msclr/marshal_cppstd.h>


RenderEngine::AnimatedSprite::AnimatedSprite(IEnumerable<String^>^ sprites, float frameDuration, Vector2 position, Vector2 size, Vector2 scale)
{

	std::vector<std::string> sp;
	for each (auto e in sprites)
		sp.push_back(msclr::interop::marshal_as<std::string>(e));

	this->nativeResources = new Rendering::AnimatedSprite(
		sp, frameDuration, 
		glm::vec2{ position.X, position.Y }, 
		glm::vec2{ size.X, size.Y }, 
		glm::vec2{ scale.X, scale.Y });
}

RenderEngine::AnimatedSprite::AnimatedSprite(IEnumerable<String^>^ sprites, float frameDuration, Vector2 position, Vector2 size)
{
	std::vector<std::string> sp;
	for each (auto e in sprites)
		sp.push_back(msclr::interop::marshal_as<std::string>(e));


	this->nativeResources = new Rendering::AnimatedSprite(
		sp, frameDuration,
		glm::vec2{ position.X, position.Y },
		glm::vec2{ size.X, size.Y });

}
