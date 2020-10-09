#include <iostream>

#include "SceneGraph.h"


rendering::SceneGraph::SceneGraph(vec2int const& windowSize, std::string const& windowName)
	: m_nodes(), m_clearColor(), m_window(), m_windowSize(windowSize), m_windowName(windowName)
{
}

rendering::SceneGraph::~SceneGraph()
{
	glfwTerminate();
}

bool rendering::SceneGraph::Init()
{
	if (!glfwInit())
		return false;
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);


	this->m_window = glfwCreateWindow(this->m_windowSize[0], this->m_windowSize[1], this->m_windowName.c_str(), NULL, NULL);
	if (!this->m_window)
	{
		glfwTerminate();
		return false;
	}

	glfwSetWindowUserPointer(this->m_window, this);


	auto resize = [](GLFWwindow* win, int x, int y) {
		auto sceneGraph = static_cast<SceneGraph*>(glfwGetWindowUserPointer(win));
		sceneGraph->WindowResizeCallback(x, y);

	};

	auto cursorPos = [](GLFWwindow* win, double x, double y) {
		auto sceneGraph = static_cast<SceneGraph*>(glfwGetWindowUserPointer(win));
		sceneGraph->MouseCallback(x, y);
	};

	auto scroll = [](GLFWwindow* win, double x, double y) {
		auto sceneGraph = static_cast<SceneGraph*>(glfwGetWindowUserPointer(win));
		sceneGraph->ScrollCallback(x, y);
	};

	glfwMakeContextCurrent(this->m_window);
	glfwSetFramebufferSizeCallback(this->m_window, resize);
	glfwSetCursorPosCallback(this->m_window, cursorPos);
	glfwSetScrollCallback(this->m_window, scroll);

	glfwSetInputMode(this->m_window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);


	if (!gladLoadGL()) {
		glfwTerminate();
		return false;
	}
	return true;
}

void rendering::SceneGraph::Render(rendering::Camera const& camera)
{
	this->Clear();


	for (auto& node : m_nodes)
		node->Render(camera);


	this->Blit();

}

int rendering::SceneGraph::AppendNode(IGraphNode* node)
{
	this->m_nodes.push_back(node);
	return this->m_nodes.size() - 1;
}

void rendering::SceneGraph::RemoveNode(GUID const& id)
{
	for (int i = 0; i < this->m_nodes.size(); i++)
	{
		if (this->m_nodes[i]->GetID() == id)
			this->m_nodes.erase(this->m_nodes.begin() + i);
	}
}

void rendering::SceneGraph::RemoveNode(int const index)
{
	this->m_nodes.erase(this->m_nodes.begin() + index);
}

void rendering::SceneGraph::SetClearColor(glm::vec4 const& color)
{
	this->m_clearColor = color;
}

void rendering::SceneGraph::MouseCallback(double const xpos, double const ypos)
{
	if (!this->m_camera)
		return;


	static bool firstMouse = true;
	static double lastX = 0, lastY = 0;


	if (firstMouse)
	{
		lastX = xpos;
		lastY = ypos;
		firstMouse = false;
	}

	float xoffset = xpos - lastX;
	float yoffset = lastY - ypos; // reversed since y-coordinates go from bottom to top

	lastX = xpos;
	lastY = ypos;


	this->m_camera->ProcessMouseMovement(xoffset, yoffset);

}

void rendering::SceneGraph::WindowResizeCallback(int const width, int const height)
{
	glViewport(0, 0, width, height);
}

void rendering::SceneGraph::ScrollCallback(double const xoff, double const yoff)
{
}

GLFWwindow* rendering::SceneGraph::GetWindow() const
{
	return this->m_window;
}

void rendering::SceneGraph::Clear()
{
	glClearColor(this->m_clearColor[0], this->m_clearColor[1], this->m_clearColor[2], this->m_clearColor[3]);

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}

void rendering::SceneGraph::Blit()
{
	glfwSwapBuffers(this->m_window);
	glfwPollEvents();
}
