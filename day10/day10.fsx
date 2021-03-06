open System

type HashState = {
    Data: int[]
    SkipSize: int
    TotalShifted: int } with
    member x.Output =
        let shiftDist = (Array.length x.Data) - (x.TotalShifted % (Array.length x.Data) )
        let shifter = shiftDist % (Array.length x.Data)
        let moveToEnd = Array.take shifter x.Data
        let newFront = Array.skip shifter x.Data
        Array.append newFront moveToEnd


let input = seq { 0 .. 255 } |> Seq.toArray

let shiftArray (shiftSize:int) (a:int[]) =
    let shifter = shiftSize % (Array.length a)
    let moveToEnd = Array.take shifter a
    let newFront = Array.skip shifter a
    Array.append newFront moveToEnd
        
// in this implementation current position is always 0
// we keep track of how far current position was shifted over all operations
// and then shift the array back to the "original order" (though it has been scrambled)
let rec knotHash1 (lengths:seq<int>) (hs:HashState) =
    if Seq.isEmpty lengths then
        // shift array back to origional config
        hs        
    else
        let len = Seq.head lengths
        let flippedData = Array.take len hs.Data |> Array.rev
        let restOfData  = Array.skip len hs.Data
        let newData = 
            Array.append flippedData restOfData // do the flip
            |> shiftArray (len+hs.SkipSize) // move "current Position" by shifting array
        knotHash1 (Seq.tail lengths) { hs with Data = newData; SkipSize = (hs.SkipSize+1); TotalShifted = (hs.TotalShifted+len+hs.SkipSize) }

let testLengths = seq [3;4;1;5]
let testInput = seq [0;1;2;3;4] |> Seq.toArray
knotHash1 testLengths { Data = testInput; SkipSize = 0; TotalShifted = 0 }


let lengthsInput = seq [102;255;99;252;200;24;219;57;103;2;226;254;1;0;69;216]
knotHash1 lengthsInput { Data = input; SkipSize = 0; TotalShifted = 0 }
|> (fun hs -> hs.Output )
|> Array.take 2
|> (fun a -> a.[0] * a.[1] )
|> printfn "part 1: %i"




let dense a = a |> Array.fold (fun j i -> j ^^^ i) 0
let lengths (s:String) = Seq.append (Seq.map int s) (seq [17;31;73;47;23])

let knotHash2 (s:String) =
    seq { 0..63 }
    |> Seq.fold (fun hs _ -> knotHash1 (lengths s) hs)  { Data = input; SkipSize = 0; TotalShifted = 0 }
    |> (fun hs -> hs.Output )
    |> Seq.chunkBySize 16
    |> Seq.map (dense >> sprintf "%02x")
    |> String.Concat
 
knotHash2 ""
knotHash2 "AoC 2017"
knotHash2 "1,2,3"
knotHash2 "1,2,4"

knotHash2 "102,255,99,252,200,24,219,57,103,2,226,254,1,0,69,216"
|> printfn "part 2: %s"



seq [64;7;255] |> Seq.map (sprintf "%02x")
dense [|65;27;9;1;4;3;40;50;91;7;6;0;2;5;68;22|]

///************************************
//let data = input
//let lengths = seq [3;4;1;5]
//let skipSize = 0
//let totalShifted = 0
//
//// repeat next two blocks in FSI as necessary to debug
//let len = Seq.head lengths
//let flippedData = Array.take len data |> Array.rev
//let restOfData  = Array.skip len data
//let newData = 
//    Array.append flippedData restOfData // do the flip
//    |> shiftArray (len+skipSize) // move "current Position" by shifting array
//
//let data = newData
//let lengths = Seq.tail lengths
//let skipSize = skipSize+1
//let totalShifted = totalShifted+len+skipSize


