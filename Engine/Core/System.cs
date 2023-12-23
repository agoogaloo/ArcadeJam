using System.Collections.Generic;

namespace Engine.Core;
public abstract class System {
	protected List<Function> components = new();

	public virtual void Start() { }
	public virtual void End() { }

	public void AddComponent(Function component) {
		components.Add(component);
	}

	public void RemoveComponent(Function component){
		components.Remove(component);
	}


}
