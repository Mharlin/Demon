module Person1

type Person (name: string, age: int) =
    member x.Name = name
    member x.Age = age

type Persona = 
    { Name: string; Age: int }
    static member op_Equality (left : Persona, right : Persona) = left = right


let p = new Person("Magnus", 36)
let p2 = new Person("Magnus", 36)
let pp2 : Persona = { Name = "Magnus"; Age = 36 }
let pp1 = { Name = "Magnus"; Age = 36 }

let p' = { pp1 with Name = "Mikael"}
//let p' : Person = null
//let p'' : Persona = null
