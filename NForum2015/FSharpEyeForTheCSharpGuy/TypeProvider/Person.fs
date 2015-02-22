module Person1
type Person (name: string, age: int) =
    member person.Name = name
    member person.Age = age

//    override person.ToString() =
//        sprintf "%s %d" name age

type Persona = { Name: string; Age: int }

let p = new Person("Magnus", 35)
let pp = { Name = "Ma"; Age = 30 }
