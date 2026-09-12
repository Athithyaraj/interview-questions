# 1. What is C# and why do we use it?
C# is a modern, object-oriented, type-safe programming language developed by Microsoft. It's designed for building a wide range of applications that run on the .NET platform.

We use C# because:

It's versatile: Suitable for web, desktop, mobile, game (Unity), and cloud development.

It offers robust tooling: Visual Studio provides excellent IDE support.

It has a large community: Abundant resources and support are available.

It provides automatic memory management: Through garbage collection.

It offers features like LINQ for querying data, async/await for asynchronous programming, and generics for type safety. E.g.

// Example of a simple C# program
using System;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}


# 2. Explain the difference between value types and reference types in C#.
Value types and reference types differ in how they store data. Value types (like int, bool, struct, enum) directly hold their value within their allocated memory. When you assign a value type variable to another, a copy of the value is created. Each variable then has its own independent copy.

Reference types (like string, class, array, delegate) store a reference (a memory address) to the actual data, which is located on the heap. When you assign a reference type variable, you're copying the reference, not the data itself. Both variables will then point to the same memory location. Modifying the data through one variable will affect the other.


# 3. What is the purpose of the 'using' statement?
The using statement in C# (and similar constructs in other languages like Python's with statement) is used to ensure that resources are properly disposed of, even if exceptions occur. It provides a convenient way to automatically call the Dispose() method on an object when it goes out of scope.

Specifically, the using statement works with objects that implement the IDisposable interface. It automatically generates a try...finally block. The object is created and used within the try block, and the Dispose() method is called in the finally block, guaranteeing resource cleanup regardless of whether the code within the try block completes successfully or throws an exception. For example:

using (Font font3 = new Font("Arial", 10.0f))
{
    // Use font3
}
// font3.Dispose() is automatically called here

# 4. Describe the concept of inheritance in C#.
Inheritance in C# is a mechanism where a new class (derived or child class) acquires properties and methods of an existing class (base or parent class). This promotes code reusability and establishes a hierarchical relationship between classes. The derived class can inherit members (fields, properties, methods, events, etc.) from the base class and can also add new members or override the inherited ones, thus extending or specializing the base class's behavior.

**Key aspects of inheritance include:**

Code Reusability: Avoids redundant code by inheriting from existing classes.
: Syntax: Used to specify inheritance (e.g., class Dog : Animal).
virtual and override: Allows derived classes to modify the behavior of inherited methods. The base class method needs to be declared virtual, and the derived class uses the override keyword.
base Keyword: Used to call the base class's constructor or method from the derived class.


# 5. What are interfaces in C# and how are they different from abstract classes?
Interfaces in C# define a contract that classes can implement. They contain only declarations of methods, properties, events, and indexers, but no implementation. A class can implement multiple interfaces.

Abstract classes, on the other hand, can contain both declarations and implementations. They can have fields, constructors, and methods with bodies. A class can inherit from only one abstract class. The key difference is that interfaces enforce a 'can-do' relationship, while abstract classes establish an 'is-a' relationship. Also, interfaces support multiple inheritance while abstract classes do not. Example:

interface IMyInterface { void MyMethod(); }
abstract class MyAbstractClass { public abstract void MyMethod(); }


# 6. What is polymorphism and how is it achieved in C#?
Polymorphism, meaning "many forms", is the ability of a class or interface to take on many forms. It allows objects of different classes to be treated as objects of a common type. In C#, polymorphism is achieved through several mechanisms:

Inheritance: Derived classes inherit from a base class and can override virtual methods. This is runtime polymorphism.
Interfaces: Classes can implement interfaces, providing different implementations for the same interface methods. This also facilitates runtime polymorphism.
Method Overloading: Creating multiple methods with the same name but different parameters within a class. This is compile-time polymorphism.
Abstract Classes: Abstract classes define abstract methods that must be implemented by derived classes, enforcing a certain structure and polymorphism.
For example:

public class Animal {
 public virtual string MakeSound() { return "Generic animal sound"; }
}

public class Dog : Animal {
 public override string MakeSound() { return "Woof!"; }
}

public class Cat : Animal {
 public override string MakeSound() { return "Meow!"; }
}

Animal myAnimal = new Dog();
Console.WriteLine(myAnimal.MakeSound()); // Output: Woof!


# 7. Explain the difference between '==' and '.Equals()' in C#.
In C#, == and .Equals() are used for comparing values, but they differ in their behavior, especially when dealing with reference types.

== : This is the equality operator. By default, for reference types, it checks for reference equality meaning it verifies if two variables point to the same memory location (i.e., are they the same object instance). For value types, == compares the values of the operands. However, the == operator can be overloaded by classes and structs to provide custom equality logic.
.Equals() : This is a method inherited from the System.Object class. By default, for reference types, its behavior is similar to == checking for reference equality. However, its main purpose is to be overridden in derived classes to provide value equality. Value equality means that it checks if the contents of two objects are the same based on defined criteria within the class. Value types typically override the .Equals() method to compare values and not memory addresses.


# 8. What is a delegate in C#?
In C#, a delegate is a type that represents references to methods with a particular parameter list and return type. Essentially, it's a type-safe function pointer. Delegates enable you to treat methods as objects, allowing them to be passed as arguments to other methods, stored in data structures, and invoked dynamically.

Think of it as a placeholder. You declare a delegate type, then you can create instances of that delegate that 'point' to specific methods. These methods must match the signature (parameter types and return type) defined by the delegate. Delegates are fundamental for implementing events and callback mechanisms in C#.


# 9. What are events in C# and how are they used?
In C#, events are a way for a class or object to notify other classes or objects when something of interest happens. They are based on the delegate type and provide a mechanism for loose coupling between objects. An event allows a class to expose notifications without exposing the underlying delegate directly.

Events are used through the following steps:

Define a delegate: This defines the signature of the event handler method.
Declare an event: Using the event keyword, based on the defined delegate.
Raise the event: When the event occurs, the class raises the event, which invokes the registered event handlers.
Subscribe/Unsubscribe to the event: Other classes or objects can subscribe to the event to be notified when it is raised using += and unsubscribe using -=.
For example:

public delegate void MyEventHandler(object sender, EventArgs e);

public class MyClass
{
 public event MyEventHandler MyEvent;

 protected virtual void OnMyEvent(EventArgs e)
 {
  MyEvent?.Invoke(this, e);
 }

 public void DoSomething()
 {
  // ... some code
  OnMyEvent(EventArgs.Empty); // Raise the event
 }
}


# 10. Describe the purpose of exception handling in C#.
Exception handling in C# is a mechanism to deal with runtime errors, ensuring that a program doesn't crash unexpectedly and can recover gracefully. It involves monitoring a block of code for potential errors, catching those errors when they occur, and then executing specific code to handle them, like logging the error or attempting a retry.

The core purpose of exception handling revolves around these key elements:

Robustness: Prevent application termination due to unexpected issues.
Error Reporting: Provide meaningful information about errors for debugging and logging.
Resource Management: Ensure resources are properly released, even in error scenarios (e.g., using finally blocks).
Code Clarity: Separate error-handling logic from normal program execution, improving readability.


# 11. What is LINQ and what are its benefits?
LINQ (Language Integrated Query) is a set of features introduced in .NET that provides a unified way to query data from various sources. These sources can include databases, XML documents, collections, and more. It allows you to use a consistent syntax to perform operations like filtering, sorting, grouping, and joining data, regardless of the underlying data source.

The benefits of LINQ are numerous:

Readability and Maintainability: LINQ queries are often more concise and easier to understand than traditional SQL or code-based data manipulation.
Type Safety: LINQ provides compile-time type checking, reducing runtime errors.
IntelliSense Support: Visual Studio provides IntelliSense support for LINQ queries, making it easier to write correct code.
Data Source Agnostic: The same LINQ syntax can be used to query different data sources.
Productivity: LINQ simplifies data access and manipulation, leading to increased developer productivity. Example: var result = from x in list where x > 5 select x;


# 12. What is an extension method and how do you create one?
An extension method allows you to add new methods to existing types without modifying the original type's definition. This is useful when you want to add functionality to a type you don't control (e.g., a class from a third-party library or a built-in type like string).

To create an extension method in C#, you define a static method in a static class. The first parameter of the method specifies the type that the method extends, and it's preceded by the this keyword. For example:

public static class StringExtensions
{
    public static string ToTitleCase(this string str)
    {
        // Implementation to convert string to title case
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    }
}

Now you can call ToTitleCase() as if it were an instance method of the string class:

string myString = "hello world";
string titleCaseString = myString.ToTitleCase(); // titleCaseString will be "Hello World"


# 13. Explain the difference between 'const' and 'readonly' keywords.
Both const and readonly are used to prevent modification of a variable after its initial assignment, but they operate at different levels.

const means the value of the variable is known at compile time and cannot be changed at runtime. It's a compile-time constant. On the other hand, readonly means the variable can only be assigned a value during the declaration or within the constructor of the class/struct it belongs to. Its value is not necessarily known at compile time and can be different for different instances of the class. Consider these examples:

public class Example {
 public readonly int runtimeValue;
 public const int compileTimeValue = 5;

 public Example(int value) {
 this.runtimeValue = value; // Allowed
 //compileTimeValue = 10; // Not allowed
 }
}


# 14. What are generics in C# and why are they useful?
Generics in C# allow you to define type-safe data structures and algorithms without committing to a specific data type. You use type parameters (e.g., T) as placeholders, which are then replaced with concrete types when the code is used. They avoid boxing/unboxing overhead and enable compile-time type checking, preventing runtime errors.

Generics are useful because they promote code reusability, type safety, and performance. Without generics, you might use object type, requiring casting and potentially introducing runtime errors. With generics, you can write code that works with different data types in a type-safe manner and improve code clarity. For example, List<int> is a list of integers, and List<string> is a list of strings, both using the same List<T> definition. This eliminates redundant code.


# 15. Describe the purpose of attributes in C#.
Attributes in C# provide a way to add declarative information to code elements (assemblies, modules, types, members, parameters, etc.). They are used to store metadata about these elements, which can then be accessed at runtime using reflection. This metadata can be used for various purposes such as:

Serialization: Specifying how objects should be serialized. For example, the [Serializable] attribute marks a class as serializable.
Code analysis: Providing information for static analysis tools.
Compiler directives: Influencing compiler behavior. For example, [Obsolete] attribute marks code as deprecated.
Runtime behavior: Modifying the behavior of code at runtime. For example, Attributes are used extensively in ASP.NET Core for routing and validation.
Attributes enhance code readability and maintainability by separating metadata from the core logic. They reduce the need for complex configuration files or code-based metadata management.


# 16. What is boxing and unboxing in C#?
Boxing is the process of converting a value type (like int, bool, struct) to a reference type (object). It implicitly creates an object on the heap and copies the value type's data into it. Unboxing is the reverse process; it explicitly extracts the value type from the object.

For example:

int i = 123;
object box = i;  // Boxing
int j = (int)box; // Unboxing
Boxing involves memory allocation and copying, so excessive boxing/unboxing can negatively impact performance. Unboxing also requires an explicit cast, and a InvalidCastException is thrown if the object doesn't contain the correct value type.


# 17. Explain the difference between 'as' and 'is' operators.
The is and as operators in C# (and similar languages) serve different purposes related to type checking and type conversion.

