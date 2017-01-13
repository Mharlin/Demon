module SimpleExamples
//open Person

let add x y = x + y

let r1 = add 3 5


let addWith5 = add 5 

let r2 = addWith5 10


//Pipelining - filter list with even numbers, and sum
let even x = x % 2 = 0
let numbers = [1..20]

let aa = List.sum(List.filter even numbers)

let filtered = 
    numbers
    |> Seq.filter even
    |> Seq.sum
    |> addWith5


//Discriminated unions - Add match for RankCard
type Suit =
  | Spades
  | Hearts
  | Clubs
  | Diamonds

type Card =
  | Joker
  | Ace of Suit
  | King of Suit
  | Queen of Suit
  | Jack of Suit
  | RankCard of int * Suit

let card1 = Ace(Diamonds)
let card2 = RankCard(3, Spades)
let getValue card = match card with
                    | RankCard(value, suit) -> value
                    | _ -> 0

let value = getValue card2




//Pattern match type
type Address = { Street: string; City: string }
type Employee = { Name: string; Company: string; Address: Address }

//let xx : Employee = null
let m = { Name = "Magnus"; Company = "Squeed"; Address = { Street = "Grönsakstorget"; City = "Göteborg" } }

let newEmployee = { m with Name = "Sofia"}
let { Company = company; Address = { City = city } } = m





type C = Circle of int | Rectangle of int * int

let c = Circle 10
let r = Rectangle (10, 20)


//Pattern matching lists - Add head and tail 
let aaa, bb, cc = (1, 2, 3)
let x::y::z = [1..4]

let matchList l =
    match l with
    | [] -> printfn "the list is empty" 
    | [first] -> printfn "the list has one element %A " first 
    | [first; second] -> printfn "list is %A and %A" first second 
    | first::rest -> printf "lengt of tail %A" rest.Length
    | _ -> printfn "the list has more than two elements"

matchList [1..5]



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
