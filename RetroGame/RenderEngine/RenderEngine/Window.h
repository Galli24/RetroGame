#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include <string>

namespace Rendering {

	using vec2int = glm::vec<2, int, glm::defaultp>;

	class Window
	{
	public:
		Window(std::string const& name, vec2int const& size);


		/*
			Logic
		*/
		void	ClearWindow() const;
		void	BlitWindow() const;
		bool	ShouldClose() const;
		void	ProcessInput() const;

		/*
			Callbacks
		*/

		void	OnWindowResize(int const x, int const y);
		void	OnScroll(double const x, double const y);
		void	OnMouseMove(double const x, double const y);

		/*
			Properties
		*/

		// Ranged Value [0, 1]
		glm::vec4		clearColor;
		vec2int			size;


	private:
		GLFWwindow*		m_window;
		std::string		m_windowName;
		glm::vec2		m_mousePosition;
	};

}