is: This operator checks if an object is compatible with a given type. It returns a boolean value (true if the object is of that type or can be implicitly converted to that type, and false otherwise). It does not perform any type conversion. For example: if (obj is string) { // do something }
as: This operator attempts to convert an object to a specified type. If the conversion is successful, it returns the object as that type. If the conversion is not possible (e.g., the object is not of that type or cannot be converted), it returns null. For example: string str = obj as string; if (str != null) { // use str }


# 18. What is the purpose of the 'sealed' keyword?
The sealed keyword in languages like C# and Java (less directly) is used to restrict inheritance. When applied to a class, it prevents other classes from inheriting from it, ensuring that the class's implementation remains final and cannot be modified or extended through inheritance.

This is useful for:

Security: Preventing malicious derived classes from altering behavior.
Performance: The compiler can optimize sealed classes better because it knows the exact type at compile time.
Version Control: Changes to a sealed class won't unintentionally break derived classes in other assemblies.
Design Control: Preventing unexpected extension.


# 19. Describe the difference between stack and heap memory.
Stack memory is used for static memory allocation, such as local variables and function call data. It's managed automatically by the system using a LIFO (Last-In, First-Out) structure. It's faster to access but has a limited size.

Heap memory, on the other hand, is used for dynamic memory allocation. Memory is allocated and deallocated explicitly by the programmer using functions like malloc()/free() in C or new/delete in C++. Heap access is slower but it provides more flexibility and a larger memory pool. Memory leaks can occur if heap memory isn't properly deallocated.


# 20. What are nullable types in C# and why are they useful?
Nullable types in C# allow you to assign null to value types (like int, bool, DateTime, etc.). Value types are, by default, non-nullable, meaning they must always have a value. Nullable types are declared using the ? symbol after the type, e.g., int?, bool?.

They're useful when you need to represent the absence of a value for a value type. For instance:

When dealing with database fields that can be NULL.
Representing optional data in data transfer objects (DTOs).
Indicating that a value hasn't been initialized or is unknown.
int? nullableInt = null; 
if (nullableInt.HasValue)
{
 int value = nullableInt.Value; 
 Console.WriteLine(value);
}
else
{
 Console.WriteLine("nullableInt is null");
}


# 21. Explain how garbage collection works in C#.
Garbage Collection (GC) in C# is an automatic memory management feature. The .NET CLR (Common Language Runtime) automatically reclaims memory that is no longer in use by the application, preventing memory leaks. The GC operates on a generational system.

When an object is created, it's placed in Generation 0. The GC checks memory periodically. If an object in Generation 0 is still in use, it's promoted to Generation 1. Less frequently, Generation 1 objects are checked; survivors move to Generation 2. Objects in Generation 2 have survived multiple garbage collection cycles. This approach optimizes performance because frequently used objects are checked less often. When memory is low, the GC runs, freeing up memory by collecting objects that are no longer referenced. The Dispose() method (and using statement) allows deterministic cleanup for objects holding unmanaged resources, complementing the GC's automatic management of managed memory.


# 22. What is asynchronous programming in C# and when should you use it?
Asynchronous programming in C# allows your program to initiate a long-running operation without blocking the main thread. This means the UI remains responsive, and the application doesn't freeze while waiting for tasks like network requests, file I/O, or database queries to complete. C# achieves this primarily using the async and await keywords.

You should use asynchronous programming when dealing with operations that may take a significant amount of time, particularly when these operations could potentially block the UI thread. Common scenarios include:

UI applications: To keep the UI responsive during long operations.
Web applications: To handle multiple requests concurrently without blocking threads.
I/O-bound operations: Tasks that involve reading from or writing to files, network streams, or databases.
CPU-bound operations: Although less common, can improve responsiveness if handled on a separate thread (consider Task.Run with async and await).


# 23. Describe the purpose of the 'virtual' and 'override' keywords.
The virtual keyword allows a method in a base class to be overridden in a derived class. It declares that derived classes can provide their own specific implementation of the method. Without virtual, the base class implementation is always used.

The override keyword is used in a derived class to provide a new implementation for a virtual method inherited from a base class. It explicitly states that the derived class method is replacing the base class method's functionality. Using override enforces compile-time checking to ensure that the method signature matches the base class's virtual method. It also signals intent, making the code clearer and easier to maintain.


# 24. What is the difference between a struct and a class?
The primary difference between a struct and a class lies in their default access modifiers. In C++, structs have members that are public by default, while class members are private by default. This means that if you don't explicitly specify an access modifier (public, private, protected) for a member in a struct, it will be accessible from anywhere. Conversely, in a class, members without an explicit access modifier are only accessible from within the class itself or its friends.

In practice, structs are often used to represent simple data structures where the members are intended to be directly accessed, while classes are used for more complex objects with encapsulated data and methods. However, functionally, both structs and classes can have methods, constructors, destructors, and inheritance, making them very similar in capability. The choice between using a struct and a class often depends on the intended use and the desired level of encapsulation. For example:

struct Point {
 int x; // public by default
 int y;
};

class Rectangle {
 int width; // private by default
 int height;
public:
 void setWidth(int w) { width = w; }
 void setHeight(int h) { height = h; }
};


# 25. Explain the concept of lambda expressions in C#.
Lambda expressions in C# are anonymous functions that can be treated as values. They provide a concise way to create function delegates or expression tree types.

They're typically used for short, inline function definitions, especially in LINQ queries and event handlers. The basic syntax is (input-parameters) => expression. For example, x => x * x is a lambda expression that takes an input x and returns its square. Multiple parameters are comma separated, (x, y) => x + y. Lambda expressions can also have statement bodies enclosed in curly braces, (x) => { return x * 2; }.


# 26. What are collection initializers in C#?
Collection initializers provide a simplified syntax to initialize collections (like lists, dictionaries, etc.) when they are created. Instead of adding elements one by one using the Add() method, you can specify the initial elements directly within curly braces {}.

For example, consider a list of integers: List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };. Or a dictionary: Dictionary<string, int> ages = new Dictionary<string, int> { { "Alice", 30 }, { "Bob", 25 } };. This makes the code more concise and readable.

# **Intermediate C# interview questions** #

# 1. Explain the difference between `Func<T, TResult>` and `Action<T>` in C#.
Func<T, TResult> and Action<T> are both delegates in C#, but they serve different purposes. Func<T, TResult> represents a method that takes one or more input parameters of type T and returns a value of type TResult. The last generic type parameter always denotes the return type. For example, Func<int, string> represents a function that takes an integer and returns a string.

Action<T> on the other hand, represents a method that takes one or more input parameters of type T but does not return a value (i.e., its return type is void). For example, Action<string> represents a method that takes a string as input and performs some action, but doesn't return anything. In essence, Func is for functions that compute and return a result, while Action is for procedures that perform an operation without returning a value.


# 2. What are extension methods, and how can they be used? Provide an example.
Extension methods allow you to add new methods to existing types without modifying the original type itself or creating a new derived type. They are special static methods defined in a static class, where the first parameter specifies the type that the method extends, preceded by the this keyword. This enables you to call the extension method as if it were a member of the extended type.

For example, to add a method that counts words in a string:

public static class StringExtensions
{
 public static int WordCount(this string str)
 {
 return str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
 }
}

//Usage
string text = "Hello world!";
int count = text.WordCount(); // count will be 2


# 3. How does the `yield` keyword work in C#, and what problem does it solve?
The yield keyword in C# allows you to create an iterator method that returns a sequence of values one at a time, without having to create a temporary collection to hold the entire result. When the yield return statement is executed, the current value is returned to the caller, and the state of the method is saved. Execution resumes from that point the next time the iterator is called. yield break can be used to terminate the iteration.

The problem yield solves is primarily related to memory efficiency and deferred execution. Instead of generating a complete list in memory before returning it, yield allows you to produce values only when they are requested. This is particularly useful when dealing with large datasets or infinite sequences as you only process elements as needed, thus improving performance and reducing memory footprint.


# 4. What is the purpose of the `async` and `await` keywords in C#, and how do they work together?
The async and await keywords in C# are used to write asynchronous code more cleanly and readably. async marks a method as asynchronous, allowing the use of await within it. The await keyword pauses the execution of the async method until the awaited task completes, without blocking the calling thread. Execution resumes at the point after the await once the task finishes.

They work together to make asynchronous operations appear more like synchronous code. The compiler transforms the async method into a state machine, handling the callbacks and continuations behind the scenes. This avoids the complex and error-prone callback-based approach to asynchronous programming. The async method must return Task, Task<T>, or void. Using async and await significantly improves the responsiveness of applications, especially those involving I/O-bound operations.


# 5. Describe the difference between `Task.Run()` and `Task.Factory.StartNew()`.
Task.Run() is a simplified wrapper around Task.Factory.StartNew(). Task.Run() is designed for the most common scenario: queuing work to the thread pool. It automatically infers the appropriate task creation options. Underneath, Task.Run(action) is equivalent to Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).

Task.Factory.StartNew() offers more control and flexibility. It allows you to specify parameters such as CancellationToken, TaskCreationOptions, and a custom TaskScheduler. This is useful for advanced scenarios, such as specifying the task's behavior (e.g., LongRunning, AttachedToParent) or running the task on a specific scheduler other than the default thread pool scheduler. Task.Run() is generally preferred for simple fire-and-forget tasks due to its simplicity, while Task.Factory.StartNew() is used when more fine-grained control is required.


# 6. What is LINQ, and how does it improve code readability and maintainability? Give a basic example.
LINQ (Language Integrated Query) is a set of features in .NET that provides a unified way to query data from various data sources. These sources can include collections (like lists and arrays), databases, XML documents, and more. LINQ allows you to use a consistent syntax to filter, sort, group, and project data, regardless of the underlying data source. This leads to more readable and maintainable code because the same query patterns can be applied across different types of data. It simplifies data access by embedding queries directly into C# or VB.NET code, eliminating the need for separate query languages or APIs.

For example, consider a list of integers. Without LINQ, filtering for even numbers might involve a loop and conditional statements. With LINQ, it's much cleaner:

List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
//Using LINQ
IEnumerable<int> evenNumbers = numbers.Where(n => n % 2 == 0);
//Without LINQ
List<int> evenNumbersList = new List<int>();
foreach (int number in numbers)
{
 if (number % 2 == 0)
 {
 evenNumbersList.Add(number);
 }
}
LINQ improves readability because the intent of the query (filtering for even numbers) is immediately clear. It improves maintainability by reducing the amount of boilerplate code required for data manipulation and using common operations, making code easier to understand and modify.


# 7. Explain the concept of deferred execution in LINQ.
Deferred execution in LINQ means that a query is not executed at the point where it is defined. Instead, the execution is postponed until the query's results are actually needed. This typically happens when you iterate over the results using a foreach loop, convert the results to a list (e.g., using .ToList()), or call a method that requires immediate evaluation (e.g., .Count(), .First(), .Single()).

The main benefit of deferred execution is performance optimization. LINQ providers can optimize the query execution based on how the results are ultimately used. If you only need the first few elements of a large dataset, the entire dataset doesn't need to be processed immediately. Also, it allows you to modify the underlying data source after defining the query but before it is executed, and the query will reflect those changes.


# 8. Describe the differences between `IEnumerable` and `IQueryable`.
IEnumerable and IQueryable are both interfaces in .NET used for querying data, but they differ significantly in how the data is retrieved and processed. IEnumerable represents a sequence of objects that can be iterated over. When using IEnumerable, the entire data source is loaded into memory, and filtering, sorting, and other operations are performed on the client-side. This means the operations happen after the data is retrieved.

IQueryable, on the other hand, represents a query that can be executed against a data source. It allows you to build up a query expression that is then translated and executed on the data source (e.g., a database). This means filtering, sorting, and other operations are performed on the server-side before the data is retrieved, leading to potentially significant performance gains, especially when dealing with large datasets. The key is that IQueryable uses expression trees to represent the query, which can be analyzed and optimized by the data source.

Key Differences:

Execution: IEnumerable executes queries in memory (client-side), while IQueryable executes queries on the data source (server-side).
Data Loading: IEnumerable loads the entire dataset, while IQueryable only retrieves the data that matches the query.
Performance: IQueryable is generally more efficient for large datasets due to server-side processing.
Namespace: IEnumerable resides in System.Collections, IQueryable resides in System.Linq.
IQueryable inherits from IEnumerable.
Example:

// IEnumerable (client-side filtering)
IEnumerable<Product> productsIEnumerable = db.Products.ToList().Where(p => p.Price > 10);

// IQueryable (server-side filtering)
IQueryable<Product> productsIQueryable = db.Products.Where(p => p.Price > 10);


# 9. What are the benefits of using interfaces in C#? Explain with an example.
Interfaces in C# offer several benefits, primarily around abstraction and loose coupling. They define a contract that classes can implement, ensuring they provide specific functionality. This allows for polymorphism, where different classes can be treated uniformly through the interface.

Consider this example:

interface ISpeak
{
    void Speak();
}

class Dog : ISpeak
{
    public void Speak() { Console.WriteLine("Woof!"); }
}

class Cat : ISpeak
{
    public void Speak() { Console.WriteLine("Meow!"); }
}

//Use
ISpeak animal1 = new Dog();
ISpeak animal2 = new Cat();

