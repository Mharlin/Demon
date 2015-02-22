module Tests

//open Xunit
open Swensen.Unquote
open FsCheck.Xunit
open FsCheck


//FsCheck, automatically parametertized tests
[<Property>]
let ``Reverse of reverse of a list is the original list ``(xs:list<int>) =
  List.rev(List.rev xs) = xs

[<Property>]
let ``abs always returns positive``(x:int) =
    (abs x) >= 0




//Simplification - to get a more clear test result
//[<xunit.fact>]
//let ``2 + 2 = 4`` () =
//   test <@ 2 + 2 = 5 @>
