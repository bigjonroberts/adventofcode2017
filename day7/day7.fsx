open System.IO
open System

let sampleInput = @"pbga (66)
xhth (57)
ebii (61)
havc (66)
ktlj (57)
fwft (72) -> ktlj, cntj, xhth
qoyq (66)
padx (45) -> pbga, havc, qoyq
tknk (41) -> ugml, padx, fwft
jptl (61)
ugml (68) -> gyxo, ebii, jptl
gyxo (61)
cntj (57)"

type Prog (holding:array<Prog>,s:String) =
    let args = s.Split(' ')
    member x.FormationString = s
    member x.Name = args.[0]
    member x.DiscWeight = args.[1].Trim([|'(';')'|]) |> int
    member x.HoldingNames = 
        match args.Length with
        | i when i > 2 -> Array.skip 3 args |> Array.map (fun s -> s.Trim(','))
        | _ -> Array.empty<String> 
    member x.HoldsName p = Array.contains p x.HoldingNames
    member x.Holding = holding
    member x.TotalWeight = x.DiscWeight + x.StackWeight
    member x.StackWeight = Array.sumBy (fun (h:Prog) -> h.TotalWeight) holding
    member x.Balanced = holding.Length > 0 && Array.TrueForAll(holding,(fun (h:Prog) -> h.TotalWeight = holding.[0].TotalWeight))

let input =
    //sampleInput.Split('\n')
    File.ReadLines @"C:\work\adventofcode2017\day7\input.txt"
    |> Seq.map (fun s -> Prog(Array.empty<Prog>,s))
    |> Seq.map (fun p -> p.Name,p)
    |> Map.ofSeq

let bottomProg =
    input
    |> Map.findKey (fun _ p -> not (Map.exists (fun _ (p2:Prog) -> p2.HoldsName p.Name) input))
    |> Map.find <| input

printfn "part 1: %s" bottomProg.Name

let rec buildGraph (prog:Prog) =
    if Array.length prog.HoldingNames = 0 then
        prog
    else
        let newHolding =
            prog.HoldingNames
            |> Array.map (fun n -> Map.find n input)
            |> Array.map buildGraph 
        Prog(newHolding,prog.FormationString)

let root = buildGraph bottomProg

let rec unBalanced (prog:Prog) (mp:Map<string,Prog>) =
    if prog.Balanced then
        mp
    else
        prog.Holding
        mp
        |> Map.add prog.Name prog

unBalanced root Map.empty<string,Prog>
        

root.Holding
|> Array.filter (fun (p:Prog) -> not p.Balanced)
|> Array.head

input
|> Map.filter (fun _ p -> p.StackWeight = 0)

