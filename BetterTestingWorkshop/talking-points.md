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
        - Structure
            class nameOfSpec
                extends PropSpec // to get nice naming from the class displayed in the tests
                with PropertyChecks // to import checks for generated test properties
	- Logical statments that a function must satisfy
	- Quantifiers
	- Assertion
            ScalaCheck
                ==, ?=, =?
            ScalaTest
                matchers
        - properties without specific generators
            demo
                all buildin types like string, int, date
                property("strings are not equal") { forAll { (str1: String, str2: String) => str1 shouldNot be (str2) } }

** Generators ** (15 mins)
	- Random choice from interval
        - Frequency
	- Built in geneartors, Customizing generators
            Demo
                case class Person(name: String, age: Int)

                Gen.resultOf(Person).sample

                val personGen: Gen[Person] = for {
                  name <- Gen.alphaStr
                  age <- Gen.posNum[Int]
                } yield Person(name, age)

                personGen.sample

                ---- Add a map to name so that the name is between 1 and 15 characters
                  n <- Gen.chooseNum(1, 15)
                  name <- Gen.alphaStr.map(_.take(n))
                
                --- Add implicit arbitrary
                    implicit val arbPerson: Arbitrary[Person] = Arbitrary(personGen)
                    arbitrary[Person].sample
                    arbPerson.arbitrary.sample

        - Lab
            Palindrome - when a word reads the same forwards and backwards
                - The generator should not be the same as the implementation
                1. create palindrome gen with even and odd number of charachters
                    should only need one custom gen
                2. write specs for checkReverse
                    check both positive and negative cases
                3. find a bug in chekcIndices with same generator
                    fix the implementation

	- Shrinkers
        - Lab 
            Prisoners dilemma

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


Installation av demomiljö som SBT om man inte har det
förslag: docker image med sbt och scala installerat och repositoryt hämtat från git

Inspiration:
http://akimboyko.in.ua/presentations/property-based_testing.html#/ - Property based testing slides in F#	
