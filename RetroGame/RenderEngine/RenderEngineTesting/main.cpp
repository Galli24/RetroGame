#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include "SceneGraph.h"
#include "AnimatedSprite.h"
#include "Font.h"

int main(void)
{
	Rendering::SceneGraph sceneGraph({ 1920, 1080 }, "RetroGame");



	float lastFrame = 0;
	float deltaTime = 0;
	auto& win = sceneGraph.GetWindow();
	win.clearColor = { 1, 1, 1, 1 };
	//auto sprite = Rendering::AnimatedSprite{
	//	{"./Sprites/Bowser.png", "./Sprites/BowserPink.png", "./Sprites/BowserBlue.png"},
	//	0.5,
	//	{0, 0},
	//	{64, 96},
	//	{5, 5}
	//};
	//sceneGraph.nodes.push_back(&sprite);
	auto font = Rendering::Font("D:/Roboto.ttf", 96, 0);


	// Loop until the user closes the window

	glBindFramebuffer(GL_FRAMEBUFFER, 0);

	while (!win.ShouldClose())
	{
		auto currentFrame = (float)glfwGetTime();
		deltaTime = currentFrame - lastFrame;
		lastFrame = currentFrame;
		sceneGraph.Update(deltaTime);
		sceneGraph.Render(deltaTime);
		font.RenderText("abc", win.size / 2, win.size, {0.5, 0.5, 0});
		sceneGraph.Blit();
	}

	return 0;
}