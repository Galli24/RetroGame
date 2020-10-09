#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include "Camera.h"
#include "SceneGraph.h"
#include "Plane.h"

void ProcessInput(GLFWwindow* window, rendering::Camera* camera, float deltaTime)
{
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
		glfwSetWindowShouldClose(window, true);

	if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
		camera->ProcessKeyboard(rendering::Camera::FORWARD, deltaTime);
	if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
		camera->ProcessKeyboard(rendering::Camera::BACKWARD, deltaTime);
	if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
		camera->ProcessKeyboard(rendering::Camera::LEFT, deltaTime);
	if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
		camera->ProcessKeyboard(rendering::Camera::RIGHT, deltaTime);

}


int main(void)
{
	rendering::SceneGraph sceneGraph({ 1080, 720 }, "OpenGL Window");


	// OpenGL Init
	sceneGraph.Init();
	glEnable(GL_DEPTH_TEST);


	rendering::Camera camera;

	// Planes (plane's matrix depends on its parent which is plane2)
	rendering::Plane plane(glm::rotate(glm::mat4(1.f), glm::radians(90.f), { 0, 1, 1 }), { 5, 5 }, "./Textures/debug.png");

	auto plane2basis = glm::rotate(glm::mat4(1.f), glm::radians(90.f), { 1, 0, 0 });
	plane2basis = glm::translate(plane2basis, { 2, 2, 2 });
	rendering::Plane plane2(plane2basis, { 5, 5 }, "./Textures/metal.png");

	// Setup SceneGraph
	sceneGraph.SetCamera(&camera);
	sceneGraph.SetClearColor({ 0.1f, 0.1f, 0.1f, 1 });

	// Setup Rendering Tree
	plane2.AddNode(&plane);
	sceneGraph.AppendNode(&plane2);

	float lastFrame = 0;
	float deltaTime = 0;
	auto win = sceneGraph.GetWindow();


	// Loop until the user closes the window

	glBindFramebuffer(GL_FRAMEBUFFER, 0);

	while (!glfwWindowShouldClose(win))
	{
		float currentFrame = glfwGetTime();
		deltaTime = currentFrame - lastFrame;
		lastFrame = currentFrame;

		// Movement inputs
		ProcessInput(win, &camera, deltaTime);

		// Actually render the scene
		sceneGraph.Render(camera);
	}

	return 0;
}