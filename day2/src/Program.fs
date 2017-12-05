// Learn more about F# at http://fsharp.org

open System.IO
open System
open AdventOfCode2017

[<EntryPoint>]
let main argv =
    if argv.Length = 0 then
        printf "Usage: day2 <inputfile>"
        1
    else if not (File.Exists(argv.[0])) then 
        printfn "Cannot find file: '%s'" argv.[0]
        2
    else
        let intRows =
            File.ReadAllLines(argv.[0])
            |> Seq.map (fun s -> s.Split('\t'))
            |> Seq.map (fun a -> a |> Seq.map (fun s -> Convert.ToInt32(s)))
            |> Seq.cache
        intRows
        |> Seq.sumBy span
        |> printfn "Solution part 1: %i"
        intRows
        |> Seq.sumBy div2
        |> printfn "Solution part 1: %i"
        Console.ReadKey |> ignore
        0 // return an integer exit code
