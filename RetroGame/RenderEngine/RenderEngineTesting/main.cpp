#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include "SceneGraph.h"
#include "Sprite.h"


int main(void)
{
	Rendering::SceneGraph sceneGraph({ 1920, 1080 }, "RetroGame");



	float lastFrame = 0;
	float deltaTime = 0;
	auto &win = sceneGraph.GetWindow();
	win.clearColor = { 1, 1, 1, 1 };
	auto sprite = Rendering::Sprite{ {0, 0}, {100, 100} };
	sceneGraph.AddNode(&sprite);

	// Loop until the user closes the window

	glBindFramebuffer(GL_FRAMEBUFFER, 0);

	while (!win.ShouldClose())
	{
		auto currentFrame = (float)glfwGetTime();
		deltaTime = currentFrame - lastFrame;
		lastFrame = currentFrame;
		sceneGraph.Render(deltaTime);
	}

	return 0;
}