#include "Window.h"

Rendering::Window::Window(std::string const& name, vec2int const& size)
	: m_window(nullptr), m_windowName(name), size(size), m_mousePosition({0, 0}), clearColor({1, 1, 1, 1})
{
	if (!glfwInit())
		throw std::exception("Can't init GLFW");

	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef _DEBUG
	this->m_window = glfwCreateWindow(1280, 720, this->m_windowName.c_str(), nullptr, nullptr);
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
		window->OnMouseMove(x, y);
	};

	auto scroll = [](GLFWwindow* win, double x, double y) {
		auto window = static_cast<Rendering::Window*>(glfwGetWindowUserPointer(win));
		window->OnScroll(x, y);
	};

	glfwMakeContextCurrent(this->m_window);
	glfwSetFramebufferSizeCallback(this->m_window, resize);
	glfwSetCursorPosCallback(this->m_window, cursorPos);
	glfwSetScrollCallback(this->m_window, scroll);

	glfwSetInputMode(this->m_window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);


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

void Rendering::Window::ProcessInput() const
{
	if (glfwGetKey(this->m_window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
		glfwSetWindowShouldClose(this->m_window, true);
}

void Rendering::Window::OnWindowResize(int const x, int const y)
{
	glViewport(0, 0, x, y);
	this->size = { x, y };
}

void Rendering::Window::OnScroll(double const x, double const y)
{
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

}
