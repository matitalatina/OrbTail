using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DriverStack<T> {
	private Stack<KeyValuePair<T, Deactivator>> stack = new Stack<KeyValuePair<T, Deactivator>>();
	private T prototype;

	public T GetHead() {
		KeyValuePair<T, Deactivator> elemWithSwitch = stack.Peek();

		while(elemWithSwitch.Value.IsActive() == false) {
			stack.Pop();
			elemWithSwitch = stack.Peek();
		}

		return elemWithSwitch.Key;
	}

	public T GetPrototype() {
		return prototype;
	}

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
