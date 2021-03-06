open System.IO
open System
open Microsoft.FSharp.Collections

let hasNoDups (sa:string[]) =
    let dups =
        sa
        |> Array.countBy (fun s -> s.Trim())
        |> Array.filter (fun (k,c) -> c > 1)
        |> Array.length
    dups = 0

let hasNoAnagrams (sa:string[]) =
    let anis =
        sa
        |> Array.map (fun s -> s |> Set.ofSeq)
        |> Array.countBy (fun s -> s)
        |> Array.filter (fun (k,c) -> c > 1)
        |> Array.length
    anis = 0

let passPhrases =
    File.ReadAllLines(@"c:\work\adventofcode2017\day4\input.txt")
    |> Seq.map (fun s -> s.Split(' ') )
    |> Seq.cache

passPhrases
|> Seq.filter hasNoDups
|> Seq.length
|> printfn "Part 1: %i"

passPhrases
|> Seq.filter hasNoDups
|> Seq.filter hasNoAnagrams
|> Seq.length
|> printfn "Part 2: %i"
