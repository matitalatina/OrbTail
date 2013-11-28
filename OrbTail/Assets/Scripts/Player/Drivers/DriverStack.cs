using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DriverStack<T> {
	private Stack<KeyValuePair<T, Deactivator>> stack = new Stack<KeyValuePair<T, Deactivator>>();
	private T prototype;

	/// <summary>
	/// Gets the head of the stack
	/// </summary>
	/// <returns>The head of the stack.</returns>
	public T GetHead() {
		KeyValuePair<T, Deactivator> elemWithSwitch = stack.Peek();

		while(elemWithSwitch.Value.IsActive() == false) {
			stack.Pop();
			elemWithSwitch = stack.Peek();
		}

		return elemWithSwitch.Key;
	}

	/// <summary>
	/// Gets the prototype which is the first inserted.
	/// </summary>
	/// <returns>The prototype. It cannot be deactivated.</returns>
	public T GetPrototype() {
		return prototype;
	}

	/// <summary>
	/// Push the specified element.
	/// </summary>
	/// <returns>The deactivator: with this object you can deactivate
	/// the element that you are pushing in the stack.</returns>
	/// <param name="elem">The element to push in.</param>
	public Deactivator Push(T elem) {
		Deactivator deactivator;
		KeyValuePair<T, Deactivator> elemWithSwitch = new KeyValuePair<T, Deactivator>(elem, new Deactivator());

		if (stack.Count > 0) {
			deactivator = elemWithSwitch.Value;
		}
		else {
			deactivator = null;
			prototype = elem;
		}

		stack.Push(elemWithSwitch);

		return deactivator;
	}
}