animal1.Speak(); // Output: Woof!
animal2.Speak(); // Output: Meow!
Benefits:

Abstraction: Hide complex implementation details.
Loose Coupling: Classes implementing the interface are independent.
Polymorphism: Treat different classes uniformly.
Multiple Inheritance: A class can implement multiple interfaces, overcoming C#'s lack of multiple class inheritance


# 10. Explain the difference between `struct` and `class` in C#.
In C#, the primary difference between struct and class lies in their nature: struct is a value type, while class is a reference type. This means that when you assign a struct to a new variable or pass it as an argument, a copy of the struct is created. With class, only a reference to the object is copied. Another significant difference is that structs are implicitly sealed, meaning they cannot be inherited from, whereas classes can be inherited.

Furthermore, structs have an implicit parameterless constructor that initializes fields to their default values, and you cannot define your own parameterless constructor. Classes, on the other hand, don't have this limitation. Also, structs are typically used for small data structures, while classes are used for more complex objects with behavior. Struct variables are allocated on the stack, while class objects are allocated on the heap.


# 11. When would you choose to use a `struct` over a `class` in C#?
In C#, use a struct over a class when you need a lightweight data structure that represents a single value, and value-type semantics are desired. struct are value types, so they're copied on assignment, which is beneficial when you want independent copies of your data and avoid unintended side effects. Choose struct for small, immutable types like Point, Rectangle, or simple data containers.

Conversely, prefer class when dealing with more complex objects that have significant behavior, require inheritance, or benefit from reference-type semantics (sharing the same instance in memory). Classes are reference types and allocated on the heap, thus suited for larger objects or situations where object identity matters.


# 12. What is boxing and unboxing in C#, and what are the performance implications?
Boxing is the process of converting a value type (like int, bool, struct) to a reference type (object). Unboxing is the reverse process, converting a reference type (that was previously boxed) back to its original value type.

The primary performance implication is due to the memory allocation and type checking involved. Boxing requires allocating memory on the heap to store the value type, and unboxing requires type checking to ensure the object being unboxed is compatible with the target value type. These operations can be relatively slow compared to direct operations on value types, especially if they occur frequently in performance-critical code. This usually leads to increased garbage collection overhead.


# 13. How does the garbage collector work in C#? What is the difference between generations?
The C# garbage collector (GC) automatically manages memory by reclaiming objects that are no longer in use. It works by periodically inspecting the heap, identifying objects that are unreachable from application roots (static variables, local variables on the stack, CPU registers), and freeing the memory they occupy. The GC uses a mark-and-sweep algorithm, enhanced with generations to optimize performance. The GC makes assumptions that recently created objects are likely to be short-lived and older objects are less likely to be garbage.

Generations are a key part of the GC's optimization strategy. Objects are grouped into generations based on their age: Generation 0 (youngest), Generation 1, and Generation 2 (oldest). When the GC runs, it first tries to collect Generation 0. If that doesn't free enough memory, it collects Generation 1 and then Generation 2 if necessary. This approach avoids collecting the entire heap every time, significantly improving performance because collecting younger generations is faster, the assumption being the younger generation has the most dead objects compared to the number of live objects.


# 14. Explain the purpose of the `using` statement in C# and how it relates to the `IDisposable` interface.
The using statement in C# provides a convenient way to ensure that an object implementing the IDisposable interface is properly disposed of, even if exceptions occur. It essentially wraps the object's usage in a try...finally block, where the Dispose() method is called in the finally block.

When an object implements IDisposable, it typically holds unmanaged resources like file handles, network connections, or database connections. The Dispose() method releases these resources. The using statement guarantees that Dispose() is called when the block is exited, preventing resource leaks. For example:

using (FileStream fs = new FileStream("file.txt", FileMode.Open))
{
  // Use the file stream
}
// fs.Dispose() is automatically called here


# 15. What are delegates in C#, and how are they used to implement event handling?
Delegates in C# are type-safe function pointers. They allow you to treat methods as objects, which can be passed as arguments to other methods, stored in data structures, and invoked dynamically. They essentially define a type that represents a reference to a method with a specific signature (return type and parameter types).

Delegates are crucial for implementing event handling in C#. Events are a mechanism for a class to notify other classes (or objects) when something of interest happens. This is achieved using delegates. An event is essentially a wrapper around a delegate. When an event occurs, the delegate's invocation list (a list of methods that should be called when the event is raised) is executed. Example:

public delegate void MyEventHandler(object sender, EventArgs e);
public event MyEventHandler MyEvent;


# 16. Explain the difference between delegates and events in C#.
Delegates are type-safe function pointers, allowing you to treat methods as objects. They define a signature and can hold references to methods that match that signature. You can directly invoke a delegate.

Events, on the other hand, are a mechanism built on top of delegates that provide encapsulation. They prevent direct invocation from outside the class or struct that defines them. Events use delegates internally, but they add restrictions that ensure only the class that declares the event can raise (invoke) it. Subscribers can only add or remove event handlers. Events enforce a publish-subscribe patter


# 17. Describe how you would implement a custom exception in C#.
To implement a custom exception in C#, you create a new class that inherits from the System.Exception class or one of its derived classes (like System.ApplicationException or System.SystemException). It's best practice to provide constructors similar to those found in System.Exception: a default constructor, a constructor that accepts a message string, and a constructor that accepts a message string and an inner exception.

Here's a basic example:

public class MyCustomException : Exception
{
    public MyCustomException() { }
    public MyCustomException(string message) : base(message) { }
    public MyCustomException(string message, Exception inner) : base(message, inner) { }
}
You can then throw this exception like any other exception:

throw new MyCustomException("Something went wrong!");
Consider adding custom properties or methods to hold or handle exception specific data, like an error code, if required.


# 18. What are attributes in C#, and how can you create and use them?
Attributes in C# are metadata that provide information about types (classes, structs, enums, interfaces, delegates), methods, fields, properties, events, parameters, and other code elements. They are used to add declarative information to your code which can then be read and acted upon at runtime or compile time using reflection. They provide a powerful way to add behavior or modify the compilation process without altering the core logic of your code.

To create a custom attribute, you define a class that inherits from System.Attribute. You then apply the attribute to code elements using square brackets [] followed by the attribute's name and any constructor parameters. Here is an example:

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MyCustomAttribute : Attribute
{
    public string Description { get; set; }

    public MyCustomAttribute(string description)
    {
        Description = description;
    }
}

[MyCustomAttribute("This is a class description")]
public class MyClass
{
    [MyCustomAttribute("This is a method description")]
    public void MyMethod() { }
}
To access attributes at runtime use reflection:

Type type = typeof(MyClass);
MyCustomAttribute attribute = (MyCustomAttribute)Attribute.GetCustomAttribute(type, typeof(MyCustomAttribute));
if (attribute != null)
{
    Console.WriteLine(attribute.Description);
}


# 19. Explain what reflection is in C#, and provide a scenario where it might be useful.
Reflection in C# allows you to inspect and manipulate types (classes, interfaces, structs, enums, delegates, and events) at runtime. Instead of knowing the type at compile time, you can discover its properties, methods, fields, events, and other members during program execution. Essentially, it's a way for code to examine and modify its own structure and behavior.

A useful scenario is creating a generic object serializer/deserializer. You could use reflection to iterate through the properties of an arbitrary object, extract their values, and then use that information to serialize the object into a format like JSON or XML. Conversely, during deserialization, you could use reflection to create an instance of the object and populate its properties with data extracted from the serialized format. This avoids writing specific serialization/deserialization code for each class.


# 20. What are generics in C#, and how do they improve type safety and performance?
Generics in C# allow you to define type-safe data structures and algorithms without committing to specific data types. You can define classes, interfaces, methods, delegates, and events that are parameterized by type. The type parameter is specified when the generic type is instantiated or the generic method is called.

Generics improve type safety by ensuring that the compiler enforces type constraints at compile time. This avoids the need for runtime type checking and casting, which can lead to InvalidCastException errors. Generics also improve performance by avoiding boxing and unboxing operations when working with value types. This is because the compiler can generate specialized code for each type used with the generic type or method, resulting in more efficient execution. The below code snippet is an example of a generic list

List<int> numbers = new List<int>();
numbers.Add(1);
numbers.Add(2);

List<string> names = new List<string>();
names.Add("Alice");
names.Add("Bob");


# **Advanced C# interview questions** # 

# 1. Explain the differences between `async` and `await` and how they contribute to building responsive applications.
async and await are syntactic sugar built on top of Promises in JavaScript, designed to make asynchronous code easier to read and write. async declares a function as asynchronous, enabling the use of await within it. await pauses the execution of the async function until the Promise it 'awaits' resolves.

By pausing execution instead of blocking the main thread, async/await allows the event loop to continue processing other tasks, like handling user interactions or rendering updates. This non-blocking behavior is crucial for building responsive applications, preventing the UI from freezing during long-running operations like network requests or complex calculations. Instead of .then() and .catch() for handling promises, we can write code in a more synchronous style. For example:

async function fetchData() {
  try {
    const response = await fetch('https://example.com/data');
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error fetching data:', error);
  }
}


# 2. How does the .NET garbage collector work, and what are the different generations of garbage collection?
The .NET garbage collector (GC) automatically manages memory allocation and deallocation for applications. It reclaims memory occupied by objects that are no longer in use. The GC operates based on the principle of generational garbage collection to improve performance. When the GC runs, it determines which objects are no longer being used by the application. It then reclaims the memory occupied by those objects.

.NET GC uses generations to categorize objects based on their age:

Generation 0: Short-lived objects, such as temporary variables. This generation is collected frequently.
Generation 1: Objects that have survived a Generation 0 collection. Serves as a buffer between short-lived and long-lived objects.
Generation 2: Long-lived objects that have survived multiple garbage collection cycles. This generation is collected the least frequently because it contains objects that are likely to remain in memory for the duration of the application's lifetime. A full garbage collection involves all generations (0, 1, and 2).


# 3. Describe the purpose of the `IDisposable` interface and the `using` statement in C#, and explain how they help manage resources.
The IDisposable interface in C# provides a mechanism to release unmanaged resources (e.g., file handles, network connections, database connections) held by an object. It defines a single method, Dispose(), which should contain the logic to clean up these resources. Classes that implement IDisposable signal that they hold resources that need to be explicitly released.

The using statement simplifies the process of calling Dispose() by ensuring that it is always called, even if exceptions occur within the using block. When an object is created within a using statement, the Dispose() method is automatically called when the block is exited. This promotes deterministic resource management, preventing resource leaks and improving application stability. The syntax of a using statement is using (ResourceType resource = new ResourceType()) { ... }


# 4. What are delegates and events in C#, and how do they facilitate communication between objects?
Delegates are type-safe function pointers. They hold the reference to a method, allowing you to pass methods as arguments to other methods. Events, on the other hand, are a mechanism for a class (publisher) to notify other classes (subscribers) when something of interest happens. They're based on delegates.

Delegates and events facilitate communication between objects by allowing loose coupling. The publisher doesn't need to know the specific subscribers; it only needs to raise the event. Subscribers register their interest in the event via delegate handlers. When the event is raised, the delegate's invocation list is called, triggering the associated methods in the subscribing objects. This publish-subscribe pattern enables a flexible and extensible way for objects to interact without tight dependencies.


# 5. Explain the concept of LINQ (Language Integrated Query) and how it simplifies data querying in C#.
LINQ (Language Integrated Query) is a powerful feature in C# that provides a unified way to query data from various sources, such as databases, XML, collections, and more, directly within the C# code. It simplifies data querying by using a consistent syntax, regardless of the data source. Instead of writing different code for each data source, LINQ allows developers to use a standard set of query operators (e.g., Where, Select, OrderBy) and syntax to filter, project, and sort data.

LINQ offers several advantages, including improved code readability and maintainability, reduced code complexity, and compile-time type checking. LINQ queries can be written using either query syntax (similar to SQL) or method syntax (using lambda expressions). For instance:

// Query Syntax
var results = from item in collection where item.Property > 10 select item.Name;

// Method Syntax
var results = collection.Where(item => item.Property > 10).Select(item => item.Name);


# 6. What are extension methods, and how can they be used to add functionality to existing classes without modifying their source code?
Extension methods allow you to add new methods to existing types (classes, structs, interfaces) without modifying the original type's source code. They are a special kind of static method that can be called as if they were instance methods of the extended type. To define an extension method in C#, you need to:

