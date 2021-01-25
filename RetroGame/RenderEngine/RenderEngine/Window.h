#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include <string>
#include "MenuManager.h"

namespace Rendering {


	class Window
	{
	public:
		Window(std::string const& name, glm::ivec2 const& size, Rendering::MenuManager *menuManager);
		~Window();
		/*
			Logic
		*/
		void	ClearWindow() const;
		void	BlitWindow() const;
		bool	ShouldClose() const;

		/*
			Callbacks
		*/

		virtual void	OnWindowResize(int const x, int const y);
		virtual void	OnScroll(double const x, double const y);
		virtual void	OnMouseMove(double const x, double const y);
		virtual void	OnMousePress(int const button, int const action);
		virtual void	OnKeyAction(int key, int scancode, int action, int mods);
		virtual void	OnCharAction(unsigned int codepoint);
		/*
			Properties
		*/

		// Ranged Value [0, 1]
		glm::vec4		clearColor;
		glm::ivec2		size;

	private:
		GLFWwindow*				m_window;
		std::string				m_windowName;
		glm::vec2				m_mousePosition;
		Rendering::MenuManager	*m_menuManager;
	};

}
