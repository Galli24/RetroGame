#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include <string>

namespace rendering {

	typedef glm::vec<2, int, glm::defaultp> vec2int;

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
		void	OnScroll(int const x, int const y);
		void	OnMouseMove(int const x, int const y);

		/*
			Properties
		*/

		// Ranged Value [0, 1]
		glm::vec4 clearColor;


	private:
		GLFWwindow*		m_window;
		std::string		m_windowName;
		vec2int			m_size;
		glm::vec2		m_mousePosition;
	};

}