Declare a static class.

Define a static method within that class.

Use the this keyword as the first parameter of the method, followed by the type you want to extend. For example:

public static class StringExtensions
{
    public static bool IsValidEmail(this string str)
    {
        // Email validation logic here
        return str.Contains("@") && str.Contains(".");
    }
}
After defining the extension method, you can call it on any instance of the extended type as if it were a regular instance method: string email = "test@example.com"; bool isValid = email.IsValidEmail();


# 7. Describe the differences between value types and reference types in C#, and how they affect memory management.
Value types (e.g., int, bool, struct, enum) store their data directly within their memory allocation. When you assign a value type to a new variable, a copy of the data is created. Each variable then has its own independent copy. Reference types (e.g., string, class, array, delegate) store a reference (a memory address) to the actual data stored elsewhere on the heap. When you assign a reference type to a new variable, only the reference is copied, not the underlying data. Both variables then point to the same memory location.

This difference significantly impacts memory management. Value types are typically allocated on the stack, providing fast allocation and deallocation, and are automatically deallocated when they go out of scope. Reference types are allocated on the heap, which requires more overhead for allocation and deallocation. Garbage collection is responsible for reclaiming memory occupied by reference types that are no longer in use, which introduces a performance cost.


# 8. What is reflection in C#, and how can it be used to inspect and manipulate types at runtime?
Reflection in C# allows you to inspect and manipulate types (classes, interfaces, structures, etc.) at runtime. This means you can discover information about types, create instances of them, invoke their methods, and access their properties, all without knowing their names or structures at compile time. It is primarily achieved through classes found in the System.Reflection namespace.

Reflection can be used for several purposes:

Inspecting Types: Discovering information like methods, properties, fields, and attributes of a type using methods like GetType(), GetMethods(), GetProperties(), etc.
Creating Instances: Dynamically creating instances of types using Activator.CreateInstance().
Invoking Methods: Calling methods of a type dynamically using MethodInfo.Invoke().
Accessing Properties and Fields: Getting and setting values of properties and fields using PropertyInfo.GetValue(), PropertyInfo.SetValue(), FieldInfo.GetValue(), and FieldInfo.SetValue().
For example, the following code snippet demonstrates the use of reflection:

Type myType = Type.GetType("MyNamespace.MyClass");
object instance = Activator.CreateInstance(myType);
MethodInfo method = myType.GetMethod("MyMethod");
method.Invoke(instance, null);


# 9. Explain the purpose of attributes in C# and how they can be used to add metadata to code elements.
Attributes in C# are used to add declarative metadata to code elements (assemblies, modules, classes, methods, properties, etc.). They provide a way to associate information with code that can be accessed at runtime using reflection. This metadata can be used for various purposes, such as serialization, validation, code generation, or documentation.

Attributes are defined as classes that inherit from System.Attribute. You apply them to code elements by placing them within square brackets [] before the element. For example, [Obsolete("Use NewMethod instead")] public void OldMethod() { ... } marks the OldMethod as obsolete, with the provided message. Reflection can then be used to read this Obsolete attribute and take appropriate action.


# 10. What are generics in C#, and how do they enable type-safe programming with reusable code?
Generics in C# allow you to define type-safe data structures and algorithms without committing to a specific data type. They use type parameters (e.g., List<T>) which are placeholders that are replaced with an actual type when the generic type is used. This eliminates the need for casting and reduces the risk of runtime type errors.

Generics enable type-safe programming because the compiler enforces type constraints at compile time. Reusable code is achieved because you can write a single generic class or method that works with multiple data types without code duplication. For instance, a generic list class can store integers, strings, or any other type safely and efficiently, avoiding the type casting associated with object-based collections. For example:

public class GenericList<T>
{
    private T[] items;
    //...
    public T GetItem(int index) { return items[index]; }
}


# 11. How does the `yield` keyword work in C#, and how can it be used to create iterators?
The yield keyword in C# is used to create iterators in a stateful manner. Instead of returning a collection all at once, a method using yield returns an IEnumerable or IEnumerator that yields elements one at a time as they are requested. When yield return is encountered, the current element is returned, and the method's state is preserved. Execution resumes from that point when the next element is requested.

Using yield, custom iterators can be created easily. For example:

public IEnumerable<int> GetNumbers(int max)
{
    for (int i = 0; i < max; i++)
    {
        yield return i;
    }
}
This avoids needing to create a separate class that implements IEnumerable and IEnumerator manually. yield break can be used to indicate the end of the iteration.


# 12. Describe the concept of covariance and contravariance in C# generics, and provide examples of their usage.
Covariance and contravariance describe how type parameters in generic interfaces and delegates can be converted implicitly. Covariance allows you to use a more derived type than originally specified (e.g., IEnumerable<string> can be used where IEnumerable<object> is expected if string inherits from object). Contravariance allows you to use a less derived type than originally specified (e.g., an action that accepts object can be used where an action accepting string is expected).

Covariance is supported for generic type parameters declared as out (output), and contravariance is supported for type parameters declared as in (input). For example:

Covariance: interface ICovariant<out T> {} ICovariant<string> cov = new Covariant<string>(); ICovariant<object> objCov = cov;
Contravariance: interface IContravariant<in T> {} IContravariant<object> contra = new Contravariant<object>(); IContravariant<string> strContra = contra;
Delegates like Action<T> and Func<T> also support variance based on their parameter types. Action<object> objAction = (o) => {}; Action<string> stringAction = objAction; (contravariance). Func<string> stringFunc = () => "test"; Func<object> objectFunc = stringFunc; (covariance).

simple answer : Covariance (out) is for output and allows derived-to-base conversion. Contravariance (in) is for input and allows base-to-derived conversion.


# 13. What are tuples in C#, and how do they provide a way to group multiple values into a single object?
Tuples in C# are a way to group multiple values of potentially different types into a single, lightweight data structure. Before C# 7.0, this was often accomplished with System.Tuple classes, but these were cumbersome due to item naming (Item1, Item2, etc.). C# 7.0 introduced value tuples, which are more efficient and offer named fields.

Value tuples are structures, so they are value types (stored on the stack). They provide a concise syntax for creating and working with composite data. For example, (int age, string name) person = (30, "Alice"); demonstrates a tuple with named fields. You can then access these fields using person.age and person.name. Value tuples enhance code readability and reduce the need to define custom classes or structs for simple data groupings. They are easily created using parenthesis and can be returned as a single return value from a function.
Example:

(int, string) GetPerson() {
 return (30, "Bob");
}


# 14. Explain the purpose of the `dynamic` keyword in C#, and how it enables late-bound programming.
The dynamic keyword in C# allows you to bypass static type checking at compile time. Variables declared as dynamic have their type determined at runtime. This enables late-bound programming, where method calls and property access are resolved during execution instead of during compilation.

Using dynamic is useful when working with objects whose structure isn't known at compile time, such as when interacting with COM objects, dynamic languages like Python or Ruby, or when using reflection. However, it's crucial to note that errors caused by incorrect usage of dynamic are only detected at runtime, potentially leading to unexpected behavior. Here's an example:

dynamic myVariable = GetUnknownObject();
myVariable.SomeMethod(); // This will compile, but might throw an exception at runtime if SomeMethod doesn't exist


# 15. What are lambda expressions in C#, and how can they be used to create anonymous functions?
Lambda expressions in C# are a concise way to create anonymous functions. They are essentially unnamed methods that can be treated as data. The syntax is (input parameters) => expression or statement block. For example, x => x * x is a lambda expression that takes an input x and returns its square.

They are used to create delegates or expression tree types. You can assign a lambda expression to a delegate type, such as Func<int, int> or Action<string>. They're very useful in LINQ queries, event handlers, and any scenario where a short, inline function is needed. Example numbers.Where(n => n % 2 == 0) filters even numbers from a list called numbers.


# 16. Describe the different types of collections available in C#, such as lists, dictionaries, and sets, and explain their use cases.
C# offers various collection types, each suited for different scenarios. Lists (List<T>) are ordered collections that allow duplicate elements and provide access by index. They are ideal when you need to maintain the order of elements and frequently access elements by their position. Dictionaries (Dictionary<TKey, TValue>) store key-value pairs, providing fast lookups based on the key. Use them when you need to quickly retrieve values based on a unique identifier.

Sets (HashSet<T>) are unordered collections that store unique elements. They are useful for checking membership efficiently or removing duplicate entries. Other collections include Queue<T> (FIFO), Stack<T> (LIFO), and specialized collections like ObservableCollection<T> (for data binding) and BlockingCollection<T> (for thread-safe operations). Choosing the right collection depends on the specific requirements of your application, considering factors like ordering, uniqueness, and performance requirements.


# 17. What is dependency injection (DI) in C#, and how can it be used to improve the testability and maintainability of code?
Dependency Injection (DI) in C# is a design pattern where a class receives its dependencies from external sources rather than creating them itself. This promotes loose coupling between components. Instead of a class creating its dependencies directly (new keyword), these dependencies are "injected" into the class, typically through its constructor, properties, or methods.

DI improves testability because dependencies can be easily replaced with mock objects or stubs during unit testing. This allows you to isolate the code being tested and verify its behavior without relying on external systems. Maintainability is enhanced because changes to one dependency are less likely to affect other parts of the application, due to the loose coupling. DI also promotes code reuse and makes it easier to refactor code.


# 18. Explain the concept of the `Task` Parallel Library (TPL) in C#, and how it simplifies parallel programming.
The Task Parallel Library (TPL) in C# is a set of classes and APIs in the System.Threading.Tasks namespace that simplifies adding parallelism and concurrency to applications. It abstracts away many of the complexities of working directly with threads, allowing developers to focus on the work that needs to be done rather than the mechanics of thread management. The TPL automatically handles partitioning the work, scheduling threads, managing the thread pool, and handling cancellation, all in an efficient and scalable way. The main component of TPL is the Task object which represents an asynchronous operation.

TPL simplifies parallel programming by providing a higher-level abstraction over threads. Key benefits include:

Simplified code: Developers can express parallel operations more concisely using Task.Run, Parallel.For, Parallel.ForEach, and async/await keywords.
Automatic thread management: The TPL handles thread pool management, reducing the overhead of creating and destroying threads manually.
Exception handling: Provides a centralized way to handle exceptions thrown by tasks.
Cancellation support: Built-in support for cancelling tasks.
Data parallelism: Simplifies parallel processing of data collections using Parallel.For and Parallel.ForEach.

# 19. What are asynchronous streams in C#, and how do they enable the processing of data streams asynchronously?
Asynchronous streams in C# (introduced with IAsyncEnumerable<T> and IAsyncEnumerator<T>) enable the processing of data streams asynchronously. This allows you to iterate through a sequence of data where each element might take some time to produce, without blocking the calling thread. This is particularly useful when dealing with I/O-bound operations like reading from a database, network stream, or file. They help make better use of resources and reduce wait times, particularly for large datasets.

Instead of using a standard foreach loop on an IEnumerable<T>, you use await foreach on an IAsyncEnumerable<T>. Here's a basic example:

async IAsyncEnumerable<int> GenerateNumbers()
{
    for (int i = 0; i < 10; i++)
    {
        await Task.Delay(100); // Simulate an async operation
        yield return i;
    }
}

async Task ProcessNumbers()
{
    await foreach (var number in GenerateNumbers())
    {
        Console.WriteLine(number);
    }
}
Key elements:

IAsyncEnumerable<T>: Represents an asynchronous sequence of data.
IAsyncEnumerator<T>: Provides the mechanism to iterate over the asynchronous sequence.
await foreach: Used to asynchronously iterate over the stream.


# 20. Describe the purpose of the `HttpClient` class in C#, and how it can be used to make HTTP requests.
The HttpClient class in C# provides a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI. It acts as a client to interact with HTTP servers. It simplifies sending various HTTP requests like GET, POST, PUT, DELETE, etc.

To use HttpClient, you create an instance of it, configure it with settings like base address or default headers if needed, and then use its methods (e.g., GetAsync, PostAsync) to send requests. These methods typically return a Task<HttpResponseMessage>, allowing for asynchronous operations. The response message can then be parsed to retrieve the content, headers, and status code. For example:

