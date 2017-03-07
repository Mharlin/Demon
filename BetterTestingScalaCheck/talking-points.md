*** Better tests with less code ***

** Why property based testing ** (5 mins)
	- Covers edge cases
	- Explicit rules about input and output
	- Fewer tests cover more
		Less code to change when refactoring
	- The tests aren't biased with the way you "think" that the code should work

** How? ** (3 mins)
	- Formal verification
	- Cannot be automized for Turing-complete languages
		Halting problem
	- Random datasets created by generators

** Properties ** (5 mins)
	- Logical statments that a function must satisfy
	- Quantifiers
	- Assertion

** Generators ** (15 mins)
	- Random choice from interval
	- Built in geneartors
		Demo
	- Customizing generators
		Demo
	- Shrinkers

** Putting it all togheter ** (10 mins)
	- Demo with more complicated examples

** Cons ** (2 mins)
	- Difficult to find good properties
	- Difficult to create good generators
	- Code has to be side effect free

** Pros ** (2 mins)
	- Enhanced readability
	- Focus on valid inputs and outputs
	- Edge cases are automaticcaly covered
	- Shrinks to simplest failing examples

** Q & A ** (3 mins)
	- Questions that were not answerd during session

Inspiration:
http://akimboyko.in.ua/presentations/property-based_testing.html#/ - Property based testing slides in F#	
