open System.IO
open System

type MemoryBank = {
    CurrentContents: array<int>
    PriorContents: List<string>
} with
    member x.HasDuplicate =
        let dups =
            x.PriorContents
            |> List.countBy (fun k -> k) 
            |> List.filter (fun (l,c) -> c > 1)
            |> List.length
        dups > 0

let rec redist (index:int) (value:int) (arr:int[]) =
    if value = 0 then
        arr
    else
        Array.set arr index (arr.[index]+1)
        redist ((index+1) % (arr.Length) ) (value-1) arr

let processBank m =
    let maxVal = Array.max m.CurrentContents
    let redistIndex = Array.IndexOf(m.CurrentContents,maxVal) // find first if there are many
    Array.set m.CurrentContents redistIndex 0
    let newBank = redist ((redistIndex+1) % (m.CurrentContents.Length) ) maxVal m.CurrentContents
    let contents = String.Join(";",newBank) |> List.singleton
    {   CurrentContents = newBank
        PriorContents = List.append m.PriorContents contents }
    
let rec processTillDuplicated counter (m:MemoryBank) =
    if m.HasDuplicate then
        counter,m
    else
        m |> processBank |> processTillDuplicated (counter+1)

let input = {
    CurrentContents = [|10;3;15;10;5;15;5;15;9;2;5;8;5;2;3;6|]
    PriorContents = List.singleton "10;3;15;10;5;15;5;15;9;2;5;8;5;2;3;6" }

let testInput = {
    CurrentContents = [|0;2;7;0|]
    PriorContents = List.singleton "0;2;7;0" }

let completeBank = processTillDuplicated 0 input

completeBank |> fst |> printfn "part 1:%i" 

let banklist = (snd completeBank).PriorContents

let cycle =
    banklist
    |> List.countBy (fun k -> k) 
    |> List.filter (fun (l,c) -> c > 1)
    |> List.head
    |> fst

let isCycle s = (s = cycle)

(List.findIndexBack isCycle banklist) - (List.findIndex isCycle banklist)
|> printfn "part 2: %i"
