module SimpleExamples
//open Person

//Lists - int, string
let a = ['a'..'z']

//Pipelining - filter list with even numbers, and sum
let even x = x % 2 = 0
let numbers = [1..20]

let aaaa = List.sum(List.filter even numbers)

let filtered = 
    numbers
    |> List.filter even
    |> List.sum



//Discriminated unions - Add match for RankCard
type Suit =
  | Spades
  | Hearts
  | Clubs
  | Diamonds

type Card =
  | Ace of Suit
  | RankCard of int * Suit
  | Jack of Suit
  | Queen of Suit
  | King of Suit
  | Joker
//
let card1 = Ace(Diamonds)
//let card2 = RankCard(3, Spades)
//let card3 = Joker



type C = Circle of int | Rectangle of int * int

let c = Circle 10
let r = Rectangle (10, 20)


//Pattern matching lists - Add head and tail 
let aa, bb, cc = (1, 2, 3)
let x::y::z = [1..4]

let matchList l =
    match l with
    | [] -> printfn "the list is empty" 
    | [first] -> printfn "the list has one element %A " first 
    | [first; second] -> printfn "list is %A and %A" first second 
    | first::rest -> printf "lengt of tail %d" rest.Length
    | _ -> printfn "the list has more than two elements"

matchList [1..2]


//Pattern match type
type Address = { Street: string; City: string }
type Employee = { Name: string; Company: string; Address: Address }

let m = { Name = "Magnus"; Company = "Squeed"; Address = { Street = "Grönsakstorget"; City = "Göteborg" } }

let { Company = company; Address = { City = city } } = m


//Null values - create Person type set to null, use GetEnvVar with "path" and "missing"
//let ppp : Option<Persona> = None
//let abc = { Name = "M"; Age = 5}
//
//let b = Some({ Name = "Magnus"; Age = 35})
//
//let name = match b with 
//           | Some r  -> r.Name 
//           | None -> ""

let GetEnvVar var = 
    match System.Environment.GetEnvironmentVariable(var) with
    | null -> None
    | value -> Some value

let n = System.Environment.GetEnvironmentVariable("invalid") 
let e = GetEnvVar "invalid"

match e with
| None -> printfn "Value is missing"
| Some(env) -> printfn "Value is %s"env
