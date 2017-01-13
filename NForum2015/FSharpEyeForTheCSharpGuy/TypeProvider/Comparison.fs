module Comparison

let squares x = x * x

let sumOfSquares num =
    [1..num]
    |> Seq.map squares
    |> Seq.sum

let sum = sumOfSquares 10


let sumOfSquaresShort num = 
    [1..num] 
    |> Seq.sumBy squares

let sum2 = sumOfSquaresShort 10



//let sumOfSquaresAnonymous num = [1..num] |> Seq.sumBy (fun x -> x * x)
//let sum3 = sumOfSquaresAnonymous 10




//QuickSort
//If the list is empty, there is nothing to do.
//Otherwise: 
//  1. Take the first element of the list
//  2. Find all elements in the rest of the list that 
//      are less than the first element, and sort them. 
//  3. Find all elements in the rest of the list that 
//      are >= than the first element, and sort them
//  4. Combine the three parts together to get the final result: 
//      (sorted smaller elements + firstElement + 
//       sorted larger elements)

let rec quicksort list =
   match list with
   | [] ->                            // If the list is empty
        []                            // return an empty list
   | firstElem::otherElements ->      // If the list is not empty     
        let smallerElements =         // extract the smaller ones    
            otherElements             
            |> List.filter (fun e -> e < firstElem) 
            |> quicksort              // and sort them
        let largerElements =          // extract the large ones
            otherElements 
            |> List.filter (fun e -> e >= firstElem)
            |> quicksort              // and sort them
        // Combine the 3 parts into a new list and return it
        List.concat [smallerElements; [firstElem]; largerElements]

//test
printfn "%A" (quicksort [1;5;23;18;9;1;3])



// Quicksort concise version
let rec quicksort2 = function
   | [] -> []                         
   | first::rest -> 
        let smaller,larger = List.partition ((>=) first) rest 
        List.concat [quicksort2 smaller; [first]; quicksort2 larger]
        
// test code        
printfn "%A" (quicksort2 [1;5;23;18;9;1;3])