#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>

#include <guiddef.h>
#include <combaseapi.h>
#include <vector>

#include "Camera.h"

namespace rendering {


	// Base class for the rendering elements
	class IGraphNode
	{
	public:
		IGraphNode(glm::mat4 const& basis) : m_basis(basis), m_parent(), m_children(), m_id() { auto _ = CoCreateGuid(&this->m_id); }

		// Renders this node and its children
		virtual void Render(rendering::Camera const& camera, int framebuffer = 0) = 0;



		/**
		*	Implemented Function
		*/

		// Adds append a node the this one. They will be rendered when the current node is rendered.
		int AddNode(IGraphNode* node) {
			this->m_children.push_back(node);
			node->SetParent(this);
			return this->m_children.size() - 1;
		}

		// Remove a node by ID
		void RemoveNode(GUID const& id) {
			for (auto it = this->m_children.begin(); it != this->m_children.end(); it++)
			{
				if ((*it)->GetID() == id) {
					this->m_children.erase(it);
					break;
				}
			}

		}

		// Remove a node by index
		void RemoveNode(int index) {
			this->m_children.erase(this->m_children.begin() + index);
		}

		// Gets the nodes count
		int GetNodeCount(void) const {
			return this->m_children.size();
		}

		// Gets the basis of this node
		glm::mat4x4 GetBasis(void) const {
			return this->m_basis;
		}

		// Sets the basis of this node
		void SetBasis(glm::mat4x4 const& basis) {
			this->m_basis = basis;
		}

		// Gets the GUID of this node
		GUID GetID() const noexcept {
			return this->m_id;
		}

		// Gets the parent of this node
		IGraphNode* GetParent() const noexcept {
			return this->m_parent;
		}

		// Sets manually the parent of this node
		void SetParent(IGraphNode* parent) noexcept {
			this->m_parent = parent;
		}

		// Get the matrix of the parents
		glm::mat4x4 InWorld() const {
			auto parent = this->m_parent;
			auto matrix = glm::mat4(1.f);
			while (parent != nullptr)
			{
				matrix = parent->GetBasis() * matrix;
				parent = parent->GetParent();
			}
			return matrix;
		}


	protected:
		std::vector<IGraphNode*>	m_children;
		IGraphNode* m_parent;
		glm::mat4x4					m_basis;
		GUID						m_id;
	};

}
