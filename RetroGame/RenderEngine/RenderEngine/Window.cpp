#include "Window.h"
#include <iostream>


Rendering::Window::Window(std::string const& name, glm::ivec2 const& size, Rendering::MenuManager* menuManager)
	: m_window(nullptr), m_windowName(name), size(size), m_mousePosition({ 0, 0 }), clearColor({ 0, 0, 0, 1 }), m_menuManager(menuManager)
{
	if (!glfwInit())
		throw std::exception("Can't init GLFW");

	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef _DEBUG
	this->m_window = glfwCreateWindow(this->size[0], this->size[1], this->m_windowName.c_str(), nullptr, nullptr);
#else
	this->m_window = glfwCreateWindow(this->size[0], this->size[1], this->m_windowName.c_str(), glfwGetPrimaryMonitor(), nullptr);
#endif
	if (!this->m_window)
	{
		glfwTerminate();
		throw std::exception("Can't create GLFW window");
	}

	glfwSetWindowUserPointer(this->m_window, this);


	auto resize = [](GLFWwindow* win, int x, int y) {
		auto window = static_cast<Rendering::Window*>(glfwGetWindowUserPointer(win));
		window->OnWindowResize(x, y);

	};

	auto cursorPos = [](GLFWwindow* win, double x, double y) {
		auto window = static_cast<Rendering::Window*>(glfwGetWindowUserPointer(win));
		window->OnMouseMove(x, window->size[1] - y);
	};

	auto scroll = [](GLFWwindow* win, double x, double y) {
		auto window = static_cast<Rendering::Window*>(glfwGetWindowUserPointer(win));
		window->OnScroll(x, y);
	};

	auto mouseButton = [](GLFWwindow* win, int button, int action, int tamer) {
		auto window = static_cast<Rendering::Window*>(glfwGetWindowUserPointer(win));
		window->OnMousePress(button, action);
	};

	auto keyAction = [](GLFWwindow* win, int key, int scancode, int action, int mods)
	{
		auto window = static_cast<Rendering::Window*>(glfwGetWindowUserPointer(win));
		window->OnKeyAction(key, scancode, action, mods);
	};

	auto charAction = [](GLFWwindow* win, unsigned int codepoint)
	{
		auto window = static_cast<Rendering::Window*>(glfwGetWindowUserPointer(win));
		window->OnCharAction(codepoint);
	};

	glfwMakeContextCurrent(this->m_window);

	glfwSetKeyCallback(this->m_window, keyAction);
	glfwSetFramebufferSizeCallback(this->m_window, resize);
	glfwSetCursorPosCallback(this->m_window, cursorPos);
	glfwSetScrollCallback(this->m_window, scroll);
	glfwSetMouseButtonCallback(this->m_window, mouseButton);
	glfwSetCharCallback(this->m_window, charAction);
	//glfwSetInputMode(this->m_window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);


}

Rendering::Window::~Window()
{
	glfwDestroyWindow(m_window);
}

void Rendering::Window::ClearWindow() const
{
	glClearColor(this->clearColor[0], this->clearColor[1], this->clearColor[2], this->clearColor[3]);

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}

void Rendering::Window::BlitWindow() const
{
	glfwSwapBuffers(this->m_window);
	glfwPollEvents();
}

bool Rendering::Window::ShouldClose() const
{
	return glfwWindowShouldClose(this->m_window);
}


void Rendering::Window::OnWindowResize(int const x, int const y)
{
	glViewport(0, 0, x, y);
	this->size = { x, y };
	std::cout << "Resize: x = " << x << ", y = " << y << std::endl;
}

void Rendering::Window::OnScroll(double const x, double const y)
{
	m_menuManager->OnScroll(x, y);
}

void Rendering::Window::OnMouseMove(double const xpos, double const ypos)
{
	static bool firstMouse = true;
	static double lastX = 0, lastY = 0;


	if (firstMouse)
	{
		lastX = xpos;
		lastY = ypos;
		firstMouse = false;
	}

	auto xoffset = xpos - lastX;
	auto yoffset = lastY - ypos; // reversed since y-coordinates go from bottom to top

	lastX = xpos;
	lastY = ypos;

	this->m_mousePosition = { xpos, ypos };
	m_menuManager->OnMouseMove(m_mousePosition.x, m_mousePosition.y);
}

void Rendering::Window::OnMousePress(int const button, int const action)
{
	if (action == GLFW_PRESS)
		m_menuManager->OnMousePress(button, m_mousePosition.x, m_mousePosition.y);
	else if (action == GLFW_RELEASE)
		m_menuManager->OnMouseRelease(button, m_mousePosition.x, m_mousePosition.y);

}

void Rendering::Window::OnKeyAction(int key, int scancode, int action, int mods)
{
	if (key == GLFW_KEY_ESCAPE)
		glfwSetWindowShouldClose(this->m_window, true);

	if (action == GLFW_PRESS || action == GLFW_REPEAT)
		m_menuManager->OnKeyPressed(key, mods);
	else if (action == GLFW_RELEASE)
		m_menuManager->OnKeyRelease(key, mods);
}



void Rendering::Window::OnCharAction(unsigned int codepoint)
{
	if (codepoint < 128)
		m_menuManager->OnCharReceived((char)codepoint);
}