using System.Net.Http;
using System.Threading.Tasks;

public class Example
{
    public static async Task Main(string[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://example.com");
            HttpResponseMessage response = await client.GetAsync("/api/data");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine(result);
            }
        }
    }
}

 # 21. What are custom attributes, and how would you implement and use them for custom metadata handling in your applications?
Custom attributes, also known as annotations in some languages, are a way to add metadata to code elements like classes, methods, properties, and fields. They allow you to associate information with these elements that can be accessed at runtime using reflection or compile time using source generators.

To implement custom attributes, you first define a class that inherits from the Attribute base class. You can then apply this attribute to code elements using the [AttributeName] syntax. To use the metadata, you would typically use reflection to inspect the code element and retrieve the attribute instance. Here's an example in C#:

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MyCustomAttribute : Attribute
{
    public string Description { get; set; }
    public MyCustomAttribute(string description)
    {
        Description = description;
    }
}

[MyCustomAttribute("This is a sample class")]
public class MyClass {
    [MyCustomAttribute("This is a sample method")]
    public void MyMethod() { }
}

//accessing the Attribute:
Type type = typeof(MyClass);
MyCustomAttribute attribute = (MyCustomAttribute)Attribute.GetCustomAttribute(type, typeof(MyCustomAttribute));
if (attribute != null) {
    Console.WriteLine(attribute.Description);
}


# 22. Explain different ways to handle errors and exceptions in C#, including try-catch blocks, custom exceptions, and global exception handling.
C# offers several mechanisms for handling errors and exceptions. The primary way is using try-catch blocks. Code that might throw an exception is placed within the try block. If an exception occurs, the control is transferred to the corresponding catch block, where you can handle the exception (e.g., log it, display an error message, or attempt recovery). Multiple catch blocks can handle different types of exceptions. A finally block can be added to ensure that certain code (e.g., releasing resources) always executes, regardless of whether an exception was thrown or caught.

You can also create custom exceptions by inheriting from the Exception class. This allows you to define specific exception types for your application's needs. Finally, global exception handling can be implemented to catch unhandled exceptions at the application level. This usually involves subscribing to events like Application.ThreadException (Windows Forms) or AppDomain.UnhandledException (Console applications) to log or handle exceptions that were not caught by try-catch blocks. This is often used to prevent application crashes and to provide more graceful error reporting. throw; can be used to rethrow the exception so that it can be handled by higher layers.


# 23. Can you discuss the purpose and benefits of using immutable data structures in C# for concurrent programming?
Immutable data structures in C# offer significant benefits for concurrent programming. Their primary purpose is to prevent race conditions and data corruption by ensuring that once created, the state of the object cannot be changed. This eliminates the need for locks or other synchronization mechanisms when multiple threads access the same data, simplifying concurrent code and improving performance.

The key benefits include:

Thread Safety: No need for locks, reducing complexity and improving performance.
Predictability: Easier to reason about the state of the application.
Reduced Bugs: Eliminates a common source of concurrency errors.
Simplified Testing: Easier to test concurrent code without race conditions.
For example, using ImmutableList<T> avoids issues compared to List<T> where concurrent modifications without proper locking can lead to unexpected behavior.


# 24. How does C# support interoperability with unmanaged code, and what are the challenges associated with it?
C# supports interoperability with unmanaged code primarily through Platform Invoke (P/Invoke) and COM Interop. P/Invoke allows C# code to call functions in DLLs written in languages like C or C++. COM Interop enables C# code to interact with COM components. To use P/Invoke, you declare the external function using the DllImport attribute, specifying the DLL name and other details. For COM Interop, you can import COM type libraries as .NET assemblies using tools like tlbimp.exe.

Challenges include:

Marshalling data: Converting data types between managed (.NET) and unmanaged environments can be complex and error-prone. Incorrect marshalling can lead to crashes or incorrect data.
Memory management: Unmanaged code often requires manual memory management, which can lead to memory leaks or corruption if not handled carefully.
Exception handling: Exceptions thrown in unmanaged code are not automatically propagated to the managed environment, requiring explicit handling.
Security: Interacting with unmanaged code can introduce security vulnerabilities if the unmanaged code is not trusted or contains vulnerabilities.


 # 25. Explain the concepts of multi-threading and parallelism in C#, and how would you implement them to improve performance?
Multi-threading and parallelism are both techniques for achieving concurrency, but they differ in their execution. Multi-threading involves multiple threads running within a single process, sharing the same memory space. This is useful for I/O-bound tasks or tasks that can be broken down into smaller, independent units of work. Parallelism, on the other hand, involves running multiple tasks simultaneously on multiple CPU cores. This is suitable for CPU-bound tasks that can be divided into independent subtasks.

To implement them in C#, you can use the System.Threading namespace for multi-threading and the System.Threading.Tasks namespace, especially the Parallel class, for parallelism. Here's a simple example using Parallel.For for parallelism:

Parallel.For(0, 100, i => {
 // Code to be executed in parallel for each value of i
 Console.WriteLine($"Task {i} running on thread {Thread.CurrentThread.ManagedThreadId}");
});
For multi-threading, you can use the Thread class or the ThreadPool class. For example:

Thread thread = new Thread(() => {
    // Code to be executed in a separate thread
    Console.WriteLine($"Task running on thread {Thread.CurrentThread.ManagedThreadId}");
});
thread.Start();
To improve performance, identify tasks that can run concurrently and choose the appropriate technique (multi-threading or parallelism) based on whether the task is I/O-bound or CPU-bound. Also, consider factors like thread synchronization and data sharing to avoid race conditions and deadlocks.


# **Expert C# interview questions** #
# 1. Explain the nuances of using `async` and `await` in a complex C# application. How do you ensure proper error handling and prevent deadlocks?
Using async and await in complex C# applications involves understanding their impact on thread context and synchronization. async marks a method as asynchronous, allowing the use of await. await suspends execution until an awaited task completes, returning control to the caller without blocking the thread. This is crucial for UI responsiveness and scalability.

Error handling is essential. Use try-catch blocks around await expressions. For example:

async Task MyAsyncMethod()
{
  try
  {
    await SomeTaskAsync();
  }
  catch (Exception ex)
  {
    // Handle the exception
  }
}
To prevent deadlocks, avoid .Result or .Wait() on Task objects in async methods (or methods called from async methods). These block the calling thread, potentially leading to a deadlock if the awaited task tries to access the blocked thread's context. ConfigureAwait(false) can mitigate deadlocks when dealing with UI contexts, allowing the continuation to run on a thread pool thread. Favor async all the way down approach. When an API only provides synchronous methods, consider wrapping them in Task.Run.


# 5. Explain the internals of the C# garbage collector. How can you profile and optimize your code to reduce garbage collection pressure?
The C# garbage collector (GC) is an automatic memory manager. It reclaims memory occupied by objects that are no longer in use. It's generational, meaning it categorizes objects based on their age. Gen 0 holds short-lived objects, Gen 1 holds objects that survived a Gen 0 collection, and Gen 2 holds long-lived objects. The GC works by identifying the root objects (static variables, objects on the stack) and traversing the object graph to find reachable objects. Unreachable objects are considered garbage and their memory is reclaimed. The GC process involves marking reachable objects, relocating them to compact memory (mostly for gen 0 and 1) and updating references, and sweeping (reclaiming the unreachable objects' memory).

To reduce garbage collection pressure, you can profile your code using tools like the .NET Performance Monitor or Visual Studio's diagnostic tools to identify where allocations occur most frequently. Optimization techniques include: object pooling to reuse objects instead of creating new ones, reducing the lifetime of objects by disposing of them promptly (using using statements or IDisposable), minimizing boxing and unboxing operations (since these create new objects), using structs instead of classes for small, value-type objects, and avoiding excessive string concatenation (use StringBuilder instead). Understanding value types vs reference types is crucial. For instance:

// Bad: Creates many string objects
string result = "";
for (int i = 0; i < 1000; i++)
{
 result += i.ToString();
}

// Good: Uses StringBuilder
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
 sb.Append(i.ToString());
}
string result = sb.ToString();


# 6. Describe the implementation details of LINQ deferred execution. How does it affect performance and debugging, and how can you optimize LINQ queries?
LINQ's deferred execution means a query isn't executed when you define it. Instead, it's executed when you iterate over the results (e.g., using a foreach loop or calling methods like ToList() or ToArray()). The query expression is translated into an expression tree, which is then executed by the LINQ provider when the results are needed. This allows for optimizations, as the provider can analyze the entire query and choose the most efficient execution strategy. It also enables query composition, where multiple queries can be chained together and executed as a single operation.

Deferred execution can impact performance. If the source data changes between query definition and execution, the results will reflect the changes. Also, repeated iteration over the same query will re-execute it each time, potentially causing performance issues. Debugging can be tricky because the actual query execution happens later, making it difficult to trace the values at the point of definition. To optimize LINQ queries: (1) Use ToList() or ToArray() if you need to cache the results and avoid re-execution. (2) Filter data as early as possible in the query to reduce the amount of data processed. (3) Consider using compiled queries for frequently executed queries, as they can improve performance by pre-compiling the expression tree. (4) Be mindful of the underlying data access technology (e.g., Entity Framework) and optimize database queries accordingly. For example, explicitly specifying which columns to select from the table to avoid fetching unnecessary data.

# 7. What are the advantages and disadvantages of using immutable data structures in C#? How do you effectively implement them and handle updates?
Immutable data structures in C# offer several advantages: they are inherently thread-safe, simplifying concurrent programming; they improve predictability by preventing unexpected state changes; and they can enhance performance through techniques like structural sharing. However, they also have disadvantages: every modification creates a new object, which can lead to increased memory consumption and garbage collection overhead. Creating and manipulating immutable objects can sometimes be less performant than working with mutable objects directly, especially when dealing with large datasets or frequent updates.

To effectively implement immutable data structures in C#, utilize features like readonly fields and properties with only a getter, preventing external modification. For handling updates, avoid direct modification; instead, create new instances with the desired changes, often using methods that return a modified copy. Libraries like System.Collections.Immutable provide pre-built immutable collections that optimize performance and memory usage through structural sharing. Example: ImmutableList<int> newList = originalList.Add(5);


# 8. Explain the use cases for different types of collections in C# (e.g., `ConcurrentDictionary`, `ImmutableList`). When should you prefer one over another?
C# offers various collection types optimized for different scenarios. List<T> is a general-purpose, resizable array, suitable when you need ordered storage and frequent access by index, but it's not thread-safe. Dictionary<TKey, TValue> provides fast lookups by key, ideal for key-value pairs, but again, not thread-safe for concurrent access.

ConcurrentDictionary<TKey, TValue> is designed for thread-safe operations in multi-threaded environments, preventing data corruption. ImmutableList<T> creates a new list whenever it is modified, thus preserving previous states. Immutable collections ensure that data isn't changed unintentionally, which can greatly assist in debugging and reasoning about code, particularly in concurrent or multi-threaded scenarios or when you need to ensure the history of your collections remains intact.



# 10. Discuss the design patterns applicable for building a scalable and maintainable microservices architecture using C# and .NET.
Several design patterns are beneficial for building a scalable and maintainable microservices architecture with C# and .NET. CQRS (Command Query Responsibility Segregation) can separate read and write operations, allowing each to be scaled and optimized independently. Eventual Consistency complements CQRS, accepting that data across services might not be immediately consistent, improving availability and responsiveness. The Saga pattern manages distributed transactions across multiple services, ensuring data consistency in complex workflows, often implemented using orchestrators or choreographies.

Furthermore, API Gateway provides a single entry point for clients, abstracting the underlying microservices and handling concerns like authentication and rate limiting. Circuit Breaker enhances resilience by preventing cascading failures between services. For discoverability and configuration, patterns like Service Discovery (e.g., using Consul or Eureka, though less common in .NET compared to cloud native solutions like Azure Service Fabric or Kubernetes with their built-in discovery) and Externalized Configuration (e.g., using Azure App Configuration or HashiCorp Vault) are crucial. Consider using MediatR library for implementing in-process messaging between services, which promotes loose coupling. Finally, employing a robust Observability strategy (logging, tracing, metrics) using tools like Application Insights, Prometheus, or Grafana is crucial for monitoring and debugging a distributed system.


