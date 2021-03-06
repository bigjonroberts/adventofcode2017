open System.IO
open System

type CommandLog = {
    CommandsProcessed: int
    CurrentRegister: int
    InstructionSet: int[]
} with
    member x.Complete =
        match x.CurrentRegister with
        | n when n < 0 -> true 
        | n when n >= Array.length x.InstructionSet -> true
        | _ -> false        
    member x.CurrentJump = 
        match x.Complete with
        | true -> 0
        | false -> x.InstructionSet.[x.CurrentRegister]        

let processInstruction (c:CommandLog) = 
    let currentVal = c.CurrentJump
    Array.set c.InstructionSet c.CurrentRegister (currentVal + 1)
    { c with CommandsProcessed = c.CommandsProcessed+1; CurrentRegister = c.CurrentRegister + currentVal }

let processInstruction2 (c:CommandLog) =
    let currentVal = c.CurrentJump
    let vShift = 
        match c.CurrentJump with
        | n when n >= 3 -> -1
        | _ -> 1   
    Array.set c.InstructionSet c.CurrentRegister (c.CurrentJump + vShift)
    { c with CommandsProcessed = c.CommandsProcessed+1; CurrentRegister = c.CurrentRegister + currentVal }


let rec processAllCommands p (c:CommandLog) = 
    if c.Complete then
        c
    else
        p c |> processAllCommands p


let input =
    File.ReadAllLines(@"c:\work\adventofcode2017\day5\input.txt")
    |> Seq.map int
    |> Seq.cache

let commandset = {
    CommandsProcessed = 0
    CurrentRegister = 0
    InstructionSet = input |> Seq.toArray
}

processAllCommands processInstruction commandset
|> (fun c -> c.CommandsProcessed)
|> printfn "part 1: %i"

processAllCommands processInstruction2 commandset
|> (fun c -> c.CommandsProcessed)
|> printfn "part 2: %i"
