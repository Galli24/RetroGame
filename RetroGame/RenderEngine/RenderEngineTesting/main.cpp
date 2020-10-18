#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include "SceneGraph.h"


int main(void)
{
	rendering::SceneGraph sceneGraph({ 1080, 720 }, "RetroGame");



	float lastFrame = 0;
	float deltaTime = 0;
	auto &win = sceneGraph.GetWindow();
	win.clearColor = { 1, 1, 1, 1 };

	// Loop until the user closes the window

	glBindFramebuffer(GL_FRAMEBUFFER, 0);

	while (!win.ShouldClose())
	{
		float currentFrame = glfwGetTime();
		deltaTime = currentFrame - lastFrame;
		lastFrame = currentFrame;
		sceneGraph.Render();
	}

	return 0;
}