# 18. Discuss the various ways to serialize and deserialize objects in C#. What are the performance and security considerations for each method?
C# offers several ways to serialize and deserialize objects, each with its own trade-offs. Common methods include: Binary Serialization (using BinaryFormatter), XML Serialization (XmlSerializer), JSON Serialization (using System.Text.Json, Newtonsoft.Json), and Data Contract Serialization (DataContractSerializer). Binary serialization is generally faster but poses significant security risks as it can execute arbitrary code during deserialization; it's also versioning-dependent. XML serialization is more secure and interoperable, but can be less efficient. JSON serialization is widely used due to its human-readable format and broad support, offering good performance, especially with System.Text.Json. Data Contract Serialization provides a balance between performance and flexibility, and it's less sensitive to versioning changes.

Performance depends on the size and complexity of the objects, the chosen serializer, and the underlying implementation. Security-wise, avoid using BinaryFormatter due to its vulnerability to deserialization attacks. Always validate input during deserialization to prevent malicious data from compromising your application. Consider using serialization attributes (e.g., [Serializable], [DataContract]) to control which members are serialized and how. When choosing a method, consider factors such as performance, security, interoperability, and versioning requirements.



# 21. How would you implement a generic retry mechanism in C# that handles different types of exceptions and implements exponential backoff?
A generic retry mechanism in C# can be implemented using a combination of delegates, exception filters, and a loop with exponential backoff. The core idea is to define a method that encapsulates the operation to be retried and execute it within a try-catch block.

Here's a simplified example:

public static T Retry<T>(Func<T> operation, int maxRetries = 3, TimeSpan? initialDelay = null, params Type[] exceptionTypes) {
    initialDelay = initialDelay ?? TimeSpan.FromSeconds(1);
    exceptionTypes = exceptionTypes ?? new Type[] { typeof(Exception) };
    int retryCount = 0;
    while (true) {
        try {
            return operation();
        } catch (Exception ex) when (exceptionTypes.Any(type => type.IsInstanceOfType(ex))) {
            if (retryCount >= maxRetries) throw;
            retryCount++;
            TimeSpan delay = initialDelay.Value * Math.Pow(2, retryCount - 1);
            Thread.Sleep(delay);
        }
    }
}
This Retry method takes a function (Func<T> operation) representing the code to be retried, a maximum number of retries, an optional initial delay, and a list of exception types to handle. The while loop executes the operation, catching exceptions that match the provided types. Exponential backoff is achieved by doubling the delay after each retry. If the maximum number of retries is reached, the exception is re-thrown. If the operation is successful, the result is returned.






Absolutely. Let’s go **topic by topic**, but in a way you can actually explain in a **5-year C#/.NET interview**.

---

# 1. Task

### Definition

A **Task** represents an asynchronous or concurrent operation that may complete in the future.

In simple words:

> A Task represents **work that is being done or will be completed later**.

Example:

```csharp
public async Task<string> GetDataAsync()
{
    return await httpClient.GetStringAsync(url);
}
```

Here:

```csharp
Task<string>
```

means the method will eventually produce a `string`.

### Important point

A `Task` **does not necessarily create a new thread**.

For example:

```csharp
await httpClient.GetAsync(url);
```

The HTTP request is I/O-bound. While waiting for the server, a ThreadPool thread doesn't need to remain blocked.

### Task with result

```csharp
Task<int> task = CalculateAsync();

int result = await task;
```

`Task<int>` means:

> "This operation will eventually give me an integer."

### Task without result

```csharp
Task task = SaveDataAsync();

await task;
```

This means:

> "This operation will eventually complete, but it doesn't return a value."

---

# 2. Thread

### Definition

A **Thread** is an independent path of execution within a process.

Think about a program:

```text
Application / Process
        |
   ----------------
   |      |       |
Thread  Thread  Thread
  1       2       3
```

Each thread can execute code independently.

Example:

```csharp
Thread thread = new Thread(() =>
{
    Console.WriteLine("Hello");
});

thread.Start();
```

A new thread is created and starts executing the specified code.

### Thread characteristics

A thread:

* Has its own execution path
* Uses memory/resources
* Is scheduled by the operating system
* Can execute code concurrently with other threads

### Why not create threads everywhere?

Creating and destroying threads has overhead.

That's why .NET provides the **ThreadPool** and higher-level abstractions such as `Task`.

---

# 3. ThreadPool

### Definition

The **ThreadPool** is a collection of reusable threads managed by .NET.

Instead of:

```text
Create Thread
     ↓
Do work
     ↓
Destroy Thread
```

ThreadPool does:

```text
Get existing thread
     ↓
Do work
     ↓
Return thread to pool
     ↓
Reuse later
```

### Example

```csharp
Task.Run(() =>
{
    ProcessData();
});
```

The work can be scheduled on a ThreadPool thread.

### Why ThreadPool?

Creating threads repeatedly is expensive.

ThreadPool provides:

* Thread reuse
* Better performance
* Reduced thread creation overhead
* Better resource management

### Important distinction

Don't say:

> Every Task uses a ThreadPool thread.

That's incorrect.

For example:

```csharp
await httpClient.GetAsync(url);
```

doesn't need a ThreadPool thread sitting there waiting for the network response.

---

# 4. Parallel Programming

### Definition

**Parallel programming** means executing multiple operations concurrently, usually by utilizing multiple CPU cores.

Suppose you have:

```text
Task 1
Task 2
Task 3
Task 4
```

Sequential:

```text
Task 1 → Task 2 → Task 3 → Task 4
```

Parallel:

```text
Task 1 ──→ CPU Core 1
Task 2 ──→ CPU Core 2
Task 3 ──→ CPU Core 3
Task 4 ──→ CPU Core 4
```

Example:

```csharp
Parallel.For(0, 100, i =>
{
    ProcessItem(i);
});
```

### When should we use parallel programming?

Mostly for **CPU-bound operations**.

Examples:

* Complex calculations
* Image processing
* Data processing
* Encryption
* Compression

### Important

Parallel programming is not the same as asynchronous programming.

---

# 5. Async vs Parallel

This is a **very important interview question**.

### Async

Async is primarily useful for **I/O-bound operations**.

Examples:

```text
Database
HTTP API
File
Network
AWS service
```

The application spends time **waiting**.

### Parallel

Parallelism is primarily useful for **CPU-bound operations**.

Examples:

```text
Calculations
Image processing
Large data transformations
```

The CPU needs to **do more work**.

### Easy way to remember

> **Async = don't block while waiting.**

> **Parallel = do multiple pieces of work at the same time.**

---

# 6. Synchronization

### Definition

**Synchronization** is the process of controlling access to shared resources when multiple threads are executing concurrently.

Consider:

```csharp
int counter = 0;
```

Two threads execute:

```csharp
counter++;
```

It looks like one operation, but internally it involves:

```text
Read value
    ↓
Add 1
    ↓
Write value
```

Suppose counter = 10.

```text
Thread 1 → reads 10
Thread 2 → reads 10

Thread 1 → writes 11
Thread 2 → writes 11
```

Expected:

```text
12
```

Actual:

```text
11
```

This is a problem caused by concurrent access.

Synchronization mechanisms help prevent this.

Examples:

```text
lock
Monitor
Mutex
Semaphore
Interlocked
```

---

# 7. Lock

### Definition

`lock` is used to ensure that **only one thread at a time** can execute a particular section of code.

Example:

```csharp
private readonly object _lock = new();

public void Increment()
{
    lock (_lock)
    {
        counter++;
    }
}
```

When Thread 1 gets the lock:

```text
Thread 1 → 🔒 → executes

Thread 2 → waits
Thread 3 → waits
```

After Thread 1 exits:

```text
Thread 1 → releases lock

Thread 2 → 🔒 → executes
```

### Why use lock?

To protect shared mutable state.

For example:

```csharp
lock (_lock)
{
    _balance -= amount;
}
```

This prevents multiple threads from modifying the balance simultaneously.

### Important

Keep the locked section small.

Bad:

```csharp
lock (_lock)
{
    CallExternalApi();
    QueryDatabase();
    DoLongCalculation();
}
```

You're holding the lock for too long.

Better:

```csharp
var result = Calculate();

lock (_lock)
{
    UpdateSharedData(result);
}
```

---

# 8. Monitor

### Definition

`Monitor` is a .NET synchronization mechanism that provides **mutual exclusion and thread coordination**.

A `lock` is essentially a simpler syntax around monitor-based locking.

This:

```csharp
lock (_lock)
{
    DoSomething();
}
```

is conceptually similar to:

```csharp
Monitor.Enter(_lock);

try
{
    DoSomething();
}
finally
{
    Monitor.Exit(_lock);
}
```

### Why Monitor?

Monitor provides more control than `lock`.

For example:

```csharp
if (Monitor.TryEnter(_lock, TimeSpan.FromSeconds(5)))
{
    try
    {
        DoSomething();
    }
    finally
    {
        Monitor.Exit(_lock);
    }
}
```

Here, the thread attempts to acquire the lock and can stop waiting after a timeout.

### Interview answer

> `lock` is the simpler syntax for mutual exclusion, while `Monitor` provides additional functionality such as `TryEnter` and thread signaling.

---

# 9. Mutex

### Definition

A **Mutex** is a synchronization mechanism that allows only one thread to access a resource at a time and can also be used for synchronization **between processes**.

This is the important distinction.

### Lock

Usually:

```text
Process
 ├── Thread 1
 ├── Thread 2
 └── Thread 3
```

`lock` is generally used within that process.

### Mutex

Can coordinate:

```text
Process A
     ↓
   Mutex
     ↑
Process B
```

### Example use case

You want only one instance of an application to run:

```text
Application Instance 1
       ↓
    Gets Mutex

Application Instance 2
       ↓
    Cannot get Mutex
```

### Interview answer

> Mutex is useful when synchronization is required across processes, whereas `lock` is normally used for synchronization within a process.

---

# 10. Semaphore

### Definition

A **Semaphore** controls the number of threads that can access a resource simultaneously.

Suppose:

```csharp
SemaphoreSlim semaphore = new SemaphoreSlim(3);
```

It allows up to **3 operations at the same time**.

```text
Thread 1 ──┐
Thread 2 ──┤
Thread 3 ──┤ → Resource
Thread 4 ──┘   Waiting
Thread 5       Waiting
```

When Thread 1 finishes:

```text
Thread 1 → leaves

Thread 4 → enters
```

### Real-world example

Imagine you have an external API that allows only 5 concurrent requests.

You can use:

```csharp
SemaphoreSlim semaphore = new SemaphoreSlim(5);
```

Then only 5 operations are allowed concurrently.

### Lock vs Semaphore

```text
lock       → 1 thread
semaphore  → N threads
```

---

# 11. Race Condition

### Definition

A **race condition** occurs when multiple threads access shared data concurrently and the final result depends on the timing/order of execution.

Example:

```csharp
private int _counter = 0;

public void Increment()
{
    _counter++;
}
```

Suppose two threads call it simultaneously.

```text
Thread 1 → Read 0
Thread 2 → Read 0

Thread 1 → Write 1
Thread 2 → Write 1
```

Expected:

```text
2
```

Actual:

```text
1
```

That's a race condition.

### How to solve it?

Use:

```csharp
lock
```

or:

```csharp
Interlocked.Increment(ref _counter);
```

or appropriate thread-safe collections.

---

# 12. Deadlock

### Definition

A **deadlock** occurs when two or more threads are waiting indefinitely for resources held by each other.

Classic example:

```text
Thread 1
   ↓
Gets Lock A
   ↓
Waiting for Lock B

Thread 2
   ↓
Gets Lock B
   ↓
Waiting for Lock A
```

Now:

```text
Thread 1 → waiting for Thread 2
Thread 2 → waiting for Thread 1
```

Nobody can continue.

---

## How to prevent deadlocks?

### 1. Acquire locks in consistent order

Bad:

```text
Thread 1: Lock A → Lock B
Thread 2: Lock B → Lock A
```

Good:

```text
Thread 1: Lock A → Lock B
Thread 2: Lock A → Lock B
```

### 2. Keep lock duration short

### 3. Avoid unnecessary nested locks

### 4. Be careful with synchronous blocking

Avoid:

```csharp
var result = GetDataAsync().Result;
```

or:

```csharp
GetDataAsync().Wait();
```

Prefer:

```csharp
var result = await GetDataAsync();
```

