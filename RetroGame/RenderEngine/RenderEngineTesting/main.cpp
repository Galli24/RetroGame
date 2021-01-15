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
	Rendering::SceneGraph sceneGraph({ 1920, 1080 }, "RetroGame", &menuManager);



	float lastFrame = 0;
	float deltaTime = 0;
	auto& win = sceneGraph.GetWindow();
	win.clearColor = { 0,0,0,1 };
	auto sprite = Rendering::AnimatedSprite{
		{"./Sprites/Bowser.png", "./Sprites/BowserPink.png", "./Sprites/BowserBlue.png"},
		0.5,
		{0, 0},
		{64, 96},
		{5, 5}
	};
	sceneGraph.nodes.push_back(&sprite);

	auto font = Rendering::Font("./Fonts/arial.ttf", 36, 0);

	auto textblock = Rendering::TextBlock({ win.size.x, 0 }, Rendering::Interface::IMenu::Anchor::BottomRight, "", &font, { 10, 10 });
	auto topLeft = Rendering::TextBlock({ 0, win.size.y }, Rendering::Interface::IMenu::Anchor::TopLeft, "topLeft", &font, { 10, 10 });
	auto top = Rendering::TextBlock({ win.size.x / 2, win.size.y }, Rendering::Interface::IMenu::Anchor::Top, "top", &font, { 10, 10 });
	//auto topRight = Rendering::TextBlock({ win.size.x, win.size.y }, Rendering::Interface::IMenu::Anchor::TopRight, "topRight", &font, { 10, 10 });
	auto left = Rendering::TextBlock({ 0, win.size.y / 2 }, Rendering::Interface::IMenu::Anchor::Left, "left", &font, { 10, 10 });
	auto center = Rendering::Button({ win.size.x, win.size.y }, Rendering::Interface::IMenu::Anchor::TopRight, "BUTTON", &font, { 10, 10 });
	auto right = Rendering::TextBlock({ win.size.x, win.size.y / 2 }, Rendering::Interface::IMenu::Anchor::Right, "right", &font, { 10, 10 });
	auto bottomLeft = Rendering::TextBlock({ 0, 0 }, Rendering::Interface::IMenu::Anchor::BottomLeft, "bottomLeft", &font, { 10, 10 });
	auto bottom = Rendering::TextBlock({ win.size.x / 2, 0 }, Rendering::Interface::IMenu::Anchor::Bot, "bottom", &font, { 10, 10 });

	center.padding = { 50, 150 };
	center.border_size = 50;
	sceneGraph.GetMenuManager()->menu_nodes.push_back(&textblock);
	sceneGraph.GetMenuManager()->menu_nodes.push_back(&topLeft);
	sceneGraph.GetMenuManager()->menu_nodes.push_back(&top);
	//sceneGraph.GetMenuManager()->menu_nodes.push_back(&topRight);
	sceneGraph.GetMenuManager()->menu_nodes.push_back(&left);
	sceneGraph.GetMenuManager()->menu_nodes.push_back(&center);
	sceneGraph.GetMenuManager()->menu_nodes.push_back(&right);
	sceneGraph.GetMenuManager()->menu_nodes.push_back(&bottomLeft);
	sceneGraph.GetMenuManager()->menu_nodes.push_back(&bottom);

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