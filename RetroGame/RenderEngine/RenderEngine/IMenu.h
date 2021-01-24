#pragma once
#include "IGraphNode.h"
#include "IInteractibleObject.h"

namespace Rendering {

	namespace Interface {

		class IMenu : public IGraphNode, public IInteractibleObject
		{
		public:
			enum class Anchor : int {
				TopLeft, Top, TopRight,
				Left, Center, Right,
				BottomLeft, Bot, BottomRight
			};

		public:
			IMenu(glm::vec2 const& pos, Anchor anchor = Anchor::TopLeft) : IGraphNode(pos), m_anchor(anchor) {}

			virtual void OnFocus() = 0;
			virtual void OnLostFocus() = 0;
			virtual glm::vec2 GetObjectSize() const = 0;



			glm::vec2 GetActualPosition() const
			{
				auto pos = position;
				auto size = GetObjectSize();
				if (m_anchor == Anchor::BottomRight || m_anchor == Anchor::Right || m_anchor == Anchor::TopRight)
					pos.x -= size.x;
				else if (m_anchor == Anchor::Top || m_anchor == Anchor::Center || m_anchor == Anchor::Bot)
					pos.x -= std::floor(size.x / 2.f);	

				if (m_anchor == Anchor::TopLeft || m_anchor == Anchor::Top || m_anchor == Anchor::TopRight)
					pos.y -= size.y;
				else if (m_anchor == Anchor::Left || m_anchor == Anchor::Center || m_anchor == Anchor::Right)
					pos.y -= std::floor(size.y / 2.f);

				return pos;
			}


		protected:
			Anchor		m_anchor;
		};
	}

}