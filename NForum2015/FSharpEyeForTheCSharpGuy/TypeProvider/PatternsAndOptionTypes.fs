module PatternsAndOptionTypes

type name = string
type number = int
type date = System.DateTime

//Discriminated unions
type meeting =
    | Personal of name   * date
    | Phone    of number * date
    
let review = Personal("Magnus",System.DateTime.Now)
let call = Phone(031123456, System.DateTime.Now)

let what_to_do m =
    match m with
    | Personal(name, date) ->
        printfn "Meeting with %s at %A" name date
    | Phone(phone, date) ->
        printfn "Call %A at %A" phone date

what_to_do review
what_to_do call
