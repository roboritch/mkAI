using UnityEngine.EventSystems;

public interface ISelectable : IEventSystemHandler{
	void selected();

	void deselected();
}
