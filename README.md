# Germany_Unity_Test
Use keys 1,2,3 for changing objects
Correct order is Platform=>Pole=>Connector and repeat

Using stack for detecting next position for stacking

Use S and L key to save and load

Use mouse for snapping

Scroll wheel for zoom in zoom out
Right click for dragging

------------------------------------------
I used **Object-Oriented Programming (OOP) Concepts** i.e 
Encapsulation, Abstraction(public interface IBuildPlacementRule)
Inheritance & Polymorphism (BasicPlacementRule : ScriptableObject, IBuildPlacementRule)
Data Structures like Dictionary for Grid placement, Lists, Queue for other data structure
SOLID principals used for Clean code
 Design Patterns
Factory Pattern (Unity-style) → Instantiate(def.Prefab)(No need of object pooling right now)
Strategy Pattern → IBuildPlacementRule 
Observer Pattern → stability.EnqueueRecheck()
