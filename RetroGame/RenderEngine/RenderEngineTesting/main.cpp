#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include "SceneGraph.h"
#include "AnimatedSprite.h"


int main(void)
{
	Rendering::SceneGraph sceneGraph({ 1920, 1080 }, "RetroGame");



	float lastFrame = 0;
	float deltaTime = 0;
	auto& win = sceneGraph.GetWindow();
	win.clearColor = { 1, 1, 1, 1 };
	auto sprite = Rendering::AnimatedSprite{ 
		{"./Sprites/Bowser.png", "./Sprites/BowserPink.png", "./Sprites/BowserBlue.png"}, 
		{0, 0}, 
		{64, 96}, 
		{5, 5} 
	};
	sceneGraph.AddNode(&sprite);

	// Loop until the user closes the window

	glBindFramebuffer(GL_FRAMEBUFFER, 0);

	while (!win.ShouldClose())
	{
		auto currentFrame = (float)glfwGetTime();
		deltaTime = currentFrame - lastFrame;
		lastFrame = currentFrame;
		sceneGraph.Update(deltaTime);
		sceneGraph.Render(deltaTime);
	}

	return 0;
}