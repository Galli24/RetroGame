#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include "SceneGraph.h"
#include "AnimatedSprite.h"
#include "Font.h"
#include "Button.h"
#include "TextBlock.h"
#include <string>
#include "MenuManager.h"
#include "IMenu.h"

int main(void)
{
	Rendering::MenuManager menuManager;
	Rendering::SceneGraph sceneGraph({ 2560, 1440 }, "RetroGame", menuManager);



	float lastFrame = 0;
	float deltaTime = 0;
	auto& win = sceneGraph.GetWindow();
	win.clearColor = { .5, .5, .5, 1 };
	auto sprite = Rendering::AnimatedSprite{
		{"./Sprites/Bowser.png", "./Sprites/BowserPink.png", "./Sprites/BowserBlue.png"},
		0.5,
		{0, 0},
		{64, 96},
		{5, 5}
	};
	sceneGraph.nodes.push_back(&sprite);

	auto font = Rendering::Font("./Fonts/Roboto.ttf", 36, 0);
	auto textblock = Rendering::TextBlock({ win.size.x, 0 }, Rendering::Interface::IMenu::Anchor::BottomRight, "", &font, { 10, 10 });
	textblock.border_color = { 0, 0, 0, 0 };

	sceneGraph.GetMenuManager().menu_nodes.push_back(&textblock);

	glBindFramebuffer(GL_FRAMEBUFFER, 0);

	while (!win.ShouldClose())
	{
		auto currentFrame = (float)glfwGetTime();
		deltaTime = currentFrame - lastFrame;
		lastFrame = currentFrame;
		std::stringstream ss1;
		ss1 << std::round(1 / deltaTime) << "fps / " << std::round(deltaTime * 1000) << "ms";
		textblock.text = ss1.str();
		sceneGraph.Update(deltaTime);
		sceneGraph.Render(deltaTime);
		sceneGraph.Blit();
	}

	return 0;
}