---

# 13. CancellationToken

### Definition

`CancellationToken` provides a mechanism for **requesting cooperative cancellation** of an asynchronous or long-running operation.

Example:

```csharp
CancellationTokenSource cts = new();

await DoWorkAsync(cts.Token);
```

At some point:

```csharp
cts.Cancel();
```

requests cancellation.

The operation can observe the token:

```csharp
public async Task DoWorkAsync(CancellationToken token)
{
    for (int i = 0; i < 100; i++)
    {
        token.ThrowIfCancellationRequested();

        await Task.Delay(100, token);
    }
}
```

### Important interview point

CancellationToken **doesn't forcibly terminate a thread**.

It is cooperative.

The code being executed must respond to the cancellation request.
CancellationToken does not forcibly stop execution. It requests cancellation. The operation must cooperate by checking the token or passing it to an API that supports cancellation. If cancellation is observed, the operation can stop, commonly by throwing OperationCanceledException.
---

# 14. async

### Definition

`async` is a C# keyword used to declare a method that can perform asynchronous operations and use the `await` keyword.

Example:

```csharp
public async Task<string> GetDataAsync()
{
    return await GetDataFromApiAsync();
}
```

The method returns a `Task` or `Task<T>`.

---

# 15. await

### Definition

`await` asynchronously waits for a Task to complete **without blocking the current thread**.

Example:

```csharp
var customer = await GetCustomerAsync();
```

Conceptually:

```text
Start operation
      ↓
Operation not complete
      ↓
Method yields
      ↓
Thread can do other work
      ↓
Operation completes
      ↓
Method continues
```

---

# 16. Why async/await for API calls?

This is one of the **most important questions for your interview**.

Suppose your API calls another service:

```csharp
var response = await httpClient.GetAsync(url);
```

HTTP is an I/O operation.

The application sends the request:

```text
.NET Application
      ↓
HTTP Request
      ↓
External Server
      ↓
Waiting for response
```

The response might take:

```text
100 ms
500 ms
2 seconds
```

During that waiting period, your application doesn't need a ThreadPool thread to sit there doing nothing.

With async:

```text
Request
   ↓
Start HTTP operation
   ↓
Thread is not blocked waiting
   ↓
Thread can serve other work
   ↓
HTTP response arrives
   ↓
Continue method
   ↓
Return response
```

### Why is this important?

Because a web server may receive thousands of requests.

If every request blocks a ThreadPool thread while waiting for I/O:

```text
Request 1 → Thread blocked
Request 2 → Thread blocked
Request 3 → Thread blocked
Request 4 → Thread blocked
...
```

Eventually the application can experience **ThreadPool starvation and poor scalability**.

With asynchronous I/O:

```text
Request 1 → I/O waiting
Request 2 → I/O waiting
Request 3 → I/O waiting
Request 4 → I/O waiting
```

The server can use its threads more efficiently.

---

# 17. Why async/await for Database calls?

Database access is also I/O-bound.

Example with Entity Framework Core:

```csharp
public async Task<Customer?> GetCustomerAsync(
    int id,
    CancellationToken token)
{
    return await _context.Customers
        .FirstOrDefaultAsync(x => x.Id == id, token);
}
```

Flow:

```text
.NET API
   ↓
SQL Query
   ↓
SQL Server
   ↓
Database processes query
   ↓
Result returned
   ↓
.NET continues
```

During database processing, there is no reason to block a ThreadPool thread just waiting.

Therefore:

> Async database operations improve scalability by avoiding unnecessary thread blocking while the database performs I/O.

---

# 18. async does NOT mean new thread

This is **very important**.

Wrong:

> "`async` creates a new thread."

Correct:

> "`async/await` allows asynchronous operations to proceed without blocking the current thread while waiting for the operation to complete."

For I/O:

```text
async/await
      ↓
No dedicated waiting thread required
```

For CPU work:

```csharp
Task.Run(() => HeavyCalculation());
```

may use a ThreadPool thread.

---

# 19. Sequential async vs Concurrent async

Suppose you need:

```text
Customer
Orders
Payments
```

### Sequential

```csharp
var customer = await GetCustomerAsync();
var orders = await GetOrdersAsync();
var payments = await GetPaymentsAsync();
```

Flow:

```text
Customer
   ↓
Orders
   ↓
Payments
```

If each takes 1 second, roughly:

```text
3 seconds
```

assuming they can be performed independently and similar latency.

---

### Concurrent

```csharp
var customerTask = GetCustomerAsync();
var ordersTask = GetOrdersAsync();
var paymentsTask = GetPaymentsAsync();

await Task.WhenAll(
    customerTask,
    ordersTask,
    paymentsTask);
```

Flow:

```text
Customer ──────────┐
Orders ────────────┤
Payments ──────────┤
                   ↓
              WhenAll
```

If they each take approximately 1 second, total waiting can be closer to:

```text
1 second
```

rather than 3 seconds.

### Important

Don't use concurrency blindly.

You need to consider:

* Database connection limits
* External API rate limits
* CPU usage
* Resource contention
* Dependency relationships

---

# 20. Task.Run vs async I/O

This is another common interview question.

### Don't unnecessarily do this:

```csharp
await Task.Run(() =>
    httpClient.GetAsync(url));
```

The HTTP client already supports asynchronous I/O.

Prefer:

```csharp
await httpClient.GetAsync(url);
```

### Task.Run is more appropriate for CPU-bound work

```csharp
var result = await Task.Run(() =>
{
    PerformHeavyCalculation();
});
```

Here the calculation needs CPU time, so moving it to a ThreadPool thread may be useful depending on the scenario.

---

# ⭐ Complete mental model

Remember this diagram:

```text
                 C# CONCURRENCY
                       |
        ┌──────────────┴──────────────┐
        |                             |
      ASYNC                       PARALLEL
        |                             |
    I/O-bound                    CPU-bound
        |                             |
   API / DB / File              Calculations
        |                             |
   async / await               Parallel / Tasks
        |
   Avoid blocking
        |
   Better scalability
```

And synchronization:

```text
             SHARED RESOURCE
                    |
             Multiple Threads
                    |
          ┌─────────┴─────────┐
          |                   |
     Race Condition        Need Control
                              |
                ┌─────────────┼────────────┐
                |             |            |
               Lock        Semaphore     Mutex
                |
           One at a time
```

---

# 🔥 Interview definitions to memorize

### Task

> A Task represents an asynchronous or concurrent operation that may complete in the future.

### Thread

> A Thread is an independent path of execution within a process.

### ThreadPool

> ThreadPool is a collection of reusable threads managed by .NET for executing work efficiently.

### Parallel Programming

> Parallel programming means executing multiple pieces of work concurrently, usually using multiple CPU cores.

### Synchronization

> Synchronization controls access to shared resources when multiple threads execute concurrently.

### Lock

> Lock ensures that only one thread can enter a protected critical section at a time.

### Monitor

> Monitor is a synchronization mechanism that provides mutual exclusion and additional thread coordination features.

### Mutex

> Mutex provides mutual exclusion and can synchronize access across different processes.

### Semaphore

> Semaphore limits the number of threads that can access a resource simultaneously.

### Race Condition

> A race condition occurs when multiple threads access shared data and the result depends on the timing of execution.

### Deadlock

> Deadlock occurs when threads wait indefinitely for resources held by each other.

### CancellationToken

> CancellationToken provides a mechanism for requesting cooperative cancellation of an operation.

### async/await

> async/await provides a programming model for asynchronous operations, allowing the application to avoid blocking while waiting for I/O.

### Why async/await for API/DB?

> **API and database operations are I/O-bound. async/await prevents threads from being blocked while waiting for I/O, allowing the server to use its threads more efficiently and handle more concurrent requests, which improves scalability.**

---

# Important C# Coding Tasks

These are common C# coding-round problems. In an interview, explain the input assumptions, handle edge cases, and mention the time and space complexity of your solution.

## 1. Reverse a string

```csharp
public static string Reverse(string value)
{
    if (value is null)
    {
        throw new ArgumentNullException(nameof(value));
    }

    char[] characters = value.ToCharArray();
    Array.Reverse(characters);
    return new string(characters);
}
```

Time complexity is $O(n)$ and space complexity is $O(n)$. For user-perceived characters such as emoji, use `StringInfo` or a text-element-aware approach because a .NET `char` is a UTF-16 code unit.

## 2. Check whether a string is a palindrome

```csharp
public static bool IsPalindrome(string value)
{
    if (value is null)
    {
        return false;
    }

    int left = 0;
    int right = value.Length - 1;

    while (left < right)
    {
        if (value[left] != value[right])
        {
            return false;
        }

        left++;
        right--;
    }

    return true;
}
```

This is a two-pointer solution with $O(n)$ time and $O(1)$ additional space. State whether comparison should ignore case, spaces, or punctuation before implementing it.

## 3. Find duplicate values in an array

```csharp
public static IEnumerable<int> FindDuplicates(IEnumerable<int> numbers)
{
    if (numbers is null)
    {
        throw new ArgumentNullException(nameof(numbers));
    }

    return numbers
        .GroupBy(number => number)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key);
}
```

This returns each duplicate value once. A `HashSet<int>` can be used for a single-pass solution when the input is enumerated only once.

```csharp
public static IEnumerable<int> FindDuplicatesSinglePass(IEnumerable<int> numbers)
{
    var seen = new HashSet<int>();
    var duplicates = new HashSet<int>();

    foreach (int number in numbers)
    {
        if (!seen.Add(number))
        {
            duplicates.Add(number);
        }
    }

    return duplicates;
}
```

Average time complexity is $O(n)$ and additional space complexity is $O(n)$.

## 4. Find the second-largest distinct number

```csharp
public static int? FindSecondLargest(IEnumerable<int> numbers)
{
    if (numbers is null)
    {
        throw new ArgumentNullException(nameof(numbers));
    }

    int? largest = null;
    int? secondLargest = null;

    foreach (int number in numbers)
    {
        if (largest is null || number > largest)
        {
            secondLargest = largest;
            largest = number;
        }
        else if (number < largest &&
                 (secondLargest is null || number > secondLargest))
        {
            secondLargest = number;
        }
    }

    return secondLargest;
}
```

The nullable return value clearly represents an input with fewer than two distinct values. This solution runs in $O(n)$ time and $O(1)$ additional space.

## 5. Count character frequency

```csharp
public static Dictionary<char, int> CharacterFrequency(string value)
{
    if (value is null)
    {
        throw new ArgumentNullException(nameof(value));
    }

    var frequencies = new Dictionary<char, int>();

    foreach (char character in value)
    {
        frequencies[character] = frequencies.GetValueOrDefault(character) + 1;
    }

    return frequencies;
}
```

Use `StringComparer.OrdinalIgnoreCase` when the requirement is case-insensitive text, but use a `Dictionary<char, int>` only when UTF-16 code-unit behavior is acceptable.

## 6. Check whether two strings are anagrams

```csharp
public static bool AreAnagrams(string first, string second)
{
    if (first is null || second is null || first.Length != second.Length)
    {
        return false;
    }

    var frequencies = new Dictionary<char, int>();

    foreach (char character in first)
    {
        frequencies[character] = frequencies.GetValueOrDefault(character) + 1;
    }

    foreach (char character in second)
    {
        if (!frequencies.TryGetValue(character, out int count))
        {
            return false;
        }

        if (count == 1)
        {
            frequencies.Remove(character);
        }
        else
        {
            frequencies[character] = count - 1;
        }
    }

    return frequencies.Count == 0;
}
```

This approach is $O(n)$ time and avoids sorting. Clarify whether whitespace, punctuation, and casing should be ignored.

## 7. Print the Fibonacci sequence

```csharp
public static IEnumerable<long> Fibonacci(int count)
{
    if (count < 0)
    {
        throw new ArgumentOutOfRangeException(nameof(count));
    }

    long previous = 0;
    long current = 1;

    for (int index = 0; index < count; index++)
    {
        yield return previous;
        (previous, current) = (current, previous + current);
    }
}
```

This iterative and lazy implementation uses $O(1)$ state, excluding values consumed by the caller. `long` eventually overflows, so use `BigInteger` when large values are required.

## 8. Solve FizzBuzz

```csharp
public static IEnumerable<string> FizzBuzz(int maximum)
{
    for (int number = 1; number <= maximum; number++)
    {
        if (number % 15 == 0)
        {
            yield return "FizzBuzz";
        }
        else if (number % 3 == 0)
        {
            yield return "Fizz";
        }
        else if (number % 5 == 0)
        {
            yield return "Buzz";
        }
        else
        {
            yield return number.ToString();
        }
    }
}
```

Check divisibility by 15 first, or the number will match 3 before it can produce `FizzBuzz`.

## 9. Remove duplicate values while preserving order

```csharp
public static IEnumerable<T> DistinctInOrder<T>(IEnumerable<T> values)
{
    if (values is null)
    {
        throw new ArgumentNullException(nameof(values));
    }

    var seen = new HashSet<T>();

    foreach (T value in values)
    {
        if (seen.Add(value))
        {
            yield return value;
        }
    }
}
```

This is equivalent to the behavior of LINQ `Distinct()` for normal equality semantics. It runs in average $O(n)$ time and uses $O(n)$ additional space.

## 10. Find the first non-repeating character

```csharp
public static char? FirstNonRepeatingCharacter(string value)
{
    if (value is null)
    {
        throw new ArgumentNullException(nameof(value));
    }

    var frequencies = new Dictionary<char, int>();

    foreach (char character in value)
    {
        frequencies[character] = frequencies.GetValueOrDefault(character) + 1;
    }

    foreach (char character in value)
    {
        if (frequencies[character] == 1)
        {
            return character;
        }
    }

    return null;
}
```

The two-pass approach preserves the original order and runs in $O(n)$ time.

## 11. Merge two sorted arrays

```csharp
public static int[] MergeSortedArrays(int[] first, int[] second)
{
    if (first is null || second is null)
    {
        throw new ArgumentNullException(first is null ? nameof(first) : nameof(second));
    }

    var result = new int[first.Length + second.Length];
    int firstIndex = 0;
    int secondIndex = 0;
    int resultIndex = 0;

    while (firstIndex < first.Length && secondIndex < second.Length)
    {
        result[resultIndex++] = first[firstIndex] <= second[secondIndex]
            ? first[firstIndex++]
            : second[secondIndex++];
    }

    while (firstIndex < first.Length)
    {
        result[resultIndex++] = first[firstIndex++];
    }

    while (secondIndex < second.Length)
    {
        result[resultIndex++] = second[secondIndex++];
    }

    return result;
}
```

This uses the two-pointer technique and runs in $O(n + m)$ time.

## 12. Group employees by department using LINQ

```csharp
public sealed record Employee(int Id, string Name, string Department, decimal Salary);

public static IEnumerable<object> GroupEmployees(IEnumerable<Employee> employees)
{
    return employees
        .GroupBy(employee => employee.Department)
        .Select(group => new
        {
            Department = group.Key,
            EmployeeCount = group.Count(),
            AverageSalary = group.Average(employee => employee.Salary),
            HighestSalary = group.Max(employee => employee.Salary)
        });
}
```

Explain deferred execution when returning `IEnumerable<T>`, and materialize with `ToList()` when the result must be evaluated once and reused.

## 13. Implement a thread-safe singleton

```csharp
public sealed class AppConfiguration
{
    private static readonly Lazy<AppConfiguration> InstanceHolder =
        new(() => new AppConfiguration());

    private AppConfiguration()
    {
    }

    public static AppConfiguration Instance => InstanceHolder.Value;
}
```

`Lazy<T>` provides lazy initialization and thread-safe publication by default. In ASP.NET Core applications, prefer registering a service with the built-in dependency injection container as a singleton instead of manually implementing a singleton unless a static instance is specifically required.

## 14. Implement retry with cancellation for an async operation

```csharp
public static async Task<T> ExecuteWithRetryAsync<T>(
    Func<CancellationToken, Task<T>> operation,
    int maximumAttempts,
    TimeSpan delay,
    CancellationToken cancellationToken)
{
    if (operation is null)
    {
        throw new ArgumentNullException(nameof(operation));
    }

    if (maximumAttempts <= 0)
    {
        throw new ArgumentOutOfRangeException(nameof(maximumAttempts));
    }

    for (int attempt = 1; attempt <= maximumAttempts; attempt++)
    {
        try
        {
            return await operation(cancellationToken);
        }
        catch (Exception) when (attempt < maximumAttempts)
        {
            await Task.Delay(delay, cancellationToken);
        }
    }

    throw new InvalidOperationException("The operation did not complete.");
}
```

Real production retry logic should catch only transient exceptions, use exponential backoff with jitter, and avoid retrying non-idempotent operations unless they support an idempotency key.


# C# Developer Services Cheat Sheet

## Related C# and .NET Development Services

| Service or component | Important feature to remember | Common development use |
| --- | --- | --- |
| C# compiler | Converts C# source into Intermediate Language (IL) and reports compile-time errors | Build type-safe applications |
| .NET runtime | Executes IL and provides garbage collection, exceptions, threading, and type loading | Run C# applications |
| CLR/CoreCLR | Runtime services such as JIT compilation, GC, and managed execution | Execute managed code across platforms |
| Base Class Library | Collections, strings, files, networking, dates, reflection, and threading APIs | Use standard, tested framework functionality |
| NuGet | Package management with package versions and dependency graphs | Add third-party or shared libraries |
| Dependency Injection | Lifetime-managed dependency registration and constructor injection | Reduce coupling and improve testability |
| `Task` and `async`/`await` | Composable asynchronous operations and non-blocking I/O | Scale I/O-heavy applications |
| ThreadPool | Reuses worker and I/O threads | Execute tasks without manually creating threads |
| `CancellationToken` | Cooperative cancellation signal | Stop requests, workers, and long-running operations safely |
| LINQ | Query and transform objects, collections, and provider-backed data | Filter, project, group, and aggregate data |
| `IEnumerable<T>` | Synchronous iteration and deferred execution | Process in-memory sequences |
| `IAsyncEnumerable<T>` | Asynchronous streaming with `await foreach` | Stream database, network, or file results |
| `System.Text.Json` | Fast built-in JSON serialization and deserialization | Convert API models to and from JSON |
| `HttpClient` | Connection reuse and HTTP request handling | Call external services |
| `IHttpClientFactory` | Centralized clients and handler lifetime management | Avoid socket exhaustion and configure resilience |
| Configuration providers | JSON, environment variables, command line, and custom sources | Load environment-specific settings |
| `ILogger<T>` | Structured logging with levels and scopes | Produce searchable diagnostics |
| Reflection | Inspect types, members, attributes, and assemblies at runtime | Frameworks, serializers, plugins, and metadata-driven behavior |
| Attributes | Attach declarative metadata to code elements | Validation, serialization, mapping, and test metadata |
| Garbage Collector | Automatically reclaims unreachable managed objects | Manage memory without manual free operations |
| `IDisposable` / `IAsyncDisposable` | Deterministic cleanup for managed wrappers and asynchronous resources | Release files, streams, connections, and handles |
| `Span<T>` / `Memory<T>` | Work with memory slices while reducing allocations and copying | High-performance parsing and buffer processing |
| `Channel<T>` | Async producer-consumer queue with backpressure options | Coordinate background workers |
| `ConcurrentDictionary<TKey,TValue>` | Thread-safe dictionary operations | Share mutable lookup state across threads |
| xUnit / NUnit | Automated unit and integration test frameworks | Verify behavior continuously |
| BenchmarkDotNet | Reliable performance benchmarking | Measure allocations and execution speed |
| Docker | Isolated, repeatable runtime packaging | Deploy consistent C# services |

## Important C# and Runtime Limits

These are common runtime or library characteristics, not universal application quotas. Actual limits depend on process architecture, operating system, .NET version, hosting platform, and available memory.

| Resource or setting | Common limit or behavior | Interview point |
| --- | --- | --- |
| `int` | 32-bit signed integer: -2,147,483,648 to 2,147,483,647 | Use `long` for larger counters or identifiers |
| `long` | 64-bit signed integer | Suitable for large counts, ticks, and database keys |
| `decimal` | 128-bit decimal type with high precision | Prefer for financial calculations; never use `double` for currency totals |
| `double` | 64-bit floating-point value | Good for scientific or approximate calculations, not exact money |
| Array length | Limited by available memory and runtime/object limits | Prefer streaming or paging for very large data sets |
| `string` length | Limited by available memory and runtime object limits | Avoid building huge strings; use streams or `StringBuilder` |
| Object size | Limited by process address space and available memory | Large objects can cause allocation failures or GC pressure |
| Large Object Heap | Objects around 85,000 bytes or larger are generally allocated on the LOH | Reuse buffers and avoid repeated large allocations |
| GC generations | Gen 0, Gen 1, Gen 2, plus the LOH | Short-lived objects are cheaper; long-lived objects increase memory pressure |
| `StringBuilder` capacity | Configurable; expansion is limited by available memory | Pre-size when the approximate output size is known |
| `List<T>` capacity | Dynamically grows as items are added | Set capacity when the item count is predictable to reduce reallocations |
| Dictionary capacity | Dynamically resizes as entries grow | Choose a suitable initial capacity for large collections |
| ThreadPool | Dynamic and process-dependent; do not assume one fixed thread count | Avoid blocking calls and thread starvation |
| `Task` | Represents an operation, not necessarily a dedicated thread | Async I/O does not require one blocked thread per request |
| `Task.WhenAll` | No fixed task count, but all results and exceptions remain relevant until completion | Bound concurrency for large batches |
| `SemaphoreSlim` | No universal concurrency default | Use it to limit calls to databases or external services |
| `CancellationToken` | Cooperative only; it does not forcibly terminate arbitrary code | Pass tokens through every async layer and honor them |
| `HttpClient.Timeout` | 100 seconds by default | Set an intentional timeout and combine it with cancellation |
| `System.Text.Json` max depth | Default maximum depth is 64 | Prevent deeply nested or malicious JSON payloads |
| JSON serialization | No single universal payload limit; server and proxy limits apply | Stream large payloads instead of buffering them all in memory |
| Regex timeout | No timeout unless configured | Always set a timeout for regex processing on untrusted input |
| FileStream | Limited by OS file handles, permissions, disk space, and file-system rules | Dispose streams and use async file I/O where appropriate |
| Socket connections | Limited by OS resources, ephemeral ports, and service quotas | Reuse `HttpClient` and avoid creating one per request |
| Database connections | Provider and connection-string dependent | Dispose connections and use pooling rather than unbounded concurrency |
| `ConcurrentQueue<T>` | Limited by available memory | Apply backpressure instead of allowing an unbounded queue |
| `Channel<T>` capacity | Unbounded unless a bounded channel is configured | Prefer bounded channels when producers can outrun consumers |
| Docker memory/CPU | Controlled by container runtime settings | Configure limits and monitor OOM kills and CPU throttling |

## Quick C# Selection Rules

| Requirement | Prefer |
| --- | --- |
| Simple value with value-based equality | `record` or `record struct` |
| Mutable business entity | `class` |
| Small immutable value | `readonly struct` when allocation and copying are understood |
| Fixed set of named values | `enum` |
| Contract for multiple implementations | `interface` |
| Shared implementation with an is-a relationship | Base class, used carefully |
| Read-only sequence | `IReadOnlyCollection<T>` or `IReadOnlyList<T>` |
| In-memory query | `IEnumerable<T>` and LINQ-to-Objects |
| Provider-translated query | `IQueryable<T>` until the intended materialization point |
| Async stream | `IAsyncEnumerable<T>` |
| Key-based lookup | `Dictionary<TKey,TValue>` |
| Unique values | `HashSet<T>` |
| FIFO producer-consumer workflow | `Channel<T>` or a durable external queue |
| Shared thread-safe lookup | `ConcurrentDictionary<TKey,TValue>` |
| I/O concurrency | `async`/`await` with cancellation |
| CPU-bound parallel work | Carefully bounded parallelism with `Parallel` or tasks |
| External HTTP calls | `IHttpClientFactory` with typed clients |
| Temporary resource cleanup | `using` or `await using` |
| High-throughput parsing | `Span<T>`, `ReadOnlySpan<T>`, or pooled buffers |
| Financial calculations | `decimal` with explicit rounding rules |
| Large data processing | Streaming, pagination, batching, and bounded concurrency |
| Runtime diagnostics | Structured logs, metrics, traces, and profilers |
